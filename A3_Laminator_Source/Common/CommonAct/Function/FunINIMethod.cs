using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CommonAct
{
    public static class FunINIMethod
    {
        /// <summary>
        /// 읽기
        /// </summary>
        /// <param name="lpAppName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpDefault"></param>
        /// <param name="lpReturnedString"></param>
        /// <param name="nSize"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        /// <summary>
        /// 쓰기
        /// </summary>
        /// <param name="lpAppName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpString"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);


        /// <summary>
        /// INI파일로부터 데이타를 일기
        /// </summary>
        /// <param name="strSection"></param>
        /// <param name="strKey"></param>
        /// <param name="strDefault"></param>
        /// <param name="strINIPath"></param>
        /// <returns></returns>
        public static string funINIReadValue(String strSection, String strKey, String strDefault, String strINIPath)
        {
            StringBuilder dstrResult = new StringBuilder(255);
            int i = 0;

            try
            {
                i = GetPrivateProfileString(strSection, strKey, strDefault, dstrResult, 255, strINIPath);   //"색션", "키", "Default", result, size, iniPath
            }
            catch
            {
            }

            return dstrResult.ToString();
        }

        /// <summary>
        /// INI파일에 데이타 쓰기
        /// </summary>
        /// <param name="strSection"></param>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <param name="strINIPath"></param>
        public static void subINIWriteValue(String strSection, String strKey, String strValue, String strINIPath)
        {
            try
            {
                WritePrivateProfileString(strSection, strKey, strValue, strINIPath);                            //"색션", "키", "설정할값", iniPath
            }
            catch
            {
            }
        }

    }
}
