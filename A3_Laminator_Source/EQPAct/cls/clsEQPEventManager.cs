using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

/// <summary>
/// Manage all PLC Event class that use in this...
/// </summary>

namespace EQPAct
{
    public class clsEQPEventManager
    {
        #region Fields
        private List<clsEQPEvent> listEQPEvent = new List<clsEQPEvent>();
        private clsEQPAct pEqpAct;

        private object objLockEQPEvent = new object();
        #endregion

        #region Properties
        public List<clsEQPEvent> ListEQPEvent
        {
            get { return listEQPEvent; }
        }
        #endregion

        #region Singleton
        #endregion

        #region Constructors
        public clsEQPEventManager()
        {

        }
        #endregion

        #region Methods
        public void funCreateEQPEvent(string strUnitID)
        {
            try
            {
                //listEQPEvent.Add(new clsEQPEventSample(strUnitID));

                listEQPEvent.Add(new clsEQPEventAlarm(strUnitID));

                //listEQPEvent.Add(new clsEQPEventAlarm(strUnitID));
                listEQPEvent.Add(new clsEQPEventAlarmExist(strUnitID));
                listEQPEvent.Add(new clsEQPEventArrive(strUnitID));
                listEQPEvent.Add(new clsEQPEventCurrentPPIDChange(strUnitID));
                listEQPEvent.Add(new clsEQPEventDeparture(strUnitID));
                //listEQPEvent.Add(new clsEQPEventEOIDChange(strUnitID));
                listEQPEvent.Add(new clsEQPEventEqpMode(strUnitID));
                listEQPEvent.Add(new clsEQPEventEqpState(strUnitID));
                listEQPEvent.Add(new clsEQPEventGlassExist(strUnitID));
                listEQPEvent.Add(new clsEQPEventHostPPIDMappingChange(strUnitID));
                listEQPEvent.Add(new clsEQPEventOnePPIDInfoRequest(strUnitID));
                listEQPEvent.Add(new clsEQPEventOPCallMessageClear(strUnitID));
                listEQPEvent.Add(new clsEQPEventEqpPPIDBodyChange(strUnitID));
                listEQPEvent.Add(new clsEQPEventPPIDCreate(strUnitID));
                listEQPEvent.Add(new clsEQPEventPPIDDelete(strUnitID));
                listEQPEvent.Add(new clsEQPEventEqpProcessState(strUnitID));
                listEQPEvent.Add(new clsEQPEventScrap(strUnitID));
                listEQPEvent.Add(new clsEQPEventUnscrap(strUnitID));
                listEQPEvent.Add(new clsEQPEventECIDChange(strUnitID));
                listEQPEvent.Add(new clsEQPEventSetupPPID(strUnitID));
                listEQPEvent.Add(new clsEQPEventOnePPIDInfoRequest(strUnitID));
                listEQPEvent.Add(new clsEQPEventFilmReport(strUnitID));
                listEQPEvent.Add(new clsEQPEventProcessDataCheck(strUnitID));
                listEQPEvent.Add(new clsEQPEventJudge(strUnitID));
                listEQPEvent.Add(new clsEQPEventPPIDDeleteMapping(strUnitID));
                listEQPEvent.Add(new clsEQPEventCaseIn(strUnitID));
                listEQPEvent.Add(new clsEQPEventCaseOut(strUnitID));
                listEQPEvent.Add(new clsEQPEventFILMCaseExist(strUnitID));
                listEQPEvent.Add(new clsEQPEventCIMTest(strUnitID));
				
                //[2015/02/03]Mapping EQP PPID(Add by HS)
                listEQPEvent.Add(new clsEQPEventPPIDMapping(strUnitID));
                listEQPEvent.Add(new clsEQPEventHostPPIDMappingEQPPPIDREQ(strUnitID));
                listEQPEvent.Add(new clsEQPEventControlStateCHG(strUnitID));

                //listEQPEvent.Add(new clsEQPEvent_TEST(strUnitID));
            }
            catch (Exception error)
            {
                throw (error);
            }
        }

        public void funSetEqpAct(EQPAct.clsEQPAct eqpAct)
        {
            try
            {
                pEqpAct = eqpAct;

                foreach (clsEQPEvent act in listEQPEvent)
                {
                    act.funSetEqpAct(eqpAct);
                }
            }
            catch (Exception error)
            {
                if (pEqpAct != null)
                {
                    pEqpAct.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                }
            }
        }

        public void funArrangeEQPEvent(string strEventName, string[] parameters)
        {
            lock (objLockEQPEvent)
            {
                try
                {
                    foreach (clsEQPEvent evt in listEQPEvent)
                    {
                        if (evt.StrEventName == strEventName)
                        {
                            ((IEQPEvent)evt).funProcessEQPEvent(parameters);
                            return;
                        }
                    }
                }
                catch (Exception error)
                {
                    if (pEqpAct != null)
                    {
                        pEqpAct.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                    }
                }
            }
        }
        #endregion
    }
}
