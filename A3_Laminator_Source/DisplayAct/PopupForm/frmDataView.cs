using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using CommonAct;

namespace DisplayAct
{
    public partial class frmDataView : Form
    {
        public InfoAct.clsInfo PInfo = InfoAct.clsInfo.Instance;                   //구조체

        public frmDataView()
        {
            InitializeComponent();
        }

        //*******************************************************************************
        //  Function Name : subFormLoad()
        //  Description   : Display에 사용할 스레드 생성 및 시작
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/10          구 정환         [L 00] 
        //*******************************************************************************
        public void subFormLoad()
        {
            try
            {
                //subshowTreeView();
                this.Show();
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            subshowTreeView();
        }

        public void subshowTreeView()
        {
            if (MessageBox.Show("CIM Data 정보를 불러오는 중입니다!!\r\n설비 가동 중에 사용하시면 Data보고누락 및 문제가 발생할 수 있습니다.\r\n그래도 사용하시겠습니까?", "CIM Data Loading", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int A = 0;
                int B = 0;
                int C = 0;
                int D = 0;
                int E = 0;
                int F = 0;
                int G = 0;
                //string dstrTemp = "";
                //string[] darrEvent;

                try
                {
                    TreeView1.Nodes.Clear();

                    //전체 EQP 공통
                    //0. All Info
                    TreeView1.Nodes.Add("00. All");
                    if (this.PInfo.All != null)
                    {
                        TreeView1.Nodes[A].Nodes.Add("SoftVersion: " + this.PInfo.All.SoftVersion);
                        TreeView1.Nodes[A].Nodes.Add("MDLN: " + this.PInfo.All.MDLN);
                        TreeView1.Nodes[A].Nodes.Add("HostConnect: " + this.PInfo.All.HostConnect);
                        TreeView1.Nodes[A].Nodes.Add("SecomDriver: " + this.PInfo.All.SecomDriver);

                        TreeView1.Nodes[A].Nodes.Add("CommPort: " + this.PInfo.All.CommPort);
                        TreeView1.Nodes[A].Nodes.Add("CommSetting: " + this.PInfo.All.CommSetting);

                        TreeView1.Nodes[A].Nodes.Add("DeviceID: " + this.PInfo.All.DeviceID);
                        TreeView1.Nodes[A].Nodes.Add("LocalPort: " + this.PInfo.All.LocalPort);
                        TreeView1.Nodes[A].Nodes.Add("RetryCount: " + this.PInfo.All.RetryCount);
                        TreeView1.Nodes[A].Nodes.Add("T3: " + this.PInfo.All.T3);
                        TreeView1.Nodes[A].Nodes.Add("T5: " + this.PInfo.All.T5);
                        TreeView1.Nodes[A].Nodes.Add("T6: " + this.PInfo.All.T6);
                        TreeView1.Nodes[A].Nodes.Add("T7: " + this.PInfo.All.T7);
                        TreeView1.Nodes[A].Nodes.Add("T8: " + this.PInfo.All.T8);
                        TreeView1.Nodes[A].Nodes.Add("T9: " + this.PInfo.All.T9);

                        TreeView1.Nodes[A].Nodes.Add("CurrentHOSTPPID: " + this.PInfo.All.CurrentHOSTPPID);
                        TreeView1.Nodes[A].Nodes.Add("CurrentEQPPPID: " + this.PInfo.All.CurrentEQPPPID);
                        TreeView1.Nodes[A].Nodes.Add("CurrentRegisteredHOSTPPIDCount: " + this.PInfo.All.CurrentRegisteredHOSTPPIDCount);
                        TreeView1.Nodes[A].Nodes.Add("CurrentRegisteredEQPPPIDCount: " + this.PInfo.All.CurrentRegisteredEQPPPIDCount);
                        TreeView1.Nodes[A].Nodes.Add("HOSTPPIDCommandCount: " + this.PInfo.All.HOSTPPIDCommandCount);
                        TreeView1.Nodes[A].Nodes.Add("EQPPPIDCommandCount: " + this.PInfo.All.EQPPPIDCommandCount);
                        TreeView1.Nodes[A].Nodes.Add("SetUpPPIDPLCWriteCount: " + this.PInfo.All.SetUpPPIDPLCWriteCount);

                        TreeView1.Nodes[A].Nodes.Add("ProgramEnd: " + this.PInfo.All.ProgramEnd);
                        TreeView1.Nodes[A].Nodes.Add("UserID: " + this.PInfo.All.UserID);
                        TreeView1.Nodes[A].Nodes.Add("OperatorCallFormVisible: " + this.PInfo.All.OperatorCallFormVisible);
                        TreeView1.Nodes[A].Nodes.Add("AutoMode: " + this.PInfo.All.AutoMode);
                        TreeView1.Nodes[A].Nodes.Add("PMCode: " + this.PInfo.All.PMCode);
                        TreeView1.Nodes[A].Nodes.Add("AlarmExist: " + this.PInfo.All.AlarmExist);
                        TreeView1.Nodes[A].Nodes.Add("UserLogInDuringTime: " + this.PInfo.All.UserLogInDuringTime);
                        TreeView1.Nodes[A].Nodes.Add("PLCActionEnd: " + this.PInfo.All.PLCActionEnd);

                        TreeView1.Nodes[A].Nodes.Add("isReceivedFromHOST: " + this.PInfo.All.isReceivedFromHOST);
                        TreeView1.Nodes[A].Nodes.Add("isReceivedFromCIM: " + this.PInfo.All.isReceivedFromCIM);

                        TreeView1.Nodes[A].Nodes.Add("SystemINIFilePath: " + this.PInfo.All.SystemINIFilePath);
                        TreeView1.Nodes[A].Nodes.Add("ONLINEModeChange: " + this.PInfo.All.ONLINEModeChange);
                        TreeView1.Nodes[A].Nodes.Add("ControlState: " + this.PInfo.All.ControlState);
                        TreeView1.Nodes[A].Nodes.Add("ControlStateOLD: " + this.PInfo.All.ControlStateOLD);
                        TreeView1.Nodes[A].Nodes.Add("WantControlState: " + this.PInfo.All.WantControlState);

                        TreeView1.Nodes[A].Nodes.Add("OccurHeavyAlarmID(Heavy): " + this.PInfo.All.OccurHeavyAlarmID);
                        TreeView1.Nodes[A].Nodes.Add("ClearHeavyAlarmID(Heavy): " + this.PInfo.All.ClearHeavyAlarmID);

                        TreeView1.Nodes[A].Nodes.Add("ControlstateChangeBYWHO: " + this.PInfo.All.ControlstateChangeBYWHO);
                        TreeView1.Nodes[A].Nodes.Add("EQPSpecifiedCtrlBYWHO: " + this.PInfo.All.EQPSpecifiedCtrlBYWHO);
                        TreeView1.Nodes[A].Nodes.Add("ECIDChangeBYWHO: " + this.PInfo.All.ECIDChangeBYWHO);
                        TreeView1.Nodes[A].Nodes.Add("EOIDChangeBYWHO: " + this.PInfo.All.EOIDChangeBYWHO);

                        TreeView1.Nodes[A].Nodes.Add("ModeChangeFormVisible: " + this.PInfo.All.ModeChangeFormVisible);
                        TreeView1.Nodes[A].Nodes.Add("SVIDPLCReadLength: " + this.PInfo.All.SVIDPLCReadLength);
                        TreeView1.Nodes[A].Nodes.Add("GLSAPDPLCReadLength: " + this.PInfo.All.GLSAPDPLCReadLength);
                        TreeView1.Nodes[A].Nodes.Add("HOSTReportEOIDCount: " + this.PInfo.All.HOSTReportEOIDCount);
                        TreeView1.Nodes[A].Nodes.Add("SEMControllerConnect: " + this.PInfo.All.SEMControllerConnect);
                        TreeView1.Nodes[A].Nodes.Add("SEMStartReplyCheck: " + this.PInfo.All.SEMStartReplyCheck);
                        TreeView1.Nodes[A].Nodes.Add("SEMAlarmTime: " + this.PInfo.All.SEMAlarmTime);
                        TreeView1.Nodes[A].Nodes.Add("SVIDPLCNotReadLength: " + this.PInfo.All.SVIDPLCNotReadLength);
                    }
                    A = A + 1;


                    //1. Eqp Info
                    TreeView1.Nodes.Add("01. EQP");
                    if (this.PInfo.EQP("Main") != null)
                    {
                        TreeView1.Nodes[A].Nodes.Add("UnitCount: " + this.PInfo.EQP("Main").UnitCount);
                        TreeView1.Nodes[A].Nodes.Add("PLCConnect: " + this.PInfo.EQP("Main").PLCConnect);
                        TreeView1.Nodes[A].Nodes.Add("PLCStartConnect: " + this.PInfo.EQP("Main").PLCStartConnect);
                        TreeView1.Nodes[A].Nodes.Add("PLCIP: " + this.PInfo.EQP("Main").PLCIP);
                        TreeView1.Nodes[A].Nodes.Add("PLCPort: " + this.PInfo.EQP("Main").PLCPort);
                        TreeView1.Nodes[A].Nodes.Add("DummyPLC: " + this.PInfo.EQP("Main").DummyPLC);
                        TreeView1.Nodes[A].Nodes.Add("WordStart: " + this.PInfo.EQP("Main").WordStart);
                        TreeView1.Nodes[A].Nodes.Add("WordEnd: " + this.PInfo.EQP("Main").WordEnd);
                        TreeView1.Nodes[A].Nodes.Add("BitScanCount: " + this.PInfo.EQP("Main").BitScanCount);
                        TreeView1.Nodes[A].Nodes.Add("Type: " + this.PInfo.EQP("Main").Type);
                        TreeView1.Nodes[A].Nodes.Add("EQPID: " + this.PInfo.EQP("Main").EQPID);
                        TreeView1.Nodes[A].Nodes.Add("EQPType: " + this.PInfo.EQP("Main").EQPType);
                        TreeView1.Nodes[A].Nodes.Add("EQPName: " + this.PInfo.EQP("Main").EQPName);
                        TreeView1.Nodes[A].Nodes.Add("RecipeCheck: " + this.PInfo.EQP("Main").RecipeCheck);
                    }
                    A = A + 1;


                    //2. Unit Info
                    TreeView1.Nodes.Add("02. Unit");
                    for (int dintUnitID = 0; dintUnitID <= this.PInfo.EQP("Main").UnitCount; dintUnitID++)
                    {
                        B = B + 1;
                        TreeView1.Nodes[A].Nodes.Add("UnitID: " + dintUnitID.ToString() + "(" + this.PInfo.Unit(dintUnitID).SubUnit(0).ModuleID + ")");

                        for (int dintSubUnit = 0; dintSubUnit <= this.PInfo.Unit(dintUnitID).SubUnitCount; dintSubUnit++)
                        {
                            //SubUnit 개수만큼 등록
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("SubUnit:" + dintSubUnit.ToString());
                            C = TreeView1.Nodes[A].Nodes[B - 1].GetNodeCount(false) - 1;

                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("UnitID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).UnitID);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("ModuleID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).ModuleID);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("GLSExist: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSExist);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("EQPState: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).EQPState);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("EQPStateOLD: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).EQPStateOLD);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("EQPProcessState: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).EQPProcessState);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("EQPProcessStateOLD: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).EQPProcessStateOLD);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("EQPStateChangeBYWHO: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).EQPStateChangeBYWHO);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("EQPStateLastCommand: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).EQPStateLastCommand);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("EQPProcessStateChangeBYWHO: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).EQPProcessStateChangeBYWHO);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("EQPProcessStateLastCommand: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).EQPProcessStateLastCommand);

                            if (dintUnitID == 0)
                            {
                                //HOSTPPID Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("HOSTPPID");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                foreach (string dstrName in this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).HOSTPPID())
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dstrName);

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("HostPPID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).HOSTPPID(dstrName).HostPPID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("PPIDVer: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).HOSTPPID(dstrName).PPIDVer);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("DateTime: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).HOSTPPID(dstrName).DateTime);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("EQPPPID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).HOSTPPID(dstrName).EQPPPID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Comment: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).HOSTPPID(dstrName).Comment);
                                }
                                E = 0;  //초기화


