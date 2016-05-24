using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F103 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F103 Instance = new clsS6F103();
        #endregion

        #region Constructors
        public clsS6F103()
        {
            this.IntStream = 6;
            this.IntFunction = 103;
            this.StrPrimaryMsgName = "S6F103";
            this.StrSecondaryMsgName = "S6F104";
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
            string dstrLotID = "";
            int dintPortID = 0;
            int dintIndex = 0;
            string dstrSLOTMAP = "";
            string dstrGLSID = "";
            try
            {
                arrayEvent = strParameters.Split(',');

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);
                dstrLotID = arrayEvent[1].Trim();
                InfoAct.clsLOT CurrentLot = pInfo.LOTID(dstrLotID);
                pMsgTran.Primary().Item("PTID").Putvalue("P0"+CurrentLot.InPortID);
                pMsgTran.Primary().Item("LOTID").Putvalue(dstrLotID);
                pMsgTran.Primary().Item("CSTID").Putvalue(CurrentLot.CSTID);
                pMsgTran.Primary().Item("LOTJUDGE").Putvalue(CurrentLot.LOTJudge);
                pMsgTran.Primary().Item("LSORTTYPE").Putvalue(CurrentLot.LSORTTYPE);
                pMsgTran.Primary().Item("OPERID").Putvalue(CurrentLot.OPERID);
                pMsgTran.Primary().Item("PRODID").Putvalue(CurrentLot.PRODID);
                StringBuilder sb;
                dintPortID = pInfo.LOTID(dstrLotID).InPortID;

                #region 수정
                for (int dintSlotNo = 0; dintSlotNo < pInfo.Port(dintPortID).SlotCount; dintSlotNo++)
                {
                    if (pInfo.Port(dintPortID).Slot(dintSlotNo + 1).SLOTINFO != "E")
                    {
                        dintIndex++;
                    }
                }

                pMsgTran.Primary().Item("QTY").Putvalue(dintIndex);
                pMsgTran.Primary().Item("L2").Putvalue(25);
                dintIndex = 0;
                for (int dintSlotNo = 0; dintSlotNo < pInfo.Port(dintPortID).SlotCount; dintSlotNo++)
                {
                    InfoAct.clsLOT CurrentLOT = pInfo.LOTID(dstrLotID);
                    dstrGLSID = pInfo.Port(dintPortID).Slot(dintSlotNo + 1).GLSID;
                    if (string.IsNullOrEmpty(dstrGLSID) == false && CurrentLOT.GLSID(dstrGLSID) != null && pInfo.Port(dintPortID).Slot(dintSlotNo + 1).SLOTINFO != "E")
                    {
                        InfoAct.clsGLS CurrentGLS = CurrentLOT.GLSID(dstrGLSID);
                        if (CurrentGLS.ScrapFlag)
                        {
                            pMsgTran.Primary().Item("SLOTNO" + dintIndex).Putvalue((dintSlotNo + 1).ToString());
                            pMsgTran.Primary().Item("GLSID" + dintIndex).Putvalue("");
                            pMsgTran.Primary().Item("PPID" + dintIndex).Putvalue("");
                            pMsgTran.Primary().Item("GLSJUDGE" + dintIndex).Putvalue("");
                            pMsgTran.Primary().Item("GSORTTYPE" + dintIndex).Putvalue(0);
                            pMsgTran.Primary().Item("SMPLFLAG" + dintIndex).Putvalue(0);
                            pMsgTran.Primary().Item("RWKCNT" + dintIndex).Putvalue(0);

                            pMsgTran.Primary().Item("L4" + dintIndex).Putvalue(0);
                            dstrSLOTMAP += "X";
                        }
                        else
                        {
                            pMsgTran.Primary().Item("SLOTNO" + dintIndex).Putvalue(CurrentGLS.SlotID);
                            pMsgTran.Primary().Item("GLSID" + dintIndex).Putvalue(CurrentGLS.GLSID);
                            pMsgTran.Primary().Item("PPID" + dintIndex).Putvalue(CurrentGLS.HostPPID);
                            pMsgTran.Primary().Item("GLSJUDGE" + dintIndex).Putvalue(CurrentGLS.GLSJudge);
                            pMsgTran.Primary().Item("GSORTTYPE" + dintIndex).Putvalue(CurrentGLS.GSORTTYPE);
                            pMsgTran.Primary().Item("SMPLFLAG" + dintIndex).Putvalue(CurrentGLS.SMPLFLAG);
                            pMsgTran.Primary().Item("RWKCNT" + dintIndex).Putvalue(CurrentGLS.RWKCNT);

                            pMsgTran.Primary().Item("L4" + dintIndex).Putvalue(0);

                            if (pInfo.Port(dintPortID).Slot(dintSlotNo + 1).SLOTINFO == "P")// || pInfo.Port(dintPortID).Slot(dintSlotNo + 1).SLOTINFO == "F" || pInfo.Port(dintPortID).Slot(dintSlotNo + 1).SLOTINFO == "S")
                            {
                                dstrSLOTMAP += "O";
                            }
                            else
                            {
                                dstrSLOTMAP += "X";
                            }
                        }
                    }
                    else
                    {
                        pMsgTran.Primary().Item("SLOTNO" + dintIndex).Putvalue((dintSlotNo + 1).ToString());
                        pMsgTran.Primary().Item("GLSID" + dintIndex).Putvalue("");
                        pMsgTran.Primary().Item("PPID" + dintIndex).Putvalue("");
                        pMsgTran.Primary().Item("GLSJUDGE" + dintIndex).Putvalue("");
                        pMsgTran.Primary().Item("GSORTTYPE" + dintIndex).Putvalue(0);
                        pMsgTran.Primary().Item("SMPLFLAG" + dintIndex).Putvalue(0);
                        pMsgTran.Primary().Item("RWKCNT" + dintIndex).Putvalue(0);

                        pMsgTran.Primary().Item("L4" + dintIndex).Putvalue(0);
                        dstrSLOTMAP += "X";
                    }

                    dintIndex++;
                }
                pMsgTran.Primary().Item("SLOTSEL").Putvalue(dstrSLOTMAP);

                #endregion

                #region 이전 소스
                //if (pInfo.Port(pInfo.LOTID(dstrLotID).InPortID).AbortFlag)
                //{
                //    int GLSRunCnt = 0;
                //    foreach (string str in pInfo.LOTID(dstrLotID).GLS())
                //    {
                //        InfoAct.clsGLS CurrentGLS = pInfo.LOTID(dstrLotID).GLSID(str);
                //        if (CurrentGLS.RunState == "E")
                //        {
                //            GLSRunCnt++;
                //        }
                //    }
                //    pMsgTran.Primary().Item("QTY").Putvalue(GLSRunCnt);

                //    pMsgTran.Primary().Item("L2").Putvalue(GLSRunCnt);
                //    sb = new StringBuilder("X".PadRight(25, 'X'));
                //    foreach (string strGLSID in CurrentLot.GLS())
                //    {

                //        InfoAct.clsGLS CurrentGLS = CurrentLot.GLSID(strGLSID);
                //        if (CurrentGLS.RunState == "E")
                //        {
                //            sb[CurrentGLS.SlotID - 1] = 'O';

                //            pMsgTran.Primary().Item("SLOTNO" + dintIndex).Putvalue(CurrentGLS.SlotID);
                //            pMsgTran.Primary().Item("GLSID" + dintIndex).Putvalue(CurrentGLS.GLSID);
                //            pMsgTran.Primary().Item("PPID" + dintIndex).Putvalue(CurrentGLS.HostPPID);
                //            pMsgTran.Primary().Item("GLSJUDGE" + dintIndex).Putvalue(CurrentGLS.GLSJudge);
                //            pMsgTran.Primary().Item("GSORTTYPE" + dintIndex).Putvalue(CurrentGLS.GSORTTYPE);
                //            pMsgTran.Primary().Item("SMPLFLAG" + dintIndex).Putvalue(CurrentGLS.SMPLFLAG);
                //            pMsgTran.Primary().Item("RWKCNT" + dintIndex).Putvalue(CurrentGLS.RWKCNT);

                //            pMsgTran.Primary().Item("L4" + dintIndex).Putvalue(0);
                //            pMsgTran.Primary().Item("PNLJUDGE" + dintIndex + 0).Putvalue("");
                //            pMsgTran.Primary().Item("PNLGRADE" + dintIndex + 0).Putvalue("");
                //            dintIndex++;
                //        }
                //    }
                //}
                //else
                //{
                //    pMsgTran.Primary().Item("L2").Putvalue(CurrentLot.GLSCount);
                //    sb = new StringBuilder("X".PadRight(25, 'X'));

                //    foreach (string strGLSID in CurrentLot.GLS())
                //    {
                //        InfoAct.clsGLS CurrentGLS = CurrentLot.GLSID(strGLSID);
                //        sb[CurrentGLS.SlotID - 1] = 'O';

                //        pMsgTran.Primary().Item("SLOTNO" + dintIndex).Putvalue(CurrentGLS.SlotID);
                //        pMsgTran.Primary().Item("GLSID" + dintIndex).Putvalue(CurrentGLS.GLSID);
                //        pMsgTran.Primary().Item("PPID" + dintIndex).Putvalue(CurrentGLS.HostPPID);
                //        pMsgTran.Primary().Item("GLSJUDGE" + dintIndex).Putvalue(CurrentGLS.GLSJudge);
                //        pMsgTran.Primary().Item("GSORTTYPE" + dintIndex).Putvalue(CurrentGLS.GSORTTYPE);
                //        pMsgTran.Primary().Item("SMPLFLAG" + dintIndex).Putvalue(CurrentGLS.SMPLFLAG);
                //        pMsgTran.Primary().Item("RWKCNT" + dintIndex).Putvalue(CurrentGLS.RWKCNT);

                //        pMsgTran.Primary().Item("L4" + dintIndex).Putvalue(0);
                //        pMsgTran.Primary().Item("PNLJUDGE" + dintIndex + 0).Putvalue("");
                //        pMsgTran.Primary().Item("PNLGRADE" + dintIndex + 0).Putvalue("");
                //        dintIndex++;
                //    }
                //    pMsgTran.Primary().Item("QTY").Putvalue(dintIndex);

                //}
                //pMsgTran.Primary().Item("SLOTSEL").Putvalue(sb.ToString());

                #endregion

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
