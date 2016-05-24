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
    public partial class tabfrmInfoSEM: UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;
        private int dintRowCountOld = 0;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmInfoSEM()
        {
            InitializeComponent();
            this.tmrUpdateSEM.Enabled = true;
            funSetDoubleBuffered(this.dataGridView1);
        }
       
        #endregion

        #region Methods
        public static void funSetDoubleBuffered(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                                          BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                                           null, control, new object[] { true });
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

        private void tabfrmSEM_Load(object sender, EventArgs e)
        {
            subUpdate();
            tmrUpdateSEM.Enabled = true;
        }

        private void subUpdate()
        {
            if (pInfo != null)
            {
                dataGridView1.Rows.Clear();
                foreach (int SVID in this.pInfo.Unit(0).SubUnit(0).SVID())
                {
                    clsSVID currentSVID = this.pInfo.Unit(0).SubUnit(0).SVID(SVID);
                    dataGridView1.Rows.Add(currentSVID.SVID, currentSVID.Name, "");
                }
            }
        }
        
        #endregion

        private void tmrUpdateSEM_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrUpdateSEM.Enabled = false;

                if (dintRowCountOld != pInfo.Unit(0).SubUnit(0).SVIDCount)
                {
                    subUpdate();
                    dintRowCountOld = pInfo.Unit(0).SubUnit(0).SVIDCount;
                }

                for (int dintLoop = 0; dintLoop < dataGridView1.Rows.Count - 1; dintLoop++)
                {
                    int dintSVID = Convert.ToInt32(dataGridView1.Rows[dintLoop].Cells[0].Value.ToString());
                    dataGridView1.Rows[dintLoop].Cells["VALUE"].Value = this.pInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value;
                }

                dataGridView1.Sort(new RowComparer(SortOrder.Ascending));

            }
            catch (Exception ex)
            {

                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                this.tmrUpdateSEM.Enabled = true;
            }
        }

        public void subClose()
        {
            tmrUpdateSEM.Enabled = false;
        }
       
        
    }


}
