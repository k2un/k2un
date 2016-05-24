using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventControlStateCHG : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventControlStateCHG(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actControlStateCHG";
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
            string strCompBit = parameters[0];
            string dstrWordAddress = "";
            string dstrLOTID = "";
            int dintSlotID = 0;

            try
            {
                dstrWordAddress = "W2014";


                string strData = m_pEqpAct.funWordRead(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data);

                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);


                switch (strData)
                {
                    case "1":
                        if (pInfo.All.ControlState != "1")
                        {
                            this.pInfo.All.ControlstateChangeBYWHO = "2";    //By Operator

                            this.pInfo.All.ControlStateOLD = this.pInfo.All.ControlState;     //현재의 ControlState를 백업
                            this.pInfo.All.WantControlState = "";
                            this.pInfo.All.ControlState = "1";        // Offline 으로 변경일 경우엔 Host에 상관없이 Offline 변경을 한다.
                            this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 71, 3, 0); //뒤에 0은 전체장비 

                            this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, this.pInfo.All.ControlState);

                            //OFFLine 으로 전환되면 Trace를 초기화 한다. 20120507 어우수
                            this.pInfo.Unit(0).SubUnit(0).RemoveTRID();
                            this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "PLC OFFLINE Change!!");     //로그출력
                        }
                        break;

                    case "2":
                        if (pInfo.All.ControlState != "3")
                        {
                            this.pInfo.All.ControlstateChangeBYWHO = "2";    //By Operator

                            //PInfo.All.ControlStateOLD =this.PInfo.All.ControlState;     //현재의 ControlState를 백업
                            this.pInfo.All.WantControlState = "3";

                            if (pInfo.All.ControlState == "1")
                            {
                                this.pInfo.All.ONLINEModeChange = true;
                                this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S1F1AreYouThereRequest);
                            }
                            else
                            {
                                //Offline으로 전환 후 바로 폼을 받는다.
                                this.pInfo.All.ControlStateOLD = this.pInfo.All.ControlState;     //현재의 ControlState를 백업
                                this.pInfo.All.ControlState = "3";
                                this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 73, 3, 0);     //HOST로 보고, 뒤에 0은 전체장비 
                                this.pInfo.All.WantControlState = "";

                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, this.pInfo.All.ControlState); //PLC에게 알린다.
                            }

                            this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "PLC ONLINE-Remote Change!!");     //로그출력
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
