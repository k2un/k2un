using System;
using System.Collections.Generic;
//using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simulator.Forms
{
    public partial class PpidManager : Form
    {
        List<Info.ProcessProgram> mPpidType0 = new List<Info.ProcessProgram>();
        List<Info.ProcessProgram> mPpidType1 = new List<Info.ProcessProgram>();
        List<Info.ProcessProgram> mPpidType2 = new List<Info.ProcessProgram>();

        string mRootFolder = "";
        string mType0Folder = "";
        string mType1Folder = "";
        string mType2Folder = "";

#if false
        // 이벤트를 정의할때는 sender 형태가 무엇인자 언더바 접미사를 붙여준다. 왜냐하면 델리게이트를 새로 정의하기가 귀찮기 때문이다 ㅋ
        // 새로 만들어볼까 ㅋ
        public event EventHandler ppidCreated_ProcessProgram = delegate(object sender, EventArgs e) { };
        public event EventHandler ppidModified_ProcessProgram = delegate(object sender, EventArgs e) { };
        public event EventHandler ppidDeleted_ProcessProgram = delegate(object sender, EventArgs e) { };
#else
        public event Info.EventHandlerProcessProgram recipeStateChanged = delegate(Info.ProcessProgram recipe, Info.Action action) { };
#endif

        string mSuffix = ".ppid";

        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter mBinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        public PpidManager()
        {
            InitializeComponent();
            this.subFolderInit();
            this.subLoading();
        }

        void subLoading()
        {
            string[] dFiles = System.IO.Directory.GetFiles(this.mType1Folder);
            System.IO.Stream dStream;
            Info.ProcessProgram dProcessProgram;
            foreach (string item in dFiles)
            {
                if (item.Split('.')[1] != "ppid")
                {
                    continue;
                }
                dStream = System.IO.File.Open(item, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                dProcessProgram = (Info.ProcessProgram)this.mBinaryFormatter.Deserialize(dStream);
                dStream.Close();
                this.mPpidType1.Add(dProcessProgram);
                this.listBoxType1.Items.Add(dProcessProgram.ID);
            }

            dFiles = System.IO.Directory.GetFiles(this.mType2Folder);
            foreach (string item in dFiles)
            {
                dStream = System.IO.File.Open(item, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                dProcessProgram = (Info.ProcessProgram)this.mBinaryFormatter.Deserialize(dStream);
                dStream.Close();
                this.mPpidType2.Add(dProcessProgram);
                this.listBoxType2.Items.Add(dProcessProgram.ID);
            }
        }

        void subFolderInit()
        {
            this.mRootFolder = System.Windows.Forms.Application.StartupPath + "\\simulator\\ppid";
            if (!System.IO.Directory.Exists(this.mRootFolder))
            {
                System.IO.Directory.CreateDirectory(this.mRootFolder);
            }
            this.mType0Folder = this.mRootFolder + "\\type0";
            if (!System.IO.Directory.Exists(this.mType0Folder))
            {
                System.IO.Directory.CreateDirectory(this.mType0Folder);
            }
            this.mType1Folder = this.mRootFolder + "\\type1";
            if (!System.IO.Directory.Exists(this.mType1Folder))
            {
                System.IO.Directory.CreateDirectory(this.mType1Folder);
            }
            this.mType2Folder = this.mRootFolder + "\\type2";
            if (!System.IO.Directory.Exists(this.mType2Folder))
            {
                System.IO.Directory.CreateDirectory(this.mType2Folder);
            }
        }

        void subCreatePPID(Info.ProcessProgram ppid)
        {
            recipeStateChanged(ppid, Info.Action.CREATE);
        }

        void subDeletePPID(Info.ProcessProgram ppid)
        {
            recipeStateChanged(ppid, Info.Action.DELETE);
        }

        void subModifyPPID(Info.ProcessProgram ppid)
        {
            if (ppid.TYPE == Info.PPIDType.TYPE_1)
            {
                recipeStateChanged(ppid, Info.Action.MODIFY);
            }
            else
            {
                
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            System.IO.Stream dFile = null;
            Info.ProcessProgram dRecipe = null;
            switch (this.tabControl1.SelectedIndex)
            {
                case 0:
                    return;
                    break;
                case 1:
                    Forms.PpidType1 type1Ppid = new PpidType1(Info.Action.CREATE);
                    DialogResult res = type1Ppid.ShowDialog();
                        
                    if (res != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                    foreach (Info.ProcessProgram item in this.mPpidType1)
                    {
                        if (item.ID == type1Ppid.pRecipe.ID)
                        {
                            System.Windows.Forms.MessageBox.Show("아이디는 유일해야 합니다");
                            return;
                        }
                    }
                    dRecipe = type1Ppid.pRecipe;
                    dFile = System.IO.File.Create(this.mType1Folder + "\\" + type1Ppid.pRecipe.ID + this.mSuffix);

                    break;
                case 2:
                    List<string> recipes = new List<string>();
                    foreach (Info.ProcessProgram item in mPpidType1)
                    {
                        recipes.Add(item.ID);
                    }

                    Forms.PpidType2 type2Ppid = new PpidType2(recipes, Info.Action.CREATE);
                    DialogResult res2 = type2Ppid.ShowDialog();
                    if (res2 != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                    foreach (Info.ProcessProgram item in this.mPpidType2)
                    {
                        if (item.ID == type2Ppid.pRecipe.ID)
                        {
                            System.Windows.Forms.MessageBox.Show("아이디는 유일해야 합니다");
                            return;
                        }
                    }
                    dRecipe = type2Ppid.pRecipe;
                    dFile = System.IO.File.Create(this.mType2Folder + "\\" + type2Ppid.pRecipe.ID + this.mSuffix);
                    break;

            }
            if (dFile == null || dRecipe == null)
            {
                if (dFile != null) dFile.Close();
                return;
            }
            this.mBinaryFormatter.Serialize(dFile, dRecipe);
            dFile.Close();
            if (dRecipe.TYPE == Info.PPIDType.TYPE_1)
            {
                this.mPpidType1.Add(dRecipe);
                this.listBoxType1.Items.Add(dRecipe.ID);
                this.listBoxType1.SelectedIndex = this.listBoxType1.Items.Count - 1;
            }
            else if (dRecipe.TYPE == Info.PPIDType.TYPE_2)
            {
                this.mPpidType2.Add(dRecipe);
                this.listBoxType2.Items.Add(dRecipe.ID);
                this.listBoxType2.SelectedIndex = this.listBoxType2.Items.Count - 1;
                foreach (Info.ProcessProgram item in this.mPpidType1)
                {
                    if (item.ID == dRecipe.processCommands[0].CCODE[0].P_PARM)
                    {
                        item.subMapping(dRecipe);
                    }
                }
            }
            
            this.subCreatePPID(dRecipe);
            
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            System.IO.Stream dFile = null;
            Info.ProcessProgram dRecipe = null;
            Info.ProcessProgram dRecipeOld = null;
            switch (this.tabControl1.SelectedIndex)
            {
                case 0:
                    return;
                    break;
                case 1:
                    if (this.listBoxType1.SelectedIndex == -1)
                    {
                        return;
                    }
                    dRecipeOld = this.mPpidType1[this.listBoxType1.SelectedIndex];
                    
                    Forms.PpidType1 type1Ppid = new PpidType1(Info.Action.MODIFY);
                    type1Ppid.pRecipe = dRecipeOld;
                    DialogResult res = type1Ppid.ShowDialog();

                    if (res != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                    dRecipe = type1Ppid.pRecipe;
                    
                    dFile = System.IO.File.Create(this.mType1Folder + "\\" + type1Ppid.pRecipe.ID + this.mSuffix);

                    break;
                case 2:
                    List<string> recipes = new List<string>();
                    foreach (Info.ProcessProgram item in mPpidType1)
                    {
                        recipes.Add(item.ID);
                    }

                    Forms.PpidType2 type2Ppid = new PpidType2(recipes, Info.Action.MODIFY);
                    DialogResult res2 = type2Ppid.ShowDialog();
                    if (res2 != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                    dRecipe = type2Ppid.pRecipe;
                    dFile = System.IO.File.Create(this.mType2Folder + "\\" + type2Ppid.pRecipe.ID + this.mSuffix);
                    break;

            }
            if (dFile == null || dRecipe == null)
            {
                if (dFile != null) dFile.Close();
                return;
            }
            this.mBinaryFormatter.Serialize(dFile, dRecipe);
            dFile.Close();
            if (dRecipe.TYPE == Info.PPIDType.TYPE_1)
            {
                foreach (Info.ProcessProgram item in dRecipeOld.funMappingList())
                {
                    dRecipe.subMapping(item);
                }
                this.mPpidType1.Remove(dRecipeOld);
                this.listBoxType1.Items.Remove(dRecipeOld.ID);
                this.mPpidType1.Add(dRecipe);
                this.listBoxType1.Items.Add(dRecipe.ID);
            }
            else if (dRecipe.TYPE == Info.PPIDType.TYPE_2)
            {
                this.mPpidType2.Add(dRecipe);
                foreach (Info.ProcessProgram item in this.mPpidType1)
                {
                    if (item.ID == dRecipe.processCommands[0].CCODE[0].P_PARM)
                    {
                        item.subMapping(dRecipe);
                    }
                }
            }

            //this.subCreatePPID(dRecipe);
            this.subModifyPPID(dRecipe);
            if (dRecipe.TYPE == Info.PPIDType.TYPE_1)
            {
                foreach (Info.ProcessProgram item in dRecipe.funMappingList())
                {
                    this.subModifyPPID(item);        
                }
            }

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            System.IO.Stream dFile = null;
            Info.ProcessProgram dRecipe = null;
            //Info.ProcessProgram dRecipeOld = null;
            switch (this.tabControl1.SelectedIndex)
            {
                case 0:
                    return;
                    break;
                case 1:
                    int dintIndex = this.listBoxType1.SelectedIndex;
                    if (dintIndex == -1)
                    {
                        return;
                    }
                    dRecipe = this.mPpidType1[dintIndex];

                    DialogResult res = System.Windows.Forms.MessageBox.Show(string.Format("EQP PPID : {0} 을(를) 정말로 삭제 하시겠습니까?", dRecipe.ID),
                        "경고", MessageBoxButtons.OKCancel);

                    if (res != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                    System.IO.File.Delete(this.mType1Folder + "\\" + dRecipe.ID + this.mSuffix);
                    this.listBoxType1.Items.RemoveAt(dintIndex);
                    this.mPpidType1.RemoveAt(dintIndex);
                    if (dintIndex - 1 >= 0)
                    {
                        this.listBoxType1.SelectedIndex = dintIndex - 1;
                    }
                    break;
                case 2:
                    int dintIndex2 = this.listBoxType2.SelectedIndex;
                    if (dintIndex2 == -1)
                    {
                        return;
                    }
                    dRecipe = this.mPpidType2[dintIndex2];

                    DialogResult res2 = System.Windows.Forms.MessageBox.Show(string.Format("HOST PPID : {0} 을(를) 정말로 삭제 하시겠습니까?", dRecipe.ID),
                        "경고", MessageBoxButtons.OKCancel);

                    if (res2 != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                    System.IO.File.Delete(this.mType2Folder + "\\" + dRecipe.ID + this.mSuffix);
                    this.listBoxType2.Items.RemoveAt(dintIndex2);
                    this.mPpidType2.RemoveAt(dintIndex2);
                    if (dintIndex2 - 1 >= 0)
                    {
                        this.listBoxType2.SelectedIndex = dintIndex2 - 1;
                    }
                    break;

            }
            
            if (dRecipe.TYPE == Info.PPIDType.TYPE_1)
            {
                //todo 매핑 정보도 지워야한다.
            }
            else if (dRecipe.TYPE == Info.PPIDType.TYPE_2)
            {
                //todo 링크를 끊어야함
            }

            this.subDeletePPID(dRecipe);
            
        }

        private void listBoxType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxType2.SelectedIndex == -1)
            {
                return;
            }
            Info.ProcessProgram dProcessProgram = this.mPpidType2[this.listBoxType2.SelectedIndex];
            this.dataGridViewType2.Rows.Clear();
            foreach (Info.Parameter item in dProcessProgram.processCommands[0].CCODE)
            {
                this.dataGridViewType2.Rows.Add(item.P_PARM_NAME, item.P_PARM);
            }
        }

        private void listBoxType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxType1.SelectedIndex == -1)
            {
                return;
            }
            Info.ProcessProgram dProcessProgram = this.mPpidType1[this.listBoxType1.SelectedIndex];
            this.dataGridViewType1.Rows.Clear();
            foreach (Info.Parameter item in dProcessProgram.processCommands[0].CCODE)
            {
                this.dataGridViewType1.Rows.Add(item.P_PARM_NAME, item.P_PARM);
            }
        }
    }
}
