using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11MaterialProcessStatusEvent : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11MaterialProcessStatusEvent Instance = new clsS6F11MaterialProcessStatusEvent();
        #endregion

        #region Constructors
        public clsS6F11MaterialProcessStatusEvent()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11MaterialProcessStatusEvent";
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
            string strGLSID = "";
            string[] arrayEvent;
            try
            {
                arrayEvent = strParameters.Split(',');

                int dintCEID = 1025;
                
                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(3).SubUnit(Convert.ToInt32(arrayEvent[4])).ModuleID);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(pInfo.Unit(0).SubUnit(0).EQPState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(pInfo.Unit(0).SubUnit(0).EQPProcessState);

                pMsgTran.Primary().Item("L3_1_Count").Putvalue(1);
                strGLSID = arrayEvent[2];
                InfoAct.clsGLS CurrentGLS = pInfo.GLSID(strGLSID);

                if (CurrentGLS == null)
                {
                    pMsgTran.Primary().Item("H_GLASS_ID" + 0).Putvalue("");
                    pMsgTran.Primary().Item("MATERIAL_ID" + 0).Putvalue("");
                    pMsgTran.Primary().Item("USE_COUNT" + 0).Putvalue("");
                    pMsgTran.Primary().Item("PROCESS_ACT" + 0).Putvalue("");
                    pMsgTran.Primary().Item("LIBRARYID" + 0).Putvalue("");
                    pMsgTran.Primary().Item("LOCATION" + 0).Putvalue("");
                    pMsgTran.Primary().Item("ADDITION_NAME1" + 0).Putvalue("");
                    pMsgTran.Primary().Item("ADDITION_NAME2" + 0).Putvalue("");
                    pMsgTran.Primary().Item("ADDITION_NAME3" + 0).Putvalue("");
                    pMsgTran.Primary().Item("ADDITION_NAME4" + 0).Putvalue("");
                }
                else
                {
                    pMsgTran.Primary().Item("H_GLASS_ID" + 0).Putvalue(CurrentGLS.H_PANELID);
                    pMsgTran.Primary().Item("MATERIAL_ID" + 0).Putvalue(CurrentGLS.FilmID);
                    pMsgTran.Primary().Item("USE_COUNT" + 0).Putvalue(CurrentGLS.USE_COUNT.PadLeft(3, '0'));
                    pMsgTran.Primary().Item("PROCESS_ACT" + 0).Putvalue("");
                    pMsgTran.Primary().Item("LIBRARYID" + 0).Putvalue("");
                    pMsgTran.Primary().Item("LOCATION" + 0).Putvalue("");
                    pMsgTran.Primary().Item("ADDITION_NAME1" + 0).Putvalue("");
                    pMsgTran.Primary().Item("ADDITION_NAME2" + 0).Putvalue("");
                    pMsgTran.Primary().Item("ADDITION_NAME3" + 0).Putvalue("");
                    pMsgTran.Primary().Item("ADDITION_NAME4" + 0).Putvalue("");
                }
            
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
