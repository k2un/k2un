using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using InfoAct;

/// <summary>
/// Manage all CIM Command class that use in this...
/// </summary>

namespace EQPAct
{
    public class clsCIMEventManager
    {
        #region Fields
        private List<clsCIMEvent> listCIMEvent = new List<clsCIMEvent>();
        private clsEQPAct pEqpAct;

        private object objLockCIMEvent = new object();
        #endregion

        #region Properties
        public List<clsCIMEvent> ListCIMEvent
        {
            get { return listCIMEvent; }
        }
        #endregion

        #region Singleton
        #endregion

        #region Constructors
        public clsCIMEventManager()
        {
            
        }
        #endregion

        #region Methods
        public void funCreateCIMEvent(string strUnitID)
        {
            try
            {
                //listCIMEvent.Add(new clsCimEventSample(strUnitID));
                listCIMEvent.Add(new clsCimEventBuzzerOff(strUnitID));
                listCIMEvent.Add(new clsCimEventBuzzerOn(strUnitID));
                listCIMEvent.Add(new clsCimEventControlState(strUnitID));
                //listCIMEvent.Add(new clsCimEventCurrentPPIDChange(strUnitID));
                //listCIMEvent.Add(new clsCimEventCurrentRunningPPIDReadFromHost(strUnitID));
                listCIMEvent.Add(new clsCimEventDateAndTimeSetting(strUnitID));
                listCIMEvent.Add(new clsCimEventEqpProcessState(strUnitID));
                listCIMEvent.Add(new clsCimEventEqpState(strUnitID));
                listCIMEvent.Add(new clsCimEventEventBitInitial(strUnitID));
                listCIMEvent.Add(new clsCimEventHostDisconnect(strUnitID));
                //listCIMEvent.Add(new clsCimEventHostPPIDMappingChange(strUnitID));
                listCIMEvent.Add(new clsCimEventMapInitial(strUnitID));
                listCIMEvent.Add(new clsCimEventMessageSend(strUnitID));
                listCIMEvent.Add(new clsCimEventOnePPIDRead(strUnitID));
                listCIMEvent.Add(new clsCimEventPPIDBodyModify(strUnitID));
                listCIMEvent.Add(new clsCimEventPPIDCreate(strUnitID));
                listCIMEvent.Add(new clsCimEventPPIDDelete(strUnitID));
                listCIMEvent.Add(new clsCimEventSEMAlarm(strUnitID));
                listCIMEvent.Add(new clsCimEventSEMEnd(strUnitID));
                listCIMEvent.Add(new clsCimEventSEMStart(strUnitID));
                listCIMEvent.Add(new clsCimEventSetupPPID(strUnitID));

                listCIMEvent.Add(new clsCimEvent_ProcessDataSet(strUnitID));
                listCIMEvent.Add(new clsCimEvent_ProcessDataDel(strUnitID));
                listCIMEvent.Add(new clsCimEvent_ProcessDataLog(strUnitID));
                listCIMEvent.Add(new clsCimEventSimulation(strUnitID));
                listCIMEvent.Add(new clsCimEventSerialPortOpen(strUnitID));
                listCIMEvent.Add(new clsCimEventSerialPortClose(strUnitID));
                listCIMEvent.Add(new clsCimEventProcessStartReqNormal(strUnitID));
                listCIMEvent.Add(new clsCimEventProcessStartReqAPC(strUnitID));
                listCIMEvent.Add(new clsCimEventProcessStartReqRPC(strUnitID));
                listCIMEvent.Add(new clsCimEventProcessStartReqPPC(strUnitID));
                listCIMEvent.Add(new clsCimEventMultiUseDataSet(strUnitID));
                listCIMEvent.Add(new clsCimEventMultiUseDataGet(strUnitID));
                listCIMEvent.Add(new clsCimEventECIDChange(strUnitID));
                listCIMEvent.Add(new clsCimEventECIDRead(strUnitID));
                //listCIMEvent.Add(new clsCimEventMsg2MCC(strUnitID));
                listCIMEvent.Add(new clsCimEventMessageClear(strUnitID));
                listCIMEvent.Add(new clsCimEventFilmCommand(strUnitID));

                listCIMEvent.Add(new clsCimEventMCCDataSend(strUnitID));
                listCIMEvent.Add(new clsCimEventLotinformationSend(strUnitID));
                listCIMEvent.Add(new clsCimEventUDPPortOpen(strUnitID)); //20141106 이원규 (SEM_UDP)
                listCIMEvent.Add(new clsCimEventUDPPortClose(strUnitID)); //20141106 이원규 (SEM_UDP)
                listCIMEvent.Add(new clsCimEventFI01Check(strUnitID)); //150122 고석현

                listCIMEvent.Add(new clsCimEventRecovery(strUnitID));//[2015/03/10]Recovery(Add by HS)
                                
            }
            catch (Exception error)
            {
                throw (error);
            }
        }

        public void funSetEqpAct(clsEQPAct eqp)
        {
            try
            {
                pEqpAct = eqp;

                foreach (clsCIMEvent cmd in listCIMEvent)
                {
                    cmd.funSetEqpAct(eqp);
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

        //public void funArrangeCIMEvent(string[] parameters)
        public void funArrangeCIMEvent(clsParam parameters)
        {
            lock (objLockCIMEvent)
            {
                try
                {
                    foreach (clsCIMEvent evt in listCIMEvent)
                    {
                        //if (evt.StrEventName == parameters[0])
                        //{
                        //    ((ICIMEvent)evt).funProcessCIMEvent(parameters);
                        //    return;
                        //}

                        if (parameters.Command.ToString().Trim() != "MCCDataSend")
                        {
                            int a = 0;
                        }
                        if (evt.StrEventName == parameters.Command.ToString().Trim())
                        {
                            ((ICIMEvent)evt).funProcessCIMEvent(parameters.pobjParameter);
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
