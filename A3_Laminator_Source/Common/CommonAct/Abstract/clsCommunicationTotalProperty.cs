using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace CommonAct
{
    public class clsCommunicationTotalProperty : IStandardProperty, IPLCproperty, INet10property, ITCPproperty, I232property
    {

        #region IStandardProperty 멤버
        private static EnuCommunication.EQPCommandType penuEQPCommandType;
        public EnuCommunication.EQPCommandType proEQPCommandType
        {
            get { return penuEQPCommandType; }
            set { penuEQPCommandType = value; }
        }

        private static EnuCommunication.EQPNameType penuEQPName;
        public EnuCommunication.EQPNameType proEQPName
        {
            get { return penuEQPName; }
            set { penuEQPName = value; }
        }

        private static EnuCommunication.CommunicationType penuCommunicationType;
        public EnuCommunication.CommunicationType proCommunicationType
        {
            get { return penuCommunicationType; }
            set { penuCommunicationType = value; }
        }

        private static bool pbolDummy;
        public bool proDummy
        {
            get { return pbolDummy; }
            set { pbolDummy = value; }
        }

        #endregion

        #region IPLCproperty 멤버

        private static string pstrWord1Start;
        public string proWord1Start
        {
            get { return pstrWord1Start; }
            set { pstrWord1Start = value; }
        }

        private static string pstrWord1End;
        public string proWord1End
        {
            get { return pstrWord1End; }
            set { pstrWord1End = value; }
        }

        private static int pintScanAreaCount;
        public int proScanAreaCount
        {
            get { return pintScanAreaCount; }
            set { pintScanAreaCount = value; }
        }

        private static bool[] pbolAreaScan;
        public bool[] proAreaScan
        {
            get { return pbolAreaScan; }
            set { pbolAreaScan = value; }
        }

        //String배열을 받아서 String배열에 저장후 String을 배열 전체를 넘긴다.
        private static string[] parrAreaStart;      // = new string[10];
        public string[] proAreaStart
        {
            get { return parrAreaStart; }
            set { parrAreaStart = value; }
        }

        private static string[] parrAreaEnd;
        public string[] proAreaEnd
        {
            get { return parrAreaEnd; }
            set { parrAreaEnd = value; }
        }

        //해시 테이블을 받아서 전체를 넘긴다.
        private static Hashtable pAddressMapHash;
        public Hashtable proAddressMap
        {
            get { return pAddressMapHash; }
            set { pAddressMapHash = value; }
        }

        #endregion

        #region INet10property 멤버

        private static string pstrChannelNo;
        public string proChannelNo
        {
            get { return pstrChannelNo; }
            set { pstrChannelNo = value; }
        }

        private static string pstrNetworkNo;
        public string proNetworkNo
        {
            get { return pstrNetworkNo; }
            set { pstrNetworkNo = value; }
        }

        private static string pstrGroupNo;
        public string proGroupNo
        {
            get { return pstrGroupNo; }
            set { pstrGroupNo = value; }
        }

        private static string pstrStationNo;
        public string proStationNo
        {
            get { return pstrStationNo; }
            set { pstrStationNo = value; }
        }

        #endregion

        #region ITCPproperty 멤버

        private static string pstrConnectionMode;
        public string proConnectionMode
        {
            get { return pstrConnectionMode; }
            set { pstrConnectionMode = value; }
        }

        private static string pstrRemoteIP;
        public string proRemoteIP
        {
            get { return pstrRemoteIP; }
            set { pstrRemoteIP = value; }
        }

        private static int pintRemotePort;
        public int proRemotePort
        {
            get { return pintRemotePort; }
            set { pintRemotePort = value; }
        }

        #endregion

        #region I232property 멤버

        private static string pstrCommSetting;
        public string proCommSetting
        {
            get { return pstrCommSetting; }
            set { pstrCommSetting = value; }
        }

        private static string pstrCommPort;
        public string proCommPort
        {
            get { return pstrCommPort; }
            set { pstrCommPort = value; }
        }

        #endregion


    }
}
