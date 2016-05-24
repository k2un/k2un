using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventSetupPPID : clsEQPEvent, IEQPEvent
    {
        #region Variables
        private int pPPIDReadCount = 15;                            //한번에 PLC로 부터 읽어오는(가져오는) PPID 개수
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventSetupPPID(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actSetupPPID";
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
            string[] dstrValue;
            StringBuilder dstrLog = new StringBuilder();
            string dstrHOSTPPID = "";
            string dstrEQPPPID = "";
            int dintPPIDType = 0;
            string dstrTemp = "";
            int dintLength = 0;
            int dintReadIndex = 0;
            int dintNotUsedArea = 64 - 12 - pInfo.Unit(0).SubUnit(0).PPIDBodyCount;
            string dstrWordAddress = "";
            string strData = "";

            try
            {
                dintPPIDType = pInfo.All.SearchPPIDType;

                if (dintPPIDType == 1) //EQPPPID
                {
                    pInfo.Unit(0).SubUnit(0).RemoveEQPPPID();
                    dstrWordAddress = "W2A00";
                    dstrTemp = m_pEqpAct.funWordRead(dstrWordAddress, 22, EnuEQP.PLCRWType.Binary_Data);
                    for (int dintLoop = 0; dintLoop < 22; dintLoop++)
                    {
                        strData = dstrTemp.Substring(dintLoop * 16, 16);
                        for (int dintLoop2 = 0; dintLoop2 < 16; dintLoop2++)
                        {
                            if (strData[dintLoop2].ToString() == "1")
                            {
                                dstrEQPPPID = ((16 - dintLoop2) * (dintLoop + 1)).ToString();
                                pInfo.Unit(0).SubUnit(0).AddEQPPPID(dstrEQPPPID);
                            }
                        }
                    }
                }
                else ///HostPPID
                {
                    //dstrWordAddress = "W2A40";
                    //dstrTemp = m_pEqpAct.funWordRead(dstrWordAddress, 220, EnuEQP.PLCRWType.Hex_Data);
                    //for (int dintLoop = 0; dintLoop < 20; dintLoop++)
                    //{
                    //    string strData1 = "";
                    //    strData = dstrTemp.Substring(44 * dintLoop, 44);
                    //    for (int dintLoop2 = 0; dintLoop2 < 40; dintLoop2 = dintLoop2 + 4)
                    //    {
                    //        string Temp = strData.Substring(dintLoop2, 4);

                    //        strData1 += FunTypeConversion.funHexSwap(Temp);
                    //    }
                    //    dstrHOSTPPID = CommonAct.FunTypeConversion.funHexConvert(strData1, EnuEQP.StringType.ASCString);
                    //    dstrEQPPPID = FunTypeConversion.funHexConvert(strData.Substring(40, 4), EnuEQP.StringType.Decimal).ToString();

                    //    pInfo.Unit(0).SubUnit(0).AddHOSTPPID(dstrHOSTPPID);
                    //    pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID).EQPPPID = dstrEQPPPID;
                    //}
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                if (pInfo.All.EQPPPIDCommandCount == 0 && pInfo.All.HOSTPPIDCommandCount == 0)            //모든 EQP, HOST PPID 를 다 읽었냐?
                {
                    if (pInfo.All.isReceivedFromHOST == true)
                    {
                        pInfo.All.isReceivedFromHOST = false;  //초기화
                        pInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                    }
                }
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
