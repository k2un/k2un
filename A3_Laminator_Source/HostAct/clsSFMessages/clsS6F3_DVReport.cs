using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F3_DVReport : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F3_DVReport Instance = new clsS6F3_DVReport();
        #endregion

        #region Constructors
        public clsS6F3_DVReport()
        {
            this.IntStream = 6;
            this.IntFunction = 3;
            this.StrPrimaryMsgName = "S6F3_DVReport";
            this.StrSecondaryMsgName = "S6F4";
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

                int dintCEID = Convert.ToInt32(arrayEvent[1]);
                try
                {
                    if (pInfo.CEID(dintCEID).Report == false)
                    {
                        return null;
                    }
                }
                catch (Exception)
                {

                }
                if (pInfo.All.DataID >= 9999)
                {
                    pInfo.All.DataID = -1;
                }
                pInfo.All.DataID++;

                int dintUnitID = Convert.ToInt32(arrayEvent[2]);
                int dintSubunitID = Convert.ToInt32(arrayEvent[3]);
                string dstrLotID = arrayEvent[4].ToString().Trim();
                string dstrGLSID = arrayEvent[5].ToString().Trim();

                if (pInfo.LOTID(dstrLotID) != null && pInfo.LOTID(dstrLotID).GLSID(dstrGLSID) != null)
                {
                    if (pInfo.LOTID(dstrLotID).GLSID(dstrGLSID).ScrapFlag)
                    {
                        return null;
                    }
                }

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                InfoAct.clsSubUnit subUnit = this.pInfo.Unit(dintUnitID).SubUnit(dintSubunitID);
                if (dintCEID == 500)
                {
                    pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                    pMsgTran.Primary().Item("UNITID").Putvalue(subUnit.ReportUnitID);
                    pMsgTran.Primary().Item("LOTID").Putvalue(dstrLotID);
                    pMsgTran.Primary().Item("CSTID").Putvalue(pInfo.LOTID(dstrLotID).CSTID);
                    pMsgTran.Primary().Item("GLSID").Putvalue(dstrGLSID);
                    pMsgTran.Primary().Item("CHID").Putvalue("");
                    pMsgTran.Primary().Item("OPERID").Putvalue(pInfo.LOTID(dstrLotID).OPERID);
                    pMsgTran.Primary().Item("PRODID").Putvalue(pInfo.LOTID(dstrLotID).PRODID);
                    pMsgTran.Primary().Item("PPID").Putvalue(pInfo.LOTID(dstrLotID).GLSID(dstrGLSID).HostPPID);

                    if (dintUnitID == 6)
                    {
                        dintUnitID = 5;
                    }
                    pMsgTran.Primary().Item("L3").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).GLSAPDCount);
                    for (int dintLoop = 0; dintLoop < pInfo.Unit(dintUnitID).SubUnit(0).GLSAPDCount; dintLoop++)
                    {
                        if (pInfo.Unit(dintUnitID).SubUnit(0).GLSAPD(dintLoop + 1).Length != 6)
                        {
                            pMsgTran.Primary().Item("DVNAME" + dintLoop).Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).GLSAPD(dintLoop + 1).Name);
                            pMsgTran.Primary().Item("DV" + dintLoop).Putvalue((pInfo.Unit(dintUnitID).SubUnit(0).GLSAPD(dintLoop + 1).Value).ToString());
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(pInfo.Unit(dintUnitID).SubUnit(0).GLSAPD(dintLoop + 1).LoadingTIME) == false)
                            {
                                pMsgTran.Primary().Item("DVNAME" + dintLoop).Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).GLSAPD(dintLoop + 1).Name);
                                pMsgTran.Primary().Item("DV" + dintLoop).Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).GLSAPD(dintLoop + 1).LoadingTIME);
                            }

                            if (string.IsNullOrEmpty(pInfo.Unit(dintUnitID).SubUnit(0).GLSAPD(dintLoop + 1).UnloadingTIME) == false)
                            {
                                pMsgTran.Primary().Item("DVNAME" + dintLoop).Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).GLSAPD(dintLoop + 1).Name);
                                pMsgTran.Primary().Item("DV" + dintLoop).Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).GLSAPD(dintLoop + 1).UnloadingTIME);
                            }
                        }
                    }
                }
                else
                {
                    pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                    pMsgTran.Primary().Item("UNITID").Putvalue(subUnit.ReportUnitID);
                    pMsgTran.Primary().Item("LOTID").Putvalue(dstrLotID);
                    pMsgTran.Primary().Item("CSTID").Putvalue(pInfo.LOTID(dstrLotID).CSTID);
                    pMsgTran.Primary().Item("GLSID").Putvalue("");
                    pMsgTran.Primary().Item("CHID").Putvalue("");
                    pMsgTran.Primary().Item("OPERID").Putvalue(pInfo.LOTID(dstrLotID).OPERID);
                    pMsgTran.Primary().Item("PRODID").Putvalue(pInfo.LOTID(dstrLotID).PRODID);
                    pMsgTran.Primary().Item("PPID").Putvalue(pInfo.LOTID(dstrLotID).GLSID(dstrGLSID).HostPPID);

                    pMsgTran.Primary().Item("L3").Putvalue(pInfo.LOTID(dstrLotID).LOTAPDCount);

                    float flDVData = 0;
                    for (int dintLoop = 0; dintLoop < pInfo.LOTID(dstrLotID).LOTAPDCount; dintLoop++)
                    {
                        if (pInfo.LOTID(dstrLotID).LOTAPD(dintLoop + 1).Length != 6)
                        {
                            flDVData = pInfo.LOTID(dstrLotID).LOTAPD(dintLoop + 1).Value;
                            //if(pInfo.Port(pInfo.LOTID(dstrLotID).InPortID).AbortFlag == true)
                            //{
                            int GLSRunCnt = 0;
                            foreach (string str in pInfo.LOTID(dstrLotID).GLS())
                            {
                                InfoAct.clsGLS CurrentGLS = pInfo.LOTID(dstrLotID).GLSID(str);
                                if (CurrentGLS.RunState == "E")
                                {
                                    GLSRunCnt++;
                                }
                            }
                            flDVData = flDVData / GLSRunCnt;

                            //}
                            //else
                            //{
                            //    flDVData = flDVData / pInfo.LOTID(dstrLotID).GLSCount;
                            //}
                            pMsgTran.Primary().Item("DVNAME" + dintLoop).Putvalue(pInfo.LOTID(dstrLotID).LOTAPD(dintLoop + 1).Name);
                            pMsgTran.Primary().Item("DV" + dintLoop).Putvalue(flDVData);
                        }
                        else
                        {
                            //시간 어떻게 할껀지 확인 필요....
                            pMsgTran.Primary().Item("DVNAME" + dintLoop).Putvalue(pInfo.LOTID(dstrLotID).LOTAPD(dintLoop + 1).Name);
                            pMsgTran.Primary().Item("DV" + dintLoop).Putvalue("");
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
