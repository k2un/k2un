using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using LogAct;

namespace DisplayAct
{
    public partial class frmMain : Form
    {
        #region Fields
        public clsInfo PInfo = clsInfo.Instance;

        public subfrmMainView pfrmMainView;
        public subfrmHistoryView pfrmHistoryView;
        public subfrmLogManagerView pfrmLogManagerView;
        public subfrmSetupView pfrmSystemSetup;
        public subfrmInfoView pfrmInfoView;

        public subfrmNavigationButton subfrmNavigationButton;

        public subfrmCommandButton_Main subfrmCommandButtonMain;
        public subfrmCommandButton_History subfrmCommandButtonHistory;
        public subfrmCommandButton_LogManager subfrmCommandButtonLogManager;
        public subfrmCommandButton_Setting subfrmCommandButtonSetting;
        public subfrmCommandButton_Info subfrmCommandButtonInfo;

        public frmExit pfrmExit;
        public frmErrMsgOverWrite pfrmErrMsgOverWrite;
        public frmErrMsgList pfrmErrMsgList;
        public frmModeChange pfrmModeChange;
        public frmEQPState pfrmEQPState;
        public frmProcessState pfrmEQPProcessState;

        public frmLogOn pfrmLogOn;
        public frmSEMConfig pfrmSEMConfig;

        private long longStartTick;
        private long longEndTick;
        private long longTimeSpan;

        private string strCurrentViewName = "";
        private string strCurrentButtonName = "";
        private HistoryType enmOldHistoryType = HistoryType.NONE;

        private frmSimulationTest pfrmsim;

        private frmOperatorCall pfrmLotInfo;
        private frmErrMsgList pfrmHostMSG;
        //private frmErrMsgOverWrite pfrmS9F13MSG;


        #endregion

        #region Properties
        public string Version
        {
            get { return "Samsung SMD HCLN V1.0"; }
        }
        #endregion

        #region Constructor
        public frmMain()
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

