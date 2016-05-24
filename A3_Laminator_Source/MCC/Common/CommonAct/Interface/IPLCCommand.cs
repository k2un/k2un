using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public interface IPLCCommand
    {

        //*******************************************************************************
        //  Function Name : funBitRead()
        //  Description   : PLC Bit 영역에 쓰여진 값을 읽어오는 함수
        //  Parameters    : 읽어야될 주소값과 데이타의 길이
        //  Return Value  : PLC 에서 보내온 응답값을 리턴한다.
        //  Special Notes : 응답값은 읽어온 데이터 값이다.
        //*******************************************************************************
        //  2007/02/06          최 성원         [L 00] 
        //*******************************************************************************
        string funBitRead(string ReadAddress, string ReadLength);
        
        //*******************************************************************************
        //  Function Name : subBitWrite()
        //  Description   : PLC Bit 영역에 값을 쓰는 함수
        //  Parameters    : 써야될 주소값과 데이타
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/02/06          최 성원         [L 00] 
        //*******************************************************************************
        void subBitWrite(string WriteAddress, string WriteData);

        //*******************************************************************************
        //  Function Name : funBitWrite()
        //  Description   : PLC Bit 영역에 값을 쓰는 함수
        //  Parameters    : 써야될 주소값과 데이타
        //  Return Value  : PLC 에서 보내온 응답값을 리턴한다.
        //  Special Notes : 응답값은 보통 정상 아니면 에러 값이다.
        //*******************************************************************************
        //  2007/03/06          어 경태         [L 00] 
        //*******************************************************************************
        bool funBitWrite(string WriteAddress, string WriteData);


        //*******************************************************************************
        //  Function Name : funWordRead()
        //  Description   : PLC Word 영역에 쓰여진 값을 읽어오는 함수
        //  Parameters    : 읽어야될 주소값과 데이타의 길이, 데이타의 값형식
        //  Return Value  : PLC 에서 보내온 응답값을 리턴한다.
        //  Special Notes : 응답값은 읽어온 데이터 값이다.
        //*******************************************************************************
        //  2007/02/06          최 성원         [L 00] 
        //*******************************************************************************
        string funWordRead(string ReadAddress, string ReadLength, CommonAct.EnuCommunication.StringType DataType);

        //*******************************************************************************
        //  Function Name : subWordWrite()
        //  Description   : PLC Word 영역에 값을 쓰는 함수
        //  Parameters    : 써야될 주소값과 데이타
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/02/06          최 성원         [L 00] 
        //*******************************************************************************
        void subWordWrite(string WriteAddress, string WriteData, CommonAct.EnuCommunication.StringType DataType);

        //*******************************************************************************
        //  Function Name : funWordWrite()
        //  Description   : PLC Word 영역에 값을 쓰는 함수
        //  Parameters    : 써야될 주소값과 데이타
        //  Return Value  : PLC 에서 보내온 응답값을 리턴한다.
        //  Special Notes : 응답값은 보통 정상 아니면 에러 값이다.
        //*******************************************************************************
        //  2007/03/06          어 경태         [L 00] 
        //*******************************************************************************
        bool funWordWrite(string WriteAddress, string WriteData, CommonAct.EnuCommunication.StringType DataType);

    }
}
