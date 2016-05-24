using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsMultiUseDataByITEM
    {
        #region Values
        private int pintIndex = 0;
        private string pstrTYPE = string.Empty;
        private int pintTypeNum = 0;
        private string pstrITEM = string.Empty;
        private int pintItemNum = 0;
        private string pstrNameToPLC = string.Empty;
        private string pstrReference = string.Empty;
        private string pstrValue = string.Empty;
        private string pstrWriteValue = string.Empty;

        #endregion


        #region Constructor
        public clsMultiUseDataByITEM()
        {
        }
        #endregion


        #region Properties
        public int INDEX
        {
            get
            {
                return this.pintIndex;
            }
            set
            {
                this.pintIndex = value;
            }
        }
        public string DATA_TYPE
        {
            get
            {
                return this.pstrTYPE;
            }
            set
            {
                this.pstrTYPE = value;
            }
        }
        public int DATA_TYPE_NUM
        {
            get
            {
                return this.pintTypeNum;
            }
            set
            {
                this.pintTypeNum = value;
            }
        }
        public string ITEM
        {
            get
            {
                return this.pstrITEM;
            }
            set
            {
                this.pstrITEM = value;
            }
        }
        public int ITEM_NUM
        {
            get
            {
                return this.pintItemNum;
            }
            set
            {
                this.pintItemNum = value;
            }
        }
        public string REFERENCE
        {
            get
            {
                return this.pstrReference;
            }
            set
            {
                this.pstrReference = value;
            }
        }
        public string ITEM_NAME_TO_PLC
        {
            get
            {
                return this.pstrNameToPLC;
            }
            set
            {
                this.pstrNameToPLC = value;
            }
        }
        public string VALUE
        {
            get
            {
                return this.pstrValue;
            }
            set
            {
                this.pstrValue = value;
            }
        }
        public string WRITE_VALUE
        {
            get
            {
                return this.pstrWriteValue;
            }
            set
            {
                this.pstrWriteValue = value;
            }
        }
        #endregion



    }
}
