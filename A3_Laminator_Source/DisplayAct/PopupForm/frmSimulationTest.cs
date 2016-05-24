using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using CommonAct;
using System.IO;
using System.Collections;

namespace DisplayAct
{
    public partial class frmSimulationTest : Form
    {
        public clsInfo PInfo = clsInfo.Instance;
        string dstrSQL = "";
        DataTable dDT = new DataTable();
        int dintIndex = 0;
        int H_AlarmOccurCount = 0;

        public frmSimulationTest()
        {
            InitializeComponent();
            PInfo.All.SimulDummyPath = Application.StartupPath + @"\System\SimulationDummy.ini";
            initial();

        }

        private void initial()
        {
            try
            {
                tmrUpdate.Enabled = true;

                dataGridView1.Rows.Clear();
                for (int dintLoop = 0; dintLoop < 20; dintLoop++)
                {
                    dataGridView1.Rows.Add(("HOSTPPID" + (dintLoop + 1)).PadRight(20, ' '), dintLoop + 1);
                }

                dataGridView2.Rows.Clear();
                for (int dintLoop = 0; dintLoop < PInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                {
                    dataGridView2.Rows.Add(PInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Name, 0);
                }
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

        public void subClose()
        {
            this.tmrUpdate.Enabled = false;
            this.tmrUpdate.Dispose();
            this.Close();
            this.Dispose();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string strAddress = "W2A40";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W200E", dataGridView1.Rows.Count - 1, "D");
                for (int dintLoop = 0; dintLoop < dataGridView1.Rows.Count-1; dintLoop++)
                {
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, dataGridView1.Rows[dintLoop].Cells[0].Value.ToString().PadRight(20, ' '), "A");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, dataGridView1.Rows[dintLoop].Cells[1].Value.ToString(), "D");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //HostPPID 생성
        private void button5_Click(object sender, EventArgs e)
        {
            string strAddress = "W2040";
            try
            {
                if (string.IsNullOrEmpty(txthostppid.Text) != true && string.IsNullOrEmpty(txtEqpPPID.Text) != true)
                {
                    dataGridView1.Rows.Add(txthostppid.Text, txtEqpPPID.Text);

                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txthostppid.Text.PadRight(20, ' '), "A");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtEqpPPID.Text.ToString(), "D");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "2", "D");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B153A", "", "1", "");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //EQPPPID 생성
        private void button14_Click(object sender, EventArgs e)
        {
            string strAddress = "W2540";
            int dintIdex = 0;
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtEqpPPID2.Text, "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "1", "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                for (int dintLoop = 0; dintLoop < dataGridView2.Rows.Count - 1; dintLoop++)
                {
                    string strTemp = dataGridView2.Rows[dintLoop].Cells[1].Value.ToString();
                    strTemp = CommonAct.FunTypeConversion.funDecimalConvert(strTemp, EnuEQP.StringType.Hex);

                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, dataGridView2.Rows[dintLoop].Cells[1].Value.ToString(), "D2");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 2);
                }
                
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B113A", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        //HostPPID 수정
        private void button20_Click(object sender, EventArgs e)
        {
            string strAddress = "W2040";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txthostppid.Text.PadRight(20, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtEqpPPID.Text, "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "2", "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B153C", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            string strAddress = "W2040";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txthostppid.Text.PadRight(20, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtEqpPPID.Text, "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "2", "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1539", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }

        }

        //EQPPPID 수정
        private void button7_Click(object sender, EventArgs e)
        {
            string strAddress = "W2540";
            try
            {
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, " ".PadRight(20, ' '), "A");
                //strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtEqpPPID2.Text, "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "1", "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                for (int dintLoop = 0; dintLoop < dataGridView2.Rows.Count - 1; dintLoop++)
                {
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, dataGridView2.Rows[dintLoop].Cells[1].Value.ToString(), "D2");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 2);
                }

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B113B", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        //EQPPPID 삭제
        private void button15_Click(object sender, EventArgs e)
        {
            string strAddress = "W2540";
            try
            {
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, " ".PadRight(20, ' '), "A");
                //strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtEqpPPID2.Text, "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "1", "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                for (int dintLoop = 0; dintLoop < dataGridView2.Rows.Count - 1; dintLoop++)
                {
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, dataGridView2.Rows[dintLoop].Cells[1].Value.ToString(), "D2");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 2);
                }
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1538", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1139", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnST01_I_Click(object sender, EventArgs e)
        {
            string strAddress = "W2040";

            try
            {
                strAddress = "W2380";

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_GLSID.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_LOTID.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 2);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_SlotID.Text.Trim(), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 28);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(20, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_HostPPID.Text.Trim().PadRight(16, ' '), "A");

