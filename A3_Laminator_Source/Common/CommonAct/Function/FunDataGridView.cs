using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Collections;

namespace CommonAct
{
    public class FunDataGridView
    {
        private DataGridView pgrdDataGridView;

        public enum CellType
        {
            TextBox = 1,
            CheckBox = 2,
            Image = 3,
            Button = 4,
            ComboBox = 5,
            Link = 6,
        }

        #region "DataGridView에 Data출력"
        /// <summary>
        /// DataGridView에 DataTable을 Binding(출력)한다.
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="dtDataTable">바인딩하고자 하는 DataTable</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView에 바인딩된다.
        /// </comment>
        public void subDisplayDataGridView(DataGridView grdDataGridView, DataTable dtDataTable)
        {
            try
            {
                this.pgrdDataGridView = grdDataGridView;
                this.pgrdDataGridView.DataSource = dtDataTable;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에 BindingSource개체를 Binding(출력)한다.
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="bsBindingSource">바인딩하고자 하는 BindingSource개체</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView에 바인딩된다.
        /// </comment>
        public void subDisplayDataGridView(DataGridView grdDataGridView, BindingSource bsBindingSource)
        {
            try
            {
                this.pgrdDataGridView = grdDataGridView;
                this.pgrdDataGridView.DataSource = bsBindingSource;
            }
            catch
            {
                throw new Exception();
            }
        }

        #endregion

        #region "DataGridView에 새로운 행 추가, 삭제, 수정"

        /// <summary>
        /// DataGridView에 새로운 행을 추가한다.
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="strArrRowData">행 데이터</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView에 행이 추가된다.
        /// </comment>
        public void subAddRow(DataGridView grdDataGridView, string[] strArrRowData)
        {
            BindingSource dBS = null;
            DataTable dDT = null;

            try
            {
                //DataGridView의 컬럼개수와 새로 입력하려고 하는 컬럼데이터 개수가 맞지 않으면 빠져나간다.
                if (grdDataGridView.ColumnCount != strArrRowData.Length) return;

                this.pgrdDataGridView = grdDataGridView;

                if (this.pgrdDataGridView.DataSource.GetType().Name == "DataTable")
                {
                    dDT = (DataTable)this.pgrdDataGridView.DataSource;
                }
                else if (this.pgrdDataGridView.DataSource.GetType().Name == "BindingSource")
                {
                    //DataGridView에 바인딩된 객체가 BindingSource이면 BindingSource로 형변환 후 DataTable을 가져온다.
                    dBS = (BindingSource)this.pgrdDataGridView.DataSource;
                    dDT = (DataTable)dBS.DataSource;
                }

                dDT.NewRow();
                dDT.Rows.Add(strArrRowData);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 행을 삭제한다.
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intRowIndex">행 인덱스</param>
        public void subDeleteRow(DataGridView grdDataGridView, int intRowIndex)
        {
            try
            {
                this.pgrdDataGridView = grdDataGridView;
                this.pgrdDataGridView.Rows.RemoveAt(intRowIndex);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 행을 수정한다.
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intRowIndex">행 인덱스</param>
        /// <param name="strArrRowData">변경할 행 데이터</param>
        public void subUpdateRow(DataGridView grdDataGridView, int intRowIndex, string[] strArrRowData)
        {
            BindingSource dBS = null;
            DataTable dDT = null;

            try
            {
                //DataGridView의 컬럼개수와 새로 입력하려고 하는 컬럼데이터 개수가 맞지 않으면 빠져나간다.
                if (grdDataGridView.ColumnCount != strArrRowData.Length) return;
        
                this.pgrdDataGridView = grdDataGridView;

                if (this.pgrdDataGridView.DataSource.GetType().Name == "DataTable")
                {
                    dDT = (DataTable)this.pgrdDataGridView.DataSource;
                }
                else if (this.pgrdDataGridView.DataSource.GetType().Name == "BindingSource")
                {
                    //DataGridView에 바인딩된 객체가 BindingSource이면 BindingSource로 형변환 후 DataTable을 가져온다.
                    dBS = (BindingSource)this.pgrdDataGridView.DataSource;
                    dDT = (DataTable)dBS.DataSource;
                }

                //열너비를 수동조정해 GridView의 너비에 맞춘다.
                for (int dintLoop = 0; dintLoop < strArrRowData.Length; dintLoop++)
                {
                    dDT.Rows[intRowIndex][dintLoop] = strArrRowData[dintLoop];
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        #endregion

        #region "DataGridView에 새로운 컬럼 입력, 삭제, 데이터 수정"
        /// <summary>
        /// DataGridView에 새로운 컬럼을 추가한다.
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="strColumnName">컬럼 이름</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView에 컬럼이 추가된다.
        /// </comment>
        public void subAddColumn(DataGridView grdDataGridView, string strColumnName)
        {
            try
            {
                int dintColumnIndex = grdDataGridView.Columns.Count;        //컬럼을 Grid의 제일 뒤(오른쪽)에 추가한다.
                string dstrColumnHeaderText = "ColumnHeader" + dintColumnIndex.ToString();  //ColumnName
                Color dcColumnColor = grdDataGridView.DefaultCellStyle.BackColor;
                
                //아래 Overloading되어 있는 함수를 호출한다.
                subAddColumn(grdDataGridView, strColumnName, dintColumnIndex, dstrColumnHeaderText, dcColumnColor, CellType.TextBox);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에 새로운 컬럼을 추가한다.
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="strColumnName">컬럼 이름</param>
        /// <param name="intColumnIndex">컬럼 인덱스</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView에 컬럼이 추가된다.
        /// </comment>
        public void subAddColumn(DataGridView grdDataGridView, string strColumnName, int intColumnIndex)
        {
            try
            {
                string dstrColumnHeaderText = "ColumnHeader" + grdDataGridView.Columns.Count.ToString();  //ColumnName
                Color dcColumnColor = grdDataGridView.DefaultCellStyle.BackColor;

                //아래 Overloading되어 있는 함수를 호출한다.
                subAddColumn(grdDataGridView, strColumnName, intColumnIndex, dstrColumnHeaderText, dcColumnColor, CellType.TextBox);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에 새로운 컬럼을 추가한다.
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="strColumnName">컬럼 이름</param>
        /// <param name="intColumnIndex">컬럼 인덱스</param>
        /// <param name="strColumnHeaderText">컬럼 HeaderText</param>
        public void subAddColumn(DataGridView grdDataGridView, string strColumnName, int intColumnIndex, string strColumnHeaderText)
        {
            try
            {
                Color dcColumnColor = grdDataGridView.DefaultCellStyle.BackColor;

                //아래 Overloading되어 있는 함수를 호출한다.
                subAddColumn(grdDataGridView, strColumnName, intColumnIndex, strColumnHeaderText, dcColumnColor, CellType.TextBox);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에 새로운 컬럼을 추가한다.
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="strColumnName">컬럼 이름</param>
        /// <param name="intColumnIndex">컬럼 인덱스</param>
        /// <param name="strColumnHeaderText">컬럼 HeaderText</param>
        /// <param name="cColumnColor">컬럼 색깔</param>
        /// <param name="ctCellType">컬럼 Type(6가지)</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView에 컬럼이 추가된다.
        /// </comment>
        public void subAddColumn(DataGridView grdDataGridView, string strColumnName, int intColumnIndex, string strColumnHeaderText, Color cColumnColor, CellType ctCellType)
        {
            BindingSource dBS = null;
            DataTable dDT = null;
            DataGridViewColumn dDC = null;
            DataGridViewCell cell = null;

            try
            {
                this.pgrdDataGridView = grdDataGridView;


                if (this.pgrdDataGridView.DataSource.GetType().Name == "DataTable")
                {
                    dDT = (DataTable)this.pgrdDataGridView.DataSource;
                }
                else if (this.pgrdDataGridView.DataSource.GetType().Name == "BindingSource")
                {
                    //DataGridView에 바인딩된 객체가 BindingSource이면 BindingSource로 형변환 후 DataTable을 가져온다.
                    dBS = (BindingSource)this.pgrdDataGridView.DataSource;
                    dDT = (DataTable)dBS.DataSource;
                }

                switch (ctCellType)
                {
                    case CellType.TextBox:
                        dDC = new DataGridViewTextBoxColumn();
                        cell = new DataGridViewTextBoxCell();
                        break;

                    case CellType.CheckBox:
                        dDC = new DataGridViewCheckBoxColumn();
                        cell = new DataGridViewCheckBoxCell();
                        break;

                    case CellType.Image:
                        dDC = new DataGridViewImageColumn();
                        cell = new DataGridViewImageCell();
                        break;

                    case CellType.Button:
                        dDC = new DataGridViewButtonColumn();
                        cell = new DataGridViewButtonCell();
                        break;

                    case CellType.ComboBox:
                        dDC = new DataGridViewComboBoxColumn();
                        cell = new DataGridViewComboBoxCell();
                        break;

                    case CellType.Link:
                        dDC = new DataGridViewLinkColumn();
                        cell = new DataGridViewLinkCell();
                        break;

                    default:
                        break;
                }
                
                dDC.Name = strColumnName;                           //ColumnName
                dDC.HeaderText = strColumnHeaderText;               //Column HeaderText

                cell.Style.BackColor = cColumnColor;
                dDC.CellTemplate = cell;

                this.pgrdDataGridView.Columns.Insert(intColumnIndex, dDC);
            }
            catch(Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 컬럼을 삭제한다.(ColumnName으로)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="strColumnName">컬럼 이름</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView에서 컬럼이 삭제된다.
        /// </comment>
        public void subDeleteColumn(DataGridView grdDataGridView, string strColumnName)
        {
            try
            {
                int dintColumnIndex = grdDataGridView.Columns[strColumnName].Index;

                //아래 Overloading되어 있는 함수를 호출한다.
                subDeleteColumn(grdDataGridView, dintColumnIndex);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 컬럼을 삭제한다.(ColumnIndex로)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intColumnIndex">컬럼 인덱스</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView에서 컬럼이 삭제된다.
        /// </comment>
        public void subDeleteColumn(DataGridView grdDataGridView, int intColumnIndex)
        {
            try
            {
                this.pgrdDataGridView = grdDataGridView;
                this.pgrdDataGridView.Columns.RemoveAt(intColumnIndex);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 셀 데이터를 수정한다.(한 셀 Data만 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intRowIndex">RowIndex</param>
        /// <param name="strColumnName">컬럼 Name</param>
        /// <param name="strCellData">변경하려고 하는 셀 Data</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 셀 Data가 수정된다.
        /// </comment>
        public void subUpdateCell(DataGridView grdDataGridView, int intRowIndex, string strColumnName, string strCellData)
        {
            try
            {
                int dintColumnIndex = grdDataGridView.Columns[strColumnName].Index;

                //아래 Overloading되어 있는 함수를 호출한다.
                subUpdateCell(grdDataGridView, intRowIndex, dintColumnIndex, strCellData);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 셀 데이터를 수정한다.(한 셀 Data만 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intRowIndex">RowIndex</param>
        /// <param name="intColumnIndex">컬럼Index</param>
        /// <param name="strCellData">변경하려고 하는 셀 Data</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 셀 Data가 수정된다.
        /// </comment>
        public void subUpdateCell(DataGridView grdDataGridView, int intRowIndex, int intColumnIndex, string strCellData)
        {
            BindingSource dBS = null;
            DataTable dDT = null;

            try
            {
                this.pgrdDataGridView = grdDataGridView;

                if (this.pgrdDataGridView.DataSource.GetType().Name == "DataTable")
                {
                    dDT = (DataTable)this.pgrdDataGridView.DataSource;
                }
                else if (this.pgrdDataGridView.DataSource.GetType().Name == "BindingSource")
                {
                    //DataGridView에 바인딩된 객체가 BindingSource이면 BindingSource로 형변환 후 DataTable을 가져온다.
                    dBS = (BindingSource)this.pgrdDataGridView.DataSource;
                    dDT = (DataTable)dBS.DataSource;
                }

                this.pgrdDataGridView.Rows[intRowIndex].Cells[intColumnIndex].Value = strCellData;   //Cell data 변경
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 컬럼 데이터를 수정한다.(Row범위를 지정해서 Data를 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intStartRowIndex">시작 RowIndex</param>
        /// <param name="intEndRowIndex">끝 RowIndex</param>
        /// <param name="strColumnName">컬럼 Name</param>
        /// <param name="strArrCellData">변경하려고 하는 셀 Data</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 셀 Data가 수정된다.
        /// </comment>
        public void subUpdateCellBlock(DataGridView grdDataGridView, int intStartRowIndex, int intEndRowIndex, string strColumnName, string[] strArrCellData)
        {
            try
            {
                int dintColumnIndex = grdDataGridView.Columns[strColumnName].Index;

                //아래 Overloading되어 있는 함수를 호출한다.
                subUpdateCellBlock(grdDataGridView, intStartRowIndex, intEndRowIndex, dintColumnIndex, strArrCellData);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 컬럼 데이터를 수정한다.(Row범위를 지정해서 Data를 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intStartRowIndex">시작 RowIndex</param>
        /// <param name="intEndRowIndex">끝 RowIndex</param>
        /// <param name="intColumnIndex">컬럼Index</param>
        /// <param name="strArrCellData">변경하려고 하는 셀 Data</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 셀 Data가 수정된다.
        /// </comment>
        public void subUpdateCellBlock(DataGridView grdDataGridView, int intStartRowIndex, int intEndRowIndex, int intColumnIndex, string[] strArrCellData)
        {
            BindingSource dBS = null;
            DataTable dDT = null;
            int dintDataIndex = 0;

            try
            {
                //Column의 Cell에 값을 입력하고자 하는 길이와 인자로 넘어온 길이가 다르면 그냥 빠져 나간다.
                if ((intEndRowIndex - intStartRowIndex + 1) != strArrCellData.Length) return;
              
                this.pgrdDataGridView = grdDataGridView;
                if (this.pgrdDataGridView.DataSource.GetType().Name == "DataTable")
                {
                    dDT = (DataTable)this.pgrdDataGridView.DataSource;
                }
                else if (this.pgrdDataGridView.DataSource.GetType().Name == "BindingSource")
                {
                    //DataGridView에 바인딩된 객체가 BindingSource이면 BindingSource로 형변환 후 DataTable을 가져온다.
                    dBS = (BindingSource)this.pgrdDataGridView.DataSource;
                    dDT = (DataTable)dBS.DataSource;
                }

                for (int dintLoop = intStartRowIndex; dintLoop <= intEndRowIndex; dintLoop++)
                {
                    if (dintLoop >= this.pgrdDataGridView.RowCount - 1)     //만약 RowIndex범위를 벗어나면 루프를 빠져나간다.
                    {
                        break;
                    }
                    this.pgrdDataGridView.Rows[dintLoop].Cells[intColumnIndex].Value = strArrCellData[dintDataIndex];   //Cell data 변경

                    dintDataIndex = dintDataIndex + 1;  //배열 인덱스 증가
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 Cell 데이터를 수정한다.(Block 범위로 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intStartRowIndex">시작 RowIndex</param>
        /// <param name="intEndRowIndex">끝 RowIndex</param>
        /// <param name="intStartColumnIndex">시작 ColumnIndex</param>
        /// <param name="intEndColumnIndex">끝 ColumnIndex</param>
        /// <param name="strCellData">변경하려고 하는 셀 Data</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 셀 Data가 수정된다.
        /// </comment>
        public void subUpdateCellBlock(DataGridView grdDataGridView, int intStartRowIndex, int intEndRowIndex, int intStartColumnIndex, int intEndColumnIndex, string[,] strCellData)
        {
            BindingSource dBS = null;
            DataTable dDT = null;
            int dintRow = 0;
            int dintColumn = 0;

            try
            {
                //Column의 Cell에 값을 입력하고자 하는 길이와 인자로 넘어온 길이가 다르면 그냥 빠져 나간다.
                if ((intEndRowIndex - intStartRowIndex + 1) != strCellData.GetLength(0)) return;
                if ((intEndColumnIndex - intStartColumnIndex + 1) != strCellData.GetLength(1)) return;

                this.pgrdDataGridView = grdDataGridView;
                if (this.pgrdDataGridView.DataSource.GetType().Name == "DataTable")
                {
                    dDT = (DataTable)this.pgrdDataGridView.DataSource;
                }
                else if (this.pgrdDataGridView.DataSource.GetType().Name == "BindingSource")
                {
                    //DataGridView에 바인딩된 객체가 BindingSource이면 BindingSource로 형변환 후 DataTable을 가져온다.
                    dBS = (BindingSource)this.pgrdDataGridView.DataSource;
                    dDT = (DataTable)dBS.DataSource;
                }

                for (int dintRowIndex = intStartRowIndex; dintRowIndex <= intEndRowIndex; dintRowIndex++)
                {
                    for (int dintColumnIndex = intStartColumnIndex; dintColumnIndex <= intEndColumnIndex; dintColumnIndex++)
                    {
                        subUpdateCell(this.pgrdDataGridView, dintRowIndex, dintColumnIndex, strCellData[dintRow, dintColumn].ToString());

                        dintColumn = dintColumn + 1;          //배열의 열 인덱스 증가
                    }
                    dintColumn = 0;                 //배열의 열 인덱스 초기화
                    dintRow = dintRow + 1;          //배열의 행 인덱스 증가
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        #endregion

        #region "DataGridView의 기타 함수"
        /// <summary>
        /// DataGridView의 ColumnWidth를 조절한다.
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intArrColumnWidth">변경하려고 하는 셀 Width</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 셀 Width가 변경된다.
        /// </comment>
        public void subResizeColumnWidth(DataGridView grdDataGridView, params int[] intArrColumnWidth)
        {
            try
            {
                //DataGridView의 컬럼개수와 설정하려고 하는 ColumnWidth 개수가 맞지 않으면 빠져나간다.
                if (grdDataGridView.ColumnCount != intArrColumnWidth.Length)
                {
                    return;
                }

                this.pgrdDataGridView = grdDataGridView;

                // 열너비를 수동조정해 GridView의 너비에 맞춘다.
                for (int dintLoop = 0; dintLoop < intArrColumnWidth.Length; dintLoop++)
                {
                    this.pgrdDataGridView.Columns[dintLoop].Width = Convert.ToInt32(intArrColumnWidth[dintLoop]);
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 Row의 CellStyle을 변경한다.(Row범위를 지정해서 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intStartRowIndex">시작 RowIndex</param>
        /// <param name="intEndRowIndex">끝 RowIndex</param>
        /// <param name="cBackColor">Row의 배경색</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 RowStyle이 변경된다.
        /// </comment>
        public void subChangeRowStyle(DataGridView grdDataGridView, int intStartRowIndex, int intEndRowIndex, Color cBackColor)
        {
            try
            {
                Color dcForeColor = grdDataGridView.ForeColor;
                FontStyle dfsFontStyle = grdDataGridView.Font.Style;

                //아래 Overloading되어 있는 함수를 호출한다.
                subChangeRowStyle(grdDataGridView, intStartRowIndex, intEndRowIndex, cBackColor, dcForeColor, dfsFontStyle);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 Row의 CellStyle을 변경한다.(Row범위를 지정해서 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intStartRowIndex">시작 RowIndex</param>
        /// <param name="intEndRowIndex">끝 RowIndex</param>
        /// <param name="cBackColor">Row의 배경색</param>
        /// <param name="cForeColor">Row의 글꼴색</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 RowStyle이 변경된다.
        /// </comment>
        public void subChangeRowStyle(DataGridView grdDataGridView, int intStartRowIndex, int intEndRowIndex, Color cBackColor, Color cForeColor)
        {
            try
            {
                FontStyle dfsFontStyle = grdDataGridView.Font.Style;

                //아래 Overloading되어 있는 함수를 호출한다.
                subChangeRowStyle(grdDataGridView, intStartRowIndex, intEndRowIndex, cBackColor, cForeColor, dfsFontStyle);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 Row의 CellStyle을 변경한다.(Row범위를 지정해서 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intStartRowIndex">시작 RowIndex</param>
        /// <param name="intEndRowIndex">끝 RowIndex</param>
        /// <param name="cBackColor">Row의 배경색</param>
        /// <param name="cForeColor">Row의 글꼴색</param>
        /// <param name="fsFontStyle">FontStyle</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 RowStyle이 변경된다.
        /// </comment>
        public void subChangeRowStyle(DataGridView grdDataGridView, int intStartRowIndex, int intEndRowIndex, Color cBackColor, Color cForeColor, FontStyle fsFontStyle)
        {
            try
            {
                this.pgrdDataGridView = grdDataGridView;

                DataGridViewCellStyle dCellStype = new DataGridViewCellStyle();     //적용할 CellStyle을 생성한다.
                dCellStype.BackColor = cBackColor;
                dCellStype.ForeColor = cForeColor;
                dCellStype.Font = new Font(this.pgrdDataGridView.Font, fsFontStyle);

                //StartRowIndex, EndRowIndex 모두 -1이면 전체 행을 변경한다.
                if (intStartRowIndex == -1 && intEndRowIndex == -1)
                {
                    intStartRowIndex = 0;
                    intEndRowIndex = this.pgrdDataGridView.RowCount - 1;
                }

                for (int dintLoop = intStartRowIndex; dintLoop <= intEndRowIndex; dintLoop++)
                {
                    if (dintLoop >= this.pgrdDataGridView.RowCount - 1)     //만약 RowIndex범위를 벗어나면 루프를 빠져나간다.
                    {
                        break;
                    }

                    this.pgrdDataGridView.Rows[dintLoop].DefaultCellStyle = dCellStype;     //Row에 CellStype 적용
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 Column의 CellStyle을 변경한다.(Column 전체를 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intColumnIndex">컬럼 Index</param>
        /// <param name="cBackColor">Row의 배경색</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 ColumnStyle이 변경된다.
        /// </comment>
        public void subChangeColumnStyle(DataGridView grdDataGridView, int intColumnIndex, Color cBackColor)
        {
            try
            {
                string dstrColumnName = grdDataGridView.Columns[intColumnIndex].Name;
                Color dcForeColor = grdDataGridView.ForeColor;
                FontStyle dfsFontStyle = grdDataGridView.Font.Style;

                //아래 Overloading되어 있는 함수를 호출한다.
                subChangeColumnStyle(grdDataGridView, dstrColumnName, cBackColor, dcForeColor, dfsFontStyle);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 Column의 CellStyle을 변경한다.(Column 전체를 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="strColumnName">컬럼 Name</param>
        /// <param name="cBackColor">Row의 배경색</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 ColumnStyle이 변경된다.
        /// </comment>
        public void subChangeColumnStyle(DataGridView grdDataGridView, string strColumnName, Color cBackColor)
        {
            try
            {
                Color dcForeColor = grdDataGridView.ForeColor;
                FontStyle dfsFontStyle = grdDataGridView.Font.Style;

                //아래 Overloading되어 있는 함수를 호출한다.
                subChangeColumnStyle(grdDataGridView, strColumnName, cBackColor, dcForeColor, dfsFontStyle);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 Column의 CellStyle을 변경한다.(Column 전체를 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intColumnIndex">컬럼 Index</param>
        /// <param name="cBackColor">Row의 배경색</param>
        /// <param name="cForeColor">Row의 글꼴색</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 ColumnStyle이 변경된다.
        /// </comment>
        public void subChangeColumnStyle(DataGridView grdDataGridView, int intColumnIndex, Color cBackColor, Color cForeColor)
        {
            try
            {
                string dstrColumnName = grdDataGridView.Columns[intColumnIndex].Name;
                FontStyle dfsFontStyle = grdDataGridView.Font.Style;

                //아래 Overloading되어 있는 함수를 호출한다.
                subChangeColumnStyle(grdDataGridView, dstrColumnName, cBackColor, cForeColor, dfsFontStyle);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 Column의 CellStyle을 변경한다.(Column 전체를 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="strColumnName">컬럼 Name</param>
        /// <param name="cBackColor">Row의 배경색</param>
        /// <param name="cForeColor">Row의 글꼴색</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 ColumnStyle이 변경된다.
        /// </comment>
        public void subChangeColumnStyle(DataGridView grdDataGridView, string strColumnName, Color cBackColor, Color cForeColor)
        {
            try
            {
                FontStyle dfsFontStyle = grdDataGridView.Font.Style;

                //아래 Overloading되어 있는 함수를 호출한다.
                subChangeColumnStyle(grdDataGridView, strColumnName, cBackColor, cForeColor, dfsFontStyle);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 Column의 CellStyle을 변경한다.(Column 전체를 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="intColumnIndex">컬럼 Index</param>
        /// <param name="cBackColor">Row의 배경색</param>
        /// <param name="cForeColor">Row의 글꼴색</param>
        /// <param name="fsFontStyle">FontStyle</param>
        /// <comment>
        /// 메소드 호출시 자동으로 DataGridView의 ColumnStyle이 변경된다.
        /// </comment>
        public void subChangeColumnStyle(DataGridView grdDataGridView, int intColumnIndex, Color cBackColor, Color cForeColor, FontStyle fsFontStyle)
        {
            try
            {
                string dstrColumnName = grdDataGridView.Columns[intColumnIndex].Name;

                //아래 Overloading되어 있는 함수를 호출한다.
                subChangeColumnStyle(grdDataGridView, dstrColumnName, cBackColor, cForeColor, fsFontStyle);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataGridView에서 Column의 CellStyle을 변경한다.(Column 전체를 수정)
        /// </summary>
        /// <param name="grdDataGridView">DataGridView</param>
        /// <param name="strColumnName">컬럼 Name</param>
        /// <param name="cBackColor">Row의 배경색</param>
        /// <param name="cForeColor">Row의 글꼴색</param>
        /// <param name="fsFontStyle">FontStyle</param>
        /// <commnet>
        /// 메소드 호출시 자동으로 DataGridView의 ColumnStyle이 변경된다.
        /// </commnet>
        public void subChangeColumnStyle(DataGridView grdDataGridView, string strColumnName, Color cBackColor, Color cForeColor, FontStyle fsFontStyle)
        {
            try
            {
                this.pgrdDataGridView = grdDataGridView;

                DataGridViewCellStyle dCellStype = new DataGridViewCellStyle();     //적용할 CellStyle을 생성한다.
                dCellStype.BackColor = cBackColor;
                dCellStype.ForeColor = cForeColor;
                dCellStype.Font = new Font(this.pgrdDataGridView.Font, fsFontStyle);

                //만약 홀수번째 행에 CellStyle이 적용되어 있으면 초기화한다.(아무것도 적용안함)
                //this.pgrdDataGridView.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle();
                this.pgrdDataGridView.Columns[strColumnName].DefaultCellStyle = dCellStype;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataTable -> 2차원배열로 변환한다.
        /// </summary>
        /// <param name="dtDataTable">DataTable</param>
        /// <returns>2차원배열</returns>
        public string[,] funDataTableTo2Array(DataTable dtDataTable)
        {
            string[,] dstrArr = null;

            try
            {
                int intRowSize = dtDataTable.Rows.Count;            //DataTable의 행 개수 -> 2차원배열의 행 개수
                int intColumnSize = dtDataTable.Columns.Count;      //DataTable의 열 개수 -> 2차원배열의 열 개수

                dstrArr = new string[intRowSize, intColumnSize];     //2차원배열 생성

                for (int dintRowIndex = 0; dintRowIndex < intRowSize; dintRowIndex++)
                {
                    for (int dintColumnIndex = 0; dintColumnIndex < intColumnSize; dintColumnIndex++)
                    {
                        dstrArr[dintRowIndex, dintColumnIndex] = dtDataTable.Rows[dintRowIndex][dintColumnIndex].ToString();
                    }
                }

                return dstrArr;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 2차원배열 -> DataTable로 변환한다.
        /// </summary>
        /// <param name="strArr">2차원배열</param>
        /// <param name="strArrColumnHeader">ColumnHeader 텍스트</param>
        /// <returns>DataTable</returns>
        public DataTable fun2ArrayToDataTable(string[,] strArr, params string[] strArrColumnHeader)
        {
            try
            {
                //1차원배열이 Null이거나 데이터가 없으면 그냥 빠져나간다.
                if (strArr == null || strArr.Length <= 0) return null;

                //컬럼 개수와 컬럼헤더 개수가 같지 않으면 그냥 빠져나간다.
                if (strArrColumnHeader.Length != 0 && strArrColumnHeader.Length != strArr.GetLength(1)) return null;

                DataTable dDT = new DataTable("NewTable");       //반환할 DataTable을 생성한다.
                DataColumn dDC;
                DataRow dDR;

                for (int dintColumnIndex = 0; dintColumnIndex < strArr.GetLength(1); dintColumnIndex++)  //컬럼을 생성한다.
                {
                    dDC = new DataColumn();
                    dDC.DataType = Type.GetType("System.String");   //Column은 String형식으로 한다.
                    if (strArrColumnHeader.Length != 0)
                    {
                        dDC.ColumnName = strArrColumnHeader[dintColumnIndex].ToString();
                        dDC.Caption = strArrColumnHeader[dintColumnIndex].ToString();
                    }
                    else
                    {
                        dDC.ColumnName = "Column" + dintColumnIndex;
                        dDC.Caption = "Column" + dintColumnIndex;
                    }

                    dDT.Columns.Add(dDC);        //생성된 컬럼을 DataTable에 추가한다.
                }

                for (int dintRowIndex = 0; dintRowIndex < strArr.GetLength(0); dintRowIndex++)       //행 데이터를 추가한다.
                {
                    dDR = dDT.NewRow();

                    for (int dintColumnIndex = 0; dintColumnIndex < strArr.GetLength(1); dintColumnIndex++)
                    {
                        dDR[dintColumnIndex] = strArr[dintRowIndex, dintColumnIndex];
                    }
                    dDT.Rows.Add(dDR);           //생성한 행 데이터를 DataTable에 추가한다.
                }

                return dDT;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// ArrayList -> DataTable로 변환한다.
        /// </summary>
        /// <param name="ArrList">ArrayList</param>
        /// <param name="strDelimiter">구분자(Delimiter)</param>
        /// <param name="strArrColumnHeader">ColumnHeader 텍스트</param>
        /// <returns>DataTable</returns>
        public DataTable funArrayListToDataTable(ArrayList ArrList, string strDelimiter, params string[] strArrColumnHeader)
        {
            int dintColumnCount = 0;
            string[] dstrArrData = null;
            string dstrTemp = "";
            char chDelimiter = strDelimiter[0];

            try
            {
                //ArrList가 Null이거나 Data가 없으면 그냥 빠져나간다.
                if (ArrList == null || ArrList.Count <= 0) return null;


                //생성할 DataTable의 컬럼개수를 가져온다.
                dstrTemp = ArrList[0].ToString().Trim();

                //가져온 Data가 Empty이거나 Null이면 그냥 빠져나간다.
                if (dstrTemp == String.Empty || dstrTemp == null) return null;

                if (dstrTemp.EndsWith(strDelimiter) == true)     //마지막에 콤마(,)가 있으면 제거한다.
                {
                    dstrTemp = dstrTemp.Remove(dstrTemp.Length - 1, 1);
                }
                //ArrayList의 첫번째 항목의 콤마로 구분된 데이터 개수가 컬럼의 개수가 된다.
                dintColumnCount = dstrTemp.Split(chDelimiter).Length;

                //컬럼 개수와 컬럼헤더 개수가 같지 않으면 그냥 빠져나간다.
                if (strArrColumnHeader.Length != 0 && strArrColumnHeader.Length != dintColumnCount) return null;



                DataTable dDT = new DataTable("NewTable");       //반환할 DataTable을 생성한다.
                DataColumn dDC;
                DataRow dDR;

                for (int dintColumnIndex = 0; dintColumnIndex < dintColumnCount; dintColumnIndex++)  //컬럼을 생성한다.
                {
                    dDC = new DataColumn();
                    dDC.DataType = Type.GetType("System.String");   //Column은 String형식으로 한다.

                    if (strArrColumnHeader.Length != 0)     //인자로 넘어온 ColumnHeader가 있으면 지정해준다.
                    {
                        dDC.ColumnName = strArrColumnHeader[dintColumnIndex].ToString();
                        dDC.Caption = strArrColumnHeader[dintColumnIndex].ToString();
                    }
                    else
                    {                                       //인자로 넘어온 ColumnHeader가 없으면 기본으로 한다.
                        dDC.ColumnName = "Column" + dintColumnIndex;
                        dDC.Caption = "Column" + dintColumnIndex;
                    }
                  

                    dDT.Columns.Add(dDC);        //생성된 컬럼을 DataTable에 추가한다.
                }

                for (int dintRowIndex = 0; dintRowIndex < ArrList.Count; dintRowIndex++)       //행 데이터를 추가한다.
                {
                    dDR = dDT.NewRow();

                    dstrTemp = ArrList[dintRowIndex].ToString();
                    if (dstrTemp.EndsWith(",") == true)
                    {
                        dstrTemp = dstrTemp.Remove(dstrTemp.Length - 1, 1); //마지막에 콤마(,)가 있으면 제거한다.
                    }
                    dstrArrData = dstrTemp.Split(new char[] { ',' });

                    for (int dintColumnIndex = 0; dintColumnIndex < dintColumnCount; dintColumnIndex++)
                    {
                        dDR[dintColumnIndex] = dstrArrData[dintColumnIndex];
                    }
                    dDT.Rows.Add(dDR);           //생성한 행 데이터를 DataTable에 추가한다.
                }

                return dDT;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataTable -> ArrayList로 변환한다.
        /// </summary>
        /// <param name="dtDataTable">DataTable</param>
        /// <param name="strDelimiter">구분자(Delimiter)</param>
        /// <returns>ArrayList</returns>
        public ArrayList funDataTableToArrayList(DataTable dtDataTable, string strDelimiter)
        {
            int dintColumnCount = 0;
            int dintRowCount = 0;
            string dstrData = "";
            ArrayList dAL = new ArrayList();

            try
            {
                //DataTable이 Null이거나 Data가 없으면 그냥 빠져나간다.
                if (dtDataTable == null || dtDataTable.Rows.Count <= 0) return null;

                dintColumnCount = dtDataTable.Columns.Count;    //컬럼개수를 가져온다.
                dintRowCount = dtDataTable.Rows.Count;

                for (int dintRowIndex = 0; dintRowIndex < dintRowCount; dintRowIndex++)       
                {
                    for (int dintColumnIndex = 0; dintColumnIndex < dintColumnCount; dintColumnIndex++)
                    {
                        dstrData = dstrData + dtDataTable.Rows[dintRowIndex][dintColumnIndex].ToString() + strDelimiter;
                    }

                    if (dstrData.EndsWith(strDelimiter) == true)
                    {
                        dstrData = dstrData.Remove(dstrData.Length - 1, 1); //마지막에 콤마(,)가 있으면 제거한다.
                    }

                    dAL.Add(dstrData);
                    dstrData = "";  //초기화
                }

                return dAL;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 1차원배열 -> DataTable로 변환한다.
        /// </summary>
        /// <param name="strArrData">1차원배열</param>
        /// <returns>1차원배열을 DataTbale로 변환한 것</returns>
        /// <commnet>
        /// 1차원 배열 값이 0,1,2, ... , 19까지 20개가 있으면
        /// 인자로 넘어온 DataTable의 Column개수로 잘라서 DataTable을 생성한다.
        /// </commnet>
        public DataTable fun1ArrayToDataTable(string[] strArrData)
        {
            DataTable dDT = null;
            string[] dstrArrColumnHeader = null;

            try
            {
                //1차원배열이 Null이거나 데이터가 없으면 그냥 빠져나간다.
                if (strArrData == null || strArrData.Length <= 0) return null;

                //생성하려고 하는 DataTable의 Column길이가 없으면 그냥 빠져나간다.
                //if (intColumnCount <= 0) return null;

                dstrArrColumnHeader = new string[strArrData.Length];

                //DataTable의 ColumnHeader를 저장한다.
                for (int dintColumnIndex = 0; dintColumnIndex < dstrArrColumnHeader.Length; dintColumnIndex++)
                {
                    dstrArrColumnHeader[dintColumnIndex] = "ColumnHeader" + dintColumnIndex.ToString();
                }

                dDT = fun1ArrayToDataTable(strArrData, dstrArrColumnHeader.Length, dstrArrColumnHeader);

                return dDT;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 1차원배열 -> DataTable로 변환한다.
        /// </summary>
        /// <param name="strArrData">1차원배열</param>
        /// <param name="intColumnCount">DataTable의 컬럼개수</param>
        /// <returns>1차원배열을 DataTbale로 변환한 것</returns>
        /// <comment>
        /// 1차원 배열 값이 0,1,2, ... , 19까지 20개가 있으면
        /// 인자로 넘어온 DataTable의 Column개수로 잘라서 DataTable을 생성한다.
        /// </comment>
        public DataTable fun1ArrayToDataTable(string[] strArrData, int intColumnCount)
        {
            DataTable dDT = null;
            string[] dstrArrColumnHeader = new string[intColumnCount];

            try
            {
                //1차원배열이 Null이거나 데이터가 없으면 그냥 빠져나간다.
                if (strArrData == null || strArrData.Length <= 0) return null;

                //생성하려고 하는 DataTable의 Column길이가 없으면 그냥 빠져나간다.
                //if (intColumnCount <= 0) return null;

                //DataTable의 ColumnHeader를 저장한다.
                for (int dintColumnIndex = 0; dintColumnIndex < intColumnCount; dintColumnIndex++)
                {
                    dstrArrColumnHeader[dintColumnIndex] = "ColumnHeader" + dintColumnIndex.ToString();
                }

                dDT = fun1ArrayToDataTable(strArrData, intColumnCount, dstrArrColumnHeader);

                return dDT;

            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 1차원배열 -> DataTable로 변환한다.
        /// </summary>
        /// <param name="strArrData">1차원배열</param>
        /// <param name="intColumnCount">DataTable의 컬럼개수</param>
        /// <param name="strArrColumnHeader">DataTable의 ColumnHeader 텍스트</param>
        /// <returns>1차원배열을 DataTbale로 변환한 것</returns>
        /// <comment>
        /// 1차원 배열 값이 0,1,2, ... , 19까지 20개가 있으면
        /// 인자로 넘어온 DataTable의 Column개수로 잘라서 DataTable을 생성한다.
        /// </comment>
        public DataTable fun1ArrayToDataTable(string[] strArrData, int intColumnCount, params string[] strArrColumnHeader)
        {
            int dintRowCount = 0;
            int dintIndex = 0;

            try
            {
                //1차원배열이 Null이거나 데이터가 없으면 그냥 빠져나간다.
                if (strArrData == null || strArrData.Length <= 0) return null;

                //생성하려고 하는 DataTable의 Column길이가 없으면 그냥 빠져나간다.
                //if (intColumnCount <= 0) return null;

                //컬럼 개수와 컬럼헤더 개수가 같지 않으면 그냥 빠져나간다.
                if (intColumnCount != strArrColumnHeader.Length) return null;


                //1차원 배열 길이를 가지고 생성할 DataTable의 Row개수를 가져온다.
                if ((strArrData.Length % intColumnCount) == 0)
                {
                    dintRowCount = strArrData.Length / intColumnCount;
                }
                else
                {
                    dintRowCount = (strArrData.Length / intColumnCount) + 1;
                }


                DataTable dDT = new DataTable("NewTable");       //반환할 DataTable을 생성한다.
                DataColumn dDC;
                DataRow dDR;


                //컬럼을 생성해서 DataTable에 추가한다.
                for (int dintColumnIndex = 0; dintColumnIndex < intColumnCount; dintColumnIndex++)  
                {
                    dDC = new DataColumn();
                    dDC.DataType = Type.GetType("System.String");   //Column은 String형식으로 한다.
                    dDC.ColumnName = strArrColumnHeader[dintColumnIndex].ToString();    //인자로 넘어온 ColumnHeader를 지정해준다.
                    dDC.Caption = strArrColumnHeader[dintColumnIndex].ToString();
                    
                    dDT.Columns.Add(dDC);        //생성된 컬럼을 DataTable에 추가한다.
                }


                //생성된 DataTable에 행 데이터를 추가한다.
                for (int dintRowIndex = 0; dintRowIndex < dintRowCount; dintRowIndex++)       //행 데이터를 추가한다.
                {
                    dDR = dDT.NewRow();

                    for (int dintColumnIndex = 0; dintColumnIndex < intColumnCount; dintColumnIndex++)
                    {
                        if (dintIndex < strArrData.Length)
                        {
                            dDR[dintColumnIndex] = strArrData[dintIndex];      //1차원 배열의 값을 DataRow에 추가한다.
                            dintIndex = dintIndex + 1;                          //배열 Index를 증가시킨다.
                        }

                    }
                    dDT.Rows.Add(dDR);           //생성한 행 데이터를 DataTable에 추가한다.
                }

                return dDT;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// DataTable -> 1차원배열로 변환한다.
        /// </summary>
        /// <param name="dtDataTable">DataTable</param>
        /// <returns>1차원배열</returns>
        public string[] funDataTableTo1Array(DataTable dtDataTable)
        {
            int dintColumnCount = 0;
            int dintRowCount = 0;
            int dintIndex = 0;
            string[] dstrArrData = null;

            try
            {
                //DataTable이 Null이거나 Data가 없으면 그냥 빠져나간다.
                //if (dtDataTable == null || dtDataTable.Rows.Count <= 0) return null;


                dintColumnCount = dtDataTable.Columns.Count;    //컬럼개수를 가져온다.
                dintRowCount = dtDataTable.Rows.Count;          //Row개수를 가져온다.

                //배열의 Size를 구한다.
                dstrArrData = new string[dintColumnCount * dintRowCount];

                for (int dintRowIndex = 0; dintRowIndex < dintRowCount; dintRowIndex++)      
                {
                    for (int dintColumnIndex = 0; dintColumnIndex < dintColumnCount; dintColumnIndex++)
                    {
                        dstrArrData[dintIndex] = dtDataTable.Rows[dintRowIndex][dintColumnIndex].ToString();    //1차원배열에 값을 저장한다.
                        dintIndex = dintIndex + 1;
                    }
                }

                return dstrArrData;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 1차원배열 -> ArrayList로 변환한다.
        /// </summary>
        /// <param name="strArrData">1차원배열</param>
        /// <returns>ArrayList</returns>
        public ArrayList fun1ArrayToArrayList(string[] strArrData)
        {
            ArrayList dAL = new ArrayList();

            try
            {
                //1차원배열이 Null이거나 데이터가 없으면 그냥 빠져나간다.
                if (strArrData == null || strArrData.Length <= 0) return null;

                //1차원 배열 Data를 ArrayList로 입력한다.
                foreach (string strData in strArrData)
                {
                    dAL.Add(strData);
                }

                return dAL;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// ArrayList -> 1차원배열로 변환한다.
        /// </summary>
        /// <param name="ArrList">ArrayList</param>
        /// <returns>1차원배열</returns>
        public string[] funArrayListTo1Array(ArrayList ArrList)
        {
            string[] dstrArrData = null;
            int dintIndex = 0;

            try
            {
                //ArrList가 Null이거나 Data가 없으면 그냥 빠져나간다.
                if (ArrList == null || ArrList.Count <= 0) return null;

                dstrArrData = new string[ArrList.Count];

                //ArrayList Data를 1차원 배열에 입력한다.
                foreach (object objData in ArrList)
                {
                    dstrArrData[dintIndex] = objData.ToString();
                    dintIndex = dintIndex + 1;
                }

                return dstrArrData;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 인자로 넘어온 DataTable에 새행을 추가해서 DataTable을 리턴한다.
        /// </summary>
        /// <param name="dtDataTable">DataTable</param>
        /// <param name="strArrRowData">추가할 행 데이터</param>
        /// <returns>새행을 추가한 DataTable</returns>
        /// <comment>
        /// 리턴된 DataTable을 응용 프로그램단에서 바인딩해주어야 한다.
        /// </comment>
        public DataTable funAddRowToDataTable(DataTable dtDataTable, string[] strArrRowData)
        {
            DataTable dDT = null;            

            try
            {
                //DataTable이 Null이거나 Data가 없으면 그냥 빠져나간다.
                if (dtDataTable == null) return null;

                //1차원배열이 Null이거나 데이터가 없으면 그냥 빠져나간다.
                if (strArrRowData == null || strArrRowData.Length <= 0) return null;

                //DataTable의 컬럼길이와 인자로 넘어온 배열의 길이가 맞지 않으면 그냥 빠져나간다.
                if (dtDataTable.Columns.Count != strArrRowData.Length) return null;

                dDT = dtDataTable;
                dDT.Rows.Add(strArrRowData);
            }
            catch
            {
                throw new Exception();
            }

            return dDT;
        }

        /// <summary>
        /// 인자로 넘어온 DataTable에서 행을 삭제하여 DataTable을 리턴한다.
        /// </summary>
        /// <param name="dtDataTable">DataTable</param>
        /// <param name="intRowIndex">행 인덱스</param>
        /// <returns>행을 삭제한 DataTable</returns>
        /// <comment>
        /// 리턴된 DataTable을 응용 프로그램단에서 바인딩해주어야 한다.
        /// </comment>
        public DataTable funDeleteRowFromDataTable(DataTable dtDataTable, int intRowIndex)
        {
            DataTable dDT = null;

            try
            {
                //DataTable이 Null이거나 Data가 없으면 그냥 빠져나간다.
                if (dtDataTable == null || dtDataTable.Rows.Count <= 0) return null;

                dDT = dtDataTable;
                dDT.Rows.RemoveAt(intRowIndex);

                return dDT;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 인자로 넘어온 DataTable에서 행을 수정하여 DataTable을 리턴한다.
        /// </summary>
        /// <param name="dtDataTable">DataTable</param>
        /// <param name="intRowIndex">행 인덱스</param>
        /// <param name="strArrRowData">변경할 행 데이터</param>
        /// <returns>행을 수정한 DataTable</returns>
        /// <comment>
        /// 리턴된 DataTable을 응용 프로그램단에서 바인딩해주어야 한다.
        /// </comment>
        public DataTable funUpdateRowFromDataTable(DataTable dtDataTable, int intRowIndex, string[] strArrRowData)
        {
            DataTable dDT = null;

            try
            {
                dDT = dtDataTable;

                //DataTable이 Null이거나 Data가 없으면 그냥 빠져나간다.
                if (dDT == null || dDT.Rows.Count <= 0) return dDT;

                //DataGridView의 컬럼개수와 새로 입력하려고 하는 컬럼데이터 개수가 맞지 않으면 빠져나간다.
                if (dDT.Columns.Count != strArrRowData.Length)
                {
                    return dDT;
                }

                //DataTable에 데이터를 업데이트한다.
                for (int dintLoop = 0; dintLoop < dDT.Columns.Count; dintLoop++)
                {
                    dDT.Rows[intRowIndex][dintLoop] = strArrRowData[dintLoop];
                }

            }
            catch
            {
                throw new Exception();
            }

            return dDT;
        }

        /// <summary>
        /// 인자로 넘어온 DataTable에서 셀내용을 수정하여 DataTable을 리턴한다.
        /// </summary>
        /// <param name="dtDataTable">DataTable</param>
        /// <param name="intRowIndex">행 인덱스</param>
        /// <param name="strColumnName">컬럼Name</param>
        /// <param name="strCellData">변경하려고 하는 셀 Data</param>
        /// <returns>셀내용을 수정한 DataTable</returns>
        /// <comment>
        /// 리턴된 DataTable을 응용 프로그램단에서 바인딩해주어야 한다.
        /// </comment>
        public DataTable funUpdateCellFromDataTable(DataTable dtDataTable, int intRowIndex, string strColumnName, string strCellData)
        {
            DataTable dDT = null;

            try
            {
                int dintColumnIndex = dtDataTable.Columns[strColumnName].Ordinal;

                //아래 Overloading되어 있는 함수를 호출한다.
                dDT = funUpdateCellFromDataTable(dtDataTable, intRowIndex, dintColumnIndex, strCellData);
            }
            catch
            {
                throw new Exception();
            }

            return dDT;
        }

        /// <summary>
        /// 인자로 넘어온 DataTable에서 셀내용을 수정하여 DataTable을 리턴한다.
        /// </summary>
        /// <param name="dtDataTable">DataTable</param>
        /// <param name="intRowIndex">행 인덱스</param>
        /// <param name="intColumnIndex">컬럼Index</param>
        /// <param name="strCellData">변경하려고 하는 셀 Data</param>
        /// <returns>셀내용을 수정한 DataTable</returns>
        /// <comment>
        /// 리턴된 DataTable을 응용 프로그램단에서 바인딩해주어야 한다.
        /// </comment>
        public DataTable funUpdateCellFromDataTable(DataTable dtDataTable, int intRowIndex, int intColumnIndex, string strCellData)
        {
            DataTable dDT = null;

            try
            {
                dDT = dtDataTable;

                //DataTable이 Null이거나 Data가 없으면 그냥 빠져나간다.
                if (dDT == null || dDT.Rows.Count <= 0) return dDT;

                dDT.Rows[intRowIndex][intColumnIndex] = strCellData;
            }
            catch
            {
                throw new Exception();
            }

            return dDT;
        }

        /// <summary>
        /// 인자로 넘어온 DataTable에서 RowCount만큼 잘라서 해당 Page번호에 해당하는 DataTable을 리턴한다.
        /// </summary>
        /// <param name="dtDataTable">DataTable</param>
        /// <param name="intRowCount">행 개수</param>
        /// <param name="intPageNo">Page에 해당하는 DataTable</param>
        /// <returns>DataTable</returns>
        /// <comment>
        /// 리턴된 DataTable을 응용 프로그램단에서 바인딩해주어야 한다.
        /// </comment>
        public DataTable funDataTablePerPage(DataTable dtDataTable, int intRowCount, int intPageNo)
        {
            DataTable dDT = null;
            DataTable dDTTemp = null;
            int dintStartRowIndex = 0;
            int dintEndRowIndex = 0;

            try
            {
                dDT = dtDataTable;

                //DataTable이 Null이거나 Data가 없으면 그냥 빠져나간다.
                if (dDT == null) return dDT;

                //RowCount가 0 혹은 음수이거나 DataTable의 Row개수보다 많으면 그냥 빠져 나간다.
                if (intRowCount <= 0 || intRowCount > dDT.Rows.Count) return dDT;

                //PageNo가 0 혹은 음수이면 그냥 빠져 나간다.
                if (intPageNo <= 0) return dDT;


                dDTTemp = dDT.Clone();

                //시작, 종료 RowIndex를 구한다.
                if (dDT.Rows.Count <= intRowCount)     //한 페이지만 존재
                {
                    dintStartRowIndex = 0;
                    dintEndRowIndex = dDT.Rows.Count - 1;
                }
                else
                {                                       //두 페이지 이상 존재
                    dintStartRowIndex = intRowCount * (intPageNo - 1);
                    dintEndRowIndex = dintStartRowIndex + intRowCount - 1;

                    if (dintStartRowIndex >= dDT.Rows.Count)    //Row개수보다 더 큰 페이지 번호가 들어오면 마지막 페이지를 출력해준다.
                    {
                        dintStartRowIndex = dDT.Rows.Count - intRowCount;
                        dintEndRowIndex = dDT.Rows.Count - 1;
                    }
                }

                //전체 DataTable에서 필요한 부분만 Loop를 돌면서 뽑아온다.
                for (int dintLoop = dintStartRowIndex; dintLoop <= dintEndRowIndex; dintLoop++)
                {
                    if (dintLoop >= dDT.Rows.Count)
                    {
                        break;
                    }
                    else
                    {
                        dDTTemp.Rows.Add(dDT.Rows[dintLoop].ItemArray);
                    }
                }
                
            }
            catch
            {
                throw new Exception();
            }

            return dDTTemp;
        }

        /// <summary>
        /// DataTable에 새로운 컬럼을 추가한다.
        /// </summary>
        /// <param name="dtDataTable">DataTable</param>
        /// <param name="strColumnName">컬럼 이름</param>
        /// <param name="strColumnHeaderText">컬럼 HeaderText</param>
        /// <returns>컬럼을 추가한 DataTable</returns>
        /// <comment>
        /// 리턴된 DataTable을 응용 프로그램단에서 바인딩해주어야 한다.
        /// </comment>
        public DataTable funAddColumn(DataTable dtDataTable, string strColumnName, string strColumnHeaderText)
        {
            DataTable dDT = null;

            try
            {
                dDT = dtDataTable;

                DataColumn dDC = new DataColumn();
                dDC.Caption = strColumnHeaderText;                  //Column HeaderText
                dDC.ColumnName = strColumnName;
                dDC.DataType = Type.GetType("System.String");       //Column은 String형식으로 한다.

                dDT.Columns.Add(dDC);
            }
            catch
            {
                throw new Exception();
            }

            return dDT;
        }

        #endregion

    }
}
