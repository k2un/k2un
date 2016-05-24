using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSECS;
using CommonAct;
using InfoAct;

namespace HostAct
{
    class clsS16F103: clsStreamFunction, IStreamFunction
    {
        #region Fields
        System.Collections.Hashtable hashPPIDbody = new System.Collections.Hashtable();
        #endregion

        #region Properties
        #endregion
        
        #region Singleton
        public static readonly clsS16F103 Instance = new clsS16F103();
        #endregion

        #region Constructors
        public clsS16F103()
        {
			this.intStream = 16;
            this.intFunction = 103;
            this.strPrimaryMsgName = "S16F103";
            this.strSecondaryMsgName = "S16F104";
        }
        #endregion
        

        #region Methods
        private void subSetHashtable()
        {
            try
            {
                hashPPIDbody.Clear();

                foreach (InfoAct.clsPPIDBody tmpPPIDbody in pInfo.Unit(0).SubUnit(0).PPIDBodyValues())
                {
                    hashPPIDbody.Add(tmpPPIDbody.Name, tmpPPIDbody);
                }
            }
			catch (Exception error)
			{
				funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
				return;
			}
        }

        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrModuleID = string.Empty;
            int dintGLSCNT = 0;
            int dintTCACK = 0;


            InfoAct.clsAPC[] dclsReceivedAPC = null;
            InfoAct.clsAPC[] dclsBackupedAPC = null;


            bool dbolPPIDsynchronized = false;

            DateTime dtSetTime = DateTime.Now;

            int dintCreateCNT = 0;
            int dintOverrideCNT = 0;

            string strAPCData = "";

			try
            {
                if(this.hashPPIDbody.Count != pInfo.Unit(0).SubUnit(0).PPIDBodyCount) subSetHashtable();


                #region "S16F103 수신 및 Reply"
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);

                if (pInfo.All.APCUSE == false) dintTCACK = 1;
                else if(!pInfo.Unit(3).SubUnit(0).ModuleID.Equals(dstrModuleID)) dintTCACK = 3;
                //else
                //{
                    dintGLSCNT = Convert.ToInt32(dstrModuleID = msgTran.Primary().Item("GLSCNT").Getvalue().ToString().Trim());
                    
                    msgTran.Secondary().Item("GLSCNT").Putvalue(dintGLSCNT);

                    dclsReceivedAPC = new InfoAct.clsAPC[dintGLSCNT];
                    dclsBackupedAPC = new InfoAct.clsAPC[dintGLSCNT];


