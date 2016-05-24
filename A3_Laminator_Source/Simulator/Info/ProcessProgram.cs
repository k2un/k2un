using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Info
{
    /// <summary>
    /// 레시피 클래스 this - ccode리스트 - 바디리스트
    /// </summary>
    [Serializable]
    public class ProcessProgram
    {
        string id;
        /// <summary>
        /// Process Program ID, read only
        /// </summary>
        public string ID
        {
            get
            {
                return this.id;
            }
            //set
            //{
            //    this.id = value;
            //}
        }

        PPIDType type;
        /// <summary>
        /// Process Program ID Type, read only
        /// </summary>
        public PPIDType TYPE
        {
            get
            {
                return this.type;
            }
            //set
            //{
            //    this.type = value;
            //}
        }

        /// <summary>
        /// 커맨드 리스트, 하부에 바디 리스트를 갖는다.
        /// </summary>
        public List<ProcessCommand> processCommands = new List<ProcessCommand>();

        List<ProcessProgram> mappingPPID = new List<ProcessProgram>();

        public void subMapping(ProcessProgram type2)
        {
            if (type2.TYPE != PPIDType.TYPE_2)
                return;
            this.mappingPPID.Add(type2);
        }

        //public List<string> funMappingList()
        //{
        //    List<string> list = new List<string>();
        //    foreach (ProcessProgram item in this.mappingPPID)
        //    {
        //        list.Add(item.ID);
        //    }
        //    return list;
        //}

        /// <summary>
        /// 사본을 리턴한다.
        /// </summary>
        /// <returns></returns>
        public List<ProcessProgram> funMappingList()
        {
            List<ProcessProgram> list = new List<ProcessProgram>();
            foreach (ProcessProgram item in this.mappingPPID)
            {
                list.Add(item);
            }
            return list;
        }

        public void subMappingDelete(ProcessProgram type2)
        {
            if (type2.TYPE != PPIDType.TYPE_2)
                return;
            foreach (ProcessProgram item in this.mappingPPID)
            {
                if (item.ID == type2.ID)
                {
                    this.mappingPPID.Remove(type2);
                }
            }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="id">PPID</param>
        /// <param name="type">PPID Type</param>
        public ProcessProgram(string id, PPIDType type)
        {
            this.id = id;
            this.type = type;
        }
    }
}
