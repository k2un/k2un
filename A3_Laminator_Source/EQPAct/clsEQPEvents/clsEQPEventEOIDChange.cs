using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventEOIDChange : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventEOIDChange(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actEOIDChange";
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
            string strEOIDItem = parameters[1];

            string[] dstrValue;
            string dstrWordAddress = "";
            string dstrLog = "";
            int dintIndex = 0;
            string dstrSection = "EOIDLastModified";
            int dintReadingEOV = 0;
            string dstrAfterEOV = "";
            SortedList dHT = new SortedList();
            string dstrSQL;
            string[] dstrData;

            try
            {
                dintIndex = pInfo.funGetEOIDItemToIndex(strEOIDItem);                     //tbEOID 테이블의 항목에 해당하는 Index 컬럼값

                switch (strEOIDItem)
                {
                    //case "VCR":
                    //    dstrWordAddress = "W1511";           //VCR Reading Mode
                    //    break;

                    case "APC":
                        dstrWordAddress = "W1512";           //Wait time for glassid key-input(At reading fail)
                        break;
                }

                m_pEqpAct.subWordReadSave(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data);               //VCR Reading Mode

                dstrValue = m_pEqpAct.funWordReadAction(true);

                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);

                dintReadingEOV = Convert.ToInt32(dstrValue[0]);


                //만약 현재 EOV와 같은 값이 오면 처리를 하지 않는다.
                if (pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV == dintReadingEOV)
                {
                }
                else
                {
                    dstrAfterEOV = dstrAfterEOV + dintIndex.ToString() + ";";       //EOID 저장 
                    dHT.Add(dintIndex, pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV);        //변경전의 EOID Index, EOV 저장

                    //구조체 변경
                    pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV = dintReadingEOV;

                    //DB Update
                    dstrSQL = "Update tbEOID set EOV=" + dintReadingEOV + " Where Index=" + dintIndex.ToString();

                    if (DBAct.clsDBAct.funExecuteQuery(dstrSQL) == false)
                    {
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "DB Update Fail: EOID Index: " + dintIndex.ToString());
                    }

                    //변경된 EOV에 대해 HOST 보고
                    dstrData = dstrAfterEOV.Split(new char[] { ';' });
                    if (dstrData.Length > 1)
                    {
                        if (pInfo.All.EOIDChangeBYWHO == "1" || pInfo.All.EOIDChangeBYWHO == "2")
                        {
                            //HOST나 OP에서 발생한것임
                        }
                        else
                        {
                            pInfo.All.EOIDChangeBYWHO = "3";       //BY Equipment
                        }

                        //HOST로 EOV값 변경 보고
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentParameterEvent, 101, dstrAfterEOV);   //마지막 인자는 EOID Index임

                        pInfo.All.EOIDChangeBYWHO = "";       //초기화

                        //변경 이력 로그 저장
                        foreach (DictionaryEntry de in dHT)
                        {
                            dstrLog = dstrLog + "EOID" + ",";
                            dstrLog = dstrLog + DateTime.Now.ToString("yyyyMMddHHmmss") + ",";
                            dstrLog = dstrLog + "변경" + ",";
                            dstrLog = dstrLog + pInfo.All.UserID + ",";
                            dstrLog = dstrLog + pInfo.Unit(0).SubUnit(0).EOID(Convert.ToInt32(de.Key)).DESC + ",";
                            dstrLog = dstrLog + de.Value.ToString() + ",";
                            dstrLog = dstrLog + pInfo.Unit(0).SubUnit(0).EOID(Convert.ToInt32(de.Key)).EOV;

                            pInfo.subLog_Set(InfoAct.clsInfo.LogType.Parameter, dstrLog);      //로그 출력

                            dstrLog = ""; //초기화
                        }
                    }


                    //System.ini 파일에 변경 날짜 갱신
                    FunINIMethod.subINIWriteValue("ETCInfo", dstrSection, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), pInfo.All.SystemINIFilePath);
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
