using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public static class EnuEQP
    {
        public enum PLCRWType
        {
            ASCII_Data = 1,                                         //ASCII Data�� Read/Write
            Binary_Data = 2,                                        //Binary Data�� Read/Write
            Hex_Data = 3,                                           //Hex Data�� Read/Write
            Int_Data = 4,                                           //Integer Data�� Read/Write
        }

        public enum CommunicationType
        {
            RS232 = 1,
            TCPIP = 2,
            NET10 = 3,
        }

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
