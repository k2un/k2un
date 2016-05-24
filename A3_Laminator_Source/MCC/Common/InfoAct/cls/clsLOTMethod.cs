using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsLOTMethod
    {
        private Hashtable pHashtableSlot = new Hashtable();
        private Hashtable pHashtableLOTAPD = new Hashtable();

        //Slot 개체등록 및 가져오기
        #region "Slot"

        public bool AddSlot(int intSlotID)
        {
            clsSlot dclsSlot;     //Slot 개체 선언

            try
            {
                int dintSlotID = intSlotID;

                if (dintSlotID <= 0)
                {
                    return false;
                }
                else
                {
                    if (pHashtableSlot.Contains(dintSlotID))
                    {
                        return true;
                    }
                    else
                    {
                        dclsSlot = new clsSlot(dintSlotID);
                        pHashtableSlot.Add(dintSlotID, dclsSlot);

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

        public clsSlot Slot(int intSlotID)
        {
            try
            {
                int dintSlotID = intSlotID;

                if (pHashtableSlot.Contains(dintSlotID))
                {
                    return (clsSlot)pHashtableSlot[dintSlotID];
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

        public ICollection Slot()
        {
            try
            {
                return pHashtableSlot.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        //public bool RemoveSlot()
        //{
        //    try
        //    {
        //        pHashtableSlot.Clear();        //HashTable에서 모든 SlotInfo를 제거한다.
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //    }
        //}

        public int SlotCount
        {
            get { return pHashtableSlot.Count; }
        }

        #endregion

        //LOTAPD 개체등록 및 가져오기
        #region "LOTAPD"

        public bool AddLOTAPD(int intIndex)
        {
            clsLOTAPD dclsLOTAPD;     //LOTAPD 개체 선언

            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableLOTAPD.Contains(intIndex))
                {
                    return false;                                   //LOT이 중복되었을 경우 조치 강구(같은 LOT이 존재하면 안됨)
                }

                dclsLOTAPD = new clsLOTAPD(intIndex);
                pHashtableLOTAPD.Add(intIndex, dclsLOTAPD);

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

        public clsLOTAPD LOTAPD(int intIndex)
        {
            try
            {
                if (pHashtableLOTAPD.Contains(intIndex))
                {
                    return (clsLOTAPD)pHashtableLOTAPD[intIndex];
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

        public ICollection LOTAPD()
        {
            try
            {
                return pHashtableLOTAPD.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveLOTAPD(int intIndex)
        {
            try
            {
                if (intIndex <= 0)
                {
                    return false;
                }

                if (pHashtableLOTAPD.Contains(intIndex))
                {
                    pHashtableLOTAPD.Remove(intIndex);     //HashTable에서 해당 LOT을 제거한다.
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

        public bool RemoveLOTAPD()
        {
            try
            {
                pHashtableLOTAPD.Clear();
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

        public int LOTAPDCount
        {
            get { return pHashtableLOTAPD.Count; }
        }


        #endregion
    }
}
