using System;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsGroup
    {
        public int GroupID = 0;                                 //Group ID(Key 값)
        public string ReadTime = "";                            //SVID를 저장한 시간

        private Hashtable pHashtableSVID = new Hashtable();          //각 Group별 SVID값이 저장

        //Constructor
        public clsGroup(int intGroupID)
        {
            this.GroupID = intGroupID;
        }

        //SVID 개체등록 및 가져오기(SVID에 대한 기준정보를 가지고 있음)
        #region "SVID"

        public bool AddSVID(int intIndex)
        {
            clsSVID dclsSVID;     //ECID 개체 선언

            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableSVID.Contains(intIndex))
                {
                    return true;
                }

                dclsSVID = new clsSVID(intIndex);
                pHashtableSVID.Add(intIndex, dclsSVID);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsSVID SVID(int intIndex)
        {
            try
            {
                if (pHashtableSVID.Contains(intIndex))
                {
                    return (clsSVID)pHashtableSVID[intIndex];
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

        public ICollection SVID()
        {
            try
            {
                return pHashtableSVID.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveSVID(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableSVID.Contains(intIndex))
                {
                    pHashtableSVID.Remove(intIndex);     //HashTable에서 해당 LOT을 제거한다.
                    return true;
                }
                else
                {
                    return false;
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

        public bool RemoveSVID()
        {
            try
            {
                pHashtableSVID.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int SVIDCount
        {
            get { return pHashtableSVID.Count; }
        }


        #endregion
    }
}
