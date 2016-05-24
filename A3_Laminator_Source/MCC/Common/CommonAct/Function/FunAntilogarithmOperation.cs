using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public class FunAntilogarithmOperation
    {
        //Data를 User가 원하는 Type으로 변경한다
        #region"Data Type Change Function"

        //*******************************************************************************
        //  Function Name : funAscStringConvert()
        //  Description   : Asc String Data 를 원하는 Data 로 바꾸어 준다
        //  Parameters    : strData         => ASCII String Data(AB)
        //                  dStringType     => 바꾸고자 하는 Type
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/07          어 경태         [L 00] 
        //*******************************************************************************
        public string funAscStringConvert(string strData, EnuCommunication.StringType StringType)
        {
            string dstrReturn = "";
            string dstrTemp = "";
            int dintTemp = 0;

            try
            {
                switch (StringType)
                {
                    //ASCII Data(AB) => Binary Data(0100 0001 0100 0010)
                    case CommonAct.EnuCommunication.StringType.Binary:
                        dstrReturn = funAscStringConvert(strData, CommonAct.EnuCommunication.StringType.Hex); //ASC를 HEX로 바꾼다.
                        dstrReturn = funHexConvert(dstrReturn, CommonAct.EnuCommunication.StringType.Binary); //HEX를 Bynary로 바꾼다.
                        break;

                    //ASCII Data(AB) => Decimal Data(16706)
                    case CommonAct.EnuCommunication.StringType.Decimal:
                        dstrReturn = funAscStringConvert(strData, CommonAct.EnuCommunication.StringType.Hex); //ASC를 HEX로 바꾼다.
                        dstrReturn = funHexConvert(dstrReturn, CommonAct.EnuCommunication.StringType.Decimal);//HEX를 10진수로 바꾼다.
                        break;

                    //ASCII Data(AB) => Hex Data(4142)
                    case CommonAct.EnuCommunication.StringType.Hex:
                        for (int i = 0; i < strData.Length; i++)
                        {
                            char c = Convert.ToChar(strData.Substring(i, 1));
                            dintTemp = Convert.ToInt32(c);                          //아스키한문자를 10진수 ASC코드(65)로 변환한다.
                            dstrTemp = string.Format("{0:X2}", dintTemp);           //10진수를 2자리를 맞춘 16진수로 바꾼다.
                            dstrReturn += dstrTemp;
                        }
                        break;

                    //ASCII Data(AB) => ASC Code(6566)
                    case CommonAct.EnuCommunication.StringType.ASCCode:
                        for (int i = 0; i < strData.Length; ++i)
                        {
                            char c = Convert.ToChar(strData.Substring(i, 1));
                            dintTemp = Convert.ToInt32(c);                         //아스키한문자를 10진수 ASC코드로 변환한다.
                            dstrTemp = string.Format("{0:D2}", dintTemp);          //10진수를 10진수 2자리 문자로 바꾼다.
                            dstrReturn += dstrTemp;
                        }
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }
            catch
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData);
            }

            return dstrReturn;
        }

        //*******************************************************************************
        //  Function Name : funHexConvert()
        //  Description   : HEX Data 를 원하는 Data 로 바꾸어 준다
        //  Parameters    : strData         => HexData = Hex Data(4142)
        //                  dStringType     => 바꾸고자 하는 Type
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00] 
        //*******************************************************************************
        public string funHexConvert(string strData, CommonAct.EnuCommunication.StringType StringType)
        {
            string dstrReturn = "";
            string dstrTemp = "";
            int dintTemp = 0;

            try
            {
                switch (StringType)
                {
                    //Hex Data(4142) => Binary Data(0100 0010 0100 0001)
                    case CommonAct.EnuCommunication.StringType.Binary:
                        for (int i = 0; i < strData.Length; i++)
                        {
                            string c = Convert.ToString(strData.Substring(i, 1));
                            dintTemp = Convert.ToInt32(c, 16);                               //16진수를 10진수로 변환
                            dstrTemp = Convert.ToString(dintTemp, 2);                        //10진수를 2진수로 바꾼다.
                            dstrTemp = string.Format("{0:0000}", Convert.ToInt32(dstrTemp)); //앞에 0이 붙는 4자리의 2진수로 바꾼다.

                            dstrReturn += dstrTemp;
                        }
                        break;

                    //Hex Data(4142) => Decimal Data(16706)
                    case CommonAct.EnuCommunication.StringType.Decimal:
                        dstrTemp = Convert.ToInt32(strData, 16).ToString();
                        dstrReturn = dstrTemp;
                        break;

                    //Hex Data(4142) => ASC String(AB)
                    case CommonAct.EnuCommunication.StringType.ASCString:
                        for (int i = 0; i < strData.Length; i = i + 2)
                        {
                            string c = Convert.ToString(strData.Substring(i, 2));            //2개씩 끊어 읽어온다.
                            dintTemp = Convert.ToInt32(c, 16);                               //16진수를 10진수로 변환한다.
                            byte d = Convert.ToByte(dintTemp);                               //10진수를 Byte형태로 전환
                            dstrTemp = Convert.ToString(Convert.ToChar(d));                  //Byte형태의 10진수를 ASC 문자로 변환한다.
                            dstrReturn += dstrTemp;
                        }
                        break;

                    //Hex Data(4142) => ASC Code(6566)
                    case CommonAct.EnuCommunication.StringType.ASCCode:
                        for (int i = 0; i < strData.Length; i = i + 2)
                        {
                            string c = Convert.ToString(strData.Substring(i, 2));           //2개씩 끊어 읽어온다.
                            dintTemp = Convert.ToInt32(c, 16);                              //16진수를 10진수로 변환한다.
                            dstrTemp = string.Format("{0:D2}", dintTemp);                   //10진수를 10진수 2자리 문자로 바꾼다.
                            dstrReturn += dstrTemp;
                        }
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }
            catch
            {
               // this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData + ", StringType:" + StringType);
            }

            return dstrReturn;
        }

        //*******************************************************************************
        //  Function Name : funDecimalConvert()
        //  Description   : 10진수 Data 를 원하는 Data 로 바꾸어 준다
        //  Parameters    : strData => Decimal Data(16706)
        //                  dStringType     => 바꾸고자 하는 Type
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00] 
        //*******************************************************************************
        public string funDecimalConvert(string strData, CommonAct.EnuCommunication.StringType StringType)
        {
            string dstrReturn = "";
            string dstrTemp = "";

            try
            {
                switch (StringType)
                {
                    //Decimal Data(16706) => Binary Data(0100 0001 0100 0010)
                    case CommonAct.EnuCommunication.StringType.Binary:
                        dstrReturn = funDecimalConvert(strData, CommonAct.EnuCommunication.StringType.Hex);   //10진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, CommonAct.EnuCommunication.StringType.Binary); //HEX를 Bynary로 바꾼다.
                        break;

                    //Decimal Data(1089) => Hex Data(0441)
                    case CommonAct.EnuCommunication.StringType.Hex:
                        dstrTemp = string.Format("{0:X}", Convert.ToInt32(strData));                    //10진수를 16진수로 바꾼다.
                        if (dstrTemp.Length % 4 != 0)
                        {
                            int dintTemp = dstrTemp.Length / 4;
                            dintTemp = dintTemp + 1;

                            dstrTemp = dstrTemp.PadLeft(dintTemp * 4, '0');
                        }
                        dstrReturn = dstrTemp;
                        break;

                    //Decimal Data(16706) => ASC String(AB)
                    case CommonAct.EnuCommunication.StringType.ASCString:
                        dstrReturn = funDecimalConvert(strData, CommonAct.EnuCommunication.StringType.Hex);       //10진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, CommonAct.EnuCommunication.StringType.ASCString);  //HEX를 ASC String으로 바꾼다.
                        break;

                    //Decimal Data(16706) => ASC Code(6566)
                    case CommonAct.EnuCommunication.StringType.ASCCode:
                        dstrReturn = funDecimalConvert(strData, CommonAct.EnuCommunication.StringType.Hex);       //10진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, CommonAct.EnuCommunication.StringType.ASCCode);    //HEX를 ASC Code로 바꾼다.
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }
            catch
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData + ", StringType:" + StringType);
            }

            return dstrReturn;
        }

        //*******************************************************************************
        //  Function Name : funBinConvert()
        //  Description   : Binary Data 를 원하는 Data 로 바꾸어 준다
        //  Parameters    : strHEXData => Decimal Data(16706)
        //                  dStringType     => 바꾸고자 하는 Type
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00] 
        //*******************************************************************************
        public string funBinConvert(string strData, CommonAct.EnuCommunication.StringType StringType)
        {
            string dstrReturn = "";
            int dintTemp = 0;

            try
            {
                switch (StringType)
                {
                    //Binary Data(0100 0001 0100 0010) => Decimal Data(1089)
                    case CommonAct.EnuCommunication.StringType.Binary:
                        dstrReturn = funBinConvert(strData, CommonAct.EnuCommunication.StringType.Hex);                   //2진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, CommonAct.EnuCommunication.StringType.Decimal);            //HEX를 Decimal로 바꾼다.
                        break;

                    //Binary Data(0100 0001 0100 0010) => Hex Data(0441)
                    case CommonAct.EnuCommunication.StringType.Hex:
                        dintTemp = Convert.ToInt32(strData, 2);                                                     //2진수를 10진수로 변환
                        dstrReturn = funDecimalConvert(dintTemp.ToString(), CommonAct.EnuCommunication.StringType.Hex);   //10진수를 16진수로 변환
                        break;

                    //Binary Data(0100 0001 0100 0010) => ASC String(AB)
                    case CommonAct.EnuCommunication.StringType.ASCString:
                        dstrReturn = funBinConvert(strData, CommonAct.EnuCommunication.StringType.Hex);                   //2진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, CommonAct.EnuCommunication.StringType.ASCString);          //HEX를 ASC String으로 바꾼다.
                        break;

                    //Binary Data(0100 0001 0100 0010) => ASC Code(6566)
                    case CommonAct.EnuCommunication.StringType.ASCCode:
                        dstrReturn = funBinConvert(strData, CommonAct.EnuCommunication.StringType.Hex);                   //2진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, CommonAct.EnuCommunication.StringType.ASCString);          //HEX를 ASC Code로 바꾼다.
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }
            catch
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData + ", StringType:" + StringType);
            }

            return dstrReturn;
        }

        //*******************************************************************************
        //  Function Name : funAscCodeConvert()
        //  Description   : Asc Code Data 를 원하는 Data 로 바꾸어 준다
        //  Parameters    : strData         => ASCII Code Data
        //                  dStringType     => 바꾸고자 하는 Type
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/02/15          최 성원         [L 00] 
        //*******************************************************************************
        public string funAscCodeConvert(string strData, CommonAct.EnuCommunication.StringType StringType)
        {
            //지역변수 선언
            string dstrReturn = "";     //리턴할 값
            string dstrTemp = "";       //임시로 저장할 함수
            int dintLength = 0;         //스트링 길이
            string dstrDataTemp = "";   //임시 저장장소


            try
            {
                dintLength = strData.Length;

                //변환 가능한 범위를 숫자와 영어대문자로 가정한다.
                if (dintLength % 2 == 0)
                {
                    //아스키 스트링형으로 변환
                    for (int i = 0; i < dintLength; i=i+2)
                    {
                        int dintTemp = Convert.ToInt32(strData.Substring(i, 2));    //아스키 코드를 가져와서 int 형으로 변환
                        dstrTemp = string.Format("{0:X}", dintTemp);                //Hex 형식으로 변환

                        dstrDataTemp = dstrDataTemp + dstrTemp;
                    }

                    //Hex 변환함수에서 다른 타입으로 변환을 한다.
                    switch (StringType)
                    {
                        case CommonAct.EnuCommunication.StringType.Binary:
                            dstrReturn = funHexConvert(dstrDataTemp, CommonAct.EnuCommunication.StringType.Binary);
                            break;

                        case CommonAct.EnuCommunication.StringType.Hex:
                            dstrReturn = dstrDataTemp;
                            break;

                        case CommonAct.EnuCommunication.StringType.Decimal:
                            dstrReturn = funHexConvert(dstrDataTemp, CommonAct.EnuCommunication.StringType.Decimal);
                            break;

                        case CommonAct.EnuCommunication.StringType.ASCString:
                            dstrReturn = funHexConvert(dstrDataTemp, CommonAct.EnuCommunication.StringType.ASCString);
                            break;

                        default:
                            dstrReturn = strData;
                            break;
                    }
                }
                else
                {
                    //변환 불가능
                }
            }
            catch
            {
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData + ", StringType:" + StringType);
            }

            return dstrReturn;
        }
        #endregion

        //어드레스 변환
        #region "어드레스 변환"

        //*******************************************************************************
        //  Function Name : funHexSwap()
        //  Description   : Hex Data를 Swap한다(4자리로 안끊어지면 뒤에 0을 두어  Swap한다)
        //  Parameters    : HexData => Hex Data(41 4243 4445 4647)
        //  Return Value  : Swap Data(4100 4342 4544 4746)
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00]
        //  2007/02/14          최 성원         [L 01]  자릿수 계산 수정
        //*******************************************************************************
        public string funHexSwap(string strHexData)
        {
            //지역변수 선언
            string dstrReturn = "";     //리턴할 주소값
            string dstrTemp = "";       //임시로 저장할 변수
            int dintMod;                //자릿수 계산을 위한 나머지 변수

            try
            {
                if (strHexData.Length % 4 != 0)
                {
                    dintMod = strHexData.Length % 4;
                    dstrTemp = dstrTemp.PadLeft(dintMod, '0');
                    strHexData = dstrTemp + strHexData;
                }

                for (int i = 0; i < strHexData.Length; i = i + 4)
                {
                    dstrReturn = dstrReturn + strHexData.Substring(i + 2, 2) + strHexData.Substring(i, 2);
                }
            }
            catch
            {
                // this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strHexData:" + strHexData);
            }

            return dstrReturn;
        }

        //*******************************************************************************
        //  Function Name : funAddressAdd()
        //  Description   : 어드레스를 원하는 만큼 증가시킨다.
        //  Parameters    : strBaseAddress => 16진수 어드레스(W1009)
        //                  intStep => 증가시킬 양(2)
        //  Return Value  : 증가시킨 16진수 어드레스(W100B)
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00] 
        //*******************************************************************************
        public string funAddressAdd(string strBaseAddress, int intStep)
        {
            string dstrAddress = "";
            int dintAddress = 0;

            try
            {
                //어드레스만 뽑아낸다.
                dstrAddress = strBaseAddress.Substring(1, strBaseAddress.Length - 1);

                //10진수로 바꾸고 어드레스를 증가한다.
                dintAddress = Convert.ToInt32(this.funHexConvert(dstrAddress, CommonAct.EnuCommunication.StringType.Decimal)) + intStep;

                //10진수를 16진수 4자리로 바꾼다.
                dstrAddress = string.Format("{0:X4}", dintAddress);                               
                dstrAddress = strBaseAddress.Substring(0, 1) + dstrAddress;
            }
            catch
            {
               // this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strBaseAddress:" + strBaseAddress + ", intStep:" + intStep);
            }

            return dstrAddress;
        }

        //*******************************************************************************
        //  Function Name : funAddressFill()
        //  Description   : 어드레스를 원하는 단위 자릿수의 배수만큼 0으로 채워준다.
        //  Parameters    : Address     => 16진수 어드레스(34)
        //                  Step        => 0을 채워서 증가할 단위 자릿수(4)
        //                  direction   => 0을 채울 방향 
        //                                 true -> 앞, false -> 뒤
        //  Return Value  : 바뀐 어드레스(0034)
        //  Special Notes : step 을 4로 설정하고 5자리로 데이터가 들어온 경우
        //                  4의 배수인 8로 자릿수를 맞추어서 0으로 채워준다.
        //                  예) Address : 12345 , Step : 4 -> 결과 : 00012345
        //*******************************************************************************
        //  2007/02/15          최 성원         [L 00]
        //*******************************************************************************
        public string funAddressFill(string Address, int Step, bool direction)
        {
            //지역변수 선언
            int dintMod = 0;                            //증가할 단위자릿수에서 채워져 있는 자릿수
            int dintPosition = 0;                       //증가할 단위자릿수에서 0으로 채울 자릿수
            string dstrTempAddress = "";                //0으로 채운 주소
            string dstrReturnAddress = "";              //리턴할 바뀐 주소값

            try
            {
                //자릿수 계산
                dintMod = Address.Length % Step;

                //단위 자릿수에 모자란 0을 채움
                if (dintMod != 0)
                {
                    dintPosition = Step - dintMod;
                    dstrTempAddress = dstrTempAddress.PadLeft(dintMod, '0');

                    if (direction == true)
                    {
                        dstrReturnAddress = dstrTempAddress + Address;                        
                    }
                    else
                    {                        
                        dstrReturnAddress = Address + dstrTempAddress;
                    }
                }
                
                //단위 자릿수와 일치하므로 그냥 리턴한다.
                else
                {
                    dstrReturnAddress = Address;
                }
            }
            catch
            {
                // this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strHexData:" + strHexData);
            }

            return dstrReturnAddress;
        }

        #endregion
    }
}
