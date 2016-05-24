using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Timers;
using System.Data;
using System.Data.OleDb;
using EQPAct;

namespace LogAct
{
    public class clsLogAct
    {
        //선언
        #region "선언"

        public string Version
        {
            get { return "Samsung SMD HCLN V1.0"; }
        }

        public InfoAct.clsInfo PInfo = InfoAct.clsInfo.Instance;

        //[2015/06/09] 메모리 초기화를 위해..(Add by HS)
        private clsEQPAct pEQP = new clsEQPAct();
       

        private System.Windows.Forms.Timer ptmrLogWrite;            //LOG를 Write할 타이머 정의      

        private string pstrLogFilesPath = @"D:\PLCLOG\MCC";

        private Hashtable pHTlogFiles = new Hashtable();

        private DateTime pdtLastDate = DateTime.Now.Date;

        private int pintLogKeepDays = 90;                           //PLCLOG 보관 기간(기본 30일)



        #region "MCC"
        private System.Windows.Forms.Timer ptmrMCCLogUpdate;        //MCC Log를 Update할 타이머 정의
        private string pstrMCCDateFolder = "";                      //yyyyMMdd 폴더경로

        //[2015/03/13]Stream과 Writer 배열(Add by HS)
        private FileStream[] parrMCCFS_T = new FileStream[15];                           //Parameter 로그를 쓰기위한 FileStream
        private StreamWriter[] parrMCCSW_T = new StreamWriter[15];                         //Parameter 로그를 쓰기위한 StreamWriter
        private FileStream[] parrMCCFS_I = new FileStream[15];                           //Parameter 로그를 쓰기위한 FileStream
        private StreamWriter[] parrMCCSW_I = new StreamWriter[15];                         //Parameter 로그를 쓰기위한 StreamWriter
        private FileStream[] parrMCCFS_IDX = new FileStream[15];                           //Parameter 로그를 쓰기위한 FileStream
        private StreamWriter[] parrMCCSW_IDX = new StreamWriter[15];                         //Parameter 로그를 쓰기위한 StreamWriter


        private int pintMCCLogWriteInterval = 250;

        private int pintMCC_TickPerSec = 1;

        private int pintCheckCount = -1;
        private int pintCheckInterval = 300;    // Sec
        private int pintMCCKeepDays = -30;
        private string pstrMCCbaseFolder = "";

        private string pstrDateFolder = "";
        private string pstrUploadDateFolder = "";
        private string pstrIDXDateFolder = "";
        private string pstrFileName_T = "";
        private string pstrUploadFileName_T = "";
        private string pstrFileName_I = "";
        private string pstrUploadFileName_I = "";
        private string pstrIDXFileName = "";
        private bool UploadIngFlag = false;

        private string pstrFileDateTime = string.Empty;
        private string pstrUploadFileDateTime = string.Empty;


        private InfoAct.clsFTP_Client pFTP;                         //추가 : 20101001 이상호

        //[2015/02/16]파일Name추가(Add by HS)
        private string[] pArrstrFileName_T = new string[15];
        private string[] pArrstrUploadFileName_T = new string[15];
        private string[] pArrstrFileName_I = new string[15];
        private string[] pArrstrUploadFileName_I = new string[15];
        private string[] pArrstrIDXFileName = new string[15];
        private int pnSubUnit = 0;
        private string[] parrFolder; 
        private string pstrFolderNameTEMP = "";
        private string pstrFolderPath = "";
        private string pstrTemp = "";
        //[2015/06/11]I Log저장 부분 변경(Add by HS)
        private StringBuilder pstrInfoLog = new StringBuilder();
        private StringBuilder pstrMCCInfoItemData = new StringBuilder();

        

        #endregion


        #endregion


        public void subClose()
        {
            try
            {
                if (!DBAct.clsDBAct.funMCCIsNullTransaction()) DBAct.clsDBAct.MCCcommand.Transaction.Commit();
            }
            catch
            {
                DBAct.clsDBAct.MCCcommand.Transaction.Rollback();
            }
            finally
            {
                DBAct.clsDBAct.funMCCDisconnect();
            }
        }


        /// <summary>
        /// 초기화시 "PLCLOG" 폴더가 없으면 생성하고 로그 출력 Thread 생성
        /// </summary>
        /// <remarks>
        /// 최초 프로그램 로딩시 한번만 수행됨
        /// </remarks>
        public void subInitialLog()
        {
            string dstrIPAddress = "192.169.1.1";
            string dstrPortNumber = "21";

            try
            {
                DirectoryInfo dir = new DirectoryInfo(pstrLogFilesPath);     //PLCLOG 폴더가 없으면 생성한다.
                dir.Create();

                //FTP 객체 생성     //추가 : 20101001 이상호
                pFTP = new InfoAct.clsFTP_Client(dstrIPAddress + ":" + dstrPortNumber, "", "");
                pFTP.UploadCompleted += new InfoAct.clsFTP_Client.FTPuploadComplete(pFTP_UploadCompleted);
                //pFTP.PInfo = this.PInfo;

                //MCCfile.mdb Transaction
                if (!DBAct.clsDBAct.funMCCConnect(DBAct.clsDBAct.pstrMCCConnection)) this.PInfo.All.pblUseMDB = false;
                else if (!DBAct.clsDBAct.funMCCBeginTransaction()) this.PInfo.All.pblUseMDB = false;
                
                if (this.PInfo.All.pblUseMDB == false) this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "MCCfile.mdb Connection failure!!");

                ////LogWrite_Tick Timer 설정
                this.ptmrLogWrite = new System.Windows.Forms.Timer();
                this.ptmrLogWrite.Tick += new EventHandler(LogWrite_Tick);
                this.ptmrLogWrite.Interval = 1000;      //500ms
                this.ptmrLogWrite.Enabled = true;
                ////GC.KeepAlive(this.ptmrLogWrite);


                pintMCC_TickPerSec = 1000 / pintMCCLogWriteInterval;
                pintCheckInterval = pintCheckInterval * pintMCC_TickPerSec;
                pintCheckCount = 10 * pintMCC_TickPerSec;                           // 최초 1회 업로드 파일 유무 확인용

                ////MMC Log Tick Timer 설정
                this.ptmrMCCLogUpdate = new System.Windows.Forms.Timer();
                this.ptmrMCCLogUpdate.Tick += new EventHandler(MCCLogUpdate_Tick);
                this.ptmrMCCLogUpdate.Interval = pintMCCLogWriteInterval;      //500ms
                this.ptmrMCCLogUpdate.Enabled = true;
                ////GC.KeepAlive(this.ptmrMCCLogUpdate);


                

                this.pstrMCCbaseFolder = this.PInfo.All.MCCLootFilePath + @"\" + this.PInfo.Unit(0).SubUnit(0).ModuleID;

            }
            catch(Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }





