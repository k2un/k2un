using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using InfoAct;

namespace DisplayAct
{
    public partial class subfrmCaption : UserControl
    {
        #region Fields
        public string Version
        {
            get { return "Samsung SMD HCLN V1.0"; }
        }

        public clsInfo PInfo = clsInfo.Instance;

        private const int MAX_ALARM_COUNT = 20;     //콤보박스 드롭다운 항목수
        private const int MAX_HOST_COUNT = 20;      //콤보박스 드롭다운 항목수

        private Boolean pbolHostConnection = false;
        private Boolean pbolPLCConnection = false;
        private Boolean pbolSecomDriver = false;
        private Boolean pbolHostDriver = false;
        private string pstrControlState = "";
        private string pstrEQPState = "";
        private string pstrEQPProcessState = "";
        private string pstrHOSTPPID = "";
        private string pstrEQPPPID = "";
        private Boolean pbolSEM = false;
        //private string pstrOPERID = "";
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public subfrmCaption()
        {
            InitializeComponent();
            funSetDoubleBuffer();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 화면 깜빡임을 최소화 하기 위한 DoubleBuffer 사용.
        /// </summary>
        private void funSetDoubleBuffer()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void tmrUpdateCaption_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrUpdateCaption.Enabled = false;

                subHostConnection();
                subPLCConnection();
                subSecomDriver();
                subControlState();
                subEQPState();
                subEQPProcessState();
                subHOSTPPID();
                subSemState();

                subMessageAdd();        //ComboBox에 Host, Alarm Message Display
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                this.tmrUpdateCaption.Enabled = true;
            }
        }