                                //EQPPPID Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("EQPPPID");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                foreach (string dstrName in this.PInfo.Unit(0).SubUnit(0).EQPPPID())
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dstrName);

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("PPIDVer: " + this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDVer);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("DateTime: " + this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).DateTime);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Comment: " + this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).Comment);

                                    //EQPPPID별 PPIDBody
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("PPIDBody");
                                    F = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].GetNodeCount(false) - 1;
                                    for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDBodyCount; dintIndex++)
                                    {
                                        G = G + 1;
                                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes[F].Nodes.Add(dintIndex.ToString());

                                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes[F].Nodes[G - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDBody(dintIndex).Name);       //실제저장한 값
                                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes[F].Nodes[G - 1].Nodes.Add("Value: " + this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDBody(dintIndex).Value);     //실제저장할 값
                                    }
                                    G = 0;      //초기화
                                }
                                E = 0;  //초기화


                                //PPIDBody Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("PPIDBody(기준정보)");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintIndex++)
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dintIndex.ToString());

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Name);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Length: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Length);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Min: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Min);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Max: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Max);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Format: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Format);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Unit: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Unit);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Range: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Range);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("DESC: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).DESC);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Value: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Value);
                                    //TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("UnitID: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).UnitID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).ModuleID);

                                }
                                E = 0;  //초기화

                                //GLSAPD Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("GLSAPD(기준정보)");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                foreach (int intIndex in this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSAPD())
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSAPD(intIndex).Name);

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Index: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSAPD(intIndex).Index);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Name: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSAPD(intIndex).Name);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Length: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSAPD(intIndex).Length);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Format: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSAPD(intIndex).Format);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("HaveMinusValue: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSAPD(intIndex).HaveMinusValue);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Value: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSAPD(intIndex).Value);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSAPD(intIndex).ModuleID);
                                    //TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("UnitID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).GLSAPD(intIndex).UnitID);
                                }
                                E = 0;  //초기화

                                //Alarm Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("Alarm(기준정보)");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                foreach (int dintAlarmID in this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).Alarm())
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dintAlarmID.ToString());

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).Alarm(dintAlarmID).ModuleID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmCode: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).Alarm(dintAlarmID).AlarmCode);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmType: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).Alarm(dintAlarmID).AlarmType);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmDesc: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).Alarm(dintAlarmID).AlarmDesc);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmOCCTime: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).Alarm(dintAlarmID).AlarmOCCTime);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmEventType: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).Alarm(dintAlarmID).AlarmEventType);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmReport: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).Alarm(dintAlarmID).AlarmReport);
                                }
                                E = 0;  //초기화

                                //User Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("User");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                foreach (string dstrUserID in this.PInfo.Unit(0).SubUnit(0).User())
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dstrUserID);

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("dstrUserID: " + dstrUserID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Level: " + this.PInfo.Unit(0).SubUnit(0).User(dstrUserID).Level);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("PassWord: " + this.PInfo.Unit(0).SubUnit(0).User(dstrUserID).PassWord);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Desc: " + this.PInfo.Unit(0).SubUnit(0).User(dstrUserID).Desc);
                                }
                                E = 0;  //초기화

                                //UserLevel Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("UserLevel(기준정보)");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                foreach (int dintIndex in this.PInfo.Unit(0).SubUnit(0).UserLevel())
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dintIndex.ToString());

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("UserLevel: " + dintIndex.ToString());
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Desc: " + this.PInfo.Unit(0).SubUnit(0).UserLevel(dintIndex).Desc);
                                }
                                E = 0;  //초기화

                                //EOID Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("EOID");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).EOIDCount; dintIndex++)
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dintIndex.ToString());

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("EOID: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("EOMD: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMD);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("EOV: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("EOVMin: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMin);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("EOVMax: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMax);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("EOVMin: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMin);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("EOVMax: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMax);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("DESC: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).DESC);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("PLCWrite: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).PLCWrite);
                                }
                                E = 0;  //초기화

                                //SVID Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("SVID");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                foreach (int dintSVID in PInfo.Unit(0).SubUnit(0).SVID())
                                {
                                    InfoAct.clsSVID CurrentSVID = PInfo.Unit(0).SubUnit(0).SVID(dintSVID);

                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dintSVID.ToString());

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Name: " + CurrentSVID.Name);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Length: " + CurrentSVID.Length);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Format: " + CurrentSVID.Format);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Value: " + CurrentSVID.Value);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Type: " + CurrentSVID.Type);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Unit: " + CurrentSVID.Unit);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Range: " + CurrentSVID.Range);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("HaveMinusValue: " + CurrentSVID.HaveMinusValue);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("DESC: " + CurrentSVID.DESC);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("UnitID: " + CurrentSVID.UnitID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ModuleID: " + CurrentSVID.ModuleID);
                                }
                                E = 0;  //초기화

                                //ECID Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("ECID");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).ECIDCount; dintIndex++)
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dintIndex.ToString());
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Name);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Min: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Min);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ECSLL: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECSLL);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ECWLL: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECWLL);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ECDEF: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECDEF);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ECWUL: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECWUL);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ECSUL: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECSUL);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Format: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Format);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Max: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Max);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ModuleID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Use: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Use);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("UnitID: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).UnitID);
                                }
                                E = 0;  //초기화

                                //CurrAlarm Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("CurrAlarm");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                foreach (int dintAlarmID in this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrAlarm())
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dintAlarmID.ToString());

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrAlarm(dintAlarmID).ModuleID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmCode: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrAlarm(dintAlarmID).AlarmCode.ToString());
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmType: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrAlarm(dintAlarmID).AlarmType);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmDesc: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrAlarm(dintAlarmID).AlarmDesc);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmOCCTime: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrAlarm(dintAlarmID).AlarmOCCTime);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmEventType: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrAlarm(dintAlarmID).AlarmEventType);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("AlarmReport: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrAlarm(dintAlarmID).AlarmReport);
                                }
                                E = 0;  //초기화

                                //CurrGLS Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("CurrGLS");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                foreach (string dstrName in this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrGLS())
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dstrName);

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("GLSID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrGLS(dstrName).H_PANELID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("SlotID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrGLS(dstrName).SlotID);
                                }
                                E = 0;  //초기화
                            }
                            else
                            {
                                //CurrGLS Info
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("CurrGLS");
                                D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                                foreach (string dstrName in this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrGLS())
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dstrName);

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("GLSID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrGLS(dstrName).H_PANELID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("SlotID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrGLS(dstrName).SlotID);
                                }
                                E = 0;  //초기화
                            }

                        }
                    }
                    A = A + 1;
                    B = 0; C = 0; D = 0; E = 0; F = 0;

                    //03. Trace
                    TreeView1.Nodes.Add("03. Trace");
                    foreach (int dintTRID in this.PInfo.Unit(0).SubUnit(0).TRID())
                    {
                        B = B + 1;
                        TreeView1.Nodes[A].Nodes.Add(dintTRID.ToString());
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("REPGSZ: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZ);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("DSPER: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).DSPER);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("TOTSMP: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TOTSMP);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("TimeAcc: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("GrpCnt: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).GrpCnt);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("REPGSZCnt: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZCnt);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("SampleNo: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).SampleNo);

                        //TRID 별 Group
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Group");
                        C = TreeView1.Nodes[A].Nodes[B - 1].GetNodeCount(false) - 1;
                        for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).GroupCount; dintIndex++)
                        {
                            D = D + 1;
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add(dintIndex.ToString());
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("GroupID: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintIndex).GroupID);       //실제저장한 값
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("ReadTime: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintIndex).ReadTime);     //실제저장할 값

                            //Group 별 SVID
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("SVID");
                            E = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].GetNodeCount(false) - 1;
                            for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintIndex).SVIDCount; dintLoop++)
                            {
                                F = F + 1;
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes.Add(this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintIndex).SVID(dintLoop).SVID.ToString());

                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes[F - 1].Nodes.Add("SVID: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintIndex).SVID(dintLoop).SVID);       //실제저장한 값
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes[F - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintIndex).SVID(dintLoop).Name);       //실제저장한 값
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes[F - 1].Nodes.Add("Value: " + this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintIndex).SVID(dintLoop).Value);     //실제저장할 값
                            }
                            F = 0;      //초기화
                        }
                        D = 0;      //초기화
                    }
                    //초기화
                    A = A + 1;
                    B = 0; C = 0; D = 0; E = 0; F = 0;

                    ////4. APC Info
                    TreeView1.Nodes.Add("04. APC");
                    foreach (string str in PInfo.APC())
                    {
                        B = B + 1;
                        InfoAct.clsAPC CurrentAPC = PInfo.APC(str);
                        TreeView1.Nodes[A].Nodes.Add(CurrentAPC.GLSID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("GLASSID: " + CurrentAPC.GLSID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("JOBID: " + CurrentAPC.JOBID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("EQPPPID:" + CurrentAPC.EQPPPID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("SetTime: " + CurrentAPC.SetTime);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("STATE: " + CurrentAPC.State);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Parameters");
                        C = TreeView1.Nodes[A].Nodes[B - 1].GetNodeCount(false) - 1;
                        for (int dintLoop = 0; dintLoop < CurrentAPC.ParameterName.Length; dintLoop++)
                        {
                            D++;
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add(CurrentAPC.ParameterName[dintLoop] + "=" + CurrentAPC.ParameterValue[dintLoop]);
                        }
                    }
                    //초기화
                    A = A + 1;
                    B = 0; C = 0; D = 0; E = 0;

                    ////5. PPC Info
                    TreeView1.Nodes.Add("05. PPC");
                    foreach (string str in PInfo.PPC())
                    {
                        B = B + 1;
                        InfoAct.clsPPC CurrentPPC = PInfo.PPC(str);
                        TreeView1.Nodes[A].Nodes.Add(CurrentPPC.HGLSID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("GLASSID: " + CurrentPPC.HGLSID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("JOBID: " + CurrentPPC.JOBID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("SetTime: " + CurrentPPC.SetTime);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("STATE: " + CurrentPPC.RunState);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("ORDER");
                        C = TreeView1.Nodes[A].Nodes[B - 1].GetNodeCount(false) - 1;
                        for (int dintLoop = 0; dintLoop < CurrentPPC.P_MODULEID.Length; dintLoop++)
                        {
                            D++;
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add(CurrentPPC.P_MODULEID[dintLoop] + " = " + CurrentPPC.P_ORDER[dintLoop] + " = " + CurrentPPC.P_STATUS[dintLoop]);
                        }
                    }
                    //초기화
                    A = A + 1;
                    B = 0; C = 0; D = 0; E = 0;

                    ////6. RPC Info
                    TreeView1.Nodes.Add("06. RPC");
                    foreach (string str in PInfo.RPC())
                    {
                        B = B + 1;
                        InfoAct.clsRPC CurrentRPC = PInfo.RPC(str);
                        TreeView1.Nodes[A].Nodes.Add(CurrentRPC.HGLSID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("GLASSID: " + CurrentRPC.HGLSID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("JOBID: " + CurrentRPC.JOBID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("RPC_PPID: " + CurrentRPC.RPC_PPID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Origin_PPID: " + CurrentRPC.OriginPPID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("SetTime: " + CurrentRPC.SetTime);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("STATE: " + CurrentRPC.RPC_STATE);
                    }
                    //초기화
                    A = A + 1;
                    B = 0; C = 0; D = 0; E = 0;

                    TreeView1.Nodes.Add("07. Film");
                    foreach (string str in PInfo.LOT())
                    {
                        InfoAct.clsLOT CurrentLot = PInfo.LOTID(str);

                        B = B + 1;
                        TreeView1.Nodes[A].Nodes.Add(CurrentLot.LOTID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Film_CaseI D: " + CurrentLot.LOTID);
                        //TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("PORTID: " + CurrentLot.PortID);
                        InfoAct.clsSlot CurrentSlot = CurrentLot.Slot(1);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Film_ID : " + CurrentSlot.GlassID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Film_Code : " + CurrentSlot.PRODUCTID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Film_Count_HostCMD : " + (CurrentLot.SlotCount + 1));
                        //TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Film_Count_Real : " + CurrentLot.FilmCount);
                        //TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("UseCount: " + CurrentSlot.SlotID);

                    }
                    //초기화
                    A = A + 1;
                    B = 0; C = 0; D = 0; E = 0;


                    //SVID 테스트용
                    //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "------------------------------------------------------------------------------" + "\r\n");
                    //for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).SVIDCount; dintLoop++)
                    //{
                    //    dstrTemp = dstrTemp +  FunStringH.funStringData(dintLoop.ToString() + "(" + this.PInfo.Unit(0).SubUnit(0).SVID(dintLoop).Name + ")" + ": ", 45) + this.PInfo.Unit(0).SubUnit(0).SVID(dintLoop).Value + "\r\n";
                    //}
                    //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrTemp);
                    //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "------------------------------------------------------------------------------" + "\r\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}