                    for (int dintLoop = 0; dintLoop < dintGLSCNT; dintLoop++)
                    {
                        InfoAct.clsAPC tempAPC = new InfoAct.clsAPC(msgTran.Primary().Item("H_GLASSID"+dintLoop).Getvalue().ToString().Trim());
                        tempAPC.SetTime = dtSetTime.AddMilliseconds(dintLoop *10);
                        tempAPC.JOBID = msgTran.Primary().Item("JOBID" + dintLoop).Getvalue().ToString().Trim();
                        tempAPC.EQPPPID = msgTran.Primary().Item("RECIPE" + dintLoop).Getvalue().ToString().Trim();

                        msgTran.Secondary().Item("H_GLASSID" + dintLoop).Putvalue(tempAPC.GLSID);
                        msgTran.Secondary().Item("JOBID" + dintLoop).Putvalue(tempAPC.JOBID);
                        msgTran.Secondary().Item("RECIPE" + dintLoop).Putvalue(tempAPC.EQPPPID);
                        msgTran.Secondary().Item("SET_TIME" + dintLoop).Putvalue(tempAPC.SetTime.ToString("yyyyMMddHHmmssff"));


                        #region "EQP PPID Check"
                        if (!dbolPPIDsynchronized && this.pInfo.Unit(0).SubUnit(0).EQPPPID(tempAPC.EQPPPID) == null)
                        {
                            //pInfo.All.isReceivedFromHOST = true;
                            //pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID, 1);
                            //pHost.subWaitDuringReadFromPLC();
                            //pInfo.All.isReceivedFromHOST = false;

                            //dbolPPIDsynchronized = true;
                        }

                        if (this.pInfo.Unit(0).SubUnit(0).EQPPPID(tempAPC.EQPPPID) == null)
                        {
                            if (dintTCACK == 0) dintTCACK = 7;
                        }

                        #endregion

                        #region "SetMode Check"
                        if (pInfo.APC(tempAPC.GLSID) != null)
                        {
                            dintCreateCNT++;
                            dclsBackupedAPC[dintLoop] = this.pInfo.APC(tempAPC.GLSID);

                            if (this.pInfo.APC(tempAPC.GLSID).State == "2")
                            {
                                if (dintTCACK == 0) dintTCACK = 5;
                                tempAPC.State = "2";
                            }
                            else
                            {
                                tempAPC.Mode = 4;
                                tempAPC.State = "1";
                            }
                        }
                        else
                        {
                            dintCreateCNT++;
                            dclsBackupedAPC[dintLoop] = null;
                            tempAPC.State = tempAPC.Mode.ToString();
                        }
                        #endregion

                        #region "Parameter Check"
                        int dintParamCNT = Convert.ToInt32(msgTran.Primary().Item("PARAMCNT" + dintLoop).Getvalue().ToString().Trim());
                        tempAPC.subSetParameterCount(dintParamCNT);

                        msgTran.Secondary().Item("PARAMCNT" + dintLoop).Putvalue(dintParamCNT);

                        for (int dintParamLoop = 0; dintParamLoop < dintParamCNT; dintParamLoop++)
                        {
                            tempAPC.ParameterName[dintParamLoop] = msgTran.Primary().Item("P_PARM_NAME" + dintLoop + dintParamLoop).Getvalue().ToString().Trim();
                            tempAPC.ParameterValue[dintParamLoop] = msgTran.Primary().Item("P_PARM_VALUE" + dintLoop + dintParamLoop).Getvalue().ToString().Trim();


                            tempAPC.PACK_Name[dintParamLoop] = (hashPPIDbody.Contains((tempAPC.ParameterName[dintParamLoop]))) ? 0 : 1;

                            if (tempAPC.PACK_Name[dintParamLoop] == 0)
                            {
                                double tmpValue;

                                if (double.TryParse(tempAPC.ParameterValue[dintParamLoop], out tmpValue))
                                {
                                    InfoAct.clsPPIDBody tmpPPIDbody = (InfoAct.clsPPIDBody)hashPPIDbody[tempAPC.ParameterName[dintParamLoop]];

                                    // ParamIndex 위치가 여기쯤이믄.. 모..
                                    tempAPC.ParameterIndex[dintParamLoop] = tmpPPIDbody.Index.ToString();

                                    tempAPC.PACK_Value[dintParamLoop] = (tmpValue < tmpPPIDbody.Min || tmpValue > tmpPPIDbody.Max) ? 2 : 0;
                                }
                                else tempAPC.PACK_Value[dintParamLoop] = 2;
                            }
                            else tempAPC.PACK_Value[dintParamLoop] = 0;


                            msgTran.Secondary().Item("P_PARM_NAME" + dintLoop + dintParamLoop).Putvalue(tempAPC.ParameterName[dintParamLoop]);
                            msgTran.Secondary().Item("PACK1" + dintLoop + dintParamLoop).Putvalue(tempAPC.PACK_Name[dintParamLoop]);

                            msgTran.Secondary().Item("P_PARM_VALUE" + dintLoop + dintParamLoop).Putvalue(tempAPC.ParameterValue[dintParamLoop]);
                            msgTran.Secondary().Item("PACK2" + dintLoop + dintParamLoop).Putvalue(tempAPC.PACK_Value[dintParamLoop]);


                            if(dintTCACK == 0 && (tempAPC.PACK_Name[dintParamLoop] != 0 || tempAPC.PACK_Value[dintParamLoop] != 0)) dintTCACK = 4;
                        }
                        #endregion

                        dclsReceivedAPC[dintLoop] = tempAPC;

                        strAPCData += tempAPC.Mode + "!" + tempAPC.GLSID + "=";
                    }
                    if (string.IsNullOrEmpty(strAPCData) == false)
                    {
                        strAPCData = strAPCData.Substring(0, strAPCData.Length - 1);
                    }

