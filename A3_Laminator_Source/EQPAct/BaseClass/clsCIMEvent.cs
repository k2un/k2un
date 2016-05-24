using System;
using System.Collections.Generic;
using System.Text;
using InfoAct;

/// <summary>
/// Base Class of CIM Event Command Classes
/// </summary>

namespace EQPAct
{
    public class clsCIMEvent
    {
        #region Variables
        protected clsEQPAct pEqpAct = null;
        protected clsInfo pInfo = clsInfo.Instance;
        protected string strEventName = "";
        protected string strUnitID = "EQP";
        #endregion

        #region Properties
        public string StrEventName
        {
            get { return strEventName; }
        }
        #endregion

        #region Constructors
        public clsCIMEvent()
        {

        }
        #endregion

        #region Methods
        public void funSetEqpAct(EQPAct.clsEQPAct eqpAct)
        {
            pEqpAct = eqpAct;
        }
        #endregion
    }
}