                //strAddress = "W2600";
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_LOTStart.Text, "D");
                //strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_LotEnd.Text, "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "3", "D");

                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1603", "", "0010", "");
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D3", "", "0010", "");

                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B160B", "", "1", "");
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16DB", "", "1", "");
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1164", "", "1", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16F3", "", "0010", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16FB", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B116E", "", "1", "");


                

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnST01_O_Click(object sender, EventArgs e)
        {
            string strAddress = "W21E0";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_GLSID.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(1, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_SlotID.Text.Trim().PadRight(16, ' '), "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtSlotID.Text, "D");
                

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "3", "D");


                if (PInfo.Unit(3).SubUnit(0).CurrGLSCount == 1)
                {
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1603", "", "1000", "");
                }
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D3", "", "1000", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B160B", "", "0", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16DB", "", "0", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1184", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnFI01_In_Click(object sender, EventArgs e)
        {
            string strAddress = "W2200";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtLOTID.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1168", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B161B", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnFI01_Out_Click(object sender, EventArgs e)
        {
            string strAddress = "W2210";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtLOTID.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1188", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B161B", "", "0", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnFI02_In_Click(object sender, EventArgs e)
        {
            string strAddress = "W2220";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtLOTID.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1169", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B162B", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnFI02_Out_Click(object sender, EventArgs e)
        {
            string strAddress = "W2230";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtLOTID.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1189", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B162B", "", "0", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnFT01_In_Click(object sender, EventArgs e)
        {
            string strAddress = "W22C0";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1673", "", "0010", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B167B", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1160", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnFT01_O_Click(object sender, EventArgs e)
        {
            string strAddress = "W22D0";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1673", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B167B", "", "0", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1180", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnAL01_In_Click(object sender, EventArgs e)
        {
            string strAddress = "W2300";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1693", "", "0010", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B169B", "", "1", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1161", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnAL01_O_Click(object sender, EventArgs e)
        {
            string strAddress = "W2310";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1693", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B169B", "", "0", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1181", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnLM01_In_Click(object sender, EventArgs e)
        {
            string strAddress = "W2320";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");
                
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A3", "", "0010", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16AB", "", "1", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1162", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnLM01_O_Click(object sender, EventArgs e)
        {
            string strAddress = "W2330";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");
                
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A3", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16AB", "", "0", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1182", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnDM01_In_Click(object sender, EventArgs e)
        {
            string strAddress = "W2340";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B3", "", "0010", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16BB", "", "1", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1163", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnDM01_O_Click(object sender, EventArgs e)
        {
            string strAddress = "W2350";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B3", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16BB", "", "0", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1183", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnIS01_In_Click(object sender, EventArgs e)
        {
            string strAddress = "W2360";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C3", "", "0010", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16CB", "", "1", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1165", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnIS01_O_Click(object sender, EventArgs e)
        {
            string strAddress = "W2370";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C3", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16CB", "", "0", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1185", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void btnFT02_In_Click(object sender, EventArgs e)
        {
            string strAddress = "W22E0";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1683", "", "0010", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B168B", "", "1", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1166", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnFT02_O_Click(object sender, EventArgs e)
        {
            string strAddress = "W22F0";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox4.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, textBox3.Text, "D");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2012", "1", "D");


                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1683", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B168B", "", "0", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1186", "", "1", "");
            }
            catch (Exception)
            {
            }
        }

        private void btnGLSScrap_Click(object sender, EventArgs e)
        {
            subScrapUnScrapCMD("G_SCRAP", txtGlassScrap_LOTID.Text, txtGlassScrap_SlotID.Text, txtGlassScrap_UnitID.Text);
        }

       
        private void btnFilmScrap_Click(object sender, EventArgs e)
        {
            subScrapUnScrapCMD("F_SCRAP", txtFilmScrap_LOTID.Text, txtFilmScrap_SlotID.Text, txtFilmScrap_UnitID.Text);
        }

        private void btnFilmUnScrap_Click(object sender, EventArgs e)
        {
            subScrapUnScrapCMD("F_UNSCRAP", txtFilmScrap_LOTID.Text, txtFilmScrap_SlotID.Text, txtFilmScrap_UnitID.Text);
        }

        private void btnGLSUnScrap_Click(object sender, EventArgs e)
        {
            subScrapUnScrapCMD("G_UNSCRAP", txtGlassScrap_LOTID.Text, txtGlassScrap_SlotID.Text, txtGlassScrap_UnitID.Text);
        }

        private void subScrapUnScrapCMD(string strType, string strLotID, string strSlotID, string strUnitID)
        {
            string strAddress = "";
            try
            {
                string[] arrCon = strType.Split('_');

                if (arrCon[1] == "SCRAP")
                {
                    strAddress = "W2020";
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strLotID.PadRight(16, ' '), "A");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strSlotID, "A");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strUnitID, "D");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                    
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B110B", "", "1", "");
                }
                else
                {
                    strAddress = "W2030";
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strLotID.PadRight(16, ' '), "A");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strSlotID, "A");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strUnitID, "D");
                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B110C", "", "1", "");
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void btnProcessDataCheck_Click(object sender, EventArgs e)
        {
            string strAddress = "W2400";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_GLSID.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_LOTID.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 2);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_SlotID.Text.Trim(), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 28);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(20, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_HostPPID.Text.Trim().PadRight(16, ' '), "A");

                strAddress = "W2600";
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_LOTStart.Text, "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_LotEnd.Text, "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1519", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnEQPPPID_ListUpdate_Click(object sender, EventArgs e)
        {
            string strAddress = "W2A00";
            string strData = "";
            try
            {

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, '0'), "D");

                strData = CommonAct.FunTypeConversion.funBinConvert(this.txtEQPPPIDMap01.Text.Trim().PadRight(16, ' '), EnuEQP.StringType.Decimal);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strData, "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                strData = CommonAct.FunTypeConversion.funBinConvert(this.txtEQPPPIDMap02.Text.Trim().PadRight(16, ' '), EnuEQP.StringType.Decimal);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strData, "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                strData = CommonAct.FunTypeConversion.funBinConvert(this.txtEQPPPIDMap03.Text.Trim().PadRight(16, ' '), EnuEQP.StringType.Decimal);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strData, "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);


                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                //strData = CommonAct.FunTypeConversion.funBinConvert(this.txtEQPPPIDMap02.Text.Trim().PadRight(16, ' '), EnuEQP.StringType.Hex);
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strData.Substring(4), "H");
                //strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                //strData = CommonAct.FunTypeConversion.funBinConvert(this.txtEQPPPIDMap03.Text.Trim().PadRight(16, ' '), EnuEQP.StringType.Hex);
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, strData.Substring(4), "H");
                //strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

            }
            catch (Exception)
            {
                throw;
            }
        }
        bool B1036_Flag = false;
        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            string strAddress = "W2040";
            try
            {
                tmrUpdate.Enabled = false;
                //if (PInfo.All.simulB1036_Data == 1)
                //{
                //    //if (PInfo.All.simulB1036_Flag2 == false)
                //    //{
                //        PInfo.All.simulB1036_Flag2 = true;
                //        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, PInfo.All.simul_EQPPPID, "D");
                //        strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                //        strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                //        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "1", "D");
                //        strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);

                //        if (PInfo.Unit(0).SubUnit(0).EQPPPID(PInfo.All.simul_EQPPPID) == null)
                //        {
                //            for (int dintLoop = 1; dintLoop <= PInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                //            {
                //                //string strTemp = dataGridView2.Rows[dintLoop].Cells[1].Value.ToString();
                //                string strTemp = CommonAct.FunTypeConversion.funDecimalConvert(dintLoop.ToString() + Convert.ToInt32(PInfo.All.simul_EQPPPID), EnuEQP.StringType.Hex);

                //                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, dintLoop.ToString() + Convert.ToInt32(PInfo.All.simul_EQPPPID), "D2");
                //                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 2);
                //            }
                //        }
                //        else
                //        {
                //            clsEQPPPID CurrentPPID = PInfo.Unit(0).SubUnit(0).EQPPPID(PInfo.All.simul_EQPPPID);

                //            for (int dintLoop = 1; dintLoop <= PInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                //            {
                //                //string strTemp = dataGridView2.Rows[dintLoop].Cells[1].Value.ToString();
                //                string strTemp = CommonAct.FunTypeConversion.funDecimalConvert(dintLoop.ToString() + Convert.ToInt32(PInfo.All.simul_EQPPPID), EnuEQP.StringType.Hex);

                //                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, FunStringH.funMakePLCData(FunStringH.funMakeRound(CurrentPPID.PPIDBody(dintLoop).Value, PInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format)), "D2");
                //                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 2);
                //            }
                //        }
                //        PInfo.All.simulB1036_Data = 0;
                //        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1036", "", "1", "");
                //        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1536", "", "1", "");
                //        //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1036", "", "0", "");
                //    //}
                //    //PInfo.All.simulB1036_Data = 0;
                //}
                //else
                //{

                //}


                if (PInfo.All.SimulEQPPM_CMD)
                {
                    PInfo.All.SimulEQPPM_CMD = false;
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "1", "D");

                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1670", "", "001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1680", "", "001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1690", "", "001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A0", "", "001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B0", "", "001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C0", "", "001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D0", "", "001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E0", "", "001", "");
                }

                if (PInfo.All.SimulEQPNormal_CMD)
                {
                    PInfo.All.SimulEQPNormal_CMD = false;
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "1", "D");

                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "100", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1670", "", "100", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1680", "", "100", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1690", "", "100", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A0", "", "100", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B0", "", "100", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C0", "", "100", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D0", "", "100", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E0", "", "100", "");
                }

                if (PInfo.All.SimulEQPProc_Pause_CMD)
                {
                    PInfo.All.SimulEQPProc_Pause_CMD = false;
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "1", "D");

                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1603", "", "0001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1673", "", "0001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1683", "", "0001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1693", "", "0001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A3", "", "0001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B3", "", "0001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C3", "", "0001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D3", "", "0001", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E3", "", "0001", "");
                }

                if (PInfo.All.SimulEQPProc_Resume_CMD)
                {
                    PInfo.All.SimulEQPProc_Resume_CMD = false;
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "1", "D");

                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1603", "", "1000", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1673", "", "1000", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1683", "", "1000", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1693", "", "1000", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A3", "", "1000", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B3", "", "1000", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C3", "", "1000", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D3", "", "1000", "");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E3", "", "1000", "");
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                tmrUpdate.Enabled = true;
            }
        }

        private void btnAlarmSet_Click(object sender, EventArgs e)
        {
            string strModuleID = "";
            int dintUnitID = 0;
            int dintSubUnitID = 0;
            bool dbolFlag = false;
            string strAddress = "B1200";
            try
            {
                if (string.IsNullOrEmpty(txtAlarmID.Text) == false)
                {
                    strModuleID = PInfo.Unit(0).SubUnit(0).Alarm(Convert.ToInt32(txtAlarmID.Text)).ModuleID;

                    for (int dintLoop = 0; dintLoop <= PInfo.UnitCount; dintLoop++)
                    {
                        for (int dintLoop2 = 0; dintLoop2 <= PInfo.Unit(dintLoop).SubUnitCount; dintLoop2++)
                        {
                            if (PInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID == strModuleID)
                            {
                                dintUnitID = dintLoop;
                                dintSubUnitID = dintLoop2;
                                dbolFlag = true;
                                break;
                            }
                        }
                        if (dbolFlag)
                        {
                            break;
                        }
                    }


                    strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, Convert.ToInt32(txtAlarmID.Text) - 1);

                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "3", "D");
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, strAddress, "", "1", "");

                    if (PInfo.Unit(0).SubUnit(0).Alarm(Convert.ToInt32(txtAlarmID.Text)).AlarmCode == 2)
                    {
                        switch (dintSubUnitID)
                        {

                            case 0:
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1670", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1680", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1690", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A0", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B0", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C0", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D0", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E0", "", "0100001", "");
                                break;

                            case 1:
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1670", "", "0100001", "");
                                break;
                            case 2:
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1680", "", "0100001", "");
                                break;
                            case 3:
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1690", "", "0100001", "");
                                break;
                            case 4:
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A0", "", "0100001", "");
                                break;
                            case 5:
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B0", "", "0100001", "");
                                break;
                            case 6:
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C0", "", "0100001", "");
                                break;
                            case 7:
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D0", "", "0100001", "");
                                break;
                            case 8:
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "0100001", "");
                                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E0", "", "0100001", "");
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                
            }
        }

        private void btnAlarmReset_Click(object sender, EventArgs e)
        {
            try
            {
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1700", "", "0".PadLeft(700, '0'), "");

                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "1001000", "");
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1670", "", "1001000", "");
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1680", "", "1001000", "");
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1690", "", "1001000", "");
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A0", "", "1001000", "");
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B0", "", "1001000", "");
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C0", "", "1001000", "");
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D0", "", "1001000", "");
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E0", "", "1001000", "");
            }
            catch (Exception ex)
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string strAddress = "W2234";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtLOTID.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B116A", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B163B", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string strAddress = "W2250";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtLOTID.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B118A", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B163B", "", "0", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strAddress = "W2260";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtLOTID.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B116B", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B164B", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strAddress = "W2270";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtLOTID.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B118B", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B164B", "", "0", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "2", "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "100", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1670", "", "100", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1680", "", "100", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1690", "", "100", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A0", "", "100", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B0", "", "100", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C0", "", "100", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D0", "", "100", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E0", "", "100", "");
            }
            catch (Exception)
            {
                
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "2", "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1600", "", "001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1670", "", "001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1680", "", "001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1690", "", "001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A0", "", "001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B0", "", "001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C0", "", "001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D0", "", "001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E0", "", "001", "");
            }
            catch (Exception)
            {

            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "2", "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1603", "", "0001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1673", "", "0001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1683", "", "0001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1693", "", "0001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A3", "", "0001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B3", "", "0001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C3", "", "0001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D3", "", "0001", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E3", "", "0001", "");
            }
            catch (Exception)
            {

            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W2011", "2", "D");


                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1603", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1673", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1683", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1693", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16A3", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16B3", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16C3", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16D3", "", "1000", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E3", "", "1000", "");
            }
            catch (Exception)
            {

            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            string strAddress = "W2280";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, this.textBox5.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B116C", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B165B", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            string strAddress = "W2290";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, this.textBox5.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B118C", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B165B", "", "0", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string strAddress = "W22A0";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, this.textBox5.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B116D", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B166B", "", "1", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string strAddress = "W22B0";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, this.textBox5.Text.Trim().PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B118D", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B166B", "", "0", "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            string strAddress = "W2400";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_GLSID.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_LOTID.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 2);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_SlotID.Text.Trim(), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 28);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(20, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_HostPPID.Text.Trim().PadRight(16, ' '), "A");

                strAddress = "W2600";
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_LOTStart.Text, "D");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_LotEnd.Text, "D");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1603", "", "0010", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E3", "", "0010", "");


                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B160B", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B168B", "", "1", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1567", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            string strAddress = "W2620";
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(16, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_GLSID.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 8);
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, "".PadRight(1, ' '), "A");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", strAddress, txtST01_SlotID.Text.Trim().PadRight(16, ' '), "A");
                strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 1);
                if (PInfo.Unit(3).SubUnit(0).CurrGLSCount == 1)
                {
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1603", "", "1000", "");
                }
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B16E3", "", "1000", "");

                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B160B", "", "0", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B168B", "", "0", "");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "B1587", "", "1", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W1441", textBox2.Text.Trim(), "D2");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W1443", textBox2.Text.Trim(), "D2");
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.Simulation, "", "W1445", textBox2.Text.Trim(), "D2");
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }




        
     



        

        

        


    }
}



