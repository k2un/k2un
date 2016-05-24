using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F41_JobCMD : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F41_JobCMD Instance = new clsS2F41_JobCMD();
        #endregion

        #region Constructors
        public clsS2F41_JobCMD()
        {
            this.IntStream = 2;
            this.IntFunction = 41;
            this.StrPrimaryMsgName = "S2F41MATERIALCMD";
            this.StrSecondaryMsgName = "S2F42M_CMDREPLY";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrModuleID = "";
            string dstrRCMD = "";
            int dintHCACK = 0;
            int dintCPACK = 0;
            string dstrData = "";
            Boolean dbolLayerExist = false;
            int dintModuleCount = 0;

            string dstrIPID = "";
            string dstrICID = "";
            string dstrOPID = "";
            string dstrOCID = "";
            string dstrSTIF = "";
            string dstrTempPortID = "";
            string dstrTempCSTID = "";
            string dstrHostGLSMapping10 = "";
            string dstrGLSProcessMapping10 = "";
            string dstrJOBID = "";      //A2 추가됨

            int dintCPACK1 = 0;
            int dintCPACK2 = 0;
            int dintCPACK3 = 0;
            int dintCPACK4 = 0;
            int dintCPACK5 = 0;
            int dintCPACK6 = 0;
            int dintCPACK7 = 0;
            int dintTemp = 0;
            //int dintTempSlotNo = 0;
            int dintLOTIndex = 0;
            int dintPortID = 0;
            int dintSlotID = 0;
            int dintProcGLSCount = 0;
            string strPortType = "";
            string strSlotInfo = "";
            try
            {
                dstrRCMD = msgTran.Primary().Item("RCMD").Getvalue().ToString();
                dintModuleCount = Convert.ToInt32(msgTran.Primary().Item("MODULECOUNT").Getvalue());
                //dstrModuleID = msgTran.Primary().Item("MODULEID" + 0).Getvalue().ToString().Trim();

                //dstrHostGLSMapping10 = "0".PadRight(56, '0');
                //dstrGLSProcessMapping10 = "0".PadRight(56, '0');

                dstrJOBID = msgTran.Primary().Item("JOBID").Getvalue().ToString().Trim();
                dstrIPID = msgTran.Primary().Item("IPID").Getvalue().ToString().Trim();
                dstrICID = msgTran.Primary().Item("ICID").Getvalue().ToString().Trim();
                dstrOPID = msgTran.Primary().Item("OPID").Getvalue().ToString().Trim();
                dstrOCID = msgTran.Primary().Item("OCID").Getvalue().ToString().Trim();
                strSlotInfo = msgTran.Primary().Item("SLOTINFO").Getvalue().ToString().Trim();
                dintProcGLSCount = Convert.ToInt32(msgTran.Primary().Item("SLOTNO").Getvalue().ToString());
                msgTran.Secondary2(strSecondaryMsgName).Item("SLOTNO").Putvalue(dintProcGLSCount);

                for (int dintLoop = 1; dintLoop <= dintProcGLSCount; dintLoop++)
                {
                    dintSlotID = Convert.ToInt32(msgTran.Primary().Item("SLOT_NO1" + (dintLoop - 1)).Getvalue().ToString());
                    msgTran.Secondary2(strSecondaryMsgName).Item("SLOT_NO1" + (dintLoop - 1)).Putvalue(dintSlotID);

                }

                //RCMD가 존재하는 경우
                if (dstrRCMD == "1001" || dstrRCMD == "1002" ) //|| dstrRCMD == "3" ) //|| dstrRCMD == "30")
                {

                    for (int dintLoop = 1; dintLoop <= pInfo.PortCount; dintLoop++)
                    {
                        InfoAct.clsPort CurrentPort = pInfo.Port(dintLoop);
                        if (CurrentPort.HostReportPortID == dstrIPID || CurrentPort.HostReportPortID == dstrOPID)
                        {
                            dintPortID = dintLoop;
                            break;
                        }
                    }
                    dbolLayerExist = true;

                    //ModuleID가 존재하지 않는 경우
                    if (dbolLayerExist == false)
                    {
                        dintHCACK = 3;          //3 = At least one parameter invalid
                        dintCPACK2 = 2;          //2 = Illegal Value specified for CPVAL
                    }
                    else
                    {
                        if (dintHCACK == 0)
                        {
                            switch (dstrRCMD)
                            {
                                case "1001":   //Job Process Start
                                    break;
                                case "1030":
                                    //Online Local인데 Start명령이 온 경우
                                    if (this.pInfo.All.ControlState == "2")
                                    {
                                        dintHCACK = 7;           //7 = Control State is Online Local. Equipment rejects command.
                                    }
                                    //이미 Start명령을 받았으면
                                    else if (dstrRCMD == "1" && this.pInfo.Port(dintPortID).S2F41StartReceived == true)
                                    {
                                        dintHCACK = 5;           //5 = Rejected, Already in desired Condition
                                    }
                                    //S3F101(CST Info)을 받지않고 바로 Start 명령을 받았을 경우
                                    else if (dstrRCMD == "1" && this.pInfo.Port(dintPortID).S3F101Received == false)
                                    {
                                        dintHCACK = 13;           //13 = Rejected, Eqp doesn't receive CASSETTE INFO
                                    }
                                    else if (this.pInfo.Port(dintPortID).PortState == "0")
                                    {
                                        if (this.pInfo.Port(dintPortID).PortType == "1")   //Both, Input
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK2 = 4;          //4 = No cassette (input port)
                                        }
                                        else if (this.pInfo.Port(dintPortID).PortType == "2")   //Output
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK4 = 5;          //5 = No cassette (output port)
                                        }
                                        else
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK2 = 4;          //4 = No cassette (input port)
                                        }
                                    }
                                    else if ((this.pInfo.Port(dintPortID).CSTID.Trim() != dstrICID.Trim()) == false && (this.pInfo.Port(dintPortID).CSTID.Trim() != dstrOCID.Trim()) == false)
                                    {
                                        if (this.pInfo.Port(dintPortID).PortType == "1")   //Both, Input
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK3 = 6;          //6 = Cassette ID not match (input port)
                                        }
                                        else if (this.pInfo.Port(dintPortID).PortType == "2")   //Output
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK5 = 7;          //7 = Cassette ID not match (output port)
                                        }
                                        else
                                        {
                                            //dintHCACK = 3;           //3 = At least one parameter invalid
                                            //dintCPACK3 = 6;          //6 = Cassette ID not match (input port)
                                        }
                                    }
                                    //Port가 2(Wait)가 아닐때
                                    else if (this.pInfo.Port(dintPortID).PortState != "2")
                                    {

                                        if (this.pInfo.Port(dintPortID).PortType == "1")   //Both, Input
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK3 = 8;          //8 = Port state error (input port)
                                        }
                                        else if (this.pInfo.Port(dintPortID).PortType == "2")   //Output
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK5 = 9;          //9 = Port state error (output port)
                                        }
                                        else
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK3 = 8;          //8 = Port state error (input port)
                                        }
                                    }
                                    //////S3F101로 내려준 작업할 정보(MAP_STIF)와 S2F41(STIF)에서 내려준 정보가 다를때
                                    ////else if (this.PInfo.Port(dintPortID).GLSCIMMapping10 != dstrSTIF)
                                    ////{
                                    ////    dintHCACK = 3;              //3 = At least one parameter invalid
                                    ////    dintCPACK5 = 2;             //2 = Illegal Value specified for CPVAL
                                    ////}
                                    else
                                    {
                                        //dintLOTIndex = this.pInfo.Port(dintPortID).LOTIndex;

                                        //HOST에서 받은 정보를 구성
                                        //S3F101에서 받은 Slot별 정보를 초기화한다.
                                        //for (int dintLoop = 1; dintLoop <= this.pInfo.EQP("Main").SlotCount; dintLoop++)
                                        //{
                                        //    this.pInfo.Port(dintPortID).Slot(dintLoop).DExist = "0";

                                        //    this.pInfo.Port(dintPortID).Slot(dintLoop).HExist = "0";

                                        //    if (this.pInfo.Port(dintPortID).Slot(dintLoop).MExist == "1")
                                        //    {
                                        //        this.pInfo.Port(dintPortID).Slot(dintLoop).GLASS_STATE = 1;           //1(Idle)
                                        //    }
                                        //}

                                        this.pInfo.Port(dintPortID).GLSProcessTotalCnt = 0;

                                        #region 이전소스
                                        //if (strSlotInfo.Substring(0,pInfo.Port(dintPortID).GLSLoaderMapping10.Length)  != pInfo.Port(dintPortID).GLSLoaderMapping10)
                                        //{
                                        //    dintHCACK = 3;              //3 = At least one parameter invalid
                                        //    dintCPACK6 = 2;             //2 = Illegal Value specified for CPVAL
                                        //    break;
                                        //}

                                        //S2F41에서 받은 정보로 다시 구성한다.

                                        //if (dintProcGLSCount == 0)     //전수임
                                        //{
                                        //    for (int dintLoop = 1; dintLoop <= this.pInfo.EQP("Main").SlotCount; dintLoop++)
                                        //    {
                                        //        if (this.pInfo.Port(dintPortID).Slot(dintLoop).MExist == "1")      //실물이 있는 경우
                                        //        {
                                        //            if (dstrSTIF.Substring(dintLoop - 1, 1) == "1")                 //실물이 있고 작업할 Slot
                                        //            {
                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).DExist = "1";
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).DExist = "1";

                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).HExist = "1";
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).HExist = "1";

                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).GLASS_STATE = 2;     //2(Selected to Process)
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).GLASS_STATE = 2;            //2(Selected to Process)

                                        //                dstrHostGLSMapping10 = dstrHostGLSMapping10.Substring(0, dintLoop - 1) + "1" + dstrHostGLSMapping10.Substring(dintLoop);
                                        //                dstrGLSProcessMapping10 = dstrGLSProcessMapping10.Substring(0, dintLoop - 1) + "1" + dstrGLSProcessMapping10.Substring(dintLoop);
                                        //            }
                                        //            else           //실물은 있고 작업을 하지 않을 경우
                                        //            {
                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).DExist = "0";
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).DExist = "0";

                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).HExist = "0";
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).HExist = "0";

                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).GLASS_STATE = 1;   //1(Idle)
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).GLASS_STATE = 1;

                                        //                dstrHostGLSMapping10 = dstrHostGLSMapping10.Substring(0, dintLoop - 1) + "0" + dstrHostGLSMapping10.Substring(dintLoop);
                                        //                dstrGLSProcessMapping10 = dstrGLSProcessMapping10.Substring(0, dintLoop - 1) + "0" + dstrGLSProcessMapping10.Substring(dintLoop);
                                        //            }
                                        //        }
                                        //        else   //실물이 없는 경우
                                        //        {
                                        //            if (dstrSTIF.Substring(dintLoop - 1, 1) == "1")                 //실물이 없는데 작업하라는 경우
                                        //            {
                                        //                //여기는 수행하지 않음(여기로 들어오면 Validation Error임).

                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).DExist = "1";
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).DExist = "1";

                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).HExist = "1";
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).HExist = "1";

                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).GLASS_STATE = 1;     //2(Selected to Process)
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).GLASS_STATE = 1;            //2(Selected to Process)

                                        //                dstrHostGLSMapping10 = dstrHostGLSMapping10.Substring(0, dintLoop - 1) + "1" + dstrHostGLSMapping10.Substring(dintLoop);
                                        //                dstrGLSProcessMapping10 = dstrGLSProcessMapping10.Substring(0, dintLoop - 1) + "1" + dstrGLSProcessMapping10.Substring(dintLoop);

                                        //                dintHCACK = 3;              //3 = At least one parameter invalid
                                        //                dintCPACK7 = 2;             //2 = Illegal Value specified for CPVAL
                                        //            }
                                        //            else           //실물도 없고 작업을 하지 않을 경우
                                        //            {
                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).DExist = "0";
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).DExist = "0";

                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).HExist = "0";
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).HExist = "0";

                                        //                //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintLoop).GLASS_STATE = 0;   //No State
                                        //                this.pInfo.Port(dintPortID).Slot(dintLoop).GLASS_STATE = 0;          //No State

                                        //                dstrHostGLSMapping10 = dstrHostGLSMapping10.Substring(0, dintLoop - 1) + "0" + dstrHostGLSMapping10.Substring(dintLoop);
                                        //                dstrGLSProcessMapping10 = dstrGLSProcessMapping10.Substring(0, dintLoop - 1) + "0" + dstrGLSProcessMapping10.Substring(dintLoop);
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        //else    //전수가 아닌경우
                                        //{
                                        //    for (int dintLoop = 1; dintLoop <= dintProcGLSCount; dintLoop++)
                                        //    {
                                        //        dintSlotID = Convert.ToInt32(msgTran.Primary().Item("SLOT_NO" + (dintLoop - 1)).Getvalue().ToString());

                                        //        if (dintSlotID > pInfo.EQP("Main").SlotCount || dintSlotID <= 0)
                                        //        {
                                        //            dintHCACK = 3;           //3 = At least one parameter invalid
                                        //            dintCPACK7 = 2;
                                        //            break;
                                        //        }
                                        //        if (this.pInfo.Port(dintPortID).Slot(dintSlotID).MExist == "1")      //실물이 있고 작업을 하는 경우
                                        //        {
                                        //            //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).DExist = "1";
                                        //            this.pInfo.Port(dintPortID).Slot(dintSlotID).DExist = "1";

                                        //            //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).HExist = "1";
                                        //            this.pInfo.Port(dintPortID).Slot(dintSlotID).HExist = "1";

                                        //            //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLASS_STATE = 2;     //2(Selected to Process)
                                        //            this.pInfo.Port(dintPortID).Slot(dintSlotID).GLASS_STATE = 2;            //2(Selected to Process)

                                        //            dstrHostGLSMapping10 = dstrHostGLSMapping10.Substring(0, dintSlotID - 1) + "1" + dstrHostGLSMapping10.Substring(dintSlotID);
                                        //            dstrGLSProcessMapping10 = dstrGLSProcessMapping10.Substring(0, dintSlotID - 1) + "1" + dstrGLSProcessMapping10.Substring(dintSlotID);
                                        //        }
                                        //        else   //실물이 없는데 작업을 하라는 경우
                                        //        {
                                        //            //여기는 수행하지 않음(여기로 들어오면 Validation Error임).
                                        //            //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).DExist = "0";
                                        //            this.pInfo.Port(dintPortID).Slot(dintSlotID).DExist = "0";

                                        //            //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).HExist = "0";
                                        //            this.pInfo.Port(dintPortID).Slot(dintSlotID).HExist = "0";

                                        //            //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintSlotID).GLASS_STATE = 0;
                                        //            this.pInfo.Port(dintPortID).Slot(dintSlotID).GLASS_STATE = 0;

                                        //            dstrHostGLSMapping10 = dstrHostGLSMapping10.Substring(0, dintSlotID - 1) + "0" + dstrHostGLSMapping10.Substring(dintSlotID);
                                        //            dstrGLSProcessMapping10 = dstrGLSProcessMapping10.Substring(0, dintSlotID - 1) + "0" + dstrGLSProcessMapping10.Substring(dintSlotID);

                                        //            dintHCACK = 3;              //3 = At least one parameter invalid
                                        //            dintCPACK7 = 2;             //2 = Illegal Value specified for CPVAL
                                        //        }
                                        //    }
                                        //}

                                        ////HOST에서 받은 정보를 재저장한다.
                                        //this.pInfo.Port(dintPortID).GLSHostMapping10 = dstrHostGLSMapping10;
                                        ////this.PInfo.LOTIndex(dintLOTIndex).GLSHostMapping10 = dstrHostGLSMapping10;

                                        //this.pInfo.Port(dintPortID).GLSProcessMapping10 = dstrGLSProcessMapping10;
                                        ////this.PInfo.LOTIndex(dintLOTIndex).GLSProcessMapping10 = dstrGLSProcessMapping10;

                                        //this.pInfo.Port(dintPortID).GLSProcessTotalCnt = FunStringH.funStringCnt(dstrGLSProcessMapping10, "1");
                                        ////this.PInfo.LOTIndex(dintLOTIndex).GLSProcessTotalCnt = FunStringH.funStringCnt(dstrGLSProcessMapping10, "1");

                                        ////this.PInfo.LOTIndex(dintLOTIndex).GLSRealMapping10 = this.PInfo.Port(dintPortID).GLSRealMapping10;
                                        ////this.PInfo.LOTIndex(dintLOTIndex).GLSRealMappingCnt = this.PInfo.Port(intPortID).GLSRealMappingCnt;

                                        ////작업할 글래스가 한장인지여부 체크
                                        ////string dstrTempMapping = "";
                                        ////for (int dintBack = 27; dintBack >= 0; dintBack--)
                                        ////{
                                        ////    dstrTempMapping += dstrGLSProcessMapping10.Substring(dintBack, 1);
                                        ////}
                                        ////dstrGLSProcessMapping10 = dstrTempMapping;

                                        //if (this.pInfo.Port(dintPortID).GLSProcessTotalCnt == 1)
                                        //{
                                        //    this.pInfo.Port(dintPortID).SingleJOB = true;
                                        //}
                                        //else
                                        //{
                                        //    this.pInfo.Port(dintPortID).SingleJOB = false;

                                        //    //작업할 첫글래스 여부저장
                                        //    int dintTempSlot = 1;
                                        //    dintTempSlot = dstrGLSProcessMapping10.IndexOf("1") + 1;
                                        //    this.pInfo.Port(dintPortID).Slot(dintTempSlot).JOBStart = true;
                                        //    //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintTempSlot).JOBStart = true;

                                        //    //작업할 마지막글래스 여부저장
                                        //    dintTempSlot = dstrGLSProcessMapping10.LastIndexOf("1") + 1;
                                        //    this.pInfo.Port(dintPortID).Slot(dintTempSlot).JOBEnd = true;
                                        //    //this.PInfo.LOTIndex(dintLOTIndex).Slot(dintTempSlot).JOBEnd = true;
                                        //}
                                        #endregion
                                    }
                                    break;

                                case "1002":   //Job Process End(Cancel)
                                    if (!(this.pInfo.Port(dintPortID).PortState == "1" || this.pInfo.Port(dintPortID).PortState == "2" || this.pInfo.Port(dintPortID).PortState == "3"))
                                    {
                                        if (this.pInfo.Port(dintPortID).PortType == "0" || this.pInfo.Port(dintPortID).PortType == "1")       //Both, Input
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK2 = 8;          //8 = Port state error (input port)
                                        }
                                        else       //Output
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK4 = 9;          //9 = Port state error (output port)
                                        }
                                    }
                                    else if (this.pInfo.Port(dintPortID).CSTID != dstrICID)   //CSTID가 다를때
                                    {
                                        if (this.pInfo.Port(dintPortID).PortType == "0" || this.pInfo.Port(dintPortID).PortType == "1")   //Both, Input
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK3 = 6;          //6 = Cassette ID not match (input port)
                                        }
                                        else       //Output
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK5 = 7;          //7 = Cassette ID not match (output port)
                                        }
                                    }

                                    break;

                                case "3":   //Job Process Abort
                                    if (this.pInfo.Port(dintPortID).PortState != "4")
                                    {
                                        if (this.pInfo.Port(dintPortID).PortType == "0" || this.pInfo.Port(dintPortID).PortType == "1")       //Both, Input
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK2 = 8;          //8 = Port state error (input port)
                                        }
                                        else       //Output
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK4 = 9;          //9 = Port state error (output port)
                                        }
                                    }
                                    else if (this.pInfo.Port(dintPortID).CSTID != dstrICID)   //CSTID가 다를때
                                    {
                                        if (this.pInfo.Port(dintPortID).PortType == "0" || this.pInfo.Port(dintPortID).PortType == "1")    //Both, Input
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK3 = 6;          //6 = Cassette ID not match (input port)
                                        }
                                        else       //Output
                                        {
                                            dintHCACK = 3;           //3 = At least one parameter invalid
                                            dintCPACK5 = 7;          //7 = Cassette ID not match (output port)
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    if (dintHCACK == 0)
                    {
                        //여기까지 오면 정상임
                        switch (dstrRCMD)
                        {
                            case "1001":   //Material Job Process Start
                                this.pInfo.Port(dintPortID).S2F41StartReceived = true;      //HOST로 부터 S2F41(Start) 받았음을 저장
                                //[2015/06/23]FI02에서만 보고됨(Add by HS)
                                if (dintPortID == 2)
                                {
                                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.FilmJobCommand, 1001, dintPortID);     //Lot Start 지시
                                }
                                break;
                            case "30":
                                this.pInfo.Port(dintPortID).S2F41StartReceived = true;      //HOST로 부터 S2F41(Start) 받았음을 저장
                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.FilmJobCommand, 1001, dintPortID);     //Lot Start 지시
                                break;

                            case "1002":   //Job Process End(Cancel)
                                this.pInfo.Port(dintPortID).RelatedJOBProcessEventChangeBYWHO = "1";   //By HOST

                                //PLC로 Cancel 명령을 써준다.
                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.FilmJobCommand, 1002, dintPortID);     //Lot Start 지시
                                pInfo.Port(dintPortID).CancelFlag = true;
                                break;

                            case "3":   //Job Process Abort
                                this.pInfo.Port(dintPortID).RelatedJOBProcessEventChangeBYWHO = "1";   //By HOST

                                this.pInfo.Port(dintPortID).EndCode = "3";
                                this.pInfo.Port(dintPortID).LOTStatus = "A";
                                pInfo.Port(dintPortID).AbortFlag = true;

                                //Unload Port일경우는 바로 Cancel(Abort) 명령을 써준다.
                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.FilmJobCommand, 2, dintPortID);     //Lot Start 지시
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedJobProcess, 3, 0, 0, pInfo.Port(dintPortID).CSTID, dintPortID, "");
                                break;
                        }
                    }
                }
                else   //RCMD가 존재하지 않는 경우
                {
                    dintHCACK = 1;          //1 = Command does not exist
                    dintCPACK2 = 2;         //2 = Illegal Value specified for CPVAL
                }

                //HOST로 S2F42 보고
                msgTran.Secondary2(strSecondaryMsgName).Item("RCMD").Putvalue(dstrRCMD);
                msgTran.Secondary2(strSecondaryMsgName).Item("JOBID1").Putvalue("JOBID");
                msgTran.Secondary2(strSecondaryMsgName).Item("JOBID").Putvalue(dstrJOBID);
                msgTran.Secondary2(strSecondaryMsgName).Item("IPID1").Putvalue("IPID");
                msgTran.Secondary2(strSecondaryMsgName).Item("IPID").Putvalue(dstrIPID);
                msgTran.Secondary2(strSecondaryMsgName).Item("ICID1").Putvalue("ICID");
                msgTran.Secondary2(strSecondaryMsgName).Item("ICID").Putvalue(dstrICID);
                msgTran.Secondary2(strSecondaryMsgName).Item("OPID1").Putvalue("OPID");
                msgTran.Secondary2(strSecondaryMsgName).Item("OPID").Putvalue(dstrOPID);
                msgTran.Secondary2(strSecondaryMsgName).Item("OCID1").Putvalue("OCID");
                msgTran.Secondary2(strSecondaryMsgName).Item("OCID").Putvalue(dstrOCID);
                msgTran.Secondary2(strSecondaryMsgName).Item("SLOTINFO1").Putvalue("SLOTINFO");
                msgTran.Secondary2(strSecondaryMsgName).Item("SLOTINFO").Putvalue(strSlotInfo);
                msgTran.Secondary2(strSecondaryMsgName).Item("ORDER").Putvalue("ORDER");
                msgTran.Secondary2(strSecondaryMsgName).Item("SLOTNO").Putvalue(dintProcGLSCount);

                msgTran.Secondary2(strSecondaryMsgName).Item("HCACK").Putvalue(dintHCACK);
                msgTran.Secondary2(strSecondaryMsgName).Item("CPACK1").Putvalue(dintCPACK1);
                msgTran.Secondary2(strSecondaryMsgName).Item("CPACK2").Putvalue(dintCPACK2);
                msgTran.Secondary2(strSecondaryMsgName).Item("CPACK3").Putvalue(dintCPACK3);
                msgTran.Secondary2(strSecondaryMsgName).Item("CPACK4").Putvalue(dintCPACK4);
                msgTran.Secondary2(strSecondaryMsgName).Item("CPACK5").Putvalue(dintCPACK5);
                msgTran.Secondary2(strSecondaryMsgName).Item("CPACK6").Putvalue(dintCPACK6);
                msgTran.Secondary2(strSecondaryMsgName).Item("CPACK7").Putvalue(dintCPACK7);

                funSendReply2(msgTran, strSecondaryMsgName);
            }
            catch (Exception error)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return;
            }
        }

        /// <summary>
        /// Primary Message를 Biuld하여 Transaction을 Return한다.
        /// </summary>
        /// <param name="strParameters">Parameter 문자열</param>
        public Transaction funPrimarySend(string strParameters)
        {
            string[] arrayEvent;
            try
            {
                arrayEvent = strParameters.Split(',');

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                return pMsgTran;
            }
            catch (Exception error)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return null;
            }
        }

        /// <summary>
        /// Secondary Message 수신에 대해 처리한다.
        /// </summary>
        /// <param name="msgTran">Secondary Message의 Transaction</param>
        public void funSecondaryReceive(Transaction msgTran)
        {
            try
            {

            }
            catch (Exception error)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }
        #endregion
    }
}
