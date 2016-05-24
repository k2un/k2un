using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CommonAct;
using System.Data.OleDb;

namespace EQPAct
{
    public class clsEQPEventECIDChange : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventECIDChange(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actECIDChange";
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
            int dintIndex = 0;
            //int dintIndex2 = 0;
            string dstrWordAddress = "";
            string dstrECIDReport = "";
            string dstrECSLL = "";
            string dstrECWLL = "";
            string dstrECDEF = "";
            string dstrECWUL = "";
            string dstrECSUL = "";
            string dstrLog = "";
            string dstrSQL = "";
            string[] dstrValue = null;
            int dintECID = 0;
            string dstrECIDReadData = "";
            string strData = "";
            string dstrECID = "";
            try
            {
                dstrWordAddress = "W2A00";

                dstrECIDReadData = m_pEqpAct.funWordRead(dstrWordAddress, pInfo.Unit(0).SubUnit(0).ECIDCount * 7, EnuEQP.PLCRWType.Hex_Data);
                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);

                // 디비트랜젝션... 해야지..
                if (DBAct.clsDBAct.funOleDbTransaction() == null) DBAct.clsDBAct.funBeginTransaction();

                OleDbTransaction odbTransaction = DBAct.clsDBAct.funOleDbTransaction();


                for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).ECIDCount; dintLoop++)
                {
                    strData = dstrECIDReadData.Substring((dintLoop - 1) * 28, 28);
                    dstrECID = strData.Substring(0, 4);
                    dintECID = Convert.ToInt32( FunTypeConversion.funHexConvert(dstrECID, EnuEQP.StringType.Decimal)); //ECID

                    //dstrECSLL = strData.Substring(4, 8); // ECSLL
                    dstrECWLL = strData.Substring(4, 8);
                    dstrECWLL = dstrECWLL.Substring(4, 4) + dstrECWLL.Substring(0, 4);
                    dstrECDEF = strData.Substring(12, 8); // ECDEF
                    dstrECDEF = dstrECDEF.Substring(4, 4) + dstrECDEF.Substring(0, 4);
                    //dstrECSUL = strData.Substring(20, 8); // ECSUL
                    dstrECWUL = strData.Substring(20, 8);
                    dstrECWUL = dstrECWUL.Substring(4, 4) + dstrECWUL.Substring(0, 4);
                    
                    //dstrECSLL = FunTypeConversion.funDecimalConvert(dstrECSLL, EnuEQP.StringType.Hex);
                    //dstrECDEF = FunTypeConversion.funDecimalConvert(dstrECDEF, EnuEQP.StringType.Hex);
                    //dstrECSUL = FunTypeConversion.funDecimalConvert(dstrECSUL, EnuEQP.StringType.Hex);

                    dstrECWLL = FunTypeConversion.funHexConvert(dstrECWLL, EnuEQP.StringType.Decimal);
                    dstrECDEF = FunTypeConversion.funHexConvert(dstrECDEF, EnuEQP.StringType.Decimal);
                    dstrECWUL = FunTypeConversion.funHexConvert(dstrECWUL, EnuEQP.StringType.Decimal);

                    ////dstrECSLL = FunTypeConversion.funPlusMinusAPDCalc(dstrECSLL);
                    //dstrECWLL = FunTypeConversion.funPlusMinusAPDCalc(dstrECWLL);
                    //dstrECDEF = FunTypeConversion.funPlusMinusAPDCalc(dstrECDEF);
                    ////dstrECSUL = FunTypeConversion.funPlusMinusAPDCalc(dstrECSUL);
                    //dstrECWUL = FunTypeConversion.funPlusMinusAPDCalc(dstrECWUL);

                    //dstrECSLL = FunStringH.funPoint(dstrECSLL, pInfo.Unit(0).SubUnit(0).ECID(dintLoop).Format);
                    dstrECWLL = FunStringH.funPoint(dstrECWLL, pInfo.Unit(0).SubUnit(0).ECID(dintLoop).Format);
                    dstrECDEF = FunStringH.funPoint(dstrECDEF, pInfo.Unit(0).SubUnit(0).ECID(dintLoop).Format);
                    //dstrECSUL = FunStringH.funPoint(dstrECSUL, pInfo.Unit(0).SubUnit(0).ECID(dintLoop).Format);
                    dstrECWUL = FunStringH.funPoint(dstrECWUL, pInfo.Unit(0).SubUnit(0).ECID(dintLoop).Format);



                    if (pInfo.Unit(0).SubUnit(0).ECID(dintECID) != null)
                    {
                        if (pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECDEF != dstrECDEF ||
                            pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWLL != dstrECWLL || 
                            pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWUL != dstrECWUL)
                           // pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSLL != dstrECSLL || 
                           //pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSUL != dstrECSUL)
                        {
                            dstrECIDReport += dintLoop.ToString() + ";";

                            //로그 출력
                            //dstrLog = dstrLog + "ECID" + ",";
                            //dstrLog = dstrLog + DateTime.Now.ToString("yyyyMMddHHmmss") + ",";
                            //dstrLog = dstrLog + "변경" + ",";
                            //dstrLog = dstrLog + pInfo.All.UserID + ",";
                            //dstrLog = dstrLog + "ECID(" + dintLoop.ToString() + "): " + pInfo.Unit(0).SubUnit(0).ECID(dintLoop).DESC + ",";
                            //dstrLog = dstrLog + pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSLL + ",";
                            //dstrLog = dstrLog + pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECDEF + ",";
                            //dstrLog = dstrLog + pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSUL + ",";

                            //dstrLog = dstrLog + dstrECSLL + ",";
                            //dstrLog = dstrLog + dstrECWLL + ",";
                            //dstrLog = dstrLog + dstrECDEF + ",";
                            //dstrLog = dstrLog + dstrECWUL + ",";
                            //dstrLog = dstrLog + dstrECSUL;


                            dstrLog = string.Format("ECID,{0},변경,{1},ECID({2}): {3},{4} -> {5}, {6} -> {7}, {8} -> {9}",
                                DateTime.Now.ToString("yyyyMMddHHmmss"),
                                pInfo.All.UserID,
                                dintLoop.ToString(),
                                pInfo.Unit(0).SubUnit(0).ECID(dintLoop).DESC,
                                pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECWLL,
                                dstrECWLL,
                                pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECDEF,
                                dstrECDEF,
                                pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECWUL,
                                dstrECWUL
                            );

                            pInfo.subLog_Set(InfoAct.clsInfo.LogType.Parameter, dstrLog);      //로그 출력
                            dstrLog = ""; //초기화

                            //구조체 업데이트

                            pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSLL = dstrECWLL;
                            pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECWLL = dstrECWLL;
                            pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECDEF = dstrECDEF;
                            pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECWUL = dstrECWUL;
                            pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSUL = dstrECWUL;

                            dstrSQL = "Update tbECID set ECSLL=" + pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSLL + ", ECWLL=" + dstrECWLL + ", ECDEF=" + dstrECDEF +
                                     ", ECWUL=" + dstrECWUL + ", ECSUL=" + pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSUL + " Where ECID=" + dintLoop.ToString();

                            if (DBAct.clsDBAct.funExecuteNonQuery(dstrSQL) == false)
                            {
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "DB Update Fail(subLoadECID): ECID: " + dintLoop.ToString());
                            }
                        }
                    }
                }

                if (DBAct.clsDBAct.funOleDbTransaction() != null) DBAct.clsDBAct.funCommitTransaction();


                // 디비내용 업데이트는 한번만 하면 되지.. 이게 뭐니...
                pInfo.DeleteTable("ECID");
                dstrSQL = "SELECT * FROM tbECID order by ECID";
                DataTable dt = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                pInfo.AddDataTable("ECID", dt);
                pInfo.AddViewEvent(InfoAct.clsInfo.ViewEvent.ECIDUpdate);

                if (pInfo.All.ECIDChangeBYWHO == "1" || pInfo.All.ECIDChangeBYWHO == "2")
                {
                    //HOST나 OP에서 발생한것임
                    dstrECIDReport = pInfo.All.ECIDChangeFromHost;
                    pInfo.All.ECIDChangeFromHost = "";
                    //m_pInfo.All.ECIDChangeBYWHO = "";
                }
                else
                {
                    pInfo.All.ECIDChangeBYWHO = "2";  //By Equipment
                }


                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentParameterEvent, 102, dstrECIDReport); //마지막 인자는 ECID임
                pInfo.All.ECIDChangeBYWHO = "";

                //최종 수정된 날짜 Ini에 변경
                FunINIMethod.subINIWriteValue("ETCInfo", "ECIDLastModified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), pInfo.All.SystemINIFilePath);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                if (!DBAct.clsDBAct.funIsNullTransaction()) DBAct.clsDBAct.funCommitTransaction();
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
