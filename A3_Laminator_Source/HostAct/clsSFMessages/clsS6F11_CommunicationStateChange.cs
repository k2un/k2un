using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_CommunicationStateChange : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_CommunicationStateChange Instance = new clsS6F11_CommunicationStateChange();
        #endregion

        #region Constructors
        public clsS6F11_CommunicationStateChange()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11_CommunicationStateChange";
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
                int dintCEID = Convert.ToInt32(arrayEvent[1]);      //CEID

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

                int dintUnitID = Convert.ToInt32(arrayEvent[2]);    //UnitID
                int dintSubUnitID = Convert.ToInt32(arrayEvent[3]); //SubUnitID
                string strLOTID = arrayEvent[4].Trim();                    //LOTID                     
                string strGLSID = arrayEvent[5].Trim();                    //GLSID

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                InfoAct.clsSubUnit subUnit = pInfo.Unit(dintUnitID).SubUnit(0);

                pMsgTran.Primary().Item("DATAID").Putvalue(pInfo.All.DataID);
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("RPTID").Putvalue(100); //FIX
                pMsgTran.Primary().Item("CRST").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("EQST").Putvalue(pInfo.Unit(0).SubUnit(0).EQPState);

                pMsgTran.Primary().Item("RPTID_SUB").Putvalue(101);                
                pMsgTran.Primary().Item("CMST").Putvalue(pInfo.Unit(0).SubUnit(0).CMST);

                return pMsgTran;
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                //pInfo.All.DataID--; pInfo.All.DataID--;

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



