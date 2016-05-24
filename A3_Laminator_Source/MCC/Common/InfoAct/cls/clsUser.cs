using System;
using System.Text;

namespace InfoAct
{
    public class clsUser
    {
        public string UserID = "";          //UserID(Key값)
        public int Level = 0;               //User Level
        public string PassWord = "";        //PassWord
        public string Desc = "";            //Desc
        
        //Constructor
        public clsUser(string dstrUserID)
        {
            this.UserID = dstrUserID;
        }
    }
}
