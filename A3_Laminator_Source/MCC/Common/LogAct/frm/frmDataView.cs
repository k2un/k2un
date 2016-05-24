using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using CommonAct;

namespace LogAct
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
                    TreeView1.Nodes[A].Nodes.Add("CV01GLSID: " + this.PInfo.All.CV01GLSID);
                    TreeView1.Nodes[A].Nodes.Add("OperatorCallFormVisible: " + this.PInfo.All.OperatorCallFormVisible);
                    TreeView1.Nodes[A].Nodes.Add("AutoMode: " + this.PInfo.All.AutoMode);
                    TreeView1.Nodes[A].Nodes.Add("PMCode: " + this.PInfo.All.PMCode);
                    TreeView1.Nodes[A].Nodes.Add("AlarmExist: " + this.PInfo.All.AlarmExist);
                    TreeView1.Nodes[A].Nodes.Add("UserLogInDuringTime: " + this.PInfo.All.UserLogInDuringTime);
                    //TreeView1.Nodes[A].Nodes.Add("PPIDComment: " + this.PInfo.All.PPIDComment);
                    TreeView1.Nodes[A].Nodes.Add("PLCActionEnd: " + this.PInfo.All.PLCActionEnd);

                    TreeView1.Nodes[A].Nodes.Add("isReceivedFromHOST: " + this.PInfo.All.isReceivedFromHOST);
                    TreeView1.Nodes[A].Nodes.Add("isReceivedFromCIM: " + this.PInfo.All.isReceivedFromCIM);

                    //TreeView1.Nodes[A].Nodes.Add("ReceivedFromHOST_EQPPPIDExist: " + this.PInfo.All.ReceivedFromHOST_EQPPPIDExist);
                    //TreeView1.Nodes[A].Nodes.Add("ReceivedFromHOST_HOSTPPIDExist: " + this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist);

                    TreeView1.Nodes[A].Nodes.Add("SystemINIFilePath: " + this.PInfo.All.SystemINIFilePath);
                    TreeView1.Nodes[A].Nodes.Add("SecomINIFilePath: " + this.PInfo.All.SecomINIFilePath);
                    TreeView1.Nodes[A].Nodes.Add("ONLINEModeChange: " + this.PInfo.All.ONLINEModeChange);
                    TreeView1.Nodes[A].Nodes.Add("ControlState: " + this.PInfo.All.ControlState);
                    TreeView1.Nodes[A].Nodes.Add("ControlStateOLD: " + this.PInfo.All.ControlStateOLD);
                    TreeView1.Nodes[A].Nodes.Add("WantControlState: " + this.PInfo.All.WantControlState);

                    //foreach (DictionaryEntry de in this.PInfo.All.ECIDChange)
                    //{
                    //    dstrTemp = dstrTemp + de.Key.ToString() + ",";
                    //}
                    //TreeView1.Nodes[A].Nodes.Add("ECIDChange:" + dstrTemp);

                    TreeView1.Nodes[A].Nodes.Add("EQPProcessTimeOverReset: " + this.PInfo.All.EQPProcessTimeOverReset);
                    TreeView1.Nodes[A].Nodes.Add("OccurHeavyAlarmID(Heavy): " + this.PInfo.All.OccurHeavyAlarmID);
                    TreeView1.Nodes[A].Nodes.Add("ClearHeavyAlarmID(Heavy): " + this.PInfo.All.ClearHeavyAlarmID);

                    TreeView1.Nodes[A].Nodes.Add("ControlstateChangeBYWHO: " + this.PInfo.All.ControlstateChangeBYWHO);
                    TreeView1.Nodes[A].Nodes.Add("EQPSpecifiedCtrlBYWHO: " + this.PInfo.All.EQPSpecifiedCtrlBYWHO);
                    TreeView1.Nodes[A].Nodes.Add("ECIDChangeBYWHO: " + this.PInfo.All.ECIDChangeBYWHO);
                    TreeView1.Nodes[A].Nodes.Add("EOIDChangeBYWHO: " + this.PInfo.All.EOIDChangeBYWHO);

                    //TreeView1.Nodes[A].Nodes.Add("EQPProcessTimeOverLapseTimeINIT: " + this.PInfo.All.EQPProcessTimeOverLapseTimeINIT);
                    //TreeView1.Nodes[A].Nodes.Add("EQPProcessTimeOverLapseTimeIDLE: " + this.PInfo.All.EQPProcessTimeOverLapseTimeIDLE);
                    //TreeView1.Nodes[A].Nodes.Add("EQPProcessTimeOverLapseTimeSETUP: " + this.PInfo.All.EQPProcessTimeOverLapseTimeSETUP);
                    //TreeView1.Nodes[A].Nodes.Add("EQPProcessTimeOverLapseTimeREADY: " + this.PInfo.All.EQPProcessTimeOverLapseTimeREADY);
                    //TreeView1.Nodes[A].Nodes.Add("EQPProcessTimeOverLapseTimeEXECUTE: " + this.PInfo.All.EQPProcessTimeOverLapseTimeEXECUTE);
                    //TreeView1.Nodes[A].Nodes.Add("EQPProcessTimeOverLapseTimePAUSE: " + this.PInfo.All.EQPProcessTimeOverLapseTimePAUSE);
                    //TreeView1.Nodes[A].Nodes.Add("EQPProcessTimeOverLapseTimeSTOP: " + this.PInfo.All.EQPProcessTimeOverLapseTimeSTOP);

                    TreeView1.Nodes[A].Nodes.Add("ModeChangeFormVisible: " + this.PInfo.All.ModeChangeFormVisible);

                    TreeView1.Nodes[A].Nodes.Add("VCRPass: " + this.PInfo.All.VCRPass);
                    TreeView1.Nodes[A].Nodes.Add("VCRPMDT: " + this.PInfo.All.VCRPMDT);
                    TreeView1.Nodes[A].Nodes.Add("VCRMatch: " + this.PInfo.All.VCRMatch);
                    TreeView1.Nodes[A].Nodes.Add("VCRMismatch: " + this.PInfo.All.VCRMismatch);
                    TreeView1.Nodes[A].Nodes.Add("VCRKeyin: " + this.PInfo.All.VCRKeyin);
                    TreeView1.Nodes[A].Nodes.Add("VCRTimeout: " + this.PInfo.All.VCRTimeout);
                    TreeView1.Nodes[A].Nodes.Add("VCRSkip: " + this.PInfo.All.VCRSkip);
                    TreeView1.Nodes[A].Nodes.Add("VCRLastModified: " + this.PInfo.All.VCRLastModified);

                    TreeView1.Nodes[A].Nodes.Add("MCCNetworkPath: " + this.PInfo.All.MCCNetworkPath);
                    TreeView1.Nodes[A].Nodes.Add("MCCNetworkPort: " + this.PInfo.All.MCCNetworkPort);
                    TreeView1.Nodes[A].Nodes.Add("MCCNetworkUserID: " + this.PInfo.All.MCCNetworkUserID);
                    TreeView1.Nodes[A].Nodes.Add("MCCNetworkPassword: " + this.PInfo.All.MCCNetworkPassword);
                    TreeView1.Nodes[A].Nodes.Add("MCCLootFilePath: " + this.PInfo.All.MCCLootFilePath);
                    TreeView1.Nodes[A].Nodes.Add("MCCFileUploadTime: " + this.PInfo.All.MCCFileUploadTime);
                    TreeView1.Nodes[A].Nodes.Add("MCCFileUploadUse: " + this.PInfo.All.MCCFileUploadUse);
                    TreeView1.Nodes[A].Nodes.Add("MCCLogFileDelete: " + this.PInfo.All.MCCLogFileDelete);

                    TreeView1.Nodes[A].Nodes.Add("SVIDPLCReadLength: " + this.PInfo.All.SVIDPLCReadLength);
                    TreeView1.Nodes[A].Nodes.Add("GLSAPDPLCReadLength: " + this.PInfo.All.GLSAPDPLCReadLength);
                    TreeView1.Nodes[A].Nodes.Add("HOSTReportEOIDCount: " + this.PInfo.All.HOSTReportEOIDCount);

                    TreeView1.Nodes[A].Nodes.Add("SEMControllerConnect: " + this.PInfo.All.SEMControllerConnect);
                    TreeView1.Nodes[A].Nodes.Add("SEMStartReplyCheck: " + this.PInfo.All.SEMStartReplyCheck);
                    TreeView1.Nodes[A].Nodes.Add("SEMAlarmTime: " + this.PInfo.All.SEMAlarmTime);

                    TreeView1.Nodes[A].Nodes.Add("SVIDPLCNotReadLength: " + this.PInfo.All.SVIDPLCNotReadLength);

                    //TreeView1.Nodes[A].Nodes.Add("SizeWidth: " + this.PInfo.All.SizeWidth);
                    //TreeView1.Nodes[A].Nodes.Add("SizeHeight: " + this.PInfo.All.SizeHeight);
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


                            //LOTAPD Info
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("LOTAPD(기준정보)");
                            D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                            foreach (int intIndex in this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).LOTAPD())
                            {
                                E = E + 1;
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).LOTAPD(intIndex).Name);

                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Index: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).LOTAPD(intIndex).Index);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Name: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).LOTAPD(intIndex).Name);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Length: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).LOTAPD(intIndex).Length);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Format: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).LOTAPD(intIndex).Format);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Value: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).LOTAPD(intIndex).Value);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Type: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).LOTAPD(intIndex).Type);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).LOTAPD(intIndex).ModuleID);
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


                            //PMCode Info
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("PMCode");
                            D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                            foreach (int dintIndex in this.PInfo.Unit(0).SubUnit(0).PMCode())
                            {
                                E = E + 1;
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dintIndex.ToString());

                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("PMCode: " + this.PInfo.Unit(0).SubUnit(0).PMCode(dintIndex).PMCode);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Desc: " + this.PInfo.Unit(0).SubUnit(0).PMCode(dintIndex).Desc);
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
                            for (int dintIndex = 1, dintSvidCount = 0; dintSvidCount < this.PInfo.Unit(0).SubUnit(0).SVIDCount; dintIndex++)
                            {
                                if (this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex) != null)
                                {
                                    E = E + 1;
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dintIndex.ToString());

                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Name);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Length: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Length);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Format: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Format);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Value: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Value);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Type: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Type);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Unit: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Unit);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Range: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Range);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("HaveMinusValue: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).HaveMinusValue);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("DESC: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).DESC);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("UnitID: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).UnitID);
                                    TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).ModuleID);

                                    dintSvidCount++;
                                }
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
                                //TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Unit: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Unit);
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

                            //MCC Info
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add("MCC");
                            D = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].GetNodeCount(false) - 1;
                            for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).MCCCount; dintIndex++)
                            {
                                E = E + 1;
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes.Add(dintIndex.ToString());

                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).MCC(dintIndex).MCCName);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Type: " + this.PInfo.Unit(0).SubUnit(0).MCC(dintIndex).MCCType);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(0).SubUnit(0).MCC(dintIndex).ModuleID);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("Desc: " + this.PInfo.Unit(0).SubUnit(0).MCC(dintIndex).MCCDesc);
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

                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("GLSID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrGLS(dstrName).GLSID);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("LOTID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrGLS(dstrName).LOTID);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("LOTIndex: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrGLS(dstrName).LOTIndex);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D].Nodes[E - 1].Nodes.Add("SlotID: " + this.PInfo.Unit(dintUnitID).SubUnit(dintSubUnit).CurrGLS(dstrName).SlotID);
                            }
                            E = 0;  //초기화
                        }

                    }
                }
                A = A + 1;
                B = 0; C = 0; D = 0; E = 0; F = 0;

                //3. LOT Info
                TreeView1.Nodes.Add("03. LOT");
                foreach (int dintLOTIndex in this.PInfo.LOTIndex())
                {
                    B = B + 1;
                    TreeView1.Nodes[A].Nodes.Add("LOTID: " + this.PInfo.LOTIndex(dintLOTIndex).LOTID + ", LOTIndex: " + dintLOTIndex.ToString());

                    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("LOTIndex: " + this.PInfo.LOTIndex(dintLOTIndex).LOTIndex);
                    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("LOTID: " + this.PInfo.LOTIndex(dintLOTIndex).LOTID);
                    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("InCount: " + this.PInfo.LOTIndex(dintLOTIndex).InCount);
                    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("OutCount: " + this.PInfo.LOTIndex(dintLOTIndex).OutCount);
                    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("ScrapCount: " + this.PInfo.LOTIndex(dintLOTIndex).ScrapCount);
                    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("UnScrapCount: " + this.PInfo.LOTIndex(dintLOTIndex).UnScrapCount);
                    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("StartTime: " + this.PInfo.LOTIndex(dintLOTIndex).StartTime);
                    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("EndTime: " + this.PInfo.LOTIndex(dintLOTIndex).EndTime);
                
                    //Slot Info
                    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Slot");
                    C = TreeView1.Nodes[A].Nodes[B - 1].GetNodeCount(false) - 1;
                    for (int dintSlotID = 1; dintSlotID <= this.PInfo.EQP("Main").SlotCount; dintSlotID++)
                    {
                        D = D + 1;
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add(dintSlotID.ToString());

                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("LOTID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).LOTID);
                        //TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("LOTIndex: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).LOTIndex);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("SlotID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).SlotID);

                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("H_PANELID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).H_PANELID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("E_PANELID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).E_PANELID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("LOTID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).LOTID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("BATCHID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).BATCHID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("JOBID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).JOBID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("PORTID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).PORTID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("SLOTNO: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).SlotID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("PRODUCT_TYPE: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).PRODUCT_TYPE);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("PRODUCT_KIND: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).PRODUCT_KIND);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("PRODUCTID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).PRODUCTID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("RUNSPECID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).RUNSPECID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("LAYERID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).LAYERID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("STEPID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).STEPID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("PPID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).HOSTPPID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("FLOWID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).FLOWID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("SIZE: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).SIZE);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("THICKNESS: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).THICKNESS);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("GLASS_STATE: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLASS_STATE);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("GLASS_ORDER: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLASS_ORDER);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("COMMENT: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).COMMENT);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("USE_COUNT: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).USE_COUNT);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("JUDGEMENT: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).JUDGEMENT);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("REASON_CODE: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).REASON_CODE);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("INS_FLAG: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).INS_FLAG);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("ENC_FLAG: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).ENC_FLAG);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("PRERUN_FLAG: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).PRERUN_FLAG);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("TURN_DIR: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).TURN_DIR);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("FLIP_STATE: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).FLIP_STATE);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("WORK_STATE: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).WORK_STATE);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("MULTI_USE: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).MULTI_USE);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("PAIR_GLASSID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).PAIR_GLASSID);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("PAIR_PPID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).PAIR_PPID);

                        for (int dintLoop = 0; dintLoop <= this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).OPTION_NAME.Length - 1; dintLoop++)
                        {
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("OPTION_NAME" + dintLoop + 1 + ": "  + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).OPTION_NAME[dintLoop]);
                            TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("OPTION_VALUE" + dintLoop + 1 + ": " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).OPTION_VALUE[dintLoop]);
                        }

                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("JOBStart: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).JOBStart);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("JOBEnd: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).JOBEnd);

                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("Scrap: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).Scrap);
                        //TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("UnScrap: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).UnScrap);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("StartTime: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).StartTime);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("EndTime: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).EndTime);


                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("GLSAPD");

                        E = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].GetNodeCount(false) - 1;
                        for (int intIndex = 1; intIndex <= this.PInfo.Unit(0).SubUnit(0).GLSAPDCount; intIndex++)
                        {
                            if (this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLSAPD(intIndex) != null)
                            {
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes.Add(this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLSAPD(intIndex).Name);
                                F = TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].GetNodeCount(false) - 1;

                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes[F].Nodes.Add("Index: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLSAPD(intIndex).Index);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes[F].Nodes.Add("Length: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLSAPD(intIndex).Length);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes[F].Nodes.Add("Format: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLSAPD(intIndex).Format);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes[F].Nodes.Add("Value: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLSAPD(intIndex).Value);
                                //TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes[F].Nodes.Add("UnitID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLSAPD(intIndex).UnitID);
                                TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes[E].Nodes[F].Nodes.Add("ModuleID: " + this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLSAPD(intIndex).ModuleID);
                            }
                        }
                        E = 0;
                    }
                    D = 0;  //초기화

                    //LOTAPD Info
                    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("LOTAPD");
                    C = TreeView1.Nodes[A].Nodes[B - 1].GetNodeCount(false) - 1;
                    foreach (int intIndex in this.PInfo.LOTIndex(dintLOTIndex).LOTAPD())
                    {
                        D = D + 1;
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add(this.PInfo.LOTIndex(dintLOTIndex).LOTAPD(intIndex).Name);

                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("Index: " + this.PInfo.LOTIndex(dintLOTIndex).LOTAPD(intIndex).Index);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("Index: " + this.PInfo.LOTIndex(dintLOTIndex).LOTAPD(intIndex).Name);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("Length: " + this.PInfo.LOTIndex(dintLOTIndex).LOTAPD(intIndex).Length);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("Format: " + this.PInfo.LOTIndex(dintLOTIndex).LOTAPD(intIndex).Format);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("HaveMinusValue: " + this.PInfo.LOTIndex(dintLOTIndex).LOTAPD(intIndex).HaveMinusValue);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("Value: " + this.PInfo.LOTIndex(dintLOTIndex).LOTAPD(intIndex).Value);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("Type: " + this.PInfo.LOTIndex(dintLOTIndex).LOTAPD(intIndex).Type);
                        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("ModuleID: " + this.PInfo.LOTIndex(dintLOTIndex).LOTAPD(intIndex).ModuleID);
                    }
                    D = 0;  //초기화
                }
                A = A + 1;
                B = 0; C = 0; D = 0; E = 0; F = 0;


                //10. Trace
                TreeView1.Nodes.Add("10. Trace");
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


                //SVID 테스트용
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "------------------------------------------------------------------------------" + "\r\n");
                //for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).SVIDCount; dintLoop++)
                //{
                //    dstrTemp = dstrTemp +  FunStringH.funStringData(dintLoop.ToString() + "(" + this.PInfo.Unit(0).SubUnit(0).SVID(dintLoop).Name + ")" + ": ", 45) + this.PInfo.Unit(0).SubUnit(0).SVID(dintLoop).Value + "\r\n";
                //}
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrTemp);
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "------------------------------------------------------------------------------" + "\r\n");


                ////4. ECID Info
                //TreeView1.Nodes.Add("04. ECID");
                //for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).ECIDCount; dintIndex++)
                //{
                //    B = B + 1;
                //    TreeView1.Nodes[A].Nodes.Add(dintIndex.ToString());

                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Name);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Unit: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Unit);                   
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("ECSLL: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECSLL);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("ECWLL: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECWLL);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("ECDEF: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECDEF);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("ECWUL: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECWUL);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("ECSUL: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECSUL);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("DESC: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).DESC);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ModuleID);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("UnitID: " + this.PInfo.Unit(0).SubUnit(0).ECID(dintIndex).UnitID);
                //}
                ////초기화
                //A = A + 1;
                //B = 0; C = 0; D = 0; E = 0;


                ////5. SVID Info
                //TreeView1.Nodes.Add("05. SVID");
                //for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).SVIDCount; dintIndex++)
                //{
                //    B = B + 1;
                //    TreeView1.Nodes[A].Nodes.Add(dintIndex.ToString());

                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Name);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Length: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Length);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Format: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Format);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Value: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Value);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Type: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Type);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Unit: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Unit);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Range: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Range);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("DESC: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).DESC);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("UnitID: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).UnitID);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).ModuleID);
                //}
                ////초기화
                //A = A + 1;
                //B = 0; C = 0; D = 0; E = 0;

                ////6. EOID Info
                //TreeView1.Nodes.Add("06. EOID");
                //for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).EOIDCount; dintIndex++)
                //{
                //    B = B + 1;
                //    TreeView1.Nodes[A].Nodes.Add(dintIndex.ToString());

                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("EOID: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("EOMD: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMD);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("EOV: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("DESC: " + this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).DESC);
                //}
                ////초기화
                //A = A + 1;
                //B = 0; C = 0; D = 0; E = 0;

                ////7. PPIDBody Info
                //TreeView1.Nodes.Add("07. PPIDBody");
                //for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintIndex++)
                //{
                //    B = B + 1;
                //    TreeView1.Nodes[A].Nodes.Add(dintIndex.ToString());

                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Name);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Length: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Length);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Min: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Min);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Max: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Max);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Format: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Format);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Unit: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Unit);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Range: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Range);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("DESC: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).DESC);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Value: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Value);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("UnitID: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).UnitID);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("ModuleID: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).ModuleID);
                //}
                ////초기화
                //A = A + 1;
                //B = 0; C = 0; D = 0; E = 0;



                ////8. EQPPPID Info
                //TreeView1.Nodes.Add("08. EQPPPID");
                //foreach (string dstrName in this.PInfo.Unit(0).SubUnit(0).EQPPPID())
                //{
                //    B = B + 1;
                //    TreeView1.Nodes[A].Nodes.Add(dstrName);

                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("PPIDVer: " + this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDVer);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("DateTime: " + this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).DateTime);


                //    //EQPPPID별 PPIDBody
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("PPIDBody");
                //    C = TreeView1.Nodes[A].Nodes[B - 1].GetNodeCount(false) - 1;
                //    for (int dintIndex = 1; dintIndex <= this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDBodyCount; dintIndex++)
                //    {
                //        D = D + 1;
                //        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes.Add(dintIndex.ToString());

                //        //TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Name);     //기준정보에 있는 값
                //        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("Name: " + this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDBody(dintIndex).Name);       //실제저장한 값
                //        TreeView1.Nodes[A].Nodes[B - 1].Nodes[C].Nodes[D - 1].Nodes.Add("Value: " + this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDBody(dintIndex).Value);     //실제저장할 값
                //    }
                //    D = 0;      //초기화
                //}
                ////초기화
                //A = A + 1;
                //B = 0; C = 0; D = 0; E = 0;


                ////9. PMCode Info
                //TreeView1.Nodes.Add("09. PMCode");
                //foreach (int dintIndex in this.PInfo.Unit(0).SubUnit(0).PMCode())
                //{
                //    B = B + 1;
                //    TreeView1.Nodes[A].Nodes.Add(dintIndex.ToString());

                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("PMCode: " + this.PInfo.Unit(0).SubUnit(0).PMCode(dintIndex).PMCode);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Desc: " + this.PInfo.Unit(0).SubUnit(0).PMCode(dintIndex).Desc);
                //}
                ////초기화
                //A = A + 1;
                //B = 0; C = 0; D = 0; E = 0;


                ////11. UserLevel Info
                //TreeView1.Nodes.Add("11. UserLevel");
                //foreach (int dintIndex in this.PInfo.Unit(0).SubUnit(0).UserLevel())
                //{
                //    B = B + 1;
                //    TreeView1.Nodes[A].Nodes.Add(dintIndex.ToString());

                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("UserLevel: " + dintIndex.ToString());
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Desc: " + this.PInfo.Unit(0).SubUnit(0).UserLevel(dintIndex).Desc);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Comment: " + this.PInfo.Unit(0).SubUnit(0).UserLevel(dintIndex).Comment);
                //}
                ////초기화
                //A = A + 1;
                //B = 0; C = 0; D = 0; E = 0;


                ////12. User Info
                //TreeView1.Nodes.Add("12. User");
                //foreach (string dstrUserID in this.PInfo.Unit(0).SubUnit(0).User())
                //{
                //    B = B + 1;
                //    TreeView1.Nodes[A].Nodes.Add(dstrUserID);

                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("dstrUserID: " + dstrUserID);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Level: " + this.PInfo.Unit(0).SubUnit(0).User(dstrUserID).Level);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("PassWord: " + this.PInfo.Unit(0).SubUnit(0).User(dstrUserID).PassWord);
                //    TreeView1.Nodes[A].Nodes[B - 1].Nodes.Add("Desc: " + this.PInfo.Unit(0).SubUnit(0).User(dstrUserID).Desc);
                //}
                ////초기화
                //A = A + 1;
                //B = 0; C = 0; D = 0; E = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}