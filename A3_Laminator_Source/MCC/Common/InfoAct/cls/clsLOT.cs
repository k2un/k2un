using System;
using System.Text;

namespace InfoAct
{
    public class clsLOT : clsLOTMethod
    {
        //LOT 속성
        public int LOTIndex = 0;             //LOTIndex(1~49번)
        public string LOTID = "";            //LOTID
        public int InCount = 0;             //input glass quantity
        public int OutCount = 0;            //output (actual processed) glass quantity
        public int ScrapCount = 0;          //해당 LOT의 Scrap 수량
        public int UnScrapCount = 0;        //해당 LOT의 UnScrap 수량
        public string StartTime = "";        //첫번째 GLS가 첫번째 Unit에 Arrive할때
        public string EndTime = "";          //마지막 GLS가 마지막 Unit를 Departure할때

        //Constructor
        public clsLOT(int intLOTIndex, string strLOTID)
        {
            this.LOTIndex = intLOTIndex;
            this.LOTID = strLOTID;
        }
    }
}