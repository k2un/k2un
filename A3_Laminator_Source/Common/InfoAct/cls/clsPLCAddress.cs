using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBAct;
using System.Data;
using CommonAct;
using System.Windows.Forms;

namespace InfoAct
{
    public class clsPLCAddress
    {
        #region Fields & Properties
        #region General Define
        private string strWordDevice = "ZR";
        //private int intCIM_WordStartAddress = 70000;
        //private int intEQP_WordStartAddress = 40000;

        private string strBitDevice = "M";
        //private int intCIM_BitStartAddress = 12000;
        //private int intEQP_BitStartAddress = 10000;
        #endregion

     
        #endregion
        
        #region Singleton
        public static readonly clsPLCAddress Instance = new clsPLCAddress();
        #endregion

        #region Constructors
        public clsPLCAddress()
        {
            //생성될때 DB에서 각 Address의 Length를 가져온다.
            try
            {
                //int dintIndex = 0;
                //string dSystemINI = Application.StartupPath + @"\system\System.ini";
                //strBitDevice = FunINIMethod.funINIReadValue("ADDRESS","BitDevice","B",dSystemINI);
                //strWordDevice = FunINIMethod.funINIReadValue("ADDRESS", "WordDevice", "W", dSystemINI);

                ////DB로부터 Address의 Length를 읽어들여 저장한다.
                //string dstrSQL = "SELECT * FROM tbAddress order by Index";
                //DataTable dDT = clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                //if (dDT != null)
                //{
                //    foreach (DataRow dr in dDT.Rows)
                //    {
                //        dintIndex = Convert.ToInt32(dr["Index"]);

                //        if (dr["ControlType"].ToString().ToUpper() == "C")
                //        {
                //            if (dr["AddressType"].ToString().ToUpper() == "WORD")   //Word Address Length 저장
                //            {
                //                int dintAddress = int.Parse(dr["Address"].ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
                //                switch (dr["Description"].ToString())
                //                {
                //                    case "TimeSet": intCIM_DateTimeSet = dintAddress; break;
                //                    case "PPIDListGroupNo": intCIM_PPIDListGroupNo = dintAddress; break;
                //                    case "PPIDCMD": intCIM_PPIDCMD = dintAddress; break;
                //                    case "MessageSet": intCIM_MessageSet = dintAddress; break;
                //                    case "RPCDataSet": intCIM_RPCDataSet = dintAddress; break;
                //                    case "APCDataSet": intCIM_APCDataSet = dintAddress; break;
                //                    case "ECIDChangeCMD": intCIM_ECIDChangeCMD = dintAddress; break;
                //                }
                //            }
                //            else  //Bit Address Length 저장
                //            {
                //                int dintAddress = int.Parse(dr["Address"].ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
                //                switch (dr["Description"].ToString())
                //                {
                //                    case "BuzzerOn": intBit_BuzzerOn = dintAddress; break;
                //                    case "BuzzerOff": intBit_BuzzerOff = dintAddress; break;
                //                    case "DateTimeSet": intBit_DateTimeSet = dintAddress; break;
                //                    case "Normal": intBit_EQPNormal = dintAddress; break;
                //                    case "PM": intBit_EQPPM = dintAddress; break;
                //                    case "PAUSE": intBit_EQPPause = dintAddress; break;
                //                    case "RESUME": intBit_EQPResume = dintAddress; break;
                //                    case "OnePPIDReq": intBit_OnePPIDInfoReq = dintAddress; break;
                //                    case "APCStart": intBit_APCStart = dintAddress; break;
                //                    case "RPCStart": intBit_RPCStart = dintAddress; break;
                //                    case "NormalStart": intBit_NormalStart = dintAddress; break;
                //                    case "HostDisconnect": intBit_HostDisconnect = dintAddress; break;
                //                    case "ECIDChangeCMD": intBit_ECIDChangeCMD = dintAddress; break;
                //                    case "ECIDList": intBit_ECIDList = dintAddress; break;
                //                    case "PPIDList": intBit_PPIDList = dintAddress; break;
                //                    case "MessageSet": intBit_HostMessage = dintAddress; break;
                //                    case "PPIDBodyChangeCMD": intBit_PPIDBodyChangeCMD = dintAddress; break;
                //                    case "PPIDMappingChangeCMD": intBit_PPIDMappingChangeCMD = dintAddress; break;
                //                    case "Offline": intBit_Offline = dintAddress; break;
                //                    case "OnlineRemote": intBit_OnlineRemote = dintAddress; break;
                //                    case "UnitPAUSE": intBit_UnitPause = dintAddress; break;
                //                    case "UnitRESUME": intBit_UnitResume = dintAddress; break;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (dr["AddressType"].ToString().ToUpper() == "WORD")   //Word Address Length 저장
                //            {
                //                int dintAddress = int.Parse(dr["Address"].ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
                //                switch (dr["Description"].ToString())
                //                {
                //                    case "CurrentHostPPID": intEQP_CurrentHostPPID = dintAddress; break;
                //                    case "CurrentEQPPPID": intEQP_CurrentEQPPPID = dintAddress; break;
                //                    case "GlassData": intEQP_GlassData = dintAddress; break;
                //                    case "PPIDCMD": intEQP_PPIDReport = dintAddress; break;
                //                    case "Scrap": intEQP_ScrapGlassID = dintAddress; break;
                //                    case "UnScrap": intEQP_UnScrapGlassID = dintAddress; break;
                //                    case "SVID": intEQP_SVIDSet = dintAddress; break;
                //                    case "PPIDList": intEQP_PPIDList = dintAddress; break;
                //                    case "HostPPIDCount": intEQP_HostPPIDCount = dintAddress; break;
                //                    case "EQPPPIDCount": intEQP_EQPPPIDCount = dintAddress; break;
                //                    case "GLSAPD": intEQP_GLSAPD = dintAddress; break;
                //                    case "MCCData": intEQP_MCCData = dintAddress; break;
                //                    case "ECIDReport": intEQP_ECIDReport = dintAddress; break;
                //                    case "Arrive": intEQP_Arrive = dintAddress; break;
                //                    case "Departure": intEQP_Departure = dintAddress; break;

                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception)
            {

            }

            ////Address Setting
            if (strWordDevice == "W")
            {
                strWordDevice = strWordDevice.PadRight(5, '0');
            }
            else
            {

            }

            if ((strBitDevice == "B") || (strBitDevice == "X") || (strBitDevice == "Y"))
            {
                strBitDevice = strBitDevice.PadRight(5, '0');
            }
            else
            {

            }
        }
        #endregion
    }
}
