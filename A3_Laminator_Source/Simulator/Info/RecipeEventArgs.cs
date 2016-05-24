using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Info
{
    public class RecipeEventArgs
    {
        readonly public ProcessProgram pProcessProgram;
        readonly public Action pAction;

        public RecipeEventArgs(ProcessProgram aProcessProgram, Action aAction)
        {
            this.pProcessProgram = aProcessProgram;
            this.pAction = aAction;
        }
    }
}
