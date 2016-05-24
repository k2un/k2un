﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace LogAct
{
    public partial class frmLOTAPD : frmCommListView
    {
        //변수 선언
        #region "변수 선언"

        private DataTable pdtLOTAPDTable;                        //알람 리스트 데이터 테이블
        private DataTable pdtUsingTable;                        //화면에 표시할 편집된 테이블
        private string[][] pstrFileContents;                    //텍스트 파일에서 읽은 데이터를 저장하는 변수
        private int pintArrIndex = 0;                           //스트링 배열의 첫번째 인덱스

        private int pintAllRows = 0;                             //생성된 원본 알람 테이블의 총 Row 수
        private int pintAllPage = 0;                            //원본 알람 테이블을 화면에 표시할 행수로 나누었을때 생성되야할 테이블 수
        private int pintLastPageRows = 0;                       //마지막 화면의 행수

        private bool pbodIsFileContents = false;                //텍스트 파일에서 읽은 것이 있는 가를 판단하는 변수

        #endregion

        //자동 실행 함수
        #region "자동 실행 함수"

        //*******************************************************************************
        //  Function Name : frmAlarmList()
        //  Description   : 폼 로드시에 실행되는 함수
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/01/31          어 경태         [L 00] 
        //*******************************************************************************
        public frmLOTAPD()
        {
            //초기화
            InitializeComponent();
            subInit();
        }

        #endregion

        //초기화 함수
        #region "초기화 함수"

        //*******************************************************************************
        //  Function Name : subInit()
        //  Description   : 초기화를 실행
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 데이터 테이블을 만들고 컬럼을 생성한다.
        //*******************************************************************************
        //  2007/03/09          최 성 원        [L 00] 
        //*******************************************************************************
        private void subInit()
        {
            try
            {
                //스트링 배열 생성
                pstrFileContents = new string[30][];

                //테이블 생성
                pdtUsingTable = new DataTable();
                this.subInitTable();
            }

            catch (Exception)
            {
                throw;
            }     
        }

        //*******************************************************************************
        //  Function Name : subInitTable()
        //  Description   : 데이터 테이블에 컬럼값을 넣는다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/22          박 근 태        [L 00] 
        //*******************************************************************************
        private void subInitTable()
        {
            try
            {
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        //로딩시 실행되는 함수
        #region subFormLoad

        //*******************************************************************************
        //  Function Name : subFormLoad()
        //  Description   : Base 폼의 Load를 호출하여 폼을 초기화 한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 폼이 안보이다가 보이게 될 경우 실행된다.
        //*******************************************************************************
        //  2007/01/31          어 경태         [L 00]
        //  2007/03/09          최 성원
        //  2007/03/22          박 근태
        //*******************************************************************************
        public void subFormLoad()
        {
            string dstrLOTName = "";        //DB에 추가되는 컬럼네임을 LOTAPD테이블에 추가하기 위한 변수.
            int dintLOTAPDCnt = 0;
            
            try
            {
                //DB에 등록되 있는 LOTAPD의 갯수를 가져온다. 
                dintLOTAPDCnt = PInfo.Unit(0).SubUnit(0).LOTAPDCount;
                //CommonListView (부모) 함수를 호출한다.
                subFormLoad("LOTAPD");

                //컬럼헤더의 높이값을 정한다.
                grdAlarmList.ColumnHeadersHeight = 27;

                
                
                //LOTAPTCount가 달라지면 테이블의 컬럼을 새로 만든다.
                //if (dintLOTAPDTmp != dintLOTAPDCnt)
                //{
                //    this.subInitTable();
                //DB에 있는 LOTAPD항목을 pdtLOTAPdtable의 컬럼에 추가. 
                pdtLOTAPDTable = new DataTable();
                for (int dintLOTCount = 1; dintLOTCount <= PInfo.Unit(0).SubUnit(0).LOTAPDCount; dintLOTCount++)
                {
                    dstrLOTName = PInfo.Unit(0).SubUnit(0).LOTAPD(dintLOTCount).Name;
                    pdtLOTAPDTable = PclsDataGridView.funAddColumn(pdtLOTAPDTable, dstrLOTName, dstrLOTName);
                }
                //    dintLOTAPDTmp = dintLOTAPDCnt;
                //}
                //데이터 테이블을 만들어서 화면에 표시한다.
                this.subDisplay();

                //폼을 표시한다.
                this.Show();
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        //화면 표시에 관련된 함수
        #region "화면 표시에 관련된 함수"

        #region subDisplay

        //*******************************************************************************
        //  Function Name : subDisplay()
        //  Description   : 데이터 테이블을 만들어서 화면에 표시한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/09          최 성 원         [L 00] 
        //*******************************************************************************
        public override void subDisplay()
        {
            try
            {
                //알람 로그 파일을 읽어서 데이터 테이블로 만든다.
                subMakeLOTAPDTable();

                //라벨을 활성,비활성화 시킨다.
                subInitialLabel();

                //그리드를 표시한다.
                this.subDisplayOutput();
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region subMakeAlarmTable

        //*******************************************************************************
        //  Function Name : subMakeAlarmTable()
        //  Description   : 로그 파일에서 데이터를 읽어서 알람 리스트 테이블을 만든다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/09          최 성 원        [L 00] 
        //*******************************************************************************
        private void subMakeLOTAPDTable()
        {
            try 
            {	        
                //로그 파일에서 데이터를 읽어서 스트링 배열로 만든다.
                subMakeFileString();

                //알람 리스트 테이블을 만든다.
                subMakeTable();
            }

	        catch (Exception)
	        {
		        throw;
	        }
        }

        #endregion

        #region subDisplayOutput

        //*******************************************************************************
        //  Function Name : subDisplayOutput()
        //  Description   : 화면을 표시한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/09          최 성 원         [L 00] 
        //*******************************************************************************
        public override void subDisplayOutput()
        {
            try
            {
                //편집된 데이터 테이블을 만든다.
                subMakeUsingTable();

                //화면에 표시한다.
                grdAlarmList.DataSource = pdtUsingTable;

                //// 열너비를 수동조정해 GridView의 너비에 맞춘다.
                for (int i = 0; i < grdAlarmList.ColumnCount; i++)
                {
                    DataGridViewColumn column = grdAlarmList.Columns[i];
                    switch (i)
                    {
                        case 0:
                            column.Width = 183;
                            break;
                        case 1:
                            column.Width = 183;
                            break;
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #endregion

        //데이터 테이블에 관련된 함수
        #region "데이터 테이블에 관련된 함수"

        #region subMakeFileString

        //*******************************************************************************
        //  Function Name : subMakeFileString()
        //  Description   : 로그 파일에서 데이터를 읽어서 스트링 배열로 만든다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/09          최 성 원         [L 00] 
        //*******************************************************************************
        private void subMakeFileString()
        {
            //지역 변수 선언
            string dstrFilePath;                        //파일의 경로 저장
            string dstrDateFrom;                        //로그를 볼 시작 날짜
            string dstrDateTo;                          //로그를 볼 마지막 날짜
            DateTime myDate;                            //날짜를 저장할 임시 변수

            string dstrChangeDate;                      //날짜의 형태를 변경시킬 변수 2007-01-01 => 20070101
            string[] dstrArrayDateFrom;                 //날짜 변환을 위한 split용 임시 저장변수
            string[] dstrArrayDateTo;                   //날짜 변환을 위한 split용 임시 저장변수

            string[] dstrArrayFileContent;              //데이터를 읽기 위한 임시 변수
            int dintArrIndex = 0;                       //스트링 배열의 첫번째 인덱스

            string dstrYear;                            //년도
            string dstrMonth;                           //월
            string dstrDay;                             //일

            bool dbolLastDay = false;                   //반복문을 벗어나기 위한 변수

            string[] dstrFileDataTmp;                      //텍스트에서 읽어온 데이터를 임시로 저장할 변수
            string[] dstrFileData = new string[30];        //텍스트에서 읽어온 데이터를 임시로 저장할 변수

            string[] dstrsplit = { String.Empty.PadLeft(6, ' ') };
            string dstrFileTemp = "";                      //파일을 스플릿해서 담을 변수.(1,2,3,4,5,)

            int dintFirst = 0;

            try
            {
                //시작 날짜 저장
                dstrChangeDate = PstrDateFrom;
                dstrArrayDateFrom = dstrChangeDate.Split('-');
                dstrDateFrom = dstrArrayDateFrom[0].Trim() + dstrArrayDateFrom[1].Trim() + dstrArrayDateFrom[2].Trim();

                //마지막 날짜 저장
                dstrChangeDate = PstrDateTo;
                dstrArrayDateTo = dstrChangeDate.Split('-');
                dstrDateTo = dstrArrayDateTo[0].Trim() + dstrArrayDateTo[1].Trim() + dstrArrayDateTo[2].Trim();

                //총 행수 리셋
                pintAllRows = 0;

                //이차원 배열 초기화
                for(int day = 0; day < 30; day ++)
                {
                    pstrFileContents[day] = null;
                }

                //스트링 배열의 인덱스 초기화
                pintArrIndex = 0;

                //선택한 From날짜와 to 날짜가 같은때
                if (dstrDateFrom == dstrDateTo)
                {
                    //파일 경로를 읽어서
                    dstrFilePath = Application.StartupPath + "\\" + "PLCLOG" + "\\" + dstrDateFrom + "\\" + "LOTAPD.Log";

                    //파일이 존재하면
                    if (File.Exists(dstrFilePath))
                    {
                        //파일의 모든 라인을 가져와 스트링 배열에 저장한다
                        dstrArrayFileContent = File.ReadAllLines(dstrFilePath);

                        //총 Row 수에 현재 Row 수를 더한다.
                        pintAllRows = pintAllRows + dstrArrayFileContent.Length;
                        dstrFileDataTmp = new string[pintAllRows];
                        //원본 데이터 스트링에 더한다.
                        for (int dintLoop = 0; dintLoop < dstrArrayFileContent.Length; dintLoop++)
                        {
                            dstrFileData = null;
                            //dstrFileData = new string[dstrArrayFileContent[dintLoop].Length / 32];
                            dstrFileData = new string[PInfo.Unit(0).SubUnit(0).LOTAPDCount];
                            //for (int dintDataLoop = 0; dintDataLoop < (dstrArrayFileContent[dintLoop].Length / 32); dintDataLoop++)
                            for(int dintDataLoop = 0; dintDataLoop < PInfo.Unit(0).SubUnit(0).LOTAPDCount; dintDataLoop++)
                            {
                                dintFirst = dintDataLoop * 32;
                                dstrFileData[dintDataLoop] = dstrArrayFileContent[dintLoop].Substring(dintFirst, 32).Trim();
                            }
                            //  읽어들인 파일의 문장별로 9개의 값만 가져온다.EX(1,2,3,4,5,6,7,8,9,10,11...)에서 9까지.. 
                            for (int dintArrayLoop = 0; dintArrayLoop < dstrFileData.Length; dintArrayLoop++)
                            {
                                //마지막 스트링은 컴마를 찍지 않는다.
                                if (dintArrayLoop == dstrFileData.Length-1)
                                {
                                    dstrFileTemp = dstrFileTemp + dstrFileData[dintArrayLoop].Trim();
                                }
                                else
                                    dstrFileTemp = dstrFileTemp + dstrFileData[dintArrayLoop].Trim() + ",";
                            }
                            dstrFileDataTmp[dintLoop] = dstrFileTemp;
                            dstrFileTemp = "";

                        }
                        pstrFileContents[dintArrIndex] = dstrFileDataTmp;

                        //인덱스를 저장한다.
                        pintArrIndex = dintArrIndex + 1;

                        //데이터가 있다는 플레그를 셋팅한다.
                        pbodIsFileContents = true;
                    }
                }

                //선택한 From날짜와 to 날짜가 다를때
                else
                {
                    //Alarm Display 시작날짜가 마지막날짜보다 크면
                    if (int.Parse(dstrDateFrom) > int.Parse(dstrDateTo))
                    {
                        MessageBox.Show("Select Again From StartDate To EndDate!!", "INFORM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //알람열람일수는 30일로 제한
                    if ((int.Parse(dstrDateTo) - int.Parse(dstrDateFrom)) > 31)
                    {
                        MessageBox.Show("Select Date Within One Month!", "INFORM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //선택한 From날짜와 To날짜가 정상적으로 범위가 있을때
                    if (int.Parse(dstrDateFrom) < int.Parse(dstrDateTo))
                    {
                        do
                        {
                            //파일 경로를 읽어서
                            dstrFilePath = Application.StartupPath + "\\" + "PLCLOG" + "\\" + dstrDateFrom + "\\" + "LOTAPD.Log";

                            //파일이 있으면
                            if (File.Exists(dstrFilePath))
                            {
                                //파일의 모든 라인을 가져와 스트링 배열에 저장한다
                                dstrArrayFileContent = File.ReadAllLines(dstrFilePath);

                                //총 Row 수에 현재 Row 수를 더한다.
                                pintAllRows = pintAllRows + dstrArrayFileContent.Length;
                                dstrFileDataTmp = new string[dstrArrayFileContent.Length];
                                for (int dintLoop = 0; dintLoop < dstrArrayFileContent.Length; dintLoop++)
                                {
                                    dstrFileData = null;
                                    //dstrFileData = new string[dstrArrayFileContent[dintLoop].Length / 32];
                                    dstrFileData = new string[PInfo.Unit(0).SubUnit(0).LOTAPDCount];
                                    for (int dintDataLoop = 0; dintDataLoop < PInfo.Unit(0).SubUnit(0).LOTAPDCount; dintDataLoop++)
                                    {
                                        dintFirst = dintDataLoop * 32;
                                        dstrFileData[dintDataLoop] = dstrArrayFileContent[dintLoop].Substring(dintFirst, 32).Trim();
                                    }
                                    //  읽어들인 파일의 문장별로 9개의 값만 가져온다.EX(1,2,3,4,5,6,7,8,9,10,11...)에서 9까지.. 
                                    for (int dintArrayLoop = 0; dintArrayLoop < dstrFileData.Length; dintArrayLoop++)
                                    {
                                        //마지막 스트링은 컴마를 찍지 않는다.
                                        if (dintArrayLoop == dstrFileData.Length - 1)
                                        {
                                            dstrFileTemp = dstrFileTemp + dstrFileData[dintArrayLoop].Trim();
                                        }
                                        else
                                            dstrFileTemp = dstrFileTemp + dstrFileData[dintArrayLoop].Trim() + ",";
                                    }
                                    dstrFileDataTmp[dintLoop] = dstrFileTemp;
                                    dstrFileTemp = "";
                                }
                                pstrFileContents[dintArrIndex] = dstrFileDataTmp;

                                //원본 배열의 인덱스를 증가시킨다.
                                dintArrIndex = dintArrIndex + 1;
                            }

                            //반복을 하기 위한 날짜 설정
                            dstrYear = dstrDateFrom.Substring(0, 4);
                            dstrMonth = dstrDateFrom.Substring(4, 2);
                            dstrDay = dstrDateFrom.Substring(6, 2);

                            myDate = new DateTime(int.Parse(dstrYear), int.Parse(dstrMonth), int.Parse(dstrDay));

                            //날짜를 하루 증가 시킨다.
                            dstrDateFrom = (myDate.AddDays(1)).ToString("yyyyMMdd");

                            //시작 날짜가 마지막 날짜 보다 클때
                            if (int.Parse(dstrDateFrom) > int.Parse(dstrDateTo))
                            {
                                //데이터가 있다는 플레그를 셋팅한다.
                                pbodIsFileContents = true;

                                //반복문을 벗어나기 위해서 변수를 설정한다.
                                dbolLastDay = true;
                            }

                        } while (dbolLastDay == false);

                        //인덱스를 저장한다.
                        pintArrIndex = dintArrIndex;

                        //변수를 리셋한다.
                        dbolLastDay = false;
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region subMakeTable

        //*******************************************************************************
        //  Function Name : subMakeTable()
        //  Description   : 알람 리스트 테이블을 만든다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/09          최 성 원        [L 00] 
        //*******************************************************************************
        private void subMakeTable()
        {
            try
            {
                if (pbodIsFileContents == true)
                {
                    //플레그를 리셋한다.
                    pbodIsFileContents = false;

                    //테이블 편집을 위한 데이터를 만든다.
                    subFindProperty();

                    //원본 데이터 테이블을 만든다.
                    subMakeOriginalTable();                    
                }
                //파일을 읽지 않았으면 총 페이지 수를 초기화 시킨다.
                else
                    PintAllPage = 0;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region subFindProperty

        //*******************************************************************************
        //  Function Name : subFindProperty()
        //  Description   : 테이블 편집을 위한 데이터를 만든다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/11          최 성 원        [L 00] 
        //*******************************************************************************
        private void subFindProperty()
        {
            //지역 변수 선언
            int dintAllRows = 0;        //원본 테이블의 총행수를 저장할 임시 변수
            int dintDisplayRows = 0;    //화면에 표시할 행수를 저장할 임시 변수
            int dintRemindRows = 0;     //마지막 화면의 행수

            try
            {
                //지역 변수 할당
                dintAllRows = pintAllRows;
                dintDisplayRows = PintPageRows;
                dintRemindRows = dintAllRows % dintDisplayRows;

                //마지막 화면이 행수가 모자르지 않고 꽉 차면
                if (dintRemindRows == 0)
                {
                    //페이지 수 저장
                    pintAllPage = dintAllRows / dintDisplayRows;
                }

                //마지막 화면이 행수가 모자르면
                else
                {
                    //페이지 수 저장 ( 나머지가 있기 때문에 1을 더한다.)
                    pintAllPage = (dintAllRows / dintDisplayRows) + 1;
                }

                //마지막 페이지의 행수 저장
                pintLastPageRows = dintRemindRows;

                //현재 폼의 페이지 수를 저장한다.
                PintAllPage = pintAllPage;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region subMakeOriginalTable

        //*******************************************************************************
        //  Function Name : subMakeOriginalTable()
        //  Description   : 원본 알람 리스트 테이블을 만든다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/11          최 성 원        [L 00] 
        //*******************************************************************************
        private void subMakeOriginalTable()
        {
            //지역 변수 선언
            string[] dstrRowValue;              //데이터 테이블에 추가할 Row 값
            char dstrsplit = ',';               //데이터를 구분하기 위한 변수
            int dintRows = 0;                   //반복할 행수

            try
            {
                //알람 테이블 초기화
                if (pdtLOTAPDTable != null)
                {
                    pdtLOTAPDTable.Clear();
                }
                //원본 데이터 테이블을 만든다.
                for (int ArrayIndex = 0; ArrayIndex < pintArrIndex; ArrayIndex++)
                {
                    //반복할 
                    dintRows = pstrFileContents[ArrayIndex].Length;

                    for (int Rows = 0; Rows < dintRows; Rows++)
                    {
                        if (pstrFileContents[ArrayIndex][Rows] != null)
                        {
                            dstrRowValue = pstrFileContents[ArrayIndex][Rows].Trim().Split(dstrsplit);
                            pdtLOTAPDTable.Rows.Add(dstrRowValue);
                        }
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region subMakeUsingTable

        //*******************************************************************************
        //  Function Name : subMakeUsingTable()
        //  Description   : 편집된 데이터 테이블을 만든다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/11          최 성 원        [L 00] 
        //*******************************************************************************
        private void subMakeUsingTable()
        {
            //지역 변수 선언
            DataTable ddtOriginal;              //원본 테이블을 저장할 임시 변수
            DataTable ddtUsing;                 //편집할 테이블을 저장할 임시 변수
            int dintStartRows = 0;              //시작할 행의 인덱스
            int dintRowsNumber = 0;             //반복문에서 행을 저장할 임시 변수
            int dintLoopNumber = 0;             //반복횟수

            try
            {
                //인덱스(클릭한 페이지 넘버) 가 허용된 테이블 수를 넘으면
                if (PintIndex > pintAllPage)
                {
                    return;
                }

                //임시 테이블 만들기
                ddtOriginal = pdtLOTAPDTable;
                ddtUsing = ddtOriginal.Clone();

                //반복문을 위한 인수 구하기
                dintStartRows = pintAllRows - (PintPageRows * PintIndex) + PintPageRows - 1;
                dintRowsNumber = dintStartRows;
                
                //반복 횟수 구하기
                dintLoopNumber = PintPageRows;

                //만약 마지막 페이지이면
                if (PintIndex == pintAllPage)
                {
                    dintLoopNumber = pintLastPageRows;
                }

                //편집 테이블 구하기
                for (int Rows = 0; Rows < dintLoopNumber; Rows++)
                {
                    //행이 음수이면 그만둔다.
                    if (dintRowsNumber < 0)
                    {
                        break;
                    }

                    //편집 테이블에 행을 추가한다.
                    ddtUsing.Rows.Add(ddtOriginal.Rows[dintRowsNumber].ItemArray);

                    //행을 감소 시킨다.
                    dintRowsNumber = dintRowsNumber - 1;

                }

                //편집 테이블을 저장한다.
                pdtUsingTable = ddtUsing;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion        

        #endregion
    }
}