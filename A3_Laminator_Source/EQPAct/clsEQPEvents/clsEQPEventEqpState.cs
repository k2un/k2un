using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventEqpState : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventEqpState(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actEQPState";
        }
        #endregion


        #region Methods
        /// <summary>
        /// 설비에서 CIM으로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : strCompBit
        /// parameters[1] : dstrACTVal
        /// parameters[2] : dintActFrom
        /// parameters[3] : dstrACTFromSub
        /// parameters[4] : intBitVal
        /// parameters[5] : Special Parameter
        /// </remarks>
        public void funProcessEQPEvent(string[] parameters)
        {
            int intUnitID = Convert.ToInt32(parameters[2]);
            string strEQPState = parameters[1];
            int dintSubUnitID = Convert.ToInt32(parameters[3]);
            int intBitVal = Convert.ToInt32(parameters[4]);

            if (intBitVal != 1) return;

            string dstrEQPStateBackup = "";
            string dstrWordAddress = "";
            Boolean dbolFlag = false;
            //int dintTemp = 0;

            try
            {
                //이전 상태를 백업
                dstrEQPStateBackup = this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPState;

                this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateOLD = dstrEQPStateBackup;     //이전상태
                this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPState = strEQPState;               //현재상태
                string strEQPStateChangeBYWHO = m_pEqpAct.funWordRead("W2011", 1, EnuEQP.PLCRWType.Int_Data).ToString();

                if (intUnitID == 0)
                {
                    pInfo.Unit(3).SubUnit(0).EQPStateOLD = pInfo.Unit(3).SubUnit(0).EQPState;     //이전상태
                    pInfo.Unit(3).SubUnit(0).EQPState = strEQPState;               //현재상태
                    pInfo.Unit(3).SubUnit(0).EQPStateChangeBYWHO = strEQPStateChangeBYWHO;
                }




                //HOST나 OP가 이벤트를 발생시키지 않았다면 장비가 발생시킨것임.
                if (this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateChangeBYWHO == "1") //|| this.pInfo.Unit(intUnitID).SubUnit(0).EQPStateChangeBYWHO == "2")
                {
                    //HOST나 OP에서 발생한것임
                }
                else
                {
                    //this.pInfo.Unit(intUnitID).SubUnit(0).EQPStateChangeBYWHO = "3;   //By Equipment

                    this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateChangeBYWHO = strEQPStateChangeBYWHO;

                    //HOST, CIM으로 부터 Normal이 왔는데 장비가 Normal 상태로 갈 수가 없으면 Fault로 간다.
                    //그럼 이 때 By Who 설정
                    if (this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateLastCommand == "1") //Normal이 왔음을 저장
                    {
                        //이전상태가 PM이고 HOST, CIM에서 Normal이 왔는데 중도 알람이 발생되 있어서 Normal로 전환 안되고 Fault로 전환된 경우
                        if (dstrEQPStateBackup == "3" && strEQPState != "3")
                        {
                            dbolFlag = true;        //초기화 해야함을 저장
                            this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subACTEQPState(Case 1): intUnitID: " + intUnitID.ToString() +
                                                ", dstrEQPStateBackup: " + dstrEQPStateBackup + ", strEQPState:" + strEQPState);
                        }
                    }
                    else
                    {
                        //this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateChangeBYWHO = "3";   //By Equipment

                        //장비 T/P에서 PM 변경시 PLC에서 PM Code를 읽는다.
                        //PM 관련 변경은 전체장비상태(Unit 0)에서만 일어날때 보고함.
                        if (strEQPState == "3")
                        {
                            //PM Code를 읽어 4자리로 맞춘다.(예: 0016)
                            //this.pInfo.All.PMCode = FunStringH.funMakeLengthStringFirst(m_pEqpAct.funWordRead(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data).Trim(), 4);
                        }
                    }
                }

                foreach (int dintAlarmID in this.pInfo.Unit(0).SubUnit(0).CurrAlarm())
                {
                    if (this.pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).ModuleID == this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).ModuleID)
                    {
                        this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).AlarmID = dintAlarmID;
                        break;
                    }
                }

                //이전상태와 현재상태가 변경이 되었으면 HOST로 보고
                if (dstrEQPStateBackup != strEQPState)
                {
                    if (intUnitID == 0)   //Layer1 보고
                    {
                        //HOST로 전체장비상태 보고
                        this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 53, 3, 0);                 //EQ(MODULE) STATE CHANGED
                        //this.pInfo.Unit(0).SubUnit(0).EQPStateChangeBYWHO = ""; //초기화 어경태
                        this.pInfo.Unit(intUnitID).SubUnit(0).EQPStateChangeBYWHO = "";
                    }
                    else
                    {
                        this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 53, intUnitID, dintSubUnitID);     //EQ(MODULE) STATE CHANGED
                        this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateChangeBYWHO = ""; //초기화 어경태
                    }
                }


                if (this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateLastCommand == this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPState)
                {
                    this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateChangeBYWHO = "";         //By Equipment로 초기화
                    this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateLastCommand = "";         //초기화
                }
                else if (dbolFlag == true)
                {
                    this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateChangeBYWHO = "";         //By Equipment로 초기화
                    this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPStateLastCommand = "";         //초기화
                }

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intUnitID:" + intUnitID + ", dintSubUnitID:" + dintSubUnitID + ", strEQPState:" + strEQPState);
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
