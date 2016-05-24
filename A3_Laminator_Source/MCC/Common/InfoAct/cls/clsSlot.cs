using System;
using System.Text;
using System.Collections;

namespace InfoAct
{
    public class clsSlot : clsSlotMethod
    {
        public int SlotID = 0;                      //SlotID

        public string H_PANELID = "";               //Host's Flat Panel ID
        public string E_PANELID = "";               //Equipment's Flat Panel ID(If EQ has not VCR, report with SPACE)
        public string LOTID = "";                   //현재 Unit에 있는 GLS의 LOTID
        public string BATCHID = "";                 //Batch ID
        public string JOBID = "";                   //JOBID

        public string PORTID = "";                  //(PORTID)
        public string SLOTNO = "";                  //(SLOTNO)
        public string PRODUCT_TYPE = "";            //(PRODUCT_TYPE (ex) S/D/C/DM))
        public string PRODUCT_KIND = "";            //(PRODUCT_KIND (Material Kind defined by SMD) ex) MASK, FRME, STCK, RTCL, etc)
        public string PRODUCTID = "";               //(Product(Device ID))
        public string RUNSPECID = "";               //(Runsheet)
        public string LAYERID = "";                 //(Processing Layer)
        public string STEPID = "";                  //(Step ID)
        public string HOSTPPID = "";                //(Recipe ID that was downloaded by Host(문자임 : ex) HOST001))
        public string FLOWID = "";                  //(Flow Recipe)
        public string SIZE = "";                    //Glass Size(mm)
        public int THICKNESS = 0;                   //Glass Thickness(㎛)
        public int GLASS_STATE = 0;                 //Glass State(3: Processing, 5: Aborting) ex) 1: Idle, 2: Selected to process, etc
        public string GLASS_ORDER = "";             //ex) nnmm : 0156, 0256, 5656, etc))
        public string COMMENT = "";                 //Comment for specific info about the glass

        public string USE_COUNT = "";               //ex) RW01, RW02, RP01, RP02, etc
        public string JUDGEMENT = "";               //ex) OK: Good, NG: Not Good, RJ: Reject, etc
        public string REASON_CODE = "";             //ex) 0010: OK, etc
        public string INS_FLAG = "";                //Optional data for glass inspection
        public string ENC_FLAG = "";                //ex) 'Y '(Encap), 'N '(Not Encap)
        public string PRERUN_FLAG = "";             //ex) 'Y '(Prerun), 'N '(Not Prerun)
        public string TURN_DIR = "";                //ex) 'Y '(Prerun), 'N '(Not Prerun),
        public string FLIP_STATE = "";              //(absolute status) 'H '(head) ↔ 'T '(tail)
        public string WORK_STATE = "";              //(Special case, Depends on EQ)
        public string MULTI_USE = "";               //For special cases of EQP management

        public string PAIR_GLASSID = "";             //Pair Host GLASSID
        public string PAIR_PPID = "";                //PPID of Pair GLASS

        public string[] OPTION_NAME = new string[5];
        public string[] OPTION_VALUE = new string[5];

        //사용자정의
        public Boolean JOBStart = false;        //LOT의 첫 GLS
        public Boolean JOBEnd = false;          //LOT의 마지막 GLS


        public Boolean Scrap = false;           //해당 Slot이 Scrap되었는지 여부(True: Scrap되었음)
        //public Boolean UnScrap = false;       //해당 Slot이 UnScrap되었는지 여부(True: UnScrap되었음)
        public string StartTime = "";           //Loader에서 취출한 GLS 시작 시간
        public string EndTime = "";             //Unloader에 투입한 GLS 종료 시간

        //Constructor
        public clsSlot(int intSlotID)
        {
            this.SlotID = intSlotID;
        }
    }
}