using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F13 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F13 Instance = new clsS2F13();
        #endregion

        #region Constructors
        public clsS2F13()
        {
            this.IntStream = 2;
            this.IntFunction = 13;
            this.StrPrimaryMsgName = "S2F13";
            this.StrSecondaryMsgName = "S2F14";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            int dintECIDCount = -1;
            bool ErrorFlag = false;
            int[] arrCon;
            try
            {
                int.TryParse(msgTran.Primary().Item("ECIDCNT").Getvalue().ToString(), out dintECIDCount);
                arrCon = new int[dintECIDCount];

                int dintECID = -1;
                for (int dintLoop = 0; dintLoop < dintECIDCount; dintLoop++)
                {
                    int.TryParse(msgTran.Primary().Item("ECID" + dintLoop).Getvalue().ToString(), out dintECID);
                    if (pInfo.Unit(0).SubUnit(0).ECID(dintECID) == null)
                    {
                        //존재하지 않는 ECID 조회
                        ErrorFlag = true;
                        break;
                    }
                    else
                    {
                        arrCon[dintLoop] = dintECID;
                    }
                }

                if (ErrorFlag == false)
                {
                    if (dintECIDCount == 0)
                    {
                        msgTran.Secondary().Item("ECVCNT").Putvalue(pInfo.Unit(0).SubUnit(0).ECIDCount);
                        for (int dintLoop2 = 0; dintLoop2 < pInfo.Unit(0).SubUnit(0).ECIDCount; dintLoop2++)
                        {
                            msgTran.Secondary().Item("ECV" + dintLoop2).Putvalue(pInfo.Unit(0).SubUnit(0).ECID(dintLoop2 + 1).DEF);
                        }
                    }
                    else
                    {
                        msgTran.Secondary().Item("ECVCNT").Putvalue(dintECIDCount);
                        for (int dintLoop2 = 0; dintLoop2 < dintECIDCount; dintLoop2++)
                        {
                            msgTran.Secondary().Item("ECV" + dintLoop2).Putvalue(pInfo.Unit(0).SubUnit(0).ECID(arrCon[dintLoop2]).DEF);
                        }
                    }
                }
                else
                {
                    msgTran.Secondary().Item("ECVCNT").Putvalue(0);
                }
               

                funSendReply(msgTran);
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
