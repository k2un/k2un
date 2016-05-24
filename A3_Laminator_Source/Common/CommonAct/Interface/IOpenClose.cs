using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public interface IOpenClose
    {
        /// <summary>
        /// 통신할 경우 통신 방식과 장치를 오픈하고 초기화 하는 함수
        /// </summary>
        /// <returns>정상적으로 오픈하였는지를 리턴한다. True -> 정상 , false -> 에러</returns>
        bool funOpenConnection();

        /// <summary>
        /// 통신할 경우 통신 방식과 장치를 종료 시키는 함수
        /// </summary>
        /// <returns>정상적으로 클로즈 하였는지를 리턴한다. True -> 정상 , false -> 에러</returns>
        bool funCloseConnection();
    }
}
