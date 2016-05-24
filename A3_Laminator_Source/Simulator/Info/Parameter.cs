using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Info
{
    [Serializable]
    public class Parameter
    {
        string name;
        /// <summary>
        /// Process Program Body 이름
        /// </summary>
        public string P_PARM_NAME
        {
            get 
            {
                return this.name;
            }
            set 
            {
                this.name = value;
            }
        }
        
        string value;
        /// <summary>
        /// Process Program Body 값
        /// </summary>
        public string P_PARM
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// 생성자, Process Program Body 를 생성한다.
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="value">값</param>
        public Parameter(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
