using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    public class clsOPCallOverWrite
    {
        public int intOPCallType = 0;       //Operator Call Type
        public int intPortID = 0;           //처리할 Port번호
        public string strFromSF = "";       //발생 Stream Function(Conversaction or Validation Error 인 경우) 
        public string strHostMsg = "";      //Host By LOT Cancel인 경우 Host Message
    }
}
