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

        private static OleDbConnection pConnection = null;         //Connection ��ü ����
        private static OleDbTransaction pTransaction = null;       //Transaction ��ü ����
        private static OleDbCommand pCommand = null;               //Command ��ü ����

        private static object pLock = "LOCK";
        private static string strEQPID = "";

        //*******************************************************************************
        //  Function Name : funConnect()
        //  Description   : DB�� �����Ѵ�.
        //  Parameters    : strDBFilePath => DB File ���
        //  Return Value  : ���� => True, ���� => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00] 
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

        //*******************************************************************************
        //  Function Name : funDisconnect()
        //  Description   : DB������ ���´�.
        //  Parameters    : None
        //  Return Value  : ���� => True, ���� => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00] 
        //*******************************************************************************              
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

        //*******************************************************************************
        //  Function Name : funOleDbConnect()
        //  Description   : ���� �������� Connection ��ü�� ��ȯ�Ѵ�.
        //  Parameters    : None
        //  Return Value  : ���� �������� Connection ��ü
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00] 
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
        //  Description   : ���� �ɷ��ִ� Transaction ��ü�� ��ȯ�Ѵ�.
        //  Parameters    : None
        //  Return Value  : ���� �ɷ��ִ� Transaction ��ü
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00] 
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
        //  Description   : Select���� ������ �����(�����ͼ�)�� DataTable�� �����Ѵ�.
        //  Parameters    : None
        //  Return Value  : �����(�����ͼ�)
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00] 
        //*******************************************************************************          
        public static DataTable funSelectQuery(string strSQL)
        {
            try 
            {
                DataTable dDataTable = new DataTable();

                if (pConnection == null) funConnect();
             
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(strSQL, pConnection);

                dAdapter.Fill(dDataTable);      //DataAdapter�� �̿��� ������ Data�� DataTable�� �����Ѵ�.

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
        //  Description   : Select���� ������ �����(�����ͼ�)�� ����� �����Ѵ�.
        //  Parameters    : None
        //  Return Value  : �����(�����ͼ�)
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00] 
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
        //  Description   : Update, Delete, Insert���� �����Ѵ�.
        //  Parameters    : None
        //  Return Value  : ���� => True, ���� => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00]
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
        //  Description   : Transaction�� �����Ѵ�.
        //  Parameters    : None
        //  Return Value  : Transaction ���� => True, Transaction ���� => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00] 
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
        //  Description   : ���������� ����� Update, Delete, Insert���� Commit�Ѵ�.
        //  Parameters    : None
        //  Return Value  : Commit ���� => True, Commit ���� => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00] 
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
        //  Description   : ������ ���(Update, Delete, Insert��)�� Rollback�Ѵ�.
        //  Parameters    : None
        //  Return Value  : Rollback ���� => True, Rollback ���� => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00] 
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
        //  Description   : Transaction�� �ɷ��ִ����� �˻��Ѵ�.
        //  Parameters    : None
        //  Return Value  : Transaction�� �ɷ����� => True, Transaction�� �ɷ����� ���� => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          �� ȿ��         [L 00] 
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

                    dAdapter.Fill(dDataTable);      //DataAdapter�� �̿��� ������ Data�� DataTable�� �����Ѵ�.

                    return dDataTable;
                }
            }
            catch
            {
                return null;
            }
        }



        #region MCC DB
        private static OleDbConnection pMCCConnection = null;         //Connection ��ü ����
        private static OleDbCommand pMCCCommand = null;               //Command ��ü ����
        private static OleDbTransaction pMCCTransaction = null;       //Transaction ��ü ����
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

                dConnection.Dispose();

                dConnection = null;
                dCommand = null;
            }

            return resualt;
        }

        #endregion
	}
}
