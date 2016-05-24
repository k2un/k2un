using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_ComponentReport : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_ComponentReport Instance = new clsS6F11_ComponentReport();
        #endregion

        #region Constructors
        public clsS6F11_ComponentReport()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11_ComponentReport";
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

                if (pInfo.LOTID(strLOTID) == null || pInfo.LOTID(strLOTID).GLSID(strGLSID) == null)
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Component Report Error!! LOTID, GLSID Error => LOTID : {0}, GLSID : {1}", strLOTID, strGLSID));
                    pInfo.All.DataID--;
                    return null;
                }
                else
                {
                    if (pInfo.LOTID(strLOTID).GLSID(strGLSID).ScrapFlag == false)
                    {
                        pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                        InfoAct.clsSubUnit subUnit = pInfo.Unit(dintUnitID).SubUnit(0);

                        pMsgTran.Primary().Item("DATAID").Putvalue(pInfo.All.DataID);
                        pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                        pMsgTran.Primary().Item("RPTID").Putvalue(100); //FIX
                        pMsgTran.Primary().Item("CRST").Putvalue(pInfo.All.ControlState);
                        pMsgTran.Primary().Item("EQST").Putvalue(pInfo.Unit(0).SubUnit(0).EQPState);

                        pMsgTran.Primary().Item("RPTID_SUB").Putvalue(321);
                        InfoAct.clsGLS CurrentGLS = pInfo.LOTID(strLOTID).GLSID(strGLSID);

                        pMsgTran.Primary().Item("UNITID").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).ReportUnitID);
                        pMsgTran.Primary().Item("LOTID").Putvalue(CurrentGLS.LOTID);
                        pMsgTran.Primary().Item("IPID").Putvalue(CurrentGLS.IPID);
                        pMsgTran.Primary().Item("OPID").Putvalue(CurrentGLS.OPID);
                        pMsgTran.Primary().Item("ICID").Putvalue(CurrentGLS.ICID);
                        pMsgTran.Primary().Item("OCID").Putvalue(CurrentGLS.OCID);
                        pMsgTran.Primary().Item("PPID").Putvalue(CurrentGLS.HostPPID);
                        pMsgTran.Primary().Item("FSLOTID").Putvalue(CurrentGLS.FSLOTID);
                        pMsgTran.Primary().Item("TSLOTID").Putvalue(CurrentGLS.TSLOTID);
                        pMsgTran.Primary().Item("RGLSID").Putvalue(CurrentGLS.RGLSID);
                        pMsgTran.Primary().Item("HGLSID").Putvalue(CurrentGLS.GLSID);
                        pMsgTran.Primary().Item("GLSJUDGE").Putvalue(CurrentGLS.GLSJudge);

                        return pMsgTran;
                    }
                    else
                    {
                        return null;
                    }
                }
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



