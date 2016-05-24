using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F101 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F101 Instance = new clsS6F101();
        #endregion

        #region Constructors
        public clsS6F101()
        {
            this.IntStream = 6;
            this.IntFunction = 101;
            this.StrPrimaryMsgName = "S6F101";
            this.StrSecondaryMsgName = "S6F102";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            int dintRPTUNIT = 0;        //A2 사양에서는 RPTUNIT로 바뀌었음. 20101015 어우수
            string dstrModuleID = "";   //추가 20101015 어우수

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dintRPTUNIT = Convert.ToInt32(msgTran.Primary().Item("RPTUNIT").Getvalue());

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("RPTUNIT").Putvalue(dintRPTUNIT);
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("YDATACNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }

                //RPTUNIT가 존재하지 않는 경우(1: GLS/Material BASE, 2: LOT BASE)
                if (dintRPTUNIT == 1 || dintRPTUNIT == 2)
                {
                    if (dintRPTUNIT == 1)
                    {
                        msgTran.Secondary().Item("VDATACNT").Putvalue(pInfo.Unit(0).SubUnit(0).GLSAPDCount);

                        int dintGLSAPDCount = pInfo.Unit(0).SubUnit(0).GLSAPDCount;
                        for (int dintLoop = 1; dintLoop <= dintGLSAPDCount; dintLoop++)
                        {
                            msgTran.Secondary().Item("DATA_ITEM" + (dintLoop - 1)).Putvalue(pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop).Name);
                        }
                    }
                    else if (dintRPTUNIT == 2)
                    {
                        msgTran.Secondary().Item("VDATACNT").Putvalue(this.pInfo.Unit(0).SubUnit(0).LOTAPDCount);

                        int dintLOTAPDCount = pInfo.Unit(0).SubUnit(0).LOTAPDCount;
                        for (int dintLoop = 1; dintLoop <= dintLOTAPDCount; dintLoop++)
                        {
                            msgTran.Secondary().Item("DATA_ITEM" + (dintLoop - 1)).Putvalue(pInfo.Unit(0).SubUnit(0).LOTAPD(dintLoop).Name);
                        }
                    }

                    //HOST로 응답
                    funSendReply(msgTran);
                }
                else
                {
                    msgTran.Secondary().Item("RPTUNIT").Putvalue(dintRPTUNIT);
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("VDATACNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }
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

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

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
