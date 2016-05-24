using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using System.Reflection;

namespace DisplayAct
{
    public partial class tabfrmInfoMultiData: UserControl
    {
        #region Fields
        public clsInfo pInfo = clsInfo.Instance;
        private int dintRowCountOld = 0;
        private bool pblNoSort = true;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmInfoMultiData()
        {
            InitializeComponent();
            this.tmrUpdate.Enabled = true;
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
            private static int RowIndex = 0;


            public RowComparer(SortOrder sortOrder, int rowIndex)
            {
                RowIndex = rowIndex;

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



                //if (Convert.ToInt32(DataGridViewRow1.Cells[RowIndex].Value.ToString()) > Convert.ToInt32(DataGridViewRow2.Cells[RowIndex].Value.ToString()))
                //{
                //    CompareResult = 1;
                //}
                //else
                //{
                //    CompareResult = -1;
                //}

                CompareResult = DataGridViewRow1.Cells[RowIndex].Value.ToString().CompareTo(DataGridViewRow2.Cells[RowIndex].Value.ToString());

                if (CompareResult == 0) CompareResult = DataGridViewRow1.Cells[0].Value.ToString().CompareTo(DataGridViewRow2.Cells[0].Value.ToString());

                return CompareResult * sortOrderModifier;
            }
        }

        private void tabfrmSEM_Load(object sender, EventArgs e)
        {
            subUpdate();
            tmrUpdate.Enabled = true;
        }

        private void subUpdate()
        {
            if (pInfo != null)
            {
                dataGridView1.Rows.Clear();

                foreach (InfoAct.clsMultiUseDataByTYPE clsTYPE in this.pInfo.Unit(0).SubUnit(0).MultiData.TYPES_VALUE())
                {
                    foreach (InfoAct.clsMultiUseDataByITEM clsITEM in clsTYPE.ITEMS_VALUE())
                    {
                        dataGridView1.Rows.Add(clsITEM.INDEX, clsITEM.DATA_TYPE, clsITEM.ITEM, clsITEM.VALUE, clsITEM.REFERENCE);
                    }
                }

                dataGridView1.Sort(new RowComparer(SortOrder.Ascending, (this.pblNoSort) ? 0 : 1));
            }
        }
        
        #endregion

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrUpdate.Enabled = false;

                if (this.Visible)
                {
                    if (dataGridView1.Rows.Count != this.pInfo.Unit(0).SubUnit(0).MultiData.ITEM_COUNT) Update();


                    for (int dintLoop = 0; dintLoop < dataGridView1.Rows.Count; dintLoop++)
                    {
                        string dstrTYPE = dataGridView1.Rows[dintLoop].Cells["DATATYPE"].Value.ToString().Trim();
                        string dstrITEM = dataGridView1.Rows[dintLoop].Cells["ITEMNAME"].Value.ToString().Trim();
                        dataGridView1.Rows[dintLoop].Cells["ITEMVALUE"].Value = this.pInfo.Unit(0).SubUnit(0).MultiData.TYPES(dstrTYPE).ITEMS(dstrITEM).VALUE;
                    }
                }
            }
            catch (Exception ex)
            {

                this.pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                this.tmrUpdate.Enabled = true;
            }
        }

        public void subClose()
        {
            tmrUpdate.Enabled = false;
        }

        private void cbSort_Click(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (cb.Text.Equals("No"))
            {
                this.cbSort2.Checked = false;

                if (this.pblNoSort)
                {
                    this.cbSort1.Checked = true;
                    return;
                }
                else
                {
                    this.pblNoSort = true;
                }
            }
            else
            {
                this.cbSort1.Checked = false;

                if (!this.pblNoSort)
                {
                    this.cbSort2.Checked = true;
                    return;
                }
                else
                {
                    this.pblNoSort = false;
                }
            }

            dataGridView1.Sort(new RowComparer(SortOrder.Ascending, (this.pblNoSort) ? 0 : 1));
        }

       
        
    }


}
