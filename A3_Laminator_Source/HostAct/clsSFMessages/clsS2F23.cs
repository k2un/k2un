using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Globalization;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F23 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F23 Instance = new clsS2F23();
        #endregion

        #region Constructors
        public clsS2F23()
        {
            this.IntStream = 2;
            this.IntFunction = 23;
            this.StrPrimaryMsgName = "S2F23";
            this.StrSecondaryMsgName = "S2F24";
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
            DateTime dDateTime;
            int intTRID = 0;
            string dstrSMPTIME = "";        //A2 사양서에서 추가 기존 DSPER 대체 20101018 어우수
            int dintTOTSMP = 0;
            int dintGRSIZE = 0;             //A2 사양서에서 추가 기존 REPGSZ 대체 20101018 어우수
            int dintSVID = 0;
            int dintSVCount = 0;

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                intTRID = Convert.ToInt32(msgTran.Primary().Item("TRID").Getvalue());
                dstrSMPTIME = msgTran.Primary().Item("SMPTIME").Getvalue().ToString().Trim();
                dintTOTSMP = Convert.ToInt32(msgTran.Primary().Item("TOTSMP").Getvalue());
                dintGRSIZE = Convert.ToInt32(msgTran.Primary().Item("GRSIZE").Getvalue());
                dintSVCount = Convert.ToInt32(msgTran.Primary().Item("SVCOUNT").Getvalue());

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("TIACK").Putvalue(2);
                    funSendReply(msgTran);

                    return;
                }

                if (intTRID > 100)
                {
                    msgTran.Secondary().Item("TIACK").Putvalue(3);
                    funSendReply(msgTran);

                    return;
                }

                //TRID Reset의 경우는 해당 TRID가 있는지만 체크한다.(다른 항목은 체크안함)
                if (pInfo.Unit(0).SubUnit(0).TRID(intTRID) != null)
                {
                    if (dintSVCount == 0)
                    {
                        //해당 TRID를 Reset하고 제거한다.
                        pInfo.Unit(0).SubUnit(0).RemoveTRID(intTRID);

                        msgTran.Secondary().Item("TIACK").Putvalue(0);
                        funSendReply(msgTran);

                        return;
                    }
                    else
                    {
                        //해당 TRID를 Reset하고 제거한다.
                        pInfo.Unit(0).SubUnit(0).RemoveTRID(intTRID);
                    }
                }
                else
                {
                    if (dintSVCount == 0)
                    {
                        msgTran.Secondary().Item("TIACK").Putvalue(3);
                        funSendReply(msgTran);

                        return;
                    }
                }

                //시간 Format 체크
                try
                {
                    dDateTime = DateTime.ParseExact(dstrSMPTIME.Substring(0, 4), "mmss", CultureInfo.InvariantCulture);

                    //추가 2010.12.08   송 은선
                    if (Convert.ToInt32(dstrSMPTIME.Substring(0, 4)) < 1)
                    {
                        msgTran.Secondary().Item("TIACK").Putvalue(1);
                        funSendReply(msgTran);

                        return;
                    }

                }
                catch
                {
                    msgTran.Secondary().Item("TIACK").Putvalue(1);
                    funSendReply(msgTran);

                    return;
                }

                //GRSIZE가 0인지 체크
                if (dintGRSIZE == 0)
                {
                    msgTran.Secondary().Item("TIACK").Putvalue(1);
                    funSendReply(msgTran);

                    return;
                }

                //받은 SVID중에 하나라도 존재하지 않는 것이면 NAK
                for (int dintLoop = 1; dintLoop <= dintSVCount; dintLoop++)
                {
                    dintSVID = Convert.ToInt32(msgTran.Primary().Item("SVID" + (dintLoop - 1)).Getvalue());

                    if (pInfo.Unit(0).SubUnit(0).SVID(dintSVID) == null)
                    {
                        msgTran.Secondary().Item("TIACK").Putvalue(1);
                        funSendReply(msgTran);

                        return;
                    }
                }

                if (dintSVCount > 0)
                {
                    if (pInfo.Unit(0).SubUnit(0).AddTRID(intTRID) == true)
                    {
                        pInfo.Unit(0).SubUnit(0).TRID(intTRID).DSPER = dstrSMPTIME.Substring(0, 4);            //수정 2010.12.08   송은선
                        pInfo.Unit(0).SubUnit(0).TRID(intTRID).REPGSZ = dintGRSIZE;
                        pInfo.Unit(0).SubUnit(0).TRID(intTRID).TOTSMP = dintTOTSMP;

                        for (int dintGroup = 1; dintGroup <= dintGRSIZE; dintGroup++)
                        {
                            if (pInfo.Unit(0).SubUnit(0).TRID(intTRID).AddGroup(dintGroup) == true)
                            {
                                //HOST에서 받은 항목을 추가한다.
                                for (int dintLoop = 1; dintLoop <= dintSVCount; dintLoop++)
                                {
                                    dintSVID = Convert.ToInt32(msgTran.Primary().Item("SVID" + (dintLoop - 1)).Getvalue());

                                    if (pInfo.Unit(0).SubUnit(0).TRID(intTRID).Group(dintGroup).AddSVID(dintLoop) == true)
                                    {
                                        pInfo.Unit(0).SubUnit(0).TRID(intTRID).Group(dintGroup).SVID(dintLoop).SVID = dintSVID;
                                        pInfo.Unit(0).SubUnit(0).TRID(intTRID).Group(dintGroup).SVID(dintLoop).Name = pInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name;
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    //현재 TRID가 한개도 없는데 Trace Reset이 온 경우 NAK
                    if (pInfo.Unit(0).SubUnit(0).TRIDCount == 0)
                    {
                        msgTran.Secondary().Item("TIACK").Putvalue(3);
                        funSendReply(msgTran);

                        return;
                    }

                    if (pInfo.Unit(0).SubUnit(0).TRID(intTRID) == null)
                    {
                        msgTran.Secondary().Item("TIACK").Putvalue(3);
                        funSendReply(msgTran);

                        return;
                    }

                    //해당 TRID를 Reset하고 제거한다.
                    pInfo.Unit(0).SubUnit(0).RemoveTRID(intTRID);
                }

                msgTran.Secondary().Item("TIACK").Putvalue(0);
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
