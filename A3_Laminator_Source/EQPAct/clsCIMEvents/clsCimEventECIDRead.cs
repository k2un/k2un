using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Data;
using System.Data.OleDb;

namespace EQPAct
{
    public class clsCimEventECIDRead : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventECIDRead(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "ECIDRead";
        }
        #endregion

        #region Methods
        /// <summary>
        /// CIM에서 설비로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : cmdName
        /// parameters[1] : 1st parameter
        /// parameters[2] : 2nd parameter
        /// parameters[3] : 3rd parameter
        /// parameters[4] : 4th parameter
        /// parameters[5] : 5th Parameter
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            string dstrWordAddress = "";
            StringBuilder dstrLog = new StringBuilder();
            string[] dstrValue = null;
            int dintIndex = 0;
            string dstrECSLL = "";
            string dstrECWLL = "";
            string dstrECDEF = "";
            string dstrECWUL = "";
            string dstrECSUL = "";
            string dstrSQL = "";
            int dintECID = 0;

            try
            {
                dstrWordAddress = "W2A00";
                for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).ECIDCount; dintLoop++)
                {
                    if (dintLoop == 1)
                    {
                        pEqpAct.subWordReadSave(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data);             //ECID
                    }
                    else
                    {
                        pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                          //ECID
                    }
                    pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.Int_Data);                          //ECSLL
                    pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.Int_Data);                          //ECDEF
                    pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.Int_Data);                          //ECSUL
                }
                dstrValue = pEqpAct.funWordReadAction(true);

                // 디비트랜젝션... 해야지..
                if (DBAct.clsDBAct.funOleDbTransaction() == null) DBAct.clsDBAct.funBeginTransaction();

                OleDbTransaction odbTransaction = DBAct.clsDBAct.funOleDbTransaction();

                //변경된 ECID 값을 구조체에 저장, DB Update, HOST로 보고
                for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).ECIDCount; dintLoop++)
                {
                    dintECID = Convert.ToInt32(dstrValue[dintIndex++]);

                    if (dintLoop == dintECID)
                    {
                        dstrECSLL = dstrValue[dintIndex++];
                        dstrECDEF = dstrValue[dintIndex++];
                        dstrECSUL = dstrValue[dintIndex++];

                        //dstrECSLL = FunTypeConversion.funDecimalConvert(dstrECSLL, EnuEQP.StringType.Hex);
                        //dstrECDEF = FunTypeConversion.funDecimalConvert(dstrECDEF, EnuEQP.StringType.Hex);
                        //dstrECSUL = FunTypeConversion.funDecimalConvert(dstrECSUL, EnuEQP.StringType.Hex);

                        //dstrECSLL = FunTypeConversion.funPlusMinusAPDCalc(dstrECSLL);
                        //dstrECDEF = FunTypeConversion.funPlusMinusAPDCalc(dstrECDEF);
                        //dstrECSUL = FunTypeConversion.funPlusMinusAPDCalc(dstrECSUL);

                        dstrECSLL = FunStringH.funPoint(dstrECSLL, pInfo.Unit(0).SubUnit(0).ECID(dintLoop).Format);
                        dstrECDEF = FunStringH.funPoint(dstrECDEF, pInfo.Unit(0).SubUnit(0).ECID(dintLoop).Format);
                        dstrECSUL = FunStringH.funPoint(dstrECSUL, pInfo.Unit(0).SubUnit(0).ECID(dintLoop).Format);

                        //if (pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSLL != dstrECSLL || //pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWLL != dstrECWLL ||
                        //pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECDEF != dstrECDEF || //pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWUL != dstrECWUL ||
                        //pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSUL != dstrECSUL)

                        //if (
                        //     pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWLL != dstrECSLL ||
                        //    pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECDEF != dstrECDEF ||
                        //     pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWUL != dstrECSUL
                        //    )
                        {
                            //구조체 업데이트
                            pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSLL = dstrECSLL;
                            pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECWLL = dstrECSLL;
                            pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECDEF = dstrECDEF;
                            pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECWUL = dstrECSUL;
                            pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSUL = dstrECSUL;

                            //dstrSQL = "Update tbECID set ECWLL=" + dstrECSLL + ", ECDEF=" + dstrECDEF +
                            //           ", ECWUL=" + dstrECSUL + " Where ECID=" + dintLoop.ToString();

                            dstrSQL = "Update tbECID set ECSLL=" + pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSLL + ", ECWLL=" + dstrECSLL + ", ECDEF=" + dstrECDEF +
                                    ", ECWUL=" + dstrECSUL + ", ECSUL=" + pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSUL + " Where ECID=" + dintLoop.ToString();

                            if (DBAct.clsDBAct.funExecuteNonQuery(dstrSQL) == false)
                            {
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "DB Update Fail(subLoadECID): ECID: " + dintLoop.ToString());
                            }
                        }

                    }

                    //dintIndex += 6;
                }

                if (DBAct.clsDBAct.funOleDbTransaction() != null) DBAct.clsDBAct.funCommitTransaction();


                // 디비내용 업데이트는 한번만 하면 되지.. 이게 뭐니...
                pInfo.DeleteTable("ECID");
                dstrSQL = "SELECT * FROM tbECID order by ECID";
                DataTable dt = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                pInfo.AddDataTable("ECID", dt);
                pInfo.AddViewEvent(InfoAct.clsInfo.ViewEvent.ECIDUpdate);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                if (!DBAct.clsDBAct.funIsNullTransaction()) DBAct.clsDBAct.funCommitTransaction();              

                //HOST로 부터 S2F29가 와서 PLC로 부터 모두 읽었다고 저장, 그리고 HOST Act단에서 S2F30을 응답
                if (pInfo.All.isReceivedFromHOST == true)
                {
                    pInfo.All.isReceivedFromHOST = false;      //초기화
                    pInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                }
            }
        }
        #endregion
    }
}
