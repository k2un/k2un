using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEvent_HandGlass : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEvent_HandGlass(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actHandGlass";
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
            string strCompBit = "";
            int dintUnitNo = 0;
            int dintHandNo = 0;
            int dintBItVal = 0;
            string dstrWordAddress = "";
            string strGLSID = "";

            try
            {
                //strCompBit = parameters[0];
                dintUnitNo = Convert.ToInt32(parameters[2]);
                dintHandNo = Convert.ToInt32(parameters[3]);
                dintBItVal = Convert.ToInt32(parameters[4]);
              

                DateTime dtNow = DateTime.Now;

                if (dintBItVal == 1)
                {
                    switch (dintUnitNo)
                    {
                        case 1:
                            if (dintHandNo == 1)
                            {
                                dstrWordAddress = "W3000";
                                strGLSID = m_pEqpAct.funWordRead(dstrWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                                pInfo.Unit(1).SubUnit(0).UpperHandGlassID = strGLSID;
                            }
                            else
                            {
                                dstrWordAddress = "W3010";
                                strGLSID = m_pEqpAct.funWordRead(dstrWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                                pInfo.Unit(1).SubUnit(0).LowerHandGlassID = strGLSID;
                            }
                            
                            break;

                        case 4:
                            if (dintHandNo == 1)
                            {
                                dstrWordAddress = "W6010";
                                strGLSID = m_pEqpAct.funWordRead(dstrWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                                pInfo.Unit(4).SubUnit(0).UpperHandGlassID = strGLSID;
                            }
                            else
                            {
                                dstrWordAddress = "W6020";
                                strGLSID = m_pEqpAct.funWordRead(dstrWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                                pInfo.Unit(4).SubUnit(0).LowerHandGlassID = strGLSID;
                            }
                            
                            break;
                    }
                }
                else
                {
                    switch (dintUnitNo)
                    {
                        case 1:
                            if (dintHandNo == 1)
                            {
                                pInfo.Unit(1).SubUnit(0).UpperHandGlassID = "";
                            }
                            else
                            {
                                pInfo.Unit(1).SubUnit(0).LowerHandGlassID= "";
                            }

                            break;

                        case 4:

                            if (dintHandNo == 1)
                            {
                                pInfo.Unit(4).SubUnit(0).UpperHandGlassID = "";
                            }              
                            else           
                            {              
                                pInfo.Unit(4).SubUnit(0).LowerHandGlassID = "";
                            }
                            break;
                    } 
                }

                //m_pEqpAct.subSetConfirmBit(strCompBit);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 알람이 발생할당시에 해당장비내에 글래스위치정보를 남긴다.
        /// </summary>
        /// <param name="strLogWriteTime"></param>
        /// <param name="intUnitID"></param>
        /// <param name="intAlarmID"></param>
        private void subGLSPosLogWrite(string strLogWriteTime, int intUnitID, int intAlarmID)
        {
            string dstrGLSID = "";
            string dstrLogdata = "";

            string dstrLOTID = "";
            int dintSlotID = 0;
            string dstrPPID = "";
            try
            {
                dstrLogdata += strLogWriteTime + "," + intUnitID + "," + intAlarmID + ",";

                for (int dintLoop = 1; dintLoop <= pInfo.Unit(intUnitID).SubUnitCount; dintLoop++)
                {
                    dstrLOTID = pInfo.Unit(intUnitID).SubUnit(dintLoop).LOTID.Trim();
                    dstrGLSID = pInfo.Unit(intUnitID).SubUnit(dintLoop).GLSID.Trim();

                    if (string.IsNullOrEmpty(dstrLOTID) == false || string.IsNullOrEmpty(dstrGLSID) == false || pInfo.LOTID(dstrLOTID) == null || pInfo.LOTID(dstrLOTID).GLSID(dstrGLSID) == null)
                    {
                        dstrLogdata += "///,";
                    }
                    else
                    {
                        dstrLogdata += pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrGLSID).GLSID + "/"
                                    + dstrLOTID + "/"
                                    + pInfo.LOTID(dstrLOTID).InPortID + "/"
                                    + pInfo.Port(pInfo.LOTID(dstrLOTID).InPortID).HostPPID + ",";
                    }
                }

                //for (int dintLoop = 1; dintLoop <= pInfo.UnitCount; dintLoop++)
                //{
                //    dstrHGLSID = pInfo.Unit(dintLoop).SubUnit(0).GLSID;

                //    if (dstrHGLSID == "")
                //    {
                //        dstrLogdata += "///,";
                //    }
                //    else
                //    {
                //        dstrLOTID = pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).LOTID;
                //        dintSlotID = pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).SlotID;
                //        

                //        dstrLogdata += pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).GLSID + "/"
                //                    + dstrLOTID + "/"
                //                    + dintSlotID + "/"
                //                    + dstrPPID + ",";
                //    }
                //}

                //마지막의 콤마는 제거
                dstrLogdata = dstrLogdata.Remove(dstrLogdata.Length - 1);

                pInfo.subLog_Set(InfoAct.clsInfo.LogType.AlarmGLSInfo, dstrLogdata);

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
