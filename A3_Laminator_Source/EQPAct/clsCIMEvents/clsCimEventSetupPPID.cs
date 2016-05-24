using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.IO;

namespace EQPAct
{
    public class clsCimEventSetupPPID : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventSetupPPID(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "SetUpPPID";
        }
        #endregion

        #region Methods
        /// <summary>
        /// CIM에서 설비로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : cmdName
        /// parameters[1] : 1st parameter
        /// parameters[2] : 2nd parameter
        /// parameters[3] : 3rd parameter
        /// parameters[4] : 4th parameter
        /// parameters[5] : 5th Parameter
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            string[] dstrValue = null;
            int dintPPIDType = 0;
            string dstrWordAddress = "";
            string dstrTemp = "";
            string strData = "";
            string dstrEQPPPID = "";
            string dstrHostPPID = "";

            try
            {
                this.pInfo.All.SetUpPPIDPLCWriteCount = 0;  //초기화

                dintPPIDType = Convert.ToInt32(parameters[0].ToString());

                if (dintPPIDType == 1) //EQPPPID
                {
                    //pInfo.Unit(0).SubUnit(0).RemoveEQPPPID();
                    dstrWordAddress = "W2500";
                    dstrTemp = pEqpAct.funWordRead(dstrWordAddress, 22, EnuEQP.PLCRWType.Binary_Data);
                    for (int dintLoop = 0; dintLoop < 22; dintLoop++)
                    {
                        strData = dstrTemp.Substring(dintLoop * 16, 16);
                        for (int dintLoop2 = 0; dintLoop2 < 16; dintLoop2++)
                        {
                            if (strData[dintLoop2].ToString() == "1")
                            {
                                dstrEQPPPID = ((16 - dintLoop2) + (dintLoop * 16)).ToString();
                                pInfo.Unit(0).SubUnit(0).AddEQPPPID(dstrEQPPPID);
                            }
                            else
                            {
                                dstrEQPPPID = ((16 - dintLoop2) + (dintLoop * 16)).ToString();
                                pInfo.Unit(0).SubUnit(0).RemoveEQPPPID(dstrEQPPPID);
                            }
                        }
                    }

                    if (pInfo.All.isReceivedFromHOST == true)
                    {
                        pInfo.All.isReceivedFromHOST = false;  //초기화
                        pInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                    }

                    if (pInfo.All.isReceivedFromCIM)
                    {
                        pInfo.All.isReceivedFromCIM = false;
                        pInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                    }
                }
                else ///HostPPID
                {
                    this.pInfo.All.HOSTPPIDCommandCount = 0; //초기화
                    pInfo.Unit(0).SubUnit(0).RemoveHOSTPPID();
                    this.pInfo.All.CurrentRegisteredHOSTPPIDCount = Convert.ToInt32(pEqpAct.funWordRead("W200E", 1, EnuEQP.PLCRWType.Int_Data));

                    if (this.pInfo.All.CurrentRegisteredHOSTPPIDCount > 0)
                    {
                        pEqpAct.subSetupPPIDCmd(true, 2);   //먼저 EQP PPID 정보 요청 지시
                    }
                }

                //PPID가 하나도 등록이 안된 경우
                if (this.pInfo.All.CurrentRegisteredHOSTPPIDCount == 0 && pInfo.Unit(0).SubUnit(0).EQPPPIDCount == 0)
                {
                    if (this.pInfo.All.isReceivedFromHOST == true)
                    {
                        this.pInfo.All.isReceivedFromHOST = false;      //초기화
                        this.pInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                    }
                    else if (this.pInfo.All.isReceivedFromCIM == true)
                    {
                        this.pInfo.All.isReceivedFromCIM = false;       //초기화
                        this.pInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                    }

                    return;
                }

                
                
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                //if (pInfo.All.isReceivedFromHOST == true)
                //{
                //    pInfo.All.isReceivedFromHOST = false;  //초기화
                //    pInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                //}
            }
        }
        #endregion
    }
}
