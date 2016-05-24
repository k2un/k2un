using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public static class FunTypeConversion
    {
        /// <summary>
        /// Asc String Data 를 원하는 Data 로 바꾸어 준다
        /// </summary>
        /// <param name="strData">ASCII String Data(AB)</param>
        /// <param name="StringType">바꾸고자 하는 Type</param>
        /// <returns></returns>
        public static string funAscStringConvert(string strData, EnuEQP.StringType StringType)
        {
            string dstrReturn = "";
            string dstrTemp = "";
            int dintTemp = 0;

            try
            {
                switch (StringType)
                {
                    //ASCII Data(AB) => Binary Data(0100 0001 0100 0010)
                    case EnuEQP.StringType.Binary:
                        dstrReturn = funAscStringConvert(strData, EnuEQP.StringType.Hex);           //ASC를 HEX로 바꾼다.
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.Binary);            //HEX를 Bynary로 바꾼다.
                        break;

                    //ASCII Data(AB) => Decimal Data(16706)
                    case EnuEQP.StringType.Decimal:
                        dstrReturn = funAscStringConvert(strData, EnuEQP.StringType.Hex);           //ASC를 HEX로 바꾼다.
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.Decimal);           //HEX를 10진수로 바꾼다.
                        break;

                    //ASCII Data(AB) => Hex Data(4142)
                    case EnuEQP.StringType.Hex:
                        for (int i = 0; i < strData.Length; i++)
                        {
                            char c = Convert.ToChar(strData.Substring(i, 1));
                            dintTemp = Convert.ToInt32(c);                                   //아스키한문자를 10진수 ASC코드(65)로 변환한다.
                            dstrTemp = string.Format("{0:X2}", dintTemp);                     //10진수를 2자리를 맞춘 16진수로 바꾼다.
                            dstrReturn += dstrTemp;
                        }
                        break;

                    //ASCII Data(AB) => ASC Code(6566)
                    case EnuEQP.StringType.ASCCode:
                        for (int i = 0; i < strData.Length; i++)
                        {
                            char c = Convert.ToChar(strData.Substring(i, 1));
                            dintTemp = Convert.ToInt32(c);                                   //아스키한문자를 10진수 ASC코드로 변환한다.
                            dstrTemp = string.Format("{0:D2}", dintTemp);                     //10진수를 10진수 2자리 문자로 바꾼다.
                            dstrReturn += dstrTemp;
                        }
                        dstrReturn = strData;
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }
            catch (Exception ex)
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData);
                throw (ex);
            }

            return dstrReturn;
        }

        /// <summary>
        /// HEX Data 를 원하는 Data 로 바꾸어 준다
        /// </summary>
        /// <param name="strData">HexData = Hex Data(4142)</param>
        /// <param name="StringType">바꾸고자 하는 Type</param>
        /// <returns></returns>
        public static string funHexConvert(string strData, EnuEQP.StringType StringType)
        {
            string dstrReturn = "";
            string dstrTemp = "";
            int dintTemp = 0;

            try
            {
                switch (StringType)
                {
                    //Hex Data(4142) => Binary Data(0100 0010 0100 0001)
                    case EnuEQP.StringType.Binary:
                        for (int i = 0; i < strData.Length; i++)
                        {
                            string c = Convert.ToString(strData.Substring(i, 1));
                            dintTemp = Convert.ToInt32(c, 16);                               //16진수를 10진수로 변환
                            dstrTemp = Convert.ToString(dintTemp, 2);                         //10진수를 2진수로 바꾼다.
                            dstrTemp = string.Format("{0:0000}", Convert.ToInt32(dstrTemp));  //앞에 0이 붙는 4자리의 2진수로 바꾼다.

                            dstrReturn += dstrTemp;
                        }
                        break;

                    //Hex Data(4142) => Decimal Data(16706)
                    case EnuEQP.StringType.Decimal:
                        dstrTemp = Convert.ToInt32(strData, 16).ToString();
                        dstrReturn = dstrTemp;
                        break;

                    //Hex Data(4142) => ASC String(AB)
                    case EnuEQP.StringType.ASCString:
                        for (int i = 0; i < strData.Length; i = i + 2)
                        {
                            string c = Convert.ToString(strData.Substring(i, 2));           //2개씩 끊어 읽어온다.
                            dintTemp = Convert.ToInt32(c, 16);                               //16진수를 10진수로 변환한다.
                            byte d = Convert.ToByte(dintTemp);                               //10진수를 Byte형태로 전환
                            dstrTemp = Convert.ToString(Convert.ToChar(d));                  //Byte형태의 10진수를 ASC 문자로 변환한다.
                            if (dstrTemp == "\0") { dstrTemp = " "; }
                            dstrReturn += dstrTemp;
                        }
                        break;

                    //Hex Data(4142) => ASC Code(6566)
                    case EnuEQP.StringType.ASCCode:
                        for (int i = 0; i < strData.Length; i = i + 2)
                        {
                            string c = Convert.ToString(strData.Substring(i, 2));           //2개씩 끊어 읽어온다.
                            dintTemp = Convert.ToInt32(c, 16);                               //16진수를 10진수로 변환한다.
                            dstrTemp = string.Format("{0:D2}", dintTemp);                     //10진수를 10진수 2자리 문자로 바꾼다.
                            dstrReturn += dstrTemp;
                        }
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }

            catch (Exception ex)
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData + ", StringType:" + StringType);
                throw (ex);
            }

            return dstrReturn;
        }

        /// <summary>
        /// Hex Data를 Swap한다(4자리로 안끊어지면 뒤에 0을 두어  Swap한다)
        /// </summary>
        /// <param name="strHexData">Hex Data(4142 4344 4546 47)</param>
        /// <returns>Swap Data(4241 4443 4645 4700)</returns>
        public static string funHexSwap(string strHexData)
        {
            string dstrReturn = "";
            string dstrTemp;

            try
            {
                if (strHexData.Length % 2 != 0)
                {
                    strHexData = "0" + strHexData;                                        //2자리로 맞춘다.
                }

                for (int i = 0; i < strHexData.Length; i = i + 4)
                {
                    dstrTemp = Convert.ToString(strHexData.Substring(i, 2));           //2개씩 끊어 읽어온다.
                    dstrTemp = dstrTemp.PadLeft(4, '0');

                    dstrReturn = dstrReturn + strHexData.Substring(i + 2, 2) + strHexData.Substring(i, 2);
                }
            }
            catch (Exception ex)
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strHexData:" + strHexData);
                throw (ex);
            }

            return dstrReturn;
        }

        /// <summary>
        /// 10진수 Data 를 원하는 Data 로 바꾸어 준다
        /// </summary>
        /// <param name="strData">Decimal Data(16706)</param>
        /// <param name="StringType">바꾸고자 하는 Type</param>
        /// <returns></returns>
        public static string funDecimalConvert(string strData, EnuEQP.StringType StringType)
        {
            string dstrReturn = "";
            string dstrTemp = "";

            try
            {
                switch (StringType)
                {
                    //Decimal Data(16706) => Binary Data(0100 0001 0100 0010)
                    case EnuEQP.StringType.Binary:
                        dstrReturn = funDecimalConvert(strData, EnuEQP.StringType.Hex);             //10진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.Binary);           //HEX를 Bynary로 바꾼다.
                        break;

                    //Decimal Data(1089) => Hex Data(0441)
                    case EnuEQP.StringType.Hex:
                        if (strData.Contains("-"))
                        {
                            if (strData.Length > 5)
                            {
                                dstrTemp = string.Format("{0:X}", Convert.ToInt64(strData));
                                if (dstrTemp.Length % 8 != 0)
                                {
                                    int dintTemp = dstrTemp.Length / 8;
                                    dintTemp = dintTemp + 1;

                                    dstrTemp = dstrTemp.PadLeft(dintTemp * 8, '0');
                                }
                                dstrTemp = dstrTemp.Substring(4, 4) + dstrTemp.Substring(0, 4);
                            }
                            else
                            {
                                dstrTemp = string.Format("{0:X}", Convert.ToInt32(strData));
                                if (dstrTemp.Length % 4 != 0)
                                {
                                    int dintTemp = dstrTemp.Length / 4;
                                    dintTemp = dintTemp + 1;

                                    dstrTemp = dstrTemp.PadLeft(dintTemp * 4, '0');
                                }
                            }
                        }
                        else
                        {
                            if (strData.Length > 4)
                            {
                                dstrTemp = string.Format("{0:X}", Convert.ToInt64(strData));
                                if (dstrTemp.Length % 8 != 0)
                                {
                                    int dintTemp = dstrTemp.Length / 8;
                                    dintTemp = dintTemp + 1;

                                    dstrTemp = dstrTemp.PadLeft(dintTemp * 8, '0');
                                }
                                dstrTemp = dstrTemp.Substring(4, 4) + dstrTemp.Substring(0, 4);
                            }
                            else
                            {
                                dstrTemp = string.Format("{0:X}", Convert.ToInt32(strData));
                                if (dstrTemp.Length % 4 != 0)
                                {
                                    int dintTemp = dstrTemp.Length / 4;
                                    dintTemp = dintTemp + 1;

                                    dstrTemp = dstrTemp.PadLeft(dintTemp * 4, '0');
                                }
                            }
                        }
                        dstrReturn = dstrTemp;
                        break;

                    //Decimal Data(16706) => ASC String(AB)
                    case EnuEQP.StringType.ASCString:
                        dstrReturn = funDecimalConvert(strData, EnuEQP.StringType.Hex);             //10진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.ASCString);        //HEX를 ASC String으로 바꾼다.
                        break;

                    //Decimal Data(16706) => ASC Code(6566)
                    case EnuEQP.StringType.ASCCode:
                        dstrReturn = funDecimalConvert(strData, EnuEQP.StringType.Hex);             //10진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.ASCCode);           //HEX를 ASC Code로 바꾼다.
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }
            catch (Exception ex)
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData + ", StringType:" + StringType);
                throw (ex);
            }

            return dstrReturn;
        }

        /// <summary>
        /// Binary Data 를 원하는 Data 로 바꾸어 준다
        /// </summary>
        /// <param name="strData">Decimal Data(16706)</param>
        /// <param name="StringType">바꾸고자 하는 Type</param>
        /// <returns></returns>
        public static string funBinConvert(string strData, EnuEQP.StringType StringType)
        {
            string dstrReturn = "";
            int dintTemp = 0;

            try
            {
                switch (StringType)
                {
                    //Binary Data(0100 0001 0100 0010) => Decimal Data(1089)
                    case EnuEQP.StringType.Decimal:
                        dstrReturn = funBinConvert(strData, EnuEQP.StringType.Hex);                 //2진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.Decimal);           //HEX를 Decimal로 바꾼다.
                        break;

                    //Binary Data(0100 0001 0100 0010) => Hex Data(0441)
                    case EnuEQP.StringType.Hex:
                        if (strData.Trim() == "") strData = "0";
                        dintTemp = Convert.ToInt32(strData, 2);                            //2진수를 10진수로 변환
                        dstrReturn = funDecimalConvert(dintTemp.ToString(), EnuEQP.StringType.Hex);               //10진수를 16진수로 변환
                        break;

                    //Binary Data(0100 0001 0100 0010) => ASC String(AB)
                    case EnuEQP.StringType.ASCString:
                        dstrReturn = funBinConvert(strData, EnuEQP.StringType.Hex);                 //2진수를 16진수¡¡¡ 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.ASCString);         //HEX를 ASC String으로 바꾼다.
                        break;

                    //Binary Data(0100 0001 0100 0010) => ASC Code(6566)
                    case EnuEQP.StringType.ASCCode:
                        dstrReturn = funBinConvert(strData, EnuEQP.StringType.Hex);                 //2진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.ASCCode);           //HEX를 ASC Code¡ 바꾼다.
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }
            catch (Exception ex)
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData + ", StringType:" + StringType);
                throw (ex);
            }

            return dstrReturn;
        }

        /// <summary>
        /// 어드레스를 원하는 만큼 증가시킨다.
        /// </summary>
        /// <param name="strBaseAddress">16진수 어드레스(W1009)</param>
        /// <param name="intStep">증가시킬 양(2)</param>
        /// <returns>증가시킨 16진수 어드레스(W100B)</returns>
        public static string funAddressAdd(string strBaseAddress, int intStep)
        {
            string dstrAddress = "";
            int dintAddress = 0;
            string dstrArea = "";

            try
            {
                dstrArea = strBaseAddress.Substring(0, 1);
                if (dstrArea == "M" || dstrArea == "D")    //M, D는 주소가 10진수임
                {
                    dstrAddress = strBaseAddress.Substring(1, strBaseAddress.Length - 1);     //어드레스만 뽑아낸다.
                    dstrAddress = Convert.ToString(Convert.ToInt32(dstrAddress) + intStep);

                    dstrAddress = strBaseAddress.Substring(0, 1) + dstrAddress;
                }
                else       //B, W는 주소가 16진수임
                {
                    dstrAddress = strBaseAddress.Substring(1, strBaseAddress.Length - 1);     //어드레스만 뽑아낸다.
                    dintAddress = Convert.ToInt32(funHexConvert(dstrAddress, EnuEQP.StringType.Decimal)) + intStep;     //10진수로 바꾸고 어드레스를 증가한다.
                    dstrAddress = string.Format("{0:X4}", dintAddress);                               //10진수를 16진수 4자리로 바꾼다.
                    dstrAddress = strBaseAddress.Substring(0, 1) + dstrAddress;
                }
            }
            catch (Exception ex)
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strBaseAddress:" + strBaseAddress + ", intStep:" + intStep);
                throw (ex);
            }

            return dstrAddress;
        }

        
        /// <summary>
        /// 인자로 넘어온 값(16진수값)이 음수이면 음수계산 로직을 탄다.
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        /// <comment>
        /// 주로 실처리 값에 사용
        /// 양의 수                1의 보수               2의 보수(음의 수표현)
        /// 0000 0000 0000 0001(1) -> 1111 1111 1111 1110 -> 1111 1111 1111 1111(-1)
        /// 0000 0000 0000 0010(2) -> 1111 1111 1111 1101 -> 1111 1111 1111 1110(-2)
        /// 0000 0000 0000 0011(3) -> 1111 1111 1111 1100 -> 1111 1111 1111 1101(-3)
        /// 0000 0000 0000 0100(4) -> 1111 1111 1111 1011 -> 1111 1111 1111 1100(-4)
        /// </comment>
        public static string funPlusMinusAPDCalc(string strData)
        {
            string strBin = "";
            string dstrTemp = "";
            string dstrValue = "";
            int dint = 0;

            try
            {
                strBin = funHexConvert(strData, EnuEQP.StringType.Binary);  //16진수 -> 2진수로 변환

                if (strBin.Substring(0, 1) == "1")  //최상위비트(MSB)가 1이면 음수값임.
                {
                    //우선 수를 0->1, 1->1로 뒤집는다.
                    for (int dintLoop = 1; dintLoop <= 16; dintLoop++)
                    {
                        if (strBin.Substring(dintLoop - 1, 1) == "0")
                        {
                            dstrTemp = dstrTemp + "1";
                        }
                        else
                        {
                            dstrTemp = dstrTemp + "0";
                        }
                    }

                    strBin = dstrTemp;

                    //위에서 뒤집은 수에다 1을 더해준다.(논리연산)
                    for (int dintLoop = 1; dintLoop <= 16; dintLoop++)
                    {
                        if (strBin.Substring(strBin.Length - dintLoop, 1) == "0")
                        {
                            dstrValue = strBin.Substring(0, 16 - dintLoop) + "1" + FunStringH.funMakeLengthStringFirst("0", dint);
                            break;      //For문을 빠져나간다.
                        }
                        else
                        {
                            dint = dint + 1;
                        }
                    }

                    dstrValue = funBinConvert(dstrValue, EnuEQP.StringType.Decimal);
                    dstrValue = "-" + dstrValue;    //계산된 2진수값을 10진수로 변환후 -를 붙여준다.(음수값이기 때문임)
                }
                else
                {
                    dstrValue = funBinConvert(strBin, EnuEQP.StringType.Decimal);
                }
            }
            catch (Exception ex)
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                throw (ex);
            }

            return dstrValue;
        }
    }
}
