using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;
using System.Globalization;

namespace LogAct
{
    public partial class frmCommListView : Form
    {
        //변수 선언
        #region "변수 선언"

        public InfoAct.clsInfo PInfo;                                   //clsInfo의 구조체를 참조하기 위한 변수
        public static CommonAct.FunDataGridView PclsDataGridView;       //FunDataGridView 를 참조하기 위한 변수

        public frmAlarmList PfrmAlarmList;                              //frmAlarmList 를 참조하기 위한 변수
        public frmGLSAPD PfrmGLSAPD;                                    //frmGLSAPD 를 참조하기 위한 변수
        public frmLOTAPD PfrmLOTAPD;                                    //frmLOTAPD 를 참조하기 위한 변수

        public static string PstrDateFrom;                              //보여줄 시작 날짜 저장
        public static string PstrDateTo;                                //보여줄 마지막 날짜 저장

        public static string PstrName;                                  //현재 화면의 이름
        public static int PintIndex = 1;                                //클릭된 화면 페이지 넘버
        public static int PintAllPage = 0;                              //현재 화면의 페이지 수
        public static int PintPageRows = 20;                            //화면에 보여줄 행수        
        
        private DateTime pdtmTime;                                      //시간과 날짜를 계산하기 위한 변수

        private Label[] plblNum = new Label[10];                        //라벨을 만들기 위한 배열        
        private int pintNextCount = 0;                                  //Next 나 Previous 버튼을 눌렀을 때 화면을 초기화 하기 위한 변수
        private static int pintNextControl = 0;                         //Next를 제한하기 위한 변수. pintNextCount가 이 변수와 같을 때까지 next를 누를수 있다.
                                                                        // 예) 7 페이지 였을 경우 Next 버튼을 눌렀을때 11일 되어야 한다.
                                   
        private int pintBeforeText = 0;                                 //그 전에 라벨 text(Convert)값을 저장한다.
        private Color pColorTemp;                                       //라벨의 변화를 주기 위함.

        private static string pstrTempDateFrom = "";
        private static string pstrTempDateTo = "";

        public static int PintComboIndex = 0;                           //GLSAPD만 사용/콤보 박스의 인덱스 값을 가져옴.

        #endregion

        //생성자 선언
        #region "생성자 선언"

        //*******************************************************************************
        //  Function Name : frmCommListView()
        //  Description   : 폼 생성시 실행되는 함수
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/01/31          어 경태         [L 00] 
        //*******************************************************************************
        public frmCommListView()
        {
            try
            {
                //스레드 출동을 막기 위하여
                CheckForIllegalCrossThreadCalls = false;
    
                pdtmTime = new DateTime();
                //초기화
                InitializeComponent();
            }
            catch (Exception)
            {
                throw;
            }          
        }

        #endregion

        //화면 출력에 관한 함수
        #region "화면 출력에 관한 함수

        #region subFormLoad

