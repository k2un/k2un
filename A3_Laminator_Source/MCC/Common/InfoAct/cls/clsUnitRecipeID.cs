using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    public class clsUnitRecipeID
    {
        public int UnitID = 0;
        public string RecipeID = null;

        //HOST Recipe별로 Unit1 ~ Unitn까지 Recipe를 저장.
        public clsUnitRecipeID(int intUnitID, string strRecipeID)
        {
            this.UnitID = intUnitID;
            this.RecipeID = strRecipeID;
        }
    }
}