                //}

                msgTran.Secondary().Item("TCACK").Putvalue(dintTCACK);

                funSendReply(msgTran);

                #endregion
           
                #region "ProcessDataSet 처리"
                if (dintTCACK != 0 ) return;

                //string[] strParams = new string[dclsReceivedAPC.Length * 2];
                for (int dintLoop = 0; dintLoop < dclsReceivedAPC.Length; dintLoop++)
                {
                    // 하나씩.... 처리해야것지? 
                    InfoAct.clsAPC CurrentAPC = dclsReceivedAPC[dintLoop];

                    if (CurrentAPC.Mode == 4)
                    {
                        InfoAct.clsAPC BackupAPC = dclsBackupedAPC[dintLoop];
                        BackupAPC.subParameterValueBackup();
                        InfoAct.clsAPC CurrentAPC2 = pInfo.APC(BackupAPC.GLSID);

                        //CurrentAPC2.ParameterValueBackup = new string[BackupAPC.ParameterValueBackup.Length];
                        //CurrentAPC2.ParameterNameBackup = new string[BackupAPC.ParameterNameBackup.Length];
                        CurrentAPC2.ParameterName = new string[CurrentAPC.ParameterName.Length];
                        CurrentAPC2.ParameterValue = new string[CurrentAPC.ParameterValue.Length];
                        CurrentAPC2.SetTime_old = CurrentAPC2.SetTime;
                        CurrentAPC2.SetTime = CurrentAPC.SetTime;

                        //for (int dintLoop2 = 0; dintLoop2 < CurrentAPC2.ParameterValueBackup.Length; dintLoop2++)
                        //{
                        //    CurrentAPC2.ParameterValueBackup[dintLoop2] = BackupAPC.ParameterValueBackup[dintLoop2];
                        //}
                        //for (int dintLoop2 = 0; dintLoop2 < CurrentAPC2.ParameterNameBackup.Length; dintLoop2++)
                        //{
                        //    CurrentAPC2.ParameterNameBackup[dintLoop2] = BackupAPC.ParameterNameBackup[dintLoop2];
                        //}
                        for (int dintLoop2 = 0; dintLoop2 < CurrentAPC2.ParameterName.Length; dintLoop2++)
                        {
                            CurrentAPC2.ParameterName[dintLoop2] = CurrentAPC.ParameterName[dintLoop2];
                        }
                        for (int dintLoop2 = 0; dintLoop2 < CurrentAPC2.ParameterValue.Length; dintLoop2++)
                        {
                            CurrentAPC2.ParameterValue[dintLoop2] = CurrentAPC.ParameterValue[dintLoop2];
                        }
                        //pInfo.RemoveAPC(CurrentAPC.GLSID);
                    }
                    else
                    {
                        pInfo.AddAPC(CurrentAPC);
                    }

                    //pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataLog, InfoAct.clsInfo.ProcessDataType.APC, CurrentAPC.Mode.ToString(), CurrentAPC.GLSID, dclsBackupedAPC[dintLoop], CurrentAPC);

                    //strParams[dintLoop * 2] = CurrentAPC.Mode.ToString();
                    //strParams[dintLoop * 2 + 1] = CurrentAPC.GLSID;
                    //pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataSet, clsInfo.ProcessDataType.APC, CurrentAPC.Mode.ToString(), CurrentAPC.GLSID, dclsBackupedAPC[dintLoop], CurrentAPC);
                }

                if (string.IsNullOrEmpty(strAPCData) == false)
                {
                    pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataSet, clsInfo.ProcessDataType.APC, "", strAPCData, "", "");
                }

                //pInfo.subSendSF_Set(clsInfo.SFName.S16F115APCDataCMD, strParams);
                #endregion

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
				
				pMsgTran = pSecsDrv.MakeTransaction(this.strPrimaryMsgName);
				
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
