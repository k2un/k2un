using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;
using InfoAct;

namespace HostAct
{
    public class clsS16F123 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS16F123 Instance = new clsS16F123();
        #endregion

        #region Constructors
        public clsS16F123()
        {
            this.intStream = 16;
            this.intFunction = 123;
            this.strPrimaryMsgName = "S16F123";
            this.strSecondaryMsgName = "S16F124";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrModuleID = string.Empty;
            int dintGLSCNT = 0;
            int dintACKC16 = 0;
         
            
            InfoAct.clsRPC[] dclsReceivedRPC = null;
            InfoAct.clsRPC[] dclsBackupedRPC = null;


            bool dbolPPIDsynchronized = false;

            DateTime dtSetTime = DateTime.Now;

            int dintCreateCNT = 0;
            int dintOverrideCNT = 0;

            string strRPCData = "";

            try
            {
                #region "S16F123 수신 및 Reply"
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);

                if (pInfo.All.RPCUSE == false) dintACKC16 = 1;
                else if (!dstrModuleID.Equals(this.pInfo.Unit(3).SubUnit(0).ModuleID)) dintACKC16 = 3;
                //else
                //{
                    dintGLSCNT = Convert.ToInt32(dstrModuleID = msgTran.Primary().Item("GLSCNT").Getvalue().ToString().Trim());

                    msgTran.Secondary().Item("GLSCNT").Putvalue(dintGLSCNT);

                    dclsReceivedRPC = new InfoAct.clsRPC[dintGLSCNT];
                    dclsBackupedRPC = new InfoAct.clsRPC[dintGLSCNT];

                    for (int dintLoop = 0; dintLoop < dintGLSCNT; dintLoop++)
                    {
                        InfoAct.clsRPC tempRPC = new InfoAct.clsRPC(msgTran.Primary().Item("H_GLASSID" + dintLoop).Getvalue().ToString().Trim());
                        tempRPC.SetTime = dtSetTime.AddMilliseconds(dintLoop *10);
                        tempRPC.JOBID = msgTran.Primary().Item("JOBID" + dintLoop).Getvalue().ToString().Trim();
                        tempRPC.RPC_PPID = msgTran.Primary().Item("RPC_PPID" + dintLoop).Getvalue().ToString().Trim(); 
                        
                        msgTran.Secondary().Item("H_GLASSID" + dintLoop).Putvalue(tempRPC.HGLSID);
                        msgTran.Secondary().Item("JOBID" + dintLoop).Putvalue(tempRPC.JOBID);
                        msgTran.Secondary().Item("RPC_PPID" + dintLoop).Putvalue(tempRPC.RPC_PPID);
                        msgTran.Secondary().Item("SET_TIME" + dintLoop).Putvalue(tempRPC.SetTime.ToString("yyyyMMddHHmmssff"));

                        #region "HOST PPID Check"
                        //if (!dbolPPIDsynchronized && this.pInfo.Unit(0).SubUnit(0).HOSTPPID(tempRPC.RPC_PPID) == null)
                        //{
                        //    pInfo.All.isReceivedFromHOST = true;
                        //    //pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID, 2);
                        //    //pHost.subWaitDuringReadFromPLC();
                        //    //pInfo.All.isReceivedFromHOST = false;

                        //    dbolPPIDsynchronized = true;
                        //}

                        if (this.pInfo.Unit(0).SubUnit(0).HOSTPPID(tempRPC.RPC_PPID) == null)
                        {
                            if (dintACKC16 == 0) dintACKC16 = 4;
                        }
                        #endregion
                    
                        #region "SetMode Check"
                        if (this.pInfo.RPC(tempRPC.HGLSID) != null)
                        {
                            dintOverrideCNT++;
                            dclsBackupedRPC[dintLoop] = this.pInfo.RPC(tempRPC.HGLSID);

                            if (this.pInfo.RPC(tempRPC.HGLSID).RPC_STATE == 2)
                            {
                                if(dintACKC16 == 0) dintACKC16 = 2;
                                tempRPC.RPC_STATE =2;
                            }
                            else
                            {
                                tempRPC.Mode = 4;
                                tempRPC.RPC_STATE = 1;
                            }
                        }
                        else
                        {
                            dintCreateCNT++;
                            dclsBackupedRPC[dintLoop] = null;
                            tempRPC.RPC_STATE = tempRPC.Mode;
                        }
                        #endregion
                    
                        dclsReceivedRPC[dintLoop] = tempRPC;

                        strRPCData += tempRPC.Mode + "!" + tempRPC.HGLSID + "=";
                    }
                //}
                    if (strRPCData.Length > 0)
                    {
                        strRPCData = strRPCData.Substring(0, strRPCData.Length - 1);
                    }
                msgTran.Secondary().Item("ACKC16").Putvalue(dintACKC16);
                
                funSendReply(msgTran);

                #endregion

                #region "ProcessDataSet 처리"
                if (dintACKC16 != 0) return;

                //string[] strParams = new string[dclsReceivedRPC.Length * 2];

                for (int dintLoop = 0; dintLoop < dclsReceivedRPC.Length; dintLoop++)
                {
                    InfoAct.clsRPC CurrentRPC = dclsReceivedRPC[dintLoop];

                    //pInfo.funDBUpdate(clsInfo.ProcessDataType.RPC, CurrentRPC.HGLSID);

                    if (CurrentRPC.Mode == 4)
                    {
                        //this.pInfo.RemoveRPC(CurrentRPC.HGLSID);
                        CurrentRPC.JOBID_Old = dclsBackupedRPC[dintLoop].JOBID;
                        CurrentRPC.RPC_PPID_Old = dclsBackupedRPC[dintLoop].RPC_PPID;
                        CurrentRPC.SetTime_Old = dclsBackupedRPC[dintLoop].SetTime;

                        InfoAct.clsRPC CurrentRPC2 = pInfo.RPC(CurrentRPC.HGLSID);
                        CurrentRPC2.JOBID_Old = CurrentRPC2.JOBID;
                        CurrentRPC2.RPC_PPID_Old = CurrentRPC2.RPC_PPID;
                        CurrentRPC2.SetTime_Old = CurrentRPC2.SetTime;

                        CurrentRPC2.JOBID = CurrentRPC.JOBID;
                        CurrentRPC2.RPC_PPID = CurrentRPC.RPC_PPID;
                        CurrentRPC2.SetTime = CurrentRPC.SetTime;
                    }
                    else
                    {
                        pInfo.AddRPC(CurrentRPC);
                    }

                    //pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataLog, InfoAct.clsInfo.ProcessDataType.RPC, CurrentRPC.Mode.ToString(), CurrentRPC.HGLSID, dclsBackupedRPC[dintLoop], CurrentRPC);

                    //strParams[dintLoop * 2] = CurrentRPC.Mode.ToString();
                    //strParams[dintLoop * 2 + 1] = CurrentRPC.HGLSID;

                    //pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataSet, clsInfo.ProcessDataType.RPC, CurrentRPC.Mode.ToString(), CurrentRPC.HGLSID, dclsBackupedRPC[dintLoop], CurrentRPC);
                }

                if (string.IsNullOrEmpty(strRPCData) == false)
                {
                    pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataSet, clsInfo.ProcessDataType.RPC, "", strRPCData, "", "");
                }

                //this.pInfo.subSendSF_Set(clsInfo.SFName.S16F135RPCDataCMD, strParams);
                #endregion
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