        #region "PROGRAM LOG"
        /// <summary>
        /// 열려있는 로그파일 핸들을 닫는다.
        /// </summary>
        /// <param name="dDelTime">기준 시간</param>
        /// <returns>전체 파일 핸들 확인시 true</returns>
        private bool funCloseLogFile(DateTime dDelTime)
        {
            try
            {
                foreach (clsLogFiles clf in this.pHTlogFiles.Values)
                {
                    if (clf.CreateDate < dDelTime)
                    {
                        clf.Close();
                        this.pHTlogFiles.Remove(clf.logType);
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 보관 기간이 만기된 로그 파일 삭제
        /// </summary>
        private void subDeleteLogFile()
        {
            string[] dstrDelDirectorys;             
            DateTime dDelTime;

            try
            {
                dDelTime = DateTime.Now.Date.AddDays(-pintLogKeepDays);

                while (!funCloseLogFile(dDelTime));

                dstrDelDirectorys = Directory.GetDirectories(pstrLogFilesPath);

                for (int iFileIndex = 0; iFileIndex < dstrDelDirectorys.Length; iFileIndex++)
                {
                    string dstrDirectoryName = dstrDelDirectorys[iFileIndex].Substring(dstrDelDirectorys[iFileIndex].LastIndexOf(@"\") + 1);

                    DateTime dCreateTime;


                    if (DateTime.TryParseExact(dstrDirectoryName, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dCreateTime))
                    {
                        if (dCreateTime < dDelTime) Directory.Delete(dstrDelDirectorys[iFileIndex], true);
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 로그 파일 생성
        /// </summary>
        /// <param name="dLogType">로그 파일 타입</param>
        /// <param name="dstrDate">생성 날짜</param>
        private void subLogFileCreate(InfoAct.clsInfo.LogType dLogType, string dstrDate)
        {
            try
            {
                if (!this.pHTlogFiles.Contains(dLogType))
                {
                    string dstrFileName = funGetLogFileName(dLogType);
                    string dstrDateFolder = this.pstrLogFilesPath + "\\" + dstrDate;
                    
                    if (!Directory.Exists(dstrDateFolder)) Directory.CreateDirectory(dstrDateFolder);

                    this.pHTlogFiles.Add(dLogType, new clsLogFiles(dLogType, dstrFileName, new FileStream(dstrDateFolder + "\\" + dstrFileName, FileMode.Append, FileAccess.Write, FileShare.Read)));
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 로그별 시간 기록방식이 상이하여
        /// 각각의 로그 발생 시간 부분 편집
        /// 및 마지막 로그시간 기록
        /// </summary>
        /// <param name="dLogData">로그 데이터</param>
        /// <param name="dLogFile">로그 파일</param>
        /// <returns>실제 출력할 로그 문자열</returns>
        private string funGetLogString(InfoAct.clsLogData dLogData, ref clsLogFiles dLogFile)
        {
            StringBuilder dstrWriteMsg = new StringBuilder();

            try
            {
                if (dLogData.Type == InfoAct.clsInfo.LogType.CIM
                    || dLogData.Type == InfoAct.clsInfo.LogType.GLSInOut
                    || dLogData.Type == InfoAct.clsInfo.LogType.PLCError
                    || dLogData.Type == InfoAct.clsInfo.LogType.PLC)
                {
                    if (dLogData.Log.Contains(dLogFile.LastModify) && !string.IsNullOrEmpty(dLogFile.LastModify)) dstrWriteMsg.Append(dLogData.Log.Substring(73));
                    else dstrWriteMsg.Append(dLogData.Log);

                    dLogFile.LastModify = dLogData.Log.Substring(1, 19);
                }
                else
                {
                    if (dLogData.Type == InfoAct.clsInfo.LogType.OPCallMSG)
                    {
                        dLogFile.LastModify = dLogData.Log.Substring(0, 19);
                    }
                    else if (dLogData.Type == InfoAct.clsInfo.LogType.Alarm
                   || dLogData.Type == InfoAct.clsInfo.LogType.AlarmGLSInfo)
                    {
                        dLogFile.LastModify = DateTime.ParseExact(dLogData.Log.Substring(0, 14), "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else if (dLogData.Type == InfoAct.clsInfo.LogType.Parameter
                       || dLogData.Type == InfoAct.clsInfo.LogType.ScrapUnScrapAbort)
                    {
                        dLogFile.LastModify = DateTime.ParseExact(dLogData.Log.Substring(6, 14), "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        dLogFile.LastModify = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    dstrWriteMsg.Append(dLogData.Log);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstrWriteMsg.ToString();
        }

        /// <summary>
        /// 로그 기록용 타이머 메소드
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="EventArgs"></param>
        private void LogWrite_Tick(Object Object, EventArgs EventArgs)
        {
            try
            {
                //// 20121018 
                //if (this.PInfo.All.pTestStart == false) return;

                //if (this.PInfo.All.pStartTime == DateTime.MinValue) this.PInfo.All.pStartTime = DateTime.Now;

                ptmrLogWrite.Enabled = false;

                InfoAct.clsLogData dLogData;
                clsLogFiles dLogFile;
                string dstrLog = "";
                DateTime dDate;
                string dstrLastModify = "";

                int dintCount = this.PInfo.funGetLogCount();

                while (dintCount-- > 0)
                {
                    

                    dLogData = this.PInfo.funGetLog();

                    if (dLogData != null)// && dLogData.Type != InfoAct.clsInfo.LogType.MCCLog)
                    {
                        subLogFileCreate(dLogData.Type, this.pdtLastDate.ToString("yyyyMMdd"));

                        dLogFile = ((clsLogFiles)this.pHTlogFiles[dLogData.Type]);

                        dstrLog = funGetLogString(dLogData, ref dLogFile);

                        if (!string.IsNullOrEmpty(dstrLog))
                        {
                            dDate = DateTime.ParseExact(dLogFile.LastModify, "yyyy-MM-dd HH:mm:ss", null).Date;

                            if (dDate != dLogFile.CreateDate)
                            {
                                dstrLastModify = dLogFile.LastModify;

                                dLogFile.Writer.Flush();
                                dLogFile.Close();
                                this.pHTlogFiles.Remove(dLogFile.logType);

                                subLogFileCreate(dLogData.Type, dDate.ToString("yyyyMMdd"));

                                dLogFile = ((clsLogFiles)this.pHTlogFiles[dLogData.Type]);
                            }

                            if (!string.IsNullOrEmpty(dstrLog) && dLogFile != null && dLogFile.Stream.CanWrite && dLogFile.Writer != null)
                            {
                                dLogFile.Writer.WriteLine(dstrLog);
                                dLogFile.Writer.Flush();
                            }
                        }
                    }
                }


                if (this.pdtLastDate < DateTime.Now.Date)
                {
                    this.pdtLastDate = DateTime.Now.Date;
                    subDeleteLogFile();
                }
                

                //// 20121018 
                //if (this.PInfo.All.pTestStart && this.PInfo.funGetLogCount() == 0)
                //{
                //    this.PInfo.All.pElapsedTime = DateTime.Now - this.PInfo.All.pStartTime;
                //    this.PInfo.All.pStartTime = DateTime.MinValue;
                //    this.PInfo.All.pTestStart = false;

                //    System.Diagnostics.Debug.WriteLine(string.Format("Elaped Time: {0}ms", this.PInfo.All.pElapsedTime.TotalMilliseconds)); 
                //}

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                ptmrLogWrite.Enabled = true;                                //Timer를 다시 기동한다.
            }
        }

        /// <summary>
        /// 생성할 로그 파일 이름 구하는 함수
        /// </summary>
        /// <param name="dLogType">로그 타입</param>
        /// <returns>로그 파일 이름</returns>
        private string funGetLogFileName(InfoAct.clsInfo.LogType dLogType)
        {
            string dstrFileName = string.Empty;

            try
            {
                switch (dLogType)
                {
                    case InfoAct.clsInfo.LogType.Alarm:
                        dstrFileName = "Alarm.log";
                        break;
                    case InfoAct.clsInfo.LogType.AlarmGLSInfo:
                        dstrFileName = "AlarmGLSInfo.log";
                        break;
                    case InfoAct.clsInfo.LogType.btnEVENT:
                        dstrFileName = "ButtonEvent.log";
                        break;
                    case InfoAct.clsInfo.LogType.CIM:
                        dstrFileName = "CIM.log";
                        break;
                    case InfoAct.clsInfo.LogType.GLSInOut:
                        dstrFileName = "GLSInOut.log";
                        break;
                    case InfoAct.clsInfo.LogType.GLSPDC:
                        dstrFileName = "GLSPDC.log";
                        break;
                    case InfoAct.clsInfo.LogType.LOTPDC:
                        dstrFileName = "LOTPDC.log";
                        break;
                    case InfoAct.clsInfo.LogType.OPCallMSG:
                        dstrFileName = "OPCallMSG.log";
                        break;
                    case InfoAct.clsInfo.LogType.Parameter:
                        dstrFileName = "Parameter.log";
                        break;
                    case InfoAct.clsInfo.LogType.PLC:
                        dstrFileName = "PLC.log";
                        break;
                    case InfoAct.clsInfo.LogType.PLCError:
                        dstrFileName = "PLCError.log";
                        break;
                    case InfoAct.clsInfo.LogType.ScrapUnScrapAbort:
                        dstrFileName = "Scrap.log";
                        break;
                    case InfoAct.clsInfo.LogType.SEM:
                        dstrFileName = "SEM.log";
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstrFileName;
        }
        #endregion

        #region "MCC LOG"
        /// <summary>
        /// MCC Log를 Write 한다.
        /// </summary>
        /// <param name="strLog"></param>
        /// <remarks>
        /// 20101001            이상호          [L 00]
        /// 20120626            이상창          [L 01]
        /// </remarks>
        private void subWriteMCCLog(string strLog)
        {
            object obj = new object();
            //[2015/06/03](Add by HS)
            InfoAct.clsMCC MCCInfo;
			int nSubUnitCount = 0;
            try
            {
                lock (obj)
                {

                    string dstrMCCLogTime = this.pstrFileDateTime;

                    if (strLog != "")
                    {
                        if (DateTime.ParseExact(this.pstrFileDateTime, "yyyyMMddHHmmss", null)
                            > DateTime.ParseExact(DateTime.Now.ToString("yyyy") + strLog.Substring(0, 4) + strLog.Substring(5, 4) + "00", "yyyyMMddHHmmss", null))
                        {
                            dstrMCCLogTime = this.pstrUploadFileDateTime;
                        }
                    }

                    string dstrModuleID = this.PInfo.EQP("Main").EQPID;

                    pstrFileName_T = @"\T-" + dstrModuleID + "-" + dstrMCCLogTime + ".csv";
                    pstrFileName_I = @"\I-" + dstrModuleID + "-" + dstrMCCLogTime + ".csv";
                    pstrIDXFileName = @"\" + "index-" + dstrModuleID + "-" + dstrMCCLogTime + ".csv";
                    string[] arrFolder; // 20150216 고석현 추가
                    string strFolderNmaeTEMP = "";// 20150216 고석현 추가
                    string strFolderPath = "";

                    //[2015/02/16] MCC folder추가(Add by HS) // 20150216 고석현 추가
                    for (int nUnitIndex = 1; nUnitIndex <= PInfo.UnitCount; nUnitIndex++)
                    {
                        for (int nSubUnitIndex = 1; nSubUnitIndex <= PInfo.Unit(nUnitIndex).SubUnitCount; nSubUnitIndex++)
                        {
                            strFolderNmaeTEMP = "";
                            strFolderPath = "";
                            arrFolder = this.PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID.Split('_');

                            for (int dintLoop = 0; dintLoop < arrFolder.Length; dintLoop++)
                            {
                                strFolderNmaeTEMP += arrFolder[dintLoop];

                                //[2015/04/13]MCC담당자 요청으로 Layer1 삭제(Add by HS)
                                if (dintLoop == 1)
                                {
                                    strFolderNmaeTEMP += "_";
                                    strFolderPath += arrFolder[dintLoop] + "_";

                                    continue;
                                }

                                if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderNmaeTEMP)) == false)
                                {
                                    Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderNmaeTEMP));
                                }
                                strFolderNmaeTEMP += @"\";
                                strFolderPath += arrFolder[dintLoop] + "_";
                                if (dintLoop < arrFolder.Length - 1)
                                {
                                    strFolderNmaeTEMP += strFolderPath;
                                }
                            }

                            strFolderPath = strFolderNmaeTEMP + "index";
                            if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderPath)) == false)
                            {
                                Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderPath));
                            }

                            pstrIDXFileName = string.Format("index-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, pstrFileDateTime);
                            if (File.Exists(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, strFolderPath, pstrIDXFileName)) == false)
                            {
                                FileStream fs = new FileStream(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderPath + @"\" + pstrIDXFileName), FileMode.Create, FileAccess.Write, FileShare.Write);
                                fs.Close();
                            }

                            strFolderPath = strFolderNmaeTEMP + dstrMCCLogTime.Substring(2, 6);
                            if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderPath)) == false)
                            {
                                Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderPath));
                            }

                            string strDateTime = pstrFileDateTime;

                            #region 임시로 T, I 폴더 생성
                            strFolderPath = strFolderNmaeTEMP + dstrMCCLogTime.Substring(2, 6) + @"\T";
                            if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderPath)) == false)
                            {
                                Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderPath));
                            }

                            if (File.Exists(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, strFolderPath, string.Format("T-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime))) == false)
                            {
                                FileStream fs = new FileStream(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, strFolderPath, string.Format("T-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime)), FileMode.Create, FileAccess.Write, FileShare.Write);
                                fs.Close();
                            }

                            strFolderPath = strFolderNmaeTEMP + dstrMCCLogTime.Substring(2, 6) + @"\I";
                            if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderPath)) == false)
                            {
                                Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, strFolderPath));
                            }

                            if (File.Exists(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, strFolderPath, string.Format("I-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime))) == false)
                            {
                                FileStream fs = new FileStream(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, strFolderPath, string.Format("I-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime)), FileMode.Create, FileAccess.Write, FileShare.Write);
                                fs.Close();
                            }
                            #endregion
                        }
                    }

                    pstrMCCDateFolder = pstrDateFolder;  //현재 날짜를 저장

                    nSubUnitCount = 0;
                    if (strLog != "")
                    {
                        string[] arrCon = strLog.Split(',');

                        //[2015/04/01] I로그와 T로그 구분(Add by HS)
                        if (arrCon[1] == "I")
                        {
                            //pstrTemp = arrCon[0] + "," + arrCon[1] + ",";
                            pstrInfoLog.Clear();
                            pstrInfoLog.Append(arrCon[0] + "," + arrCon[1] + ",");

                            //[2015/03/15]Module ID 매칭(Add by HS)
                            try
                            {
                                for (int nUnitIndex = 1; nUnitIndex <= PInfo.UnitCount; nUnitIndex++)
                                {
                                    for (int nSubUnitIndex = 1; nSubUnitIndex <= PInfo.Unit(nUnitIndex).SubUnitCount; nSubUnitIndex++)
                                    {
                                        for (int dintLoop = 0; dintLoop < PInfo.Unit(0).SubUnit(0).MCCInfoCount; dintLoop++)
                                        {
                                            if (this.PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID == PInfo.Unit(0).SubUnit(0).MCCInfo(dintLoop + 1).ModuleID)
                                            {
                                                //pstrTemp += arrCon[dintLoop + 2] + ",";
                                                pstrInfoLog.Append(arrCon[dintLoop + 2]);
                                                pstrInfoLog.Append(",");
                                            }
                                        }
                                        //여기에 값을 나누는 부분 추가.
                                        if (pstrInfoLog.Length != 0)
                                        {
                                            //pstrTemp = pstrTemp.Substring(0, pstrTemp.Length - 1);
                                            pstrInfoLog.Remove(pstrInfoLog.Length - 1, 1);
                                        }

                                        if (arrCon[1].Trim() == "I")
                                        {
                                            parrMCCSW_I[nSubUnitCount].WriteLine(pstrInfoLog);
                                            parrMCCSW_I[nSubUnitCount].Flush();
                                        }

                                        //pstrTemp = arrCon[0] + "," + arrCon[1] + ",";
                                        pstrInfoLog.Clear();
                                        pstrInfoLog.Append(arrCon[0] + "," + arrCon[1] + ",");

                                        nSubUnitCount++;


                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + "nSubUnitCount : " + nSubUnitCount);
                            }
                        }
                        else //T Log
                        {
                            int nIndex = 0;

                            switch (arrCon[1].Substring(arrCon[1].Length - 4, 4))
                            {
                                case "FI01":
                                    nIndex = 0;
                                    break;
                                case "FI02":
                                    nIndex = 1;
                                    break;
                                case "FO03":
                                    nIndex = 2;
                                    break;
                                case "FO04":
                                    nIndex = 3;
                                    break;
                                case "PI01":
                                    nIndex = 4;
                                    break;
                                case "PO02":
                                    nIndex = 5;
                                    break;
                                case "FT01":
                                    nIndex = 6;
                                    break;
                                case "FT02":
                                    nIndex = 7;
                                    break;
                                case "AL01":
                                    nIndex = 8;
                                    break;
                                case "LM01":
                                    nIndex = 9;
                                    break;
                                case "DM01":
                                    nIndex = 10;
                                    break;
                                case "IS01":
                                    nIndex = 11;
                                    break;
                                case "ST01":
                                    nIndex = 12;
                                    break;
                                case "ST02":
                                    nIndex = 13;
                                    break;
                                case "GL01":
                                    nIndex = 14;
                                    break;
                                default:
                                    return;
                            }
                            parrMCCSW_T[nIndex].WriteLine(strLog.Trim());
                            parrMCCSW_T[nIndex].Flush();

                        }
                    }
                    else
                    {
                        pstrMCCInfoItemData.Clear();
                        pstrMCCInfoItemData.Append("EventTime,LogType,");
                        //[2015/02/23]반복되는 Count변경(modify by HS)
                        //[2015/03/15]모듈 ID 매칭(Add by HS)
                        for (int nUnitIndex = 1; nUnitIndex <= PInfo.UnitCount; nUnitIndex++)
                        {
                            for (int nSubUnitIndex = 1; nSubUnitIndex <= PInfo.Unit(nUnitIndex).SubUnitCount; nSubUnitIndex++)
                            {
                                for (int dintLoop = 0; dintLoop < PInfo.Unit(0).SubUnit(0).MCCInfoCount; dintLoop++)
                                {
                                    MCCInfo = PInfo.Unit(0).SubUnit(0).MCCInfo(dintLoop + 1);
                                    if (this.PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID.Substring(this.PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID.Length - 4, 4) == MCCInfo.ModuleID.Substring(MCCInfo.ModuleID.Length - 4, 4))
                                    {
                                        pstrMCCInfoItemData.Append(MCCInfo.ModuleID.Substring(MCCInfo.ModuleID.Length - 4, 4) + "=" + MCCInfo.MCCName + "(" + MCCInfo.Unit + "),");
                                    }
                                    MCCInfo = null;
                                }
                                if (pstrMCCInfoItemData.Length != 0)
                                {
                                    pstrMCCInfoItemData.Remove(pstrMCCInfoItemData.Length - 1, 1);
                                }

                                parrMCCSW_I[nSubUnitCount].WriteLine(pstrMCCInfoItemData);
                                parrMCCSW_I[nSubUnitCount].Flush();
                                nSubUnitCount++;
                                pstrMCCInfoItemData.Clear();
                                pstrMCCInfoItemData.Append("EventTime,LogType,");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                pstrInfoLog.Clear();
            }
        }

        /// <summary>
        /// 설정된 날짜가 지난 파일 및 폴더 삭제
        /// 매 시각이 변할때만 실행하자,.
        /// </summary>
        private void subDeleteMCCLogFile()
        {
            string[] dstrDelDirectorys;        //삭제할 폴더의 Loot 경로안의 Log 폴더들을 저장              
            DateTime dDelTime;
            string[] dstrDelFiles;
            DateTime dCreateTime;
            string dstrDirectoryName;

            try
            {
                dstrDelDirectorys = Directory.GetDirectories(pstrMCCbaseFolder);

                dDelTime = DateTime.ParseExact(DateTime.Now.AddDays(-this.PInfo.All.MCCLogFileDelete).ToString("yyMMdd"), "yyMMdd", null);

                try
                {
                    if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();


                    for (int iFileIndex = 0; iFileIndex < dstrDelDirectorys.Length; iFileIndex++)
                    {
                        dstrDirectoryName = dstrDelDirectorys[iFileIndex].Substring(dstrDelDirectorys[iFileIndex].LastIndexOf(@"\") + 1);

                        if (dstrDirectoryName.Equals("index"))
                        {
                            bool DelFileExist = false;
                            string dstrDelDate = "";

                            // 인덱스 폴더 따로 처리하자.. 파일 목록 불러와서 이름에서 시간 확인 후... 삭제..
                            // 물론 디비도 확인한다.
                            dstrDelFiles = Directory.GetFiles(dstrDelDirectorys[iFileIndex]);

                            for (int dintLoop = 0; dintLoop < dstrDelFiles.Length; dintLoop++)
                            {
                                string strFileName = dstrDelFiles[dintLoop];

                                if (strFileName.Length > 16)
                                {
                                    string strDateTime = strFileName.Substring(0, strFileName.Length - 10);           // HH0000.csv 자르기
                                    strDateTime = strDateTime.Substring(strDateTime.Length - 6);                      // yyMMdd 부분만 챙겨보자.

                                    DateTime dIndexCreateTime;

                                    if (DateTime.TryParseExact(strDateTime, "yyMMdd", null, System.Globalization.DateTimeStyles.None, out dIndexCreateTime))
                                    {
                                        if (dIndexCreateTime < dDelTime)
                                        {
                                            // 이거 지워야된다.
                                            File.Delete(strFileName);

                                            DelFileExist = true;

                                            if (!dstrDelDate.Contains(strDateTime))
                                            {
                                                dstrDelDate += strDateTime + ",";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // 프로그램이 생성한 파일이 아니다..
                                        // 어떻게 처리할까?
                                    }
                                }
                            }



                            if (DelFileExist && this.PInfo.All.pblUseMDB)   // 인덱스 파일 중 지운게 있단다.
                            {
                                // CIM PC 보관 기간 90일 MCC FTP 파일 보관 기간 30일.. 그래서 그냥 지우면 된다.
                                // DELETE FROM `tbMCCfile` WHERE `CreateTime`= strDateTime AND `FileType`='INDEX'

                                string[] dstrDelDates = dstrDelDate.Split(',');

                                for (int dintLoop = 0; dintLoop < dstrDelDates.Length - 1; dintLoop++)
                                {
                                    DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `CreateDate`='{0}' AND `FileType`='INDEX';", dstrDelDates[dintLoop]);
                                    DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                    //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `CreateDate`='{0}' AND `FileType`='INDEX';", dstrDelDates[dintLoop]));
                                }

                            }
                        }
                        else if (DateTime.TryParseExact(dstrDirectoryName, "yyMMdd", null, System.Globalization.DateTimeStyles.None, out dCreateTime))
                        {
                            if (dCreateTime < dDelTime)
                            {
                                // 이거 폴더다... 지워야된다.
                                Directory.Delete(dstrDelDirectorys[iFileIndex], true);

                                if (this.PInfo.All.pblUseMDB)
                                {
                                    // CIM PC 보관 기간 90일 MCC FTP 파일 보관 기간 30일.. 그래서 그냥 지우면 된다.
                                    // DELETE FROM `tbMCCfile` WHERE `CreateTime`=dstrDirectoryName AND `FileType`='LOG'
                                    //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `CreateDate`='{0}' AND `FileType`='LOG';", dstrDirectoryName));
                                    DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `CreateDate`='{0}' AND `FileType`='LOG';", dstrDirectoryName);
                                    DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                }
                            }


                        }
                        else
                        {
                            // 프로그램이 생성한 폴더가 아니다..
                            // 어떻게 처리할까?
                        }
                    }

                    if (this.PInfo.All.pblUseMDB)
                    {
                        DBAct.clsDBAct.MCCcommand.Transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    if (this.PInfo.All.pblUseMDB) DBAct.clsDBAct.MCCcommand.Transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void pFTP_UploadCompleted()
        {
            try
            {
                UploadIngFlag = false;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

       




        /// <summary>
        /// MCC Log를 출력한다.
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="EventArgs"></param>
        /// <remarks>
        /// 20120625            이상창
        /// </remarks>
        /// 
        string dstrMCCLogTime = string.Empty;
        private void MCCLogUpdate_Tick(Object Object, EventArgs EventArgs)
        {
            try
            {
                string dstrData = "";
                this.ptmrMCCLogUpdate.Enabled = false;
                string strDateTime = "";

                // 메소드 실행 시간
                string dstrDateNowTime = DateTime.Now.ToString("yyyyMMddHHmm") + "00";

                #region "프로그램 처음 시작시 이건 한번만 실행되면 OK"
                try
                {
                    if (string.IsNullOrEmpty(this.pstrFileDateTime))
                    {
                        // 현재 로그 기록에 사용할 파일명
                        // 여기서 확인해야 할게...
                        // 기존 사용중이던 마지막 파일이 있는지 확인..
                        // 파일 명이 interval 범위에 포함되는지 보고..
                        // 변경 없이 사용 가능 하다면.. 기존 파일에 이어서 쓴다.

                        string strTemp = string.Empty;

                        // 되도록 정시 기준으로 시작..
                        strTemp = dstrDateNowTime.Substring(0, 10) + "0000";

                        DateTime dtTemp = DateTime.ParseExact(strTemp, "yyyyMMddHHmmss", null);

                        while (dtTemp <= DateTime.ParseExact(dstrDateNowTime, "yyyyMMddHHmmss", null))
                        {
                            dtTemp = dtTemp.AddMinutes(this.PInfo.All.MCCFileUploadTime);
                        }

                        strTemp = dtTemp.AddMinutes(-this.PInfo.All.MCCFileUploadTime).ToString("yyyyMMddHHmmss");

                        this.pstrFileDateTime = strTemp;
                        this.pstrFileName_T = @"\T-" + this.PInfo.EQP("Main").EQPID + "-" + strTemp + ".csv";
                        this.pstrFileName_I = @"\I-" + this.PInfo.EQP("Main").EQPID + "-" + strTemp + ".csv";

                        //if (this.PInfo.All.pblUseMDB)
                        //{
                        //    #region// 기존파일 사용 가능 여부 확인
                        //    DBAct.clsDBAct.MCCcommand.CommandText = @"SELECT * FROM `tbMCCfile` ORDER BY `Index` DESC;";
                        //    OleDbDataReader dr = DBAct.clsDBAct.MCCcommand.ExecuteReader(CommandBehavior.SingleRow);

                        //    if (dr != null)
                        //    {
                        //        if (dr.Read())
                        //        {
                        //            strTemp = dr.GetString(3);

                        //            if (!string.IsNullOrEmpty(strTemp))
                        //            {
                        //                strTemp = strTemp.Substring(strTemp.Length - 18, 14);   // 파일명에서 시간만 ("yyyyMMddHHmmss") 자른다.
                        //                if (DateTime.ParseExact(strTemp, "yyyyMMddHHmmss", null).AddMinutes(this.PInfo.All.MCCFileUploadTime) > DateTime.ParseExact(dstrDateNowTime, "yyyyMMddHHmmss", null))
                        //                {
                        //                    // 기존 파일 사용가능하다..
                        //                    this.pstrFileDateTime = strTemp;
                        //                    this.pstrFileName_T = @"\T-" + this.PInfo.EQP("Main").EQPID + "-" + strTemp + ".csv";
                        //                    this.pstrFileName_I = @"\I-" + this.PInfo.EQP("Main").EQPID + "-" + strTemp + ".csv";
                        //                }
                        //            }
                        //        }

                        //        dr.Close();
                        //        dr.Dispose();
                        //        dr = null;
                        //    }
                        //    #endregion

                            pstrDateFolder = this.PInfo.All.MCCLootFilePath + @"\" + this.PInfo.Unit(0).SubUnit(0).ModuleID + "\\" + this.pstrFileDateTime.Substring(2, 6);

                            //[2015/03/13] 파일 생성 및 파일 저장 부분(Add by HS)
                            pnSubUnit = 0;
                            
                            for (int nUnitIndex = 1; nUnitIndex <= PInfo.UnitCount; nUnitIndex++)
                            {
                                for (int nSubUnitIndex = 1; nSubUnitIndex <= PInfo.Unit(nUnitIndex).SubUnitCount; nSubUnitIndex++)
                                {
                                    dstrMCCLogTime = this.pstrFileDateTime;

                                    this.pstrFolderNameTEMP = "";
                                    this.pstrFolderPath = "";
                                    this.parrFolder = this.PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID.Split('_');

                                    for (int dintLoop = 0; dintLoop < this.parrFolder.Length; dintLoop++)
                                    {
                                        this.pstrFolderNameTEMP += this.parrFolder[dintLoop];

                                        //[2015/04/13]MCC담당자 요청으로 Layer1 삭제(Add by HS)
                                        if (dintLoop == 1)
                                        {
                                            this.pstrFolderNameTEMP += "_";
                                            this.pstrFolderPath += this.parrFolder[dintLoop] + "_";
                                            //if (dintLoop < this.parrFolder.Length - 1)
                                            //{
                                            //    this.pstrFolderNameTEMP += this.pstrFolderPath;
                                            //}
                                            continue;
                                        }

                                        if (Directory.Exists(string.Format("{0}\\{1}", this.pstrFolderPath, this.pstrFolderNameTEMP)) == false)
                                        {
                                            Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderNameTEMP));
                                        }
                                        this.pstrFolderNameTEMP += @"\";
                                        this.pstrFolderPath += this.parrFolder[dintLoop] + "_";
                                        if (dintLoop < this.parrFolder.Length - 1)
                                        {
                                            this.pstrFolderNameTEMP += this.pstrFolderPath;
                                        }
                                    }

                                    this.pstrFolderPath = this.pstrFolderNameTEMP + "index";
                                    if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath)) == false)
                                    {
                                        Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath));
                                    }

                                    pstrIDXFileName = string.Format("index-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, pstrFileDateTime);
                                    if (File.Exists(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, pstrIDXFileName)) == false)
                                    {
                                        this.parrMCCFS_IDX[pnSubUnit] = new FileStream(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath + @"\" + pstrIDXFileName), FileMode.Create, FileAccess.Write, FileShare.Write);
                                        this.pArrstrIDXFileName[pnSubUnit] = PInfo.All.MCCLootFilePath + @"\" + this.pstrFolderPath + @"\" + pstrIDXFileName;
                                    }
                                    else
                                    {
                                        this.parrMCCFS_IDX[pnSubUnit] = new FileStream(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath + @"\" + pstrIDXFileName), FileMode.Create, FileAccess.Write, FileShare.Write);
                                        this.pArrstrIDXFileName[pnSubUnit] = PInfo.All.MCCLootFilePath + @"\" + this.pstrFolderPath + @"\" + pstrIDXFileName;

                                    }

                                    this.pstrFolderPath = this.pstrFolderNameTEMP + dstrMCCLogTime.Substring(2, 6);
                                    if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath)) == false)
                                    {
                                        Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath));
                                    }

                                    strDateTime = pstrFileDateTime;

                                    this.pstrFolderPath = this.pstrFolderNameTEMP + dstrMCCLogTime.Substring(2, 6) + @"\T";
                                    if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath)) == false)
                                    {
                                        Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath));
                                    }

                                    if (File.Exists(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, string.Format("T-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime))) == false)
                                    {
                                        this.parrMCCFS_T[pnSubUnit] = new FileStream(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, string.Format("T-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime)), FileMode.Create, FileAccess.Write, FileShare.Write);
                                        this.pArrstrFileName_T[pnSubUnit] = PInfo.All.MCCLootFilePath + @"\" + this.pstrFolderPath + @"\" + string.Format("T-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime);
                                    }
                                    else
                                    {
                                        this.parrMCCFS_T[pnSubUnit] = new FileStream(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, string.Format("T-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime)), FileMode.Create, FileAccess.Write, FileShare.Write);
                                        this.pArrstrFileName_T[pnSubUnit] = PInfo.All.MCCLootFilePath + @"\" + this.pstrFolderPath + @"\" + string.Format("T-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime);

                                    }

                                    this.pstrFolderPath = this.pstrFolderNameTEMP + dstrMCCLogTime.Substring(2, 6) + @"\I";
                                    if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath)) == false)
                                    {
                                        Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath));
                                    }

                                    if (File.Exists(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, string.Format("I-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime))) == false)
                                    {
                                        this.parrMCCFS_I[pnSubUnit] = new FileStream(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, string.Format("I-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime)), FileMode.Create, FileAccess.Write, FileShare.Write);
                                        this.pArrstrFileName_I[pnSubUnit] = PInfo.All.MCCLootFilePath + @"\" + this.pstrFolderPath + @"\" + string.Format("I-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime);
                                    }
                                    else
                                    {
                                        this.parrMCCFS_I[pnSubUnit] = new FileStream(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, string.Format("I-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime)), FileMode.Create, FileAccess.Write, FileShare.Write);
                                        this.pArrstrFileName_I[pnSubUnit] = PInfo.All.MCCLootFilePath + @"\" + this.pstrFolderPath + @"\" + string.Format("I-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime);
                                    }


                                    this.parrMCCSW_T[pnSubUnit] = new StreamWriter(this.parrMCCFS_T[pnSubUnit]);
                                    this.parrMCCSW_I[pnSubUnit] = new StreamWriter(this.parrMCCFS_I[pnSubUnit]);
                                    this.parrMCCSW_IDX[pnSubUnit] = new StreamWriter(this.parrMCCFS_IDX[pnSubUnit]);

                                    pnSubUnit++;
                                }
                            }
                            subWriteMCCLog("");
                            // 다음에 FTP 업로드할 파일명...기록.. 
                            this.pstrUploadFileDateTime = this.pstrFileDateTime;
                            pnSubUnit = 0;
                            for (int nUnitIndex = 1; nUnitIndex <= PInfo.UnitCount; nUnitIndex++)
                            {
                                for (int nSubUnitIndex = 1; nSubUnitIndex <= PInfo.Unit(nUnitIndex).SubUnitCount; nSubUnitIndex++)
                                {
                                    this.pArrstrUploadFileName_T[pnSubUnit] = this.pArrstrFileName_T[pnSubUnit];
                                    this.pArrstrUploadFileName_I[pnSubUnit] = this.pArrstrFileName_I[pnSubUnit];
                                    pnSubUnit++;
                                }
                            }
                    }
                }
                catch (Exception ex)
                {
                    PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                }
                #endregion

                // 사용중인 LOG FILE 이름 + System.ini에 설정된 Interval <= 현재시간 일 경우 새 파일 생성
                if (DateTime.ParseExact(this.pstrFileDateTime, "yyyyMMddHHmmss", null).AddMinutes(this.PInfo.All.MCCFileUploadTime) <= DateTime.ParseExact(dstrDateNowTime, "yyyyMMddHHmmss", null))
                {
                    dstrDateNowTime = DateTime.Now.ToString("yyyyMMddHHmm") + "00";
                    this.pstrUploadFileDateTime = this.pstrFileDateTime;
                    this.pstrUploadFileName_T = @"\T-" + this.PInfo.EQP("Main").EQPID + "-" + this.pstrFileDateTime + ".csv";
                    this.pstrUploadFileName_I = @"\I-" + this.PInfo.EQP("Main").EQPID + "-" + this.pstrFileDateTime + ".csv";
                    this.pstrUploadDateFolder = this.pstrDateFolder;

                    this.pstrFileDateTime = dstrDateNowTime;
                    this.pstrDateFolder = this.PInfo.All.MCCLootFilePath + @"\" + this.PInfo.Unit(0).SubUnit(0).ModuleID + "\\" + this.pstrFileDateTime.Substring(2, 6);
                    this.pstrFileName_T = @"\T-" + this.PInfo.EQP("Main").EQPID + "-" + this.pstrFileDateTime + ".csv";
                    this.pstrFileName_I = @"\I-" + this.PInfo.EQP("Main").EQPID + "-" + this.pstrFileDateTime + ".csv";

                    #region stream 재설정 부분 (Add by HS)
                    for (int nloop = 0; nloop < this.parrMCCFS_T.Length; nloop++)
                    {
                        if (this.parrMCCSW_T[nloop] != null)
                        {
                            this.parrMCCSW_T[nloop].Flush();
                            this.parrMCCSW_T[nloop].Close();
                            this.parrMCCSW_T[nloop] = null;
                        }
                        if (this.parrMCCFS_T[nloop] != null)
                        {
                            this.parrMCCFS_T[nloop].Close();
                            this.parrMCCFS_T[nloop] = null;
                        }

                        if (this.parrMCCSW_I[nloop] != null)
                        {
                            this.parrMCCSW_I[nloop].Flush();
                            this.parrMCCSW_I[nloop].Close();
                            this.parrMCCSW_I[nloop] = null;
                        }
                        if (this.parrMCCFS_I[nloop] != null)
                        {
                            this.parrMCCFS_I[nloop].Close();
                            this.parrMCCFS_I[nloop] = null;
                        }

                        if (this.parrMCCSW_IDX[nloop] != null)
                        {
                            this.parrMCCSW_IDX[nloop].Flush();
                            this.parrMCCSW_IDX[nloop].Close();
                            this.parrMCCSW_IDX[nloop] = null;
                        }
                        if (this.parrMCCSW_IDX[nloop] != null)
                        {
                            this.parrMCCSW_IDX[nloop].Close();
                            this.parrMCCSW_IDX[nloop] = null;
                        }
                    }
                    #endregion

                    int nMCCModuleCount = 0;
                    #region //[2015/03/13] 파일 생성 및 파일 저장 부분(Add by HS)
                    for (int nUnitIndex = 1; nUnitIndex <= PInfo.UnitCount; nUnitIndex++)
                    {
                        for (int nSubUnitIndex = 1; nSubUnitIndex <= PInfo.Unit(nUnitIndex).SubUnitCount; nSubUnitIndex++)
                        {
                            dstrMCCLogTime = this.pstrFileDateTime;

                            this.pstrFolderNameTEMP = "";
                            this.pstrFolderPath = "";
                            this.parrFolder = this.PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID.Split('_');

                            for (int dintLoop = 0; dintLoop < this.parrFolder.Length; dintLoop++)
                            {
                                this.pstrFolderNameTEMP += this.parrFolder[dintLoop];

                                //[2015/04/13]MCC담당자 요청으로 Layer1 삭제(Add by HS)
                                if (dintLoop == 1)
                                {
                                    this.pstrFolderNameTEMP += "_";
                                    this.pstrFolderPath += this.parrFolder[dintLoop] + "_";

                                    continue;
                                }
                                if (Directory.Exists(string.Format("{0}\\{1}", this.pstrFolderPath, this.pstrFolderNameTEMP)) == false)
                                {
                                    Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderNameTEMP));
                                }
                                this.pstrFolderNameTEMP += @"\";
                                this.pstrFolderPath += this.parrFolder[dintLoop] + "_";
                                if (dintLoop < this.parrFolder.Length - 1)
                                {
                                    this.pstrFolderNameTEMP += this.pstrFolderPath;
                                }
                            }

                            this.pstrFolderPath = this.pstrFolderNameTEMP + "index";
                            if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath)) == false)
                            {
                                Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath));
                            }

                            pstrIDXFileName = string.Format("index-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, pstrFileDateTime);
                            if (File.Exists(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, pstrIDXFileName)) == false)
                            {
                                this.parrMCCFS_IDX[nMCCModuleCount] = new FileStream(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath + @"\" + pstrIDXFileName), FileMode.Create, FileAccess.Write, FileShare.Write);
                                this.pArrstrIDXFileName[nMCCModuleCount] = PInfo.All.MCCLootFilePath + @"\" + this.pstrFolderPath + @"\" + pstrIDXFileName;
                            }

                            this.pstrFolderPath = this.pstrFolderNameTEMP + dstrMCCLogTime.Substring(2, 6);
                            if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath)) == false)
                            {
                                Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath));
                            }

                            strDateTime = pstrFileDateTime;

                            this.pstrFolderPath = this.pstrFolderNameTEMP + dstrMCCLogTime.Substring(2, 6) + @"\T";
                            if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath)) == false)
                            {
                                Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath));
                            }

