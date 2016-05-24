using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InfoAct;

namespace DisplayAct
{
    public partial class subfrmFind : Form
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;

        private string pstrHglassID = string.Empty;

        #endregion



        #region Constructor
        //public subfrmFind()
        //{
        //    InitializeComponent();
        //}

        //public subfrmFind(APCgridType xpcType)
        //{
        //    InitializeComponent();
        //    this.xPCgrid.FormType = xpcType;
        //}

        public subfrmFind(APCgridType xpcType, string strHglassID)
        {
            InitializeComponent();
            this.xPCgrid.FormType = xpcType;
            this.pstrHglassID = strHglassID;
            this.Text = string.Format("{0} - {1} Find", this.pstrHglassID, this.xPCgrid.FormType.ToString());
            this.Tag = strHglassID;
            
        }
        #endregion


        #region Properties
        public string Caption
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                this.pstrHglassID = value;
                this.Text = string.Format("{0} - {1} Find", this.pstrHglassID, this.xPCgrid.FormType.ToString());
            }
        }

        public APCgridType FormType
        {
            get
            {
                return this.xPCgrid.FormType;
            }
            set
            {
                this.xPCgrid.FormType = value;
            }
        }
        #endregion


        #region Methods
        public void subFormLoad()
        {
            try
            {
                // 데이터 받아와서 바인딩까지 해야지...
                this.xPCgrid.FindEnable = true;


                subSetGridData();

                this.Show();
                this.Activate();
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }


        private void subSetGridData()
        {
            try
            {
                string strSQL = string.Empty;

                switch (this.xPCgrid.FormType)
                {
                    case APCgridType.APC:
                        strSQL = string.Format("SELECT tbAPC.SET_TIME, tbAPC.H_GLASSID, tbAPC.JOBID, tbAPC.RECIPE, tbAPC_Sub.P_PARM_NAME, tbAPC_Sub.P_PARM_VALUE, IIF(tbAPC.[APC_STATE]='1', 'Waiting', IIF(tbAPC.[APC_STATE]= '2', 'Running', IIF(tbAPC.[APC_STATE]='3', 'Done', '9'))) AS APC_STATE, tbAPC.Operation, tbAPC_Sub.isCenter FROM tbAPC INNER JOIN tbAPC_Sub ON tbAPC.[H_GLASSID] = tbAPC_Sub.[H_GLASSID] WHERE tbAPC.[H_GLASSID]='{0}'ORDER BY tbAPC.SET_TIME, tbAPC_Sub.P_PARM_NAME ASC;", this.pstrHglassID);
                        break;
                    case APCgridType.PPC:
                        strSQL = string.Format("SELECT tbPPC.SET_TIME, tbPPC.H_GLASSID, tbPPC.JOBID, tbPPC_Sub.P_MODULEID, tbPPC_Sub.P_ORDER, IIF(tbPPC_Sub.[P_STATE] ='1', 'Waiting', IIF(tbPPC_Sub.[P_STATE] = '2', 'Running', IIF(tbPPC_Sub.[P_STATE] = '3', 'Done', '9'))) AS P_STATE, tbPPC.Operation, tbPPC_Sub.isCenter, tbPPC.isRun FROM tbPPC INNER JOIN tbPPC_Sub ON tbPPC.[H_GLASSID] = tbPPC_Sub.[H_GLASSID] WHERE tbPPC.[H_GLASSID]='{0}' ORDER BY tbPPC.SET_TIME, tbPPC_Sub.P_ORDER ASC;", this.pstrHglassID);
                        break;
                    case APCgridType.RPC:
                        strSQL = string.Format("SELECT tbRPC.SET_TIME, tbRPC.H_GLASSID, tbRPC.JOBID, tbRPC.RPC_PPID, tbRPC.ORIGINAL_PPID, IIF(tbRPC.[RPC_STATE] ='1', 'Waiting', IIF(tbRPC.[RPC_STATE] = '2', 'Running', IIF(tbRPC.[RPC_STATE] = '3', 'Done', '9'))) AS RPC_STATE, tbRPC.Operation FROM tbRPC WHERE tbRPC.[H_GLASSID]='{0}' ORDER BY tbRPC.SET_TIME ASC;", this.pstrHglassID);
                        break;
                }

                if (!string.IsNullOrEmpty(strSQL))
                {
                    DataTable dt = DBAct.clsDBAct.funSelectQuery(strSQL);

                    this.xPCgrid.DataSource = dt;
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subInitGrid()
        {
            try
            {
                this.xPCgrid.FindEnable = true;
                //this.xPCgrid.OnDelete += new APCgrid.APCgridDeleteEventHandler(xPCgrid_OnDelete);
                
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }


        #endregion
    }
}
