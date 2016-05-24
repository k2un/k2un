using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;
using System.Collections;

namespace HostAct
{
    class clsS16F115 : clsStreamFunction, IStreamFunction
    {
         #region Fields
        #endregion

        #region Properties
        #endregion
        
        #region Singleton
        public static readonly clsS16F115 Instance = new clsS16F115();
        #endregion

        #region Constructors
        public clsS16F115()
        {
			this.intStream = 16;
            this.intFunction = 115;
            this.strPrimaryMsgName = "S16F115";
            this.strSecondaryMsgName = "S16F116";
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
            try
            {
                arrayEvent = strParameters.Split(',');

                pMsgTran = pSecsDrv.MakeTransaction(this.strPrimaryMsgName);

                string[] arrParamName;
                string[] arrParamValue;

                string dstrMode = arrayEvent[1].ToString();        //MDOE(Creation :1, Deletion : 2, Expiration : 3, Override : 4)
                dstrHGLSID = arrayEvent[2];


                pMsgTran.Primary().Item("MODULEID").Putvalue(this.pInfo.Unit(3).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("MODE").Putvalue(dstrMode);

                string[] arrData = dstrHGLSID.Split('!');
                int dintBYWHO = 0;
                switch (dstrMode)
                {
                    case "1":
                        dintBYWHO = 1;
                        pMsgTran.Primary().Item("BYWHO").Putvalue(dintBYWHO);

                        pMsgTran.Primary().Item("L2").Putvalue(arrData.Length);
                        for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                        {
                            InfoAct.clsAPC CurrentAPC = this.pInfo.APC(arrData[dintLoop]);

                            pMsgTran.Primary().Item("H_GLASSID" + dintLoop).Putvalue(CurrentAPC.GLSID);
                            pMsgTran.Primary().Item("JOBID" + dintLoop).Putvalue(CurrentAPC.JOBID);
                            pMsgTran.Primary().Item("RECIPE" + dintLoop).Putvalue(CurrentAPC.EQPPPID);
                            pMsgTran.Primary().Item("SET_TIME" + dintLoop).Putvalue(CurrentAPC.SetTime.ToString("yyyyMMddHHmmssff"));

                            arrParamName = CurrentAPC.ParameterName;
                            arrParamValue = CurrentAPC.ParameterValue;

                            pMsgTran.Primary().Item("L4" + dintLoop).Putvalue(CurrentAPC.ParameterName.Length);
                            for (int dintLoop2 = 0; dintLoop2 < CurrentAPC.ParameterName.Length; dintLoop2++)
                            {
                                pMsgTran.Primary().Item("P_PARM_NAME" + dintLoop + dintLoop2).Putvalue(arrParamName[dintLoop2]);
                                pMsgTran.Primary().Item("P_PARM_VALUE" + dintLoop + dintLoop2).Putvalue(arrParamValue[dintLoop2]);
                            }
                        }
                        break;
                    case "4":
                        dintBYWHO = 1;
                        pMsgTran.Primary().Item("BYWHO").Putvalue(dintBYWHO);

                        pMsgTran.Primary().Item("L2").Putvalue(arrData.Length);
                        for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                        {
                            InfoAct.clsAPC CurrentAPC = this.pInfo.APC(arrData[dintLoop]);

                            pMsgTran.Primary().Item("H_GLASSID" + dintLoop).Putvalue(CurrentAPC.GLSID);
                            pMsgTran.Primary().Item("JOBID" + dintLoop).Putvalue(CurrentAPC.JOBID);
                            pMsgTran.Primary().Item("RECIPE" + dintLoop).Putvalue(CurrentAPC.EQPPPID);
                            pMsgTran.Primary().Item("SET_TIME" + dintLoop).Putvalue(CurrentAPC.SetTime_old.ToString("yyyyMMddHHmmssff"));

                            arrParamName = CurrentAPC.ParameterNameBackup;
                            arrParamValue = CurrentAPC.ParameterValueBackup;

                            pMsgTran.Primary().Item("L4" + dintLoop).Putvalue(CurrentAPC.ParameterNameBackup.Length);
                            for (int dintLoop2 = 0; dintLoop2 < CurrentAPC.ParameterNameBackup.Length; dintLoop2++)
                            {
                                pMsgTran.Primary().Item("P_PARM_NAME" + dintLoop + dintLoop2).Putvalue(arrParamName[dintLoop2]);
                                pMsgTran.Primary().Item("P_PARM_VALUE" + dintLoop + dintLoop2).Putvalue(arrParamValue[dintLoop2]);
                            }
                        }

                        break;

                    case "2":
                        if (pInfo.All.APCDataDel == true)
                        {
                            dintBYWHO = 2;
                            pInfo.All.APCDataDel = false;
                        }
                        else
                        {
                            dintBYWHO = 1;
                        }
                        pMsgTran.Primary().Item("BYWHO").Putvalue(dintBYWHO);

                        if (string.IsNullOrEmpty(arrData[0]) == false)
                        {
                            pMsgTran.Primary().Item("L2").Putvalue(arrData.Length);
                            for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                            {
                                InfoAct.clsAPC CurrentAPC = this.pInfo.APC(arrData[dintLoop]);

                                pMsgTran.Primary().Item("H_GLASSID" + dintLoop).Putvalue(CurrentAPC.GLSID);
                                pMsgTran.Primary().Item("JOBID" + dintLoop).Putvalue(CurrentAPC.JOBID);
                                pMsgTran.Primary().Item("RECIPE" + dintLoop).Putvalue(CurrentAPC.EQPPPID);
                                pMsgTran.Primary().Item("SET_TIME" + dintLoop).Putvalue(CurrentAPC.SetTime.ToString("yyyyMMddHHmmssff"));

                                arrParamName = CurrentAPC.ParameterName;
                                arrParamValue = CurrentAPC.ParameterValue;
                                pMsgTran.Primary().Item("L4" + dintLoop).Putvalue(CurrentAPC.ParameterName.Length);
                                for (int dintLoop2 = 0; dintLoop2 < CurrentAPC.ParameterName.Length; dintLoop2++)
                                {
                                    pMsgTran.Primary().Item("P_PARM_NAME" + dintLoop + dintLoop2).Putvalue(arrParamName[dintLoop2]);
                                    pMsgTran.Primary().Item("P_PARM_VALUE" + dintLoop + dintLoop2).Putvalue(arrParamValue[dintLoop2]);
                                }
                                pInfo.RemoveAPC(arrData[dintLoop]);
                            }
                        }
                        else
                        {
                            pMsgTran.Primary().Item("L2").Putvalue(pInfo.APCCount);
                            int dintCount = 0;
                            foreach (string strAPC in pInfo.APC())                            
                            {
                                InfoAct.clsAPC CurrentAPC = this.pInfo.APC(strAPC);

                                pMsgTran.Primary().Item("H_GLASSID" + dintCount).Putvalue(CurrentAPC.GLSID);
                                pMsgTran.Primary().Item("JOBID" + dintCount).Putvalue(CurrentAPC.JOBID);
                                pMsgTran.Primary().Item("RECIPE" + dintCount).Putvalue(CurrentAPC.EQPPPID);
                                pMsgTran.Primary().Item("SET_TIME" + dintCount).Putvalue(CurrentAPC.SetTime.ToString("yyyyMMddHHmmssff"));

                                arrParamName = CurrentAPC.ParameterName;
                                arrParamValue = CurrentAPC.ParameterValue;

                                pMsgTran.Primary().Item("L4" + dintCount).Putvalue(CurrentAPC.ParameterName.Length);
                                for (int dintLoop2 = 0; dintLoop2 < CurrentAPC.ParameterName.Length; dintLoop2++)
                                {
                                    pMsgTran.Primary().Item("P_PARM_NAME" + dintCount + dintLoop2).Putvalue(arrParamName[dintLoop2]);
                                    pMsgTran.Primary().Item("P_PARM_VALUE" + dintCount + dintLoop2).Putvalue(arrParamValue[dintLoop2]);
                                }
                                dintCount++;
                            }
                            pInfo.RemoveAPC();
                        }
                        break;

                    case "3":
                        dintBYWHO = 3;
                        pMsgTran.Primary().Item("BYWHO").Putvalue(dintBYWHO);
                        ArrayList arrCon = new ArrayList();
                        pMsgTran.Primary().Item("L2").Putvalue(arrData.Length);
                            for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                            {
                                InfoAct.clsAPC CurrentAPC = this.pInfo.APC(arrData[dintLoop]);

                                pMsgTran.Primary().Item("H_GLASSID" + dintLoop).Putvalue(CurrentAPC.GLSID);
                                pMsgTran.Primary().Item("JOBID" + dintLoop).Putvalue(CurrentAPC.JOBID);
                                pMsgTran.Primary().Item("RECIPE" + dintLoop).Putvalue(CurrentAPC.EQPPPID);
                                pMsgTran.Primary().Item("SET_TIME" + dintLoop).Putvalue(CurrentAPC.SetTime.ToString("yyyyMMddHHmmssff"));

                                arrParamName = CurrentAPC.ParameterName;
                                arrParamValue = CurrentAPC.ParameterValue;
                                pMsgTran.Primary().Item("L4" + dintLoop).Putvalue(CurrentAPC.ParameterName.Length);
                                for (int dintLoop2 = 0; dintLoop2 < CurrentAPC.ParameterName.Length; dintLoop2++)
                                {
                                    pMsgTran.Primary().Item("P_PARM_NAME" + dintLoop + dintLoop2).Putvalue(arrParamName[dintLoop2]);
                                    pMsgTran.Primary().Item("P_PARM_VALUE" + dintLoop + dintLoop2).Putvalue(arrParamValue[dintLoop2]);
                                }
                                arrCon.Add(CurrentAPC.GLSID);
                            }

                            for (int dintLoop = 0; dintLoop < arrCon.Count; dintLoop++)
                            {
                                pInfo.RemoveAPC(arrCon[dintLoop].ToString());
                            }
                        break;
                }
                this.pInfo.All.APCDBUpdateCheck = true;
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
