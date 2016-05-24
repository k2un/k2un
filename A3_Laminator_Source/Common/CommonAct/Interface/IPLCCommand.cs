using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public interface IPLCCommand
    {

        /// <summary>
        /// PLC Bit 영역에 쓰여진 값을 읽어오는 함수
        /// </summary>
        /// <param name="ReadAddress">주소</param>
        /// <param name="ReadLength">길이</param>
        /// <returns>PLC 에서 보내온 응답값</returns>
        /// <comment>
        /// 응답값은 읽어온 데이터 값이다.
        /// </comment>
        string funBitRead(string ReadAddress, string ReadLength);
        
        /// <summary>
        /// PLC Bit 영역에 값을 쓰는 함수
        /// </summary>
        /// <param name="WriteAddress">주소</param>
        /// <param name="WriteData">값</param>
        void subBitWrite(string WriteAddress, string WriteData);

        /// <summary>
        /// PLC Bit 영역에 값을 쓰는 함수
        /// </summary>
        /// <param name="WriteAddress">주소</param>
        /// <param name="WriteData">값</param>
        /// <returns>PLC 에서 보내온 응답값</returns>
        /// <comment>
        /// 응답값은 보통 정상 아니면 에러 값이다.
        /// </comment>
        bool funBitWrite(string WriteAddress, string WriteData);

        /// <summary>
        /// PLC Word 영역에 쓰여진 값을 읽어오는 함수
        /// </summary>
        /// <param name="ReadAddress">주소</param>
        /// <param name="ReadLength">길이</param>
        /// <param name="DataType">데이터 형식</param>
        /// <returns>PLC 에서 보내온 응답값</returns>
        /// <comment>
        /// 응답값은 읽어온 데이터 값이다.
        /// </comment>
        string funWordRead(string ReadAddress, string ReadLength, CommonAct.EnuCommunication.StringType DataType);

        /// <summary>
        /// PLC Word 영역에 값을 쓰는 함수
        /// </summary>
        /// <param name="WriteAddress">주소</param>
        /// <param name="WriteData">데이터</param>
        /// <param name="DataType">데이터 타입</param>
        void subWordWrite(string WriteAddress, string WriteData, CommonAct.EnuCommunication.StringType DataType);

        /// <summary>
        /// PLC Word 영역에 값을 쓰는 함수
        /// </summary>
        /// <param name="WriteAddress">주소</param>
        /// <param name="WriteData">데이터</param>
        /// <param name="DataType">데이터 타입</param>
        /// <returns>PLC 에서 보내온 응답값</returns>
        /// <comment>
        /// 응답값은 보통 정상 아니면 에러 값이다.
        /// </comment>
        bool funWordWrite(string WriteAddress, string WriteData, CommonAct.EnuCommunication.StringType DataType);

    }
}
