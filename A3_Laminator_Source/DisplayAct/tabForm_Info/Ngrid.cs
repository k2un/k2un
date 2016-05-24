using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace DisplayAct
{
    /// <summary>
    /// SDC A3 신규 사양 S16 계열 데이터 출력용 컨트롤
    /// </summary>
    public partial class Ngrid : UserControl
    {
        #region Fields
        /// <summary>
        /// 검색 요청 이벤트
        /// </summary>
        public event NgridFindEventHandler OnFind;
        /// <summary>
        /// 정렬 기준 변경 이벤트
        /// </summary>
        public event NgridSortEventHandler OnSort;
        /// <summary>
        /// 데이터 삭제 이벤트
        /// </summary>
        public event NgridDeleteEventHandler OnDelete;

        /// <summary>
        /// 시간 기준 정렬 여부 플레그
        /// </summary>
        private bool pblTimeSort = true;
        /// <summary>
        /// DataGridView 데이터 바인딩 진행 플레그
        /// </summary>
        private bool pblBinding = false;
        /// <summary>
        /// DataGridView 검색결과 창 모드 플레그
        /// </summary>
        private bool pblFindMode = false;

        /// <summary>
        /// DataGridView 데이터 타입 (APC, PPC, RPC)
        /// </summary>
        private NgridType pNgridType;

        /// <summary>
        /// 실 데이터 테이블
        /// </summary>
        private DataTable pDTdata = new DataTable();

        /// <summary>
        /// DataGridView Row Count 변경시 딜레이
        /// </summary>
        private DateTime pdtSkip = DateTime.Now;

        /// <summary>
        /// 실 데이터 테이블의 'isCenter' 컬럼 인덱스
        /// </summary>
        private int pintisCenterColumnIndex = -1;
        #endregion
        #region Properties
        /// <summary>
        /// 실 데이터의 타입을 가져오거나 설정합니다.
        /// </summary>
        public NgridType FormType
        {
            get
            {
                return this.pNgridType;
            }
            set
            {
                this.pNgridType = value;
                subSetUI();
            }
        }
        /// <summary>
        /// 실 데이터를 가져오거나 설정합니다.
        /// </summary>
        public object DataSource
        {
            get
            {
                return this.pDTdata;
            }
            set
            {
                this.pblBinding = true;
                this.pDTdata = (DataTable)value;
                subSetColumns();
                subSetGridView();
                this.pblBinding = false;
            }
        }
        /// <summary>
        /// 컨트롤의 검색창으로 사용여부를 가져오거나 설정합니다. 
        /// </summary>
        public bool FindEnable
        {
            get
            {
                return this.pblFindMode;
            }
            set
            {
                this.pblFindMode = value;
                subSetFindMode(!this.pblFindMode);
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// 세부 사항 설정없이 컨트롤만 생성.
        /// </summary>
        public Ngrid()
        {
            InitializeComponent();     
        }
        /// <summary>
        /// 실데이터 타입을 설정하여 컨트롤을 생성합니다.
        /// </summary>
        /// <param name="formType">데이터 타입</param>
        public Ngrid(NgridType formType)
        {
            InitializeComponent();
            this.pNgridType = formType;
            subSetUI();
        }
        #endregion
        #region Methods
        /// <summary>
        /// 검색창으로 사용시 컨트롤 레이아웃 변경
        /// </summary>
        /// <param name="SetFindMode">검색 모드 사용 여부</param>
        private void subSetFindMode(bool SetFindMode)
        {
            try
            {
                this.tLPbase.RowStyles[1].Height = 0;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 실 데이터와 DataGridView의 Column 동기화
        /// </summary>
        private void subSetColumns()
        {
            try
            {
                this.dgv.RowCount = 0;
                this.dgv.Columns.Clear();

                if (this.pDTdata == null) return;

                if (this.pDTdata != null && this.pDTdata.Columns.Count > 0)
                {
                    this.pintisCenterColumnIndex = -1;

                    for (int dintLoop = 0; dintLoop < this.pDTdata.Columns.Count; dintLoop++)
                    {
                        this.dgv.Columns.Add(this.pDTdata.Columns[dintLoop].ColumnName, this.pDTdata.Columns[dintLoop].ColumnName);
                        if (this.pDTdata.Columns[dintLoop].ColumnName == "isCenter") this.pintisCenterColumnIndex = dintLoop;
                    }
                }

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 데이터 타입에 따라 컨트롤 노출 문자 변경
        /// </summary>
        private void subSetUI()
        {
            try
            {
                this.pblTimeSort = true;
                this.cbSort1.Checked = true;
                this.cbSort2.Checked = false;
                this.tbFindGlassID.Text = string.Empty;

                switch (this.pNgridType)
                {
                    case NgridType.APC:
                        this.lbTitle.Text = "Advanced Process Control (APC)";
                        this.cbSort2.Text = "APC_STATE";
                        break;
                    case NgridType.PPC:
                        this.lbTitle.Text = "Process Path Control (PPC)";
                        this.cbSort2.Text = "P_STATE";
                        break;
                    case NgridType.RPC:
                        this.lbTitle.Text = "Recipe Process Control (RPC)";
                        this.cbSort2.Text = "RPC_STATE";
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 실 데이터를 DataGridView에 출력합니다.
        /// </summary>
        private void subSetGridView()
        {
            try
            {
                for (int dintLoop = 0; dintLoop < this.dgv.Columns.Count; dintLoop++)
                {
                    DataGridViewCellStyle cs = new DataGridViewCellStyle();
                    cs.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                    cs.BackColor = System.Drawing.SystemColors.Window;
                    cs.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                    cs.ForeColor = System.Drawing.SystemColors.ControlText;
                    cs.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                    cs.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                    cs.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
                    cs.Padding = new System.Windows.Forms.Padding(7, 1, 7, 1);


                    this.dgv.Columns[dintLoop].DefaultCellStyle = cs;

                    this.dgv.Columns[dintLoop].SortMode = DataGridViewColumnSortMode.NotSortable;

                    if (this.dgv.Columns[dintLoop].Name == "isCenter" || this.dgv.Columns[dintLoop].Name == "isRun")
                    {
                        this.dgv.Columns[dintLoop].Visible = false;
                    }
                    else if (this.dgv.Columns[dintLoop].Name == "Operation")
                    {
                        this.dgv.Columns["Operation"].Visible = !this.pblFindMode;
                        this.dgv.Columns[dintLoop].MinimumWidth = 65;
                        this.dgv.Columns[dintLoop].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    else
                    {
                        this.dgv.Columns[dintLoop].MinimumWidth = funGetPixel(funGetMaxLength(dintLoop));
                        this.dgv.Columns[dintLoop].Width = this.dgv.Columns[dintLoop].MinimumWidth;
                        this.dgv.Columns[dintLoop].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                    
                }

                if (this.pDTdata != null) this.dgv.RowCount = (this.pDTdata.Rows.Count > 50) ? 50 : this.pDTdata.Rows.Count;
                if (this.dgv.RowCount > 0) this.dgv.Rows[0].Selected = false;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 문자열의 화면 출력시 폭을 구합니다.
        /// </summary>
        /// <param name="strValue">확인할 문자열</param>
        /// <returns>문자열 실 출력 픽셀</returns>
        private int funGetPixel(string strValue)
        {
            int dintPixel = 0;

            try
            {
                Label dLabel = new Label();
                dLabel.Font = new System.Drawing.Font("Calibri", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                dLabel.Padding = new System.Windows.Forms.Padding(5, 1, 5, 1);
                dLabel.Margin = new System.Windows.Forms.Padding(0);
                dLabel.AutoEllipsis = false;
                dLabel.AutoSize = true;

                dLabel.Text = strValue;

                dintPixel = dLabel.PreferredWidth;
            }
            catch
            {
                throw;
            }

            return dintPixel;
        }

        /// <summary>
        /// 실 데이터에서 해당 컬럼의 가장 긴 값을 구합니다.
        /// </summary>
        /// <param name="dintColumn">컬럼 인덱스</param>
        /// <returns>가장 긴 값 문자열</returns>
        private string funGetMaxLength(int dintColumn)
        {
            string dstrMaxValue = string.Empty;

            try
            {
                for (int dintLoop = 0; dintLoop < this.pDTdata.Rows.Count; dintLoop++)
                {
                    if (dstrMaxValue.Length < this.pDTdata.Rows[dintLoop].ItemArray[dintColumn].ToString().Length)
                    {
                        dstrMaxValue = this.pDTdata.Rows[dintLoop].ItemArray[dintColumn].ToString();
                    }
                }               
            }
            catch
            {
                throw;
            }

            return dstrMaxValue;
        }
        /// <summary>
        /// 컬럼의 현 Row와 상위 Row의 값이 동일한지 확인합니다.
        /// </summary>
        /// <param name="column">컬럼 인덱스</param>
        /// <param name="row">로우 인덱스</param>
        /// <returns>값 동일 여부</returns>
        private bool funIsTheSameCellValue(int column, int row)
        {
            try
            {
                string strHGlassID1 = this.pDTdata.Rows[row].ItemArray[1].ToString();
                string strHGlassID2 = this.pDTdata.Rows[row - 1].ItemArray[1].ToString();

                object cell1 = this.pDTdata.Rows[row].ItemArray[column];
                object cell2 = this.pDTdata.Rows[row - 1].ItemArray[column];

                if (cell1 == null || cell2 == null)
                {
                    if (strHGlassID1 == strHGlassID2) return true;
                    else return false;
                }

                if (cell1.ToString() == cell2.ToString() && strHGlassID1 == strHGlassID2) return true;
                else return false;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 실 데이터에서 컬럼명으로 인덱스를 구합니다.
        /// </summary>
        /// <param name="strColumnName">컬럼명</param>
        /// <returns>컬럼 인덱스 (해당 컬럼 없을시 -1)</returns>
        private int funGetIndex(string strColumnName)
        {
            try
            {
                for (int dintIndex = 0; dintIndex < this.pDTdata.Columns.Count; dintIndex++)
                {
                    if (this.pDTdata.Columns[dintIndex].ColumnName == strColumnName) return dintIndex;
                }
            }
            catch
            {
                throw;
            }

            return -1;
        }

        #region Control Event Handler
        /// <summary>
        /// 검색 버튼 클릭시 이벤트 처리
        /// 검색 글라스 아이디 텍스트 박스에 값이 있을경우
        /// 검색 이벤트를 발생.
        /// </summary>
        private void btnFindHGLASSID_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.tbFindGlassID.Text.Trim()) && this.OnFind != null)
                    this.OnFind(new NgridFindEventArgs(this.tbFindGlassID.Text.Trim()));
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 정렬 기준 버튼 클릭시 이벤트 처리
        /// 2개의 체크 박스의 선택 상태 변경 후 정렬 변경 이벤트 발생
        /// </summary>
        private void cbSortBtn_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox cb = (CheckBox)sender;
                
                if (cb.Text.Equals("SET_TIME"))
                {
                    this.cbSort2.Checked = false;

                    if (this.pblTimeSort)
                    {
                        this.cbSort1.Checked = true;
                        return;
                    }
                    else
                    {
                        this.pblTimeSort = true;
                    }
                }
                else
                {
                    this.cbSort1.Checked = false;

                    if (!this.pblTimeSort)
                    {
                        this.cbSort2.Checked = true;
                        return;
                    }
                    else
                    {
                        this.pblTimeSort = false;
                    }
                }

                if (this.OnSort != null)
                    this.OnSort(new NgridSortEventArgs(this.pblTimeSort));
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// DataGridView의 Operation 컬럼의 Delete 버튼 클릭시 삭제 이벤트 발생
        /// </summary>
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                DataGridViewCell dgvc = dgv.Rows[e.RowIndex].Cells[(this.pNgridType == NgridType.APC) ? 6 : 5];
                bool isRun = (dgvc.Value != null) ? dgvc.Value.ToString().Equals("Running") : false;

                switch (this.pNgridType)
                {
                    case NgridType.APC:
                        if (e.ColumnIndex != 7 || isRun || Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["isCenter"].Value) == false) return;
                        break;
                    case NgridType.PPC:
                        if (e.ColumnIndex != 6 || Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["isCenter"].Value) == false
                            || this.dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor != Color.Yellow) return;
                        break;
                    case NgridType.RPC:
                        if (e.ColumnIndex != 6 || isRun) return;
                        break;
                }

                if (this.OnDelete != null) this.OnDelete(new NgridDeleteEventArgs(dgv.Rows[e.RowIndex].Cells[1].Value.ToString()));
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// DataGridView의 Cell 더블 클릭시 검색 GLASSID 입력
        /// </summary>
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                this.tbFindGlassID.Text = this.pDTdata.Rows[e.RowIndex].ItemArray[1].ToString().Trim();
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// DataGridView의 값에 따라 셀 테두리 및 배경색 변경.
        /// </summary>
        private void dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (this.pblBinding) return;


                if (this.pNgridType != NgridType.RPC)
                {
                    e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

                    if (e.ColumnIndex < 0 || e.RowIndex < 1)
                    {
                        if (e.RowIndex == 0)
                        {
                            e.AdvancedBorderStyle.Top = dgv.AdvancedCellBorderStyle.Top;
                        }
                    }
                    else if (e.RowIndex >= dgv.Rows.Count)
                    {
                        e.AdvancedBorderStyle.Bottom = dgv.AdvancedCellBorderStyle.Bottom;
                    }
                    else if (funIsTheSameCellValue(e.ColumnIndex, e.RowIndex))
                    {
                        switch (this.pNgridType)
                        {
                            case NgridType.APC:
                                if (e.ColumnIndex != 4 && e.ColumnIndex != 5 && Convert.ToBoolean(dgv.Rows[e.RowIndex - 1].Cells["isCenter"].Value) == false)
                                    e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                                break;

                            case NgridType.PPC:
                                if (e.ColumnIndex >= 3 && e.ColumnIndex <= 5) e.AdvancedBorderStyle.Top = dgv.AdvancedCellBorderStyle.Top;
                                if (e.ColumnIndex < 3 && e.ColumnIndex > 5 && Convert.ToBoolean(dgv.Rows[e.RowIndex - 1].Cells["isCenter"].Value) == false)
                                    e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

                                break;
                        }
                    }
                    else
                    {
                        e.AdvancedBorderStyle.Top = dgv.AdvancedCellBorderStyle.Top;
                    }
                }

                // Cell 배경색
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    bool isRun;

                    switch (this.pNgridType)
                    {
                        #region "APC"
                        case NgridType.APC:

                            isRun = this.pDTdata.Rows[e.RowIndex].ItemArray[6].ToString().Equals("Running");//dgv.Rows[e.RowIndex].Cells[6].Value.ToString().Equals("Running");

                            if (e.ColumnIndex == 7)
                            {
                                if (Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["isCenter"].Value))
                                {
                                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = (isRun) ? Color.Gray : Color.Yellow;
                                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = (isRun) ? Color.DarkGray : Color.Black;

                                    e.AdvancedBorderStyle.All = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
                                    e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
                                }
                                else
                                {
                                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                                }
                            }
                            else
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                                    (isRun) ? System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(209)))), ((int)(((byte)(104))))) :
                                        System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(244)))), ((int)(((byte)(155)))));
                            }
                            break;
                        #endregion

                        #region "PPC"
                        case NgridType.PPC:

                            isRun = (Convert.ToBoolean(this.pDTdata.Rows[e.RowIndex].ItemArray[8] /*dgv.Rows[e.RowIndex].Cells["isRun"].Value*/)) ? true : false;

                            if (e.ColumnIndex == 6)
                            {
                                if (Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["isCenter"].Value))
                                {
                                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = (isRun) ? Color.Gray : Color.Yellow;
                                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = (isRun) ? Color.DarkGray : Color.Black;

                                    e.AdvancedBorderStyle.All = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
                                    e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
                                }
                                else
                                {
                                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                                }
                            }
                            else
                            {
                                string strPstate = this.pDTdata.Rows[e.RowIndex].ItemArray[5].ToString(); //dgv.Rows[e.RowIndex].Cells[5].Value.ToString();

                                if (e.ColumnIndex >= 0 && e.ColumnIndex <= 2)
                                {
                                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                                    (isRun) ? System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(209)))), ((int)(((byte)(104))))) :
                                        System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(244)))), ((int)(((byte)(155)))));
                                }
                                else
                                {
                                    switch (strPstate)
                                    {
                                        case "Done":
                                            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = 
                                                System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
                                            break;

                                        case "Running":
                                            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = 
                                                System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(209)))), ((int)(((byte)(104)))));
                                            break;

                                        case "Waiting":
                                            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = 
                                                System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(244)))), ((int)(((byte)(155)))));
                                            break;
                                    }
                                }
                            }

                            break;
                        #endregion

                        #region "RPC"
                        case NgridType.RPC:

                            isRun = this.pDTdata.Rows[e.RowIndex].ItemArray[5].ToString().Equals("Running");//dgv.Rows[e.RowIndex].Cells[5].Value.ToString().Equals("Running");

                            if (e.ColumnIndex == 6)
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = (isRun) ? Color.Gray : Color.Yellow;
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = (isRun) ? Color.DarkGray : Color.Black;
                            }
                            else
                            {
                                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                                    (isRun) ? System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(209)))), ((int)(((byte)(104))))) :
                                        System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(244)))), ((int)(((byte)(155)))));
                            }
                            break;
                        #endregion
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// DataGridView의 중복 값 Merge
        /// </summary>
        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.pblBinding) return;
                bool dblisCenter = (this.pDTdata.Columns.Contains("isCenter")) ? Convert.ToBoolean(this.pDTdata.Rows[e.RowIndex].ItemArray[pintisCenterColumnIndex]) : true;
                switch (this.pNgridType)
                {
                    case NgridType.APC:
                        if (e.RowIndex == 0 || (e.ColumnIndex != 4 && e.ColumnIndex != 5))
                        {
                            if (dblisCenter == false && e.ColumnIndex != 4 && e.ColumnIndex != 5)
                            {
                                e.Value = "";
                                e.FormattingApplied = true;
                            }
                        }
                        break;

                    case NgridType.PPC:
                        if (e.RowIndex == 0 || (e.ColumnIndex < 3 || e.ColumnIndex > 5))
                        {
                            if (dblisCenter == false && (e.ColumnIndex < 3 || e.ColumnIndex > 5))
                            {
                                e.Value = "";
                                e.FormattingApplied = true;
                            }
                        }
                        break;
                    case NgridType.RPC:
                        break;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// DataGridView Selection 제거용.
        /// </summary>
        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.pblBinding || dgv.SelectedRows.Count < 0) return;
                if (dgv.SelectedCells.Count > 0)
                    this.dgv.SelectedCells[0].Selected = false;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 실제 화면에 출력되는 Cell에만 실데이터를 기록함.
        /// </summary>
        private void dgv_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if (this.pblBinding) return;
                if (this.dgv.RowCount == 0 || this.pDTdata.Rows.Count == 0) return;

                int dintDpFirstRow = this.dgv.FirstDisplayedCell.RowIndex;
                int dintDpFirstColumn = this.dgv.FirstDisplayedCell.ColumnIndex;

                if (e.RowIndex < dintDpFirstRow || e.RowIndex > this.dgv.DisplayedRowCount(false) + dintDpFirstRow ||
                    e.ColumnIndex < dintDpFirstColumn || e.ColumnIndex > this.dgv.DisplayedColumnCount(false) + dintDpFirstColumn + 1) return;

                e.Value = this.pDTdata.Rows[e.RowIndex][e.ColumnIndex];
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// DataGridView의 스크롤 발생시 Row Count 추가.
        /// </summary>
        private void dgv_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (this.dgv.RowCount == 0 || this.pDTdata.Rows.Count == 0) return;
                int dintDpLastRow = this.dgv.DisplayedRowCount(false) + this.dgv.FirstDisplayedCell.RowIndex;

                if (dintDpLastRow >= this.dgv.Rows.Count && dintDpLastRow < this.pDTdata.Rows.Count)
                {
                    if (pdtSkip > DateTime.Now)
                    {
                        e.NewValue--;
                        return;
                    }

                    this.dgv.RowCount += ((this.pDTdata.Rows.Count - this.dgv.RowCount) > 20) ? 20 : (this.pDTdata.Rows.Count - this.dgv.RowCount);
                    pdtSkip = DateTime.Now.AddMilliseconds(50);
                }

                if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll && this.dgv.FirstDisplayedCell.ColumnIndex > 0) this.dgv.InvalidateColumn(0);
            }
            catch
            {
                throw;
            }
        }
        #endregion

       

 
        #endregion

    }
    #region EventHandler
    /// <summary>
    /// 검색 요청 이벤트 핸들러
    /// </summary>
    /// <param name="ar">검색 요청 이벤트 데이터</param>
    public delegate void NgridFindEventHandler(NgridFindEventArgs ar);
    /// <summary>
    /// 정렬 기준 변경 이벤트 핸들러
    /// </summary>
    /// <param name="ar">정렬 기분 변경 이벤트 데이터</param>
    public delegate void NgridSortEventHandler(NgridSortEventArgs ar);
    /// <summary>
    /// 테이터 삭제 이벤트 핸들러
    /// </summary>
    /// <param name="ar">데이터 삭제 이벤트 데이터</param>
    public delegate void NgridDeleteEventHandler(NgridDeleteEventArgs ar);
    #endregion
    #region EventArgs
    /// <summary>
    /// Ngrid 데이터 삭제 이벤트 데이터
    /// </summary>
    public class NgridDeleteEventArgs : EventArgs
    {
        private readonly string pstrHGlassID;

        /// <summary>
        /// Ngrid 데이터 삭제 이벤트 데이터를 생성합니다.
        /// </summary>
        /// <param name="strHGlassID">GlassID</param>
        public NgridDeleteEventArgs(string strHGlassID)
        {
            this.pstrHGlassID = strHGlassID;
        }

        /// <summary>
        /// GlassID를 가져옵니다.
        /// </summary>
        public string GlassID
        {
            get
            {
                return this.pstrHGlassID;
            }
        }
    }
    /// <summary>
    /// Ngrid 검색 이벤트 데이터
    /// </summary>
    public class NgridFindEventArgs : EventArgs
    {
        private readonly string pstrHGlassID;

        /// <summary>
        /// Ngrid 검색 이벤트 데이터를 생성합니다.
        /// </summary>
        /// <param name="strFindID">검색할 GlassID</param>
        public NgridFindEventArgs(string strFindID)
        {
            this.pstrHGlassID = strFindID;
        }

        /// <summary>
        /// 검색할 GlassID를 가져옵니다.
        /// </summary>
        public string GlassID
        {
            get
            {
                return this.pstrHGlassID;
            }
        }
    }
    /// <summary>
    /// Ngrid 정렬 변경 이벤트 데이터
    /// </summary>
    public class NgridSortEventArgs : EventArgs
    {
        private readonly bool timeSort;

        /// <summary>
        /// Ngrid 정렬 변경 이벤트 데이터를 생성합니다.
        /// </summary>
        /// <param name="Timesort">시간기준 정렬 여부</param>
        public NgridSortEventArgs(bool Timesort)
        {
            this.timeSort = Timesort;
        }

        /// <summary>
        /// 시간기준 정렬 여부를 가져옵니다.
        /// </summary>
        public bool TimeSort
        {
            get
            {
                return this.timeSort;
            }
        }
    }
    #endregion

    /// <summary>
    /// Ngrid 타입
    /// </summary>
    public enum NgridType
    {
        /// <summary>
        /// Process Path Control (PPC)
        /// </summary>
        PPC = 0,
        /// <summary>
        /// Advanced Process Control (APC)
        /// </summary>
        APC = 1,
        /// <summary>
        /// Recipe Process Control (RPC)
        /// </summary>
        RPC = 2,
    }
}
