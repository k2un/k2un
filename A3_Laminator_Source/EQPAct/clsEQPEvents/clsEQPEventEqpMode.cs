using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventEqpMode : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventEqpMode(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actEQPMode";
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
            int intValue = Convert.ToInt32(parameters[4]);

            Boolean dbolAuto = false;

            try
            {
                //Auto 모드임
                if (intValue == 1)
                {
                    dbolAuto = true;

                    //CIM<->HOST와 Connect되어 있고 현재 Control State가 Offline인데
                    //PLC T/P에서 Manual -> Auto 모드로 전환하면 자동으로 HOST로 Remote 모드로 전환한다. 
                    //if (PInfo.All.HostConnect == true && PInfo.All.ControlState == "1")
                    //{
                    //    PInfo.All.ControlstateChangeBYWHO = "3";    //By EQP

                    //    PInfo.All.ControlStateOLD = PInfo.All.ControlState;     //현재의 ControlState를 백업
                    //    PInfo.All.WantControlState = "3";

                    //    PInfo.All.ONLINEModeChange = true;
                    //    m_pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S1F1AreYouThereRequest);
                    //}
                }
                else
                {
                    dbolAuto = false;

                    //장비에서 Auto Mode -> Manual Mode로 전환시 만약 CIM<->HOST가 Online이면 Offline으로 전환한다.
                    //if (PInfo.All.ControlState != "1")
                    //{
                    //    PInfo.All.ControlstateChangeBYWHO = "3";    //By EQP

                    //    PInfo.All.ControlStateOLD = PInfo.All.ControlState;     //현재의 ControlState를 백업
                    //    PInfo.All.WantControlState = "";
                    //    PInfo.All.ControlState = "1";        // Offline 으로 변경일 경우엔 Host에 상관없이 Offline 변경을 한다.
                    //    m_pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 71, 0); //뒤에 0은 전체장비 

                    //    m_pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, PInfo.All.ControlState);
                    //}
                }

                pInfo.All.AutoMode = dbolAuto;
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intValue:" + intValue);
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
