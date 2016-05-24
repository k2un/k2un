using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventJudge : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventJudge(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actJudge";
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
            string dstrGLSID = "";
            string dstrValue;
            string dstrLogMsg = "";
            int dintUnitID = 0;
            string dstrJudgementCode = "";
            try
            {
                dstrWordAddress = "W2012";
                dstrValue = m_pEqpAct.funWordRead(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data);

                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);
                
                //dstrGLSID = pInfo.Unit(3).SubUnit(7).HGLSID;
                //dstrLOTID = pInfo.Unit(3).SubUnit(7).CurrGLS(dstrGLSID).LOTID;
                //dintSlotID = Convert.ToInt32(pInfo.Unit(3).SubUnit(7).CurrGLS(dstrGLSID).SlotID);
                
                //switch (dstrValue)
                //{

                //    case "1":
                //        dstrJudgementCode = "OK";
                //        break;

                //    case "2":
                //        dstrJudgementCode = "NG";
                //        break;

                //    case "3":
                //        dstrJudgementCode = "RJ";
                //        break;

                //    case "4":
                //        dstrJudgementCode = "RW";
                //        break;
                //}


                //this.pInfo.LOTID(dstrLOTID).Slot(dintSlotID).JUDGEMENT = dstrJudgementCode;
                //pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 15, 8, dstrLOTID, dintSlotID, dstrGLSID);


            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", dstrLOTID:" + dstrLOTID + ", dintSlotID: " + dintSlotID.ToString());
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
