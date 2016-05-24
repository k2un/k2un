using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;
using System.Threading;

namespace HostAct
{
    public class clsS8F3 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS8F3 Instance = new clsS8F3();
        #endregion

        #region Constructors
        public clsS8F3()
        {
            this.IntStream = 8;
            this.IntFunction = 3;
            this.StrPrimaryMsgName = "S8F3";
            this.StrSecondaryMsgName = "S8F4";
        }
        #endregion

        #region Methods

        enum ACK
        { 
            OK = 0, 
            ModuleID_msMatch = 1, 
            At_least_One_data_invalid = 2
        }
        class MultiUseDataBody
        {
            public string type = "";
            public string name = "";
            public string value = "";
            public string reference = "";
            public MultiUseDataBody(string type, string name)
            {
                this.type = type;
                this.name = name;
            }
        }
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            List<MultiUseDataBody> dListReceiveData = new List<MultiUseDataBody>();
            //List<List<string>> dListBodyOfType = new List<List<string>>();
            ACK ack = ACK.OK;    

            try
            {
                if (this.pInfo.All.HostConnect == false) return;

                #region ModuleID Check
                string dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim(); 
                if (!dstrModuleID.Equals(this.pInfo.Unit(3).SubUnit(0).ModuleID))   // 모듈아이디 나크
                {
                    msgTran.Secondary().Item("L2").Putvalue(0);
                    msgTran.Secondary().Item("ACK").Putvalue((int)ACK.ModuleID_msMatch);
                    funSendReply(msgTran);
                    return;
                }
                #endregion


                int dintTypeCount = Convert.ToInt32(msgTran.Primary().Item("L2").Getvalue());   // 타입 카운트 

                #region 장비 전체 리스트 조회 시
                if (dintTypeCount == 0)  // 모두 조회
                {
                    foreach (string dstrTYPE in this.pInfo.Unit(0).SubUnit(0).MultiData.TYPES())
                    {
                        foreach (string dstrITEM in this.pInfo.Unit(0).SubUnit(0).MultiData.TYPES(dstrTYPE).ITEMS())
                        {
                            dListReceiveData.Add(new MultiUseDataBody(dstrTYPE, dstrITEM));
                        }
                    }
                    //dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_BASIC_PATH"));
                    //dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_SAMPLING_TIME"));
                    //dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_HOST_IP"));
                    //dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_LOGIN_ID"));
                    //dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_LOGIN_PW"));
                }
                #endregion
                #region 일부 타입 리스트 조회시
                else  // n 개 조회
                {
                    for (int i = 0; i < dintTypeCount; i++)
                    {
                        
                        string dstrType = msgTran.Primary().Item("DATA_TYPE" + i.ToString()).Getvalue().ToString().Trim();

                        InfoAct.clsMultiUseDataByTYPE clsTYPE = this.pInfo.Unit(0).SubUnit(0).MultiData.TYPES(dstrType);

                        if (clsTYPE != null)
                        {
                            int dintBodyCount = int.Parse(msgTran.Primary().Item("L4" + i.ToString()).Getvalue().ToString().Trim());

                            if (dintBodyCount == 0)
                            {
                                foreach (string dstrITEM in this.pInfo.Unit(0).SubUnit(0).MultiData.TYPES(dstrType).ITEMS())
                                {
                                    dListReceiveData.Add(new MultiUseDataBody(dstrType, dstrITEM));
                                }

                                //dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_BASIC_PATH"));
                                //dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_SAMPLING_TIME"));
                                //dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_HOST_IP"));
                                //dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_LOGIN_ID"));
                                //dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_LOGIN_PW"));
                            }
                            else  // n 의 n 개 조회
                            {
                                for (int j = 0; j < dintBodyCount; j++)
                                {
                                    //string dstrBodyName = msgTran.Primary().Item("ITEM_NAME" + i.ToString()).Getvalue().ToString().Trim();
                                    string dstrBodyName = msgTran.Primary().Item("ITEM_NAME" + i.ToString() + j.ToString()).Getvalue().ToString().Trim();

                                    dListReceiveData.Add(new MultiUseDataBody(dstrType, dstrBodyName));


                                    if (!clsTYPE.ItemContains(dstrBodyName))
                                    {
                                        ack = ACK.At_least_One_data_invalid;
                                        break;
                                    }
                                    //switch (dstrBodyName)
                                    //{
                                    //    case "MCC_BASIC_PATH":
                                    //        dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_BASIC_PATH"));
                                    //        break;
                                    //    case "MCC_SAMPLING_TIME":
                                    //        dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_SAMPLING_TIME"));
                                    //        break;
                                    //    case "MCC_HOST_IP":
                                    //        dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_HOST_IP"));
                                    //        break;
                                    //    case "MCC_LOGIN_ID":
                                    //        dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_LOGIN_ID"));
                                    //        break;
                                    //    case "MCC_LOGIN_PW":
                                    //        dListReceiveData.Add(new MultiUseDataBody("MCC", "MCC_LOGIN_PW"));
                                    //        break;
                                    //    default:
                                    //        ack = ACK.At_least_One_data_invalid;    // 지정된 네임이 아니면 나크
                                    //        break;
                                    //}
                                }
                            }
                        }
                        else  // 타입이 MCC가 아니면 나크
                        {
                            ack = ACK.At_least_One_data_invalid;    
                            break;
                        }
                    }
                }
                #endregion


                if (ack == ACK.At_least_One_data_invalid)
                {
                    msgTran.Secondary().Item("L2").Putvalue(0);
                    msgTran.Secondary().Item("ACK").Putvalue((int)ack);
                }
                else if( ack == ACK.OK)
                {
                    #region MultiUseData Read
                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.Msg2MCC, "GET");
                    subWaitDuring(1);
                    #endregion

                    #region ITEM Setting
                    msgTran.Secondary().Item("L2").Putvalue(dListReceiveData.Count);
                    msgTran.Secondary().Item("ACK").Putvalue((int)ack);
                    for (int i = 0; i < dListReceiveData.Count; i++)
                    {
                        InfoAct.clsMultiUseDataByITEM clsITEM = this.pInfo.Unit(0).SubUnit(0).MultiData.TYPES(dListReceiveData[i].type).ITEMS(dListReceiveData[i].name);


                        if (clsITEM != null)
                        {

                            msgTran.Secondary().Item("DATA_TYPE" + i.ToString()).Putvalue(clsITEM.DATA_TYPE);
                            msgTran.Secondary().Item("ITEM_NAME" + i.ToString()).Putvalue(clsITEM.ITEM);
                            msgTran.Secondary().Item("ITEM_VALUE" + i.ToString()).Putvalue(clsITEM.VALUE);
                            msgTran.Secondary().Item("REFERENCE" + i.ToString()).Putvalue(clsITEM.REFERENCE);
                        }
                        else
                        {
                            msgTran.Secondary().Item("L2").Putvalue(0);
                            msgTran.Secondary().Item("ACK").Putvalue((int)ack);
                        }
                        
                        
                        //switch (dListReceiveData[i].name)
                        //{
                        //    case "MCC_BASIC_PATH":
                        //        msgTran.Secondary().Item("DATA_TYPE" + i.ToString()).Putvalue(dListReceiveData[i].type);
                        //        msgTran.Secondary().Item("ITEM_NAME" + i.ToString()).Putvalue(dListReceiveData[i].name); 
                        //        msgTran.Secondary().Item("ITEM_VALUE" + i.ToString()).Putvalue(pInfo.All.MCCNetworkBasicPath); 
                        //        msgTran.Secondary().Item("REFERENCE" + i.ToString()).Putvalue("PATH");
                        //        break;
                        //    case "MCC_HOST_IP":
                        //        msgTran.Secondary().Item("DATA_TYPE" + i.ToString()).Putvalue(dListReceiveData[i].type);
                        //        msgTran.Secondary().Item("ITEM_NAME" + i.ToString()).Putvalue(dListReceiveData[i].name); 
                        //        msgTran.Secondary().Item("ITEM_VALUE" + i.ToString()).Putvalue(pInfo.All.MCCNetworkPath);
                        //        msgTran.Secondary().Item("REFERENCE" + i.ToString()).Putvalue("IP");
                        //        break;
                        //    case "MCC_LOGIN_ID":
                        //        msgTran.Secondary().Item("DATA_TYPE" + i.ToString()).Putvalue(dListReceiveData[i].type);
                        //        msgTran.Secondary().Item("ITEM_NAME" + i.ToString()).Putvalue(dListReceiveData[i].name); 
                        //        msgTran.Secondary().Item("ITEM_VALUE" + i.ToString()).Putvalue(pInfo.All.MCCNetworkUserID);
                        //        msgTran.Secondary().Item("REFERENCE" + i.ToString()).Putvalue("ID");
                        //        break;
                        //    case "MCC_LOGIN_PW":
                        //        msgTran.Secondary().Item("DATA_TYPE" + i.ToString()).Putvalue(dListReceiveData[i].type);
                        //        msgTran.Secondary().Item("ITEM_NAME" + i.ToString()).Putvalue(dListReceiveData[i].name); 
                        //        msgTran.Secondary().Item("ITEM_VALUE" + i.ToString()).Putvalue(pInfo.All.MCCNetworkPassword);
                        //        msgTran.Secondary().Item("REFERENCE" + i.ToString()).Putvalue("PW");
                        //        break;
                        //    case "MCC_SAMPLING_TIME":
                        //        msgTran.Secondary().Item("DATA_TYPE" + i.ToString()).Putvalue(dListReceiveData[i].type);
                        //        msgTran.Secondary().Item("ITEM_NAME" + i.ToString()).Putvalue(dListReceiveData[i].name); 
                        //        msgTran.Secondary().Item("ITEM_VALUE" + i.ToString()).Putvalue(pInfo.All.MCCFileUploadTime);
                        //        msgTran.Secondary().Item("REFERENCE" + i.ToString()).Putvalue("MINUTE");
                        //        break;
                        //}
                    }
                    #endregion
                }

