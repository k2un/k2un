using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using System.Reflection;

namespace DisplayAct
{
    public partial class tabfrmInfoAPC : UserControl
    {
         #region Fields
        private clsInfo pInfo = clsInfo.Instance;

        //Merge시 사용할 변수
        DataSet dataset = new DataSet();
        private List<string> MergedRowsInFirstColumn = new List<string>();
        private int intRowHeight = 0;

       public Ngrid apcGrid = new Ngrid(NgridType.APC);
        DateTime dtNow = new DateTime();
        
        //APC_Grid apcGrid;

        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmInfoAPC()
        {
            InitializeComponent();
            apcGrid.OnDelete += new NgridDeleteEventHandler(apcGrid_OnDelete);
        }

        
      
        #endregion

        #region Methods
        void apcGrid_OnDelete(NgridDeleteEventArgs ar)
        {
            string dstrLogData = "";
            try
            {
                pInfo.All.APCDataDel = true;
                //if (this.pInfo.ProcessDataDel(clsInfo.ProcessDataType.APC, "2", ar.GlassID,true))
                pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataDel, clsInfo.ProcessDataType.APC, "2", "2!"+ ar.GlassID, true);

                // APC Log 작성
                //dstrLogData += "APC Data 삭제!! => ";
                //dstrLogData += "GLASSID : " + ar.GlassID;

                //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.APC, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dstrLogData);
                // 2012 12 07 조영훈 , 전달인자 형 변경
                //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.APC, DateTime.Now, dstrLogData);

                this.pInfo.All.APCDBUpdateCheck = true;
                    
              
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private class RowComparer : System.Collections.IComparer
        {
            private static int sortOrderModifier = 1;

            public RowComparer(SortOrder sortOrder)
            {
                if (sortOrder == SortOrder.Descending)
                {
                    sortOrderModifier = -1;
                }
                else if (sortOrder == SortOrder.Ascending)
                {
                    sortOrderModifier = 1;
                }
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow DataGridViewRow1 = (DataGridViewRow)x;
                DataGridViewRow DataGridViewRow2 = (DataGridViewRow)y;

                // Try to sort based on the Last Name column.
                int CompareResult = 0;
                if (Convert.ToInt32(DataGridViewRow1.Cells[0].Value.ToString()) > Convert.ToInt32(DataGridViewRow2.Cells[0].Value.ToString()))
                {
                    CompareResult = 1;
                }
                else
                {
                    CompareResult = -1;
                }

                return CompareResult * sortOrderModifier;
            }
        }

        private void tabfrmAPC_Load(object sender, EventArgs e)
        {
            string strSQL = "";
            try
            {              
                this.apcGrid.OnSort += new NgridSortEventHandler(apCgrid1_OnSort);
                strSQL = "SELECT tbAPC.SET_TIME, tbAPC.H_GLASSID, tbAPC.JOBID, tbAPC.RECIPE, tbAPC_Sub.P_PARM_NAME, tbAPC_Sub.P_PARM_VALUE, IIF(tbAPC.[APC_STATE]='1', 'Waiting', IIF(tbAPC.[APC_STATE]= '2', 'Running', IIF(tbAPC.[APC_STATE]='3', 'Done', '9'))) AS APC_STATE, tbAPC.Operation, tbAPC_Sub.isCenter FROM tbAPC INNER JOIN tbAPC_Sub ON tbAPC.[H_GLASSID] = tbAPC_Sub.[H_GLASSID] ORDER BY tbAPC.SET_TIME, tbAPC_Sub.P_PARM_NAME ASC;"; //, tbAPC_Sub.P_PARM_NAME
                DataTable dDataTable = DBAct.clsDBAct.funSelectQuery(strSQL);
                apcGrid.DataSource = dDataTable;
                panel1.Controls.Add(apcGrid);

                apcGrid.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void apCgrid1_OnSort(NgridSortEventArgs ar)
        {
            string strSQL = string.Empty;

            if (ar.TimeSort) strSQL = "SELECT tbAPC.SET_TIME, tbAPC.H_GLASSID, tbAPC.JOBID, tbAPC.RECIPE, tbAPC_Sub.P_PARM_NAME, tbAPC_Sub.P_PARM_VALUE, IIF(tbAPC.[APC_STATE]='1', 'Waiting', IIF(tbAPC.[APC_STATE]= '2', 'Running', IIF(tbAPC.[APC_STATE]='3', 'Done', '9'))) AS APC_STATE, tbAPC.Operation, tbAPC_Sub.isCenter FROM tbAPC INNER JOIN tbAPC_Sub ON tbAPC.[H_GLASSID] = tbAPC_Sub.[H_GLASSID] ORDER BY tbAPC.SET_TIME, tbAPC_Sub.P_PARM_NAME ASC;"; //, tbAPC_Sub.P_PARM_NAME
            else strSQL = "SELECT tbAPC.SET_TIME, tbAPC.H_GLASSID, tbAPC.JOBID, tbAPC.RECIPE, tbAPC_Sub.P_PARM_NAME, tbAPC_Sub.P_PARM_VALUE, IIF(tbAPC.[APC_STATE]='1', 'Waiting', IIF(tbAPC.[APC_STATE]= '2', 'Running', IIF(tbAPC.[APC_STATE]='3', 'Done', '9'))) AS APC_STATE, tbAPC.Operation, tbAPC_Sub.isCenter FROM tbAPC INNER JOIN tbAPC_Sub ON tbAPC.[H_GLASSID] = tbAPC_Sub.[H_GLASSID] ORDER BY tbAPC.APC_STATE DESC, tbAPC.SET_TIME, tbAPC_Sub.P_PARM_NAME ASC;"; //, tbAPC_Sub.P_PARM_NAME

            DataTable dDataTable = DBAct.clsDBAct.funSelectQuery(strSQL);

            apcGrid.DataSource = dDataTable;
        }


        private void gridAPC_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Height = intRowHeight;
        }

        #endregion

    }
}
