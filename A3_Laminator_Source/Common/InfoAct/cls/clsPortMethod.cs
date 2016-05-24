using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsPortMethod
    {
        private Hashtable pHashtableSlot = new Hashtable();

        //Slot 개체등록 및 가져오기
        #region "Slot"

        public bool AddSlot(int intSlotID)
        {
            clsSlot dclsSlotInfo;     //SlotInfo 개체 선언

            try 
            {
                int dintSlotID = intSlotID;

                if (dintSlotID <= 0) {
                    return false;
                } else {

                    if (pHashtableSlot.Contains(dintSlotID)) {
                        return true;
                    } else {
                        dclsSlotInfo = new clsSlot(dintSlotID);
                        pHashtableSlot.Add(dintSlotID, dclsSlotInfo);
                        return true;
                    }
                }

            } catch {
                return false;

            } finally {}
        }

        public clsSlot Slot(int intSlotID)
        {
            try 
            {
                int dintSlotID = intSlotID;

                if (pHashtableSlot.Contains(dintSlotID)) {
                    return (clsSlot)pHashtableSlot[dintSlotID];
                } else {
                    return null;
                }

            } catch {
                return null;

            } finally {}
        }

        public ICollection Slot()
        {
            try 
            {
                return pHashtableSlot.Keys;

            } catch {
                return null;

            } finally {}
        }

        public int SlotCount
        {
            get { return pHashtableSlot.Count; }
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

        #endregion
    }
}
