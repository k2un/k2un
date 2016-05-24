using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public abstract class absPLC : absCommunication, IPLC
    {
        //함수는 추상화로 각자 구현하도록 한다.
        #region IOpenClose 멤버

        public absPLC PclsCommunicationType;              //참조할 dll 안에 있는 클래스 선언

        public abstract bool funOpenConnection();
        public abstract bool funCloseConnection();

        #endregion

        #region IPLCCommand 멤버

        //*******************************************************************************
        //  Function Name : funBitRead()
        //  Description   : PLC Bit Read 명령을 연결된 클래스에 전달한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/06          어 경태         [L 00] 
        //*******************************************************************************
        public virtual string funBitRead(string ReadAddress, string ReadLength)
        {
            //지역변수 선언
            string dstrReturnData = "";

            dstrReturnData = PclsCommunicationType.funBitRead(ReadAddress, ReadLength);

            return dstrReturnData;
        }

        //*******************************************************************************
        //  Function Name : subBitWrite()
        //  Description   : PLC Bit Write 명령을 연결된 클래스에 전달한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/06          어 경태         [L 00] 
        //*******************************************************************************
        public virtual void subBitWrite(string WriteAddress, string WriteData)
        {
            PclsCommunicationType.subBitWrite(WriteAddress, WriteData);
        }

        //*******************************************************************************
        //  Function Name : funBitWrite()
        //  Description   : PLC Bit Write 명령을 연결된 클래스에 전달하고 그 결과를 리턴한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/06          어 경태         [L 00] 
        //*******************************************************************************
        public virtual bool funBitWrite(string WriteAddress, string WriteData)
        {
            //지역변수 선언
            bool dbolReturnData = false;

            dbolReturnData = PclsCommunicationType.funBitWrite(WriteAddress, WriteData);

            return dbolReturnData;
        }

        //*******************************************************************************
        //  Function Name : funWordRead()
        //  Description   : PLC Word Read 명령을 연결된 클래스에 전달하고 그 값을 리턴한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/06          어 경태         [L 00] 
        //*******************************************************************************
        public virtual string funWordRead(string ReadAddress, string ReadLength, EnuCommunication.StringType DataType)
        {
            //지역변수 선언
            string dstrReturnData = "";

            dstrReturnData = PclsCommunicationType.funWordRead(ReadAddress, ReadLength, DataType);

            return dstrReturnData;
        }

        //*******************************************************************************
        //  Function Name : subWordWrite()
        //  Description   : PLC Word Write 명령을 연결된 클래스에 전달한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/06          어 경태         [L 00] 
        //*******************************************************************************
        public virtual void subWordWrite(string WriteAddress, string WriteData, EnuCommunication.StringType DataType)
        {
            PclsCommunicationType.subWordWrite(WriteAddress, WriteData, DataType);
        }

        //*******************************************************************************
        //  Function Name : funWordWrite()
        //  Description   : PLC Word Write 명령을 연결된 클래스에 전달하고 그 값을 리턴한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/06          어 경태         [L 00] 
        //*******************************************************************************
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
