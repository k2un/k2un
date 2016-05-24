using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public abstract class absPLC : absCommunication, IPLC
    {
        #region IOpenClose 멤버

        public absPLC PclsCommunicationType;              //참조할 dll 안에 있는 클래스 선언

        public abstract bool funOpenConnection();
        public abstract bool funCloseConnection();

        #endregion

        #region IPLCCommand 멤버

        /// <summary>
        /// PLC Bit Read 명령을 연결된 Class에 전달한다.
        /// </summary>
        /// <param name="ReadAddress"></param>
        /// <param name="ReadLength"></param>
        /// <returns></returns>
        public virtual string funBitRead(string ReadAddress, string ReadLength)
        {
            //지역변수 선언
            string dstrReturnData = "";

            dstrReturnData = PclsCommunicationType.funBitRead(ReadAddress, ReadLength);

            return dstrReturnData;
        }

        /// <summary>
        /// PLC Bit Write 명령을 연결된 클래스에 전달한다.
        /// </summary>
        /// <param name="WriteAddress"></param>
        /// <param name="WriteData"></param>
        public virtual void subBitWrite(string WriteAddress, string WriteData)
        {
            PclsCommunicationType.subBitWrite(WriteAddress, WriteData);
        }

        /// <summary>
        /// PLC Bit Write 명령을 연결된 클래스에 전달하고 그 결과를 리턴한다.
        /// </summary>
        /// <param name="WriteAddress"></param>
        /// <param name="WriteData"></param>
        /// <returns></returns>
        public virtual bool funBitWrite(string WriteAddress, string WriteData)
        {
            //지역변수 선언
            bool dbolReturnData = false;

            dbolReturnData = PclsCommunicationType.funBitWrite(WriteAddress, WriteData);

            return dbolReturnData;
        }

        /// <summary>
        /// PLC Word Read 명령을 연결된 클래스에 전달하고 그 값을 리턴한다.
        /// </summary>
        /// <param name="ReadAddress"></param>
        /// <param name="ReadLength"></param>
        /// <param name="DataType"></param>
        /// <returns></returns>
        public virtual string funWordRead(string ReadAddress, string ReadLength, EnuCommunication.StringType DataType)
        {
            //지역변수 선언
            string dstrReturnData = "";

            dstrReturnData = PclsCommunicationType.funWordRead(ReadAddress, ReadLength, DataType);

            return dstrReturnData;
        }

        /// <summary>
        /// PLC Word Write 명령을 연결된 클래스에 전달한다.
        /// </summary>
        /// <param name="WriteAddress"></param>
        /// <param name="WriteData"></param>
        /// <param name="DataType"></param>
        public virtual void subWordWrite(string WriteAddress, string WriteData, EnuCommunication.StringType DataType)
        {
            PclsCommunicationType.subWordWrite(WriteAddress, WriteData, DataType);
        }

        /// <summary>
        /// PLC Word Write 명령을 연결된 클래스에 전달하고 그 값을 리턴한다.
        /// </summary>
        /// <param name="WriteAddress"></param>
        /// <param name="WriteData"></param>
        /// <param name="DataType"></param>
        /// <returns></returns>
        public virtual bool funWordWrite(string WriteAddress, string WriteData, EnuCommunication.StringType DataType)
        {
            //지역변수 선언
            bool dbolReturnData = false;

            dbolReturnData = PclsCommunicationType.funWordWrite(WriteAddress, WriteData, DataType);

            return dbolReturnData;
        }
        
        #endregion
    }
}
