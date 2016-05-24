using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using DBAct;
using CommonAct;
using InfoAct;
using System.Net;

namespace STM
{
    class clsInfoActPlugIn
    {
        private clsInfo PInfo = clsInfo.Instance;

        /// <summary>
        /// 구조체를 초기화한다.
        /// </summary>
        public void subInitialInfo(string strModuleID)
        {
            try
            {
                subEQPInfoInitial(PInfo.All.MODEL_NAME);                                 //INI의 데이타를 읽어 구조체 EQP에 저장한다.
                subUnitInfoInitial(strModuleID);
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Act를 종료시킨다.
        /// </summary>
        public void subClose()
        {

        }

        /// <summary>
        /// UnitInfo 구조체를 초기화 한다.
        /// </summary>
        /// <comment>
        /// DB로부터 데이타를 읽어 저장한다.
        /// </comment>
        private void subUnitInfoInitial(string strModuleID)
        {
            string dstrSQL;
            DataTable dDT;
            DataTable dDT2;
            string dstrName;
            int dintIndex = 0;
            int dintPPIDBodyID = 0;

            int dintUnitID = 0;
            int dintSubUnitID = 0;

            try
            {
                bool bolDBConnect = DBAct.clsDBAct.funConnect(strModuleID);
                bool bolStartProcessUnit = false;

                dstrSQL = "SELECT * FROM tbUnit";
                dDT = DBAct.clsDBAct.funSelectQuery2(dstrSQL);                          //DataTable을 받아온다.
                PInfo.AddDataTable("UNIT", dDT);
                if (dDT != null)
                {
                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintUnitID = Convert.ToInt32(dr["UnitID"]);
                        dintSubUnitID = Convert.ToInt32(dr["SubUnitID"]);

                        if (PInfo.Unit(dintUnitID) == null)
                        {
                            if (PInfo.AddUnit(dintUnitID) == false)
                            {
                                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subUnitInfoInitial: Unit 등록 에러");
                            }
                        }

                        if (PInfo.Unit(dintUnitID).SubUnit(dintSubUnitID) == null)
                        {
                            if (PInfo.Unit(dintUnitID).AddSubUnit(dintSubUnitID) == false)
                            {
                                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subUnitInfoInitial: SubUnit 등록 에러");
                            }
                        }

                        PInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID = dr["ModuleID"].ToString();


                        if (dintUnitID == 1 || dintUnitID == 2)
                        {
                            if (dintSubUnitID != 0)
                            {
                                dintIndex = Convert.ToInt32(dr["Index"]);
                                PInfo.AddPort(dintIndex);
                                PInfo.Port(dintIndex).PortState = "0";
                                PInfo.Port(dintIndex).HostReportPortID = dr["ModuleID"].ToString().Substring(dr["ModuleID"].ToString().Length -4, 4);
                                switch (dr["ModuleID"].ToString().Substring(dr["ModuleID"].ToString().Length - 4, 4))
                                {
                                    case "FI01":
                                    case "FI02":
                                    case "PI01":
                                        PInfo.Port(dintIndex).PortType = "1";
                                        break;

                                    case "FO03":
                                    case "FO04":
                                    case "PO02":
                                        PInfo.Port(dintIndex).PortType = "2";
                                        break;
                                }
                            }
                        }
                    }
                }

                /// <comment>
                /// 기준정보 저장.
                /// </comment>
                # region <기준 정보 저장(UnitID=0)>

                #region "DColl"
                //DB로부터 GLS APD를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbGLSAPD order by Index";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("GLSAPD");
                    PInfo.AddDataTable("GLSAPD", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["Index"]);
                        PInfo.Unit(0).SubUnit(0).AddGLSAPD(dintIndex);

                        PInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).Index = Convert.ToInt32(dr["Index"]);
                        PInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).Name = dr["Name"].ToString();
                        PInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).Length = Convert.ToInt32(dr["Length"]);
                        PInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).Format = dr["Format"].ToString();
                        PInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).HaveMinusValue = Convert.ToBoolean(dr["HaveMinusValue"]);
                        PInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).ModuleID = dr["ModuleID"].ToString();
                        PInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).HostReportFlag = Convert.ToBoolean(dr["HOSTReport"].ToString());
                        //if (PInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).Format.Trim() != "")
                        //{
                            this.PInfo.All.GLSAPDPLCReadLength += this.PInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).Length;
                        //}

                            if (PInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).HostReportFlag)
                            {
                                PInfo.Unit(0).SubUnit(0).GLSAPDReportCount++;
                            }
                    }


                }

                //장비전체(UnitID=0)에 LOTAPD를 추가한다.
                //장비전체(UnitID=0) 밑에 LOTAPD 생성(LOT정보 생성시 생성된 LOT의 LOTAPD에 이 기준정보 값들을 넣어준다.)
                //LOTAPD Data
                dstrSQL = "SELECT * FROM tbLOTAPD order by Index";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("LOTAPD");
                    PInfo.AddDataTable("LOTAPD", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["Index"]);
                        PInfo.Unit(0).SubUnit(0).AddLOTAPD(dintIndex);

                        PInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).Index = Convert.ToInt32(dr["Index"]);
                        PInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).Name = dr["Name"].ToString();
                        PInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).Length = Convert.ToInt32(dr["Length"]);
                        PInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).Format = dr["Format"].ToString();
                        PInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).HaveMinusValue = Convert.ToBoolean(dr["HaveMinusValue"]);
                        PInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).Type = dr["Type"].ToString();
                        //PInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).UnitID = this.PInfo.funGetModuleIDToUnitID(dr["ModuleID"].ToString());
                        PInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).ModuleID = dr["ModuleID"].ToString();
                    }
                }
                #endregion

                #region "SVID"
                //DB로부터 SVID를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbSVID order by SVID";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("SVID");
                    PInfo.AddDataTable("SVID", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["SVID"]);
                        PInfo.Unit(0).SubUnit(0).AddSVID(dintIndex);

                        PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Name = dr["SVNAME"].ToString();
                        PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Length = Convert.ToInt32(dr["Length"].ToString());
                        PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Format = dr["Format"].ToString();
                        PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Value = dr["SV"].ToString();
                        PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Type = dr["Type"].ToString();
                        PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Unit = dr["Unit"].ToString();
                        PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Range = dr["Range"].ToString();
                        PInfo.Unit(0).SubUnit(0).SVID(dintIndex).HaveMinusValue = Convert.ToBoolean(dr["HaveMinusValue"]);
                        PInfo.Unit(0).SubUnit(0).SVID(dintIndex).DESC = dr["Description"].ToString();
                        //PInfo.Unit(0).SubUnit(0).SVID(dintIndex).UnitID = this.PInfo.funGetModuleIDToUnitID(dr["ModuleID"].ToString());
                        PInfo.Unit(0).SubUnit(0).SVID(dintIndex).ModuleID = dr["ModuleID"].ToString();

                        if (PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Format.Trim() != "")
                        {
                            this.PInfo.All.SVIDPLCReadLength = this.PInfo.All.SVIDPLCReadLength + this.PInfo.Unit(0).SubUnit(0).SVID(dintIndex).Length;
                        }
                    }
                }
                #endregion

                #region "EOID"
                //DB로부터 EOID를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbEOID order by Index";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("EOID");
                    PInfo.AddDataTable("EOID", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["Index"]);
                        PInfo.Unit(0).SubUnit(0).AddEOID(dintIndex);

                        PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID = Convert.ToInt32(dr["EOID"]);
                        PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMD = Convert.ToInt32(dr["EOMD"]);
                        PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMDMin = Convert.ToInt32(dr["EOMDMin"]);
                        PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMDMax = Convert.ToInt32(dr["EOMDMax"]);
                        PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV = Convert.ToInt32(dr["EOV"]);
                        PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMin = Convert.ToInt32(dr["EOVMin"]);
                        PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMax = Convert.ToInt32(dr["EOVMax"]);
                        PInfo.Unit(0).SubUnit(0).EOID(dintIndex).DESC = dr["Description"].ToString();
                        PInfo.Unit(0).SubUnit(0).EOID(dintIndex).PLCWrite = Convert.ToBoolean(dr["PLCWrite"]);

                        switch (Convert.ToInt32(dr["EOID"]))
                        {
                            case 17:
                                PInfo.All.MCC_UPLOAD = (dr["EOV"].ToString() == "1")? true : false;
                                break;

                            case 19:
                                PInfo.All.SEM_ON = (dr["EOV"].ToString() == "1") ? true : false;
                                break;

                            case 21:
                                PInfo.All.APCUSE = (dr["EOV"].ToString() == "1") ? true : false;
                                break;

                            case 22:
                                PInfo.All.RPCUSE = (dr["EOV"].ToString() == "1") ? true : false;
                                break;
                        }

                    }

                    this.PInfo.All.HOSTReportEOIDCount = dDT.Rows.Count;               //S1F5(SFCD=1)일 경우 HOST로 보고하는 EOID List 개수

                }
                #endregion

                #region "ECID"
                //DB로부터 ECID를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbECID order by ECID";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("ECID");
                    PInfo.AddDataTable("ECID", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["ECID"]);
                        PInfo.Unit(0).SubUnit(0).AddECID(dintIndex);

                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Name = dr["ECNAME"].ToString();
                        //PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Unit = dr["Unit"].ToString();
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Min = dr["Min"].ToString();
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECSLL = dr["ECSLL"].ToString();
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECWLL = dr["ECWLL"].ToString();
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECDEF = dr["ECDEF"].ToString();
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECWUL = dr["ECWUL"].ToString();
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECSUL = dr["ECSUL"].ToString();
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Max = dr["Max"].ToString();
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Use = Convert.ToBoolean(dr["Use"]);
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).Format = dr["Format"].ToString();
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).DESC = dr["Description"].ToString();
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).UnitID = this.PInfo.funGetModuleIDToUnitID(dr["ModuleID"].ToString());
                        PInfo.Unit(0).SubUnit(0).ECID(dintIndex).ModuleID = dr["ModuleID"].ToString();
                    }
                    this.PInfo.All.HOSTReportECIDCount = dDT.Rows.Count;
                }
                #endregion

                #region "User"
                //DB로부터 UserLevel를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbUserLevel order by UserLevel";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["UserLevel"]);
                        PInfo.Unit(0).SubUnit(0).AddUserLevel(dintIndex);

                        PInfo.Unit(0).SubUnit(0).UserLevel(dintIndex).Desc = dr["Description"].ToString();
                        PInfo.Unit(0).SubUnit(0).UserLevel(dintIndex).Comment = dr["LevelComment"].ToString();
                    }
                }

                //DB로부터 User 정보를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbUser order by UserLevel";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("User");
                    PInfo.AddDataTable("User", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dstrName = dr["UserID"].ToString();
                        PInfo.Unit(0).SubUnit(0).AddUser(dstrName);

                        PInfo.Unit(0).SubUnit(0).User(dstrName).Level = Convert.ToInt32(dr["UserLevel"].ToString());
                        PInfo.Unit(0).SubUnit(0).User(dstrName).PassWord = dr["Pass"].ToString();
                        PInfo.Unit(0).SubUnit(0).User(dstrName).Desc = dr["Description"].ToString();
                    }
                }
                #endregion

                #region "Alarm"
                //Alarm Data
                dstrSQL = "SELECT * FROM tbAlarm order by AlarmID";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("Alarm");
                    PInfo.AddDataTable("Alarm", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        if (Convert.ToBoolean(dr["AlarmReport"].ToString().Trim()) ==false )
                            continue;
                        dintIndex = Convert.ToInt32(dr["AlarmID"]);
                        PInfo.Unit(0).SubUnit(0).AddAlarm(dintIndex);                                         //Recipe Data 추가

                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmCode = Convert.ToInt32(dr["AlarmCD"].ToString().Trim());
                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmType = dr["AlarmType"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmDesc = dr["AlarmDesc"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmReport = Convert.ToBoolean(dr["AlarmReport"].ToString().Trim());
                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).UnitID = this.PInfo.funGetModuleIDToUnitID(dr["ModuleID"].ToString());
                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).ModuleID = dr["ModuleID"].ToString();
                    }
                }
                #endregion

                #region "PPID Body"
                //DB로부터 PPID Body 기준정보를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbBodyInfo order by BodyID";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("PPID");
                    PInfo.AddDataTable("PPID", dDT);
                    
                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["BodyID"]);
                        PInfo.Unit(0).SubUnit(0).AddPPIDBody(dintIndex);

                        PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Name = dr["Name"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Length = Convert.ToInt32(dr["Length"].ToString());
                        PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Min = Convert.ToDouble(dr["Min"].ToString());
                        PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Max = Convert.ToDouble(dr["Max"].ToString());
                        PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Format = dr["Format"].ToString();
                        PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Unit = dr["Unit"].ToString();
                        PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Range = dr["Range"].ToString();
                        PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).DESC = dr["Description"].ToString();
                        PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).ModuleID = dr["ModuleID"].ToString();
                        PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).UseMode = Convert.ToBoolean(dr["USE"].ToString());

                        if (PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Name.Trim().ToUpper() == "RESERVED")
                        {
                            PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Name = "RESERVED" + dintIndex;
                        }

                        PInfo.Unit(0).SubUnit(0).pHashPPIDBodyName_GetIndex.Add(PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Name, dintIndex);
                    }
                }


                #endregion

                #region "DummyPLC"
                //Dummy일 경우 PPID 관련 Test를 위해 DB에 미리 등록된 PPID를 읽어서 하고
                //Real일 경우 DB에 PPID 정보를 저장하지 않고 CIM은 Toss 역활만 해준다.
                if (this.PInfo.EQP("Main").DummyPLC == true)
                {
                    ////DB로부터 HOSTPPID를 읽어들여 저장한다.
                    //dstrSQL = "SELECT * FROM tbHOSTPPID order by HOSTPPID";  //HOST PPID를 추가
                    //dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                    //if (dDT != null)
                    //{
                    //    foreach (DataRow dr in dDT.Rows)
                    //    {
                    //        dstrName = dr["HostPPID"].ToString();
                    //        PInfo.Unit(0).SubUnit(0).AddHOSTPPID(dstrName);                                         //Recipe Data 추가

                    //        //pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrName).UnitID = Convert.ToInt32(dr["UnitID"]);
                    //        PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrName).PPIDVer = dr["PPIDRev"].ToString();
                    //        PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrName).DateTime = dr["DTime"].ToString();


                    //        //HOSTPPID에 해당되는 EQPPPID를 tbPPIDMapping 테이블에서 가져온다.
                    //        dstrSQL = "SELECT * FROM tbPPIDMapping where HOSTPPID='" + dstrName + "'";
                    //        dDT2 = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                    //        if (dDT2 != null)
                    //        {
                    //            foreach (DataRow dr2 in dDT2.Rows)
                    //            {
                    //                PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrName).EQPPPID = dr2["EQPPPID"].ToString();
                    //            }
                    //        }
                    //    }
                    //}

                    ////DB로부터 EQPPPID를 읽어들여 저장한다.
                    //dstrSQL = "SELECT * FROM tbEQPPPID order by EQPPPID";  //EQP PPID를 추가
                    //dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                    //if (dDT != null)
                    //{
                    //    foreach (DataRow dr in dDT.Rows)
                    //    {
                    //        dstrName = dr["EQPPPID"].ToString();
                    //        PInfo.Unit(0).SubUnit(0).AddEQPPPID(dstrName);                                         //Recipe Data 추가

                    //        //pInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).UnitID = Convert.ToInt32(dr["UnitID"]);
                    //        PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDVer = dr["PPIDRev"].ToString();
                    //        PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).DateTime = dr["DTime"].ToString();


                    //        //EQPPPID에 해당되는 Body값을 tbPPIDBody 테이블에서 가져온다.
                    //        dstrSQL = "SELECT * FROM tbPPIDBody where EQPPPID=" + dstrName + " order by BodyID";
                    //        dDT2 = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                    //        if (dDT2 != null)
                    //        {
                    //            foreach (DataRow dr2 in dDT2.Rows)
                    //            {
                    //                dintPPIDBodyID = Convert.ToInt32(dr2["BodyID"]);
                    //                PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).AddPPIDBody(dintPPIDBodyID);

                    //                //기준정보에 있는 PPID Body Name -> 실제 저장할 PPID Body 속성값에 저장
                    //                PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDBody(dintPPIDBodyID).Name = PInfo.Unit(0).SubUnit(0).PPIDBody(dintPPIDBodyID).Name;
                    //                PInfo.Unit(0).SubUnit(0).EQPPPID(dstrName).PPIDBody(dintPPIDBodyID).Value = dr2["Value"].ToString();
                    //            }
                    //        }
                    //    }
                    //}
                }
                #endregion

                #region "xPC"
                string dstrGLSID = "";
                //DB로부터 APC Data를 읽어와서 구조체에 저장한다.
                dstrSQL = "SELECT * FROM tbAPC order by H_GLASSID";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    foreach (DataRow dr in dDT.Rows)
                    {
                        dstrGLSID = dr["H_GLASSID"].ToString();
                        if (PInfo.APC(dstrGLSID) == null) PInfo.AddAPC(dstrGLSID);

                        InfoAct.clsAPC CurrentAPC = PInfo.APC(dstrGLSID);

                        CurrentAPC.JOBID = dr["JOBID"].ToString();
                        CurrentAPC.EQPPPID = dr["RECIPE"].ToString();
                        CurrentAPC.State = dr["APC_STATE"].ToString();
                        CurrentAPC.SetTime = DateTime.ParseExact(dr["SET_TIME"].ToString(), "yyyy-MM-dd HH:mm:ss.ff", null);
                        dstrSQL = "SELECT * FROM `tbAPC_Sub` WHERE `H_GLASSID` = '" + dstrGLSID + "';";

                        dDT2 = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                        foreach (DataRow dr2 in dDT2.Rows)
                        {
                            funAPCSetParam(ref CurrentAPC, dr2["P_PARM_NAME"].ToString(), "NAME");
                            funAPCSetParam(ref CurrentAPC, dr2["P_PARM_VALUE"].ToString(), "VALUE");
                        }

                        CurrentAPC.ParameterIndex = new string[CurrentAPC.ParameterName.Length];
                        for (int dintLoop = 0; dintLoop < CurrentAPC.ParameterName.Length; dintLoop++)  // 20121210 cho young hoon
                        {
                            for (int dintLoop2 = 1; dintLoop2 <= PInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop2++)
                            {
                                if (this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop2).Name == CurrentAPC.ParameterName[dintLoop])
                                {
                                    CurrentAPC.ParameterIndex[dintLoop] = dintLoop2.ToString(); // 피피아이디 바디 인덱스 삽입 
                                }
                            }
                        }
                    }
                }

                ////DB로부터 PPC Data를 읽어와서 구조체에 저장한다.
                dstrSQL = "SELECT * FROM tbPPC order by H_GLASSID";  //PPID를 추가
                if (dDT != null)
                    dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                {
                    foreach (DataRow dr in dDT.Rows)
                    {
                        dstrGLSID = dr["H_GLASSID"].ToString();
                        if (PInfo.PPC(dstrGLSID) == null) PInfo.AddPPC(dstrGLSID);

                        InfoAct.clsPPC CurrentPPC = PInfo.PPC(dstrGLSID);

                        CurrentPPC.JOBID = dr["JOBID"].ToString();
                        CurrentPPC.SetTime = DateTime.ParseExact(dr["SET_TIME"].ToString(), "yyyy-MM-dd HH:mm:ss.ff", null);
                        dstrSQL = "SELECT * FROM `tbPPC_Sub` WHERE `H_GLASSID` = '" + dstrGLSID + "';";

                        dDT2 = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                        foreach (DataRow dr2 in dDT2.Rows)
                        {
                            funPPCSetParam(ref CurrentPPC, dr2["P_MODULEID"].ToString(), "MODULEID");
                            funPPCSetParam(ref CurrentPPC, dr2["P_ORDER"].ToString(), "ORDER");
                            funPPCSetParam(ref CurrentPPC, dr2["P_STATE"].ToString(), "STATE");
                        }
                    }
                }

                ////DB로부터 RPC Data를 읽어와서 구조체에 저장한다.
                dstrSQL = "SELECT * FROM tbRPC order by H_GLASSID";  //PPID를 추가
                if (dDT != null)
                    dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                {
                    foreach (DataRow dr in dDT.Rows)
                    {
                        dstrGLSID = dr["H_GLASSID"].ToString();
                        if (PInfo.RPC(dstrGLSID) == null) PInfo.AddRPC(dstrGLSID);

                        InfoAct.clsRPC CurrentRPC = PInfo.RPC(dstrGLSID);

                        CurrentRPC.JOBID = dr["JOBID"].ToString();
                        CurrentRPC.OriginPPID = dr["ORIGINAL_PPID"].ToString();
                        CurrentRPC.RPC_PPID = dr["RPC_PPID"].ToString();
                        CurrentRPC.SetTime = DateTime.ParseExact(dr["SET_TIME"].ToString(), "yyyy-MM-dd HH:mm:ss.ff", null);
                        CurrentRPC.RPC_STATE = Convert.ToInt32(dr["RPC_STATE"].ToString());


                    }
                }
                #endregion

                #region "Process Step"
                dstrSQL = "SELECT * FROM tbProcessStep order by StepNo";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("ProcessStep");
                    PInfo.AddDataTable("ProcessStep", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        string dstrStepNo = dr["StepNo"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).AddProcessStep(dstrStepNo);

                        PInfo.Unit(0).SubUnit(0).ProcessStep(dstrStepNo).StepDesc = dr["StepDesc"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).ProcessStep(dstrStepNo).StartModuleID = dr["StartModuleID"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).ProcessStep(dstrStepNo).EndModuleID = dr["EndModuleID"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).ProcessStep(dstrStepNo).ProcessEvent = dr["Range"].ToString().Trim();
                    }
                }
                #endregion

                #region "MultiUseData"
                dstrSQL = "SELECT * FROM tbMultiUseData order by Index";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {


                    foreach (DataRow dr in dDT.Rows)
                    {
                        InfoAct.clsMultiUseDataByITEM clsITEM = new InfoAct.clsMultiUseDataByITEM();
                        clsITEM.INDEX = Convert.ToInt32(dr["Index"]);
                        clsITEM.DATA_TYPE = dr["TYPE"].ToString().Trim();
                        clsITEM.DATA_TYPE_NUM = Convert.ToInt32(dr["TYPE_NUM"]);
                        clsITEM.ITEM = dr["ITEM"].ToString().Trim();
                        clsITEM.ITEM_NUM = Convert.ToInt32(dr["ITEM_NUM"]);
                        clsITEM.REFERENCE = dr["REFERENCE"].ToString().Trim();
                        clsITEM.ITEM_NAME_TO_PLC = dr["PLC_NAME"].ToString().Trim();


                        PInfo.Unit(0).SubUnit(0).MultiData.AddTYPE(new InfoAct.clsMultiUseDataByTYPE(clsITEM.DATA_TYPE, clsITEM.DATA_TYPE_NUM));
                        PInfo.Unit(0).SubUnit(0).MultiData.TYPES(clsITEM.DATA_TYPE).AddITEM(clsITEM);
                        PInfo.Unit(0).SubUnit(0).MultiData.TYPES(clsITEM.DATA_TYPE).ITEMS(clsITEM.ITEM).VALUE = dr["ITEM_VALUE"].ToString().Trim();
                    }
                }
                #endregion

                #region "Mapping EQPPPID"
                //DB로부터 PPID Body 기준정보를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbEQPPPID";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("MappingEQPPPID");
                    PInfo.AddDataTable("MappingEQPPPID", dDT);

                    string strEQPPPID = "";
                    foreach (DataRow dr in dDT.Rows)
                    {
                        strEQPPPID = dr["EQPPPID"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).AddMappingEQPPPID(strEQPPPID);

                        PInfo.Unit(0).SubUnit(0).MappingEQPPPID(strEQPPPID).UP_EQPPPID = dr["UP_PPID"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).MappingEQPPPID(strEQPPPID).LOW_EQPPPID = dr["LOW_PPID"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).MappingEQPPPID(strEQPPPID).DateTime = dr["DTIME"].ToString().Trim();
                    }
                }
                #endregion

                #region "HostPPID"
                //DB로부터 PPID Body 기준정보를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbHOSTPPID";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("HOSTPPID");
                    PInfo.AddDataTable("HOSTPPID", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        PInfo.Unit(0).SubUnit(0).AddHOSTPPID(dr["HOSTPPID"].ToString().Trim());

                        PInfo.Unit(0).SubUnit(0).HOSTPPID(dr["HOSTPPID"].ToString().Trim()).EQPPPID = dr["EQPPPID"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).HOSTPPID(dr["HOSTPPID"].ToString().Trim()).DateTime = dr["DTIME"].ToString().Trim();
                    }
                }
                #endregion

                #region "MCC Info"
                //DB로부터 PPID Body 기준정보를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbMCC_I";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("MCC_I");
                    PInfo.AddDataTable("MCC_I", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["index"].ToString());
                        PInfo.Unit(0).SubUnit(0).AddMCCInfo(dintIndex);

                        clsMCC MCCinfo = PInfo.Unit(0).SubUnit(0).MCCInfo(dintIndex);
                        MCCinfo.MCCName = dr["MCCName"].ToString().Trim();
                        MCCinfo.MCCDesc = dr["Description"].ToString().Trim();
                        MCCinfo.MCCOnlyItem = Convert.ToBoolean(dr["OnlyItem"].ToString().Trim());
                        MCCinfo.MCCType = dr["Type"].ToString().Trim();
                        MCCinfo.ModuleID = dr["ModuleID"].ToString().Trim();
                        MCCinfo.ToPosition = dr["ToPosition"].ToString().Trim();
                        MCCinfo.FromPosition = dr["FromPosition"].ToString().Trim();

                        MCCinfo.Unit = dr["Unit"].ToString().Trim();

                        //[2015/04/22] MCC관련 추가(ADD byHS)
                        MCCinfo.PLCReadFlag = Convert.ToBoolean(dr["WordReadFlag"].ToString().Trim());
                        MCCinfo.SVIDIndex = Convert.ToInt32(dr["SVID"].ToString());
                        if (MCCinfo.SVIDIndex > 0)
                        {
                            if (PInfo.Unit(0).SubUnit(0).SVID(MCCinfo.SVIDIndex) == null)
                            {

                            }
                            else
                            {
                                PInfo.Unit(0).SubUnit(0).SVID(MCCinfo.SVIDIndex).MCCInfoIndex = dintIndex;
                            }
                        }
                    }
                }

                #endregion 

                #endregion
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                MessageBox.Show("UclsInfoActPlugIn subUnitInfoInitial중 Error발생!!" + ex.ToString(), "INFORM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                DBAct.clsDBAct.funDisconnect();
            }
        }

        private void funPPCSetParam(ref clsPPC CurrentPPC, string strValue, string strType)
        {
            string[] arrData;
            try
            {
                switch (strType.ToUpper())
                {
                    case "MODULEID":

                        if (CurrentPPC.P_MODULEID == null)
                        {
                            CurrentPPC.P_MODULEID = new string[1];
                            CurrentPPC.P_MODULEID[0] = strValue;
                        }
                        else
                        {
                            arrData = CurrentPPC.P_MODULEID;
                            CurrentPPC.P_MODULEID = new string[CurrentPPC.P_MODULEID.Length + 1];

                            for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                            {
                                CurrentPPC.P_MODULEID[dintLoop] = arrData[dintLoop];
                            }
                            CurrentPPC.P_MODULEID[CurrentPPC.P_MODULEID.Length - 1] = strValue;
                        }
                        break;

                    case "ORDER":
                        if (CurrentPPC.P_ORDER == null)
                        {
                            CurrentPPC.P_ORDER = new string[1];
                            CurrentPPC.P_ORDER[0] = strValue;
                        }
                        else
                        {
                            arrData = CurrentPPC.P_ORDER;
                            CurrentPPC.P_ORDER = new string[CurrentPPC.P_ORDER.Length + 1];

                            for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                            {
                                CurrentPPC.P_ORDER[dintLoop] = arrData[dintLoop];
                            }
                            CurrentPPC.P_ORDER[CurrentPPC.P_ORDER.Length - 1] = strValue;
                        }
                        break;

                    case "STATE":
                        if (CurrentPPC.P_STATUS == null)
                        {
                            CurrentPPC.P_STATUS = new string[1];
                            CurrentPPC.P_STATUS[0] = strValue;
                        }
                        else
                        {
                            arrData = CurrentPPC.P_STATUS;
                            CurrentPPC.P_STATUS = new string[CurrentPPC.P_STATUS.Length + 1];

                            for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                            {
                                CurrentPPC.P_STATUS[dintLoop] = arrData[dintLoop];
                            }
                            CurrentPPC.P_STATUS[CurrentPPC.P_STATUS.Length - 1] = strValue;
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void funAPCSetParam(ref clsAPC CurrentAPC, string strValue, string strType)
        {
            string[] arrData;
            try
            {
                if (strType.ToUpper() == "NAME")
                {
                    if (CurrentAPC.ParameterName == null)
                    {
                        CurrentAPC.ParameterName = new string[1];
                        CurrentAPC.ParameterName[0] = strValue;
                    }
                    else
                    {
                        arrData = CurrentAPC.ParameterName;
                        CurrentAPC.ParameterName = new string[CurrentAPC.ParameterName.Length + 1];

                        for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                        {
                            CurrentAPC.ParameterName[dintLoop] = arrData[dintLoop];
                        }
                        CurrentAPC.ParameterName[CurrentAPC.ParameterName.Length - 1] = strValue;
                    }
                }
                else
                {
                    if (CurrentAPC.ParameterValue == null)
                    {
                        CurrentAPC.ParameterValue = new string[1];
                        CurrentAPC.ParameterValue[0] = strValue;
                    }
                    else
                    {
                        arrData = CurrentAPC.ParameterValue;
                        CurrentAPC.ParameterValue = new string[CurrentAPC.ParameterValue.Length + 1];

                        for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                        {
                            CurrentAPC.ParameterValue[dintLoop] = arrData[dintLoop];
                        }
                        CurrentAPC.ParameterValue[CurrentAPC.ParameterValue.Length - 1] = strValue;
                    }
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// System.ini를 읽어 EQP 정보를 구성한다.
        /// </summary>
        /// <comment>
        /// INI로부터 데이타를 읽어 저장한다.
        /// </comment>
        private void subEQPInfoInitial(string strEQPName)
        {
            string dSystemINI = Application.StartupPath + @"\system\System_"+strEQPName+".ini";
            string dCFGFile = Application.StartupPath + @"\system\SDCA3.cfg";
            string dstrSectionUnit = "UNIT";
            string dstrSectionPort = "PORT";
            string dstrSectionInfo = "ETCInfo";
            string dstrSectionHOST = "HOST";
            string dstrSectionSEM = "SEM";              //SEM 추가 - 110915 고석현
            string dstrSectionSecom = "EAP01";   //Secom Driver 이름
            string dstrSectionEQP = "CommunicationEQP";   //EQP 이름
            string dstrSectionLOG = "LOG";
            string dstrEQPID = "";
            string dstrKey = "";
            int dintCount = 0;

            try
            {
                //EQP에 관계없이 응용 프로그램 공통으로 사용할 정보(All)를 구성한다.
                InfoAct.clsAll all = PInfo.All;
                if (all == null)
                {
                    PInfo.AddAll();
                }

                //INI파일경로를 저장한다.
                all.SystemINIFilePath = dSystemINI;
                all.CfgFilePath = dCFGFile;

                //HOST정보를 INI에서 읽어 구조체이 저장한다.
                all.DeviceID = Convert.ToInt32(FunINIMethod.funINIReadValue("EAP01", "DEVICEID", "1", all.CfgFilePath));
                all.RetryCount = Convert.ToInt32(FunINIMethod.funINIReadValue("EAP01", "RETRYCOUNT", "3", all.CfgFilePath));
                all.LocalPort = FunINIMethod.funINIReadValue("EAP01", "LOCALPORTNUMBER", "7000", all.CfgFilePath);

                all.T3 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSecom, "T3", "45", all.CfgFilePath));
                all.T5 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSecom, "T5", "10", all.CfgFilePath));
                all.T6 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSecom, "T6", "5", all.CfgFilePath));
                all.T7 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSecom, "T7", "10", all.CfgFilePath));
                all.T8 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSecom, "T8", "5", all.CfgFilePath));
                all.T9 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionHOST, "T9", "45", all.CfgFilePath));

                //SEM 관련 설정을 읽어온다. - 110915 고석현
                all.CommPort = FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_Port", "NULL", all.SystemINIFilePath);
                all.SEM_BaudRate = FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_BaudRate", "115200", all.SystemINIFilePath);
                all.SEMAlarmTime = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_AlarmTime", "8", all.SystemINIFilePath));
                all.SEM_ErrorDelayCheckTime = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_ErrorDelayCheckTime", "10", all.SystemINIFilePath));
                all.SEM_ErrorCheckCount = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_ErrorCheckCount", "60", all.SystemINIFilePath));
                all.CommSetting = all.SEM_BaudRate + ",e,8,1";

                //ETCInfo정보를 INI에서 읽어 구조체이 저장한다.
                all.MDLN = FunINIMethod.funINIReadValue(dstrSectionInfo, "MDLN", "", all.SystemINIFilePath);
                all.CurrentLOTIndex = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionInfo, "CurrentLOTIndex", "0", all.SystemINIFilePath));                 //현재까지 발번한 LOTIndex(1~999)
                all.SoftVersion = FunINIMethod.funINIReadValue(dstrSectionInfo, "SOFTREV", "", all.SystemINIFilePath);
                all.SpecCode = FunINIMethod.funINIReadValue(dstrSectionInfo, "SpecCode", "", all.SystemINIFilePath);
                all.SizeWidth = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionInfo, "WindowSizeWidth", "1280", all.SystemINIFilePath));
                all.SizeHeight = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionInfo, "WindowSizeHeight", "1024", all.SystemINIFilePath));
                all.HOSTPPIDFilePath = FunINIMethod.funINIReadValue(dstrSectionInfo, "HOSTPPIDFilePath", "", PInfo.All.SystemINIFilePath);
                all.ProcDataKeepDays = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionInfo, "KEEPDAYS", "7", PInfo.All.SystemINIFilePath));
                all.EQPID = FunINIMethod.funINIReadValue(dstrSectionInfo, "EQPID", "A3TLM02S", PInfo.All.SystemINIFilePath);

                //Log 관련 설정 - 2012.09.06 Kim Youngsik
                //2012.08.01 Kim Youngsik... 프로그램 화면 최소 Size는 1024x768로 하자...
                //if (all.SizeWidth < 1024) all.SizeWidth = 1024;
                //if (all.SizeHeight < 768) all.SizeHeight = 768;

                //2012.10.23 Kim Youngsik... Log관련 설정 리딩
                all.LogFilePath = FunINIMethod.funINIReadValue(dstrSectionLOG, "LogFilePath", "\\PLCLOG", all.SystemINIFilePath);
                //all.LogFilePath = Application.StartupPath.ToString() + "\\" + all.LogFilePath;

                all.LogUseThread = Convert.ToBoolean(FunINIMethod.funINIReadValue(dstrSectionLOG, "LogUseThread", "True", all.SystemINIFilePath));
                all.LogThreadInterval = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionLOG, "LogThreadInterval", "50", all.SystemINIFilePath));
                all.LogFileKeepDays = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionLOG, "LogKeepDays", "90", all.SystemINIFilePath));

                all.MCCprocPath = FunINIMethod.funINIReadValue("MCC", "ProcPath", "", all.SystemINIFilePath);
                all.MCC_RunFlag = Convert.ToBoolean(FunINIMethod.funINIReadValue("MCC", "MCC_RunFlag", "FALSE", all.SystemINIFilePath));

                //EQPCount를 가져온다.
                dintCount = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionInfo, "EQPCount", "1", all.SystemINIFilePath));

                //EQPCount 만큼 돌면서 기준정보를 저장한다.
                for (int dintLoop = 1; dintLoop <= dintCount; dintLoop++)
                {
                    dstrKey = "EQP" + dintLoop.ToString();
                    dstrEQPID = FunINIMethod.funINIReadValue(dstrSectionEQP, dstrKey, "Main", all.SystemINIFilePath);
                    if (PInfo.AddEQP(dstrEQPID) == true)
                    {
                        InfoAct.clsEQP eqp = PInfo.EQP(dstrEQPID);
                        //UNIT정보를 INI에서 읽어 구조체에 저장한다.
                        eqp.UnitCount = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionUnit, "Count", "0", all.SystemINIFilePath));

                        //PORT정보를 INI에서 읽어 구조체에 저장한다.
                        eqp.SlotCount = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionPort, "SlotCount", "28", all.SystemINIFilePath));

                        //PLC정보를 INI에서 읽어 구조체에 저장한다.
                        eqp.DummyPLC = Convert.ToBoolean(FunINIMethod.funINIReadValue(dstrEQPID, "Dummy", "true", all.SystemINIFilePath));
                        eqp.Type = FunINIMethod.funINIReadValue(dstrEQPID, "Type", "PLC", all.SystemINIFilePath);
                        eqp.WordStart = FunINIMethod.funINIReadValue(dstrEQPID, "Word1 Start", "W0000", all.SystemINIFilePath);
                        eqp.WordEnd = FunINIMethod.funINIReadValue(dstrEQPID, "Word1 End", "W0000", all.SystemINIFilePath);
                        eqp.BitScanCount = FunINIMethod.funINIReadValue(dstrEQPID, "Scan Area Count", "3", all.SystemINIFilePath);

                        //Scan에 필요한 값들을 읽어온다. 어경태 20071119
                        eqp.ScanTime = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrEQPID, "ScanTime", "200", dSystemINI));
                        eqp.WorkingSizeMin = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrEQPID, "WorkingSizeMin", "1", dSystemINI));
                        eqp.WorkingSizeMax = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrEQPID, "WorkingSizeMax", "3", dSystemINI));

                        for (int dintIndex = 1; dintIndex <= 10; dintIndex++)
                        {
                            dstrKey = "Area" + dintIndex.ToString() + " ";
                            eqp.BitScanEnabled[dintIndex] = Convert.ToBoolean(FunINIMethod.funINIReadValue(dstrEQPID, dstrKey + "Scan", "", all.SystemINIFilePath));
                            eqp.BitScanStart[dintIndex] = FunINIMethod.funINIReadValue(dstrEQPID, dstrKey + "Start", "", all.SystemINIFilePath);
                            eqp.BitScanEnd[dintIndex] = FunINIMethod.funINIReadValue(dstrEQPID, dstrKey + "End", "", all.SystemINIFilePath);
                        }
                    }
                }
                //EQP
                PInfo.EQP("Main").EQPID = FunINIMethod.funINIReadValue(dstrSectionInfo, "EQPID", "EAP01", all.SystemINIFilePath);
                PInfo.EQP("Main").EQPName = FunINIMethod.funINIReadValue(dstrSectionInfo, "EQPName", "WET ETCH", all.SystemINIFilePath);
                PInfo.EQP("Main").EQPType = FunINIMethod.funINIReadValue(dstrSectionInfo, "EQPType", "", all.SystemINIFilePath);
                PInfo.EQP("Main").RecipeCheck = Convert.ToBoolean(FunINIMethod.funINIReadValue(dstrSectionInfo, "RecipeCheck", "true", all.SystemINIFilePath));

                PInfo.All.MCC_Simulation = Convert.ToBoolean(FunINIMethod.funINIReadValue("MCC", "MCC_SIMULATION", "false", dSystemINI));

                //20141106 이원규 (SEM_UDP)
                all.CommPort = FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_Port", "NULL", all.SystemINIFilePath);
                all.SEM_BaudRate = FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_BaudRate", "57600", all.SystemINIFilePath);
                all.SEMAlarmTime = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_AlarmTime", "8", all.SystemINIFilePath));
                all.SEM_ErrorDelayCheckTime = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_ErrorDelayCheckTime", "10", all.SystemINIFilePath));
                all.SEM_ErrorCheckCount = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_ErrorCheckCount", "60", all.SystemINIFilePath));
                all.SEMMode = Convert.ToBoolean(FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_On", "true", all.SystemINIFilePath));
                all.CommSetting = all.SEM_BaudRate + ",e,8,1";

                all.USE_UDP = Convert.ToBoolean(FunINIMethod.funINIReadValue("SEM", "USE_UDP", "false", all.SystemINIFilePath));
                IPAddress.TryParse(FunINIMethod.funINIReadValue("SEM", "UDP_IP", "192.168.100.200", all.SystemINIFilePath), out all.UDP_IP);
                all.UDP_PORT = Convert.ToInt32(FunINIMethod.funINIReadValue("SEM", "UDP_PORT", "9001", all.SystemINIFilePath));
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}
