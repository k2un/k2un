using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_Scrap : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_Scrap Instance = new clsS6F11_Scrap();
        #endregion

        #region Constructors
        public clsS6F11_Scrap()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11_SCRAP";
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
            string dstrLOTID = "";
            string dstrGLSID = "";
            int dintUnitID = 0;
            int dintSubunitID = 0;
            try
            {

                arrayEvent = strParameters.Split(',');
                int dintCEID = 331;                       //CEID
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
                dstrLOTID = arrayEvent[1].Trim();
                dstrGLSID = arrayEvent[2].Trim();
                dintUnitID = Convert.ToInt32(arrayEvent[3]);
                dintSubunitID = Convert.ToInt32(arrayEvent[4]);
                
                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(pInfo.All.DataID);
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);

                pMsgTran.Primary().Item("RPTID").Putvalue(100); //FIX
                pMsgTran.Primary().Item("CRST").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("EQST").Putvalue(pInfo.Unit(0).SubUnit(0).EQPState);

                pMsgTran.Primary().Item("RPTID_SUB").Putvalue(321);
                InfoAct.clsGLS CurrentGLS = pInfo.LOTID(dstrLOTID).GLSID(dstrGLSID);

                pMsgTran.Primary().Item("UNITID").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).ReportUnitID);
                pMsgTran.Primary().Item("LOTID").Putvalue(CurrentGLS.LOTID);
                pMsgTran.Primary().Item("PTID").Putvalue(CurrentGLS.IPID);
                pMsgTran.Primary().Item("CSTID").Putvalue(CurrentGLS.ICID);
                pMsgTran.Primary().Item("SLOTID").Putvalue(CurrentGLS.SlotID);
                pMsgTran.Primary().Item("GLSID").Putvalue(CurrentGLS.GLSID);
                pMsgTran.Primary().Item("GLSJUDGE").Putvalue(CurrentGLS.GLSJudge);
                pMsgTran.Primary().Item("TEXT").Putvalue(CurrentGLS.ScrapComment);

                funSendReply(pMsgTran);

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
