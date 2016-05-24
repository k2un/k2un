using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;

namespace HostAct
{
    public class clsS16F3 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS16F3 Instance = new clsS16F3();
        #endregion

        #region Constructors
        public clsS16F3()
        {
            this.intStream = 16;
            this.intFunction = 3;
            this.strPrimaryMsgName = "S16F3";
            this.strSecondaryMsgName = "S16F4";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrModuleID = "";
            string dstrHGLSID = "";
            int dintGLSCount = 0;
            int dintParamCount = 0;
            int dintACKC16 = 0;
            bool dbolModuleIDCheck = false;
            
            string[] darrP_ModuleID;
            string[] darrP_ORDER;
            string dstrJOBID = "";
            int dintMode = 1;


            try
            {
                //if (this.pInfo.All.HostConnect == false) return;
                //if (funACTSECSAort_Send(this.pSecsDrv.S16F103APCDataSet.Header) == true) return;

                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dstrHGLSID = msgTran.Primary().Item("H_GLASSID").Getvalue().ToString().Trim();
                dintGLSCount = Convert.ToInt32(msgTran.Primary().Item("L2").Getvalue().ToString().Trim());

                //EOID Check
                //if (this.pInfo.Unit(0).SubUnit(0).EOID(3) == null || this.pInfo.Unit(0).SubUnit(0).EOID(3).EOV == 0) dintACKC16 = 1;
                if (this.pInfo.All.PPCUSE == false)
                {
                    dintACKC16 = 1;
                }
                else
                {
                    #region "DMS 장비 사용 안함"
                    //ModuleID Check
                    if (dstrModuleID.Trim() == this.pInfo.Unit(0).SubUnit(0).ModuleID)
                    {
                        //dintGLSCount = Convert.ToInt32(msgTran.Primary().Item("L2").Getvalue().ToString().Trim());
                        for (int dintLoop = 0; dintLoop < dintGLSCount; dintLoop++)
                        {
                            dstrHGLSID = msgTran.Primary().Item("H_GLASSID" + dintLoop).Getvalue().ToString();

                            if (this.pInfo.PPC(dstrHGLSID) != null)
                            {
                                if (this.pInfo.PPC(dstrHGLSID).RunState != 1)
                                {
                                    dintACKC16 = 2;
                                    break;
                                }
                                dintMode = 4;
                            }

                            if (dintACKC16 == 0)
                            {
                                dintParamCount = Convert.ToInt32(msgTran.Primary().Item("L4" + dintLoop).Getvalue().ToString().Trim());
                                darrP_ModuleID = new string[dintParamCount];
                                for (int dintLoop2 = 0; dintLoop2 < dintParamCount; dintLoop2++)
                                {
                                    darrP_ModuleID[dintLoop2] = msgTran.Primary().Item("P_MODULEID" + dintLoop + dintLoop2).Getvalue().ToString().Trim();

                                    if (this.pInfo.ModuleID(darrP_ModuleID[dintLoop2]) == null)
                                    {
                                        dintACKC16 = 5;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        dintACKC16 = 4;
                    }
                    #endregion
                }

                if (dintACKC16 == 0)
                {
                    funPPCDataSave(msgTran, dintMode);
                }

                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                msgTran.Secondary().Item("ACKC16").Putvalue(dintACKC16);

                msgTran.Secondary().Item("L2").Putvalue(dintGLSCount);
                for (int dintLoop = 0; dintLoop < dintGLSCount; dintLoop++)
                {
                    dstrHGLSID = msgTran.Primary().Item("H_GLASSID" + dintLoop).Getvalue().ToString();
                    msgTran.Secondary().Item("H_GLASSID" + dintLoop).Putvalue(dstrHGLSID);

                    dstrJOBID = msgTran.Primary().Item("JOBID" + dintLoop).Getvalue().ToString();
                    msgTran.Secondary().Item("JOBID" + dintLoop).Putvalue(dstrJOBID);

                    dintParamCount = Convert.ToInt32(msgTran.Primary().Item("L4" + dintLoop).Getvalue().ToString().Trim());
                    msgTran.Secondary().Item("L4" + dintLoop).Putvalue(dintParamCount);

                    dintParamCount = Convert.ToInt32(msgTran.Primary().Item("L4" + dintLoop).Getvalue().ToString().Trim());
                    darrP_ModuleID = new string[dintParamCount];
                    darrP_ORDER = new string[dintParamCount];
                    for (int dintLoop2 = 0; dintLoop2 < dintParamCount; dintLoop2++)
                    {
                        darrP_ModuleID[dintLoop2] = msgTran.Primary().Item("P_MODULEID" + dintLoop + dintLoop2).Getvalue().ToString().Trim();
                        darrP_ORDER[dintLoop2] = msgTran.Primary().Item("P_ORDER" + dintLoop + dintLoop2).Getvalue().ToString().Trim();

                        msgTran.Secondary().Item("P_MODULEID" + dintLoop + dintLoop2).Putvalue(darrP_ModuleID[dintLoop2]);
                        msgTran.Secondary().Item("P_ORDER" + dintLoop + dintLoop2).Putvalue(darrP_ORDER[dintLoop2]);
                    }
                }

                funSendReply2(msgTran, "S16F4");
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return;
            }  
        }

        private void funPPCDataSave(Transaction msgTran, int intMode)
        {
            int dintGLSCount=0;
            string dstrHGLSID = "";
            int dintParamCount = 0;
            string[] darrP_ModuleID;
            string[] darrP_ORDER;
            string[] darrP_Status;
            DateTime dtTime;

            try
            {
                dtTime = DateTime.Now;
                dintGLSCount = Convert.ToInt32(msgTran.Primary().Item("L2").Getvalue().ToString().Trim());


                for (int dintLoop = 0; dintLoop < dintGLSCount; dintLoop++)
                {
                    dstrHGLSID = msgTran.Primary().Item("H_GLASSID" + dintLoop).Getvalue().ToString();
                    if (this.pInfo.PPC(dstrHGLSID) == null)
                    {
                        this.pInfo.AddPPC(dstrHGLSID);
                    }
                    else
                    {
                        this.pInfo.RemovePPC(dstrHGLSID);
                        this.pInfo.AddPPC(dstrHGLSID);
                    }
                    InfoAct.clsPPC CurrentPPC = this.pInfo.PPC(dstrHGLSID);

                    CurrentPPC.JOBID = msgTran.Primary().Item("JOBID" + dintLoop).Getvalue().ToString();
                    CurrentPPC.SetTime = dtTime;
                    CurrentPPC.RunState = 1;

                    dintParamCount = Convert.ToInt32(msgTran.Primary().Item("L4" + dintLoop).Getvalue().ToString().Trim());

                    darrP_ModuleID = new string[dintParamCount];
                    darrP_ORDER = new string[dintParamCount];
                    darrP_Status = new string[dintParamCount];

                    for (int dintLoop2 = 0; dintLoop2 < dintParamCount; dintLoop2++)
                    {
                        darrP_ModuleID[dintLoop2] = msgTran.Primary().Item("P_MODULEID" + dintLoop+dintLoop2).Getvalue().ToString().Trim();
                        darrP_ORDER[dintLoop2] = msgTran.Primary().Item("P_ORDER" + dintLoop+dintLoop2).Getvalue().ToString().Trim();
                        darrP_Status[dintLoop2] ="1";

                    }

                    CurrentPPC.P_MODULEID = darrP_ModuleID;
                    CurrentPPC.P_ORDER = darrP_ORDER;
                    CurrentPPC.P_STATUS = darrP_Status;

                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ProcessDataSet, InfoAct.clsInfo.ProcessDataType.PPC, intMode, CurrentPPC);
                }
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
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
