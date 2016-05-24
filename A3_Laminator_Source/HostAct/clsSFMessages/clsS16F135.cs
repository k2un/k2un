using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;
using System.Collections;

namespace HostAct
{
    public class clsS16F135 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS16F135 Instance = new clsS16F135();
        #endregion

        #region Constructors
        public clsS16F135()
        {
            this.intStream = 16;
            this.intFunction = 135;
            this.strPrimaryMsgName = "S16F135";
            this.strSecondaryMsgName = "S16F136";
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
            int dintIndex = 0;
            try
            {
                arrayEvent = strParameters.Split(',');

                pMsgTran = pSecsDrv.MakeTransaction(this.strPrimaryMsgName);

                string[] arrParamName;
                string[] arrParamValue;

                string dstrMode = arrayEvent[1].ToString();        //MDOE(Creation :1, Deletion : 2, Expiration : 3, Override : 4)
                dstrHGLSID = arrayEvent[2];

                pMsgTran.Primary().Item("MODULEID").Putvalue(this.pInfo.Unit(3).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("MODE").Putvalue(dstrMode);

                string[] arrData = dstrHGLSID.Split('!');
                int dintBYWHO = 0;
                switch (dstrMode)
                {
                    case "1":
                         if (pInfo.All.RPCDataSet == true)
                        {
                            dintBYWHO = 2;
                            pInfo.All.RPCDataSet = false;
                        }
                        else
                        {
                            dintBYWHO = 1;
                        }

                        pMsgTran.Primary().Item("L2").Putvalue(arrData.Length);
                        for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                        {
                            InfoAct.clsRPC CurrentRPC = pInfo.RPC(arrData[dintLoop]);
                            pMsgTran.Primary().Item("H_GLASSID" + dintLoop).Putvalue(CurrentRPC.HGLSID);
                            pMsgTran.Primary().Item("JOBID" + dintLoop).Putvalue(CurrentRPC.JOBID);
                            pMsgTran.Primary().Item("RPC_PPID" + dintLoop).Putvalue(CurrentRPC.RPC_PPID);
                            pMsgTran.Primary().Item("SET_TIME" + dintLoop).Putvalue(CurrentRPC.SetTime.ToString("yyyyMMddHHmmssff"));
                            pMsgTran.Primary().Item("BYWHO" + dintLoop).Putvalue(dintBYWHO);
                        }

                        break;
                    case "4":
                        if (pInfo.All.RPCDataSet == true)
                        {
                            dintBYWHO = 2;
                            pInfo.All.RPCDataSet = false;
                        }
                        else
                        {
                            dintBYWHO = 1;
                        }

                        pMsgTran.Primary().Item("L2").Putvalue(arrData.Length);
                        for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                        {
                            InfoAct.clsRPC CurrentRPC = pInfo.RPC(arrData[dintLoop]);
                            pMsgTran.Primary().Item("H_GLASSID" + dintLoop).Putvalue(CurrentRPC.HGLSID);
                            pMsgTran.Primary().Item("JOBID" + dintLoop).Putvalue(CurrentRPC.JOBID_Old);
                            pMsgTran.Primary().Item("RPC_PPID" + dintLoop).Putvalue(CurrentRPC.RPC_PPID_Old);
                            pMsgTran.Primary().Item("SET_TIME" + dintLoop).Putvalue(CurrentRPC.SetTime_Old.ToString("yyyyMMddHHmmssff"));
                            pMsgTran.Primary().Item("BYWHO" + dintLoop).Putvalue(dintBYWHO);
                        }

                        break;

                    case "2":
                        if (pInfo.All.RPCDataDel == true)
                        {
                            dintBYWHO = 2;
                            pInfo.All.RPCDataDel = false;
                        }
                        else
                        {
                            dintBYWHO = 1;
                        }

                        if (string.IsNullOrEmpty(arrData[0]) == false)
                        {
                            pMsgTran.Primary().Item("L2").Putvalue(arrData.Length);
                            for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                            {
                                InfoAct.clsRPC CurrentRPC = pInfo.RPC(arrData[dintLoop]);
                                pMsgTran.Primary().Item("H_GLASSID" + dintLoop).Putvalue(CurrentRPC.HGLSID);
                                pMsgTran.Primary().Item("JOBID" + dintLoop).Putvalue(CurrentRPC.JOBID);
                                pMsgTran.Primary().Item("RPC_PPID" + dintLoop).Putvalue(CurrentRPC.RPC_PPID);
                                pMsgTran.Primary().Item("SET_TIME" + dintLoop).Putvalue(CurrentRPC.SetTime.ToString("yyyyMMddHHmmssff"));
                                pMsgTran.Primary().Item("BYWHO" + dintLoop).Putvalue(dintBYWHO);
                                pInfo.RemoveRPC(arrData[dintLoop]);
                            }
                        }
                        else
                        {
                            pMsgTran.Primary().Item("L2").Putvalue(pInfo.RPCCount);
                            int dintCount = 0;
                            //ArrayList arrCon = new ArrayList();
                            foreach(string strRPC in pInfo.RPC())
                            {
                                InfoAct.clsRPC CurrentRPC = pInfo.RPC(strRPC);
                                pMsgTran.Primary().Item("H_GLASSID" + dintCount).Putvalue(CurrentRPC.HGLSID);
                                pMsgTran.Primary().Item("JOBID" + dintCount).Putvalue(CurrentRPC.JOBID);
                                pMsgTran.Primary().Item("RPC_PPID" + dintCount).Putvalue(CurrentRPC.RPC_PPID);
                                pMsgTran.Primary().Item("SET_TIME" + dintCount).Putvalue(CurrentRPC.SetTime.ToString("yyyyMMddHHmmssff"));
                                pMsgTran.Primary().Item("BYWHO" + dintCount).Putvalue(dintBYWHO);
                                dintCount++;
                                //arrCon.Add(strRPC);
                                //pInfo.RemoveRPC(strRPC);
                            }
                            pInfo.RemoveRPC();
                        }
                        break;

                    case "3":
                        dintBYWHO = 3;
                          pMsgTran.Primary().Item("L2").Putvalue(arrData.Length);
                          ArrayList arrCon = new ArrayList();
                        for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                        {
                            InfoAct.clsRPC CurrentRPC = pInfo.RPC(arrData[dintLoop]);
                            pMsgTran.Primary().Item("H_GLASSID" + dintLoop).Putvalue(CurrentRPC.HGLSID);
                            pMsgTran.Primary().Item("JOBID" + dintLoop).Putvalue(CurrentRPC.JOBID);
                            pMsgTran.Primary().Item("RPC_PPID" + dintLoop).Putvalue(CurrentRPC.RPC_PPID);
                            pMsgTran.Primary().Item("SET_TIME" + dintLoop).Putvalue(CurrentRPC.SetTime.ToString("yyyyMMddHHmmssff"));
                            pMsgTran.Primary().Item("BYWHO" + dintLoop).Putvalue(dintBYWHO);
                            arrCon.Add(CurrentRPC.HGLSID);
                        }

                        for (int dintLoop = 0; dintLoop < arrCon.Count; dintLoop++)
                        {
                            pInfo.RemoveRPC(arrCon[dintLoop].ToString());
                        }
                        break;
                }

                this.pInfo.All.RPCDBUpdateCheck = true;
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
