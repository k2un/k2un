using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using CommonAct;
using InfoAct;
using DBAct;
using System.Collections;

namespace DisplayAct
{
    public partial class tabfrmSetupPPID : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;
        private frmPPIDList pfrmPPIDList;
        private string strSelectPPIDName = "";
        private string strSelectSubPPIDName = "";

        private frmHostPPIDCreate pfrmHostPPIDCreate;
        private frmHostPPIDModify pfrmHostPPIDModify;
        private frmEQPPPIDCreate pfrmEQPPPIDCreate;
        private frmEQPPPIDModify pfrmEQPPPIDModify;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmSetupPPID()
        {
            InitializeComponent();
            subEventAdd();
            subInit();
        }

        private void subEventAdd()
        {
            try
            {
                //pInfo.Unit(0).SubUnit(0).Event_EQPPPID_Update += new clsSubUnitMethod.EQPPPID_Update_Event(tabfrmSetupPPID_Event_EQPPPID_Update);
                pInfo.Unit(0).SubUnit(0).Event_HOSTPPID_Update += new clsSubUnitMethod.HOSTPPID_Update_Event(tabfrmSetupPPID_Event_HOSTPPID_Update);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void tabfrmSetupPPID_Event_HOSTPPID_Update()
        {
            try
            {
                subDBLoadInit(2);
                subInit();
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void tabfrmSetupPPID_Event_EQPPPID_Update()
        {
            try
            {
                subDBLoadInit(1);
                subInit();
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subDBLoadInit(int dintType)
        {
            string dstrSQL = "";
            DataTable dDT = new DataTable();
            try
            {
                if (dintType == 1)
                {
                    //pInfo.Unit(0).SubUnit(0).RemoveMappingEQPPPID();
                    dstrSQL = "SELECT * FROM tbEQPPPID";
                    dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                    if (dDT != null)
                    {
                        pInfo.DeleteTable("MappingEQPPPID");
                        pInfo.AddDataTable("MappingEQPPPID", dDT);

                        string strEQPPPID = "";
                        foreach (DataRow dr in dDT.Rows)
                        {
                            strEQPPPID = dr["EQPPPID"].ToString().Trim();
                            if (pInfo.Unit(0).SubUnit(0).MappingEQPPPID(strEQPPPID) == null)
                            {
                                pInfo.Unit(0).SubUnit(0).AddMappingEQPPPID(strEQPPPID);

                                pInfo.Unit(0).SubUnit(0).MappingEQPPPID(strEQPPPID).UP_EQPPPID = dr["UP_PPID"].ToString().Trim();
                                pInfo.Unit(0).SubUnit(0).MappingEQPPPID(strEQPPPID).LOW_EQPPPID = dr["LOW_PPID"].ToString().Trim();
                                pInfo.Unit(0).SubUnit(0).MappingEQPPPID(strEQPPPID).DateTime = dr["DTIME"].ToString().Trim();
                            }
                        }
                    }
                }
                else
                {
                    //pInfo.Unit(0).SubUnit(0).RemoveHOSTPPID();
                    dstrSQL = "SELECT * FROM tbHOSTPPID";
                    dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                    if (dDT != null)
                    {
                        pInfo.DeleteTable("HOSTPPID");
                        pInfo.AddDataTable("HOSTPPID", dDT);

                        foreach (DataRow dr in dDT.Rows)
                        {
                            pInfo.Unit(0).SubUnit(0).AddHOSTPPID(dr["HOSTPPID"].ToString().Trim());

                            pInfo.Unit(0).SubUnit(0).HOSTPPID(dr["HOSTPPID"].ToString().Trim()).EQPPPID = dr["EQPPPID"].ToString().Trim();
                            pInfo.Unit(0).SubUnit(0).HOSTPPID(dr["HOSTPPID"].ToString().Trim()).DateTime = dr["DTIME"].ToString().Trim();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

      
        #endregion

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            subInit();
        }

        private void subInit()
        {
            int dintIndex = 0;
            try
            {
                int dintSelectPage = tabControl1.SelectedIndex;
                if (dintSelectPage == 0)
                {
                    grdHostInfo.Rows.Clear();

                    foreach (string HOSTPPID in pInfo.Unit(0).SubUnit(0).HOSTPPID())
                    {
                        dintIndex++;
                        grdHostInfo.Rows.Add(dintIndex, HOSTPPID, pInfo.Unit(0).SubUnit(0).HOSTPPID(HOSTPPID).EQPPPID);
                    }
                }
                else
                {
                    //grdEQPInfo.Rows.Clear();

                    //foreach (string EQPPPID in pInfo.Unit(0).SubUnit(0).MappingEQPPPID())
                    //{
                    //    dintIndex++;
                    //    grdEQPInfo.Rows.Add(dintIndex, EQPPPID, pInfo.Unit(0).SubUnit(0).MappingEQPPPID(EQPPPID).UP_EQPPPID, pInfo.Unit(0).SubUnit(0).MappingEQPPPID(EQPPPID).LOW_EQPPPID);
                    //}
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            subInit();

        }

        private void grdHostInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (grdHostInfo.SelectedRows[0] != null)
            //    {
            //        txtHost_HOSTPPID.Text = grdHostInfo.SelectedRows[0].Cells[1].Value.ToString();
            //        txtHOST_MappingEQPPPID.Text = grdHostInfo.SelectedRows[0].Cells[2].Value.ToString();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            //}
        }

        private void grdEQPInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (grdEQPInfo.SelectedRows[0] != null)
            //    {
            //        this.txtEQP_EQPPPID.Text = grdEQPInfo.SelectedRows[0].Cells[1].Value.ToString();
            //        this.txtEQP_UP_EQPPPID.Text = grdEQPInfo.SelectedRows[0].Cells[2].Value.ToString();
            //        this.txtEQP_LOW_EQPPPID.Text = grdEQPInfo.SelectedRows[0].Cells[3].Value.ToString();

            //    }
            //}
            //catch (Exception ex)
            //{
            //    pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            //}
        }

        private void txtHost_HOSTPPID_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (pfrmPPIDList != null) pfrmPPIDList.Close();
                pfrmPPIDList = new frmPPIDList();
                pfrmPPIDList.ppidselect += new frmPPIDList.PPIDSelect(pfrmPPIDList_ppidselect);
                pfrmPPIDList.subFormLoad("HOST", 2);

                if (pfrmPPIDList.ShowDialog() == DialogResult.Yes)
                {
                    subDisplayChange("Host_HostPPID");
                }
                else
                {
                    strSelectPPIDName = "";
                }

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subDisplayChange(string strType)
        {
            try
            {
                switch(strType)
                {
                    case "Host_HostPPID":
                        //for (int dintLoop = 0; dintLoop < grdHostInfo.Rows.Count; dintLoop++)
                        //{
                        //    if (grdHostInfo[1, dintLoop].Value.ToString() == strSelectPPIDName)
                        //    {
                        //        grdHostInfo.Rows[dintLoop].Selected = true;
                        //        txtHost_HOSTPPID.Text = grdHostInfo.Rows[dintLoop].Cells[1].Value.ToString();
                        //        txtHOST_MappingEQPPPID.Text = grdHostInfo.Rows[dintLoop].Cells[2].Value.ToString();
                        //        break;
                        //    }
                        //}
                        break;

                    case "Host_EQPPPID":
                        for (int dintLoop = 0; dintLoop < grdHostInfo.Rows.Count; dintLoop++)
                        {
                            if (grdHostInfo[1, dintLoop].Value.ToString() == strSelectPPIDName)
                            {
                                grdHostInfo.Rows[dintLoop].Selected = true;
                                txtHost_HOSTPPID.Text = grdHostInfo.Rows[dintLoop].Cells[1].Value.ToString();
                                txtHOST_MappingEQPPPID.Text = strSelectSubPPIDName;
                                break;
                            }
                        }
                        break;
                }

                
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void pfrmPPIDList_ppidselect(string strType, int dintType, string strPPIDName)
        {
            try
            {
                if (strType == "HOST")
                {
                    if (dintType == 1)
                    {
                        strSelectSubPPIDName = strPPIDName;
                    }
                    else
                    {
                        strSelectPPIDName = strPPIDName;
                        strSelectSubPPIDName = "";
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void txtHOST_MappingEQPPPID_DoubleClick(object sender, EventArgs e)
        {
            //try
            //{
            //    if (string.IsNullOrEmpty(txtHost_HOSTPPID.Text) == false)
            //    {
            //        strSelectPPIDName = txtHost_HOSTPPID.Text;

            //        if (pfrmPPIDList != null) pfrmPPIDList.Close();
            //        pfrmPPIDList = new frmPPIDList();
            //        pfrmPPIDList.ppidselect += new frmPPIDList.PPIDSelect(pfrmPPIDList_ppidselect);
            //        pfrmPPIDList.subFormLoad("HOST", 1);

            //        if (pfrmPPIDList.ShowDialog() == DialogResult.Yes)
            //        {
            //            subDisplayChange("Host_EQPPPID");
            //        }
            //        else
            //        {
            //            strSelectPPIDName = "";
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            //}
        }

        private void grdHostInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void grdEQPInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdHostInfo_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdHostInfo.SelectedRows.Count > 0)
                {
                    txtHost_HOSTPPID.Text = grdHostInfo.SelectedRows[0].Cells[1].Value.ToString();
                    txtHOST_MappingEQPPPID.Text = grdHostInfo.SelectedRows[0].Cells[2].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
       
        private void btnCreate_Hostppid_Click(object sender, EventArgs e)
        {
            try
            {
                if (pfrmHostPPIDCreate != null) pfrmHostPPIDCreate.Close();

                pfrmHostPPIDCreate = new frmHostPPIDCreate();

                if (pfrmHostPPIDCreate.ShowDialog() == DialogResult.Yes)
                {
                    subInit();
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnModify_Hostppid_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtHost_HOSTPPID.Text) == false)
                {
                    if (pfrmHostPPIDModify != null) pfrmHostPPIDModify.Close();
                    pfrmHostPPIDModify = new frmHostPPIDModify();
                    pfrmHostPPIDModify.subFormLoad(txtHost_HOSTPPID.Text);
                    if (pfrmHostPPIDModify.ShowDialog() == DialogResult.Yes)
                    {
                        subInit();
                    }
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnDelete_Hostppid_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtHost_HOSTPPID.Text) == false)
                {
                    string dstrSQL = string.Format("Delete FROM `tbHOSTPPID` Where HOSTPPID='{0}';", txtHost_HOSTPPID.Text);
                    clsDBAct.funExecuteQuery(dstrSQL);

                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport, "2", "2", txtHost_HOSTPPID.Text, txtHOST_MappingEQPPPID.Text);
                    pInfo.Unit(0).SubUnit(0).RemoveHOSTPPID(txtHost_HOSTPPID.Text);

                    subInit();                    
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
       
        private void tabControl1_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.Visible)
                {
                    subInit();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}