        private void frmMainNew_Load(object sender, EventArgs e)
        {
            try
            {
                // Version d
                this.Text = "NCIM V1.27 2016-02-05";


                #region load
                //1. 설정된 Size로 Form Size 조절...
                Size sizeForm = new Size(PInfo.All.SizeWidth, PInfo.All.SizeHeight);
                this.Size = sizeForm;

                //영역 구분 확인용 Label을 감춘다.
                foreach (Control ctrl in this.tableLayoutPanel1.Controls)
                {
                    if (ctrl.GetType() == typeof(Label))
                    {
                        ctrl.Visible = false;
                    }
                }

                longStartTick = DateTime.Now.Ticks;

                //2. Sub-Form을 생성한다.
                pfrmMainView = new subfrmMainView();
                pfrmHistoryView = new subfrmHistoryView();
                pfrmLogManagerView = new subfrmLogManagerView();
                pfrmSystemSetup = new subfrmSetupView();
                pfrmInfoView = new subfrmInfoView();

                subfrmNavigationButton = new subfrmNavigationButton();

                subfrmCommandButtonMain = new subfrmCommandButton_Main();
                subfrmCommandButtonHistory = new subfrmCommandButton_History();
                subfrmCommandButtonLogManager = new subfrmCommandButton_LogManager();
                subfrmCommandButtonSetting = new subfrmCommandButton_Setting();
                subfrmCommandButtonInfo = new subfrmCommandButton_Info();

                this.subfrmTitlePanel.funInitializeForm();
                this.subfrmCaption.funInitializeForm();
                this.subfrmCommandButtonMain.funInitializeForm();
                this.subfrmNavigationButton.funInitializeForm();

                this.pfrmMainView.funInitializeForm();
                this.pfrmHistoryView.funInitializeForm();
                this.pfrmSystemSetup.funInitializeForm();
                pfrmInfoView.funInitializeForm();

                //3. TableLayoutPanel의 해당 영역에 Sub-Form을 위치시킨다.
                this.panelInfo.Controls.Add(pfrmMainView);
                this.panelInfo.Controls.Add(pfrmSystemSetup);
                this.panelInfo.Controls.Add(pfrmLogManagerView);
                this.panelInfo.Controls.Add(pfrmHistoryView);
                panelInfo.Controls.Add(pfrmInfoView);

                this.panelNavigation.Controls.Add(subfrmNavigationButton);

                this.panelCommand.Controls.Add(subfrmCommandButtonMain);
                this.panelCommand.Controls.Add(subfrmCommandButtonHistory);
                this.panelCommand.Controls.Add(subfrmCommandButtonLogManager);
                this.panelCommand.Controls.Add(subfrmCommandButtonSetting);
                this.panelCommand.Controls.Add(subfrmCommandButtonInfo);


                //4. 기타 Pop-Up Form을 생성한다.
                pfrmErrMsgList = new frmErrMsgList();
                this.pfrmErrMsgOverWrite = new frmErrMsgOverWrite();                
                this.pfrmModeChange = new frmModeChange();
                this.pfrmLogOn = new frmLogOn();
                this.pfrmEQPState = new frmEQPState();
                this.pfrmEQPProcessState = new frmProcessState();

                //5. Sub-Form의 Event들을 등록한다.
                int i = 0;
                i += 1;
                this.pfrmSystemSetup.SelectSaveDisableTab += new subfrmSetupView.eventSystemSetup(SaveDisalbeSettingTabSelected);
                this.pfrmSystemSetup.SelectSaveEnableTab += new subfrmSetupView.eventSystemSetup(SaveEnalbeSettingTabSelected);
                this.pfrmSystemSetup.SelectReloadEnableTab += new subfrmSetupView.eventSystemSetup(SelectReloadEnableTab);
                this.pfrmSystemSetup.SelectReloadDisableTab += new subfrmSetupView.eventSystemSetup(SelectReloadDisableTab);
                this.pfrmSystemSetup.SelectEditModeEnableTab += new subfrmSetupView.eventSystemSetup(SelectEditModeEnableTab);
                this.pfrmSystemSetup.SelectEditModeDisableTab += new subfrmSetupView.eventSystemSetup(SelectEditModeDisableTab);

                this.subfrmNavigationButton.ClickMainViewButton += new subfrmNavigationButton.eventClickNavigationButton(ClickMainView);
                this.subfrmNavigationButton.ClickHistoryViewButton += new subfrmNavigationButton.eventClickNavigationButton(ClickHistoryView);
                this.subfrmNavigationButton.ClickLogViewButton += new subfrmNavigationButton.eventClickNavigationButton(ClickLogView);
                this.subfrmNavigationButton.ClickSetupViewButton += new subfrmNavigationButton.eventClickNavigationButton(ClickSetupView);
                //this.subfrmNavigationButton.ClickSEMViewButton += new subfrmNavigationButton.eventClickNavigationButton(ClickSEMView);
                this.subfrmNavigationButton.ClickInfoButton += new DisplayAct.subfrmNavigationButton.eventClickNavigationButton(ClickInfoView);

                pfrmInfoView.SelectSaveEnableTab += new subfrmInfoView.eventInfoView(pfrmInfoView_SelectSaveEnableTab);
                pfrmInfoView.SelectSaveDisableTab += new subfrmInfoView.eventInfoView(pfrmInfoView_SelectSaveDisableTab);

                pfrmInfoView.SelectConfigTab += new subfrmInfoView.eventInfoView(pfrmInfoView_SelectConfigTab);
                pfrmInfoView.SelectHistotyTab += new subfrmInfoView.eventInfoView(pfrmInfoView_SelectHistotyTab);

                this.subfrmCommandButtonMain.ClickModeChangeButton += new subfrmCommandButton_Main.eventClickCommandButton_Main(ClickModeChange);
                this.subfrmCommandButtonMain.ClickEQPStateButton += new subfrmCommandButton_Main.eventClickCommandButton_Main(ClickEQPState);
                this.subfrmCommandButtonMain.ClickProcessStateButton += new subfrmCommandButton_Main.eventClickCommandButton_Main(ClickProcessState);
                this.subfrmCommandButtonMain.ClickBuzzerOffButton += new subfrmCommandButton_Main.eventClickCommandButton_Main(ClickBuzzerOff);
                this.subfrmCommandButtonMain.ClickMessageClearButton += new subfrmCommandButton_Main.eventClickCommandButton_Main(ClickMessageClear);
                this.subfrmCommandButtonHistory.ClickClearButton += new subfrmCommandButton_History.eventClickCommandButton_History(ClickHistoryClear);
                this.subfrmCommandButtonHistory.ClickSaveButton += new subfrmCommandButton_History.eventClickCommandButton_History(ClickHistorySave);

                this.subfrmCommandButtonLogManager.ClickSaveButton += new subfrmCommandButton_LogManager.eventClickCommandButton_LogManager(ClickLogSave);

                this.subfrmCommandButtonSetting.ClickSaveButton += new subfrmCommandButton_Setting.eventClickCommandButton_Setting(ClickSettingSave);
                this.subfrmCommandButtonSetting.ClickReloadButton += new subfrmCommandButton_Setting.eventClickCommandButton_Setting(ClickSettingReload);
                this.subfrmCommandButtonSetting.ClickEditMode += new subfrmCommandButton_Setting.eventClickCommandButton_Setting(ClickSettingEditMode);

                this.subfrmCommandButtonInfo.ClickHistoryButton += new subfrmCommandButton_Info.eventClickCommandButton_Info(subfrmCommandButtonInfo_ClickHistoryButton);
                subfrmCommandButtonInfo.ClickSEMConfigButton += new subfrmCommandButton_Info.eventClickCommandButton_Info(subfrmCommandButtonInfo_ClickSEMConfigButton);
                           
                this.pfrmLogOn.LogonSuccess += new frmLogOn.eventLogonDialog(SystemSetupLogon);

                //6. 화면 설정에 맞게 Sub-Form 크기 조절 및 보여질 화면, Button을 설정한다.
                funSetWindowSize();
                SetCommandButtonArea(typeof(subfrmCommandButton_Main).ToString());
                SetInfoPanelArea(typeof(subfrmMainView).ToString());
                funWriteLog_InitialTime("MainView");

                #endregion


            }
            catch (Exception error)
            {
                if (PInfo != null)
                {
                    PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                }
            }
        }

