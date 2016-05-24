using System;
using System.Text;

namespace InfoAct
{
    public class clsPort : clsPortMethod
    {
        //Port 관련 속성
        public int PortID = 0;
        public string HostReportPortID = "";      //Wet Etch & Strip의 경우 Host로 보고할 PortID
        public string PortStatus = "";            //Port State("LDRQ"/"LDCM"/"UDRQ"/"UDCM"/"DOWN")         
        public string PortEnable = "E";           //Port Enable("E": Port can be used (port usable), "D": Port cannot be used (port unusable))
        public string CSTSetCode = "";            //Carrier Setting Code
        public string PortType = "L";             //Port Type("L": Loader(Source), "U": Unloader(Target), "B": Buffer, "": Wet Etch & Strip에서 Through Mode로 사용될때)
        public string ACKC766 = "0000000";        //Confirmation of download data of S7F66
        public Boolean DummyCST = false;          //Dummy CST인지의 여부              
        public Boolean ULPortMappingNG = false;   //Unloader 포트인데 GLS가 존재하는지 여부
        public Boolean funSaveS7F66NG = false;    //funSaveS7F66에서 NG인경우 저장 (OPCall Start창 로딩시 사용)
        public Boolean S7F72Zero = false;         //S7F66이 0이고 S7F72가 0인지 여부, HOST로 S1F83(CAEN)을 보고할지 여부(True: HOST로 보고함, False: HOST로 보고하지 않음)
        public int RecipeBodyCheckConfirmCount = 0; //Port별 CIM의 Recipe Body Request에 대한 Unit들의 Confirm Bit 카운트(Recipe Body Check)
        public int RecipeBodyCheckMustCount = 0;    //Port별 Recipe Body Check를 해야 할 개수(Unit가 Offline인것은 제외)
        public string LCEQPMode = "0";            //LC 되었을때의 EQP Mode(0: Offline, 1: Online Monitor, 2: Online Control)

        //CT 관련 속성
        public long CTCheckTime = 0;            //CT Check시작 시간
        public string HostWaitSF = "";          //Host로부터 기다리게될 SF의 이름(Conversation TimeOut시 사용)
        public Boolean bolFlagSch = false;      //True: CT 체크 설정, False: CT 체크 해제
        public Boolean CSTOnState = false;      //Cassette 재고상태 여부   

        //Port, LOT 둘다 있는 속성
        public string LOTID = "";               //LOTID
        public int LOTIndex = 0;                //Port의 Down받은 LOT의 LOTIndex
        public string CSTID = "";               //Carrier ID
        public string UserID = "";              //User ID(Barcode로 읽은 User ID)
        public string DLPPID = "";              //Recipe ID that was downloaded by Host(문자임 : ex) HOST001)
        public string ACPPID = "";              //Recipe ID that is actual used in Equipmen(문자임 : ex) HOST001)
        public int InCount = 0;                 //input glass quantity
        public int OutCount = 0;                //output (actual processed) glass quantity
        public string LOTStatus = "";           //LOT Status(""= Wait되기 전상태, WAIT= Waiting, PROC= Processing, PREN= Process end, ABND= AbNormal End, EMPT= Carrier becomes Empty, ABOT= Abort End)

        public string GLSLoaderMapping10 = "";  //Loader가 최초에 읽은 Mapping Data(ex:111000111101010)
        public string GLSLoaderMappingYN = "";  //Loader가 최초에 읽은 Mapping Data(ex:YYYNNNYYYYNYNYN)
        public int GLSLoaderTotalCnt = 0;       //Loader가 최초에 읽은 GLS 개수

        public string GLSHostMapping10 = "";    //Host에서 수신한 GLS Mapping정보(ex:111000111101010)
        public string GLSHostMappingYN = "";    //Host에서 수신한 GLS Mapping정보(ex:YYYNNNYYYYNYNYN)

        public string GLSCIMMapping10 = "";     //CIM에서 Operator가 선택한 GLS Mapping정보(ex:111000111101010)
        public string GLSCIMMappingYN = "";     //CIM에서 Operator가 선택한 GLS Mapping정보(ex:YYYNNNYYYYNYNYN)

        public int GLSProcessTotalCnt = 0;      //작업할 GLS 총 갯수

        public string GLSRealMapping10 = "";    //현재 Port의 실시간 Mapping정보(ex:011000111101010)
        public string GLSProcessingInfo = "";   //Port에서 GLS 취출, 투입시 실시간으로 변경되는 정보(GLS없음:N, 작업할(안한)GLS있음:Y, 작업안할(안한)GLS있음:S, 작업중 GLS:P, 작업완료한GLS:E)

