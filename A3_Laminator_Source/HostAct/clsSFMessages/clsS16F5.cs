﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;

namespace HostAct
{
    public class clsS16F5 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS16F5 Instance = new clsS16F5();
        #endregion

        #region Constructors
        public clsS16F5()
        {
            this.intStream = 16;
            this.intFunction = 5;
            this.strPrimaryMsgName = "S16F5";
            this.strSecondaryMsgName = "S16F6";
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

                if (dstrModuleID == this.pInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    dintHGLSCount = Convert.ToInt32(msgTran.Primary().Item("L2").Getvalue().ToString().Trim());
                    if (dintHGLSCount == 0)
                    {
                        foreach (string strGLSID in this.pInfo.PPC())
                        {
                            if (this.pInfo.PPC(strGLSID).RunState == 2)
                            {
                                dintACKC16 = 3;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int dintLoop = 0; dintLoop < dintHGLSCount; dintLoop++)
                        {
                            dstrHGLSID = msgTran.Primary().Item("H_GLASSID"+dintLoop).Getvalue().ToString().Trim();

                            if (this.pInfo.PPC(dstrHGLSID) == null)
                            {
                                dintACKC16 = 2;
                                break;
                            }
                            else if (this.pInfo.PPC(dstrHGLSID).RunState == 2)
                            {
                                dintACKC16 = 3;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    dintACKC16 = 1;
                }

               msgTran.Secondary().Item("ACK16").Putvalue(dintACKC16);
               
                funSendReply2(msgTran, "S16F6");

                if (dintACKC16 == 0)
                {
                    if (dintHGLSCount == 0)
                    {
                        //this.pInfo.ProcessDataDel(InfoAct.clsInfo.ProcessDataType.PPC, "2", "",true);
                        pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ProcessDataDel, InfoAct.clsInfo.ProcessDataType.PPC, "2", "", true);
                    }
                    else
                    {
                        for (int dintLoop = 0; dintLoop < dintHGLSCount; dintLoop++)
                        {
                            //삭제  
                            dstrHGLSID = msgTran.Primary().Item("H_GLASSID" + dintLoop).Getvalue().ToString().Trim();
                            //this.pInfo.ProcessDataDel(InfoAct.clsInfo.ProcessDataType.PPC, "2", dstrHGLSID,true);
                            pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ProcessDataDel, InfoAct.clsInfo.ProcessDataType.PPC, "2", dstrHGLSID, true);


                        }
                    }
                }

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