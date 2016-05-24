using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoAct
{
    public class clsXPCoverride
    {
        private DateTime pdtSetTime;
        private InfoAct.clsInfo.SFName pintSFName;
        private string pstrGLSID;

        public clsXPCoverride(DateTime setTime, InfoAct.clsInfo.SFName sfName, string glsID)
        {
            this.pdtSetTime = setTime;
            this.pintSFName = sfName;
            this.pstrGLSID = glsID;
        }

        public DateTime SetTime
        {
            get
            {
                return this.pdtSetTime;
            }
            set
            {
                this.pdtSetTime = value;
            }
        }
        public InfoAct.clsInfo.SFName SFname
        {
            get
            {
                return this.pintSFName;
            }
            set
            {
                this.pintSFName = value;
            }
        }
        public string GlassID
        {
            get
            {
                return this.pstrGLSID;
            }
            set
            {
                this.pstrGLSID = value;
            }
        }

    }
}
