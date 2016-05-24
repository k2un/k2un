using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using System.Threading;

namespace DisplayAct
{
    public partial class frmPPIDList : Form
    {
        public clsInfo PInfo = clsInfo.Instance;
        public delegate void PPIDSelect(string strType, int intType, string strPPIDName);
        public event PPIDSelect ppidselect;
        public int dType = 0;
        public string dstrType = "";

        public frmPPIDList()
        {
            InitializeComponent();
        }

        #region "Form Initial"
        public void subFormLoad(string strType, int dintType)
        {

            try
            {
                dstrType = strType;
                dType = dintType;
                subFormInitial(dintType);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subFormInitial(int dintType)
        {
            try
            {
                //PInfo.All.SelectPPIDType = dintType;
                grdPPIDList.Rows.Clear();
                if (dintType == 1)
                {
                    foreach (string EQPPPPID in PInfo.Unit(0).SubUnit(0).EQPPPID())
                    {
                        grdPPIDList.Rows.Add(EQPPPPID);
                    }
                }
                else if (dintType == 2)
                {
                    foreach (string HostPPID in PInfo.Unit(0).SubUnit(0).HOSTPPID())
                    {
                        grdPPIDList.Rows.Add(HostPPID);
                    }
                }
                else
                {
                    PInfo.All.isReceivedFromCIM = true;
                    PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID, 1);
                    subWaitDuringReadFromPLC();
                    PInfo.All.isReceivedFromCIM = false;

                    foreach (string EQPPPPID in PInfo.Unit(0).SubUnit(0).EQPPPID())
                    {
                        grdPPIDList.Rows.Add(EQPPPPID);
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }   
        }
        #endregion


        public void subWaitDuringReadFromPLC()
        {
            long dlngTimeTick = 0;
            long dlngSec = 0;
            int dintTimeOut = 40;   //TimeOut은 20초로 함

            try
            {
                this.PInfo.All.PLCActionEnd = false;        //초기화
                dlngTimeTick = DateTime.Now.Ticks;


                while (this.PInfo.All.PLCActionEnd == false)
                {
                    dlngSec = DateTime.Now.Ticks - dlngTimeTick;
                    if (dlngSec < 0) dlngTimeTick = 0;
                    if (dlngSec > dintTimeOut * 10000000 || this.PInfo.All.PLCActionEnd == true)
                    {
                        this.PInfo.All.PLCActionEnd = false;        //초기화
                        return;
                    }
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void grdPPIDList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DialogResult = System.Windows.Forms.DialogResult.Yes;
                    if (ppidselect != null) ppidselect(dstrType, dType, grdPPIDList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    this.Hide();
                }

            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void frmPPIDList_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = System.Windows.Forms.DialogResult.Yes;
                if (ppidselect != null) ppidselect(dstrType, dType, grdPPIDList.SelectedRows[0].Cells[0].Value.ToString());
                this.Hide();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Hide();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}