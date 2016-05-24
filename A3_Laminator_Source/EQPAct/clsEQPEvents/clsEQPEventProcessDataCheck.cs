using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Threading;

namespace EQPAct
{
    public class clsEQPEventProcessDataCheck : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventProcessDataCheck(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actProcessDataCheck";
        }
        #endregion

        #region Methods
        /// <summary>
        /// 설비에서 CIM으로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : strCompBit
        /// parameters[1] : dstrACTVal
        /// parameters[2] : dintActFrom
        /// parameters[3] : dstrACTFromSub
        /// parameters[4] : intBitVal
        /// parameters[5] : Special Parameter
        /// </remarks>
        public void funProcessEQPEvent(string[] parameters)
        {
            string strCompBit = parameters[0];
            //int intUnitID = Convert.ToInt32(parameters[2]);
            StringBuilder dstrLog = new StringBuilder();
            string[] dstrValue = null;
            string dstrHGLSID = "";
            string dstrLOTID = "";
            int dintSlotID = 0;
            string dstrGlassSize1 = "";
            string dstrGlassSize2 = "";
            InfoAct.clsSlot dslot = new InfoAct.clsSlot(0);
            string[] dstrDataValue = new string[4];     //MCC Log Data

            int dintIndex = 0;

            int dintPortID = 0;     //LD/UD 작업 Port

            int dintJobStart = 0;
            int dintJobEnd = 0;

            bool dbolXPCStart = false;

            bool dbolProcChanged = false;

            try
            {
                try
                {
                    m_pEqpAct.subWordReadSave("W2040", 8, EnuEQP.PLCRWType.ASCII_Data);               //H-Glass(=panel)-ID
                    m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //E-Glass(=panel)-ID
                    m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Lot-ID
                    m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Batch-ID
                    m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Job-ID
                    m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Port-ID
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Slot-NO
                    m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Prod-Type
                    m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Prod-Kind
                    m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //ProductID
                    m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Runspec-ID
                    m_pEqpAct.subWordReadSave("", 4, EnuEQP.PLCRWType.ASCII_Data);                            //Layer-ID
                    m_pEqpAct.subWordReadSave("", 4, EnuEQP.PLCRWType.ASCII_Data);                            //Step-ID
                    m_pEqpAct.subWordReadSave("", 10, EnuEQP.PLCRWType.ASCII_Data);                           //HOST PPID
                    m_pEqpAct.subWordReadSave("", 10, EnuEQP.PLCRWType.ASCII_Data);                            //Flow-ID

                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Glass(=panel)-Size(하위)         ---->U2 2임.(하위 + Space(1) + 상위)
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Glass(=panel)-Size(상위)         ---->U2 2임.(하위 + Space(1) + 상위)
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Glass-thickness(thickness)
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Glass-State(빈값)

                    m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Glass-Order
                    m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Comment

                    m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Use-Count
                    m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Judgement
                    m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Reason-Code
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Inspection-Flag
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Enc-Flag
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Prerun-Flag
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Turn-Dir
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Flip-State
                    m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Work-State
                    m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Multi-Use
                    m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Pair Glass-ID
                    m_pEqpAct.subWordReadSave("", 10, EnuEQP.PLCRWType.ASCII_Data);                           //Pair PPID

                    m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                           //Option Name1
                    m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                              //Option Value1
                    m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                           //Option Name2
                    m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                              //Option Value2
                    m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                           //Option Name3
                    m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                              //Option Value3
                    m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                           //Option Name4
                    m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                              //Option Value4
                    m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                           //Option Name5
                    m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                              //Option Value5

                    m_pEqpAct.subWordReadSave("", 168, EnuEQP.PLCRWType.Hex_Data);                              //Spare

                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Job Start
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Job End

                    //장비에서 GLS Data정보를 한꺼번에 읽는다.
                    dstrValue = m_pEqpAct.funWordReadAction(true);


                    //읽은 GLS Data를 Parsing해서 변수에 저장
                    dslot.H_PANELID = dstrValue[0].Trim();
                    dslot.E_PANELID = dstrValue[1].Trim();
                    dslot.LOTID = dstrValue[2].Trim();
                    dslot.BATCHID = dstrValue[3].Trim();
                    dslot.JOBID = dstrValue[4].Trim();
                    dslot.PORTID = dstrValue[5].Trim();
                    dintPortID = Convert.ToInt32(FunStringH.funMakeLengthStringFirst(dslot.PORTID, 4).Substring(2, 2));
                    dslot.SlotID = Convert.ToInt32(dstrValue[6]);
                    dslot.SLOTNO = dstrValue[6];
                    dslot.PRODUCT_TYPE = dstrValue[7].Trim();
                    dslot.PRODUCT_KIND = dstrValue[8].Trim();
                    dslot.PRODUCTID = dstrValue[9].Trim();
                    dslot.RUNSPECID = dstrValue[10].Trim();
                    dslot.LAYERID = dstrValue[11].Trim();
                    dslot.STEPID = dstrValue[12].Trim();
                    dslot.HOSTPPID = dstrValue[13].Trim();
                    dslot.FLOWID = dstrValue[14].Trim();
                    dstrGlassSize1 = dstrValue[15].Trim();
                    dstrGlassSize2 = dstrValue[16].Trim();
                    dslot.SIZE = dstrGlassSize1 + " " + dstrGlassSize2;
                    dslot.THICKNESS = Convert.ToInt32(dstrValue[17]);
                    //dstrValue[18]은 Reserved임. Glass State
                    dslot.GLASS_ORDER = dstrValue[19].Trim();
                    dslot.COMMENT = dstrValue[20].Trim();
                    dslot.USE_COUNT = dstrValue[21].Trim();
                    dslot.JUDGEMENT = dstrValue[22].Trim();
                    dslot.REASON_CODE = dstrValue[23].Trim();
                    dslot.INS_FLAG = dstrValue[24].Trim();
                    dslot.ENC_FLAG = dstrValue[25].Trim();
                    dslot.PRERUN_FLAG = dstrValue[26].Trim();
                    dslot.TURN_DIR = dstrValue[27].Trim();
                    dslot.FLIP_STATE = dstrValue[28].Trim();
                    dslot.WORK_STATE = dstrValue[29].Trim();
                    dslot.MULTI_USE = dstrValue[30].Trim();

                    dslot.PAIR_GLASSID = dstrValue[31].Trim();
                    dslot.PAIR_PPID = dstrValue[32].Trim();

                    for (int dintLoop = 0; dintLoop <= dslot.OPTION_NAME.Length - 1; dintLoop++)
                    {
                        dslot.OPTION_NAME[dintLoop] = dstrValue[dintIndex + 33].Trim();
                        dslot.OPTION_VALUE[dintLoop] = dstrValue[dintIndex + 34].Trim();
                        //dslot.OPTION_NAME[dintLoop] = "0";
                        //dslot.OPTION_VALUE[dintLoop] = "0";
                        dintIndex = dintIndex + 2;
                    }

                    dintIndex = 0;

                    dintJobStart = Convert.ToInt32(dstrValue[44]);
                    dintJobEnd = Convert.ToInt32(dstrValue[45]);

                    if (dintJobStart == 1) dslot.JOBStart = true;
                    else dslot.JOBStart = false;

                    if (dintJobEnd == 1) dslot.JOBEnd = true;
                    else dslot.JOBEnd = false;

                    dstrLOTID = dslot.LOTID;
                    dintSlotID = dslot.SlotID;
                    dstrHGLSID = dslot.H_PANELID;
                    pInfo.All.DataCheckGLSID = dstrHGLSID;
                    pInfo.All.HostPPID = dstrValue[13].Trim();
                }
                catch (Exception ex)
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                }
                finally
                {
                    //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                    m_pEqpAct.subSetConfirmBit(strCompBit);
                }

                
                
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", dstrLOTID:" + dstrLOTID + ", dintSlotID: " + dintSlotID.ToString());
            }
        }

       

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
