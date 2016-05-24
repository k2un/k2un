using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public static class EnuCommunication
    {
        //통신 타입
        public enum CommunicationType
        {
            TCP = 1,
            RS232 = 2,
            NET10 = 3,
        }

        //장비 이름 
        public enum EQPNameType
        {
            BCRPC = 1,
            CMOLoaderPC = 2,
            MelsecPLC = 3,
            OmronPLC = 4,
        }

        //장비 Type(명령위주의) 
        public enum EQPCommandType
        {
            PC = 1,
            PLC = 2,
        }

        //스트링 타입
        public enum StringType
        {
            Binary = 1,
            Decimal = 2,
            Hex = 3,
            ASCCode = 4,
            ASCString = 5,
        }
    }
}
