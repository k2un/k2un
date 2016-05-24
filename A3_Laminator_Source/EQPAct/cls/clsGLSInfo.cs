
namespace EQPAct
{
    class clsGLSInfo
    {
        public int LOTIndex = 0;
        public int OutSlotID = 0;           //Loader, Unloader Port: Loader에서 취출한 GLS의 SlotID
        public int InSlotID = 0;            //Loader Port: 사용안함, Unloader Port: Unloader에 적재되는 SlotID
        public int DummyFlag = 0;
        public int NGFlag = 0;
        public int EndFlag = 0;
    }
}
