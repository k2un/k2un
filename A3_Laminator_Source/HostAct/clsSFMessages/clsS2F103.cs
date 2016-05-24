using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;
using System.Data;

namespace HostAct
{
    /// <summary>
    /// Equipment Online Parameter Change
    /// </summary>
    public class clsS2F103 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F103 Instance = new clsS2F103();
        #endregion

        #region Constructors
        public clsS2F103()
        {
            this.IntStream = 2;
            this.IntFunction = 103;
            this.StrPrimaryMsgName = "S2F103";
            this.StrSecondaryMsgName = "S2F104";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrModuleID = "";
            int dintEOIDTemp = 0; ;
            int dintEOIDCount = 0;
            int dintEOID = 0;
            int dintEOMD = 0;
            int dintEOV = 0;
            int dintEAC = 0;
            int dintMIACK = 0;
            string dstrEOIDIndex = "";
            int dintIndex = 0;
            int dintEOMDCount = 0;
            string dstrSQL = "";
            int dintMatchEOIDCount = 0;                         //한개라도 안 맞으면 적용안함 20130308
            Boolean dbolEQPProcessTimeOverReset = false;        //기존에 설정되어 있던 EQP Process Time Over 체크 설정을 다시 할지 여부
            Boolean dbolSuccess = false;
            bool dbolEOIDExist = true;

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("MIACK").Putvalue(1);
                    msgTran.Secondary().Item("EOIDCOUNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }

                dintEOIDCount = Convert.ToInt32(msgTran.Primary().Item("EOIDCOUNT").Getvalue());

                //EOID가 6이나 17이 아닌게 하나라도 있으면 NACK보고 후 리턴
                //2011.01.13    송 은선
                for (int dintLoop = 1; dintLoop <= dintEOIDCount; dintLoop++)
                {
                    dintEOIDTemp = Convert.ToInt32(msgTran.Primary().Item("EOID" + (dintLoop - 1)).Getvalue());

                    if (pInfo.funGetEOIDToIndex(dintEOIDTemp) == 0)
                    {
                        dbolEOIDExist = false;
                        //msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                        //msgTran.Secondary().Item("MIACK").Putvalue(2);
                        //msgTran.Secondary().Item("EOIDCOUNT").Putvalue(0);

                        //funSendReply(msgTran);
                        //return;

                    }
                }

                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                msgTran.Secondary().Item("EOIDCOUNT").Putvalue(dintEOIDCount);

                //받은 EOID중에 존재하지 않는것이 하나라도 있으면 NAK으로 보고하고 모두 적용하지 않는다.
                for (int dintLoop = 1; dintLoop <= dintEOIDCount; dintLoop++)
                {
                    dintEOMDCount = Convert.ToInt32(msgTran.Primary().Item("EOMDCOUNT" + (dintLoop - 1)).Getvalue());

                    for (int dintLoop2 = 1; dintLoop2 <= dintEOMDCount; dintLoop2++)
                    {
                        dintEOID = Convert.ToInt32(msgTran.Primary().Item("EOID" + (dintLoop - 1)).Getvalue());
                        dintEOMD = Convert.ToInt32(msgTran.Primary().Item("EOMD" + (dintLoop - 1) + (dintLoop2 - 1)).Getvalue());
                        dintEOV = Convert.ToInt32(msgTran.Primary().Item("EOV" + (dintLoop - 1) + (dintLoop2 - 1)).Getvalue());

                        //EOMD, EOMD값으로 Index를 찾는다.(Index가 Key값이기 떄문)
                        dintIndex = pInfo.funGetEOIDEOMDToIndex(dintEOID, dintEOMD);
                        if (pInfo.Unit(0).SubUnit(0).EOID(dintIndex) != null)   //존재하는 EOID임.
                        {
                            //EOMD 유효성 검사
                            if (dintEOMD >= pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMDMin && dintEOMD <= pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMDMax)
                            {
                                if (dintEOV >= pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMin && dintEOV <= pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMax)
                                {
                                    if (dintEOID == 8)     //VCR Reading Mode는 0,1,2,3 만 값이 있다.(이외의 값이 오면 Error임.)
                                    {
                                        if (dintEOV == 0 || dintEOV == 1 || dintEOV == 2 || dintEOV == 3)       //EOV값 정상
                                        {
                                            dintEAC = 0;                                        //EOMD 정상
                                            dintMatchEOIDCount += 1;        //옳은 EOID 개수 누적
                                        }
                                        else
                                        {
                                            dintEAC = 3;          //3 = Denied, At least one constant out of range
                                        }
                                    }
                                    else
                                    {
                                        dintEAC = 0;                                        //EOMD 정상
                                        dintMatchEOIDCount += 1;        //옳은 EOID 개수 누적
                                    }
                                }
                                else
                                {
                                    dintEAC = 3;          //3 = Denied, At least one constant out of range
                                }
                            }
                            else
                            {
                                dintEAC = 1;        //1 = Denied, At least one constant does not exist
                            }
                        }
                        else                        //존재하지 않는 EOID임.
                        {
                            dintEAC = 1;            //1 = Denied, At least one constant does not exist
                        }

                        msgTran.Secondary().Item("EOID" + (dintLoop - 1)).Putvalue(dintEOID);
                        msgTran.Secondary().Item("EOMDCOUNT" + (dintLoop - 1)).Putvalue(dintEOMDCount);

                        msgTran.Secondary().Item("EOMD" + (dintLoop - 1) + (dintLoop2 - 1)).Putvalue(dintEOMD);
                        msgTran.Secondary().Item("EAC" + (dintLoop - 1) + (dintLoop2 - 1)).Putvalue(dintEAC);
                    }
                }

                //if (dbolEOIDExist == false)
                //{
                //    dintMIACK = 2;  //없는 EOID를 받았다.
                //}
                //else
                {
                    dintMIACK = 0;  //정상
                }

                msgTran.Secondary().Item("MIACK").Putvalue(dintMIACK);


                //HOST로 S2F104 보고
                funSendReply(msgTran);

                //유효성 검사에서 OK이면 S6F11(CEID=101) 보고
                if (dintMatchEOIDCount == dintEOIDCount)
                {
                    pInfo.All.EOIDChangeBYWHO = "1";   //BY HOST

                    //구조체를 변경
                    for (int dintLoop = 1; dintLoop <= dintEOIDCount; dintLoop++)
                    {
                        dintEOMDCount = Convert.ToInt32(msgTran.Primary().Item("EOMDCOUNT" + (dintLoop - 1)).Getvalue());

                        for (int dintLoop2 = 1; dintLoop2 <= dintEOMDCount; dintLoop2++)
                        {
                            dintEOID = Convert.ToInt32(msgTran.Primary().Item("EOID" + (dintLoop - 1)).Getvalue());
                            dintEOMD = Convert.ToInt32(msgTran.Primary().Item("EOMD" + (dintLoop - 1) + (dintLoop2 - 1)).Getvalue());
                            dintEOV = Convert.ToInt32(msgTran.Primary().Item("EOV" + (dintLoop - 1) + (dintLoop2 - 1)).Getvalue());

                            //EOMD, EOMD값으로 Index를 찾는다.(Index가 Key값이기 떄문)
                            dintIndex = pInfo.funGetEOIDEOMDToIndex(dintEOID, dintEOMD);

                            if (pInfo.Unit(0).SubUnit(0).EOID(dintIndex) != null)
                            {
                                if (dintEOV >= pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMin && dintEOV <= pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMax) // && dintEOV != this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV
                                {
                                    //if (dintEOID == 8)     //VCR Reading Mode는 0,1,2,3 만 값이 있다.(이외의 값이 오면 Error임.)
                                    //{
                                    //    if (dintEOV == 0 || dintEOV == 1 || dintEOV == 2 || dintEOV == 3)       //EOV값 정상
                                    //    {
                                    //        dbolSuccess = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        dbolSuccess = false;
                                    //    }
                                    //}
                                    //else
                                    {
                                        dbolSuccess = true;
                                    }

                                    //True이면 업데이트 한다.  이때 EOV의 이전값과 바꾸는 값이 동일하면 넘어간다(S6F11 보고를 안하기 위해서임) - 같은값이 와도 보고해야됨(입고전검수) 20101208 KJK
                                    if (dbolSuccess == true)    // && dintEOV != this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV
                                    {

                                        #region "LOG 출력"
                                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.Parameter, string.Format("EOID,{0},변경,{1},{2},{3},{4}", DateTime.Now.ToString("yyyyMMddHHmmss"), pInfo.All.UserID, pInfo.Unit(0).SubUnit(0).EOID(dintIndex).DESC, pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV, dintEOV));      //로그 출력
                                        #endregion




                                        this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV = dintEOV;
                                        dstrEOIDIndex = dstrEOIDIndex + dintIndex.ToString() + ";";

                                        //DB Update
                                        dstrSQL = "Update tbEOID set EOV=" + dintEOV.ToString() + " Where Index=" + dintIndex.ToString();

                                        if (DBAct.clsDBAct.funExecuteQuery(dstrSQL) == false)
                                        {
                                            funSetLog(InfoAct.clsInfo.LogType.CIM, "DB Update Fail: EOID Index: " + dintIndex.ToString());
                                        }

                                        pInfo.DeleteTable("EOID");
                                        dstrSQL = "SELECT * FROM tbEOID order by Index";
                                        DataTable dt = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                                        pInfo.AddDataTable("EOID", dt);
                                        pInfo.AddViewEvent(InfoAct.clsInfo.ViewEvent.EOIDUpdate);


                                        switch (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID)
                                        {
                                            case 17:                       //MCC Reporting Mode (Decision to transfer MCC log file to File Server)
                                                if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV == 1)
                                                {
                                                    FunINIMethod.subINIWriteValue("MCC", "MCCFileUploadUse", "True", this.pInfo.All.SystemINIFilePath);
                                                    this.pInfo.All.MCC_UPLOAD = true;
                                                }
                                                else
                                                {
                                                    FunINIMethod.subINIWriteValue("MCC", "MCCFileUploadUse", "False", this.pInfo.All.SystemINIFilePath);
                                                    this.pInfo.All.MCC_UPLOAD = false;
                                                }
                                                break;

                                            case 19:            // SEM On/Off
                                                if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV == 1)
                                                {
                                                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.UDPPortOpen);
                                                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerStart);
                                                }
                                                else
                                                {
                                                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerEnd);
                                                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.UDPPortClose);
                                                }
                                                break;

                                            case 21:
                                                if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV == 1)
                                                {
                                                    //FunINIMethod.subINIWriteValue("MCC", "MCCFileUploadUse", "True", this.pInfo.All.SystemINIFilePath);
                                                    pInfo.All.APCUSE = true;
                                                }
                                                else
                                                {
                                                    //FunINIMethod.subINIWriteValue("MCC", "MCCFileUploadUse", "False", this.pInfo.All.SystemINIFilePath);
                                                    pInfo.All.APCUSE = false;
                                                }
                                                break;

                                            case 22:
                                                if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV == 1)
                                                {
                                                    //FunINIMethod.subINIWriteValue("MCC", "MCCFileUploadUse", "True", this.pInfo.All.SystemINIFilePath);
                                                    pInfo.All.RPCUSE = true;
                                                }
                                                else
                                                {
                                                    //FunINIMethod.subINIWriteValue("MCC", "MCCFileUploadUse", "False", this.pInfo.All.SystemINIFilePath);
                                                    pInfo.All.RPCUSE = false;
                                                }
                                                break;

