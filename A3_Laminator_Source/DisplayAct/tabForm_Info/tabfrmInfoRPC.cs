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
    public partial class tabfrmInfoRPC : UserControl
    {
         #region Fields
        private clsInfo pInfo = clsInfo.Instance;

        //Merge시 사용할 변수
        DataSet dataset = new DataSet();
        private List<string> MergedRowsInFirstColumn = new List<string>();
        private int intRowHeight = 0;

        public Ngrid rpcGrid = new Ngrid(NgridType.RPC);
        DateTime dtNow = new DateTime();
        

        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmInfoRPC()
        {
            InitializeComponent();
            rpcGrid.OnDelete += new NgridDeleteEventHandler(rpcGrid_OnDelete);
        }

        
      
        #endregion

        #region Methods
        void rpcGrid_OnDelete(NgridDeleteEventArgs ar)
        {
            string dstrLogData = "";
            try
            {
                pInfo.All.RPCDataDel = true;

                pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataDel, clsInfo.ProcessDataType.RPC, "2", "2!" + ar.GlassID, true);
                // APC Log 작성
                //dstrLogData += "RPC Data 삭제!! => ";
                //dstrLogData += "GLASSID : " + ar.GlassID;

                //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.APC, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dstrLogData);
                // 2012 12 07 조영훈 = 전달 인자 형 변경 
                //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.RPC, DateTime.Now, dstrLogData);
                //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.PPC, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dstrLogData);

                this.pInfo.All.RPCDBUpdateCheck = true;

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

        private void tabfrmRPC_Load(object sender, EventArgs e)
        {
            string strSQL = "";
            try
            {
                this.rpcGrid.OnSort += new NgridSortEventHandler(rpCgrid1_OnSort);
                strSQL = "SELECT tbRPC.SET_TIME, tbRPC.H_GLASSID, tbRPC.JOBID, tbRPC.RPC_PPID, tbRPC.ORIGINAL_PPID, IIF(tbRPC.[RPC_STATE] ='1', 'Waiting', IIF(tbRPC.[RPC_STATE] = '2', 'Running', IIF(tbRPC.[RPC_STATE] = '3', 'Done', '9'))) AS RPC_STATE, tbRPC.Operation FROM tbRPC ORDER BY tbRPC.SET_TIME ASC;";
                DataTable dDataTable = DBAct.clsDBAct.funSelectQuery(strSQL);
                rpcGrid.DataSource = dDataTable;
                panel1.Controls.Add(rpcGrid);

                rpcGrid.Dock = DockStyle.Fill;

            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void rpCgrid1_OnSort(NgridSortEventArgs ar)
        {
            string strSQL = string.Empty;

            if (ar.TimeSort) strSQL = "SELECT tbRPC.SET_TIME, tbRPC.H_GLASSID, tbRPC.JOBID, tbRPC.RPC_PPID, tbRPC.ORIGINAL_PPID, IIF(tbRPC.[RPC_STATE] ='1', 'Waiting', IIF(tbRPC.[RPC_STATE] = '2', 'Running', IIF(tbRPC.[RPC_STATE] = '3', 'Done', '9'))) AS RPC_STATE, tbRPC.Operation FROM tbRPC ORDER BY tbRPC.SET_TIME ASC;";
            else strSQL = "SELECT tbRPC.SET_TIME, tbRPC.H_GLASSID, tbRPC.JOBID, tbRPC.RPC_PPID, tbRPC.ORIGINAL_PPID, IIF(tbRPC.[RPC_STATE] ='1', 'Waiting', IIF(tbRPC.[RPC_STATE] = '2', 'Running', IIF(tbRPC.[RPC_STATE] = '3', 'Done', '9'))) AS RPC_STATE, tbRPC.Operation FROM tbRPC ORDER BY tbRPC.RPC_STATE DESC, tbRPC.SET_TIME ASC;";

            DataTable dDataTable = DBAct.clsDBAct.funSelectQuery(strSQL);

            rpcGrid.DataSource = dDataTable;
        }


        private void gridAPC_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Height = intRowHeight;
        }

        #endregion

    }
}
