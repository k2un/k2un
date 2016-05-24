using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace DBAct
{
    public static class clsDBAct
	{
        public static string Version
        {
            get { return "V1.0"; }
        }
        private static string pstrConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + @"\system\System.mdb";

        private static object pLock = "LOCK";
        private static OleDbConnection pConnection = null;         //Connection 개체 선언
        private static OleDbTransaction pTransaction = null;       //Transaction 개체 선언
        private static OleDbCommand pCommand = null;               //Command 개체 선언

        private static string strEQPID = "";

        /// <summary>
        /// DB에 연결한다.
        /// </summary>
        /// <returns>성공 => True, 실패 => False</returns>
        public static bool funConnect()
		{
            try
            {
                if ((pConnection != null) && (pCommand != null)) return true;
                pstrConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + @"\system\System_" + strEQPID + ".mdb";
                
                pConnection = new OleDbConnection(pstrConnection);      //Connection 개체 생성 
                pConnection.Open();                                     //DB를 Open한다.

                pCommand = new OleDbCommand();                          //Update, Delete, Insert 수행을 위한 Command 개체 생성
                pCommand.Connection = pConnection;                      //Command가 수행되기 위한 Connection개체를 지정

                return true;
            }
            catch
            {
                return false;
            }
		}

        public static bool funConnect(string dstrEQPID)
        {
            try
            {
                if ((pConnection != null) && (pCommand != null)) return true;
                strEQPID = dstrEQPID;
                pstrConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + @"\system\System_" + dstrEQPID + ".mdb";

                pConnection = new OleDbConnection(pstrConnection);      //Connection 개체 생성 
                pConnection.Open();                                     //DB를 Open한다.

                pCommand = new OleDbCommand();                          //Update, Delete, Insert 수행을 위한 Command 개체 생성
                pCommand.Connection = pConnection;                      //Command가 수행되기 위한 Connection개체를 지정

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// DB연결을 끊는다.
        /// </summary>
        /// <returns>성공 => True, 실패 => False</returns>
        public static bool funDisconnect()
        {
            try 
            {
                if (pConnection != null) pConnection.Close();           //DB를 Close한다.

                pConnection = null;
                pTransaction = null;
                pCommand = null; 

                return true;
            } 
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// 현재 연결중인 Connection 개체를 반환한다.
        /// </summary>
        /// <returns>현재 연결중인 Connection 개체</returns>
        public static OleDbConnection funOleDbConnect()
        {
            try
            {
                return pConnection;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 현재 걸려있는 Transaction 개체를 반환한다.
        /// </summary>
        /// <returns>현재 걸려있는 Transaction 개체</returns>
        public static OleDbTransaction funOleDbTransaction()
        {
            try
            {
                return pTransaction;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Select문을 수행해 결과값(데이터셋)을 DataTable로 리턴한다.
        /// </summary>
        /// <param name="strSQL">Query문</param>
        /// <returns>결과값(데이터셋)</returns>
        public static DataTable funSelectQuery(string strSQL)
        {
            try 
            {
                lock (pLock)
                {
                    DataTable dDataTable = new DataTable();

                    if (pConnection == null) funConnect();

                    OleDbDataAdapter dAdapter = new OleDbDataAdapter(strSQL, pConnection);

                    dAdapter.Fill(dDataTable);      //DataAdapter를 이용해 가져온 Data를 DataTable에 저장한다.

                    if (pConnection != null) funDisconnect();

                    return dDataTable;
                }
            } 
            catch 
            {
                return null;
            }
        }

        /// <summary>
        /// Select문을 수행해 결과값(데이터셋)을 DataTable로 리턴한다.
        /// </summary>
        /// <param name="strSQL">Query문</param>
        /// <returns>결과값(데이터셋)</returns>
        /// <comment>
        /// 2012.08.06 김영식
        /// Query 후 Connection을 닫지 않도록 Test
        /// </comment>
        public static DataTable funSelectQuery2(string strSQL)
        {
            try
            {
                lock (pLock)
                {
                    DataTable dDataTable = new DataTable();

                    if (pConnection == null) funConnect();

                    OleDbDataAdapter dAdapter = new OleDbDataAdapter(strSQL, pConnection);

                    dAdapter.Fill(dDataTable);      //DataAdapter를 이용해 가져온 Data를 DataTable에 저장한다.

                    return dDataTable;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Select문을 수행해 결과값(데이터셋)의 행수를 리턴한다.
        /// </summary>
        /// <param name="strSQL">Query문</param>
        /// <returns>결과값(데이터셋)</returns>
        public static int funSelectCountQuery(string strSQL)
        {
            int dintCount = 0;

            try
            {
                lock (pLock)
                {
                    if (pConnection == null) funConnect();

                    OleDbCommand dCommand = new OleDbCommand(strSQL, pConnection);
                    dintCount = Convert.ToInt32(dCommand.ExecuteScalar());

                    if (pConnection != null) funDisconnect();

                    return dintCount;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Update, Delete, Insert문을 실행한다.
        /// </summary>
        /// <param name="strSQL">Query문</param>
        /// <returns>성공 => True, 실패 => False</returns>
        public static bool funExecuteQuery(string strSQL) 
		{
			try 
            {
                lock (pLock)
                {
                    if (pConnection == null) funConnect();

                    pCommand.CommandText = strSQL;
                    pCommand.ExecuteNonQuery();

                    if (pConnection != null) funDisconnect();

                    return true;
                }
			} 
            catch 
            {
				return false;
			}
		}

        /// <summary>
        /// Transaction을 시작한다.
        /// </summary>
        /// <returns>Transaction 성공 => True, Transaction 실패 => False</returns>
        public static bool funBeginTransaction()
        {
            try
            {
                if (pConnection == null) funConnect();

                pTransaction = pConnection.BeginTransaction();
                pCommand.Transaction = pTransaction;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool funExecuteNonQuery(string strSQL)
        {
            try
            {
                pCommand.CommandText = strSQL;
                pCommand.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 성공적으로 실행된 Update, Delete, Insert문을 Commit한다.
        /// </summary>
        /// <returns>Commit 성공 => True, Commit 실패 => False</returns>
        public static bool funCommitTransaction()
        {
            try 
            {
                if (pTransaction != null) pTransaction.Commit();
                pCommand.Transaction = null;

                return true;
            } 
            catch 
            {
                return false;
            }
        }
     
        /// <summary>
        /// 실패한 명령(Update, Delete, Insert문)을 Rollback한다.
        /// </summary>
        /// <returns>Rollback 성공 => True, Rollback 실패 => False</returns>
        public static bool funRollbackTransaction()
        {
            try 
            {
                if (pTransaction != null) pTransaction.Rollback();
                pCommand.Transaction = null;

                return true;
            } 
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// Transaction이 걸려있는지를 검사한다.
        /// </summary>
        /// <returns>Transaction이 걸려있음 => True, Transaction이 걸려있지 않음 => False</returns>
        public static bool funIsNullTransaction()
        {
            try 
            {
                if (pCommand == null)
                {
                    return true;
                }
                else if (pCommand.Transaction == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } 
            catch 
            {
                return false;
            }
        }

        #region MCC DB
        private static OleDbConnection pMCCConnection = null;         //Connection 개체 선언
        private static OleDbCommand pMCCCommand = null;               //Command 개체 선언
        public static string pstrMCCConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + @"\system\MCCfile.mdb";

        public static bool funMCCConnect(string strConnection)
        {
            try
            {
                pMCCConnection = new OleDbConnection(strConnection);      //Connection 개체 생성 
                pMCCConnection.Open();                                     //DB를 Open한다.

                pMCCCommand = new OleDbCommand();                          //Update, Delete, Insert 수행을 위한 Command 개체 생성
                pMCCCommand.Connection = pMCCConnection;                      //Command가 수행되기 위한 Connection개체를 지정

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool funMCCDisconnect()
        {
            try
            {
                if (pMCCConnection != null) pMCCConnection.Close();           //DB를 Close한다.

                pMCCConnection = null;
                pMCCCommand = null;

                return true;
            }
            catch
            {
                return false;
            }
        }
         
        public static DataTable funSelectQuery(string strConnection, string strSQL)
        {
            try
            {
                DataTable dDataTable = new DataTable();

                if (pMCCConnection == null) funMCCConnect(strConnection);

                OleDbDataAdapter dAdapter = new OleDbDataAdapter(strSQL, pMCCConnection);

                dAdapter.Fill(dDataTable);      //DataAdapter를 이용해 가져온 Data를 DataTable에 저장한다.

                if (pMCCConnection != null) funMCCDisconnect();

                return dDataTable;
            }
            catch
            {
                return null;
            }
        }

     
        public static int funSelectCountQuery(string strConnection, string strSQL)
        {
            int dintCount = 0;

            try
            {
                if (pMCCConnection == null) funMCCConnect(strConnection);

                OleDbCommand dCommand = new OleDbCommand(strSQL, pMCCConnection);
                dintCount = Convert.ToInt32(dCommand.ExecuteScalar());

                if (pMCCConnection != null) funMCCDisconnect();

                return dintCount;
            }
            catch
            {
                return 0;
            }
        }

         
        public static bool funExecuteQuery(string strConnection, string strSQL)
        {
            OleDbConnection dConnection = null;
            OleDbCommand dCommand = null;

            bool resualt = true;


            try
            {
                dConnection = new OleDbConnection(strConnection);
                dCommand = new OleDbCommand();

                dConnection.Open();                                     //DB를 Open한다.

                dCommand.Connection = dConnection;

                dCommand.CommandText = strSQL;
                dCommand.ExecuteNonQuery();
            }
            catch
            {
                resualt = false;
            }
            finally
            {
                if (dCommand != null) dCommand.Dispose();
                if (dConnection != null) dConnection.Close();           //DB를 Close한다.

                dConnection = null;
                dCommand = null;
            }

            return resualt;
        }

        #endregion
    }
}
