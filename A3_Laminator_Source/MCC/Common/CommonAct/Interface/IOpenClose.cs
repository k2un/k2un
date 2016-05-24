using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public interface IOpenClose
    {
        //*******************************************************************************
        //  Function Name : funOpenConnection()
        //  Description   : 통신할 경우 통신 방식과 장치를 오픈하고 초기화 하는 함수
        //  Parameters    : 
        //  Return Value  : 정상적으로 오픈하였는지를 리턴한다.
        //                  True -> 정상 , false -> 에러
        //  Special Notes : 
        //*******************************************************************************
        //  2007/02/06          최 성원         [L 00] 
        //*******************************************************************************
        bool funOpenConnection();

        //*******************************************************************************
        //  Function Name : funCloseConnection()
        //  Description   : 통신할 경우 통신 방식과 장치를 종료 시키는 함수
        //  Parameters    : 
        //  Return Value  : 정상적으로 클로즈 하였는지를 리턴한다.
        //                  True -> 정상 , false -> 에러
        //  Special Notes : 
        //*******************************************************************************
        //  2007/02/06          최 성원         [L 00] 
        //*******************************************************************************
        bool funCloseConnection();
    }
}
