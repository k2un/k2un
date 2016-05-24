using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulator;

namespace Simulator
{
    public class Simulator
    {
        Forms.MainView mainView = new Forms.MainView();
        Info.Unit mainUnit = new Info.Unit("A2CLN51B_CLNU");
        EQPAct.clsEQPAct eqpAct;
        Queue<Info.RecipeEventArgs> mRecipeQueue = new Queue<Info.RecipeEventArgs>();
        System.Threading.Thread mRecipeEventThread;// = new System.Threading.Thread(new System.Threading.ThreadStart(subRecipeEventThread));
        object mRecipeLock = new object();
        public Simulator(EQPAct.clsEQPAct eqpAct)
        {
            this.eqpAct = eqpAct;
            this.mRecipeEventThread = new System.Threading.Thread(new System.Threading.ThreadStart(subRecipeEventThread));
            this.mRecipeEventThread.Name = "RecipeEventThread";
            this.mRecipeEventThread.Start();

            this.mainUnit.processStateChanged += new EventHandler(mainUnit_processStateChanged);
            this.mainView.recipeStateChanged += new Info.EventHandlerProcessProgram(mainView_recipeStateChanged);

            Info.Unit LD01 = new Info.Unit("A2CLN51B_CLNU_LD01");
            Info.Unit UP01 = new Info.Unit("A2CLN51B_CLNU_UP01");
            Info.Unit UP02 = new Info.Unit("A2CLN51B_CLNU_UP02");
            Info.Unit UP03 = new Info.Unit("A2CLN51B_CLNU_UP03");
            Info.Unit UP04 = new Info.Unit("A2CLN51B_CLNU_UP04");
            Info.Unit UP05 = new Info.Unit("A2CLN51B_CLNU_UP05");
            Info.Unit IN01 = new Info.Unit("A2CLN51B_CLNU_IN01");
            Info.Unit BB01 = new Info.Unit("A2CLN51B_CLNU_BB01");
            Info.Unit HF01 = new Info.Unit("A2CLN51B_CLNU_HF01");
            Info.Unit HF02 = new Info.Unit("A2CLN51B_CLNU_HF02");
            Info.Unit BB02 = new Info.Unit("A2CLN51B_CLNU_BB02");
            Info.Unit OZ01 = new Info.Unit("A2CLN51B_CLNU_OZ01");
            Info.Unit RN01 = new Info.Unit("A2CLN51B_CLNU_RN01");
            Info.Unit AK01 = new Info.Unit("A2CLN51B_CLNU_AK01");
            Info.Unit UD01 = new Info.Unit("A2CLN51B_CLNU_UD01");

            LD01.processStateChanged += new EventHandler(LD01_processStateChanged);
            UP01.processStateChanged += new EventHandler(UP01_processStateChanged);
            UP02.processStateChanged +=new EventHandler(UP02_processStateChanged);
            UP03.processStateChanged +=new EventHandler(UP03_processStateChanged);
            UP04.processStateChanged +=new EventHandler(UP04_processStateChanged);
            UP05.processStateChanged +=new EventHandler(UP05_processStateChanged);
            IN01.processStateChanged +=new EventHandler(IN01_processStateChanged);
            BB01.processStateChanged +=new EventHandler(BB01_processStateChanged);
            HF01.processStateChanged +=new EventHandler(HF01_processStateChanged);
            HF02.processStateChanged +=new EventHandler(HF02_processStateChanged);
            BB02.processStateChanged +=new EventHandler(BB02_processStateChanged);
            OZ01.processStateChanged +=new EventHandler(OZ01_processStateChanged);
            RN01.processStateChanged +=new EventHandler(RN01_processStateChanged);
            AK01.processStateChanged +=new EventHandler(AK01_processStateChanged);
            UD01.processStateChanged += new EventHandler(UD01_processStateChanged);

            this.mainUnit.subUnit.Add(LD01);
            this.mainUnit.subUnit.Add(UP01);
            this.mainUnit.subUnit.Add(UP02);
            this.mainUnit.subUnit.Add(UP03);
            this.mainUnit.subUnit.Add(UP04);
            this.mainUnit.subUnit.Add(UP05);
            this.mainUnit.subUnit.Add(IN01);
            this.mainUnit.subUnit.Add(BB01);
            this.mainUnit.subUnit.Add(HF01);
            this.mainUnit.subUnit.Add(HF02);
            this.mainUnit.subUnit.Add(BB02);
            this.mainUnit.subUnit.Add(OZ01);
            this.mainUnit.subUnit.Add(RN01);
            this.mainUnit.subUnit.Add(AK01);
            this.mainUnit.subUnit.Add(UD01);

            this.eqpAct.bitChangedCIM += new EventHandler(eqpAct_bitChangedCIM);
            mainView.Show();
        }

