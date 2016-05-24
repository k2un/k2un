using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;

namespace HostAct
{
    public class clsS16F121 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS16F121 Instance = new clsS16F121();
        #endregion

        #region Constructors
        public clsS16F121()
        {
            this.intStream = 16;
            this.intFunction = 121;
            this.strPrimaryMsgName = "S16F121";
            this.strSecondaryMsgName = "S16F122";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            int dintACKC16 = 0;
            string dstrModuleID = "";
            int dintHGLSCount = 0;
            int dintIndex = 0;
            string dstrHGLSID = "";
            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                if (dstrModuleID == this.pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    dintHGLSCount = Convert.ToInt32(msgTran.Primary().Item("L2").Getvalue().ToString().Trim());
                    if (dintHGLSCount == 0)
                    {
                        msgTran.Secondary().Item("L2").Putvalue(this.pInfo.RPCCount);
                        foreach (string strGLSID in this.pInfo.RPC())
                        {
                            InfoAct.clsRPC CurrentRPC = this.pInfo.RPC(strGLSID);

                            msgTran.Secondary().Item("H_GLASSID" + dintIndex).Putvalue(CurrentRPC.HGLSID);
                            msgTran.Secondary().Item("JOBID" + dintIndex).Putvalue(CurrentRPC.JOBID);
                            msgTran.Secondary().Item("SET_TIME" + dintIndex).Putvalue(CurrentRPC.SetTime.ToString("yyyyMMddHHmmssff"));
                            msgTran.Secondary().Item("RPC_PPID" + dintIndex).Putvalue(CurrentRPC.RPC_PPID);
                            msgTran.Secondary().Item("RPC_STATE" + dintIndex).Putvalue(CurrentRPC.RPC_STATE);
                            
                            dintIndex++;
                        }
                    }
                    else
                    {
                        for (int dintLoop = 0; dintLoop < dintHGLSCount; dintLoop++)
                        {
                            dstrHGLSID = msgTran.Primary().Item("H_GLASSID" + dintLoop).Getvalue().ToString().Trim();

                            if (this.pInfo.RPC(dstrHGLSID) == null)
                            {
                                dintACKC16 = 2;
                                break;
                            }
                        }

                        if (dintACKC16 == 0)
                        {
                            msgTran.Secondary().Item("L2").Putvalue(dintHGLSCount);
                            for (int dintLoop = 0; dintLoop < dintHGLSCount; dintLoop++)
                            {
                                dstrHGLSID = msgTran.Primary().Item("H_GLASSID" + dintLoop).Getvalue().ToString().Trim();
                                InfoAct.clsRPC CurrentRPC = this.pInfo.RPC(dstrHGLSID);

                                msgTran.Secondary().Item("H_GLASSID" + dintIndex).Putvalue(CurrentRPC.HGLSID);
                                msgTran.Secondary().Item("JOBID" + dintIndex).Putvalue(CurrentRPC.JOBID);
                                msgTran.Secondary().Item("SET_TIME" + dintIndex).Putvalue(CurrentRPC.SetTime.ToString("yyyyMMddHHmmssff"));
                                msgTran.Secondary().Item("RPC_PPID" + dintIndex).Putvalue(CurrentRPC.RPC_PPID);
                                msgTran.Secondary().Item("RPC_STATE" + dintIndex).Putvalue(CurrentRPC.RPC_STATE);

                            }
                        }
                    }
                }
                else
                {
                    dintACKC16 = 1;
                }

                if (dintACKC16 != 0)
                {
                    msgTran.Secondary().Item("L2").Putvalue(1);
                    msgTran.Secondary().Delete("L4");
                }

                msgTran.Secondary().Item("ACKC16").Putvalue(dintACKC16);
                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);

                funSendReply(msgTran);
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
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
