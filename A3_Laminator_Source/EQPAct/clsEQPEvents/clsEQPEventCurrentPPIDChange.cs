using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventCurrentPPIDChange : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventCurrentPPIDChange(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actCurrentPPIDChange";
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
            string dstrWordAddress = "W2003";
            string dstrOLDHOSTPPID = "";
            string dstrNEWHOSTPPID = "";
            string dstrOLDEQPPPID = "";
            string dstrNEWEQPPPID = "";
            string[] dstrValue;
            string dstrLogMsg = "";

            try
            {
                //변경된 현재 PPID를 읽어온다.
                m_pEqpAct.subWordReadSave(dstrWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data);     //변경된 현재 HOSTPPID
                m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);     //변경된 현재 HOSTPPID

                dstrValue = m_pEqpAct.funWordReadAction(true);    //Word영역을 Block으로 읽어서 결과값을 DataType에 맞게 배열로 넘겨준다.

                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);

                dstrNEWHOSTPPID = dstrValue[0];
                dstrNEWEQPPPID = dstrValue[1];
                //로그 출력
                dstrLogMsg = "Current PPID Change-> Before: " + pInfo.All.CurrentHOSTPPID + ", After: " + dstrNEWHOSTPPID;
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrLogMsg);

                if (pInfo.All.CurrentHOSTPPID != dstrNEWHOSTPPID)
                {
                    dstrOLDHOSTPPID = pInfo.All.CurrentHOSTPPID;   //이전 PPID 백업
                    pInfo.All.CurrentHOSTPPID = dstrNEWHOSTPPID;                                                      //변경된 HOSTPPID를 입력

                    if (pInfo.All.EQPSpecifiedCtrlBYWHO == "1")// || pInfo.All.EQPSpecifiedCtrlBYWHO == "2")
                    {
                        //HOST나 OP에서 발생한것임
                    }
                    else
                    {
                        pInfo.All.EQPSpecifiedCtrlBYWHO = "2"; //BY EQP
                    }

                    //CEID=131보고
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11EquipmentSpecifiedControlEvent, 131, dstrOLDHOSTPPID, dstrNEWHOSTPPID);

                    pInfo.All.EQPSpecifiedCtrlBYWHO = "";  //초기화
                }

                //현재 운영중인 HOST PPID가 변경되면 현재 운영중인 EQP PPID를 다시 읽는다.
                dstrWordAddress = "W";
                m_pEqpAct.subWordReadSave(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data);             //현재 운영중인 EQP_PPID

                dstrValue = m_pEqpAct.funWordReadAction(true);

                pInfo.All.CurrentEQPPPID = dstrValue[0];

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
