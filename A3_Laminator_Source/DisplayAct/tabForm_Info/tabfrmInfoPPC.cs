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
    public partial class tabfrmInfoPPC : UserControl
    {
         #region Fields
        private clsInfo pInfo = clsInfo.Instance;

        //Merge시 사용할 변수
        DataSet dataset = new DataSet();
        private List<string> MergedRowsInFirstColumn = new List<string>();
        private int intRowHeight = 0;

        public Ngrid ppcGrid = new Ngrid(NgridType.PPC);
        DateTime dtNow = new DateTime();
        

        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmInfoPPC()
        {
            InitializeComponent();
            ppcGrid.OnDelete +=new NgridDeleteEventHandler(ppcGrid_OnDelete);
        }

        
      
        #endregion

        #region Methods
        void ppcGrid_OnDelete(NgridDeleteEventArgs ar)
        {
            string dstrLogData = "";
            try
            {
                pInfo.All.PPCDataDel = true;
                pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataDel, clsInfo.ProcessDataType.PPC, "2", ar.GlassID, true);

                // APC Log 작성
                //dstrLogData += "PPC Data 삭제!! => ";
                //dstrLogData += "GLASSID : " + ar.GlassID;

                //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.APC, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dstrLogData);
                // 2012 12 07 조영훈 , 전달인자 형 변경
                //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.PPC, DateTime.Now, dstrLogData);
                //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.PPC, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dstrLogData);

                this.pInfo.All.PPCDBUpdateCheck = true;
                
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
                this.ppcGrid.OnSort += new NgridSortEventHandler(apCgrid1_OnSort);
                strSQL = "SELECT tbPPC.SET_TIME, tbPPC.H_GLASSID, tbPPC.JOBID, tbPPC_Sub.P_MODULEID, tbPPC_Sub.P_ORDER, IIF(tbPPC_Sub.[P_STATE] ='1', 'Waiting', IIF(tbPPC_Sub.[P_STATE] = '2', 'Running', IIF(tbPPC_Sub.[P_STATE] = '3', 'Done', '9'))) AS P_STATE, tbPPC.Operation, tbPPC_Sub.isCenter, tbPPC.isRun FROM tbPPC INNER JOIN tbPPC_Sub ON tbPPC.[H_GLASSID] = tbPPC_Sub.[H_GLASSID] ORDER BY tbPPC.SET_TIME, tbPPC_Sub.P_ORDER ASC;";
                DataTable dDataTable = DBAct.clsDBAct.funSelectQuery(strSQL);
                ppcGrid.DataSource = dDataTable;
                panel1.Controls.Add(ppcGrid);

                ppcGrid.Dock = DockStyle.Fill;

             }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void apCgrid1_OnSort(NgridSortEventArgs ar)
        {
            string strSQL = string.Empty;

            if (ar.TimeSort) strSQL = "SELECT tbPPC.SET_TIME, tbPPC.H_GLASSID, tbPPC.JOBID, tbPPC_Sub.P_MODULEID, tbPPC_Sub.P_ORDER, IIF(tbPPC_Sub.[P_STATE] ='1', 'Waiting', IIF(tbPPC_Sub.[P_STATE] = '2', 'Running', IIF(tbPPC_Sub.[P_STATE] = '3', 'Done', '9'))) AS P_STATE, tbPPC.Operation, tbPPC_Sub.isCenter, tbPPC.isRun FROM tbPPC INNER JOIN tbPPC_Sub ON tbPPC.[H_GLASSID] = tbPPC_Sub.[H_GLASSID] ORDER BY tbPPC.SET_TIME, tbPPC_Sub.P_ORDER ASC;";
            else strSQL = "SELECT tbPPC.SET_TIME, tbPPC.H_GLASSID, tbPPC.JOBID, tbPPC_Sub.P_MODULEID, tbPPC_Sub.P_ORDER, IIF(tbPPC_Sub.[P_STATE] ='1', 'Waiting', IIF(tbPPC_Sub.[P_STATE] = '2', 'Running', IIF(tbPPC_Sub.[P_STATE] = '3', 'Done', '9'))) AS P_STATE, tbPPC.Operation, tbPPC_Sub.isCenter, tbPPC.isRun FROM tbPPC INNER JOIN tbPPC_Sub ON tbPPC.[H_GLASSID] = tbPPC_Sub.[H_GLASSID] ORDER BY tbPPC.isRun, tbPPC.SET_TIME, tbPPC_Sub.P_ORDER ASC;";

            DataTable dDataTable = DBAct.clsDBAct.funSelectQuery(strSQL);

            ppcGrid.DataSource = dDataTable;
        }


        private void gridAPC_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Height = intRowHeight;
        }

        #endregion

    }
}
