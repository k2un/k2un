using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS8F1 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS8F1 Instance = new clsS8F1();
        #endregion

        #region Constructors
        public clsS8F1()
        {
            this.IntStream = 8;
            this.IntFunction = 1;
            this.StrPrimaryMsgName = "S8F1MultiUseDataSet";
            this.StrSecondaryMsgName = "S8F2";
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
            string[] dstrDATATYPE;
            string[] dstrITEMNAME;
            string[] dstrITEMVALUE;
            string[] dstrREFERENCE;
            string[] dstrArrTemp;

            int dintDATACOUNT = 0;
            int dintACK = 0;
            int dintTemp = 0;
            int[] dintEAC;

            bool dbolNACK = false;

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dintDATACOUNT = Convert.ToInt32(msgTran.Primary().Item("L2").Getvalue());
                msgTran.Secondary().Item("L2").Putvalue(dintDATACOUNT);


                dstrDATATYPE = new string[dintDATACOUNT];
                dstrITEMNAME = new string[dintDATACOUNT];
                dstrITEMVALUE = new string[dintDATACOUNT];
                dstrREFERENCE = new string[dintDATACOUNT];
                dintEAC = new int[dintDATACOUNT];

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("ACK").Putvalue("1");

                    funSendReply(msgTran);

                    return;
                }

                //Secondary에 Data 저장
                for (int dintLoop = 0; dintLoop < dintDATACOUNT; dintLoop++)
                {
                    dstrDATATYPE[dintLoop] = msgTran.Primary().Item("DATA_TYPE" + dintLoop).Getvalue().ToString().Trim();
                    dstrITEMNAME[dintLoop] = msgTran.Primary().Item("ITEM_NAME" + dintLoop).Getvalue().ToString().Trim();
                    dstrITEMVALUE[dintLoop] = msgTran.Primary().Item("ITEM_VALUE" + dintLoop).Getvalue().ToString().Trim();
                    dstrREFERENCE[dintLoop] = msgTran.Primary().Item("REFERENCE" + dintLoop).Getvalue().ToString().Trim();


                    InfoAct.clsMultiUseDataByTYPE clsTYPE = this.pInfo.Unit(0).SubUnit(0).MultiData.TYPES(dstrDATATYPE[dintLoop]);

                    if(clsTYPE != null)
                    {
                        InfoAct.clsMultiUseDataByITEM clsITEM = clsTYPE.ITEMS(dstrITEMNAME[dintLoop]);

                        if (clsITEM != null)
                        {
                            if (dstrREFERENCE[dintLoop ] != clsITEM.REFERENCE)
                            {
                                dintACK = 2;
                                dintEAC[dintLoop ] = 4;
                            }
                            else
                            {
                                dintEAC[dintLoop ] = 0;

                                #region IP check
                                if (clsITEM.REFERENCE.Contains("IP") || clsITEM.ITEM.Contains("IP"))
                                {
                                    dstrArrTemp = dstrITEMVALUE[dintLoop ].Split('.');
                                    if (dstrArrTemp.Length != 4)
                                    {
                                        dintACK = 2;
                                        dintEAC[dintLoop ] = 3;
                                    }
                                    else
                                    {
                                        for (int dintLoopTemp = 0; dintLoopTemp < dstrArrTemp.Length; dintLoopTemp++)
                                        {
                                            int dintIPTemp = 0;

                                            if (int.TryParse(dstrArrTemp[dintLoopTemp], out dintIPTemp))
                                            {
                                                if (dintIPTemp > 255 || dintIPTemp < 0)
                                                {
                                                    dintACK = 2;
                                                    dintEAC[dintLoop ] = 3;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                dintACK = 2;
                                                dintEAC[dintLoop] = 3;
                                                break;
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #region TIME check
                                else if (clsITEM.REFERENCE.Equals("MINUTE") || clsITEM.ITEM.Contains("TIME"))
                                {
                                    int dintTimeTemp = 0;

                                    if (int.TryParse(dstrITEMVALUE[dintLoop ], out dintTimeTemp))
                                    {
                                        dintEAC[dintLoop ] = 0;
                                    }
                                    else
                                    {
                                        dintACK = 2;
                                        dintEAC[dintLoop ] = 3;
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            dintACK = 2;
                            dintEAC[dintLoop ] = 2;
                        }
                    }
                    else
                    {
                        dintACK = 2;
                        dintEAC[dintLoop ] = 1;
                    }

                    if (dintACK != 0) dbolNACK = true;

                }

                for (int dintLoop = 0; dintLoop < dintDATACOUNT; dintLoop++)
                {
                    msgTran.Secondary().Item("DATA_TYPE" + dintLoop).Putvalue(dstrDATATYPE[dintLoop]);
                    msgTran.Secondary().Item("ITEM_NAME" + dintLoop).Putvalue(dstrITEMNAME[dintLoop]);
                    msgTran.Secondary().Item("ITEM_VALUE" + dintLoop).Putvalue(dstrITEMVALUE[dintLoop]);
                    msgTran.Secondary().Item("REFERENCE" + dintLoop).Putvalue(dstrREFERENCE[dintLoop]);

                    msgTran.Secondary().Item("EAC" + dintLoop).Putvalue(dintEAC[dintLoop]);
                }


                msgTran.Secondary().Item("ACK").Putvalue(dintACK);
                funSendReply(msgTran);

                if (!dbolNACK)
                {
                    StringBuilder sb = new StringBuilder("SET;");

                    for (int dintLoop = 0; dintLoop < dstrITEMNAME.Length; dintLoop++)
                    {
                        pInfo.Unit(0).SubUnit(0).MultiData.TYPES("MCC").ITEMS(dstrITEMNAME[dintLoop].Trim()).VALUE = dstrITEMVALUE[dintLoop].Trim();
                        
                        //sb.Append(dstrITEMNAME[dintLoop].Trim());
                        //sb.Append("=");
                        //sb.Append(dstrITEMVALUE[dintLoop].Trim());

                        //if (dintLoop < dstrITEMNAME.Length - 1) sb.Append(",");
                    }

                    //this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.Msg2MCC, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
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
