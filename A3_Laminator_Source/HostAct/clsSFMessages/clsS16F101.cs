using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;

namespace HostAct
{
    class clsS16F101: clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion
        
        #region Singleton
        public static readonly clsS16F101 Instance = new clsS16F101();
        #endregion

        #region Constructors
        public clsS16F101()
        {
			this.intStream = 16;
            this.intFunction = 101;
            this.strPrimaryMsgName = "S16F101";
            this.strSecondaryMsgName = "S16F102";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
  
            string dstrHGLSID = "";
            string dstrModuleID = "";
            int dintTCACK = 0;
            int dintIndex = 0;
            string[] arrParamName;
            string[] arrParamValue;
            int dintAPCCount = 0;

			try
			{
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                if (dstrModuleID != this.pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    dintTCACK = 1;
                }
                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                dintAPCCount = Convert.ToInt32(msgTran.Primary().Item("L2").Getvalue().ToString().Trim());

                if (dintAPCCount != 0)
                {
                    msgTran.Secondary().Item("L2").Putvalue(dintAPCCount);

                    for (int dintCount = 0; dintCount < dintAPCCount; dintCount++)
                    {

                        dstrHGLSID = msgTran.Primary().Item("H_GLASSID").Getvalue().ToString().Trim();

                        if (this.pInfo.APC(dstrHGLSID) == null)
                        {
                            dintTCACK = 2;
                            msgTran.Secondary().Item("L2").Putvalue(0);
                            msgTran.Secondary().Item("L2").Delete("L3");
                            msgTran.Secondary().Item("TCACK").Putvalue(dintTCACK);
                            funSendReply2(msgTran, "S16F102");
                            return;
                        }
                        else
                        {
                            InfoAct.clsAPC CurrentAPC = this.pInfo.APC(dstrHGLSID);

                            msgTran.Secondary().Item("H_GLASSID" + dintCount).Putvalue(CurrentAPC.GLSID);

                            msgTran.Secondary().Item("JOBID" + dintCount).Putvalue(CurrentAPC.JOBID);
                            msgTran.Secondary().Item("RECIPE" + dintCount).Putvalue(CurrentAPC.EQPPPID);
                            msgTran.Secondary().Item("SET_TIME" + dintCount).Putvalue(CurrentAPC.SetTime.ToString("yyyyMMddHHmmssff"));
                            msgTran.Secondary().Item("APC_STATE" + dintCount).Putvalue(CurrentAPC.State);

                            msgTran.Secondary().Item("L4" + dintCount).Putvalue(CurrentAPC.ParameterName.Length);

                            arrParamName = CurrentAPC.ParameterName;
                            arrParamValue = CurrentAPC.ParameterValue;

                            for (int dintLoop = 0; dintLoop < CurrentAPC.ParameterName.Length; dintLoop++)
                            {
                                msgTran.Secondary().Item("P_PARM_NAME" + dintCount + dintLoop).Putvalue(arrParamName[dintLoop]);
                                msgTran.Secondary().Item("P_PARM_VALUE" + dintCount + dintLoop).Putvalue(arrParamValue[dintLoop]);
                            }
                        }
                    }
                }
                else
                {
                    msgTran.Secondary().Item("L2").Putvalue(this.pInfo.APC().Count);

                    if (this.pInfo.APCCount == 0)
                    {
                        //dintTCACK = 2;
                        msgTran.Secondary().Item("L2").Putvalue(0);
                        msgTran.Secondary().Item("TCACK").Putvalue(dintTCACK);
                        funSendReply2(msgTran, "S16F102");
                        return;
                    }

                    dintIndex = 0;

                    foreach (string dstrAPCHGLSID in this.pInfo.APC())
                    {
                        InfoAct.clsAPC CurrentAPC = this.pInfo.APC(dstrAPCHGLSID);

                        msgTran.Secondary().Item("H_GLASSID" + dintIndex).Putvalue(CurrentAPC.GLSID);

                        msgTran.Secondary().Item("JOBID" + dintIndex).Putvalue(CurrentAPC.JOBID);
                        msgTran.Secondary().Item("RECIPE" + dintIndex).Putvalue(CurrentAPC.EQPPPID);
                        msgTran.Secondary().Item("SET_TIME" + dintIndex).Putvalue(CurrentAPC.SetTime.ToString("yyyyMMddHHmmssff"));
                        msgTran.Secondary().Item("APC_STATE" + dintIndex).Putvalue(CurrentAPC.State);

                        msgTran.Secondary().Item("L4" + dintIndex).Putvalue(CurrentAPC.ParameterName.Length);

                        arrParamName = CurrentAPC.ParameterName;
                        arrParamValue = CurrentAPC.ParameterValue;

                        for (int dintLoop = 0; dintLoop < CurrentAPC.ParameterName.Length; dintLoop++)
                        {
                            msgTran.Secondary().Item("P_PARM_NAME" + dintIndex + dintLoop).Putvalue(arrParamName[dintLoop]);
                            msgTran.Secondary().Item("P_PARM_VALUE" + dintIndex + dintLoop).Putvalue(arrParamValue[dintLoop]);
                        }
                        dintIndex++;
                    }
                }

                msgTran.Secondary().Item("TCACK").Putvalue(dintTCACK);
                funSendReply2(msgTran, "S16F102");
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
				
				pMsgTran = pSecsDrv.MakeTransaction(this.strPrimaryMsgName);

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
