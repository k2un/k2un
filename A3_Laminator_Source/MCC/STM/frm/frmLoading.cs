using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CommonAct;
using InfoAct;


namespace STM
{
    public partial class frmLoading : Form
    {
        private clsEqpActPlugIn pclsEqpActPlugIn;
        private clsInfoActPlugIn pclsInfoPlugIn;
        private clsDBActPlugIn pclsDBActPlugIn;
        private clsLogActPlugIn pclsLogActPlugIn;

        private clsInfo pInfo = clsInfo.Instance;

        public bool ConnectFlag = false;
        


        //*******************************************************************************
        //  Function Name : frmLoading()
        //  Description   : Form을 초기화한다.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/02          어 경태             [L 00]
        //*******************************************************************************
        public frmLoading()
        {
            try
            {
                InitializeComponent();

                

                pgbLoading.Visible = true;
                pgbLoading.Minimum = 0;
                pgbLoading.Maximum = 100;
                pgbLoading.Value = 0;
                pgbLoading.Step = 10;

                this.Show();
                this.Refresh();

                subclsInitial();

                subDataLoad();
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subDataLoad()
        {
            try
            {
                //contextMenuStrip1.Items.Add("RESTART");
                contextMenuStrip1.Items.Add("CLOSE");
                notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            }
            catch (Exception ex)
            {
                
            }
        }

        


        //*******************************************************************************
        //  Function Name : subclsInitial()
        //  Description   : 각 Class를 Loading하고 Initial한다.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/02          어 경태             [L 00]
        //*******************************************************************************
        public void subclsInitial()
        {
            try
            {
                subProgressText("PlugIn Loading");
                pclsDBActPlugIn = new clsDBActPlugIn();                                 //DB를 다루기위한 PlugIn cls정의및 생성
                pclsInfoPlugIn = new clsInfoActPlugIn();                                //구조체를 다루기위한 PlugIn cls정의및 생성
                pclsLogActPlugIn = new clsLogActPlugIn();
                pclsEqpActPlugIn = new clsEqpActPlugIn();                               //장비를 다루기위한 PlugIn cls정의및 생성

                if (pInfo.AddAll() == true)
                {
                    string dModelINI = @"D:\Source\STM\bin\Debug\system\EqpModel.ini";
                    pInfo.All.MODEL_NAME = FunINIMethod.funINIReadValue("MODEL", "NAME", "", dModelINI);
                }

                subProgressText("DBAct Initial");
                pclsDBActPlugIn.funConnectDB(pInfo.All.MODEL_NAME);                                         //DB OPEN
                
                subProgressText("InfoAct Initial");
                pclsInfoPlugIn.subInitialInfo(pInfo.All.MODEL_NAME);                    //구조체 초기화

                subProgressText("LogAct Initial");
                //pclsLogActPlugIn.PInfo = this.pInfo;
                pclsLogActPlugIn.subInitialLog();                                       //Log 폴더 Initial

                subProgressText("EqpAct Initial");
                pclsEqpActPlugIn.funOpenPLC();                                          //장비 초기화

                pclsDBActPlugIn.funDisconnectDB();                                      //DB CLOSE
                this.pgbLoading.Value = 100;


            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subProgressText()
        //  Description   : Prograss Bar를 증가시킨다.
        //  Parameters    : strText => 표시할 데이타.
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/02          어 경태             [L 00]
        //*******************************************************************************
        public void subProgressText(string strText)
        {
            try
            {
                this.pgbLoading.PerformStep();
                this.lblProcess.Text = strText;
                this.Refresh();
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : tmrControl_Tick()
        //  Description   : 프로그램 시작시 폼을 숨기고 프로그램 종료신호를 받아 모든 리소스를 해제후 종료한다.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/02          어 경태             [L 00]
        //*******************************************************************************
        private void tmrControl_Tick(object sender, EventArgs e)
        {
            bool checkFlag = false;
            try
            {
                tmrControl.Enabled = false;
                if (pgbLoading.Value == 100)
                {
                    pgbLoading.Value = 0;
                    this.Hide();
                }

                if (pInfo.EQP("Main").MainEQPConnect != ConnectFlag)
                {
                    ConnectFlag = pInfo.EQP("Main").MainEQPConnect;

                    if (ConnectFlag)
                    {
                        label1.BackColor = Color.Yellow;
                        label1.Text = "Connected";
                    }
                    else
                    {
                        label1.BackColor = Color.Red;
                        label1.Text = "DisConnected";
                    }
                }

                checkFlag = true;
                if (pInfo.All.ProgramEnd == true)
                {
                    this.tmrControl.Enabled = false;
                    subClose();
                    checkFlag = false;
                    System.Environment.Exit(0);

                }
                tmrControl.Enabled = checkFlag;

            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subClose()
        {
            try
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Program End");

             

                //pclsHostActPlugIn.subClose();
                pclsEqpActPlugIn.subClosePLC();
                pclsDBActPlugIn.funDisconnectDB();
                
                //pclsDisplayActPlugIn.subClose();
                pclsInfoPlugIn.subClose();

                this.Close();

                GC.Collect();                       //더이상 사용하지 않는 객체를 수집하라.
                GC.WaitForPendingFinalizers();      //수집한 객체들이 메모리에서 사라질때까지 대기하라.
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                pInfo.All.ProgramEnd = true;
            }
            catch (Exception ex)
            {
                
                //throw;
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {

                switch (e.ClickedItem.ToString())
                {
                    //case "RESTART":
                    //    break;

                    case "CLOSE":
                        pInfo.All.ProgramEnd = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                
            }
        }



       
    }
}