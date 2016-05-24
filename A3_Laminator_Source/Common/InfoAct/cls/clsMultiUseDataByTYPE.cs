using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsMultiUseDataByTYPE
    {
        #region Values
        private string pstrTYPE = string.Empty;
        private int pintTypeNum = 0;

        private Hashtable phtITEMS = new Hashtable();
        
        #endregion


        #region Constructor
        public clsMultiUseDataByTYPE(string dstrTYPE, int dintTypeNum)
        {
            this.pstrTYPE = dstrTYPE;
            this.pintTypeNum = dintTypeNum;
        }
        #endregion


        #region Properties
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
        //public ICollection ITEMS
        //{
        //    get
        //    {
        //        return this.phtITEMS.Keys;
        //    }
        //}
        public int COUNT
        {
            get
            {
                return this.phtITEMS.Count;
            }
        }
        #endregion

        #region Methods
        public bool ItemContains(string dstrITEM)
        {
            return this.phtITEMS.Contains(dstrITEM);
        }

        public ICollection ITEMS_VALUE()
        {
            return this.phtITEMS.Values;
        }

        public ICollection ITEMS()
        {
            return this.phtITEMS.Keys;
        }

        public clsMultiUseDataByITEM ITEMS(string dstrName)
        {
            if (this.phtITEMS.Contains(dstrName))
            {
                return (clsMultiUseDataByITEM)this.phtITEMS[dstrName];
            }
            else return null;
        }

        public bool AddITEM(clsMultiUseDataByITEM clsData)
        {
            try
            {
                if (clsData == null) return false;

                if (this.phtITEMS.Contains(clsData.ITEM))
                {
                    this.phtITEMS.Remove(clsData.ITEM);
                }

                this.phtITEMS.Add(clsData.ITEM, clsData);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
