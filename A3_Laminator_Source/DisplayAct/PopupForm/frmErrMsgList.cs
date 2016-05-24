using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InfoAct;

namespace DisplayAct
{
    public partial class frmErrMsgList : Form
    {
        public clsInfo PInfo = clsInfo.Instance;

        public frmErrMsgList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form Load시 필요한 작업을 수행
        /// </summary>
        /// <param name="strMessage">표시할 메세지</param>
        /// <param name="intOPCallType">InfoAct.clsInfo.OPCallOverWrite(부저를 울리는지 안울리는지)</param>
        public void subFormLoad(string strMessage, int intOPCallType)
        {
            string dstrDateTime;
            string dstrMessage;

            try
            {

                dstrDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dstrMessage = "[ " + dstrDateTime + " ]" + "\r\n" + "\r\n" + strMessage;

                lstMessage.Items.Insert(0,dstrMessage);
                //if (lstMessage.Items.Count == 1)
                //{
                lstMessage.SelectedIndex = 0;
                //this.txtMessage.Text = dstrMessage;
                //}
                //this.btnClose.Focus();

                this.CenterToScreen();
                this.Show();

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "frmErrMsgList(subFormLoad): " + dstrMessage + ", intOPCallType: "
                  + (InfoAct.clsInfo.OPCallOverWrite)intOPCallType);  //로그 출력
            }
            catch (Exception ex)
                {
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                }
            
         }
        

        /// <summary>
        /// 선택된 리스트의 항목을 Text에표시한다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstMessage.SelectedItem != null)
                {
                    txtMessage.Text = lstMessage.SelectedItem.ToString();
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 선택된 리스트의 항목을 지운다 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.MessageClear);
                this.subClose(false);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Frm을 닫는다.
        /// </summary>
        public void subClose(Boolean bolClear)
        {
            try
            {
                this.lstMessage.Items.Clear();
                this.Hide();

                //if (bolClear == true)
                //{
                //    this.lstMessage.Items.Clear();
                //    this.Hide();
                //}
                //else
                //{
                //    if (lstMessage.Items.Count > 1)
                //    {
                //        int dintIndex = this.lstMessage.SelectedIndex;

                //        this.lstMessage.Items.RemoveAt(dintIndex);
                //        this.lstMessage.SelectedIndex = 0;
                //    }
                //    else
                //    {
                //        int dintIndex = this.lstMessage.SelectedIndex;
                //        this.lstMessage.Items.RemoveAt(dintIndex);

                //        this.Hide();
                //    }
                //}
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Buzzer를 OFF시킨다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuzzerOff_Click(object sender, EventArgs e)
        {
            this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOff);     //Buzzer Off
        }

    }
}