        private void subSemState()
        {
            try
            {
                if (PInfo.EQP("Main").UDPConnect != pbolSEM)
                {
                    if (PInfo.EQP("Main").UDPConnect)
                    {
                        lblSEMState.Text = "Connected";
                        lblSEMState.BackColor = Color.Blue;
                        lblSEMState.ForeColor = Color.White;
                    }
                    else
                    {
                        lblSEMState.Text = "DisConnected";
                        lblSEMState.BackColor = Color.Red;
                        lblSEMState.ForeColor = Color.White;
                    }
                    pbolSEM = PInfo.EQP("Main").UDPConnect;
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subfrmCaption_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (PInfo != null)
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void funInitializeForm()
        {
            try
            {
                this.tmrUpdateCaption.Enabled = true;
                this.Show();
            }
            catch (Exception error)
            {
                if (PInfo != null)
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }

        /// <summary>
        /// Host, Alarm Message를 해당 Combo Box에 추가
        /// </summary>
        public void subMessageAdd()
        {
            string dstrData = "";
            string[] darrEvent;

            try
            {
                if (this.PInfo.funGetMessageCount() > 0)
                {
                    dstrData = this.PInfo.funGetMessage().ToString();      //방금 Queue에서 읽은 내용을 삭제한다.
                    darrEvent = dstrData.Split(new char[] { ';' });

                    switch (Convert.ToInt32(darrEvent[0]))
                    {
                        case (int)InfoAct.clsInfo.MsgType.HostMsg:
                            //darrEvent = PInfo.funGetMessage().ToString().Split(new char[] { ';' });
                            subHostMsgAdd(darrEvent[1]);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        /// <summary>
        /// Host Message를 Combo Box에 Display 한다.
        /// </summary>
        /// <param name="strMessage"></param>
        public void subHostMsgAdd(string strMessage)
        {
            string[] arrData;
            try
            {
                arrData = strMessage.Split(new string[] { "[", "]" }, StringSplitOptions.None);

                //PInfo.subLog_Set(clsInfo.LogType.HostMSG, arrData[1] + "," + arrData[3] + "," + arrData[4]);

                if (this.cboHostMessage.Items.Count == MAX_HOST_COUNT)
                {
                    this.cboHostMessage.Items.RemoveAt(MAX_HOST_COUNT - 1);      //최대 개수가 되면 제일 아래(오래된것)있는것 삭제
                }
                this.cboHostMessage.Items.Insert(0, strMessage);    //최근에 발생한 것이 제일 위로 추가되게 한다.
                this.cboHostMessage.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnHostMessageClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.cboHostMessage.Items.Clear();      //콤보박스항목 삭제
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.MessageClear);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

        }
        /// <summary>
        /// Host Connection 상태를 Display 한다.
        /// </summary>
        private void subHostConnection()
        {
            try
            {
                if (this.PInfo.All.HostConnect != this.pbolHostConnection)
                {
                    if (this.PInfo.All.HostConnect == true)
                    {
                        this.lblHostConnection.Text = "CONNECT";
                        this.lblHostConnection.BackColor = Color.Blue;
                        this.lblHostConnection.ForeColor = Color.White;
                    }
                    else
                    {
                        this.lblHostConnection.Text = "DISCONNECT";
                        this.lblHostConnection.BackColor = Color.Red;
                        this.lblHostConnection.ForeColor = Color.White;
                    }
                    this.pbolHostConnection = this.PInfo.All.HostConnect;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// PLC Connection 상태를 Display
        /// </summary>
        private void subPLCConnection()
        {
            try
            {
                if (this.PInfo.EQP("Main").PLCConnect != this.pbolPLCConnection)
                {
                    if (PInfo.EQP("Main").PLCConnect == true)
                    {
                        this.lblPLCConnection.Text = "CONNECT";
                        this.lblPLCConnection.BackColor = Color.Blue;
                        this.lblPLCConnection.ForeColor = Color.White;
                    }
                    else
                    {
                        this.lblPLCConnection.Text = "DISCONNECT";
                        this.lblPLCConnection.BackColor = Color.Red;
                        this.lblPLCConnection.ForeColor = Color.White;

                        PInfo.All.ControlState = "0";
                        // 장비가 Disconnect 되면 HOST 에 Off-Line 보고 후 Off-Line로 변경한다.      //추가 : 20100217 이상호
                        this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 111, 0, 0); //뒤에 0은 전체장비 

                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "PLC Disconnection으로 Off-Line 전환!");
                    }
                    this.pbolPLCConnection = PInfo.EQP("Main").PLCConnect;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// SECS Driver 상태를 Display
        /// </summary>
        private void subSecomDriver()
        {
            try
            {
                if (this.PInfo.All.HostDriver != this.pbolHostDriver)
                {
                    if (this.PInfo.All.HostDriver == true)
                    {
                        this.lblHOSTDriver.Text = "YES";
                        this.lblHOSTDriver.BackColor = Color.Blue;
                        this.lblHOSTDriver.ForeColor = Color.White;
                    }
                    else
                    {
                        this.lblHOSTDriver.Text = "NO";
                        this.lblHOSTDriver.BackColor = Color.Red;
                        this.lblHOSTDriver.ForeColor = Color.White;
                    }
                    this.pbolHostDriver = this.PInfo.All.HostDriver;

                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Online Mode 상태를 Display (Offline, Online Local, Online Remote)
        /// </summary>
        private void subControlState()
        {
            try
            {
                if (PInfo.All.ControlState != this.pstrControlState)
                {
                    switch (PInfo.All.ControlState)
                    {
                        case "1":
                            this.lblControlState.Text = "OFFLINE";
                            this.lblControlState.BackColor = Color.Gray;
                            this.lblControlState.ForeColor = Color.White;
                            break;


                        case "3":
                            this.lblControlState.Text = "ONLINE REMOTE";
                            this.lblControlState.BackColor = Color.Blue;
                            this.lblControlState.ForeColor = Color.White;
                            break;

                        default:
                            this.lblControlState.Text = "";
                            this.lblControlState.BackColor = Color.White;
                            this.lblControlState.ForeColor = Color.Black;

                            break;
                    }
                    this.pstrControlState = PInfo.All.ControlState;
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.ControlState, PInfo.All.ControlState);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// EQP State Display (Normal, Fault, PM)
        /// </summary>
        private void subEQPState()
        {
            try
            {
                if (PInfo.Unit(0).SubUnit(0).EQPState != this.pstrEQPState)
                {
                    switch (PInfo.Unit(0).SubUnit(0).EQPState)
                    {
                        case "1":
                            this.lblEQPState.Text = "Normal";
                            this.lblEQPState.BackColor = Color.Yellow;
                            this.lblEQPState.ForeColor = Color.Black;
                            break;

                        case "2":
                            this.lblEQPState.Text = "Fault";
                            this.lblEQPState.BackColor = Color.Red;
                            this.lblEQPState.ForeColor = Color.White;
                            break;

                        case "3":
                            this.lblEQPState.Text = "PM";
                            this.lblEQPState.BackColor = Color.Gray;
                            this.lblEQPState.ForeColor = Color.White;
                            break;

                        default:
                            this.lblEQPState.Text = "";
                            this.lblEQPState.BackColor = Color.White;
                            this.lblEQPState.ForeColor = Color.Black;
                            break;
                    }
                    this.pstrEQPState = PInfo.Unit(0).SubUnit(0).EQPState;

                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        private void subEQPProcessState()
        {
            try
            {
                if (PInfo.Unit(0).SubUnit(0).EQPProcessState != this.pstrEQPProcessState)
                {
                    switch (PInfo.Unit(0).SubUnit(0).EQPProcessState)
                    {
                        case "1":
                            this.lblEQPProcessState.Text = "IDLE";
                            this.lblEQPProcessState.BackColor = Color.Yellow;
                            this.lblEQPProcessState.ForeColor = Color.Black;
                            break;

                        case "2":
                            this.lblEQPProcessState.Text = "SETUP";
                            this.lblEQPProcessState.BackColor = Color.LightGreen;
                            this.lblEQPProcessState.ForeColor = Color.White;
                            break;

                        case "3":
                            this.lblEQPProcessState.Text = "Executing";
                            this.lblEQPProcessState.BackColor = Color.Blue;
                            this.lblEQPProcessState.ForeColor = Color.White;
                            break;

                        case "4":
                            this.lblEQPProcessState.Text = "Pause";
                            this.lblEQPProcessState.BackColor = Color.LightPink;
                            this.lblEQPProcessState.ForeColor = Color.Black;
                            break;

                        case "5":
                            this.lblEQPProcessState.Text = "Disabled";
                            this.lblEQPProcessState.BackColor = Color.Red;
                            this.lblEQPProcessState.ForeColor = Color.White;
                            break;

                        default:
                            this.lblEQPProcessState.Text = "";
                            this.lblEQPProcessState.BackColor = Color.White;
                            this.lblEQPProcessState.ForeColor = Color.Black;
                            break;
                    }
                    this.pstrEQPProcessState = PInfo.Unit(0).SubUnit(0).EQPProcessState;
                }

            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
        /// <summary>
        /// Host PPID Display
        /// </summary>
        private void subHOSTPPID()
        {
            try
            {
                if (PInfo.All.CurrentHOSTPPID != this.pstrHOSTPPID)
                {
                    this.lblHOSTPPID.Text = PInfo.All.CurrentHOSTPPID;
                    this.pstrHOSTPPID = PInfo.All.CurrentHOSTPPID;

                    if (PInfo.Unit(0).SubUnit(0).HOSTPPID(pstrHOSTPPID) != null)
                    {
                        this.lblEQPPPID.Text = PInfo.Unit(0).SubUnit(0).HOSTPPID(pstrHOSTPPID).EQPPPID;
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void frmCaption_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
        }
        #endregion
    }
}