        //*******************************************************************************
        //  Function Name : subFormLoad()
        //  Description   : 각 폼에서 From Load 시에 호출해서 필요한 작업을 수행
        //  Parameters    : strName -> 실행할 폼에 이름
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/18          박 근 태         [L 00] 
        //*******************************************************************************
        public void subFormLoad(string strName)
        {
            try
            {
                //폼 이름 저장
                PstrName = strName;

                //콤보박스 비활성화 GLSAPD일 경우만 활성화 된다.
                this.comUnitNum.Visible = false;
               
                //라벨 생성
                this.plblNum = new Label[] { this.lblNum1, this.lblNum2, this.lblNum3, this.lblNum4, this.lblNum5, this.lblNum6, this.lblNum7, this.lblNum8, this.lblNum9, this.lblNum10 };
                
                //next클릭시 11번째 라벨을 발강색으로 바꾸고 그 전 클릭된 라벨은 검은색으로 변경.
                this.plblNum[pintBeforeText].ForeColor = System.Drawing.Color.Black;
                this.plblNum[0].ForeColor = System.Drawing.Color.Red;
                pintBeforeText = 0;

                //날짜를 저장한다.
                subInitialTime();

                //폼에 이름을 표시한다.
                grbSearch.Text = "Seach " + PstrName;
                this.Name = PstrName;

                //뒤로 가는 화살표 비활성화
                this.lblPrevious.Enabled = false;
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region subDisplay

        //*******************************************************************************
        //  Function Name : subDisplay()
        //  Description   : 폼에서 로그 파일을 불러서 데이터 테이블을 만들고
        //                  화면에 표시한다. 
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 각 폼 클래스에서 재정의 한다.
        //*******************************************************************************
        //  2007/03/09          최 성 원         [L 00] 
        //*******************************************************************************
        public virtual void subDisplay()
        {
           
        }

        #endregion

        #region subDisplayOutput

        //*******************************************************************************
        //  Function Name : subDisplayOutput()
        //  Description   : 폼에서 데이터 테이블을 만들지 않고 화면에 표시한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 각 폼 클래스에서 재정의 한다.
        //*******************************************************************************
        //  2007/03/09          최 성 원         [L 00] 
        //*******************************************************************************
        public virtual void subDisplayOutput()
        {
        }

        #endregion

        #endregion

        //달력에 관한 함수들
        #region "달력에 관한 함수들"

        #region lblAlarmDateFrom_Click

        //*******************************************************************************
        //   Function Name : lblAlarmDateFrom_Click
        //   Description   : From 달력을 표시
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/02/06         박 근 태
        //*******************************************************************************
        private void lblAlarmDateFrom_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.calAlarmFrom.Visible == true)
                {
                    this.calAlarmFrom.Visible = false;
                }
                this.calAlarmFrom.Visible = true;
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region lblAlarmDateTo_Click

        //*******************************************************************************
        //   Function Name : lblAlarmDateTo_Click
        //   Description   : To 달력을 표시
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/02/06         박 근 태
        //*******************************************************************************
        private void lblAlarmDateTo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.calAlarmTo.Visible == true)
                {
                    this.calAlarmTo.Visible = false;
                }
                this.calAlarmTo.Visible = true;
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region calAlarmFrom_DateSelected

