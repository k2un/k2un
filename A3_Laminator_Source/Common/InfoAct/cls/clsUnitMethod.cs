using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsUnitMethod
    {
        private Hashtable pSubUnitHash = new Hashtable();

        //SubUnit 개체등록 및 가져오기
        #region "SubUnit"

        public bool AddSubUnit(int intSubUnitID)
        {
            clsSubUnit dclsSubUnit;     //SubUnit 개체 선언

            try
            {
                if (intSubUnitID < 0)
                {
                    return false;
                }
                else
                {
                    if (pSubUnitHash.Contains(intSubUnitID))
                    {
                        return true;
                    }
                    else
                    {
                        dclsSubUnit = new clsSubUnit(intSubUnitID);
                        pSubUnitHash.Add(intSubUnitID, dclsSubUnit);

                        return true;
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

        public clsSubUnit SubUnit(int intSubUnitID)
        {
            try
            {
                if (pSubUnitHash.Contains(intSubUnitID))
                {
                    return (clsSubUnit)pSubUnitHash[intSubUnitID];
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

        public ICollection SubUnit()
        {
            try
            {
                return pSubUnitHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public int SubUnitCount
        {
            //Unit전체(SubUnitID=0) 개수는 제외한다.
            get { return pSubUnitHash.Count - 1; }            
        }

        #endregion
    }
}
