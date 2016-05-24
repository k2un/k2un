using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoAct
{
    public class clsAPC
    {
        public string GLSID = "";
        public string JOBID = "";
        public string EQPPPID = "";
        public DateTime SetTime = new DateTime();
        public DateTime SetTime_old = new DateTime();
        public string[] ParameterName;
        public string[] ParameterValue;
        public string[] ParameterIndex;
        public string[] ParameterNameBackup;
        public string[] ParameterValueBackup;
        public int[] PACK_Name;
        public int[] PACK_Value;
        public string State = "";
        public int Mode = 1;


        //Constructor
        public clsAPC(string strGLSID)
        {
            this.GLSID = strGLSID;
        }

        public int ParameterCNT
        {
            get
            {
                if (this.ParameterName == null) return 0;
                return this.ParameterName.Length;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>20121208 cho young hoon</remarks>
        public void subParameterValueBackup()
        {
            int dintLength = ParameterValue.Length;
            this.ParameterValueBackup = new string[dintLength];
            ParameterNameBackup = new string[dintLength];
            for (int i = 0; i < dintLength; i++)
            {
                this.ParameterValueBackup[i] = ParameterValue[i];
                ParameterNameBackup[i] = ParameterName[i];
            }
        }


        public void subSetParameterCount(int dintCNT)
        {
            ParameterName = new string[dintCNT];
            ParameterValue = new string[dintCNT];
            ParameterIndex = new string[dintCNT];
            //ParameterValueBackup = new string[dintCNT];
            PACK_Name = new int[dintCNT];
            PACK_Value = new int[dintCNT];
        }
    }
}
