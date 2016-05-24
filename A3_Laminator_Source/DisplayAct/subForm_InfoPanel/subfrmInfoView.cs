using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using DBAct;
using System.Collections;

namespace DisplayAct
{
    public partial class subfrmInfoView : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;
        private tabfrmInfoSEM tabfrmSEM;
        private tabfrmInfoAPC tabfrmAPC;
        //private tabfrmInfoPPC tabfrmPPC;
        private tabfrmInfoRPC tabfrmRPC;
        private tabfrmInfoMultiData tabfrmMultiUseData;
        
        private TabPage pageSEM = new TabPage();
        private TabPage pageAPC = new TabPage();
        //private TabPage pagePPC = new TabPage();
        private TabPage pageRPC = new TabPage();
        private TabPage pageMulti = new TabPage();

        #endregion


        #region Properties
        /// <summary>
        /// 현재 선택된 Tab의 TEXT를 가져 옵니다.
        /// </summary>
        public string SelectedTab   // 20121129 이상창 추가
        {
            get
            {
                return this.tabControl1.SelectedTab.Text;
            }
        }
        #endregion


        #region Events
        public delegate void eventInfoView (object sender, EventArgs e);

        public event eventInfoView SelectSaveEnableTab;
        public event eventInfoView SelectSaveDisableTab;

        public event eventInfoView SelectConfigTab;
        public event eventInfoView SelectHistotyTab;

        #endregion

        #region Constructors
        public subfrmInfoView()
        {
            InitializeComponent();
        }
        #endregion

