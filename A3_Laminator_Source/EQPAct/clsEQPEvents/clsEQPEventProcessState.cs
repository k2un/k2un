using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventEqpProcessState : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventEqpProcessState(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actEQPProcessState";
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

            string strEQPProcessState = parameters[1].ToString();
            int intUnitID = Convert.ToInt32(parameters[2]);
            int intBitVal = Convert.ToInt32(parameters[4]);
            int dintSubUnitID = Convert.ToInt32(parameters[3]);
            if (intBitVal != 1) return;

            string dstrEQPProcessStateBackup = "";
            Boolean dbolFlag = false;
            //int dintTemp = 0;

            try
            {
                //이전 상태를 백업
                dstrEQPProcessStateBackup = this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessState;

                this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateOLD = dstrEQPProcessStateBackup;
                this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessState = strEQPProcessState;

                string strEQPStateChangeBYWHO = m_pEqpAct.funWordRead("W2011", 1, EnuEQP.PLCRWType.Int_Data).ToString();

                if (intUnitID == 0)
                {
                    pInfo.Unit(3).SubUnit(0).EQPProcessStateOLD = dstrEQPProcessStateBackup;
                    pInfo.Unit(3).SubUnit(0).EQPProcessState = strEQPProcessState;
                    pInfo.Unit(3).SubUnit(0).EQPProcessStateChangeBYWHO = strEQPStateChangeBYWHO;
                }

                this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateChangeBYWHO = strEQPStateChangeBYWHO;

                //HOST나 OP가 이벤트를 발생시키지 않았다면 장비가 발생시킨것임.
                if (this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateChangeBYWHO == "1") // || this.pInfo.Unit(intUnitID).SubUnit(0).EQPProcessStateChangeBYWHO == "2")
                {
                    //HOST나 OP에서 발생한것임
                }
                else
                {
                   // this.pInfo.Unit(intUnitID).SubUnit(0).EQPProcessStateChangeBYWHO = "3";   //By Equipment
                    this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateChangeBYWHO = strEQPStateChangeBYWHO;
                }

                //HOST, CIM으로 부터 Pause가 왔는데 어떤 Unit에 대해 Pause가 안되고 다른 상태로 변경이 될때는 By Who를 EQP로 해서 보고하고 초기화함.
                if (this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateLastCommand == "4") //Pause가 왔음을 저장
                {
                    if (strEQPProcessState != "4")   //Pause가 아니면(하위 Process단임)
                    {
                        this.pInfo.Unit(intUnitID).SubUnit(0).EQPProcessStateChangeBYWHO = "3";   //By Equipment

                        dbolFlag = true;        //초기화 해야함을 저장
                        this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subACTEQPProcessState(Case 1): intUnitID: " + intUnitID.ToString() +
                                            ", dstrEQPProcessStateBackup: " + dstrEQPProcessStateBackup + ", strEQPProcessState:" + strEQPProcessState);
                    }
                }
                //HOST, CIM에서 Resume이 왔고 이전상태가 Pause이고 현재상태가 Pause가 아니면 HOST 보고 후 By Who를 초기화
                //else if (this.pInfo.Unit(intUnitID).SubUnit(0).EQPProcessStateLastCommand == "7") 
                else if (this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateLastCommand == "8")
                {
                    if (dstrEQPProcessStateBackup == "4" && strEQPProcessState != "4")
                    {
                        dbolFlag = true;        //초기화 해야함을 저장
                        this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subACTEQPProcessState(Case 2): intUnitID: " + intUnitID.ToString() +
                                           ", dstrEQPProcessStateBackup: " + dstrEQPProcessStateBackup + ", strEQPProcessState:" + strEQPProcessState);
                    }
                    //HOST, CIM에서 Resume이 왔는데 이전상태가 Pause가 아닌상태에서 상태 변경이 일어난 경우 By Who를 EQP로 해서 보고하고 초기화함.
                    else
                    {
                        this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateChangeBYWHO = "3";   //By Equipment

                        dbolFlag = true;        //초기화 해야함을 저장
                        this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subACTEQPProcessState(Case 3): intUnitID: " + intUnitID.ToString() +
                                          ", dstrEQPProcessStateBackup: " + dstrEQPProcessStateBackup + ", strEQPProcessState:" + strEQPProcessState);
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
                if (dstrEQPProcessStateBackup != strEQPProcessState)
                {
                    if (intUnitID == 0)   //Layer1 보고
                    {
                        this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 51, 3, 0);     //CEID=51(EQ(MODULE) PROCESS STATE CHANGED)
                        this.pInfo.Unit(intUnitID).SubUnit(0).EQPProcessStateChangeBYWHO = ""; //초기화 어경태
                    }
                    else
                    {
                        this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 51, intUnitID, dintSubUnitID);     //CEID=51(EQ(MODULE) PROCESS STATE CHANGED)
                        this.pInfo.Unit(intUnitID).SubUnit(0).EQPProcessStateChangeBYWHO = ""; //초기화 어경태

                    }
                }


                if (this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateLastCommand == this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessState)
                {
                    this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateChangeBYWHO = "";         //By Equipment로 초기화
                    this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateLastCommand = "";         //초기화
                    this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Bywho 초기화");
                }
                else if (dbolFlag == true)
                {
                    this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateChangeBYWHO = "";         //By Equipment로 초기화
                    this.pInfo.Unit(intUnitID).SubUnit(dintSubUnitID).EQPProcessStateLastCommand = "";         //초기화
                    this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Bywho 초기화");
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intUnitID:" + intUnitID + ", strEQPProcessState:" + strEQPProcessState);
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