        void subRecipeEventThread()
        {
            while (true)
            {
                //System.Diagnostics.Debug.WriteLine(string.Format("체인지 이벤트 쓰레드 : {0}", System.Threading.Thread.CurrentThread.GetHashCode()));
                //System.Diagnostics.Debug.WriteLine(string.Format("체인지 이벤트 쓰레드 : {0}", System.Threading.Thread.CurrentThread.Name));

                if (this.mRecipeQueue.Count == 0)
                {
                    //this.mRecipeEventThread.Join();
                    System.Threading.Thread.Sleep(100);
                    continue; //return;
                }

                Info.RecipeEventArgs dRecipeEventArgs;
                lock (this.mRecipeLock)
                {
                    dRecipeEventArgs = this.mRecipeQueue.Dequeue();
                }

                if (dRecipeEventArgs == null)
                {
                    System.Threading.Thread.Sleep(100);
                    continue; //return;
                }

                switch (dRecipeEventArgs.pAction)
                {
                    case Info.Action.CREATE:
                        this.recipeCreated(dRecipeEventArgs.pProcessProgram);
                        break;
                    case Info.Action.DELETE:
                        this.recipeDeleted(dRecipeEventArgs.pProcessProgram);
                        break;
                    case Info.Action.MODIFY:
                        this.recipeModified(dRecipeEventArgs.pProcessProgram);
                        break;
                }
                this.subWaittingRecipeEvent();  // 이안에 슬립있음
            }
        }

        void mainView_recipeStateChanged(Info.ProcessProgram recipe, Info.Action action)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("체인지 이벤트 함수 : {0}", System.Threading.Thread.CurrentThread.GetHashCode()));
            System.Diagnostics.Debug.WriteLine(string.Format("체인지 이벤트 함수 : {0}", System.Threading.Thread.CurrentThread.Name));
            System.Diagnostics.Debug.WriteLine(string.Format("mRecipeEventThread 상태 : {0}", this.mRecipeEventThread.ThreadState.ToString()));

