using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;

namespace HostAct
{
    class clsS16F105 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS16F105 Instance = new clsS16F105();
        #endregion

        #region Constructors
        public clsS16F105()
        {
            this.intStream = 16;
            this.intFunction = 105;
            this.strPrimaryMsgName = "S16F105";
            this.strSecondaryMsgName = "S16F106";
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
            int dintAPCCount = 0;
            string strAPCData = "";

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                if (dstrModuleID != this.pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    dintTCACK = 1;
                }

               
                //dintAPCCount = Convert.ToInt32(this.PSecsDrv.S16F105APCDataDeletCMD.APCDelCount.ToString());
                dintAPCCount = Convert.ToInt32(msgTran.Primary().Item("L2").Getvalue().ToString().Trim());

                if (dintAPCCount != 0)
                {
                    for (int dintLoop = 0; dintLoop < dintAPCCount; dintLoop++)
                    {
                        //dstrHGLSID = this.PSecsDrv.S16F105APCDataDeletCMD.get_H_GLASSID(dintLoop).ToString().Trim();
                        dstrHGLSID = msgTran.Primary().Item("H_GLASSID" + dintLoop).Getvalue().ToString().Trim();
                        if (this.pInfo.APC(dstrHGLSID) == null)
                        {
                            dintTCACK = 2; //2 : Denied, tried to delete non-existing APC data
                            break;
                        }
                    }
                }
                else
                {
                    foreach (string strGLSID in this.pInfo.APC())
                    {
                        if (this.pInfo.APC(strGLSID).State == "2")
                        {
                            dintTCACK = 3;
                            break;
                        }
                    }
                }


                msgTran.Secondary().Item("TCACK").Putvalue(dintTCACK);
                funSendReply(msgTran);

                if (this.pInfo.APCCount != 0 && dintTCACK == 0)
                {
                    if (dintAPCCount == 0)
                    {
                        this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ProcessDataDel, InfoAct.clsInfo.ProcessDataType.APC, "2", "", true);
                    }
                    else
                    {
                        for (int dintLoop = 0; dintLoop < dintAPCCount; dintLoop++)
                        {
                            dstrHGLSID = msgTran.Primary().Item("H_GLASSID" + dintLoop).Getvalue().ToString().Trim();
                            strAPCData += "2!" + dstrHGLSID + "=";
                        }

                        if (string.IsNullOrEmpty(strAPCData) == false)
                        {
                            strAPCData = strAPCData.Substring(0, strAPCData.Length - 1);
                        }

                        this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ProcessDataDel, InfoAct.clsInfo.ProcessDataType.APC, "2", strAPCData, true);

                    }
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
