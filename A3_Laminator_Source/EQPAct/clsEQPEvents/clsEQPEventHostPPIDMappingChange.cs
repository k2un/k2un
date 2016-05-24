using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventHostPPIDMappingChange : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventHostPPIDMappingChange(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actHOSTPPIDMappingChange";
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
            StringBuilder dstrLog = new StringBuilder();
            int dintLength = 0;
            string dstrTemp = "";
            int dintBodyIndex = 3;     //PPID Body Index


            string dstrAfterHOSTPPID = "";
            string dstrAfterEQPPPID = "";
            string dstrBeforeEQPPPID = "";
            int dintAfterPPIDType = 0;
            string[] dstrAfterValue;

            try
            {


                dstrWordAddress = "W2040";
                //m_pEqpAct.subWordReadSave(dstrWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data);        //HOST PPID
                m_pEqpAct.subWordReadSave(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data);                        //EQP PPID
                m_pEqpAct.subWordReadSave("", 10, EnuEQP.PLCRWType.Int_Data);                       //Spare
                m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                        //PPID Type
                //for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                //{
                //    dintLength = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length;
                //    m_pEqpAct.subWordReadSave("", dintLength, EnuEQP.PLCRWType.Int_Data);                 //Body
                //}

                dstrAfterValue = m_pEqpAct.funWordReadAction(true);

                //PLC Req에 대한 CIM의 Confirm Bit를 준다.5
                m_pEqpAct.subSetConfirmBit(strCompBit);

                dstrAfterHOSTPPID = dstrAfterValue[0];
                dstrAfterEQPPPID = dstrAfterValue[1];
                dintAfterPPIDType = Convert.ToInt32(dstrAfterValue[3]);

                if (pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrAfterHOSTPPID) == null)
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("존재하지 않는 HostPPID: {0} 에 대한 Mapping 변경 보고됨.", dstrAfterHOSTPPID));
                    return;
                }

                if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrAfterEQPPPID) == null)
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("HostPPID: {0} 에 존재하지 않는 EQPPPID: {1} Mapping 변경 보고됨.", dstrAfterHOSTPPID, dstrAfterEQPPPID));
                    return;
                }



                dstrBeforeEQPPPID = pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrAfterHOSTPPID).EQPPPID;

                pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrAfterHOSTPPID).EQPPPID = dstrAfterEQPPPID;

                //로그를 출력한다.
                dstrLog.Append(FunStringH.funPaddingStringData("-", 30, '-') + "\r\n");
                dstrLog.Append("HOST PPID Mapping 수정(After Data)" + "\r\n");
                dstrLog.Append("AfterHOSTPPID:" + dstrAfterHOSTPPID + ",");
                dstrLog.Append("AfterEQPPPID:" + dstrAfterEQPPPID + ",");
                dstrLog.Append("AfterPPIDType:" + dintAfterPPIDType.ToString() + "\r\n");

                //for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                //{
                //    dstrTemp = FunStringH.funPoint(dstrAfterValue[dintBodyIndex + dintLoop - 1], pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format);
                //    dstrLog.Append(pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Name + ":" + dstrTemp + "\r\n");
                //}
                dstrLog.Append(FunStringH.funPaddingStringData("-", 30, '-') + "\r\n");
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrLog.ToString());


                //HOST로 HOST Mapping 변경 보고
                //인자:변경(3), PPIDTYPE, HOSTPPID, EQPPPID
                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport, "3", dintAfterPPIDType, dstrAfterHOSTPPID, dstrAfterEQPPPID);

                m_pEqpAct.subSetParameterLog( "HOST PPID Mapping 수정", dstrAfterHOSTPPID, null, dstrBeforeEQPPPID, dstrAfterEQPPPID);

                //최종 수정된 날짜 Ini에 변경
                FunINIMethod.subINIWriteValue("ETCInfo", "RECIPELastModified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), pInfo.All.SystemINIFilePath);

                
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