                funSendReply(msgTran);
                return;
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #region funPrimaryReceive 변경전
#if false
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrModuleID = "";
            string[] dstrDATATYPE;
            string[] dstrITEMNAME;
            string[] dstrITEMVALUE;

            int dintDATACOUNT = 0;
            //int dintDATACOUNT3 = 0;
            int dintACK = 0;

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dintDATACOUNT = Convert.ToInt32(msgTran.Primary().Item("L2").Getvalue());
                


                if (dintDATACOUNT == 0)
                {
                    dintDATACOUNT = 5;
                    msgTran.Secondary().Item("L2").Putvalue(dintDATACOUNT);
                    dstrDATATYPE = new string[dintDATACOUNT];
                    dstrITEMNAME = new string[dintDATACOUNT];
                    dstrITEMVALUE = new string[dintDATACOUNT];

                    dstrITEMNAME[0] = "MCC_BASIC_PATH";
                    dstrITEMNAME[1] = "MCC_SAMPLING_TIME";
                    dstrITEMNAME[2] = "MCC_HOST_IP";
                    dstrITEMNAME[3] = "MCC_LOGIN_ID";
                    dstrITEMNAME[4] = "MCC_LOGIN_PW";
                    

                    for (int dintLoop = 0; dintLoop < dintDATACOUNT; dintLoop++)
                    {
                        dstrDATATYPE[dintLoop] = "MCC";

                        msgTran.Secondary().Item("DATA_TYPE" + dintLoop).Putvalue(dstrDATATYPE[dintLoop]);
                        msgTran.Secondary().Item("ITEM_NAME" + dintLoop).Putvalue(dstrITEMNAME[dintLoop]);
                    }
                }
                else
                {

                    dintDATACOUNT = Convert.ToInt32(msgTran.Primary().Item("L4").Getvalue());
                    msgTran.Secondary().Item("L2").Putvalue(dintDATACOUNT);
                    dstrDATATYPE = new string[dintDATACOUNT];
                    dstrITEMNAME = new string[dintDATACOUNT];
                    dstrITEMVALUE = new string[dintDATACOUNT];

                    for (int dintLoop = 0; dintLoop < dintDATACOUNT; dintLoop++)
                    {
                        dstrDATATYPE[dintLoop] = msgTran.Primary().Item("DATA_TYPE0").Getvalue().ToString().Trim();
                        dstrITEMNAME[dintLoop] = msgTran.Primary().Item("ITEM_NAME0" + dintLoop).Getvalue().ToString().Trim();

                        msgTran.Secondary().Item("DATA_TYPE" + dintLoop).Putvalue(dstrDATATYPE[dintLoop]);
                        msgTran.Secondary().Item("ITEM_NAME" + dintLoop).Putvalue(dstrITEMNAME[dintLoop]);
                    }
                }

                

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.pInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("ACK").Putvalue("1");
                    funSendReply(msgTran);
                    return;
                }

                //pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.GetMultiUseData);
                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.Msg2MCC, "GET");
                subWaitDuring(1);


                for (int dintLoop = 0; dintLoop < dintDATACOUNT; dintLoop++)
                {
                    if (dstrDATATYPE[dintLoop] == "MCC")
                    {
                        switch (dstrITEMNAME[dintLoop])
                        {
                            case "MCC_BASIC_PATH":
                                msgTran.Secondary().Item("ITEM_VALUE" + dintLoop).Putvalue(pInfo.All.MCCNetworkBasicPath);
                                msgTran.Secondary().Item("REFERENCE" + dintLoop).Putvalue("PATH");
                                break;
                            case "MCC_HOST_IP":
                                msgTran.Secondary().Item("ITEM_VALUE" + dintLoop).Putvalue(pInfo.All.MCCNetworkPath);
                                msgTran.Secondary().Item("REFERENCE" + dintLoop).Putvalue("IP");
                                break;
                            case "MCC_LOGIN_ID":
                                msgTran.Secondary().Item("ITEM_VALUE" + dintLoop).Putvalue(pInfo.All.MCCNetworkUserID);
                                msgTran.Secondary().Item("REFERENCE" + dintLoop).Putvalue("ID");
                                break;
                            case "MCC_LOGIN_PW":
                                msgTran.Secondary().Item("ITEM_VALUE" + dintLoop).Putvalue(pInfo.All.MCCNetworkPassword);
                                msgTran.Secondary().Item("REFERENCE" + dintLoop).Putvalue("PW");
                                break;
                            case "MCC_SAMPLING_TIME":
                                msgTran.Secondary().Item("ITEM_VALUE" + dintLoop).Putvalue(pInfo.All.MCCFileUploadTime);
                                msgTran.Secondary().Item("REFERENCE" + dintLoop).Putvalue("MINUTE");
                                break;
                            default:
                                dintACK = 2;
                                msgTran.Secondary().Item("ITEM_VALUE" + dintLoop).Putvalue("");
                                msgTran.Secondary().Item("REFERENCE" + dintLoop).Putvalue("");
                                break;
                        }
                    }
                    else
                    {
                        dintACK = 2;
                        msgTran.Secondary().Item("ITEM_VALUE" + dintLoop).Putvalue("");
                        msgTran.Secondary().Item("REFERENCE" + dintLoop).Putvalue("");
                    }
                }
                msgTran.Secondary().Item("ACK").Putvalue(dintACK);

                funSendReply(msgTran);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
#endif
        #endregion

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


        //2초간만 Delay 후 빠져나간다.
        public void subWaitDuring(int intDelaySecond)
        {
            long dlngTimeTick = 0;
            long dlngSec = 0;

            try
            {
                dlngTimeTick = DateTime.Now.Ticks;

                while (true)
                {
                    dlngSec = DateTime.Now.Ticks - dlngTimeTick;
                    if (dlngSec < 0) dlngTimeTick = 0;
                    if (dlngSec > intDelaySecond * TimeSpan.TicksPerSecond)     //지정한 시간(2초)만큼 Delay를 한다.
                    {
                        return;
                    }

                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }


        #endregion
    }
}
