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

        #region "System DB"
        private static string pstrConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + @"\system\System_A3TLM02S.mdb";

        private static OleDbConnection pConnection = null;         //Connection 개체 선언
        private static OleDbTransaction pTransaction = null;       //Transaction 개체 선언
        private static OleDbCommand pCommand = null;               //Command 개체 선언

        private static object pLock = "LOCK";
        private static string strEQPID = "";

        //*******************************************************************************
        //  Function Name : funConnect()
        //  Description   : DB에 연결한다.
        //  Parameters    : strDBFilePath => DB File 경로
        //  Return Value  : 성공 => True, 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00] 
        //*******************************************************************************               
        public static bool funConnect()
		{
            string[] arrCon;

			try 
            {
                string strBasicPath = Application.StartupPath;
                arrCon = strBasicPath.Split('\\');
                strBasicPath = "";
                for (int dintLoop = 0; dintLoop < arrCon.Length; dintLoop++)
                {
                    if (arrCon[dintLoop] != "MCC")
                    {
                        if (dintLoop == arrCon.Length - 1)
                        {
                            strBasicPath += arrCon[dintLoop];
                        }
                        else
                        {
                            strBasicPath += arrCon[dintLoop] + "\\";
                        }
                    }
                }

                pstrConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + strBasicPath + @"\system\System_" + strEQPID + ".mdb";

                //pstrConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + strBasicPath + @"\system\System.mdb";

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

                string strBasicPath = Application.StartupPath;
                string[] arrCon = strBasicPath.Split('\\');
                strBasicPath = "";
                for (int dintLoop = 0; dintLoop < arrCon.Length; dintLoop++)
                {
                    if (arrCon[dintLoop] != "MCC")
                    {
                        if (dintLoop == arrCon.Length - 1)
                        {
                            strBasicPath += arrCon[dintLoop];
                        }
                        else
                        {
                            strBasicPath += arrCon[dintLoop] + "\\";
                        }
                    }
                }

                pstrConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + strBasicPath + @"\system\System_" + dstrEQPID + ".mdb";
                //pstrConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + @"\system\System_" + dstrEQPID + ".mdb";

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

        //*******************************************************************************
        //  Function Name : funDisconnect()
        //  Description   : DB연결을 끊는다.
        //  Parameters    : None
        //  Return Value  : 성공 => True, 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00] 
        //*******************************************************************************              
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

        //*******************************************************************************
        //  Function Name : funOleDbConnect()
        //  Description   : 현재 연결중인 Connection 개체를 반환한다.
        //  Parameters    : None
        //  Return Value  : 현재 연결중인 Connection 개체
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00] 
        //*******************************************************************************        
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

        //*******************************************************************************
        //  Function Name : funOleDbTransaction()
        //  Description   : 현재 걸려있는 Transaction 개체를 반환한다.
        //  Parameters    : None
        //  Return Value  : 현재 걸려있는 Transaction 개체
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00] 
        //*******************************************************************************        
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

        //*******************************************************************************
        //  Function Name : funSelectQuery()
        //  Description   : Select문을 수행해 결과값(데이터셋)을 DataTable로 리턴한다.
        //  Parameters    : None
        //  Return Value  : 결과값(데이터셋)
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00] 
        //*******************************************************************************          
        public static DataTable funSelectQuery(string strSQL)
        {
            try 
            {
                DataTable dDataTable = new DataTable();

                if (pConnection == null) funConnect();
             
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(strSQL, pConnection);

                dAdapter.Fill(dDataTable);      //DataAdapter를 이용해 가져온 Data를 DataTable에 저장한다.

                if (pConnection != null) funDisconnect();

                return dDataTable;
            } 
            catch 
            {
                return null;
            }
        }

        //*******************************************************************************
        //  Function Name : funSelectCountQuery()
        //  Description   : Select문을 수행해 결과값(데이터셋)의 행수를 리턴한다.
        //  Parameters    : None
        //  Return Value  : 결과값(데이터셋)
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00] 
        //*******************************************************************************          
        public static int funSelectCountQuery(string strSQL)
        {
            int dintCount = 0;

            try
            {
                if (pConnection == null) funConnect();

                OleDbCommand dCommand = new OleDbCommand(strSQL, pConnection);
                dintCount = Convert.ToInt32(dCommand.ExecuteScalar());

                if (pConnection != null) funDisconnect();

                return dintCount;
            }
            catch
            {
                return 0;
            }
        }

        //*******************************************************************************
        //  Function Name : funExecuteQuery()
        //  Description   : Update, Delete, Insert문을 실행한다.
        //  Parameters    : None
        //  Return Value  : 성공 => True, 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00]
        //*******************************************************************************          
        public static bool funExecuteQuery(string strSQL) 
		{
			try 
            {
                if (pConnection == null) funConnect();

                pCommand.CommandText = strSQL;
                pCommand.ExecuteNonQuery();

                if (pConnection != null) funDisconnect();

				return true;
			} 
            catch 
            {
				return false;
			}
		}

        //*******************************************************************************
        //  Function Name : funBeginTransaction()
        //  Description   : Transaction을 시작한다.
        //  Parameters    : None
        //  Return Value  : Transaction 성공 => True, Transaction 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00] 
        //*******************************************************************************          
        public static bool funBeginTransaction()
        {
            try 
            {
                pTransaction = pConnection.BeginTransaction();
                pCommand.Transaction = pTransaction;

                return true;
            } 
            catch 
            {
                return false;
            }
        }

        //*******************************************************************************
        //  Function Name : funCommitTransaction()
        //  Description   : 성공적으로 실행된 Update, Delete, Insert문을 Commit한다.
        //  Parameters    : None
        //  Return Value  : Commit 성공 => True, Commit 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00] 
        //*******************************************************************************          
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
     
        //*******************************************************************************
        //  Function Name : funRollbackTransaction()
        //  Description   : 실패한 명령(Update, Delete, Insert문)을 Rollback한다.
        //  Parameters    : None
        //  Return Value  : Rollback 성공 => True, Rollback 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00] 
        //*******************************************************************************        
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

        //*******************************************************************************
        //  Function Name : funIsNullTransaction()
        //  Description   : Transaction이 걸려있는지를 검사한다.
        //  Parameters    : None
        //  Return Value  : Transaction이 걸려있음 => True, Transaction이 걸려있지 않음 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00] 
        //*******************************************************************************        
        public static bool funIsNullTransaction()
        {
            try 
            {
                if (pCommand.Transaction == null)
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
        #endregion

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



        #region MCC DB
        private static OleDbConnection pMCCConnection = null;         //Connection 개체 선언
        private static OleDbCommand pMCCCommand = null;               //Command 개체 선언
        private static OleDbTransaction pMCCTransaction = null;       //Transaction 개체 선언
        public static string pstrMCCConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + @"\system\MCCfile.mdb";

        public static OleDbTransaction MCCbTransaction
        {
            get
            {
                return pMCCTransaction;
            }
            set
            {
                pMCCTransaction = value;
            }
        }

        public static OleDbCommand MCCcommand
        {
            get
            {
                return pMCCCommand;
            }
            set
            {
                pMCCCommand = value;
            }
        }

        public static bool funMCCBeginTransaction()
        {
            try
            {
                pMCCTransaction = pMCCConnection.BeginTransaction();
                pMCCCommand.Transaction = pMCCTransaction;

                return true;
            }
            catch
            {
                return false;
            }
        }

       

        public static bool funMCCIsNullTransaction()
        {
            try
            {
                if (pMCCCommand.Transaction == null)
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

                dConnection.Dispose();

                dConnection = null;
                dCommand = null;
            }

            return resualt;
        }

        #endregion
	}
}