                                            default:
                                                {

                                                }
                                                break;
                                        }

                                    }

                                    dbolSuccess = false;  //초기화
                                }
                            }
                        }
                    }

                    //CEID=101, Host로 보고
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentParameterEvent, 101, dstrEOIDIndex);   //마지막 인자는 EOID Index임
                    
                    pInfo.All.EOIDChangeBYWHO = "";   //초기화

                    //최종 수정된 날짜 Ini에 변경
                    FunINIMethod.subINIWriteValue("ETCInfo", "EOIDLastModified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), pInfo.All.SystemINIFilePath);

                }

                if (dbolEQPProcessTimeOverReset == true)
                {
                    pInfo.All.EQPProcessTimeOverReset = true;
                }
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return;
            }
        }

        /// <summary>
        /// Primary Message를 Biuld하여 Transaction을 Return한다.
        /// </summary>
        /// <param name="strParameters">Parameter 문자열</param>
        public Transaction funPrimarySend(string strParameters)
        {
            string[] arrayEvent;
            try
            {
                arrayEvent = strParameters.Split(',');

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                return pMsgTran;
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return null;
            }
        }

        /// <summary>
        /// Secondary Message 수신에 대해 처리한다.
        /// </summary>
        /// <param name="msgTran">Secondary Message의 Transaction</param>
        public void funSecondaryReceive(Transaction msgTran)
        {
            try
            {

            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }
        #endregion
    }
}
