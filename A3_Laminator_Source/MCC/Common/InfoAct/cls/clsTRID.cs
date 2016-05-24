using System;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsTRID
    {
        public int TRID = 0;                //TRID(Trace Request ID, Key 값)
        public int REPGSZ =0;               //Reporting Groupt Size
        public string DSPER = "";           //Data Sample Period(Interval)
        public int TOTSMP = 0;              //Total Smaple
        public double TimeAcc = 0;             //횟수 누적
        public int GrpCnt = 0;              //Group Count(한번 보고시에 Group이 몇개인가)
        public int REPGSZCnt = 0;
        public int SampleNo = 0;           //누적Count

        private Hashtable pHashtableGroup = new Hashtable();       //TRID별 Group

        //Constructor
        public clsTRID(int intTRID)
        {
            this.TRID = intTRID;
        }

        //Group 개체등록 및 가져오기
        #region "Group"

        public bool AddGroup(int intGroupID)
        {
            clsGroup dclsGroup;     //ECID 개체 선언

            try
            {
                if (intGroupID <= 0)
                {
                    return false;
                }

                if (pHashtableGroup.Contains(intGroupID))
                {
                    return true;
                }

                dclsGroup = new clsGroup(intGroupID);
                pHashtableGroup.Add(intGroupID, dclsGroup);

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

        public clsGroup Group(int intGroupID)
        {
            try
            {
                if (pHashtableGroup.Contains(intGroupID))
                {
                    return (clsGroup)pHashtableGroup[intGroupID];
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

        public ICollection Group()
        {
            try
            {
                return pHashtableGroup.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveGroup(int intGroupID)
        {
            try
            {
                if (intGroupID <= 0)
                {
                    return false;
                }

                if (pHashtableGroup.Contains(intGroupID))
                {
                    pHashtableGroup.Remove(intGroupID);     //HashTable에서 해당 LOT을 제거한다.
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

        public bool RemoveGroup()
        {
            try
            {
                pHashtableGroup.Clear();
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

        public int GroupCount
        {
            get { return pHashtableGroup.Count; }
        }


        #endregion
    }
}