        public string RWKID = "";               //Space:Normal, Rework ID will be defined later.

        public string PRODID = "";              //Download from S7F66(Product ID)
        public string SPLTID = "";              //Download from S7F66(Split ID)

        public string ProcessStartTime = "";    //Port의 작업시작시간
        public string ProcessEndTime = "";      //Port의 작업종료시간

        //Only Wet Etch & Strip, Strip 사용 속성
        public Boolean TransferStageNextCSTExist = false;    //Transfer Stage에서 작업이 끝나고 난 후 NextCST가 들어오는지 여부(True: 들어옴)
        public Boolean LoadStageCSTExist = false;            //Load 단(수취) Stage에 CST 존재 유뮤(True: 존재함)
        public Boolean UnloadStageCSTExist = false;          //Unload 단(반출) Stage에 CST 존재 유뮤(True: 존재함)

        public string UCCSTID = "";                          //UC시보고시 사용할 CSTID
        //Only Wet Etch & Strip, Strip 사용 속성


        //Constructor
        public clsPort(int intPortID)
        {
            this.PortID = intPortID;
        }

        //*******************************************************************************
        //  Function Name : Initial()
        //  Description   : Port 정보를 초기화 한다.
        //  Parameters    : intPortID => 초기화할 PortID
        //  Return Value  : True => 성공, False => 실패
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/20          김 효주         [L 00] 
        //*******************************************************************************
        public Boolean Initial()
        {
            Boolean dbolRetuen = false;

            try
            {
                this.CSTSetCode = "";
                this.ACKC766 = String.Empty.PadLeft(7, '0');
                this.DummyCST = false;
                this.ULPortMappingNG = false;
                this.funSaveS7F66NG = false;
                this.S7F72Zero = false;
                this.RecipeBodyCheckConfirmCount = 0;
                this.RecipeBodyCheckMustCount = 0;
                this.LCEQPMode = "0";
                this.CTCheckTime = 0;
                this.HostWaitSF = "";
                this.bolFlagSch = false;
                this.CSTOnState = false;
                this.LOTID = "";
                this.LOTIndex = 0;
                this.CSTID = "";
                this.UserID = "";
                this.DLPPID = "";
                this.ACPPID = "";
                this.InCount = 0;
                this.OutCount = 0;
                this.LOTStatus = "";
                this.GLSLoaderMapping10 = String.Empty.PadLeft(this.SlotCount, '0');
                this.GLSLoaderMappingYN = String.Empty.PadLeft(this.SlotCount, 'N');
                this.GLSLoaderTotalCnt = 0;
                this.GLSHostMapping10 = String.Empty.PadLeft(this.SlotCount, '0');
                this.GLSHostMappingYN = String.Empty.PadLeft(this.SlotCount, 'N');
                this.GLSCIMMapping10 = String.Empty.PadLeft(this.SlotCount, '0');
                this.GLSCIMMappingYN = String.Empty.PadLeft(this.SlotCount, 'N');
                this.GLSProcessTotalCnt = 0;
                this.GLSRealMapping10 = String.Empty.PadLeft(this.SlotCount, '0');
                this.GLSProcessingInfo = String.Empty.PadLeft(this.SlotCount, 'N');
                this.RWKID = "";
                this.PRODID = "";
                this.SPLTID = "";
                this.ProcessStartTime = "";
                this.ProcessEndTime = "";

                for(int dintSlot = 1; dintSlot <= this.SlotCount; dintSlot++)
                {
                    this.Slot(dintSlot).SlotID = dintSlot;
                    this.Slot(dintSlot).GLSID = "";
                    this.Slot(dintSlot).GLSStartTime = "";
                    this.Slot(dintSlot).GLSEndTime = "";
                    this.Slot(dintSlot).MExist = "0";
                    this.Slot(dintSlot).DExist = "0";
                    this.Slot(dintSlot).HExist = "0";
                    this.Slot(dintSlot).DLPPID = "";
                    this.Slot(dintSlot).ACPPID = "";
                    this.Slot(dintSlot).PROCFLAG = "";
                    this.Slot(dintSlot).RWKCOUNT  = "";
                    this.Slot(dintSlot).PanelperGLS = "";
                    this.Slot(dintSlot).X_AXIS = "";
                    this.Slot(dintSlot).Y_AXIS = "";
                    this.Slot(dintSlot).PassUnitID = "";
                    this.Slot(dintSlot).HashUnitPPID.Clear();
                }

                dbolRetuen = true;
            }
            catch
            {
                //LogAct.clsLogAct.subWriteCIMLog(ex.ToString() + ", PortID:" + this.PortID);    //에러 로그 출력
            }

            return dbolRetuen;
        }
    }
}
