using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEvent_TEST : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEvent_TEST(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "actTest";
        }

        #endregion

        #region Methods
        /// <summary>
        /// 설비에서 CIM으로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : strCompBit
        /// parameters[1] : dstrACTVal
        /// parameters[2] : dintActFrom
        /// parameters[3] : dstrACTFromSub
        /// parameters[4] : intBitVal
        /// parameters[5] : Special Parameter
        /// </remarks>
        public void funProcessEQPEvent(string[] parameters)
        {
            System.Windows.Forms.MessageBox.Show("TEST");
        }

        public void funProcessEQPStatus(string[] parameters)
        {
            System.Windows.Forms.MessageBox.Show("TEST");
        }

        #endregion
    }

}