                            if (File.Exists(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, string.Format("T-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime))) == false)
                            {
                                this.parrMCCFS_T[nMCCModuleCount] = new FileStream(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, string.Format("T-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime)), FileMode.Create, FileAccess.Write, FileShare.Write);
                                this.pArrstrFileName_T[nMCCModuleCount] = PInfo.All.MCCLootFilePath + @"\" + this.pstrFolderPath + @"\" + string.Format("T-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime);
                            }

                            this.pstrFolderPath = this.pstrFolderNameTEMP + dstrMCCLogTime.Substring(2, 6) + @"\I";
                            if (Directory.Exists(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath)) == false)
                            {
                                Directory.CreateDirectory(string.Format("{0}\\{1}", PInfo.All.MCCLootFilePath, this.pstrFolderPath));
                            }

                            if (File.Exists(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, string.Format("I-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime))) == false)
                            {
                                this.parrMCCFS_I[nMCCModuleCount] = new FileStream(string.Format("{0}\\{1}\\{2}", PInfo.All.MCCLootFilePath, this.pstrFolderPath, string.Format("I-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime)), FileMode.Create, FileAccess.Write, FileShare.Write);
                                this.pArrstrFileName_I[nMCCModuleCount] = PInfo.All.MCCLootFilePath + @"\" + this.pstrFolderPath + @"\" + string.Format("I-{0}-{1}.csv", PInfo.Unit(nUnitIndex).SubUnit(nSubUnitIndex).ModuleID, strDateTime);
                            }

                            this.parrMCCSW_T[nMCCModuleCount] = new StreamWriter(this.parrMCCFS_T[nMCCModuleCount]);
                            this.parrMCCSW_I[nMCCModuleCount] = new StreamWriter(this.parrMCCFS_I[nMCCModuleCount]);
                            this.parrMCCSW_IDX[nMCCModuleCount] = new StreamWriter(this.parrMCCFS_IDX[nMCCModuleCount]);

                            nMCCModuleCount++;
                        }
                    }
                    #endregion

                    // 새파일 생성
                    subWriteMCCLog("");

                    #region 업로드 DB 기록
                    if (this.PInfo.All.MCCFileUploadUse)
                    {
                        // DB에 업로딩 중임을 기록.
                        UploadIngFlag = true;

                        if (this.PInfo.All.pblUseMDB)
                        {
                            try
                            {
                                if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Upload`= 'True', `Ing`='True' WHERE `FileName`='{0}';", pstrUploadFileName.Substring(1)));
                                //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Upload`= 'True', `Ing`='True' WHERE `FileName`='{0}';", pstrUploadFileName.Substring(1).Insert(0, "index-")));

                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Upload`= 'True', `Ing`='True' WHERE `FileName`='{0}';", pstrUploadFileName_T.Substring(1));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Upload`= 'True', `Ing`='True' WHERE `FileName`='{0}';", pstrUploadFileName_T.Substring(1).Insert(0, "index-"));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();

                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Upload`= 'True', `Ing`='True' WHERE `FileName`='{0}';", pstrUploadFileName_I.Substring(1));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Upload`= 'True', `Ing`='True' WHERE `FileName`='{0}';", pstrUploadFileName_I.Substring(1).Insert(0, "index-"));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();

                                DBAct.clsDBAct.MCCcommand.Transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                                DBAct.clsDBAct.MCCcommand.Transaction.Rollback();
                            }
                        }

                        nMCCModuleCount = 0;
                        //for (int nUnitIndex = 1; nUnitIndex <= PInfo.UnitCount; nUnitIndex++)
                        //{
                        //    for (int nSubUnitIndex = 1; nSubUnitIndex <= PInfo.Unit(nUnitIndex).SubUnitCount; nSubUnitIndex++)
                        //    {
                        //        pFTP.subFTPFunc_2("Both", this.PInfo.All.MCCNetworkPath, this.pstrUploadDateFolder, this.pArrstrUploadFileName_T[nMCCModuleCount], this.pArrstrUploadFileName_I[nMCCModuleCount], this.pstrUploadFileDateTime);
                        //        nMCCModuleCount++;
                        //    }
                        //}

                        //pFTP.FtpUploadStart();

                        pFTP.subFTPFunc_2("Both", this.PInfo.All.MCCNetworkPath, this.pstrUploadDateFolder, this.pArrstrUploadFileName_T[nMCCModuleCount], this.pArrstrUploadFileName_I[nMCCModuleCount], this.pstrUploadFileDateTime);
                    }
                    #endregion

                    nMCCModuleCount = 0;
                    for (int nUnitIndex = 1; nUnitIndex <= PInfo.UnitCount; nUnitIndex++)
                    {
                        for (int nSubUnitIndex = 1; nSubUnitIndex <= PInfo.Unit(nUnitIndex).SubUnitCount; nSubUnitIndex++)
                        {
                            this.pArrstrUploadFileName_T[nMCCModuleCount] = this.pArrstrFileName_T[nMCCModuleCount];
                            this.pArrstrUploadFileName_I[nMCCModuleCount] = this.pArrstrFileName_I[nMCCModuleCount];
                            nMCCModuleCount++;
                        }
                    }
                    //pstrUploadFileName_T = pstrFileName_T;
                    //pstrUploadFileName_I = pstrFileName_I;
                    //pstrUploadDateFolder = pstrDateFolder;

                    // 지울놈 지우자
                    subDeleteMCCLogFile();

                    //[2015/06/09]
                    pEQP.subMemoryLog();
                }



                // MMC Log Write
                dstrData = "";
                int dintLogCount = this.PInfo.funGetMCCCLogCount();

                while (dintLogCount-- > 0)
                {
                    dstrData = this.PInfo.funGetMCCLog();     //방금 Queue에서 읽은 내용을 삭제한다.
                    if (dstrData.Trim() != "")
                    {
                        subWriteMCCLog(dstrData);
                    }
                }

                #region "업로드 실패한 파일 재 업로드"

                if (this.PInfo.All.pblUseMDB && pintCheckCount <= 0 && !UploadIngFlag)  // 5분에 한번?
                {
                    //System.Diagnostics.Debug.WriteLine("TimeCheck!!!");

                    pintCheckCount = pintCheckInterval;
                    // 디비에서 확인후... 한개씩 작업...
                    // @"SELECT * FROM `tbMCCfile` WHERE `Upload`='True' AND `Ing`='False' ORDER BY `Index`;"
                    // 이 쿼리문 결과 카운트가 1 이상이면..
                    // 첫번째 것만 전송 시도...

                    if (this.PInfo.All.MCCFileUploadUse && this.PInfo.All.pblUseMDB)
                    {
                        try
                        {
                            if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();

                            // 디비에서..  `Upload`='True' AND `Ing`='True' 인건.... 잘못 된거다..
                            //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, @"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `Upload`='True' AND `Ing`='True';");
                            DBAct.clsDBAct.MCCcommand.CommandText = @"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `Upload`='True' AND `Ing`='True';";
                            DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();


                            DBAct.clsDBAct.MCCcommand.Transaction.Commit();

                            // 디비에서.. 현재 작성중인 파일을 제외 하고는 `Upload`='False' AND `Ing`='False' 인건.... 잘못 된거다..
                            // DataTable dt = DBAct.clsDBAct.funSelectQuery(DBAct.clsDBAct.pstrMCCConnection, @"SELECT * FROM `tbMCCfile` WHERE `Upload`='False' AND `Ing`='False' ORDER BY `Index`;");

                            DBAct.clsDBAct.MCCcommand.CommandText = @"SELECT * FROM `tbMCCfile` WHERE `Upload`='False' AND `Ing`='False' ORDER BY `Index`;";
                            OleDbDataAdapter dAdapter = new OleDbDataAdapter(DBAct.clsDBAct.MCCcommand);
                            DataTable dt = new DataTable();
                            dAdapter.Fill(dt);
                            dAdapter.Dispose();

                            if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if ((dr["FileName"].ToString().Contains(pstrFileName_T.Substring(1))) == false)
                                    {
                                        DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Upload`='True' WHERE `Index`={0};", dr["Index"].ToString());
                                        DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();


                                        //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Upload`='True' WHERE `Index`={0};", dr["Index"].ToString()));
                                    }

                                    if ((dr["FileName"].ToString().Contains(pstrFileName_I.Substring(1))) == false)
                                    {
                                        DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Upload`='True' WHERE `Index`={0};", dr["Index"].ToString());
                                        DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();


                                        //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Upload`='True' WHERE `Index`={0};", dr["Index"].ToString()));
                                    }
                                }

                                DBAct.clsDBAct.MCCcommand.Transaction.Commit();
                            }

                            //dt = DBAct.clsDBAct.funSelectQuery(DBAct.clsDBAct.pstrMCCConnection, @"SELECT * FROM `tbMCCfile` WHERE `Upload`='True' AND `Ing`='False' ORDER BY `Index`;");


                            DBAct.clsDBAct.MCCcommand.CommandText = @"SELECT * FROM `tbMCCfile` WHERE `Upload`='True' AND `Ing`='False' ORDER BY `Index`;";
                            dAdapter = new OleDbDataAdapter(DBAct.clsDBAct.MCCcommand);
                            dt.Clear();
                            dAdapter.Fill(dt);
                            dAdapter.Dispose();

                            if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                // 30일 이상 지난 건 포기 하자..
                                DateTime dtDueDate = DateTime.ParseExact(DateTime.Now.ToString("yyMMdd"), "yyMMdd", null).AddDays(pintMCCKeepDays);
                                bool dblStop = false;

                                for (int dintLoop = 0; dintLoop < dt.Rows.Count && !dblStop; dintLoop++)
                                {

                                    long dlngIndex = Convert.ToInt64(dt.Rows[dintLoop]["Index"]);
                                    string strCreateDate = dt.Rows[dintLoop]["CreateDate"].ToString();
                                    string strFileName = dt.Rows[dintLoop]["FileName"].ToString();
                                    string strFileType = dt.Rows[dintLoop]["FileType"].ToString();

                                    DateTime dtCrateDate = DateTime.ParseExact(strCreateDate, "yyMMdd", null);

                                    if (dtCrateDate < dtDueDate)
                                    {
                                        //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `Index`={0};", dlngIndex));

                                        DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `Index`={0};", dlngIndex);
                                        DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        string dstrPath = (strFileType == "LOG") ? strCreateDate : "index";
                                        string dstrFileName = pstrMCCbaseFolder + "\\" + dstrPath + "\\" + strFileName;

                                        if (File.Exists(dstrFileName))
                                        {
                                            UploadIngFlag = true;
                                            dblStop = true;

                                            //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Ing`='True' WHERE `Index`={0};", dlngIndex));

                                            DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Ing`='True' WHERE `Index`={0};", dlngIndex);
                                            DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();


                                            pFTP.subFTPFunc(strFileType, this.PInfo.All.MCCNetworkPath, strCreateDate, pstrMCCbaseFolder + "\\" + strCreateDate, "\\" + strFileName);
                                        }
                                        else
                                        {
                                            //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `Index`={0};", dlngIndex));

                                            DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `Index`={0};", dlngIndex);
                                            DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();

                                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("{0} File Not Found. Delete Record From Database.", dstrFileName));
                                        }
                                    }

                                }

                                dt.Clear();
                                dt.Dispose();
                            }

                            DBAct.clsDBAct.MCCcommand.Transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                            DBAct.clsDBAct.MCCcommand.Transaction.Rollback();
                        }
                    }
                }

                pintCheckCount--;
                #endregion
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());

            }
            finally
            {
                ptmrMCCLogUpdate.Enabled = true;
            }
        } 

        #endregion
    }
}