        //*******************************************************************************
        //   Function Name : calAlarmFrom_DateSelected
        //   Description   : 달력의 날짜를 선택
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/02/06         박 근 태
        //*******************************************************************************
        private void calAlarmFrom_DateSelected(object sender, DateRangeEventArgs e)
        {
            try
            {
                this.calAlarmFrom.Visible = false;
                this.lblAlarmDateFrom.Text = e.Start.ToShortDateString();
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region calAlarmTo_DateSelected

        //*******************************************************************************
        //   Function Name : calAlarmTo_DateSelected
        //   Description   : 달력의 날짜를 선택
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/02/06         박 근 태
        //*******************************************************************************
        private void calAlarmTo_DateSelected(object sender, DateRangeEventArgs e)
        {
            try
            {
                this.calAlarmTo.Visible = false;
                this.lblAlarmDateTo.Text = e.Start.ToShortDateString();
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #endregion

        //라벨에 관한 함수
        #region subInitialLabel

        //*******************************************************************************
        //   Function Name : subInitialLabel
        //   Description   : 
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : 라벨을 초기화 시킨다.
        //*******************************************************************************
        //   2007/03/12         박 근 태
        //*******************************************************************************
        public void subInitialLabel()
        {
            try
            {
                if ( PintAllPage > 10 )
                {
                    //10페이지를 활성화 시킨다.
                    for (int Page = 0; Page < 10; Page++)
                    {
                        //폼 hide후 show시 라벨 텍스트를 1부터 시작하도록 설정한다.
                        this.plblNum[Page].Text = Convert.ToString(Page + 1);
                        //라벨을 활성화 시킨다.
                        this.plblNum[Page].Enabled = true;
                    }

                    //Next 버튼의 활성화
                    this.lblNext.Enabled = true;
                }
                else
                {
                    for (int Page = 0; Page < 10; Page++)
                    {
                        //폼 hide후 show시 라벨 텍스트를 1부터 시작하도록 설정한다.
                        this.plblNum[Page].Text = Convert.ToString(Page + 1);
                        if (Page < PintAllPage)
                        {
                           // PintAllPage = 0;
                            //라벨을 활성화 시킨다.
                            this.plblNum[Page].Enabled = true;
                        }
                        else
                        {
                            //나머지는 비활성화
                            this.plblNum[Page].Enabled = false;
                        }
                    }

                    //Next 버튼의 비활성화
                    this.lblNext.Enabled = false;
                }
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion        

        //시간 동기화 함수
        #region "시간 동기화 함수"

        #region subInitialTime

        //*******************************************************************************
        //  Function Name : subInitialTime()
        //  Description   : 시간을 동기화 시킨다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/09          최 성 원        [L 00] 
        //*******************************************************************************
        private void subInitialTime()
        {
            try
            {
                //폼에 시간 표시
                pdtmTime = DateTime.Now;
                this.lblAlarmDateFrom.Text = pdtmTime.ToString("yyyy - MM - dd");
                this.lblAlarmDateTo.Text = pdtmTime.ToString("yyyy - MM - dd");

                PstrDateFrom = this.lblAlarmDateFrom.Text;
                PstrDateTo = this.lblAlarmDateTo.Text;
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region subSettingTime

        //*******************************************************************************
        //  Function Name : subSettingTime()
        //  Description   : 화면에서 선택한 날짜를 저장한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/09          최 성 원        [L 00] 
        //*******************************************************************************
        private void subSettingTime()
        {
            try
            {
                //화면에서 선택한 날짜를 저장한다.
                PstrDateFrom = this.lblAlarmDateFrom.Text;
                PstrDateTo = this.lblAlarmDateTo.Text;
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #endregion

        //로그 파일이 바뀌었을 경우
        #region subChangeLogfile

        //*******************************************************************************
        //   Function Name : subChangeLogfile()
        //   Description   : 
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : 로그 파일이 바뀌거나 디스플레이 버튼을 클릭했을 경우 호출된다.
        //*******************************************************************************
        //   2007/02/07         박 근 태
        //   2007/03/09         최 성 원
        //*******************************************************************************
        public void subChangeLogfile(string Name)
        {
            try
            {
                //날짜를 저장한다.
                subSettingTime();

                if (Name.Equals(PstrName) == true)
                {
                    //라벨 숫자를 증가 시킨다.
                    for (int dintLoop = 0; dintLoop < 10; dintLoop++)
                    {
                        this.plblNum[dintLoop].Text = (dintLoop + 1).ToString();
                    }

                    //현재 페이지 넘버를 초기화 한다.
                    PintIndex = 1;

                    //로그 파일을 업데이트 해서 화면을 표시한다.
                    this.subDisplay();
                }
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        //버튼에 관한 함수
        #region "버튼에 관한 함수"

        #region btnAlarmDisplay_Click

        //*******************************************************************************
        //   Function Name : btnAlarmDisplay_Click
        //   Description   : btn디스플레이 클릭시 DataGrid에 화면 갱신
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/03/12         박 근 태
        //*******************************************************************************
        private void btnAlarmDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                //Dispaly버튼 클릭시 폼의 숫자 라벨들을 모두 비활성으로 하고 시작하기 위해 0으로 초기화
                PintAllPage = 0;
                this.pintNextCount = 0;
                
                //GLSAPD폼의 Unit Num을 초기화.
                if (PstrName == "GLSAPD")
                {
                    if (pstrTempDateFrom == "" || pstrTempDateTo == "")
                    {
                        pstrTempDateFrom = this.lblAlarmDateFrom.Text;
                        pstrTempDateTo = this.lblAlarmDateTo.Text;
                    }
                    if (this.lblAlarmDateFrom.Text != pstrTempDateFrom || this.lblAlarmDateTo.Text != pstrTempDateTo)
                    {
                        PintComboIndex = 0;
                        this.comUnitNum.SelectedIndex = 0;
                    }
                    pstrTempDateFrom = this.lblAlarmDateFrom.Text;
                    pstrTempDateTo = this.lblAlarmDateTo.Text;
                }
                //Display버튼 클릭시 previous라벨을 비활성시킴.
                this.lblPrevious.Enabled = false;
                subChangeLogfile(PstrName);

                //next클릭시 11번째 라벨을 발강색으로 바꾸고 그 전 클릭된 라벨은 검은색으로 변경.
                this.plblNum[pintBeforeText].ForeColor = System.Drawing.Color.Black;
                this.plblNum[0].ForeColor = System.Drawing.Color.Red;
                pintBeforeText = 0;
               
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region lblNum_Click

        //*******************************************************************************
        //   Function Name : lblNum_Click
        //   Description   : 숫자라벨 클릭시 라벨 넘버를 저장한다.
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : 
        //*******************************************************************************
        //   2007/01/31         박 근 태
        //   2007/03/09         최 성 원
        //   2007/03/12         박 근 태
        //*******************************************************************************
        private void lblNum_Click(object sender, EventArgs e)
        {
            int dintIndex = 0;
            
            try
            {
                //시간을 동기화 시킨다.
                subSettingTime();

                //페이지 넘버를 저장한다.
                PintIndex = Convert.ToInt32(((Label)sender).Text);
                dintIndex = PintIndex;

                //label text의 숫자가 10, 100보다 크면
                if (PintIndex > 10)
                {   //맨 뒷자리 숫자만 뽑아옴
                    dintIndex = Convert.ToInt32(PintIndex.ToString().Substring(1, 1));
                    if (PintIndex > 100)
                    {
                        dintIndex = Convert.ToInt32(PintIndex.ToString().Substring(2, 1));
                    }//맨 뒷자리 숫자가 0이면 끝라벨인 10번째 라벨로 간주 
                    if (dintIndex == 0)
                    {
                        dintIndex = 10;
                    }
                }
                //폼 밑 숫자 라벨의 클릭시 빨강색으로 변하고 이전 클릭된 라벨은 검은색으로 변함.
                this.plblNum[pintBeforeText].ForeColor = System.Drawing.Color.Black;
                this.plblNum[dintIndex - 1].ForeColor = System.Drawing.Color.Red;
             
                pintBeforeText = dintIndex - 1;

                //화면출력
                this.subDisplayOutput();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region btnExit_Click

        //*******************************************************************************
        //   Function Name : btnExit_Click
        //   Description   : 폼을 숨김.
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/02/05         어 경태
        //*******************************************************************************
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.PInfo.proLogActFormShowIndex = InfoAct.clsInfo.LogActFormShowType.None;
                this.Hide();
                PintIndex = 1;
                pintNextCount = 0;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region lblNext_Click

        //*******************************************************************************
        //   Function Name : lblNext_Click
        //   Description   : lblnext클릭시 라벨텍스트값 증가 ex) 1,2,3 => 11,12,13
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/03/12         박 근 태
        //*******************************************************************************
        private void lblNext_Click(object sender, EventArgs e)
        {
            //지역 변수 선언
            int dintTemp = 0;               //비교를 위한 변수

            try
            {
                //뒤 버튼을 활성화시킨다.
                this.lblPrevious.Enabled = true;

                //화면을 초기화한다.
                pintNextCount++;
                PintIndex = pintNextCount * 10 + 1;

                if (pintNextControl == pintNextCount)
                {
                    this.lblNext.Enabled = false;
                }

                //next클릭시 11번째 라벨을 발강색으로 바꾸고 그 전 클릭된 라벨은 검은색으로 변경.
                this.plblNum[pintBeforeText].ForeColor = System.Drawing.Color.Black;
                this.plblNum[0].ForeColor = System.Drawing.Color.Red;
                pintBeforeText = 0;

                //라벨 숫자를 증가 시키고, 비활성화 시킨다.
                for (int Lable = 0; Lable < 10; Lable++)
                {
                    this.plblNum[Lable].Text = (Convert.ToInt32(this.plblNum[Lable].Text) + 10).ToString();
                    this.plblNum[Lable].Enabled = false;
                }

                //나타낸 10 범위안에 포함되는 것을 체크하기 위해서 변수 할당
                dintTemp = PintIndex + 10;

                //마지막 페이지가 있는 경우
                if (dintTemp > PintAllPage)
                {
                    for (int Page = 0; Page < (PintAllPage % 10); Page++)
                    {
                        this.plblNum[Page].Enabled = true;
                    }
                    this.lblNext.Enabled = false;
                }

                //마지막 페이지가 넘어가는 경우
                else
                {
                    for (int Page = 0; Page < 10; Page++)
                    {
                        this.plblNum[Page].Enabled = true;
                    }
                    this.lblNext.Enabled = true;
                }

                //화면출력
                this.subDisplayOutput();
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region lblPrevious_Click

        //*******************************************************************************
        //   Function Name : lblPrevious_Click
        //   Description   : lblnext클릭시 라벨텍스트값 감소 ex) 11,12,13 => 1,2,3
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/03/12         박 근 태
        //*******************************************************************************
        private void lblPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                //앞 버튼을 활성화시킨다.
                this.lblNext.Enabled = true;

                //화면을 초기화한다.
                pintNextCount--;
                PintIndex = pintNextCount * 10 + 1;

                //next클릭시 11번째 라벨을 발강색으로 바꾸고 그 전 클릭된 라벨은 검은색으로 변경.
                this.plblNum[pintBeforeText].ForeColor = System.Drawing.Color.Black;
                this.plblNum[0].ForeColor = System.Drawing.Color.Red;
                pintBeforeText = 0;

                //라벨 숫자를 증가 시키고, 비활성화 시킨다.
                for (int Lable = 0; Lable < 10; Lable++)
                {
                    this.plblNum[Lable].Text = (Convert.ToInt32(this.plblNum[Lable].Text) - 10).ToString();
                    this.plblNum[Lable].Enabled = true;
                }

                if (pintNextCount <= 0)
                {
                    this.lblPrevious.Enabled = false;
                }

                else if (pintNextCount > 0)
                {
                    this.lblPrevious.Enabled = true;
                }

                //화면 출력
                this.subDisplayOutput();
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region btnDelete_Click

        //*******************************************************************************
        //   Function Name : btnDelete_Click
        //   Description   : 그리드의 내용을 삭제함.
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/02/09         박 근 태
        //*******************************************************************************
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.grdAlarmList.DataSource = null;
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion



        #endregion

        #region "라벨 효과 관련"

        int dintIndex = 1;
        //*******************************************************************************
        //   Function Name : lblNext_MouseEnte
        //   Description   : next라벨의 위에 마우스가 왔을시 변화를 줌.
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/03/15         박 근 태
        //*******************************************************************************
        private void lblNext_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                //라벨 텍스트 및 색, 폰트를 바꿔줌.
                this.lblNext.Text = "Next";
                this.lblNext.ForeColor = System.Drawing.Color.Blue;
                //this.lblNext.Font = new System.Drawing.Font("휴먼매직체", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //   Function Name : lblPrevious_MouseEnter
        //   Description   : previous라벨의 위에 마우스가 왔을시 변화를 줌.
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/03/15         박 근 태
        //*******************************************************************************
        private void lblPrevious_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                //라벨 텍스트 및 색, 폰트를 바꿔줌.
                this.lblPrevious.Text = "Previous";
                this.lblPrevious.ForeColor = System.Drawing.Color.Blue;
                //this.lblPrevious.Font = new System.Drawing.Font("휴먼매직체", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //   Function Name : lblNext_MouseLeave
        //   Description   : 마우스 포인터가 next라벨 위를 벗어날때 상태를 원복시킴
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/03/15         박 근 태
        //*******************************************************************************
        private void lblNext_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                //라벨 텍스트 및 색, 폰트를 바꿔줌.
                this.lblNext.Text = ">  Next";
                this.lblNext.ForeColor = System.Drawing.Color.Black;
                //this.lblNext.Font = new System.Drawing.Font("휴먼매직체", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //   Function Name : lblPrevious_MouseLeave
        //   Description   : 마우스 포인터가 next라벨 위를 벗어날때 상태를 원복시킴
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/03/15         박 근 태
        //*******************************************************************************
        private void lblPrevious_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                //라벨 텍스트 및 색, 폰트를 바꿔줌.
                this.lblPrevious.Text = "Previous  <";
                this.lblPrevious.ForeColor = System.Drawing.Color.Black;
                //this.lblPrevious.Font = new System.Drawing.Font("휴먼매직체", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //   Function Name : lblNum_MouseEnter
        //   Description   : 마우스 포인터가 숫자 라벨(1,2,3,.....)위에 있을시 변화를 줌
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/03/15         박 근 태
        //*******************************************************************************
        private void lblNum_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                //마우스 포인터 밑에 있는 라벨의 텍스트 숫자를 얻어온다.
                dintIndex = Convert.ToInt32(((Label)sender).Text);

                //라벨 텍스트가 10보다 크면 
                if (dintIndex > 10)
                {   //맨 뒷자리 숫자만 뽑아옴
                    if (dintIndex > 100)
                    {
                        dintIndex = Convert.ToInt32(dintIndex.ToString().Substring(2, 1));
                    }
                    else
                        dintIndex = Convert.ToInt32(dintIndex.ToString().Substring(1, 1));

                    //맨 뒷자리 숫자가 0이면 끝라벨인 10번째 라벨로 간주 
                    if (dintIndex == 0)
                    {
                        dintIndex = 10;
                    }
                }

                //기존에 설정되 있던 색상을 저장한다.
                pColorTemp = this.plblNum[dintIndex - 1].ForeColor;

                //선택된 라벨의 폰트, 색상을 변경한다.
                this.plblNum[dintIndex - 1].ForeColor = System.Drawing.Color.Blue;
                //this.plblNum[dintIndex - 1].Font = new System.Drawing.Font("휴먼매직체", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //   Function Name : lblNum_MouseLeave
        //   Description   : 마우스 포인터가 숫자 라벨(1,2,3,.....)위를 벗어날시 원복시킴
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/03/15         박 근 태
        //*******************************************************************************
        private void lblNum_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                //마우스를 벗어나기전 클릭 이벤트가 일어났을시 색을 붉은색으로 변화시킴
                if (this.plblNum[dintIndex - 1].ForeColor == System.Drawing.Color.Red)
                {
                    this.plblNum[dintIndex - 1].ForeColor = System.Drawing.Color.Red;
                }
                else   //그렇지 아니하면 원래 색으로 변화
                {
                    this.plblNum[dintIndex - 1].ForeColor = pColorTemp;
                    //this.plblNum[dintIndex - 1].Font = new System.Drawing.Font("휴먼매직체", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion


        //콤보 박스 인덱스 변경시
        #region "comUnitNum_SelectionChangeCommitted"

        //*******************************************************************************
        //   Function Name : comUnitNum_SelectedIndexChanged
        //   Description   : 폼에서 콤보 박스의 값 선택시 선택된 유닛별로 그리드 출력
        //   Parameters    : 
        //   Return Value  : 
        //   Special Notes : None
        //*******************************************************************************
        //   2007/03/19         박 근 태
        //*******************************************************************************
        private void comUnitNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.comUnitNum.Text == "All Unit")
                {
                    PintComboIndex = 0;
                }
                else
                {
                    PintComboIndex = Convert.ToInt32((this.comUnitNum.Text).Substring(5, ((this.comUnitNum.Text).Length) - 5));
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

      
        #endregion

        //Grid_DataError
        #region "Grid_DataError"

        //*******************************************************************************
        //  Function Name : Grid_DataError()
        //  Description   : DataGridView에서 다른 예외가 발생할때 이곳으로 
        //                  에러 루틴을 타므로 지우지 말것 
        //  Parameters    : 
        //  Return Value  : None
        //  Special Notes : 지우지 말것
        //*******************************************************************************
        //  2007/03/01          김 효주         [L 00] 
        //*******************************************************************************
        private void Grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
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

      
    }
}
