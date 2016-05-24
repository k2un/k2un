using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using InfoAct;

namespace DisplayAct
{
    public partial class frmLogViewAlarm : Form
    {

        public clsInfo PInfo = clsInfo.Instance;

        public int pintUnitID = 0;
        public int pintAlarmID = 0;
        public string pstrDate = "";

        public frmLogViewAlarm()
        {
            InitializeComponent();
        }

        public void subFormLoad(string strDate, int intAlarmID)
        {
            try
            {
                //pintUnitID = this.PInfo.funEQPNameToUnitID(strUnitName.Trim());
                pintAlarmID = intAlarmID;
                pstrDate = strDate;

                subAlarmInfoDisplay();

                subGlassInfoDisplay();

                this.CenterToScreen();
                this.Show();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// �ش� �˶����� Display
        /// </summary>
        private void subAlarmInfoDisplay()
        {
            string dstrDateTime = "";
            try
            {
                this.lvwAlarmInfo.Items.Clear();

                string[] dstrData = new string[] { "", "" };
                ListViewItem lvIPort;

                dstrData[0] = "Alarm ID";
                dstrData[1] = pintAlarmID.ToString();

                lvIPort = new ListViewItem(dstrData);
                this.lvwAlarmInfo.Items.Add(lvIPort);

                dstrData[0] = "Alarm Description";
                if (PInfo.Unit(pintUnitID).SubUnit(0).Alarm(pintAlarmID) == null) dstrData[1] = "";
                else dstrData[1] = PInfo.Unit(pintUnitID).SubUnit(0).Alarm(pintAlarmID).AlarmDesc;

                lvIPort = new ListViewItem(dstrData);
                this.lvwAlarmInfo.Items.Add(lvIPort);

                dstrData[0] = "Occur Time";
                dstrDateTime = pstrDate.Substring(0, 4) + "-" + pstrDate.Substring(4, 2) + "-" + pstrDate.Substring(6, 2) + " " +
                               pstrDate.Substring(8, 2) + ":" + pstrDate.Substring(10, 2) + ":" + pstrDate.Substring(12, 2);
                dstrData[1] = dstrDateTime;

                lvIPort = new ListViewItem(dstrData);
                this.lvwAlarmInfo.Items.Add(lvIPort);

                lvIPort = null;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //�ش� �˶��߻����� �۷��� ��ġ���� Display
        private void subGlassInfoDisplay()
        {
            string dstrFilePath = "";
            string dstrData = "";
            string[] dstrArr = null;
            string[] dstrSubArr = null;
            string[] dstrArrayFileContent;              //�����͸� �б� ���� �ӽ� ����


            try
            {
                this.grdGlassInfo.Rows.Clear();

                //���� ��θ� �о
                dstrFilePath = Application.StartupPath + "\\" + "PLCLOG" + "\\" + pstrDate.Substring(0, 8) + "\\" + "AlarmGLSInfo.Log";

                //������ ��� ������ ������ ��Ʈ�� �迭�� �����Ѵ�(���κ� ������ �迭�� ����)
                if (File.Exists(dstrFilePath) == true)
                {
                    System.IO.FileStream fs = new FileStream(dstrFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    System.IO.StreamReader sr = new StreamReader(fs);


                    dstrArrayFileContent = sr.ReadToEnd().Split('\n');
                    

                    //dstrArrayFileContent = File.ReadAllLines(dstrFilePath);
                    for (int dintLoop = 0; dintLoop <= dstrArrayFileContent.Length - 1; dintLoop++)
                    {
                        dstrData = dstrArrayFileContent[dintLoop];
                        dstrArr = dstrData.Split(new char[] { ',' });

                        if (dstrArr[0] == pstrDate && dstrArr[1] == pintUnitID.ToString() && dstrArr[2] == pintAlarmID.ToString())
                        {
                            
                            for (int dintIndex = 0; dintIndex < this.PInfo.UnitCount; dintIndex++)
                            {
                                this.grdGlassInfo.Rows.Add();
                                this.grdGlassInfo.Rows[dintIndex].Cells[0].Value = this.PInfo.Unit(dintIndex+1).SubUnit(0).ModuleID.Substring(14,4);  //������

                                if (this.PInfo.Unit(0).SubUnit(0).Alarm(pintAlarmID).ModuleID == this.PInfo.Unit(dintIndex + 1).SubUnit(0).ModuleID)
                                {
                                    grdGlassInfo.Rows[dintIndex].DefaultCellStyle.BackColor = Color.Red;
                                }

                                if (dstrArr[3 + dintIndex].Contains("/") == true)
                                {
                                    dstrSubArr = dstrArr[3 + dintIndex].Split(new char[] { '/' });
                                    this.grdGlassInfo.Rows[dintIndex].Cells[1].Value = dstrSubArr[0].Trim();   //Glass ID
                                    this.grdGlassInfo.Rows[dintIndex].Cells[2].Value = dstrSubArr[1].Trim();   //Lot ID
                                    this.grdGlassInfo.Rows[dintIndex].Cells[3].Value = dstrSubArr[2].Trim();   //Slot No
                                    this.grdGlassInfo.Rows[dintIndex].Cells[4].Value = dstrSubArr[3].Trim();   //Host PPID

                                    dstrSubArr = null;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
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
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void grdGlassInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void frmLogView_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}