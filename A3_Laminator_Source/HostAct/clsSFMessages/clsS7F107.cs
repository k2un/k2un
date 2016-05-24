using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS7F107 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS7F107 Instance = new clsS7F107();
        #endregion

        #region Constructors
        public clsS7F107()
        {
            this.IntStream = 7;
            this.IntFunction = 107;
            this.StrPrimaryMsgName = "S7F107";
            this.StrSecondaryMsgName = "S7F108";
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
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
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

                string dstrMode = arrayEvent[1];    //Mode(1: 생성, 2: 삭제, 3: 수정)
                string dstrPPIDType = arrayEvent[2];
                string dstrHOSTPPID = arrayEvent[3];
                string dstrEQPPPID = arrayEvent[4];

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("MODE").Putvalue(dstrMode); //Mode(1: 생성, 2: 삭제, 3: 수정)
                pMsgTran.Primary().Item("MODULEID1").Putvalue(pInfo.Unit(3).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("PPID_TYPE").Putvalue(dstrPPIDType);
                
                switch (dstrPPIDType)
                {
                    case "1":
                        {
                            //string UP_EQPPPID = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(dstrEQPPPID).UP_EQPPPID;
                            //string LOW_EQPPPID = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(dstrEQPPPID).LOW_EQPPPID;
                            
                            //pHost.subWaitPPIDSearch();

                            pMsgTran.Primary().Item("PPID").Putvalue(dstrEQPPPID);
                            pMsgTran.Primary().Item("COMMANDCOUNT").Putvalue(1);
                            pMsgTran.Primary().Item("CCODE" + 0).Putvalue(0);

                            int dintPPIDBodyCount = 0;
                            for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                            {
                                InfoAct.clsPPIDBody currentPPIDBody = pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop);
                                if (currentPPIDBody.UseMode)
                                {
                                    dintPPIDBodyCount++;
                                }
                            }

                            pMsgTran.Primary().Item("PPARMCOUNT" + 0).Putvalue(dintPPIDBodyCount);

                            int dintCount = 0;
                            for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                            {
                                InfoAct.clsPPIDBody currentPPIDBody = pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop);
                                if (currentPPIDBody.UseMode)
                                {
                                    pMsgTran.Primary().Item("P_PARM_NAME" + 0 + dintCount).Putvalue(currentPPIDBody.Name);
                                    pMsgTran.Primary().Item("P_PARM" + 0 + dintCount).Putvalue(currentPPIDBody.Value);
                                    dintCount++;
                                }
                            }
                        }
                        break;
                    case "2":
                        {
                            pMsgTran.Primary().Item("PPID").Putvalue(dstrHOSTPPID);
                            pMsgTran.Primary().Item("COMMANDCOUNT").Putvalue(1);
                            pMsgTran.Primary().Item("CCODE" + 0).Putvalue(0);
                            pMsgTran.Primary().Item("PPARMCOUNT" + 0).Putvalue(1);
                            pMsgTran.Primary().Item("P_PARM_NAME" + 0 + 0).Putvalue("SUBPPID");
                            pMsgTran.Primary().Item("P_PARM" + 0 + 0).Putvalue(dstrEQPPPID);
                        }
                        break;
                }

                return pMsgTran;
            }
            catch (Exception error)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
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
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }
        #endregion
    }
}
