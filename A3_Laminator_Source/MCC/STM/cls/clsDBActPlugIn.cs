using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace STM
{
    class clsDBActPlugIn
    {
        public clsDBActPlugIn() { }
   
        //*******************************************************************************
        //  Function Name : funConnectDB()
        //  Description   : DBAct DLL의 인스턴스를 생성시키고 DB에 연결한다.
        //  Parameters    : None
        //  Return Value  : 성공 => True, 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00]
        //*******************************************************************************   
        public Boolean funConnectDB()
        {
            try 
            {
                //DB Connect를 시도한다.
                Boolean dbolDBConnect = DBAct.clsDBAct.funConnect();
                
                //리턴값이 True이면 DB 연결 성공
                if (dbolDBConnect == true)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("DB Connection Fail!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public Boolean funConnectDB(string strEQPName)
        {
            try
            {
                //DB Connect를 시도한다.
                Boolean dbolDBConnect = DBAct.clsDBAct.funConnect(strEQPName);

                //리턴값이 True이면 DB 연결 성공
                if (dbolDBConnect == true)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("DB Connection Fail!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //*******************************************************************************
        //  Function Name : funDisconnectDB()
        //  Description   : DB와 연결을 해제한다.
        //  Parameters    : None
        //  Return Value  : 성공 => True, 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00]
        //*******************************************************************************   
        public Boolean funDisconnectDB()
        {
            try 
            {
                //DB Disconnect를 시도한다.
                Boolean dbolDBDisconnect = DBAct.clsDBAct.funDisconnect();

                //리턴값이 True이면 DB 연결 해제 성공
                if (dbolDBDisconnect == true)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("DB Disconnect Fail!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
