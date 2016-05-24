using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsSubUnitMethod
    {
        private Hashtable pHashtableGLSAPD = new Hashtable();
        private Hashtable pHashtableCurrAlarm = new Hashtable();
        private Hashtable pHOSTPPIDHash = new Hashtable();
        private Hashtable pEQPPPIDHash = new Hashtable();
        private Hashtable pPPIDBodyHash = new Hashtable();
        private Hashtable pAlarmHash = new Hashtable();
        private Hashtable pHashtableCurrGLS = new Hashtable();
        private Hashtable pHashtableLOTAPD = new Hashtable();
        private Hashtable pHashtableECID = new Hashtable();
        private Hashtable pHashtableSVID = new Hashtable();
        private Hashtable pHashtableEOID = new Hashtable();
        private Hashtable pHashtablePMCode = new Hashtable();
        private Hashtable pHashtableMaterial = new Hashtable();
        private Hashtable pHashtableTRID = new Hashtable();
        private Hashtable pHashtableUserLevel = new Hashtable();
        private Hashtable pHashtableUser = new Hashtable();
        //private Hashtable pHashtableMCC = new Hashtable();
        private Hashtable pHashtableMCCList = new Hashtable();
        private Hashtable pHashMCCInfo = new Hashtable();

        public Hashtable pHashPPIDBodyName_GetIndex = new Hashtable();

        private Hashtable pMappingEQPPPIDHash = new Hashtable();
        //private bool EQPPPID_UpDate_Flag = false;
        //private bool HOSTPPID_UpDate_Flag = false;
        public int GLSAPDReportCount = 0;

        #region "Process Step"
        private Hashtable pHashProcessStep = new Hashtable();

        public bool AddProcessStep(string strStepNo)
        {
            try
            {
                string dstrStepNo = strStepNo.Trim();
                int dintStepNo;

                if (string.IsNullOrWhiteSpace(dstrStepNo)) return false;
                if (!int.TryParse(dstrStepNo, out dintStepNo)) return false;

                if (pHashProcessStep.Contains(dstrStepNo)) pHashProcessStep.Remove(dstrStepNo);

                clsProcessStep procStep = new clsProcessStep(dstrStepNo);

                pHashProcessStep.Add(dstrStepNo, procStep);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public clsProcessStep ProcessStep(string strStepNo)
        {
            try
            {
                string dstrStepNo = strStepNo.Trim();

                if (pHashProcessStep.Contains(dstrStepNo)) return (clsProcessStep)pHashProcessStep[dstrStepNo];
                else return null;
            }
            catch
            {
                return null;
            }
        }

        public clsProcessStep ProcessStep(int intStepNo)
        {
            try
            {
                string dstrStepNo = intStepNo.ToString();

                if (string.IsNullOrWhiteSpace(dstrStepNo)) return null;

                if (pHashProcessStep.Contains(dstrStepNo)) return (clsProcessStep)pHashProcessStep[dstrStepNo];
                else return null;
            }
            catch
            {
                return null;
            }
        }

        public ICollection ProcessStep()
        {
            try
            {
                return pHashProcessStep.Keys;
            }
            catch
            {
                return null;
            }
        }

        public ICollection ProcessStepValues()
        {
            try
            {
                return pHashProcessStep.Values;
            }
            catch
            {
                return null;
            }
        }

        public bool RemoveProcessStep(string strStepNo)
        {
            try
            {
                string dstrStepNo = strStepNo.Trim();

                if (pHashProcessStep.Contains(dstrStepNo))
                {
                    pHashProcessStep.Remove(dstrStepNo);
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveProcessStep()
        {
            try
            {
                pHashProcessStep.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int ProcessStepCount
        {
            get { return pHashProcessStep.Count; }
        }


        #endregion

        //GLSAPD 개체등록 및 가져오기(APD에 대한 기준정보를 가지고 있음)
        #region "GLSAPD"

        public bool AddGLSAPD(int intIndex)
        {
            clsGLSAPD dclsGLSAPD;     //GLSAPD 개체 선언

            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableGLSAPD.Contains(intIndex))
                {
                    return true;
                }

                dclsGLSAPD = new clsGLSAPD(intIndex);
                pHashtableGLSAPD.Add(intIndex, dclsGLSAPD);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsGLSAPD GLSAPD(int intIndex)
        {
            try
            {
                if (pHashtableGLSAPD.Contains(intIndex))
                {
                    return (clsGLSAPD)pHashtableGLSAPD[intIndex];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection GLSAPD()
        {
            try
            {
                return pHashtableGLSAPD.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveGLSAPD(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableGLSAPD.Contains(intIndex))
                {
                    pHashtableGLSAPD.Remove(intIndex);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveGLSAPD()
        {
            try
            {
                pHashtableGLSAPD.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int GLSAPDCount
        {
            get { return pHashtableGLSAPD.Count; }
        }


        #endregion

        //Alarm 개체등록 및 가져오기
        #region "Alarm"

        public bool AddAlarm(int intAlarmID)
        {
            clsAlarm dclsAlarm;

            try
            {
                if (intAlarmID < 0)
                {
                    return false;
                }

                if (pAlarmHash.Contains(intAlarmID))
                {
                    return true;
                }

                dclsAlarm = new clsAlarm(intAlarmID);
                pAlarmHash.Add(intAlarmID, dclsAlarm);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsAlarm Alarm(int intAlarmID)
        {
            try
            {
                if (pAlarmHash.Contains(intAlarmID))
                {
                    return (clsAlarm)pAlarmHash[intAlarmID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection Alarm()
        {
            try
            {
                return pAlarmHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveAlarm(int intAlarmID)
        {
            try
            {
                if (intAlarmID < 0)
                {
                    return false;
                }

                if (pAlarmHash.Contains(intAlarmID))
                {
                    pAlarmHash.Remove(intAlarmID);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveAlarm()
        {
            try
            {
                pAlarmHash.Clear();        //HashTable에서 모든 Alarm을 제거한다.
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int AlarmCount
        {
            get { return pAlarmHash.Count; }
        }

        #endregion

        //HOSTPPID 개체등록 및 가져오기
        #region "HOSTPPID"

        public bool AddHOSTPPID(string strHOSTPPID)
        {
            clsHOSTPPID dclsHOSTPPID;     //LOT 媛쒖껜 ?좎뼵

            try
            {
                string dstrHOSTPPID = strHOSTPPID.Trim();

                if (dstrHOSTPPID == "" || dstrHOSTPPID == null)
                {
                    return false;
                }

                if (pHOSTPPIDHash.Contains(dstrHOSTPPID))
                {
                    return false;
                }

                dclsHOSTPPID = new clsHOSTPPID(dstrHOSTPPID);
                pHOSTPPIDHash.Add(dstrHOSTPPID, dclsHOSTPPID);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }

        }

        public clsHOSTPPID HOSTPPID(string strRecipeID)
        {
            try
            {
                string dstrRecipeID = strRecipeID.Trim();

                if (pHOSTPPIDHash.Contains(dstrRecipeID))
                {
                    return (clsHOSTPPID)pHOSTPPIDHash[dstrRecipeID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection HOSTPPID()
        {
            try
            {
                return pHOSTPPIDHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection HOSTPPIDvalues()
        {
            try
            {
                return pHOSTPPIDHash.Values;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveHOSTPPID(string strRecipeID)
        {
            try
            {
                string dstrRecipeID = strRecipeID.Trim();

                if (dstrRecipeID == "" || dstrRecipeID == null)
                {
                    return false;
                }

                if (pHOSTPPIDHash.Contains(dstrRecipeID))
                {
                    pHOSTPPIDHash.Remove(dstrRecipeID);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveHOSTPPID()
        {
            try
            {
                pHOSTPPIDHash.Clear();        //HashTable에서 모든 Recipe를 제거한다.
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int HOSTPPIDCount
        {
            get { return pHOSTPPIDHash.Count; }
        }

        #endregion

        //EQPPPID 개체등록 및 가져오기
        #region "HostPPID Mapping EQPPPID"

        public bool AddMappingEQPPPID(string strEQPPPID)
        {
            clsMappingEQPPPID dclsEQPPPID;     //LOT 媛쒖껜 ?좎뼵

            try
            {
                string dstrEQPPPID = strEQPPPID.Trim();

                if (dstrEQPPPID == "" || dstrEQPPPID == null)
                {
                    return false;
                }

                //if (Convert.ToInt32(dstrEQPPPID) >= 1 || Convert.ToInt32(dstrEQPPPID) <= 99)
                //{
                //}
                //else
                //{
                //    return false;
                //}

                if (pMappingEQPPPIDHash.Contains(dstrEQPPPID))
                {
                    return true;
                }

                dclsEQPPPID = new clsMappingEQPPPID(strEQPPPID);
                pMappingEQPPPIDHash.Add(dstrEQPPPID, dclsEQPPPID);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }

        }

        public clsMappingEQPPPID MappingEQPPPID(string strEQPPPID)
        {
            try
            {
                string dstrEQPPPID = strEQPPPID.Trim();

                if (pMappingEQPPPIDHash.Contains(dstrEQPPPID))
                {
                    return (clsMappingEQPPPID)pMappingEQPPPIDHash[dstrEQPPPID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection MappingEQPPPID()
        {
            try
            {
                return pMappingEQPPPIDHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveMappingEQPPPID(string strEQPPPID)
        {
            try
            {
                string dstrEQPPPID = strEQPPPID.Trim();

                if (string.IsNullOrEmpty(dstrEQPPPID))
                {
                    return false;
                }

                if (pMappingEQPPPIDHash.Contains(dstrEQPPPID))
                {
                    pMappingEQPPPIDHash.Remove(dstrEQPPPID);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveMappingEQPPPID()
        {
            try
            {
                pMappingEQPPPIDHash.Clear();        //HashTable에서 모든 Recipe를 제거한다.
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int MappingEQPPPIDCount
        {
            get { return pMappingEQPPPIDHash.Count; }
        }

        #endregion

        //EQPPPID 개체등록 및 가져오기
        #region "EQPPPID"

        public bool AddEQPPPID(string strEQPPPID)
        {
            clsEQPPPID dclsEQPPPID;     //LOT 媛쒖껜 ?좎뼵

            try
            {
                string dstrEQPPPID = strEQPPPID.Trim();

                if (dstrEQPPPID == "" || dstrEQPPPID == null)
                {
                    return false;
                }

                //if (Convert.ToInt32(dstrEQPPPID) >= 1 || Convert.ToInt32(dstrEQPPPID) <= 99)
                //{
                //}
                //else
                //{
                //    return false;
                //}

                if (pEQPPPIDHash.Contains(dstrEQPPPID))
                {
                    return true;
                }

                dclsEQPPPID = new clsEQPPPID(dstrEQPPPID);
                pEQPPPIDHash.Add(dstrEQPPPID, dclsEQPPPID);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }

        }

        public clsEQPPPID EQPPPID(string strEQPPPID)
        {
            try
            {
                string dstrEQPPPID = strEQPPPID.Trim();

                if (pEQPPPIDHash.Contains(dstrEQPPPID))
                {
                    return (clsEQPPPID)pEQPPPIDHash[dstrEQPPPID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection EQPPPID()
        {
            try
            {
                return pEQPPPIDHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveEQPPPID(string strEQPPPID)
        {
            try
            {
                string dstrEQPPPID = strEQPPPID.Trim();

                if (dstrEQPPPID == "" || dstrEQPPPID == null)
                {
                    return false;
                }

                //if (Convert.ToInt32(strEQPPPID) >= 1 || Convert.ToInt32(strEQPPPID) <= 99)
                //{
                //}
                //else
                //{
                //    return false;
                //}

                if (pEQPPPIDHash.Contains(dstrEQPPPID))
                {
                    pEQPPPIDHash.Remove(dstrEQPPPID);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveEQPPPID()
        {
            try
            {
                pEQPPPIDHash.Clear();        //HashTable에서 모든 Recipe를 제거한다.
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int EQPPPIDCount
        {
            get { return pEQPPPIDHash.Count; }
        }

        #endregion

        //PPIDBody 개체등록 및 가져오기(PPIDBody에 대한 기준정보를 가지고 있음)
        #region "PPIDBody"

        public bool AddPPIDBody(int intIndex)
        {
            clsPPIDBody dclsPPIDBody;

            try
            {
                if (intIndex < 0)
                {
                    return false;
                }

                if (pPPIDBodyHash.Contains(intIndex))
                {
                    return true;
                }

                dclsPPIDBody = new clsPPIDBody(intIndex);
                pPPIDBodyHash.Add(intIndex, dclsPPIDBody);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsPPIDBody PPIDBody(int intIndex)
        {
            try
            {
                if (pPPIDBodyHash.Contains(intIndex))
                {
                    return (clsPPIDBody)pPPIDBodyHash[intIndex];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection PPIDBodyValues()
        {
            try
            {
                return pPPIDBodyHash.Values;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection PPIDBody()
        {
            try
            {
                return pPPIDBodyHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemovePPIDBody(int intIndex)
        {
            try
            {
                if (intIndex < 0)
                {
                    return false;
                }

                if (pPPIDBodyHash.Contains(intIndex))
                {
                    pPPIDBodyHash.Remove(intIndex);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemovePPIDBody()
        {
            try
            {
                pPPIDBodyHash.Clear();        //HashTable에서 모든 Alarm을 제거한다.
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int PPIDBodyCount
        {
            get { return pPPIDBodyHash.Count; }
        }


        #endregion

        //CurrAlarm 개체등록 및 가져오기
        #region "CurrAlarm"

        public bool AddCurrAlarm(int intAlarmID)
        {
            clsAlarm dclsCurrAlarm;     //CurrAlarmInfo 개체 선언

            try
            {
                if (intAlarmID < 0)
                {
                    return false;
                }

                if (pHashtableCurrAlarm.Contains(intAlarmID))
                {
                    pHashtableCurrAlarm.Remove(intAlarmID);    //현재 Unit에 이미 발생한 AlarmID가 있으면 그것을 제거하고 추가한다.
                    //return false;
                }

                dclsCurrAlarm = new clsAlarm(intAlarmID);
                pHashtableCurrAlarm.Add(intAlarmID, dclsCurrAlarm);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsAlarm CurrAlarm(int intAlarmID)
        {
            try
            {
                if (pHashtableCurrAlarm.Contains(intAlarmID))
                {
                    return (clsAlarm)pHashtableCurrAlarm[intAlarmID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection CurrAlarm()
        {
            try
            {
                return pHashtableCurrAlarm.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveCurrAlarm(int intAlarmID)
        {
            try
            {
                if (intAlarmID < 0)
                {
                    return false;
                }
                else
                {
                    if (pHashtableCurrAlarm.Contains(intAlarmID))
                    {
                        pHashtableCurrAlarm.Remove(intAlarmID);     //HashTable에서 해당 Alarm을 제거한다.
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }


        public bool RemoveCurrAlarm()
        {
            try
            {
                pHashtableCurrAlarm.Clear();        //HashTable에서 모든 Alarm을 제거한다.
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int CurrAlarmCount
        {
            get { return pHashtableCurrAlarm.Count; }
        }


        #endregion

        //CurrGLS 개체등록 및 가져오기
        #region "CurrGLS"

        public bool AddCurrGLS(string strGLSID)
        {
            clsGLS dclsGLS;     //clsGLS 개체 선언

            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (dstrGLSID == "" || dstrGLSID == null)
                {
                    return false;
                }

                if (pHashtableCurrGLS.Contains(dstrGLSID))
                {
                    pHashtableCurrGLS.Remove(dstrGLSID);
                }

                dclsGLS = new clsGLS(dstrGLSID);
                pHashtableCurrGLS.Add(dstrGLSID, dclsGLS);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsGLS CurrGLS(string strGLSID)
        {
            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (pHashtableCurrGLS.Contains(dstrGLSID))
                {
                    return (clsGLS)pHashtableCurrGLS[dstrGLSID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection CurrGLS()
        {
            try
            {
                return pHashtableCurrGLS.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection CurrGLSValues()
        {
            try
            {
                return pHashtableCurrGLS.Values;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveCurrGLS(string strGLSID)
        {
            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (dstrGLSID == "" || dstrGLSID == null)
                {
                    return false;
                }

                if (pHashtableCurrGLS.Contains(dstrGLSID))
                {
                    pHashtableCurrGLS.Remove(dstrGLSID);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveCurrGLS()
        {
            try
            {
                pHashtableCurrGLS.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int CurrGLSCount
        {
            get { return pHashtableCurrGLS.Count; }
        }

        #endregion

        //LOTAPD 개체등록 및 가져오기(장비전체(UnitID=0) 밑에 LOTAPD 생성(LOT정보 생성시 LOTInfo에 이 값들을 넣어준다.)
        #region "LOTAPD"

        public bool AddLOTAPD(int intIndex)
        {
            clsLOTAPD dclsLOTAPD;     //LOTAPD 개체 선언

            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableLOTAPD.Contains(intIndex))
                {
                    return false;                                   //LOT이 중복되었을 경우 조치 강구(같은 LOT이 존재하면 안됨)
                }

                dclsLOTAPD = new clsLOTAPD(intIndex);
                pHashtableLOTAPD.Add(intIndex, dclsLOTAPD);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsLOTAPD LOTAPD(int intIndex)
        {
            try
            {
                if (pHashtableLOTAPD.Contains(intIndex))
                {
                    return (clsLOTAPD)pHashtableLOTAPD[intIndex];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection LOTAPD()
        {
            try
            {
                return pHashtableLOTAPD.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveLOTAPD(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableLOTAPD.Contains(intIndex))
                {
                    pHashtableLOTAPD.Remove(intIndex);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveLOTAPD()
        {
            try
            {
                pHashtableLOTAPD.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int LOTAPDCount
        {
            get { return pHashtableLOTAPD.Count; }
        }


        #endregion

        //ECID 개체등록 및 가져오기(ECID에 대한 기준정보를 가지고 있음)
        #region "ECID"

        public bool AddECID(int intIndex)
        {
            clsECID dclsECID;     //ECID 개체 선언

            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableECID.Contains(intIndex))
                {
                    return true;
                }

                dclsECID = new clsECID(intIndex);
                pHashtableECID.Add(intIndex, dclsECID);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsECID ECID(int intIndex)
        {
            try
            {
                if (pHashtableECID.Contains(intIndex))
                {
                    return (clsECID)pHashtableECID[intIndex];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection ECID()
        {
            try
            {
                return pHashtableECID.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveECID(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableECID.Contains(intIndex))
                {
                    pHashtableECID.Remove(intIndex);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveECID()
        {
            try
            {
                pHashtableECID.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int ECIDCount
        {
            get { return pHashtableECID.Count; }
        }


        #endregion

        //SVID 개체등록 및 가져오기(SVID에 대한 기준정보를 가지고 있음)
        #region "SVID"

        public bool AddSVID(int intIndex)
        {
            clsSVID dclsSVID;     //ECID 개체 선언

            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableSVID.Contains(intIndex))
                {
                    return true;
                }

                dclsSVID = new clsSVID(intIndex);
                pHashtableSVID.Add(intIndex, dclsSVID);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsSVID SVID(int intIndex)
        {
            try
            {
                if (pHashtableSVID.Contains(intIndex))
                {
                    return (clsSVID)pHashtableSVID[intIndex];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection SVID()
        {
            try
            {
                return pHashtableSVID.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveSVID(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableSVID.Contains(intIndex))
                {
                    pHashtableSVID.Remove(intIndex);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveSVID()
        {
            try
            {
                pHashtableSVID.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int SVIDCount
        {
            get { return pHashtableSVID.Count; }
        }


        #endregion

        //EOID 개체등록 및 가져오기(EOID 값 저장)
        #region "EOID"

        public bool AddEOID(int intIndex)
        {
            clsEOID dclsEOID;     //ECID 개체 선언

            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableEOID.Contains(intIndex))
                {
                    return true;
                }

                dclsEOID = new clsEOID(intIndex);
                pHashtableEOID.Add(intIndex, dclsEOID);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsEOID EOID(int intIndex)
        {
            try
            {
                if (pHashtableEOID.Contains(intIndex))
                {
                    return (clsEOID)pHashtableEOID[intIndex];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection EOID()
        {
            try
            {
                return pHashtableEOID.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveEOID(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableEOID.Contains(intIndex))
                {
                    pHashtableEOID.Remove(intIndex);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveEOID()
        {
            try
            {
                pHashtableEOID.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int EOIDCount
        {
            get { return pHashtableEOID.Count; }
        }


        #endregion

        //PMCode 개체등록 및 가져오기(PMCode 값 저장)
        #region "PMCode"

        public bool AddPMCode(int intIndex)
        {
            clsPMCode dclsPMCode;     //ECID 개체 선언

            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtablePMCode.Contains(intIndex))
                {
                    return true;
                }

                dclsPMCode = new clsPMCode(intIndex);
                pHashtablePMCode.Add(intIndex, dclsPMCode);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsPMCode PMCode(int intIndex)
        {
            try
            {
                if (pHashtablePMCode.Contains(intIndex))
                {
                    return (clsPMCode)pHashtablePMCode[intIndex];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection PMCode()
        {
            try
            {
                return pHashtablePMCode.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemovePMCode(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtablePMCode.Contains(intIndex))
                {
                    pHashtablePMCode.Remove(intIndex);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemovePMCode()
        {
            try
            {
                pHashtablePMCode.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int PMCodeCount
        {
            get { return pHashtablePMCode.Count; }
        }


        #endregion

        //Material 개체등록 및 가져오기(Material 값 저장)
        #region "Material"

        public bool AddMaterial(int intIndex)
        {
            clsMaterial dclsMaterial;     //ECID 개체 선언

            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableMaterial.Contains(intIndex))
                {
                    return true;
                }

                dclsMaterial = new clsMaterial(intIndex);
                pHashtableMaterial.Add(intIndex, dclsMaterial);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsMaterial Material(int intIndex)
        {
            try
            {
                if (pHashtableMaterial.Contains(intIndex))
                {
                    return (clsMaterial)pHashtableMaterial[intIndex];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection Material()
        {
            try
            {
                return pHashtableMaterial.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveMaterial(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableMaterial.Contains(intIndex))
                {
                    pHashtableMaterial.Remove(intIndex);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveMaterial()
        {
            try
            {
                pHashtableMaterial.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int MaterialCount
        {
            get { return pHashtableMaterial.Count; }
        }


        #endregion

        //TRID 개체등록 및 가져오기(TRID 값 저장)
        #region "TRID"

        public bool AddTRID(int intTRID)
        {
            clsTRID dclsTRID;     //ECID 개체 선언

            try
            {
                if (intTRID < 0)
                {
                    return false;
                }

                if (pHashtableTRID.Contains(intTRID))
                {
                    return true;
                }

                dclsTRID = new clsTRID(intTRID);
                pHashtableTRID.Add(intTRID, dclsTRID);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsTRID TRID(int intTRID)
        {
            try
            {
                if (pHashtableTRID.Contains(intTRID))
                {
                    return (clsTRID)pHashtableTRID[intTRID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection TRID()
        {
            try
            {
                return pHashtableTRID.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveTRID(int intTRID)
        {
            try
            {
                if (intTRID < 0)
                {
                    return false;
                }

                if (pHashtableTRID.Contains(intTRID))
                {
                    pHashtableTRID.Remove(intTRID);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveTRID()
        {
            try
            {
                pHashtableTRID.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int TRIDCount
        {
            get { return pHashtableTRID.Count; }
        }


        #endregion

        //UserLevel 개체등록 및 가져오기(UserLevel에 대한 기준정보를 가지고 있음)
        #region "UserLevel"

        public bool AddUserLevel(int intUserLevel)
        {
            clsUserLevel dclsUserLevel;     //ECID 개체 선언

            try
            {
                if (intUserLevel < 0)
                {
                    return false;
                }

                if (pHashtableUserLevel.Contains(intUserLevel))
                {
                    return true;
                }

                dclsUserLevel = new clsUserLevel(intUserLevel);
                pHashtableUserLevel.Add(intUserLevel, dclsUserLevel);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsUserLevel UserLevel(int intUserLevel)
        {
            try
            {
                if (pHashtableUserLevel.Contains(intUserLevel))
                {
                    return (clsUserLevel)pHashtableUserLevel[intUserLevel];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection UserLevel()
        {
            try
            {
                return pHashtableUserLevel.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveUserLevel(int intUserLevel)
        {
            try
            {
                if (intUserLevel < 0)
                {
                    return false;
                }

                if (pHashtableUserLevel.Contains(intUserLevel))
                {
                    pHashtableUserLevel.Remove(intUserLevel);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveUserLevel()
        {
            try
            {
                pHashtableUserLevel.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int UserLevelCount
        {
            get { return pHashtableUserLevel.Count; }
        }


        #endregion

        //User 개체등록 및 가져오기
        #region "User"

        public bool AddUser(string strUserID)
        {
            clsUser dclsUser;

            try
            {
                string dstrUserID = strUserID.Trim();

                if (dstrUserID == "" || dstrUserID == null)
                {
                    return false;
                }

                if (pHashtableUser.Contains(dstrUserID))
                {
                    return false;
                }

                dclsUser = new clsUser(dstrUserID);
                pHashtableUser.Add(dstrUserID, dclsUser);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }

        }

        public clsUser User(string strUserID)
        {
            try
            {
                string dstrstrUserID = strUserID.Trim();

                if (pHashtableUser.Contains(dstrstrUserID))
                {
                    return (clsUser)pHashtableUser[dstrstrUserID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection User()
        {
            try
            {
                return pHashtableUser.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveUser(string strUserID)
        {
            try
            {
                string dstrstrUserID = strUserID.Trim();

                if (dstrstrUserID == "" || dstrstrUserID == null)
                {
                    return false;
                }

                if (pHashtableUser.Contains(dstrstrUserID))
                {
                    pHashtableUser.Remove(dstrstrUserID);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveUser()
        {
            try
            {
                pHashtableUser.Clear();        //HashTable에서 모든 Recipe를 제거한다.
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int UserCount
        {
            get { return pHashtableUser.Count; }
        }

        #endregion

        //MCC Information
        #region "MCC Info"

        public bool AddMCCInfo(int intIndex)
        {
            clsMCC dclsMCC;
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (this.pHashMCCInfo.Contains(intIndex))
                {
                    return true;
                }

                dclsMCC = new clsMCC(intIndex);
                pHashMCCInfo.Add(intIndex, dclsMCC);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsMCC MCCInfo(int intIndex)
        {
            try
            {
                if (pHashMCCInfo.Contains(intIndex))
                {
                    return (clsMCC)pHashMCCInfo[intIndex];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection MCCInfo()
        {
            try
            {
                return pHashMCCInfo.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveMCCInfo(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashMCCInfo.Contains(intIndex))
                {
                    pHashMCCInfo.Remove(intIndex);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveMCCInfo()
        {
            try
            {
                pHashMCCInfo.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int MCCInfoCount
        {
            get { return pHashMCCInfo.Count; }
        }

        public int MCCInfoReadCount
        {
            get
            {
                int dintCount = 0;
                lock (new object())
                {
                    foreach (int dintIndex in pHashMCCInfo.Keys)
                    {
                        clsMCC dclsMCC = (clsMCC)pHashMCCInfo[dintIndex];
                        if (dclsMCC.PLCReadFlag)
                        {
                            dintCount++;
                        }
                    }
                }

                return dintCount;
            }
        }

        #endregion

        public delegate void EQPPPID_Update_Event();
        public event EQPPPID_Update_Event Event_EQPPPID_Update;
        public void EQPPPID_UPDATE()
        {
            if (Event_EQPPPID_Update != null) Event_EQPPPID_Update();
        }

        public delegate void HOSTPPID_Update_Event();
        public event HOSTPPID_Update_Event Event_HOSTPPID_Update;
        public void HOSTPPID_UPDATE()
        {
            if (Event_HOSTPPID_Update != null) Event_HOSTPPID_Update();
        }

    }
 
}
