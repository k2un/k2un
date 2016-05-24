using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CommonAct
{
    public class Win32
    {
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public UInt32 cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        public const Int32 WM_COPYDATA = 0x004A;

        public void SendMSG2MCC(IntPtr hMCC, String dstrMsg)
        {
            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            cds.dwData = (IntPtr)(13525);
            cds.cbData = (UInt32)dstrMsg.Length * sizeof(char);
            cds.lpData = dstrMsg;

            SendMessage(hMCC, WM_COPYDATA, IntPtr.Zero, ref cds);
        }
    }

    public class nMessageFilter : IMessageFilter
    {
        public delegate void ReceiveMCCEvent(ReceiveMCCEventArg arg);

        public event ReceiveMCCEvent ReceiveMsg;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == Win32.WM_COPYDATA)
            {
                // 메시지 카피 하고 이벤트..
                Win32.COPYDATASTRUCT cds = new Win32.COPYDATASTRUCT();
                cds = (Win32.COPYDATASTRUCT)m.GetLParam(cds.GetType());

                if (!string.IsNullOrEmpty(cds.lpData))
                {
                    ReceiveMCCEventArg args = new ReceiveMCCEventArg();

                    if (args.SetMessage(cds.lpData))
                    {
                        OnReceiveMsg(args);
                    }
                }
                return true;
            }
            return false;
        }

        protected virtual void OnReceiveMsg(ReceiveMCCEventArg arg)
        {
            if (this.ReceiveMsg != null) this.ReceiveMsg(arg);
        }


        public class ReceiveMCCEventArg
        {
            private string mType;
            private string mMsg;


            public ReceiveMCCEventArg()
            {
            }

            public string MessageType
            {
                get
                {
                    return this.mType;
                }
            }
            public string Message
            {
                get
                {
                    return this.mMsg;
                }
            }

            public bool SetMessage(string dstrMsg)
            {
                bool result = false;

                try
                {
                    if (string.IsNullOrEmpty(dstrMsg)) return false;

                    string[] temp = dstrMsg.Split(';');

                    if (temp.Length < 1) return false;

                    if (temp.Length == 1 && temp[0] == "GET")
                    {
                        this.mType = "GET";
                        this.mMsg = string.Empty;
                        result = true;
                    }
                    else if (temp.Length == 2 && temp[0] == "SET")
                    {
                        this.mType = "SET";
                        this.mMsg = temp[1];
                        result = true;
                    }
                }
                catch
                {
                    result = false;
                }

                return result;
            }
        }
    }

}
