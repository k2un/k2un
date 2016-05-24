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
        public int SIZE1 = 0;
        public int SIZE2 = 0;
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

        public Boolean IsAPCRunning = false;
        public Boolean IsRPCRunning = false;
        public Boolean IsPPCRunning = false;

        public int FilmExistUnitID = 0;
        public string MExist = "0";             //설비에서 수신한 GLS 존재 유무(1:Exist, 0 : None)
        public string DExist = "0";             //작업 할 선택 GLS 정보(1:작업 대상, 0 : 작업할 대상이 아님)
        public string HExist = "0";             //Host에서 수신한 GLS 존재 유무(1:Exist, 0 : None)
        public string RExist = "0";             //현재 실시간 글래스 존재유무(1:Exist, 0 : None)

        public bool GlassRunFlag = false;   //작업진행여부
        public bool GlassRunCompleteFlag = false; // 작업완료 여부


        //Process Step 추가
        public int StepNo = 0;
        public int StepNo_OLD = 0;


        //Constructor
        public clsSlot(int intSlotID)
        {
            this.SlotID = intSlotID;
        }


        public delegate void GlassExistChange(int dintSlotID, string strGlassID);
        public event GlassExistChange GlassExist;
        public string GlassID
        {
            get { return H_PANELID; }
            set
            {
                if (this.H_PANELID != value)
                {
                    this.H_PANELID = value;
                    if (this.GlassExist != null)
                    {
                        this.GlassExist(SlotID, H_PANELID);
                    }
                }
            }
        }

        public void CopyFrom(clsSlot slot)
        {
            this.SlotID = slot.SlotID;
            this.H_PANELID = slot.H_PANELID;
            this.E_PANELID = slot.E_PANELID;
            this.LOTID = slot.LOTID;
            this.BATCHID = slot.BATCHID;
            this.JOBID = slot.JOBID;

            this.PORTID = slot.PORTID;
            this.SLOTNO = slot.SLOTNO;
            this.PRODUCT_TYPE = slot.PRODUCT_TYPE;
            this.PRODUCT_KIND = slot.PRODUCT_KIND;
            this.PRODUCTID = slot.PRODUCTID;
            this.RUNSPECID = slot.RUNSPECID;
            this.LAYERID = slot.LAYERID;
            this.STEPID = slot.STEPID;
            this.HOSTPPID = slot.HOSTPPID;
            this.FLOWID = slot.FLOWID;
            this.SIZE = slot.SIZE;
            this.THICKNESS = slot.THICKNESS;
            this.GLASS_STATE = slot.GLASS_STATE;
            this.GLASS_ORDER = slot.GLASS_ORDER;
            this.COMMENT = slot.COMMENT;

            this.USE_COUNT = slot.USE_COUNT;
            this.JUDGEMENT = slot.JUDGEMENT;
            this.REASON_CODE = slot.REASON_CODE;
            this.INS_FLAG = slot.INS_FLAG;
            this.ENC_FLAG = slot.ENC_FLAG;
            this.PRERUN_FLAG = slot.PRERUN_FLAG;
            this.TURN_DIR = slot.TURN_DIR;
            this.FLIP_STATE = slot.FLIP_STATE;
            this.WORK_STATE = slot.WORK_STATE;
            this.MULTI_USE = slot.MULTI_USE;

            this.PAIR_GLASSID = slot.PAIR_GLASSID;
            this.PAIR_PPID = slot.PAIR_PPID;

            for (int intLoop = 0; intLoop < this.OPTION_NAME.Length; intLoop++)
            {
                this.OPTION_NAME[intLoop] = slot.OPTION_NAME[intLoop];
                this.OPTION_VALUE[intLoop] = slot.OPTION_VALUE[intLoop];
            }

            this.JOBStart = slot.JOBStart;
            this.JOBEnd = slot.JOBEnd;

            this.Scrap = slot.Scrap;
            this.StartTime = slot.StartTime;
            this.EndTime = slot.EndTime;

            this.IsAPCRunning = slot.IsAPCRunning;
            this.IsRPCRunning = slot.IsRPCRunning;
            this.IsPPCRunning = slot.IsPPCRunning;

        }
    }
}