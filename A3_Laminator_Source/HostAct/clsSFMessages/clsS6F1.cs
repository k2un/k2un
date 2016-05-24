using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F1 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F1 Instance = new clsS6F1();
        #endregion

        #region Constructors
        public clsS6F1()
        {
            this.IntStream = 6;
            this.IntFunction = 1;
            this.StrPrimaryMsgName = "S6F1";
            this.StrSecondaryMsgName = "S6F2";
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

                int dintTRID = Convert.ToInt32(arrayEvent[1]);

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                InfoAct.clsSubUnit subUnit = this.pInfo.Unit(0).SubUnit(0);

                pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(3).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("TRID").Putvalue(dintTRID);
                pMsgTran.Primary().Item("SMPLN").Putvalue(subUnit.TRID(dintTRID).SampleNo);

                if (subUnit.TRID(dintTRID).REPGSZ == 1)       //Group Size가 1인경우
                {
                    pMsgTran.Primary().Item("REPGSZCOUNTER").Putvalue(1);
                    pMsgTran.Primary().Item("SCTIME" + 0).Putvalue(subUnit.TRID(dintTRID).Group(1).ReadTime);

                    int dintSVIDCount = subUnit.TRID(dintTRID).Group(1).SVIDCount;
                    pMsgTran.Primary().Item("SVCOUNT" + 0).Putvalue(dintSVIDCount);
                    for (int dintLoop = 1; dintLoop <= dintSVIDCount; dintLoop++)
                    {
                        InfoAct.clsSVID svID = this.pInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(1).SVID(dintLoop);
                     

                        pMsgTran.Primary().Item("SVID" + 0 + (dintLoop - 1)).Putvalue(svID.SVID);
                        pMsgTran.Primary().Item("SVNAME" + 0 + (dintLoop - 1)).Putvalue(svID.Name);
                        //pMsgTran.Primary().Item("SV" + 0 + (dintLoop - 1)).Putvalue(svID.Value);
                        // 20130502 조영훈 위의 경우 벨류가 공백임
                        pMsgTran.Primary().Item("SV" + 0 + (dintLoop - 1)).Putvalue(this.pInfo.Unit(0).SubUnit(0).SVID(svID.SVID).Value);
                    }
                }
                else if (subUnit.TRID(dintTRID).REPGSZ > 1)    //Group Size가 1이상인 경우
                {
                    int dintGroupSize = subUnit.TRID(dintTRID).REPGSZ;
                    pMsgTran.Primary().Item("REPGSZCOUNTER").Putvalue(dintGroupSize);

                    for (int dintLoop = 1; dintLoop <= dintGroupSize; dintLoop++)
                    {
                        InfoAct.clsGroup reportGroup = this.pInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintLoop);

                        pMsgTran.Primary().Item("SCTIME" + (dintLoop - 1)).Putvalue(reportGroup.ReadTime);
                        pMsgTran.Primary().Item("SVCOUNT" + (dintLoop - 1)).Putvalue(reportGroup.SVIDCount);

                        for (int dintLoop2 = 1; dintLoop2 <= reportGroup.SVIDCount; dintLoop2++)
                        {
                            pMsgTran.Primary().Item("SVID" + (dintLoop - 1) + (dintLoop2 - 1)).Putvalue(reportGroup.SVID(dintLoop2).SVID);
                            //pMsgTran.Primary().Item("SVNAME" + (dintLoop - 1) + (dintLoop2 - 1)).Putvalue(reportGroup.SVID(dintLoop2).Name);
                            //pMsgTran.Primary().Item("SV" + (dintLoop - 1) + (dintLoop2 - 1)).Putvalue(reportGroup.SVID(dintLoop2).Value); 
                            pMsgTran.Primary().Item("SVNAME" + (dintLoop - 1) + (dintLoop2 - 1)).Putvalue(this.pInfo.Unit(0).SubUnit(0).SVID(reportGroup.SVID(dintLoop2).SVID).Name);
                            pMsgTran.Primary().Item("SV" + (dintLoop - 1) + (dintLoop2 - 1)).Putvalue(this.pInfo.Unit(0).SubUnit(0).SVID(reportGroup.SVID(dintLoop2).SVID).Value);
                        }
                    }
                }

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
