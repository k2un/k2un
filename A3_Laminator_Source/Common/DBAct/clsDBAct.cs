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
        private static OleDbConnection pConnection = null;         //Connection ��ü ����
        private static OleDbTransaction pTransaction = null;       //Transaction ��ü ����
        private static OleDbCommand pCommand = null;               //Command ��ü ����

        private static string strEQPID = "";

        /// <summary>
        /// DB�� �����Ѵ�.
        /// </summary>
        /// <returns>���� => True, ���� => False</returns>
        public static bool funConnect()
		{
            try
            {
                if ((pConnection != null) && (pCommand != null)) return true;
                pstrConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + @"\system\System_" + strEQPID + ".mdb";
                
                pConnection = new OleDbConnection(pstrConnection);      //Connection ��ü ���� 
                pConnection.Open();                                     //DB�� Open�Ѵ�.

                pCommand = new OleDbCommand();                          //Update, Delete, Insert ������ ���� Command ��ü ����
                pCommand.Connection = pConnection;                      //Command�� ����Ǳ� ���� Connection��ü�� ����

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

                pConnection = new OleDbConnection(pstrConnection);      //Connection ��ü ���� 
                pConnection.Open();                                     //DB�� Open�Ѵ�.

                pCommand = new OleDbCommand();                          //Update, Delete, Insert ������ ���� Command ��ü ����
                pCommand.Connection = pConnection;                      //Command�� ����Ǳ� ���� Connection��ü�� ����

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// DB������ ���´�.
        /// </summary>
        /// <returns>���� => True, ���� => False</returns>
        public static bool funDisconnect()
        {
            try 
            {
                if (pConnection != null) pConnection.Close();           //DB�� Close�Ѵ�.

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
        /// ���� �������� Connection ��ü�� ��ȯ�Ѵ�.
        /// </summary>
        /// <returns>���� �������� Connection ��ü</returns>
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
        /// ���� �ɷ��ִ� Transaction ��ü�� ��ȯ�Ѵ�.
        /// </summary>
        /// <returns>���� �ɷ��ִ� Transaction ��ü</returns>
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
        /// Select���� ������ �����(�����ͼ�)�� DataTable�� �����Ѵ�.
        /// </summary>
        /// <param name="strSQL">Query��</param>
        /// <returns>�����(�����ͼ�)</returns>
        public static DataTable funSelectQuery(string strSQL)
        {
            try 
            {
                lock (pLock)
                {
                    DataTable dDataTable = new DataTable();

                    if (pConnection == null) funConnect();

                    OleDbDataAdapter dAdapter = new OleDbDataAdapter(strSQL, pConnection);

                    dAdapter.Fill(dDataTable);      //DataAdapter�� �̿��� ������ Data�� DataTable�� �����Ѵ�.

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
        /// Select���� ������ �����(�����ͼ�)�� DataTable�� �����Ѵ�.
        /// </summary>
        /// <param name="strSQL">Query��</param>
        /// <returns>�����(�����ͼ�)</returns>
        /// <comment>
        /// 2012.08.06 �迵��
        /// Query �� Connection�� ���� �ʵ��� Test
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

                    dAdapter.Fill(dDataTable);      //DataAdapter�� �̿��� ������ Data�� DataTable�� �����Ѵ�.

                    return dDataTable;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Select���� ������ �����(�����ͼ�)�� ����� �����Ѵ�.
        /// </summary>
        /// <param name="strSQL">Query��</param>
        /// <returns>�����(�����ͼ�)</returns>
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
        /// Update, Delete, Insert���� �����Ѵ�.
        /// </summary>
        /// <param name="strSQL">Query��</param>
        /// <returns>���� => True, ���� => False</returns>
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
        /// Transaction�� �����Ѵ�.
        /// </summary>
        /// <returns>Transaction ���� => True, Transaction ���� => False</returns>
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
        /// ���������� ����� Update, Delete, Insert���� Commit�Ѵ�.
        /// </summary>
        /// <returns>Commit ���� => True, Commit ���� => False</returns>
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
        /// ������ ���(Update, Delete, Insert��)�� Rollback�Ѵ�.
        /// </summary>
        /// <returns>Rollback ���� => True, Rollback ���� => False</returns>
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
        /// Transaction�� �ɷ��ִ����� �˻��Ѵ�.
        /// </summary>
        /// <returns>Transaction�� �ɷ����� => True, Transaction�� �ɷ����� ���� => False</returns>
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
        private static OleDbConnection pMCCConnection = null;         //Connection ��ü ����
        private static OleDbCommand pMCCCommand = null;               //Command ��ü ����
        public static string pstrMCCConnection = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + @"\system\MCCfile.mdb";

        public static bool funMCCConnect(string strConnection)
        {
            try
            {
                pMCCConnection = new OleDbConnection(strConnection);      //Connection ��ü ���� 
                pMCCConnection.Open();                                     //DB�� Open�Ѵ�.

                pMCCCommand = new OleDbCommand();                          //Update, Delete, Insert ������ ���� Command ��ü ����
                pMCCCommand.Connection = pMCCConnection;                      //Command�� ����Ǳ� ���� Connection��ü�� ����

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
                if (pMCCConnection != null) pMCCConnection.Close();           //DB�� Close�Ѵ�.

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

                dAdapter.Fill(dDataTable);      //DataAdapter�� �̿��� ������ Data�� DataTable�� �����Ѵ�.

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

                dConnection.Open();                                     //DB�� Open�Ѵ�.

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
                if (dConnection != null) dConnection.Close();           //DB�� Close�Ѵ�.

                dConnection = null;
                dCommand = null;
            }

            return resualt;
        }

        #endregion
    }
}
