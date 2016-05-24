using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CommonAct
{
    public static class FunINIMethod
    {
        //읽기
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        //쓰기
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);


        //*******************************************************************************
        //  Function Name : IniReadValue()
        //  Description   : INI파일로부터 데이타를 일기
        //  Parameters    : 색션", "키", iniPath
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/18          어 경태             [L 00]
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : IniWriteValue()
        //  Description   : INI파일에 데이타 쓰기
        //  Parameters    : 
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/18          어 경태             [L 00]
        //*******************************************************************************
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
