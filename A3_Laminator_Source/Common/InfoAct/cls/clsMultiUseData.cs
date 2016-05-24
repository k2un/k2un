using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsMultiUseData
    {
        #region Values
        private Hashtable phtType = new Hashtable();
        
        #endregion


        #region Constructor
        public clsMultiUseData()
        {
        }
        #endregion


        #region Properties
        public int ITEM_COUNT
        {
            get
            {
                int dintCount = 0;

                foreach (string dstrType in this.phtType.Keys)
                {
                    dintCount += ((clsMultiUseDataByTYPE)this.phtType[dstrType]).COUNT;
                }

                return dintCount;
            }
        }
        public int TYPE_COUNT
        {
            get
            {
                return this.phtType.Count;
            }

        }

        #endregion

        #region Methods
        public bool TypeContains(string dstrTYPE)
        {
            return this.phtType.Contains(dstrTYPE);
        }

        public ICollection TYPES()
        {
            return this.phtType.Keys;
        }

        public ICollection TYPES_VALUE()
        {
            return this.phtType.Values;
        }

        public clsMultiUseDataByTYPE TYPES(string dstrTYPE)
        {
            if (this.phtType.Contains(dstrTYPE))
            {
                return (clsMultiUseDataByTYPE)this.phtType[dstrTYPE];
            }
            else return null;
        }

        public bool AddTYPE(clsMultiUseDataByTYPE clsTYPE)
        {
            try
            {
                if (clsTYPE == null) return false;

                if (!this.phtType.Contains(clsTYPE.DATA_TYPE)) this.phtType.Add(clsTYPE.DATA_TYPE, clsTYPE);

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
