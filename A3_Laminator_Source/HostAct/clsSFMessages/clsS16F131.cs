using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;

namespace HostAct
{
    public class clsS16F131 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS16F131 Instance = new clsS16F131();
        #endregion

        #region Constructors
        public clsS16F131()
        {
            this.intStream = 16;
            this.intFunction = 131;
            this.strPrimaryMsgName = "S16F131";
            this.strSecondaryMsgName = "S16F132";
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
            string dstrHGLSID = "";

            try
            {
                arrayEvent = strParameters.Split(',');
                dstrHGLSID = arrayEvent[1];
                InfoAct.clsRPC CurrentRPC = this.pInfo.RPC(dstrHGLSID);

                pMsgTran = pSecsDrv.MakeTransaction(this.strPrimaryMsgName);

                pMsgTran.Primary().Item("MODULEID").Putvalue(this.pInfo.Unit(3).SubUnit(0).ModuleID);

                pMsgTran.Primary().Item("L2").Putvalue(1);

                pMsgTran.Primary().Item("H_GLASSID").Putvalue(CurrentRPC.HGLSID);
                pMsgTran.Primary().Item("JOBID").Putvalue(CurrentRPC.JOBID);
                pMsgTran.Primary().Item("RPC_PPID").Putvalue(CurrentRPC.RPC_PPID);
                pMsgTran.Primary().Item("SET_TIME").Putvalue(CurrentRPC.SetTime.ToString("yyyyMMddHHmmssff"));

                CurrentRPC.RPC_STATE = 2;       // 20130308 lsc

                pInfo.subProcessDataStatusSet(InfoAct.clsInfo.ProcessDataType.RPC, dstrHGLSID);

                return pMsgTran;
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return null;
            } 
            #region 20130308 변경전
#if false    
            
            string[] arrayEvent;
            string dstrHGLSID = "";

            try
            {
                arrayEvent = strParameters.Split(',');
                dstrHGLSID = arrayEvent[1];
                InfoAct.clsRPC CurrentRPC = this.pInfo.RPC(dstrHGLSID);
                
                pMsgTran = pSecsDrv.MakeTransaction(this.strPrimaryMsgName);

                pMsgTran.Primary().Item("MODULEID").Putvalue(this.pInfo.Unit(0).SubUnit(0).ModuleID);

                pMsgTran.Primary().Item("L2").Putvalue(1);

                pMsgTran.Primary().Item("H_GLASSID").Putvalue(CurrentRPC.HGLSID);
                pMsgTran.Primary().Item("JOBID").Putvalue(CurrentRPC.JOBID);
                pMsgTran.Primary().Item("RPC_PPID").Putvalue(CurrentRPC.RPC_PPID);
                pMsgTran.Primary().Item("SET_TIME").Putvalue(CurrentRPC.SetTime.ToString("yyyyMMddHHmmssff"));
                
                pInfo.subProcessDataStatusSet(InfoAct.clsInfo.ProcessDataType.RPC, dstrHGLSID);
                
                return pMsgTran;
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return null;
            }
#endif
            #endregion
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
