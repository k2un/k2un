using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;
using InfoAct;

namespace HostAct
{
    public class clsS6F11_PPIDChange : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_PPIDChange Instance = new clsS6F11_PPIDChange();
        #endregion

        #region Constructors
        public clsS6F11_PPIDChange()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11_PPIDChange";
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

                try
                {
                    if (pInfo.CEID(401).Report == false)
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

                string PPID = arrayEvent[1];
                string PPIDReportType = arrayEvent[2];


                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("CEID").Putvalue(401);
                pMsgTran.Primary().Item("RPTID").Putvalue(401);
                pMsgTran.Primary().Item("PPID").Putvalue(PPID);
                pMsgTran.Primary().Item("PPCINFO").Putvalue(PPIDReportType);
                pMsgTran.Primary().Item("LCTIME").Putvalue(pInfo.Unit(0).SubUnit(0).HOSTPPID(PPID).DateTime);

                if (PPIDReportType == "3")
                {
                    pInfo.Unit(0).SubUnit(0).RemoveHOSTPPID(PPID);
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

        public bool funPPID_DBDelete()
        {
            bool dbolDBUpdateFlag = false;
            string dstrSQL = "";
            try
            {
                dstrSQL = string.Format("DELETE FROM `tbHOSTPPID` ");
                dbolDBUpdateFlag = DBAct.clsDBAct.funExecuteQuery(dstrSQL);

            }
            catch (Exception)
            {
            }
            return dbolDBUpdateFlag;
        }

        public bool funPPID_DBInsert()
        {
            bool dbolDBUpdateFlag = false;
            string dstrSQL = "";
            try
            {

                foreach (string PPIDName in pInfo.Unit(0).SubUnit(0).HOSTPPID())
                {
                    clsHOSTPPID PPID = pInfo.Unit(0).SubUnit(0).HOSTPPID(PPIDName);

                    dstrSQL = string.Format(@"INSERT INTO tbHOSTPPID VALUES ('{0}', '{1}', '{2}', '{3}','{4}');", PPID.HostPPID, PPID.Tickness, PPID.CLEANER_EQPPPID, PPID.Oven1_EQPPPID, PPID.Oven2_EQPPPID);
                    dbolDBUpdateFlag = DBAct.clsDBAct.funExecuteQuery(dstrSQL);
                }

            }
            catch (Exception)
            {
            }
            return dbolDBUpdateFlag;
        }
        #endregion
    }
}
