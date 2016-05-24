using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS3F1 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS3F1 Instance = new clsS3F1();
        #endregion

        #region Constructors
        public clsS3F1()
        {
            this.IntStream = 3;
            this.IntFunction = 1;
            this.StrPrimaryMsgName = "S3F1";
            this.StrSecondaryMsgName = "S3F2";
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

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").ToString().Trim();

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("COUNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }

                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                msgTran.Secondary().Item("COUNT").Putvalue(pInfo.Unit(0).SubUnit(0).MaterialCount);

                int dintMaterialCount = pInfo.Unit(0).SubUnit(0).MaterialCount;

                for (int dintLoop = 1; dintLoop <= dintMaterialCount; dintLoop++)
                {
                    msgTran.Secondary().Item("M_ID" + (dintLoop - 1)).Putvalue(pInfo.Unit(0).SubUnit(0).Material(dintLoop).Name);
                    msgTran.Secondary().Item("PROD_TYPE" + (dintLoop - 1)).Putvalue("Filter");
                    msgTran.Secondary().Item("LIBRARYID" + (dintLoop - 1)).Putvalue("");
                    msgTran.Secondary().Item("STAGE_STATE" + (dintLoop - 1)).Putvalue("");
                    msgTran.Secondary().Item("STATE" + (dintLoop - 1)).Putvalue("");
                    msgTran.Secondary().Item("LOCATION" + (dintLoop - 1)).Putvalue("");
                    msgTran.Secondary().Item("SIZE" + (dintLoop - 1)).Putvalue("");
                    msgTran.Secondary().Item("PRODCNT" + (dintLoop - 1) + 0).Putvalue("");
                    msgTran.Secondary().Item("PROD_ID" + (dintLoop - 1) + 0).Putvalue("");
                    msgTran.Secondary().Item("STEPID" + (dintLoop - 1) + 0).Putvalue("");
                    msgTran.Secondary().Item("PPID" + (dintLoop - 1) + 0).Putvalue("");
                }

                funSendReply(msgTran);
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
