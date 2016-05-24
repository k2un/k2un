using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Text;
using Microsoft.VisualBasic;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F37 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F37 Instance = new clsS2F37();
        #endregion

        #region Constructors
        public clsS2F37()
        {
            this.IntStream = 2;
            this.IntFunction = 37;
            this.StrPrimaryMsgName = "S2F37";
            this.StrSecondaryMsgName = "S2F38";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            int dintERACK = 0;
            int dintCEIDCount = 0;
            string dstrCEED = "";
            try
            {
                dstrCEED = msgTran.Primary().Item("CEED").Getvalue().ToString().Trim();
                dintCEIDCount = Convert.ToInt32(msgTran.Primary().Item("CEIDCNT").Getvalue().ToString());

                if (dstrCEED == "0" || dstrCEED == "1")
                {

                    //CEID 존재 여부 확인
                    if (dintCEIDCount != 0)
                    {
                        int intCEID = 0;
                        for (int dintLoop = 0; dintLoop < dintCEIDCount; dintLoop++)
                        {
                            intCEID = Convert.ToInt32(msgTran.Primary().Item("CEID" + dintLoop).Getvalue().ToString());

                            if (pInfo.CEID(intCEID) == null)
                            {
                                dintERACK = 1;
                                break;
                            }
                        }

                        if (dintERACK == 0) //0이면 변경가능
                        {
                            for (int dintLoop = 0; dintLoop < dintCEIDCount; dintLoop++)
                            {
                                intCEID = Convert.ToInt32(msgTran.Primary().Item("CEID" + dintLoop).Getvalue().ToString());

                                if (dstrCEED == "0")
                                {
                                    pInfo.CEID(intCEID).Report = true;
                                }
                                else
                                {
                                    pInfo.CEID(intCEID).Report = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (int dintCEID in pInfo.CEID())
                        {
                            InfoAct.clsCEID ceid = pInfo.CEID(dintCEID);

                            if (dstrCEED == "0") //사용
                            {
                                ceid.Report = true;
                            }
                            else
                            {
                                ceid.Report = false;
                            }
                        }
                    }
                }
                else
                {
                    dintERACK = 2;
                }

                msgTran.Secondary().Item("ERACK").Putvalue(dintERACK);
                funSendReply(msgTran);
                
                if (dintERACK == 0)
                {
                    pInfo.funCEID_DBDelete();
                    pInfo.funCEID_DBInsert();
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
