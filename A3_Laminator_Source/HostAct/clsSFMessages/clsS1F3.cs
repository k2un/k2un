using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS1F3 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS1F3 Instance = new clsS1F3();
        #endregion

        #region Constructors
        public clsS1F3()
        {
            this.IntStream = 1;
            this.IntFunction = 3;
            this.StrPrimaryMsgName = "S1F3";
            this.StrSecondaryMsgName = "S1F4";
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
            int dintSVIDCount = 0;
            int dintSVID = 0;
            int dintSVIDCnt = 0;

            ArrayList darraySVID = new ArrayList();
            ArrayList array2 = new ArrayList();

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                if (dstrModuleID != this.pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("SVCNT").Putvalue(0);

                    funSendReply(msgTran);
                    return;
                }

                dintSVIDCount = Convert.ToInt32(msgTran.Primary().Item("SVCNT").Getvalue());

                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);

                if (dintSVIDCount == 0)   //SVIDCount == 0 : 모든 SVID에 대해 보고
                {
                    int tintSVIDCount = this.pInfo.Unit(0).SubUnit(0).SVIDCount;
                    msgTran.Secondary().Item("SVCNT").Putvalue(tintSVIDCount);

                    foreach (int intSVID in this.pInfo.Unit(0).SubUnit(0).SVID())
                    {
                        darraySVID.Add(this.pInfo.Unit(0).SubUnit(0).SVID(intSVID).SVID);
                    }
                    darraySVID.Sort();

                    for (int dintLoop = 0; dintLoop < tintSVIDCount; dintLoop++)
                    {
                        dintSVID = Convert.ToInt32(darraySVID[dintLoop].ToString());
                        InfoAct.clsSVID svID = pInfo.Unit(0).SubUnit(0).SVID(dintSVID);

                        msgTran.Secondary().Item("SVID" + dintLoop).Putvalue(svID.SVID);
                        msgTran.Secondary().Item("SV" + dintLoop).Putvalue(svID.Value);
                        msgTran.Secondary().Item("SVNAME" + dintLoop).Putvalue(svID.Name);
                        msgTran.Secondary().Item("DATATYPE" + (dintLoop)).Putvalue(svID.Type.ToUpper());    //2012.09.19 Kim Youngsik for SMD A3,V1
                        msgTran.Secondary().Item("MODULEID1" + (dintLoop)).Putvalue(svID.ModuleID);          //2012.09.19 Kim Youngsik for SMD A3,V1
                    }

                    funSendReply(msgTran);
                }
                else
                {
                    //받은 SVID중에 존재하지 않는것이 하나라도 있으면 L, 0으로 보고한다.
                    for (int dintLoop = 1; dintLoop <= dintSVIDCount; dintLoop++)
                    {
                        dintSVID = Convert.ToInt32(msgTran.Primary().Item("SVID" + (dintLoop - 1)).Getvalue());

                        int svIDCount = pInfo.Unit(0).SubUnit(0).SVIDCount;
                         foreach (int dintSvCnt in pInfo.Unit(0).SubUnit(0).SVID())
                        {
                            InfoAct.clsSVID svID = pInfo.Unit(0).SubUnit(0).SVID(dintSvCnt);
                        //for (int dintSvCnt = 1; dintSvCnt <= svIDCount; dintSvCnt++)
                        //{
                            //InfoAct.clsSVID svID = pInfo.Unit(0).SubUnit(0).SVID(dintSvCnt);

                            if (dintSVID == svID.SVID)
                            {
                                dintSVIDCnt += 1;
                            }
                        }
                    }

                    if (dintSVIDCnt != dintSVIDCount)
                    {
                        msgTran.Secondary().Item("SVCNT").Putvalue(0);
                        funSendReply(msgTran);

                        return;
                    }

                    if (dintSVIDCnt == 0)
                    {
                        msgTran.Secondary().Item("SVCNT").Putvalue(0);
                        funSendReply(msgTran);
                        return;
                    }

                    msgTran.Secondary().Item("SVCNT").Putvalue(dintSVIDCount);

                    for (int dintLoop = 1; dintLoop <= dintSVIDCount; dintLoop++)
                    {
                        dintSVID = Convert.ToInt32(msgTran.Primary().Item("SVID" + (dintLoop - 1)).Getvalue());

                        // SVID 중 글라스 DATA 항목 6는 SVID가 6000번 부터 사용하기 때문에 객체 검색으로 하면 안됨.
                        //수정 : 20100219 이상호
                        int svIDCount = pInfo.Unit(0).SubUnit(0).SVIDCount;
                        foreach (int dintSvCnt in pInfo.Unit(0).SubUnit(0).SVID())
                        {
                            InfoAct.clsSVID svID = pInfo.Unit(0).SubUnit(0).SVID(dintSvCnt);

                            if (dintSVID == svID.SVID)
                            {
                                msgTran.Secondary().Item("SVID" + (dintLoop - 1)).Putvalue(dintSVID);
                                msgTran.Secondary().Item("SV" + (dintLoop - 1)).Putvalue(svID.Value);
                                msgTran.Secondary().Item("SVNAME" + (dintLoop - 1)).Putvalue(svID.Name);
                                msgTran.Secondary().Item("DATATYPE" + (dintLoop - 1)).Putvalue(svID.Type.ToUpper());    //2012.09.19 Kim Youngsik for SMD A3,V1
                                msgTran.Secondary().Item("MODULEID1" + (dintLoop - 1)).Putvalue(svID.ModuleID);          //2012.09.19 Kim Youngsik for SMD A3,V1
                            }
                        }
                    }

                    funSendReply(msgTran);
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
