using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public static class FunStringH
    {
        /// <summary>
        /// Null값이면 공백(Empty)으로 반환한다.
        /// Null값이 아니면 받은 값 그대로 리턴한다.
        /// </summary>
        /// <param name="objString">체크할 인자</param>
        /// <returns></returns>
        /// <comment>
        /// 예) "ABC" -> "ABC", Null -> ""
        /// </comment>
        public static string funNullToEmpty(Object objString)
        {
            string dstrReturn = "";

            try
            {
                if (objString == null)
                {
                    dstrReturn = System.String.Empty;
                }
                else
                {
                    dstrReturn = objString.ToString();
                }
            }
            catch
            {
                if (objString == null)
                {
                    //LogAct.clsLogAct.subWriteCIMLog(ex.ToString());
                }
                else
                {
                    //LogAct.clsLogAct.subWriteCIMLog(ex.ToString() + ", objString:" + objString.ToString());
                }
            }

            return dstrReturn;
        }

        /// <summary>
        /// strSource문자열에서 strTarger의 개수를 찾는다
        /// </summary>
        /// <param name="strSource">비교대상 String</param>
        /// <param name="strTarger">찾을 String</param>
        /// <returns>strTarger의 개수</returns>
        public static int funStringCnt(string strSource, string strTarger)
        {
            int dintReturn = 0;
            string[] dstrArray;
            char dstrTarger;

            try
            {
                if (strSource != "" && strTarger != "")
                {
                    dstrTarger = strTarger[0];

                    dstrArray = strSource.Split(dstrTarger);
                    dintReturn = dstrArray.Length - 1;
                }
            }
            catch
            {
                //LogAct.clsLogAct.subWriteCIMLog(ex.ToString() + ", strSource:" + strSource + ", strTarger:" + strTarger);
            }

            return dintReturn;
        }

        /// <summary>
        /// param[] 형태의 배열을 콤마로 구분된 문자열로 반환한다.
        /// </summary>
        /// <param name="objString">params string[] => 가변길이 배열</param>
        /// <returns>콤마로 구분된 문자열</returns>
        /// <comment>
        /// 주로 출력시 사용한다.
        /// </comment>
        public static string funArrayToString(params string[] objString)
        {
            string dstrReturn = "";

            try
            {
                if (objString != null && objString.Length != 1)
                {
                    for (int i = 0; i <= objString.Length - 1; i++)
                    {
                        dstrReturn = dstrReturn + objString[i] + ",";
                    }
                }
            }
            catch
            {
                //LogAct.clsLogAct.subWriteCIMLog(ex.ToString());
            }

            return dstrReturn;
        }

        /// <summary>
        /// 인자로 넘어온 문자열을 지정한 길이로 맞추어 준다.
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="intLen"></param>
        /// <returns>지정한 길이로 맞춘 문자열</returns>
        public static string funMakeLengthStringFirst(string strData, int intLen)
        {
            string dstrReturn = "";

            try
            {
                if (intLen == 0)
                {
                    dstrReturn = "";
                }
                else
                {
                    dstrReturn = strData.PadLeft(intLen, '0');
                }
            }
            catch
            {
                //LogAct.clsLogAct.subWriteCIMLog(ex.ToString() + ", strData:" + strData + ", intLen:" + intLen);
            }

            return dstrReturn;
        }

        /// <summary>
        /// 인자로 넘어온 문자열을 지정한 길이로 맞추어 준다.
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="intLen"></param>
        /// <returns>지정한 길이로 맞춘 문자열</returns>
        public static string funMakeLengthStringLast(string strData, int intLen)
        {
            string dstrReturn = "";

            try
            {
                if (intLen == 0)
                {
                    dstrReturn = "";
                }
                else
                {
                    dstrReturn = strData.PadRight(intLen, '0');
                }
            }
            catch
            {
                //LogAct.clsLogAct.subWriteCIMLog(ex.ToString() + ", strData:" + strData + ", intLen:" + intLen);
            }

            return dstrReturn;
        }

        /// <summary>
        /// intLen길이에서 strData의 길이를 뺀 나머지 길이만큼의 공백을 데이타 뒤에 더해 리턴한다
        /// </summary>
        /// <param name="strData">문자(ex:123)</param>
        /// <param name="intLen">원하는 공백(ex:5)</param>
        /// <returns>"123  " Space 2개</returns>
        public static string funStringData(string strData, int intLen)
        {
            string dstrReturn = "";

            try
            {
                dstrReturn = strData.Trim();
                if (intLen < 0) return dstrReturn;

                dstrReturn = strData.PadRight(intLen, ' ');
            }
            catch
            {
                //LogAct.clsLogAct.subWriteCIMLog(ex.ToString() + ", strData:" + strData + ", intLen:" + intLen);
            }

            return dstrReturn;
        }

        /// <summary>
        /// strData에 intLen길이만큼 오른족에 strPaddingData를 채움
        /// </summary>
        /// <param name="strData">문자(ex:YYY)</param>
        /// <param name="intLen">원하는 길이(ex:5)</param>
        /// <param name="chrPaddingData">오른쪽에 채울 문자열(N)</param>
        /// <returns>"YYYNN"</returns>
        public static string funPaddingStringData(string strData, int intLen, char chrPaddingData)
        {
            string dstrReturn = "";

            try
            {
                dstrReturn = strData.Trim();
                if (intLen < 0) return dstrReturn;

                dstrReturn = strData.PadRight(intLen, chrPaddingData);
            }
            catch
            {
                //LogAct.clsLogAct.subWriteCIMLog(ex.ToString() + ", strData:" + strData + ", intLen:" + intLen);
            }

            return dstrReturn;
        }

        /// <summary>
        /// 인자로 넘어온 데이터 값을 지정한 Format으로 구성한다.
        /// </summary>
        /// <param name="strValue">Data</param>
        /// <param name="strFormat">Format(예) 99.99, 99.9, 9.99 등)</param>
        /// <returns></returns>
        /// <comment>
        /// 주로 GLS, LOT APD 값에 소수점을 붙이는데 사용
        /// </comment>
        public static string funPoint(string strValue, string strFormat)
        {
            string dstrReturn = "";
            int dintLength = 0;         //소수점을 제외한 Format의 문자열 길이
            int dintPointLocation = 0;  //소수점 자리수

            try
            {
                strValue = strValue.Trim();
                dstrReturn = strValue;

                //값이 공백이거나 0이면 소수점을 붙이지 않고 그냥 빠져나간다.
                //if (strValue == "" || strValue == "0") return dstrReturn;
                if (strValue == "" ) return dstrReturn;
                //만약 인자로 넘오온 데이터의 Format이 99.99이고 인자값이 9.9인 경우 빠져나간다. - 101028 김중권
                if (strValue.Contains(".") == true) return dstrReturn;

                //만약 소수점을 붙이지 않는 값이면 그냥 빠져나간다.
                //if (strFormat.Contains(".") == false) return dstrReturn;

                //소수점 자리수를 가져온다.
                dintPointLocation = strFormat.Substring(strFormat.IndexOf('.') + 1).Length;

                if (strValue.StartsWith("-"))
                {
                    strValue = strValue.TrimStart('-');

                    dintLength = strFormat.Replace(".", "").Length; //인자로 넘어온 데이터와 포맷을 비교하여 자리수가 맞지 않으면 왼쪽에 0으로 채운다.
                    if (strValue.Length != dintLength)
                    {
                        strValue = strValue.PadLeft(dintLength, '0');   //자리수가 맞지 않으면 왼쪽에 0을 채운다.
                    }

                    strValue = Convert.ToInt32(strValue.Substring(0, strValue.Length - dintPointLocation)).ToString() + "." +
                                               strValue.Substring(strValue.Length - dintPointLocation, dintPointLocation);      //소수점을 붙인다.
                    strValue = '-' + strValue;
                }
                else
                {
                    dintLength = strFormat.Replace(".", "").Length; //인자로 넘어온 데이터와 포맷을 비교하여 자리수가 맞지 않으면 왼쪽에 0으로 채운다.
                    if (strValue.Length != dintLength)
                    {
                        strValue = strValue.PadLeft(dintLength, '0');   //자리수가 맞지 않으면 왼쪽에 0을 채운다.
                    }

                    if (strValue.Length != dintPointLocation)
                    {
                        strValue = Convert.ToInt32(strValue.Substring(0, strValue.Length - dintPointLocation)).ToString() + "." +
                                                   strValue.Substring(strValue.Length - dintPointLocation, dintPointLocation);      //소수점을 붙인다.
                    }
                    else
                    {
                        strValue = Convert.ToInt32(strValue).ToString();
                    }
                }
                
                dstrReturn = strValue;
            }
            catch
            {
            }

            return dstrReturn;
        }

        /// <summary>
        /// 문자열 사이값 가져오기
        /// </summary>
        /// <param name="str">Data</param>
        /// <param name="begin">시작 문자열</param>
        /// <param name="end">마지막 문자열</param>
        /// <returns>문자열 사이값</returns>
        public static string funGetMiddleString(string strValue, string begin, string end)
        {
            string dstrReturn = "";

            try
            {
                if (string.IsNullOrEmpty(strValue))
                {
                    return null;
                }

                if (strValue.IndexOf(begin) > -1)
                {
                    strValue = strValue.Substring(strValue.IndexOf(begin) + begin.Length);
                    if (strValue.IndexOf(end) > -1) dstrReturn = strValue.Substring(0, strValue.IndexOf(end));
                    else dstrReturn = strValue;
                }
            }
            catch
            {
            }
            return dstrReturn;
        }

        /// <summary>
        /// 인자로 넘어온 데이터 값을 지정한 Format으로 구성한다.
        /// </summary>
        /// <param name="strValue">Data</param>
        /// <param name="strFormat">Format(예) 99.99, 99.9, 9.99 등)</param>
        /// <returns></returns>
        /// <comment>
        /// PLC에 써줄 Data를 만듬(소주점을 쓸 수 없기 때문)
        /// </comment>
        public static string funMakePointPLCData(string strValue, string strFormat)
        {
            string dstrReturn = "";
            int dintLength = 0;         //소수점을 제외한 Format의 문자열 길이
            int dintPointLocation = 0;  //소수점 자리수

            try
            {
                strValue = strValue.Trim();
                dstrReturn = strValue;

                //값이 공백이거나 0이면 소수점을 붙이지 않고 그냥 빠져나간다.
                if (strValue == "" || strValue == "0") return dstrReturn;

                //소수점 자리수를 가져온다.
                dintPointLocation = strFormat.Substring(strFormat.IndexOf('.') + 1).Length;


                dintLength = strFormat.Replace(".", "").Length; //인자로 넘어온 데이터와 포맷을 비교하여 자리수가 맞지 않으면 왼쪽에 0으로 채운다.
                if (strValue.Length != dintLength)
                {
                    strValue = strValue.PadLeft(dintLength, '0');   //자리수가 맞지 않으면 왼쪽에 0을 채운다.
                }

                strValue = Convert.ToInt32(strValue.Substring(0, strValue.Length - dintPointLocation)).ToString()
                    + "." + strValue.Substring(strValue.Length - dintPointLocation, dintPointLocation);      //소수점을 붙인다.
                
                dstrReturn = strValue;
            }
            catch
            {
            }

            return dstrReturn;
        }

        /// <summary>
        /// 인자로 받은 시간을 hhmmss로 변환한다.
        /// </summary>
        /// <param name="strMinuteTime">인자: (0 ~ 99분)</param>
        /// <returns>90분 -> hhmmss(013000) 이렇게 변환한다.</returns>
        public static string funConvertTime(string strMinuteTime)
        {
            int dintMinuteTime = 0;
            string dstrReturn = "";

            try
            {
                dintMinuteTime = Convert.ToInt32(strMinuteTime);
                if (dintMinuteTime >= 60)
                {
                    dstrReturn = Convert.ToString(dintMinuteTime - 60);
                    dstrReturn = "01" + string.Format("{0:00}", Convert.ToInt32(dstrReturn)) + "00";
                }
                else
                {
                    dstrReturn = "00" + string.Format("{0:00}", Convert.ToInt32(dintMinuteTime)) + "00";
                }

            }
            catch
            {
                dstrReturn = "00" + string.Format("{0:00}", Convert.ToInt32(dintMinuteTime)) + "00";
            }

             return dstrReturn;
        }

        /// <summary>
        /// 인자로 넘어온 데이터 값의 소수점을 제외한다.
        /// </summary>
        /// <param name="strValue">Data</param>
        /// <returns></returns>
        /// <comment>
        /// PLC에 써줄 Data를 만듬(소주점을 쓸 수 없기 때문)
        /// </comment>
        public static string funMakePLCData(string strValue)
        {
            string dstrReturn = "";
            int dintLength = 0;         //소수점을 제외한 Format의 문자열 길이
            int dintPointLocation = 0;  //소수점 자리수
            string[] arrData;
            try
            {
                strValue = strValue.Trim();
                arrData = strValue.Split('.');

                if (arrData.Length < 2) return strValue;

                for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                {
                    dstrReturn += arrData[dintLoop];
                }
            }
            catch
            {
            }

            return dstrReturn;
        }

        public static string funMakeRound(string strValue, string strFormat)
        {
            string dstrReturn = strValue;

            try
            {
                string[] tempFormat = strFormat.Split('.');


                if (tempFormat.Length > 1)
                {
                    int dintRound = tempFormat[1].Length;

                    if (strValue.Contains("."))
                    {
                        int dintPoint = strValue.IndexOf('.') + 1;

                        if (strValue.Substring(dintPoint).Length > dintRound) dstrReturn = strValue.Substring(0, dintPoint + dintRound);
                        else dstrReturn = strValue.PadRight(dintPoint + dintRound, '0');
                    }
                    else
                    {
                        strValue += ".";
                        dstrReturn = strValue.PadRight(strValue.Length + dintRound, '0');
                    }
                }
                else
                {
                    if (strValue.Contains("."))
                    {
                        dstrReturn = strValue.Remove(strValue.IndexOf('.'));
                    }
                }
            }
            catch
            {
                dstrReturn = string.Empty;
            }

            return dstrReturn;
        }
        
    }
}