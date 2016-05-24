using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;
using System.Collections;

namespace HostAct
{
    public class clsS16F111 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS16F111 Instance = new clsS16F111();
        #endregion

        #region Constructors
        public clsS16F111()
        {
            this.intStream = 16;
            this.intFunction =111;
            this.strPrimaryMsgName = "S16F111";
            this.strSecondaryMsgName = "S16F112";
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
            string[] arrParamName;
            string[] arrParamValue;
            string dstrHGLSID = "";

            int dintMode = 1;
            string dstrSQL = "";
            string dstrMODE = "";
            string dstrLogData = "";
            int dintCenterIndex = 0;

            try
            {
                arrayEvent = strParameters.Split(',');

                pMsgTran = pSecsDrv.MakeTransaction(this.strPrimaryMsgName);

                dstrHGLSID = arrayEvent[1];

                pMsgTran.Primary().Item("MODULEID").Putvalue(this.pInfo.Unit(3).SubUnit(0).ModuleID);

                pMsgTran.Primary().Item("L2").Putvalue(1);

                InfoAct.clsAPC CurrentAPC = this.pInfo.APC(dstrHGLSID);
                pMsgTran.Primary().Item("H_GLASSID").Putvalue(CurrentAPC.GLSID);
                pMsgTran.Primary().Item("JOBID").Putvalue(CurrentAPC.JOBID);
                pMsgTran.Primary().Item("RECIPE").Putvalue(CurrentAPC.EQPPPID);
                pMsgTran.Primary().Item("SET_TIME").Putvalue(CurrentAPC.SetTime.ToString("yyyyMMddHHmmssff"));

                arrParamName = CurrentAPC.ParameterName;
                arrParamValue = CurrentAPC.ParameterValue;

                pMsgTran.Primary().Item("L4").Putvalue(CurrentAPC.ParameterName.Length);
                for (int dintLoop2 = 0; dintLoop2 < CurrentAPC.ParameterName.Length; dintLoop2++)
                {
                    pMsgTran.Primary().Item("P_PARM_NAME"+0 + dintLoop2).Putvalue(arrParamName[dintLoop2]);
                    pMsgTran.Primary().Item("P_PARM_VALUE"+0 + dintLoop2).Putvalue(arrParamValue[dintLoop2]);
                }

                CurrentAPC.State = "2"; // Running

                //DB Update
                pInfo.subProcessDataStatusSet(InfoAct.clsInfo.ProcessDataType.APC, dstrHGLSID);
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