        public void funInitializeForm()
        {
            try
            {
                tabfrmSEM = new tabfrmInfoSEM();
                tabfrmSEM.Dock = DockStyle.Fill;

                tabfrmAPC = new tabfrmInfoAPC();
                tabfrmAPC.Dock = DockStyle.Fill;

                //tabfrmPPC = new tabfrmInfoPPC();
                //tabfrmPPC.Dock = DockStyle.Fill;

                tabfrmRPC = new tabfrmInfoRPC();
                tabfrmRPC.Dock = DockStyle.Fill;

                tabfrmMultiUseData = new tabfrmInfoMultiData();
                tabfrmMultiUseData.Dock = DockStyle.Fill;

                this.tabControl1.Controls.Clear();
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subfrmInfoView_Load(object sender, EventArgs e)
        {
            try
            {
                tmrUpdate.Enabled = true;

                pageSEM.Text = "SEM";
                pageSEM.Controls.Add(tabfrmSEM);

                pageAPC.Text = "APC";
                pageAPC.Controls.Add(tabfrmAPC);

                //pagePPC.Text = "PPC";
                //pagePPC.Controls.Add(tabfrmPPC);

                pageRPC.Text = "RPC";
                pageRPC.Controls.Add(tabfrmRPC);

                pageMulti.Text = "MultiData";
                pageMulti.Controls.Add(tabfrmMultiUseData);

                this.tabControl1.Controls.Add(pageSEM);
                this.tabControl1.Controls.Add(pageAPC);
                //this.tabControl1.Controls.Add(pagePPC);
                this.tabControl1.Controls.Add(pageRPC);
                this.tabControl1.Controls.Add(pageMulti);
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subLoadData(string strPageName)
        {
            try
            {
                foreach (Control con in tabControl1.Controls)
                {
                    try
                    {
                        TabPage tp = (TabPage)con;
                        if (tp.Text == "SEM")
                        {
                            tabControl1.SelectedIndex = 0;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void SaveSetting()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.tabControl1.SelectedTab.Text == "SEM")
                {
                    if (SelectConfigTab != null) SelectConfigTab(this, e);
                }
                else if (this.tabControl1.SelectedTab.Text == "APC" || this.tabControl1.SelectedTab.Text == "PPC" || this.tabControl1.SelectedTab.Text == "RPC")
                {
                    if (SelectHistotyTab != null) SelectHistotyTab(this.tabControl1, e);    // 20121129 RPC, PPC Create New Set 추가로 sender 변경
                }
                else
                {
                    if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                }
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            string dstrLogData = "";
            string dstrSQL = "";
            DateTime dtNow;
            ArrayList darrExpiration = new ArrayList();

            try
            {
                tmrUpdate.Enabled = false;
                if (this.pInfo.All.APCDBUpdateCheck == true)
                {
                    dstrSQL = "SELECT tbAPC.SET_TIME, tbAPC.H_GLASSID, tbAPC.JOBID, tbAPC.RECIPE, tbAPC_Sub.P_PARM_NAME, tbAPC_Sub.P_PARM_VALUE, IIF(tbAPC.[APC_STATE]='1', 'Waiting', IIF(tbAPC.[APC_STATE]= '2', 'Running', IIF(tbAPC.[APC_STATE]='3', 'Done', '9'))) AS APC_STATE, tbAPC.Operation, tbAPC_Sub.isCenter FROM tbAPC INNER JOIN tbAPC_Sub ON tbAPC.[H_GLASSID] = tbAPC_Sub.[H_GLASSID] ORDER BY tbAPC.SET_TIME, tbAPC_Sub.P_PARM_NAME ASC;";//, tbAPC_Sub.P_PARM_NAME
                    DataTable dDataTable = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                    tabfrmAPC.apcGrid.DataSource = dDataTable;

                    //apcGrid.subSetGridView();
                    this.pInfo.All.APCDBUpdateCheck = false;
                }

                //if (this.pInfo.All.PPCDBUpdateCheck == true)
                //{
                //    dstrSQL = "SELECT tbPPC.SET_TIME, tbPPC.H_GLASSID, tbPPC.JOBID, tbPPC_Sub.P_MODULEID, tbPPC_Sub.P_ORDER, IIF(tbPPC_Sub.[P_STATE] ='1', 'Waiting', IIF(tbPPC_Sub.[P_STATE] = '2', 'Running', IIF(tbPPC_Sub.[P_STATE] = '3', 'Done', '9'))) AS P_STATE, tbPPC.Operation, tbPPC_Sub.isCenter, tbPPC.isRun FROM tbPPC INNER JOIN tbPPC_Sub ON tbPPC.[H_GLASSID] = tbPPC_Sub.[H_GLASSID] ORDER BY tbPPC.SET_TIME, tbPPC_Sub.P_ORDER ASC;";
                //    DataTable dDataTable = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                //    tabfrmPPC.ppcGrid.DataSource = dDataTable;

                //    //apcGrid.subSetGridView();
                //    this.pInfo.All.PPCDBUpdateCheck = false;
                //}

                if (this.pInfo.All.RPCDBUpdateCheck == true)
                {
                    dstrSQL = "SELECT tbRPC.SET_TIME, tbRPC.H_GLASSID, tbRPC.JOBID, tbRPC.RPC_PPID, tbRPC.ORIGINAL_PPID, IIF(tbRPC.[RPC_STATE] ='1', 'Waiting', IIF(tbRPC.[RPC_STATE] = '2', 'Running', IIF(tbRPC.[RPC_STATE] = '3', 'Done', '9'))) AS RPC_STATE, tbRPC.Operation FROM tbRPC ORDER BY tbRPC.SET_TIME ASC;";
                    DataTable dDataTable = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                    tabfrmRPC.rpcGrid.DataSource = dDataTable;

                    //apcGrid.subSetGridView();
                    this.pInfo.All.RPCDBUpdateCheck = false;
                }

                dtNow = DateTime.Now;

                if (dtNow.ToString("mmss") == "0000")
                {
                    foreach (string str in this.pInfo.APC())
                    {
                        clsAPC currentAPC = this.pInfo.APC(str);

                        DateTime dtCompare = currentAPC.SetTime;

                        if ((dtNow - dtCompare).TotalDays >= pInfo.All.ProcDataKeepDays)
                        {
                            darrExpiration.Add(str);
                        }
                    }
                    subExpirationReport(darrExpiration, clsInfo.ProcessDataType.APC);

                    foreach (string str in this.pInfo.PPC())
                    {
                        clsPPC currentPPC = this.pInfo.PPC(str);

                        DateTime dtCompare = currentPPC.SetTime;

                        if ((dtNow - dtCompare).Days >= pInfo.All.ProcDataKeepDays)
                        {
                            darrExpiration.Add(str);
                        }
                    }
                    subExpirationReport(darrExpiration, clsInfo.ProcessDataType.PPC);

                    foreach (string str in this.pInfo.RPC())
                    {
                        clsRPC currentRPC = this.pInfo.RPC(str);

                        DateTime dtCompare = currentRPC.SetTime;

                        if ((dtNow - dtCompare).Days >= pInfo.All.ProcDataKeepDays)
                        {
                            darrExpiration.Add(str);
                        }
                    }
                    subExpirationReport(darrExpiration, clsInfo.ProcessDataType.RPC);

                }

                tmrUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
                tmrUpdate.Enabled = true;
            }
        }

        private void subExpirationReport(ArrayList darrExpiration, clsInfo.ProcessDataType pdType)
        {
            string dstrLogData = "";
            try
            {
                foreach (string str in darrExpiration)
                {
                    switch (pdType)
                    {
                        case clsInfo.ProcessDataType.APC:
                            InfoAct.clsAPC CurrentAPC = pInfo.APC(str);
                           CurrentAPC.SetTime = DateTime.Now;

                            pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataDel, clsInfo.ProcessDataType.APC, "3", "3!"+CurrentAPC.GLSID, true);
                            //// APC Log 작성
                            //dstrLogData += "APC Data 만료!! => ";
                            //dstrLogData += "GLASSID : " + CurrentAPC.GLSID;
                            //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.APC, CurrentAPC.SetTime.ToString("yyyyMMddHHmmss"), dstrLogData);
                            //dstrLogData = "";
                            this.pInfo.All.APCDBUpdateCheck = true;
                            break;

                        case clsInfo.ProcessDataType.PPC:
                            InfoAct.clsPPC CurrentPPC = this.pInfo.PPC(str);
                            CurrentPPC.SetTime = DateTime.Now;

                            pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataDel, clsInfo.ProcessDataType.PPC, "3", "3!" + CurrentPPC.HGLSID, true);
                            //// PPC Log 작성
                            //dstrLogData += "PPC Data 만료!! => ";
                            //dstrLogData += "GLASSID : " + CurrentPPC.HGLSID;
                            //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.PPC, CurrentPPC.SetTime.ToString("yyyyMMddHHmmss"), dstrLogData);
                            //dstrLogData = "";
                            this.pInfo.All.PPCDBUpdateCheck = true;
                            break;

                        case clsInfo.ProcessDataType.RPC:
                            InfoAct.clsRPC CurrentRPC = this.pInfo.RPC(str);
                            CurrentRPC.SetTime = DateTime.Now;

                            pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataDel, clsInfo.ProcessDataType.RPC, "3", "3!" + CurrentRPC.HGLSID, true);
                            //// RPC Log 작성
                            //dstrLogData += "RPC Data 만료!! => ";
                            //dstrLogData += "GLASSID : " + CurrentRPC.HGLSID;
                            //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.RPC, CurrentRPC.SetTime.ToString("yyyyMMddHHmmss"), dstrLogData);
                            this.pInfo.All.RPCDBUpdateCheck = true;
                            break;

                        default:
                            return;
                    }
                }

                darrExpiration.Clear();

                
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subClose()
        {
            tmrUpdate.Enabled = false;
            tabfrmSEM.subClose();
        }

    }
}
