using System;
using System.Text;

namespace InfoAct
{
    public class clsProcessStep
    {
        #region Variables
        private string pstrStepNo;
        private string pstrStepDesc;
        private string pstrStartModuleID;
        private string pstrEndModuleID;
        private string pstrProcRange;
        #endregion

        #region Constructor
        public clsProcessStep(string strStepNo)
        {
            int dintTemp;
            
            if (int.TryParse(strStepNo, out dintTemp))
            {
                this.pstrStepNo = strStepNo;
            }
        }
        public clsProcessStep(int intStepNo)
        {
            this.pstrStepNo = intStepNo.ToString();
        }
        #endregion

        #region Properties
        public string StepNo
        {
            get
            {
                return this.pstrStepNo;
            }
            set
            {
                int dintTemp;

                if (int.TryParse(value, out dintTemp))
                {
                    this.pstrStepNo = value;
                }
            }
        }
        public int StepNO
        {
            get
            {
                return Convert.ToInt32(this.pstrStepNo);
            }
        }
        public string StepDesc
        {
            get
            {
                return this.pstrStepDesc;
            }
            set
            {
                this.pstrStepDesc = value;
            }
        }
        public string StartModuleID
        {
            get
            {
                return this.pstrStartModuleID;
            }
            set
            {
                this.pstrStartModuleID = value;
            }
        }
        public string EndModuleID
        {
            get
            {
                return this.pstrEndModuleID;
            }
            set
            {
                this.pstrEndModuleID = value;
            }
        }
        public string ProcessEvent
        {
            get
            {
                return this.pstrProcRange;
            }
            set
            {
                this.pstrProcRange = value;
            }
        }
        #endregion
    }
}