        void subfrmCommandButtonInfo_ClickSEMConfigButton(object sender, EventArgs e)
        {
            try
            {
                if (pfrmSEMConfig != null) pfrmSEMConfig.Close();

                pfrmSEMConfig = new frmSEMConfig();
                pfrmSEMConfig.subFormLoad();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void pfrmInfoView_SelectHistotyTab(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonInfo.ShowHistoryButton();
                this.subfrmCommandButtonInfo.HideConfigButton();
                this.subfrmCommandButtonInfo.HideReloadButton();

                // 20121129 이상창 추가
                TabControl dtc = (TabControl)sender;
                if (dtc.SelectedTab.Text == "RPC" /*|| dtc.SelectedTab.Text == "PPC"*/)
                {
                    this.subfrmCommandButtonInfo.ShowCreateNewButton();
                }
                else
                {
                    this.subfrmCommandButtonInfo.HideCreateNewButton();
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void pfrmInfoView_SelectConfigTab(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonInfo.ShowConfigButton();
                this.subfrmCommandButtonInfo.HideReloadButton();
                this.subfrmCommandButtonInfo.HideHistoryButton();
                this.subfrmCommandButtonInfo.HideCreateNewButton();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void pfrmInfoView_SelectSaveDisableTab(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonSetting.HideSaveButton();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void pfrmInfoView_SelectSaveEnableTab(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonSetting.ShowSaveButton();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }


        private void funWriteLog_InitialTime(string strName)
        {
            try
            {
                longEndTick = DateTime.Now.Ticks;
                longTimeSpan = longEndTick - longStartTick;
                longStartTick = longEndTick;

                string strText = string.Format("{0} InitialTime : {1:0.0000}ms", strName, (double)longTimeSpan / 10000.0);
                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, strText);
            }
            catch (Exception ex)
            {
                if (PInfo != null)
                {
                    PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                }
            }
        }

        /// <summary>
        /// System.ini에 설정된 Window Size에 맞추어 창을 조절한다.
        /// </summary>
        private void funSetWindowSize()
        {
           
        }

        /// <summary>
        /// MainView Area, 즉 Info Panel 위치에 어떤 화면을 보여줄지 설정한다.
        /// 선택된 View 외에는 Hide한다.
        /// </summary>
        /// <param name="strViewName">InfoPanel에 보여줄 View Name</param>
        private void SetInfoPanelArea(string strViewName)
        {
            try
            {
                string[] arrayString = new string[100];

                if ((strViewName == strCurrentViewName)) return;

                foreach (Control control in this.panelInfo.Controls)
                {
                    if (control.GetType().ToString() == strViewName)
                    {
                        strCurrentViewName = strViewName;
                        control.Dock = DockStyle.Fill;
                        control.Show();
                        subfrmTitlePanel.SetViewName(control.Tag.ToString());
                    }
                    else
                    {
                        control.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void SetInfoPanelArea(string strViewName, string strSubtabPage)
        {
            try
            {
                string[] arrayString = new string[100];

                if ((strViewName == strCurrentViewName)) return;

                foreach (Control control in this.panelInfo.Controls)
                {
                    if (control.GetType().ToString() == strViewName)
                    {
                        strCurrentViewName = strViewName;
                        control.Dock = DockStyle.Fill;
                        control.Show();

                        //if (strSubtabPage.Contains("OVEN"))
                        //{
                        //    subfrmSubDisplayView frm = (subfrmSubDisplayView)control;
                        //    frm.subLoadData(strSubtabPage);
                        //}
                        //else
                        //{
                        //    subfrmSubDisplayView frm = (subfrmSubDisplayView)control;
                        //    frm.subLoadData(strSubtabPage);
                        //}
                        subfrmTitlePanel.SetViewName(control.Tag.ToString());
                        break;
                    }
                    else
                    {
                        control.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Command Panel에 어떤 종류의 Command Button Control을 보여줄지 결정
        /// 선택된 Command Button Control외에는 Hide한다.
        /// </summary>
        /// <param name="strButtonName"></param>
        private void SetCommandButtonArea(string strButtonName)
        {
            try
            {
                if (strButtonName == strCurrentButtonName) return;

                foreach (Control control in this.panelCommand.Controls)
                {
                    if (control.GetType().ToString() == strButtonName)
                    {
                        strCurrentButtonName = strButtonName;
                        control.Dock = DockStyle.Fill;
                        control.Show();
                    }
                    else
                    {
                        control.Hide();
                    }
                }

            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void tmrMainView_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrMainView.Enabled = false;
                if (this.PInfo.funOPCallCount() > 0)
                {
                    subOPCallGet();     //OPCall Queue에 항목이 들어있으면 처리한다
                }

                if (this.PInfo.funOPCallOverWriteCount() > 0)
                {
                    subOPCallOverWriteGet();
                }

                if (PInfo.ViewEventCount() > 0)
                {
                    subProcViewEvent();
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }

            tmrMainView.Enabled = true;

        }

        //Queue<string> S9F13Msg = new Queue<string>();
        //private void subS9F13MsgPop(string strMSG)
        //{
        //    //string strMSG = "";
        //    try
        //    {
        //        S9F13Msg.Enqueue(strMSG);
                
        //        if (S9F13Msg.Count > 10)
        //        {
        //            S9F13Msg.Dequeue();
        //        }

        //        pfrmS9F13MSG.subFormLoad(strMSG);

        //    }
        //    catch (Exception ex)
        //    {
        //        PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
        //    }
        //}

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.pfrmExit = new frmExit();
                //this.pfrmExit.PInfo = this.PInfo;
                this.pfrmExit.subFormLoad();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subClose()
        {
            this.Close();
            this.Dispose();
        }


        private void subProcViewEvent()
        {
            clsInfo.ViewEvent evtView = PInfo.GetViewEvent();

            switch (evtView)
            {
                case clsInfo.ViewEvent.ECIDUpdate:
                    {
                        pfrmSystemSetup.subUpdateSettingData(evtView);
                    }
                    break;
                case clsInfo.ViewEvent.EOIDUpdate:
                    {
                        pfrmSystemSetup.subUpdateSettingData(evtView);
                    }
                    break;
            }
        }


        /// <summary>
        /// Queue에서 OPCall Object를 가져와 Type별로 분기시켜 Form을 발생
        /// </summary>
        private void subOPCallGet()
        {
            int dintOPCallType;
            int dintPortID;
            string dstrFromSF;
            string dstrHostMsg;
            InfoAct.clsOPCall dclsOPCall;

            try
            {
                //이미 다른 OPCall Form이 떠있으면 빠져나간다
                if (PInfo.All.OperatorCallFormVisible == true) return;

                dclsOPCall = (InfoAct.clsOPCall)PInfo.funGetOPCall();     //OPCall Queue에서 항목을 가져온다

                dintOPCallType = dclsOPCall.intOPCallType;
                dintPortID = dclsOPCall.intPortID;
                dstrFromSF = dclsOPCall.strFromSF;
                dstrHostMsg = dclsOPCall.strHostMsg;

                switch (dintOPCallType)     //OPCall Type별로 분기하여 Form을 띄운다
                {
                    case (int)InfoAct.clsInfo.OPCall.MSG:
                        //Buzzer를 울린다
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn);
                        //this.pfrmErrMSG.subFormLoad(dstrHostMsg, dintOPCallType);   //Message창을 띄운다
                        this.PInfo.All.OperatorCallFormVisible = true;
                        break;

                    //case (int)InfoAct.clsInfo.OPCall.S10F3HostMSG:
                    //    //Buzzer를 울린다
                    //    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn);

                    //    this.pfrmErrMSG.PInfo = this.PInfo;
                    //    this.pfrmErrMSG.subFormLoad(dstrHostMsg, dintOPCallType);   //Message창을 띄운다
                    //   this.PInfo.All.OperatorCallFormVisible = true;
                    //    break;

                    case (int)InfoAct.clsInfo.OPCall.Scrap:
                        //Buzzer를 울린다
                        //this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn);
                        //this.pfrmScrapUnscrapAbort.subFormLoad(dstrHostMsg, dintOPCallType);   //Message창을 띄운다
                        //PInfo.All.OperatorCallFormVisible = true;
                        break;

                    case (int)InfoAct.clsInfo.OPCall.Unscrap:
                        //Buzzer를 울린다
                        //this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn);
                        //this.pfrmScrapUnscrapAbort.subFormLoad(dstrHostMsg, dintOPCallType);   //Message창을 띄운다
                        //this.PInfo.All.OperatorCallFormVisible = true;
                        break;

                    case (int)InfoAct.clsInfo.OPCall.GLSAbort:
                        //Buzzer를 울린다
                        //this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn);
                        //this.pfrmScrapUnscrapAbort.subFormLoad(dstrHostMsg, dintOPCallType);   //Message창을 띄운다
                        //this.PInfo.All.OperatorCallFormVisible = true;
                        break;

                    case (int)InfoAct.clsInfo.OPCall.OnlineModeChangeT3TimeOut:
                        //Buzzer를 울린다
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn);
                        break;

                    case (int)InfoAct.clsInfo.OPCall.T3TimeOut:
                        //Buzzer를 울린다
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn);
                        break;

                    case (int)InfoAct.clsInfo.OPCall.FormClose:                             //OPCall.FormClose는 frmMSG와 frmModeChange에서 공동사용
                        if (this.PInfo.All.ModeChangeFormVisible == true)
                        {
                            this.pfrmModeChange.Hide();
                            this.PInfo.All.ModeChangeFormVisible = false;
                        }
                        break;

                    case (int)InfoAct.clsInfo.OPCall.ModeChangeShow:                         //Online Mode Change창을 띄운다.
                        this.pfrmModeChange.subFormLoad();
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Queue에서 OpCall Object를 가져와 Type별로 분기시켜 Form 발생
        /// 창이 떠 있어도 Overwrite 가능
        /// </summary>
        private void subOPCallOverWriteGet()
        {
            int dintOPCallType;
            int dintPortID;
            string dstrFromSF;
            string dstrHostMsg;
            InfoAct.clsOPCall dclsOPCall;

            try
            {
                dclsOPCall = (InfoAct.clsOPCall)PInfo.funGetOPCallOverWrite();     //OPCallOverWrite Queue에서 항목을 가져온다

                dintOPCallType = dclsOPCall.intOPCallType;
                dintPortID = dclsOPCall.intPortID;
                dstrFromSF = dclsOPCall.strFromSF;
                dstrHostMsg = dclsOPCall.strHostMsg;

                switch (dintOPCallType)     //OPCall Type별로 분기하여 Form을 띄운다
                {
                    case (int)InfoAct.clsInfo.OPCallOverWrite.MSGBuzzer:   //Buzzer On
                        //Buzzer를 울린다
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn);
                        this.pfrmErrMsgOverWrite.subFormLoad(dstrHostMsg, dintOPCallType, dintPortID);   //Message창을 띄운다
                        break;

                    case (int)InfoAct.clsInfo.OPCallOverWrite.MSGNoBuzzer:   //No Buzzer
                        //this.pfrmErrMsgOverWrite.PInfo = this.PInfo;
                        //this.pfrmErrMsgOverWrite.subFormLoad(dstrHostMsg, dintOPCallType, dintPortID);   //Message창을 띄운다
                        //break;
                        //this.pfrmErrMsgList.PInfo = this.PInfo;
                        //PInfo.All.OperatorCallFormVisible = true;
                        this.pfrmErrMsgList.subFormLoad(dstrHostMsg, dintOPCallType);   //Message창을 띄운다
                        break;

                    case (int)InfoAct.clsInfo.OPCallOverWrite.MSGList:
                        //Buzzer를 울린다
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn);

                        //this.pfrmErrMsgList.PInfo = this.PInfo;
                        //PInfo.All.OperatorCallFormVisible = true;
                        this.pfrmErrMsgList.subFormLoad(dstrHostMsg, dintOPCallType);   //Message창을 띄운다
                        break;

                    case (int)InfoAct.clsInfo.OPCallOverWrite.OPCallClear:
                        this.pfrmErrMsgList.subClose(true);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void frmMainNew_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Alt == true && e.KeyCode == Keys.F4)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion

        #region System Setup Events
        /// <summary>
        /// 설정을 변경할 수 없는 Setting 화면이 선택되었을 때 Save Button을 숨긴다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveDisalbeSettingTabSelected(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonSetting.HideSaveButton();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 설정을 변경할 수 있는 Setting 화면이 선택되었을 때 Save Button을 나타낸다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveEnalbeSettingTabSelected(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonSetting.ShowSaveButton();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void SelectReloadEnableTab(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonSetting.ShowReloadButton();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void SelectReloadDisableTab(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonSetting.HideReloadButton();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void SelectEditModeEnableTab(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonSetting.ShowListEditCheckBox();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void SelectEditModeDisableTab(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonSetting.HideListEditCheckBox();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }


        void pfrmInfomationView_SelectConfigTab(object sender, EventArgs e)
        {
            try
            {
                this.subfrmCommandButtonInfo.ShowConfigButton();
                this.subfrmCommandButtonInfo.HideReloadButton();
                this.subfrmCommandButtonInfo.HideHistoryButton();
                this.subfrmCommandButtonInfo.HideCreateNewButton();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void pfrmInfomationView_SelectHistotyTab(object sender, EventArgs e)
        {
            try
            {
                


                this.subfrmCommandButtonInfo.ShowHistoryButton();
                this.subfrmCommandButtonInfo.HideConfigButton();
                this.subfrmCommandButtonInfo.HideReloadButton();


                // 20121129 이상창 추가
                TabControl dtc = (TabControl)sender;
                if (dtc.SelectedTab.Text == "RPC" /*|| dtc.SelectedTab.Text == "PPC"*/)
                {
                    this.subfrmCommandButtonInfo.ShowCreateNewButton();
                }
                else
                {
                    this.subfrmCommandButtonInfo.HideCreateNewButton();
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }



        #endregion

        #region Navigation Button Events
        /// <summary>
        /// Navigation Button 중 Main View Button이 Click 되었을 때의 Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickMainView(object sender, EventArgs e)
        {
            try
            {
                if ((typeof(subfrmMainView).ToString()) == strCurrentViewName) return;

                SetInfoPanelArea(typeof(subfrmMainView).ToString());
                SetCommandButtonArea(typeof(subfrmCommandButton_Main).ToString());
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Navigation Button 중 History View Button이 Click 되었을 때의 Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickHistoryView(object sender, EventArgs e)
        {
            try
            {
                if ((typeof(subfrmHistoryView).ToString()) == strCurrentViewName) return;

                SetInfoPanelArea(typeof(subfrmHistoryView).ToString());
                SetCommandButtonArea(typeof(subfrmCommandButton_History).ToString());
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Navigation Button 중 Log View Button이 Click 되었을 때의 Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickLogView(object sender, EventArgs e)
        {
            try
            {
                if ((typeof(subfrmLogManagerView).ToString()) == strCurrentViewName) return;

                SetInfoPanelArea(typeof(subfrmLogManagerView).ToString());
                SetCommandButtonArea(typeof(subfrmCommandButton_LogManager).ToString());
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Navigation Button 중 Setup View Button이 Click 되었을 때의 Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickSetupView(object sender, EventArgs e)
        {
            try
            {
                if ((typeof(subfrmSetupView).ToString()) == strCurrentViewName) return;

                pfrmLogOn.pstrVCRorSYstemSetup = "SYSTEMSETUP";
                pfrmLogOn.subFormLoad();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Setup View 진입 시 LogOn이 성공적으로 이루어 졌을 때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemSetupLogon(object sender, EventArgs e)
        {
            try
            {
                SetInfoPanelArea(typeof(subfrmSetupView).ToString());
                SetCommandButtonArea(typeof(subfrmCommandButton_Setting).ToString());
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void ClickSEMView(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void ClickInfoView(object sender, EventArgs e)
        {
            try
            {
                if ((typeof(subfrmInfoView).ToString()) == strCurrentViewName) return;
                SetInfoPanelArea(typeof(subfrmInfoView).ToString());
                SetCommandButtonArea(typeof(subfrmCommandButton_Info).ToString());
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region Command Button (Main) Events
        /// <summary>
        /// Main Command Button의 Mode Change Command Button Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickModeChange(object sender, EventArgs e)
        {
            try
            {
                this.PInfo.subOPCall_Set(InfoAct.clsInfo.OPCall.ModeChangeShow, 0, "", "");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Main Command Button의 EQP State Change Command Button Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickEQPState(object sender, EventArgs e)
        {
            try
            {
                this.pfrmEQPState.subFormLoad();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Main Command Button의 Process State Change Command Button Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickProcessState(object sender, EventArgs e)
        {
            try
            {
                this.pfrmEQPProcessState.subFormLoad();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Main Command Button의 Buzzer Off Command Button Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickBuzzerOff(object sender, EventArgs e)
        {
            try
            {
                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOff);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Main Command Button의 Message Clear Command Button Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickMessageClear(object sender, EventArgs e)
        {
            try
            {
                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageClear);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion

        #region Command Button (History) Events
        /// <summary>
        /// History Command Button의 Clear Command Button Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickHistoryClear(object sender, EventArgs e)
        {
            try
            {
                pfrmHistoryView.Clear();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// History Command Button의 Save Command Button Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickHistorySave(object sender, EventArgs e)
        {
            try
            {
                //pfrmHistoryView.Save(); // 20130219 lsc
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Command Button (Log Manager) Events
        /// <summary>
        /// LogManager Commaan Button의 Save Button Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickLogSave(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion

        #region Command Button (Setting) Event
        /// <summary>
        /// Setup Command Button의 Save Command Button Event 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickSettingSave(object sender, EventArgs e)
        {
            try
            {
                pfrmSystemSetup.SaveSetting();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void ClickSettingEditMode(object sender, EventArgs e)
        {
            try
            {
                pfrmSystemSetup.EditModeSetting();
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void ClickSettingReload(object sender, EventArgs e)
        {
            try
            {

                pfrmSystemSetup.Reload();

                
            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion

        #region Command Button (Info) Event
        void subfrmCommandButtonInfo_ClickHistoryButton(object sender, EventArgs e)
        {
            try
            {
                if ((typeof(subfrmHistoryView).ToString()) == strCurrentViewName) return;

                SetInfoPanelArea(typeof(subfrmHistoryView).ToString());
                SetCommandButtonArea(typeof(subfrmCommandButton_History).ToString());

                //this.pfrmHistoryView.
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (PInfo.All.ProgramEnd == false)
                {
                    e.Cancel = true;
                    btnExit_Click(null, null);
                    return;
                }
            }
            catch
            {
            }
        }
    }
}
