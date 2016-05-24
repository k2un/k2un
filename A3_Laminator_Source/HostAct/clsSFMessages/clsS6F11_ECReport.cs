using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_ECReport : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_ECReport Instance = new clsS6F11_ECReport();
        #endregion

        #region Constructors
        public clsS6F11_ECReport()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11_ECReport";
            this.StrSecondaryMsgName = "S6F12";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            try
            {
                
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
                int dintCEID = 109;                       //CEID
                try
                {
                    if (pInfo.CEID(dintCEID).Report == false)
                    {
                        return null;
                    }
                }
                catch (Exception)
                {

                }
                if (pInfo.All.DataID >= 9999)
                {
                    pInfo.All.DataID = -1;
                }
                pInfo.All.DataID++;

                string[] ECID = arrayEvent[1].Split('=');
                string[] ECV = arrayEvent[2].Split('=');

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(pInfo.All.DataID);
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);

                pMsgTran.Primary().Item("RPTID").Putvalue(100); //FIX
                pMsgTran.Primary().Item("CRST").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("EQST").Putvalue(pInfo.Unit(0).SubUnit(0).EQPState);

                pMsgTran.Primary().Item("RPTID_SUB").Putvalue(109);

                if (ECID.Length == 1)
                {
                    pMsgTran.Primary().Item("ECLIST").Putvalue(0);

                }
                else
                {
                    pMsgTran.Primary().Item("ECLIST").Putvalue(ECID.Length - 1);

                    for (int dintLoop = 0; dintLoop < ECID.Length - 1; dintLoop++)
                    {
                        pMsgTran.Primary().Item("ECID" + dintLoop).Putvalue(ECID[dintLoop]);
                        pMsgTran.Primary().Item("ECV" + dintLoop).Putvalue(ECV[dintLoop]);
                    }
                }

                return pMsgTran;
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                pInfo.All.DataID--; pInfo.All.DataID--;

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
