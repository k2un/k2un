using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11RelatedPortEvent : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11RelatedPortEvent Instance = new clsS6F11RelatedPortEvent();
        #endregion

        #region Constructors
        public clsS6F11RelatedPortEvent()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11RelatedPortEvent";
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
            int dintPortID = 0;
            string strType = "";
            string dstrCaseID = "";
            int dintUnitID = 0;
            int dintSubUnitID = 0;

            try
            {
                arrayEvent = strParameters.Split(',');

                int dintCEID = Convert.ToInt32(arrayEvent[1]);   //CEID
                dintPortID = Convert.ToInt32(arrayEvent[3]);    //UnitID
                strType = arrayEvent[2];
                dstrCaseID = arrayEvent[4];
                dintUnitID = Convert.ToInt32(arrayEvent[5]);
                dintSubUnitID = Convert.ToInt32(arrayEvent[6]);

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);  //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("RPTID").Putvalue(1);
                pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(pInfo.Unit(0).SubUnit(0).EQPState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(pInfo.Unit(0).SubUnit(0).EQPProcessState);
                pMsgTran.Primary().Item("BYWHO").Putvalue("3");
                pMsgTran.Primary().Item("OPERID").Putvalue(pInfo.All.UserID);

                pMsgTran.Primary().Item("RPTID1").Putvalue(2);      //Fixed Value;
                pMsgTran.Primary().Item("PORTID").Putvalue(pInfo.Port(dintPortID).HostReportPortID);

                switch (dintCEID)
                {
                    case 31:
                        pInfo.Port(dintPortID).PortState = "0";
                        break;
                    case 32:
                        pInfo.Port(dintPortID).PortState = "1";
                        break;
                    case 33:
                        pInfo.Port(dintPortID).PortState = "2";
                        break;
                    case 34:
                        pInfo.Port(dintPortID).PortState = "5";
                        break;
                    case 35:
                        pInfo.Port(dintPortID).PortState = "0";
                        break;
                }

                pMsgTran.Primary().Item("PORT_STATE").Putvalue(pInfo.Port(dintPortID).PortState);
				
                #region 150122 고석현
                //Port Type = 2 - 150323 고석현
                //if (dintPortID < 0)
                //{
                //    pMsgTran.Primary().Item("PORT_TYPE").Putvalue(12); //FILM CST
                //}
                //else
                //{
                //    pMsgTran.Primary().Item("PORT_TYPE").Putvalue(13); //13 PAPER CST
                //}
                #endregion

                pMsgTran.Primary().Item("PORT_TYPE").Putvalue(pInfo.Port(dintPortID).PortType);


                pMsgTran.Primary().Item("PORT_MODE").Putvalue("OK");
                pMsgTran.Primary().Item("SORT_TYPE").Putvalue("0");

                pMsgTran.Primary().Item("RPTID2").Putvalue(4);      //Fixed Value;
                pMsgTran.Primary().Item("MATERIALCOUNT").Putvalue(1);

                //for (int dintLoop = 0; dintLoop < 1; dintLoop++)
                //{
                #region 150122 고석현 변경
                if (dintUnitID == 1)
                {
                    if (dintCEID == 31 || dintCEID == 32)
                    {
                        pMsgTran.Primary().Item("MATERIAL_KIND").Putvalue("FILMCST");
                        pMsgTran.Primary().Item("MATERIAL_ID").Putvalue("");
                    }
                    else
                    {
                        pMsgTran.Primary().Item("MATERIAL_KIND").Putvalue("FILMCST");
                        pMsgTran.Primary().Item("MATERIAL_ID").Putvalue(pInfo.Port(dintPortID).CSTID);
                    }
                }
                else
                {
                    if (dintCEID == 31 || dintCEID == 32)
                    {
                        pMsgTran.Primary().Item("MATERIAL_KIND").Putvalue("PAPERCST");
                        pMsgTran.Primary().Item("MATERIAL_ID").Putvalue("");
                    }
                    else
                    {
                        pMsgTran.Primary().Item("MATERIAL_KIND").Putvalue("PAPERCST");
                        pMsgTran.Primary().Item("MATERIAL_ID").Putvalue(pInfo.Port(dintPortID).CSTID);
                    }
                }
                #endregion 
                //}
            
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