            lock (this.mRecipeLock)
            {
                this.mRecipeQueue.Enqueue(new Info.RecipeEventArgs(recipe, action));
            }
        }

        void subWaittingRecipeEvent()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(100);
                if(this.eqpAct.funBitRead("B1539", 4) == "0000" && this.eqpAct.funBitRead("B1039", 4) == "0000")
                {
                    break;
                }
                
            }
            System.Diagnostics.Debug.WriteLine(string.Format("체인지 이벤트 쓰레드 : {0}", System.Threading.Thread.CurrentThread.GetHashCode()));
        }

        void recipeCreated(Info.ProcessProgram recipe)
        {
            string dstrHostPpid = "                    ";
            string dstrEqpPpid = "0";
            if (recipe.TYPE == Info.PPIDType.TYPE_1)
            {
                dstrEqpPpid = recipe.ID;
            }
            else if (recipe.TYPE == Info.PPIDType.TYPE_2)
            {
                dstrHostPpid = recipe.ID.PadRight(20, ' ');
                dstrEqpPpid = recipe.processCommands[0].CCODE[0].P_PARM;
            }
            string dstrType = ((int)recipe.TYPE).ToString();
            
            this.eqpAct.funWordWrite("W26C0", dstrHostPpid, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
            this.eqpAct.funWordWrite("W26CA", dstrEqpPpid, CommonAct.EnuEQP.PLCRWType.Int_Data);
            this.eqpAct.funWordWrite("W26CB", dstrType, CommonAct.EnuEQP.PLCRWType.Int_Data);

            int dintIndex = 0;
            if (recipe.TYPE != Info.PPIDType.TYPE_2)
            {
                foreach (Info.Parameter item in recipe.processCommands[0].CCODE)
                {
                    this.eqpAct.funWordWrite(CommonAct.FunTypeConversion.funAddressAdd("W26CC", dintIndex++), CommonAct.FunStringH.funMakePLCData(item.P_PARM), CommonAct.EnuEQP.PLCRWType.Int_Data);
                }
            }

            this.eqpAct.funBitWrite("B153A", "1");  // 피피아이디 생성 보고
        }
        void recipeModified(Info.ProcessProgram recipe)
        {
            string dstrHostPpid = "                    ";
            string dstrEqpPpid = "0";
            if (recipe.TYPE == Info.PPIDType.TYPE_1)
            {
                dstrEqpPpid = recipe.ID;
            }
            else if (recipe.TYPE == Info.PPIDType.TYPE_2)
            {
                dstrHostPpid = recipe.ID.PadRight(20, ' ');
                dstrEqpPpid = recipe.processCommands[0].CCODE[0].P_PARM;
            }
            string dstrType = ((int)recipe.TYPE).ToString();

            this.eqpAct.funWordWrite("W26C0", dstrHostPpid, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
            this.eqpAct.funWordWrite("W26CA", dstrEqpPpid, CommonAct.EnuEQP.PLCRWType.Int_Data);
            this.eqpAct.funWordWrite("W26CB", dstrType, CommonAct.EnuEQP.PLCRWType.Int_Data);

            int dintIndex = 0;
            if (recipe.TYPE != Info.PPIDType.TYPE_2)
            {
                foreach (Info.Parameter item in recipe.processCommands[0].CCODE)
                {
                    this.eqpAct.funWordWrite(CommonAct.FunTypeConversion.funAddressAdd("W26CC", dintIndex++), CommonAct.FunStringH.funMakePLCData(item.P_PARM), CommonAct.EnuEQP.PLCRWType.Int_Data);
                }
            }

            if (recipe.TYPE == Info.PPIDType.TYPE_1)
            {
                this.eqpAct.funBitWrite("B153B", "1");  // 피피아이디 수정 보고
            }
            else if (recipe.TYPE == Info.PPIDType.TYPE_2)
            {
                this.eqpAct.funBitWrite("B153C", "1");  // 피피아이디 수정 보고
            }
            
        }
        void recipeDeleted(Info.ProcessProgram recipe)
        {
            string dstrHostPpid = "                    ";
            string dstrEqpPpid = "0";
            if (recipe.TYPE == Info.PPIDType.TYPE_1)
            {
                dstrEqpPpid = recipe.ID;
            }
            else if (recipe.TYPE == Info.PPIDType.TYPE_2)
            {
                dstrHostPpid = recipe.ID.PadRight(20, ' ');
                dstrEqpPpid = recipe.processCommands[0].CCODE[0].P_PARM;
            }
            string dstrType = ((int)recipe.TYPE).ToString();

            this.eqpAct.funWordWrite("W26C0", dstrHostPpid, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
            this.eqpAct.funWordWrite("W26CA", dstrEqpPpid, CommonAct.EnuEQP.PLCRWType.Int_Data);
            this.eqpAct.funWordWrite("W26CB", dstrType, CommonAct.EnuEQP.PLCRWType.Int_Data);

            int dintIndex = 0;
            if (recipe.TYPE != Info.PPIDType.TYPE_2)
            {
                foreach (Info.Parameter item in recipe.processCommands[0].CCODE)
                {
                    this.eqpAct.funWordWrite(CommonAct.FunTypeConversion.funAddressAdd("W26CC", dintIndex++), CommonAct.FunStringH.funMakePLCData(item.P_PARM), CommonAct.EnuEQP.PLCRWType.Int_Data);
                }
            }

            this.eqpAct.funBitWrite("B1539", "1");  // 피피아이디 삭제 보고
        }

        void UD01_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B1723", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void AK01_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B1713", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void RN01_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B1703", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void OZ01_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B16F3", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void BB02_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B16E3", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void HF02_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B16D3", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void HF01_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B16C3", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void BB01_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B16B3", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void IN01_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B16A3", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void UP05_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B1693", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void UP04_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B1683", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void UP03_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B1673", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void UP02_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B1663", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void UP01_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B1653", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void LD01_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B1643", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        void mainUnit_processStateChanged(object sender, EventArgs e)
        {
            Info.Unit unit = (Info.Unit)sender;
            this.eqpAct.funBitWrite("B1603", Info.Constant.ProcessStateToString(unit.PROCESSSTATE));
        }

        

        void eqpAct_bitChangedCIM(object sender, EventArgs e)
        {
            string Address = (string)sender;
            
            switch (Address)
            {
                case "B1020":   // root
                    this.eqpAct.funBitWrite("B1520", "1");
                    this.mainUnit.PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1021":
                    this.eqpAct.funBitWrite("B1521", "1");
                    this.mainUnit.PROCESSSTATE = this.mainUnit.PROCESSSTATEOLD;
                    break;
                case "B1040":   // sub unit 0
                    this.eqpAct.funBitWrite("B1540", "1");
                    this.mainUnit.subUnit[0].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1070":
                    this.eqpAct.funBitWrite("B1570", "1");
                    this.mainUnit.subUnit[0].PROCESSSTATE = this.mainUnit.subUnit[0].PROCESSSTATEOLD;
                    break;
                case "B1041":   // subunit 1
                    this.eqpAct.funBitWrite("B1541", "1");
                    this.mainUnit.subUnit[1].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1071":
                    this.eqpAct.funBitWrite("B1571", "1");
                    this.mainUnit.subUnit[1].PROCESSSTATE = this.mainUnit.subUnit[1].PROCESSSTATEOLD;
                    break;
                case "B1042":   // subUnit 2
                    this.eqpAct.funBitWrite("B1542", "1");
                    this.mainUnit.subUnit[2].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1072":
                    this.eqpAct.funBitWrite("B1572", "1");
                    this.mainUnit.subUnit[2].PROCESSSTATE = this.mainUnit.subUnit[2].PROCESSSTATEOLD;
                    break;
                case "B1043":   // sub Unit 3
                    this.eqpAct.funBitWrite("B1543", "1");
                    this.mainUnit.subUnit[3].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1073":
                    this.eqpAct.funBitWrite("B1573", "1");
                    this.mainUnit.subUnit[3].PROCESSSTATE = this.mainUnit.subUnit[3].PROCESSSTATEOLD;
                    break;
                case "B1044":   // sub Unit 4
                    this.eqpAct.funBitWrite("B1544", "1");
                    this.mainUnit.subUnit[4].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1074":
                    this.eqpAct.funBitWrite("B1574", "1");
                    this.mainUnit.subUnit[4].PROCESSSTATE = this.mainUnit.subUnit[4].PROCESSSTATEOLD;
                    break;
                case "B1045":   // sub Unit 5
                    this.eqpAct.funBitWrite("B1545", "1");
                    this.mainUnit.subUnit[5].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1075":
                    this.eqpAct.funBitWrite("B1575", "1");
                    this.mainUnit.subUnit[5].PROCESSSTATE = this.mainUnit.subUnit[5].PROCESSSTATEOLD;
                    break;
                case "B1046":   // sub Unit 6
                    this.eqpAct.funBitWrite("B1546", "1");
                    this.mainUnit.subUnit[6].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1076":
                    this.eqpAct.funBitWrite("B1576", "1");
                    this.mainUnit.subUnit[6].PROCESSSTATE = this.mainUnit.subUnit[6].PROCESSSTATEOLD;
                    break;
                case "B1047":   // sub Unit 7
                    this.eqpAct.funBitWrite("B1547", "1");
                    this.mainUnit.subUnit[7].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1077":
                    this.eqpAct.funBitWrite("B1577", "1");
                    this.mainUnit.subUnit[7].PROCESSSTATE = this.mainUnit.subUnit[7].PROCESSSTATEOLD;
                    break;
                case "B1050":   // sub Unit 8
                    this.eqpAct.funBitWrite("B1550", "1");
                    this.mainUnit.subUnit[8].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1080":
                    this.eqpAct.funBitWrite("B1580", "1");
                    this.mainUnit.subUnit[8].PROCESSSTATE = this.mainUnit.subUnit[8].PROCESSSTATEOLD;
                    break;
                case "B1051":   // sub Unit 9
                    this.eqpAct.funBitWrite("B1551", "1");
                    this.mainUnit.subUnit[9].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1081":
                    this.eqpAct.funBitWrite("B1581", "1");
                    this.mainUnit.subUnit[9].PROCESSSTATE = this.mainUnit.subUnit[9].PROCESSSTATEOLD;
                    break;
                case "B1052":   // sub Unit 10
                    this.eqpAct.funBitWrite("B1552", "1");
                    this.mainUnit.subUnit[10].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1082":
                    this.eqpAct.funBitWrite("B1582", "1");
                    this.mainUnit.subUnit[10].PROCESSSTATE = this.mainUnit.subUnit[10].PROCESSSTATEOLD;
                    break;
                case "B1053":   // sub Unit 11
                    this.eqpAct.funBitWrite("B1553", "1");
                    this.mainUnit.subUnit[11].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1083":
                    this.eqpAct.funBitWrite("B1583", "1");
                    this.mainUnit.subUnit[11].PROCESSSTATE = this.mainUnit.subUnit[11].PROCESSSTATEOLD;
                    break;
                case "B1054":   // sub Unit 12
                    this.eqpAct.funBitWrite("B1554", "1");
                    this.mainUnit.subUnit[12].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1084":
                    this.eqpAct.funBitWrite("B1584", "1");
                    this.mainUnit.subUnit[12].PROCESSSTATE = this.mainUnit.subUnit[12].PROCESSSTATEOLD;
                    break;
                case "B1055":   // sub Unit 13
                    this.eqpAct.funBitWrite("B1555", "1");
                    this.mainUnit.subUnit[13].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1085":
                    this.eqpAct.funBitWrite("B1585", "1");
                    this.mainUnit.subUnit[13].PROCESSSTATE = this.mainUnit.subUnit[13].PROCESSSTATEOLD;
                    break;
                case "B1056":   // sub Unit 14
                    this.eqpAct.funBitWrite("B1556", "1");
                    this.mainUnit.subUnit[14].PROCESSSTATE = Info.ProcessState.PAUSE;
                    break;
                case "B1086":
                    this.eqpAct.funBitWrite("B1586", "1");
                    this.mainUnit.subUnit[14].PROCESSSTATE = this.mainUnit.subUnit[14].PROCESSSTATEOLD;
                    break;
                
                
                
            }
        }

        
    }
}
