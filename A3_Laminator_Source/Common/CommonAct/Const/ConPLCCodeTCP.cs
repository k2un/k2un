using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public class ConPLCCodeTCP
    {
        public string CommandSet = "FF000A";                       //FF -> PLC 번호 (자국일 경우)
                                                                   //0A00 -> PLC 감시 타이머 (단위 250ms)
                                                                   //    두자릿수가 반대로 셋팅됨 (의도한 것은 000A 임)
                                                                   //    따라서 250 * A(10) = 2500ms 로 사용한다는 것임
        /// <summary>
        /// 커맨드별 기능 코드
        /// </summary>
        public string BitRead_StartCode   = "00";   //비트단위 일괄읽기 커맨드 코드
        public string BitWrite_StartCode  = "02";   //비트단위 일괄쓰기 커맨드 코드
        public string WordRead_StartCode  = "01";   //워드단위 일괄읽기 커맨드 코드
        public string WordWrite_StartCode = "03";   //워드단위 일괄쓰기 커맨드 코드

        /// <summary>
        /// 종별 기능 코드
        /// </summary>
        public string BitRead_FinishCode   = "80";  //비트단위 일괄읽기 종별 코드
        public string BitWrite_FinishCode  = "82";  //비트단위 일괄쓰기 종별 코드
        public string WordRead_FinishCode  = "81";  //워드단위 일괄읽기 종별 코드
        public string WordWrite_FinishCode = "83";  //워드단위 일괄쓰기 종별 코드

        /// <summary>
        /// PLC 디바이스 코드(어드레스 영역별)
        /// </summary>
        public string BitXDevice = "5820";          //입력
        public string BitYDevice = "5920";          //출력
        public string BitMDevice = "4D20";          //내부릴레이
        public string BitBDevice = "4220";          //링크릴레이
        public string BitFDevice = "4620";          //어넌시에이터

        public string WordDDevice = "4420";         //데이타 레지스터
        public string WordWdevice = "5720";         //링크 레지스터
        public string WordRdevice = "5220";         //화일 레지스터

        /// <summary>
        /// 종료 코드
        /// </summary>
        public string RegularityFinish = "00";      //정상종료
    }
}
