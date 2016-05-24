using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    public class clsRobot : clsRobotMethod
    {
        public int RobotID = 0;
        public string RobotStatus = "";             //Robot Status(Run(R), Idle(I), Assist Wait(A), Down(D) or Trouble)

        //Constructor
        public clsRobot(int intRobotID)
        {
            this.RobotID = intRobotID;
        }
    }
}
