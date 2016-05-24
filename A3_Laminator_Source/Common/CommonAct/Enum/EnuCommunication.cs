using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public static class EnuCommunication
    {
        /// <summary>
        /// 통신 Type
        /// </summary>
        public enum CommunicationType
        {
            TCP = 1,
            RS232 = 2,
            NET10 = 3,
        }

        /// <summary>
        /// 장비 이름
        /// </summary>
        public enum EQPNameType
        {
            BCRPC = 1,
            CMOLoaderPC = 2,
            MelsecPLC = 3,
            OmronPLC = 4,
        }

        /// <summary>
        /// 장비 Type(명령위주의) 
        /// </summary>
        public enum EQPCommandType
        {
            PC = 1,
            PLC = 2,
        }

        /// <summary>
        /// 스트링 타입
        /// </summary>
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
