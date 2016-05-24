using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsRecipeMethod
    {
        private Hashtable pHashTableUnitRecipe = new Hashtable();

        //UnitRecipeID 개체등록 및 가져오기
        #region "UnitRecipeID"

        public bool AddUnitRecipeID(int intUnitID, string strRecipeID)
        {
            clsUnitRecipeID dclsUnitRecipeID;     //UnitRecipeID 개체 선언

            try
            {
                int dintUnitID = intUnitID;

                if (dintUnitID <= 0)
                {
                    return false;
                }
                else
                {
                    if (pHashTableUnitRecipe.Contains(dintUnitID))
                    {
                        return false;
                    }
                    else
                    {
                        dclsUnitRecipeID = new clsUnitRecipeID(dintUnitID, strRecipeID);
                        pHashTableUnitRecipe.Add(dintUnitID, dclsUnitRecipeID);

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

        public clsUnitRecipeID UnitRecipeID(int intUnitID)
        {
            try
            {
                int dintUnitID = intUnitID;

                if (pHashTableUnitRecipe.Contains(dintUnitID))
                {
                    return (clsUnitRecipeID)pHashTableUnitRecipe[dintUnitID];
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

        public ICollection UnitRecipeID()
        {
            try
            {
                return pHashTableUnitRecipe.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public int UnitRecipeIDCount
        {
            get { return pHashTableUnitRecipe.Count; }
        }

        #endregion
    }
}
