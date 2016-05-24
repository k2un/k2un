using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsEQPPPIDMethod
    {
        private Hashtable pPPIDBodyHash = new Hashtable();


        //PPIDBody 개체등록 및 가져오기(PPIDBody에 대한 실질적인 값 저장)
        #region "PPIDBody"

        public bool AddPPIDBody(int intIndex)
        {
            clsPPIDBody dclsPPIDBody;

            try
            {
                if (intIndex < 0)
                {
                    return false;
                }

                if (pPPIDBodyHash.Contains(intIndex))
                {
                    return true;
                }

                dclsPPIDBody = new clsPPIDBody(intIndex);
                pPPIDBodyHash.Add(intIndex, dclsPPIDBody);

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

        public clsPPIDBody PPIDBody(int intIndex)
        {
            try
            {
                if (pPPIDBodyHash.Contains(intIndex))
                {
                    return (clsPPIDBody)pPPIDBodyHash[intIndex];
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

        public ICollection PPIDBody()
        {
            try
            {
                return pPPIDBodyHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemovePPIDBody(int intIndex)
        {
            try
            {
                if (intIndex < 0)
                {
                    return false;
                }

                if (pPPIDBodyHash.Contains(intIndex))
                {
                    pPPIDBodyHash.Remove(intIndex);
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

        public bool RemovePPIDBody()
        {
            try
            {
                pPPIDBodyHash.Clear();        //HashTable에서 모든 Alarm을 제거한다.
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

        public int PPIDBodyCount
        {
            get { return pPPIDBodyHash.Count; }
        }

        #endregion

    }
}
