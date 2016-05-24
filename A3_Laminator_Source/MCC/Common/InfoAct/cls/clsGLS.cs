using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    public class clsGLS
    {
        public string GLSID = "";               //현재 Unit에 있는 GLS의 GLSID
        public string LOTID = "";               //현재 Unit에 있는 GLS의 LOTID
        public int LOTIndex = 0;                //LOTIndex
        public int SlotID = 0;                  //SlotID

        //Constructor
        public clsGLS(string strGLSID)
        {
            this.GLSID = strGLSID;
        }
    }
}
