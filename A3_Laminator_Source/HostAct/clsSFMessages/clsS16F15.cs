using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;

namespace HostAct
{
    public class clsS16F15 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS16F15 Instance = new clsS16F15();
        #endregion

        #region Constructors
        public clsS16F15()
        {
            this.intStream = 16;
            this.intFunction = 15;
            this.strPrimaryMsgName = "S16F15";
            this.strSecondaryMsgName = "S16F16";
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
            string dstrHGLSID = "";
            int dintIndex = 0;
            string[] arrP_Moduleid;
            string[] arrP_Order;
            string[] arrP_Status;


            try
            {
                arrayEvent = strParameters.Split(',');

                pMsgTran = pSecsDrv.MakeTransaction(this.strPrimaryMsgName);



                string dstrMode = arrayEvent[1].ToString();        //MDOE(Creation :1, Deletion : 2, Expiration : 3, Override : 4)


                pMsgTran.Primary().Item("MODULEID").Putvalue(this.pInfo.Unit(0).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("MODE").Putvalue(dstrMode);
                pMsgTran.Primary().Item("BYWHO").Putvalue(3);   //Fix 3

                if (dstrMode == "3")
                {
                    //만료 보고
                }
                else if (dstrMode == "2") //삭제
                {
                    dstrHGLSID = arrayEvent[2];
                    if (dstrHGLSID == "") // 전체삭제
                    {
                        pMsgTran.Primary().Item("L2").Putvalue(this.pInfo.PPC().Count);

                        foreach (string strGLSID in this.pInfo.PPC())
                        {
                            InfoAct.clsPPC CurrentPPC = this.pInfo.PPC(strGLSID);

                            pMsgTran.Primary().Item("H_GLASSID" + dintIndex).Putvalue(CurrentPPC.HGLSID);
                            pMsgTran.Primary().Item("JOBID" + dintIndex).Putvalue(CurrentPPC.JOBID);
                            pMsgTran.Primary().Item("SET_TIME" + dintIndex).Putvalue(CurrentPPC.SetTime.ToString("yyyyMMddmmssff"));

                            arrP_Moduleid = CurrentPPC.P_MODULEID;
                            arrP_Order = CurrentPPC.P_ORDER;
                            arrP_Status = CurrentPPC.P_STATUS;

                            pMsgTran.Primary().Item("L3" + dintIndex).Putvalue(arrP_Moduleid.Length);
                            for (int dintLoop = 0; dintLoop < arrP_Moduleid.Length; dintLoop++)
                            {
                                pMsgTran.Primary().Item("P_MODULEID" + dintIndex + dintLoop).Putvalue(arrP_Moduleid[dintLoop]);
                                pMsgTran.Primary().Item("P_ORDER" + dintIndex + dintLoop).Putvalue(arrP_Order[dintLoop]);
                                pMsgTran.Primary().Item("P_STATUS" + dintIndex + dintLoop).Putvalue(arrP_Status[dintLoop]);
                            }
                            dintIndex++;
                        }

                        this.pInfo.RemoveAPC();
                    }
                    else //한개씩 삭제
                    {
                        pMsgTran.Primary().Item("L2").Putvalue(1);

                        InfoAct.clsPPC CurrentPPC = this.pInfo.PPC(dstrHGLSID);

                        pMsgTran.Primary().Item("H_GLASSID" + dintIndex).Putvalue(CurrentPPC.HGLSID);
                        pMsgTran.Primary().Item("JOBID" + dintIndex).Putvalue(CurrentPPC.JOBID);
                        pMsgTran.Primary().Item("SET_TIME" + dintIndex).Putvalue(CurrentPPC.SetTime.ToString("yyyyMMddmmssff"));

                        arrP_Moduleid = CurrentPPC.P_MODULEID;
                        arrP_Order = CurrentPPC.P_ORDER;
                        arrP_Status = CurrentPPC.P_STATUS;

                        pMsgTran.Primary().Item("L3" + dintIndex).Putvalue(arrP_Moduleid.Length);
                        for (int dintLoop = 0; dintLoop < arrP_Moduleid.Length; dintLoop++)
                        {
                            pMsgTran.Primary().Item("P_MODULEID" + dintIndex + dintLoop).Putvalue(arrP_Moduleid[dintLoop]);
                            pMsgTran.Primary().Item("P_ORDER" + dintIndex + dintLoop).Putvalue(arrP_Order[dintLoop]);
                            pMsgTran.Primary().Item("P_STATUS" + dintIndex + dintLoop).Putvalue(arrP_Status[dintLoop]);
                        }
                    }
                }
                else // 생성, 수정
                {
                    //사양서에 따르면 한개씩 보고 되게 되어있음.
                    pMsgTran.Primary().Item("L2").Putvalue(1);

                    InfoAct.clsPPC CurrentPPC = this.pInfo.PPC(arrayEvent[2].ToString());

                    pMsgTran.Primary().Item("H_GLASSID" + dintIndex).Putvalue(CurrentPPC.HGLSID);
                    pMsgTran.Primary().Item("JOBID" + dintIndex).Putvalue(CurrentPPC.JOBID);
                    pMsgTran.Primary().Item("SET_TIME" + dintIndex).Putvalue(CurrentPPC.SetTime.ToString("yyyyMMddmmssff"));

                    arrP_Moduleid = CurrentPPC.P_MODULEID;
                    arrP_Order = CurrentPPC.P_ORDER;
                    arrP_Status = CurrentPPC.P_STATUS;

                    pMsgTran.Primary().Item("L3" + dintIndex).Putvalue(arrP_Moduleid.Length);
                    for (int dintLoop = 0; dintLoop < arrP_Moduleid.Length; dintLoop++)
                    {
                        pMsgTran.Primary().Item("P_MODULEID" + dintIndex + dintLoop).Putvalue(arrP_Moduleid[dintLoop]);
                        pMsgTran.Primary().Item("P_ORDER" + dintIndex + dintLoop).Putvalue(arrP_Order[dintLoop]);
                        pMsgTran.Primary().Item("P_STATUS" + dintIndex + dintLoop).Putvalue(arrP_Status[dintLoop]);
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
