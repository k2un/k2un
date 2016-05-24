using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace DBAct
{
    public class clsAlarmItem
    {
        #region Fields
        private int intAlarmID;
        private int intAlarmCode;
        private string strAlarmDesc;
        private bool bolAlarmReport;
        private string strModuleID;
        private int intUnitID;
        #endregion

        #region Properties
        public int AlarmID
        {
            get { return intAlarmID; }
            set { intAlarmID = value; }
        }
        public int AlarmCode
        {
            get { return intAlarmCode; }
            set { intAlarmCode = value; }
        }
        public string AlarmDesc
        {
            get { return strAlarmDesc; }
            set { strAlarmDesc = value; }
        }
        public bool AlarmReport
        {
            get { return bolAlarmReport; }
            set { bolAlarmReport = value; }
        }
        public string ModuleID
        {
            get { return strModuleID; }
            set { strModuleID = value; }
        }
        public int UnitID
        {
            get { return intUnitID; }
            set { intUnitID = value; }
        }
        #endregion

        #region Constructors
        public clsAlarmItem()
        {
            intAlarmID = 0;
            intAlarmCode = 0;
            strAlarmDesc = "";
            bolAlarmReport = false;
            strModuleID = "";
            intUnitID = 0;
        }
        #endregion
    }

    public class clsAlarmList
    {
        #region Fields
        private List<clsAlarmItem> listAlarm = new List<clsAlarmItem>();
        #endregion

        #region Properties
        public List<clsAlarmItem> ListAlarm
        {
            get { return listAlarm; }
            set { listAlarm = value; }
        }
        #endregion

        #region Singleton
        public static readonly clsAlarmList Instance = new clsAlarmList();
        #endregion

        #region Constructors
        public clsAlarmList()
        {
        }
        #endregion

        #region Methods
        public void WriteToXml()
        {
            StreamWriter sw = null;
            XmlSerializer xml = new XmlSerializer(this.GetType());

            string fileName = Application.StartupPath + @"\system\AlarmList.xml";

            try
            {
                sw = new StreamWriter(fileName);
                xml.Serialize(sw, this);
                sw.Close();
            }
            catch
            {

            }
        }

        public bool ReadFromXml()
        {
            string fileName = Application.StartupPath + @"\system\AlarmList.xml";

            FileInfo file = new FileInfo(fileName);

            if (file.Exists == false)
            {
                return false;
            }

            StreamReader sr = new StreamReader(fileName);
            XmlSerializer xml = new XmlSerializer(typeof(clsAlarmList));
            clsAlarmList list = xml.Deserialize(sr) as clsAlarmList;

            return true;
        }
        #endregion
    }
}
