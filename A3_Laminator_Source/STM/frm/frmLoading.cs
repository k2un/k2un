//#define SIMULATION // 조영훈, 시뮬레이션 키거나 끈다

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using CommonAct;



namespace STM
{
    public partial class frmLoading : Form
    {
        private clsHostActPlugIn pclsHostActPlugIn;
        private clsEqpActPlugIn pclsEqpActPlugIn;
        private clsInfoActPlugIn pclsInfoPlugIn;
        private clsDBActPlugIn pclsDBActPlugIn;
        private clsDisplayActPlugIn pclsDisplayActPlugIn;
        private clsLogActPlugIn pclsLogActPlugIn;

        private clsInfo pInfo = clsInfo.Instance;



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

                this.TopMost = true;

                this.Show();
                this.Refresh();

                subclsInitial();
                

            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }



        /// <summary>
        /// 각 Class를 Loading하고 Initial한다.
        /// </summary>
        public void subclsInitial()
        {
            try
            {
                subProgressText("PlugIn Loading");
                pclsDBActPlugIn = new clsDBActPlugIn();                                 //DB를 다루기위한 PlugIn cls정의및 생성
                pclsInfoPlugIn = new clsInfoActPlugIn();                                //구조체를 다루기위한 PlugIn cls정의및 생성
                pclsLogActPlugIn = new clsLogActPlugIn();
                pclsHostActPlugIn = new clsHostActPlugIn();                             //호스트를 다루기위한 PlugIn cls정의및 생성
                pclsEqpActPlugIn = new clsEqpActPlugIn();                               //장비를 다루기위한 PlugIn cls정의및 생성
                pclsDisplayActPlugIn = new clsDisplayActPlugIn();

                if (pInfo.AddAll() == true)
                {
                    string dModelINI = Application.StartupPath + @"\system\EqpModel.ini";
                    pInfo.All.MODEL_NAME = FunINIMethod.funINIReadValue("MODEL", "NAME", "", dModelINI);
                }

                subProgressText("DBAct Initial");
                pclsDBActPlugIn.funConnectDB(pInfo.All.MODEL_NAME);                                         //DB OPEN

                subProgressText("InfoAct Initial");
                pclsInfoPlugIn.subInitialInfo(pInfo.All.MODEL_NAME);                                        //구조체 초기화

                subProgressText("LogAct Initial");
                pclsLogActPlugIn.subInitialLog();                                       //Log 폴더 Initial

                subProgressText("DisplayAct Initial");
                pclsDisplayActPlugIn.subInitial();                                      //Main Form Load

                subProgressText("EqpAct Initial");
                pclsEqpActPlugIn.funOpenPLC();                                          //장비 초기화

                subProgressText("HostAct Initial");
                pclsHostActPlugIn.funOpenSecs("EAP01");                                 //호스트 초기화

                pclsDBActPlugIn.funDisconnectDB();                                      //DB CLOSE
                this.pgbLoading.Value = 100;
               
                
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Prograss Bar를 증가시킨다.
        /// </summary>
        /// <param name="strText">표시할 데이타.</param>
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

        /// <summary>
        /// 프로그램 시작시 폼을 숨기고 프로그램 종료신호를 받아 모든 리소스를 해제후 종료한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrControl_Tick(object sender, EventArgs e)
        {
            try
            {
                if (pgbLoading.Value == 100)
                {
                    pgbLoading.Value = 0;
                    this.Hide();
                }

                if (pInfo.All.ProgramEnd == true)
                {
                    this.tmrControl.Enabled = false;
                    subClose();
                }
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


                pclsHostActPlugIn.subClose();
                pclsEqpActPlugIn.subClosePLC();
                pclsDBActPlugIn.funDisconnectDB();
                pclsDisplayActPlugIn.subClose();
                pclsInfoPlugIn.subClose();
                pclsLogActPlugIn.subClose();


                this.Close();

                //GC.Collect();                       //더이상 사용하지 않는 객체를 수집하라.
                //GC.WaitForPendingFinalizers();      //수집한 객체들이 메모리에서 사라질때까지 대기하라.

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}