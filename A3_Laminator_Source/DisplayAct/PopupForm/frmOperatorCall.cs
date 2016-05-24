using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonAct;
using InfoAct;

namespace DisplayAct
{
    public partial class frmOperatorCall : Form
    {
        private clsInfo PInfo = clsInfo.Instance;

        public frmOperatorCall()
        {
            InitializeComponent();
        }

        //*******************************************************************************
        //  Function Name : subFormLoad(int intPort)
        //  Description   : Form Load시 필요한 작업을 수행한다
        //  Parameters    : intPort - Port Number
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/20          구 정환         [L 00] 
        //*******************************************************************************
        public void subFormLoad(int intPort)
        {
            try
            {
                //Invoke(new MethodInvoker(delegate()
                //    {
                subFormInitial(intPort);
                subComboBoxDisplay(intPort);            //PortID, Main Recipe 데이타를 Combo에 추가한다 => Century는 Port에 Recipe가 없음!
                subFormDataInitial(intPort);            //Form Data를 Initial한다
                subSlotDisplay(intPort);
                //}));


                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "frmOperatorCall(subFormLoad)" + ", PortID : " + intPort);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intPort:" + intPort);
            }
        }

        //*******************************************************************************
        //  Function Name : subFormInitial(int intPort)
        //  Description   : Form의 Control을 Initial한다.
        //  Parameters    : intPort - Port Number
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/12          김 효주         [L 00] 
        //*******************************************************************************
        private void subFormInitial(int intPort)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intPort:" + intPort);
            }
        }

        /// <summary>
        /// Lot Recipe ComboBox의 List를 추가한다.
        /// </summary>
        /// <param name="intPort">Port Number</param>
        private void subComboBoxDisplay(int intPort)
        {
            //string dstrRecipeID = "";
            //int dintLoop = 0;

            try
            {
                //this.Invoke(new MethodInvoker(delegate()
                //{
                cboRecipeID.Items.Clear();

                foreach (string dstrName in this.PInfo.Unit(0).SubUnit(0).HOSTPPID())    //Main RecipeID를 등록한다.
                {
                    this.cboRecipeID.Items.Add(dstrName);
                    this.cboRecipeID.Text = dstrName;
                }

                this.cboRecipeID.SelectedIndex = 0;
                //}));
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Form의 Data를 Initial한다.
        /// </summary>
        /// <param name="intPort">Port Number</param>
        private void subFormDataInitial(int intPort)
        {
            string dstrLOTID = "";
            Random drandObj = new Random();     //Offline일 경우 고유한 LOTID를 자동으로 생성하기 위해 난수 발생

            try
            {
               
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intPortID:" + intPort);
            }
        }

        //*******************************************************************************
        //  Function Name : subSlotDisplay()
        //  Description   : dataGridView에 데이터를 넣어준다
        //  Parameters    : intPort - Port Number
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/23          어 경태         [L 00] 
        //  2008/04/26          구 정환         [L 01] 장비에 등록된 레시피만 선택가능하도록
        //*******************************************************************************
        private void subSlotDisplay(int intPort)
        {
            try
            {

                this.CenterToScreen();
                this.Show();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString() + ", intPortID:" + intPort);
            }
        }

        private void btnLOTCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnLOTStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnAllCancel_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }



        private void txtLOTID_TextChanged(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void cboRecipeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }

        }

        private void txtPRODID_TextChanged(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void txtGLSID_TextChanged(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void grdLotInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }

        }

        private void btnLOTAbort_Click(object sender, EventArgs e)
        {
            try
            {
                
                this.Hide();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void grdLotInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

       
    }
}