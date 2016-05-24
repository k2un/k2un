using System;
using System.Collections.Generic;
using System.Text;
using NSECS;
using CommonAct;
using InfoAct;

/// <summary>
/// Base Class of SmFn Classes
/// </summary>
namespace HostAct
{
    public class clsStreamFunction
    {
        #region StreamFunction Common Fields
        protected int intStream;
        protected int intFunction;
        protected string strPrimaryMsgName;
        protected string strSecondaryMsgName;
        protected clsInfo pInfo = clsInfo.Instance;
        protected clsHostAct pHost;
        protected NSECS.clsNSecsW pSecsDrv;
        protected Transaction pMsgTran;
        #endregion

        #region Properties
        public int IntStream
        {
            get { return intStream; }
            set { intStream = value; }
        }
        public int IntFunction
        {
            get { return intFunction; }
            set { intFunction = value; }
        }
        public string StrPrimaryMsgName
        {
            get { return strPrimaryMsgName; }
            set { strPrimaryMsgName = value; }
        }
        public string StrSecondaryMsgName
        {
            get { return strSecondaryMsgName; }
            set { strSecondaryMsgName = value; }
        }
        #endregion

        #region Constructors
        public clsStreamFunction()
        {
            intStream = 0;
            intFunction = 0;
            strPrimaryMsgName = "";
            strSecondaryMsgName = "";
        }
        #endregion

        #region Methods
        /// <summary>
        /// HostAct의 Reference를 받아온다.
        /// </summary>
        /// <param name="host">HostAct의 Reference</param>
        public void funSetHostAct(clsHostAct host)
        {
            this.pHost = host;
            this.pSecsDrv = host.PSecsDrv;
        }

        /// <summary>
        /// Secondary Message 송신
        /// </summary>
        /// <param name="msgTran"> 송신할 Transaction </param>
        protected void funSendReply(Transaction msgTran)
        {
            try
            {
                short dshtReturn = Convert.ToInt16(msgTran.Reply());

                if (dshtReturn < 0)
                {
                    string dstrLogText = string.Format("Message S{0}F{1} Send Fail. Return({2})", msgTran.Secondary().Stream.ToString(),
                                                                                              msgTran.Secondary().Function.ToString(),
                                                                                              dshtReturn.ToString());
                    funSetLog(InfoAct.clsInfo.LogType.CIM, dstrLogText);
                }
            }
            catch (Exception error)
            {
                if (pInfo != null)
                {
                    funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                    return;
                }
            }
        }

        /// <summary>
        /// Secondary Message 송신
        /// </summary>
        /// <param name="msgTran"> 송신할 Transaction </param>
        /// <param name="strReplyName"> 송신할 Transaction Name</param>
        protected void funSendReply2(Transaction msgTran, string strReplyName)
        {
            try
            {
                short dshtReturn = Convert.ToInt16(msgTran.Reply2(strReplyName));

                if (dshtReturn < 0)
                {
                    string dstrLogText = string.Format("Message S{0}F{1}({2}) Send Fail. Return({3})", msgTran.Secondary().Stream.ToString(),
                                                                                              msgTran.Secondary().Function.ToString(),
                                                                                              strReplyName,
                                                                                              dshtReturn.ToString());
                    funSetLog(InfoAct.clsInfo.LogType.CIM, dstrLogText);
                }
            }
            catch (Exception error)
            {
                if (pInfo != null)
                {
                    funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                    return;
                }
            }
        }

        /// <summary>
        /// clsSmFn에서 발생하는 Log 처리
        /// </summary>
        /// <param name="logType"> Type of LOG </param>
        /// <param name="strLogText"> Test for record </param>
        protected void funSetLog(InfoAct.clsInfo.LogType logType, string strLogText)
        {
            try
            {
                if (pInfo != null)
                {
                    pInfo.subLog_Set(logType, strLogText);
                }
            }
            catch (Exception error)
            {
                if (pInfo != null)
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                    return;
                }
            }
        }
        #endregion
    }
}
