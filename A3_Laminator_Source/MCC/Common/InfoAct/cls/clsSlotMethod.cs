using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsSlotMethod
    {
        private Hashtable pHashtableGLSAPD = new Hashtable();

        //GLSAPD 개체등록 및 가져오기(실제 APD 값이 저장됨)
        #region "GLSAPD"

        public bool AddGLSAPD(int intIndex)
        {
            clsGLSAPD dclsGLSAPD;     //GLSAPD 개체 선언

            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableGLSAPD.Contains(intIndex))
                {
                    return true;
                }

                dclsGLSAPD = new clsGLSAPD(intIndex);
                pHashtableGLSAPD.Add(intIndex, dclsGLSAPD);

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

        public clsGLSAPD GLSAPD(int intIndex)
        {
            try
            {
                if (pHashtableGLSAPD.Contains(intIndex))
                {
                    return (clsGLSAPD)pHashtableGLSAPD[intIndex];
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

        public ICollection GLSAPD()
        {
            try
            {
                return pHashtableGLSAPD.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveGLSAPD(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableGLSAPD.Contains(intIndex))
                {
                    pHashtableGLSAPD.Remove(intIndex);     //HashTable에서 해당 GLS APD을 제거한다.
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

        public bool RemoveGLSAPD()
        {
            try
            {
                pHashtableGLSAPD.Clear();
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

        public int GLSAPDCount
        {
            get { return pHashtableGLSAPD.Count; }
        }


        #endregion
    }
}
