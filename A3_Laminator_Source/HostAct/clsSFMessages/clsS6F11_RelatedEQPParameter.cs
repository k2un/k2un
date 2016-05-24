using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_RelatedEQPParameter : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_RelatedEQPParameter Instance = new clsS6F11_RelatedEQPParameter();
        #endregion

        #region Constructors
        public clsS6F11_RelatedEQPParameter()
        {
            this.IntStream = 6;
            this.IntFunction = 1;
            this.StrPrimaryMsgName = "S6F11RelatedEQPParameter";
            this.StrSecondaryMsgName = "S6F12";
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
            try
            {
                arrayEvent = strParameters.Split(',');

                int dintBYWHO = 0;
                int dintIndex = 0;
                int dintEOIDSingleCount = 0;
                int dintSendIndex = 0;
                int dintECID = 0;
                int dintCount = 0;
                string[] dstrData;
                Queue dqEOIDIndex = new Queue();

                int dintCEID = Convert.ToInt32(arrayEvent[1]);      //CEID
                string dstrValue = arrayEvent[2];                   //Value(102는 ECID값(KEY값), EOID의 경우 Index이다.)

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);      //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("RPTID").Putvalue(1);       //Fixed Value

                pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(3).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(pInfo.Unit(3).SubUnit(0).EQPState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(pInfo.Unit(3).SubUnit(0).EQPProcessState);

                //각 CEID별로 BYWHO값을 설정한다.
                switch (dintCEID)
                {
                    case 101:
                        {
                            dintBYWHO = Convert.ToInt32(pInfo.All.EOIDChangeBYWHO);

                            pMsgTran.Primary().Item("ECCOUNT").Putvalue(0);

                            dstrValue = dstrValue.Remove(dstrValue.LastIndexOf(';'));
                            dstrData = dstrValue.Split(';');                        //보고할 EOID Index 값

                            //EOID Index가 4~9, 그 이외의 값 개수 체크
                            for (int dintLoop = 0; dintLoop <= dstrData.Length - 1; dintLoop++)
                            {
                                dintIndex = Convert.ToInt32(dstrData[dintLoop]);

                                dintEOIDSingleCount = dintEOIDSingleCount + 1;      //EOID Index가 4~9 이외의 값 개수

                                if (dqEOIDIndex.Contains(pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID) == false)
                                {
                                    dqEOIDIndex.Enqueue(pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID);
                                }
                            }

                            pMsgTran.Primary().Item("EOCOUNT").Putvalue(dqEOIDIndex.Count);

                            for (int dintLoop = 1; dintLoop <= dstrData.Length; dintLoop++)
                            {
                                dintIndex = Convert.ToInt32(dstrData[dintLoop - 1]);
                                int dintEOID = pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID;
                                int dintEOMD = pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMD;
                                int dintEOV = pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV;

                                pMsgTran.Primary().Item("EOMDCOUNT" + (dintLoop - 1)).Putvalue(1);
                                pMsgTran.Primary().Item("EOID" + (dintLoop - 1)).Putvalue(dintEOID);
                                pMsgTran.Primary().Item("EOMD" + (dintLoop - 1) + 0).Putvalue(dintEOMD);
                                pMsgTran.Primary().Item("EOV" + (dintLoop - 1) + 0).Putvalue(dintEOV);
                            }
                        }
                        break;

                    case 102:
                        {
                            dintBYWHO = Convert.ToInt32(pInfo.All.ECIDChangeBYWHO);

                            pMsgTran.Primary().Item("EOCOUNT").Putvalue(0);

                            dintCount = dstrValue.Split(';').Length - 1;    //보고할 List 개수
                            dstrData = dstrValue.Split(';');

                            pMsgTran.Primary().Item("ECCOUNT").Putvalue(dintCount);

                            for (int dintLoop = 1; dintLoop <= dintCount; dintLoop++)
                            {
                                dintECID = Convert.ToInt32(dstrData[dintLoop - 1]);
                                pMsgTran.Primary().Item("ECID" + (dintLoop - 1)).Putvalue(dintECID);

                                InfoAct.clsECID currentECID = pInfo.Unit(0).SubUnit(0).ECID(dintECID);
                                pMsgTran.Primary().Item("ECNAME" + (dintLoop - 1)).Putvalue(currentECID.Name);
                                pMsgTran.Primary().Item("ECDEF" + (dintLoop - 1)).Putvalue(currentECID.ECDEF);
                                pMsgTran.Primary().Item("ECSLL" + (dintLoop - 1)).Putvalue(currentECID.ECSLL);
                                pMsgTran.Primary().Item("ECSUL" + (dintLoop - 1)).Putvalue(currentECID.ECSUL);
                                pMsgTran.Primary().Item("ECWLL" + (dintLoop - 1)).Putvalue(currentECID.ECWLL);
                                pMsgTran.Primary().Item("ECWUL" + (dintLoop - 1)).Putvalue(currentECID.ECWUL);
                            }
                        }
                        break;
                    default:
                        break;
                }

                pMsgTran.Primary().Item("BYWHO").Putvalue(dintBYWHO);
                pMsgTran.Primary().Item("OPERID").Putvalue(pInfo.All.UserID);
                pMsgTran.Primary().Item("RPTID1").Putvalue(5);  //Fixed Value
                pMsgTran.Primary().Item("RPTID2").Putvalue(6);  //Fixed Value

                pInfo.All.ECIDChangeBYWHO = "";    //초기화

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
