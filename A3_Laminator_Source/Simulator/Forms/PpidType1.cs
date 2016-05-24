using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simulator.Forms
{
    public partial class PpidType1 : Form
    {
        public Info.ProcessProgram pRecipe;
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter mBinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        string mDefaultFilePath = System.Windows.Forms.Application.StartupPath + "\\simulator\\ppid\\type1\\default.pp";
        Info.Action action = Info.Action.NONE;

        public PpidType1(Info.Action action)
        {
            InitializeComponent();
            if (action == Info.Action.MODIFY)
            {
                this.textBoxId.Enabled = false;
            }
            this.action = action;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBoxId.Text))
            {
                MessageBox.Show("아이디는 필수로 입력해야 합니다.");
                return;
            }
            
            this.buttonSave.Enabled = false;
            if (this.action != Info.Action.MODIFY)
            {
                this.pRecipe = new Info.ProcessProgram(this.textBoxId.Text, Info.PPIDType.TYPE_1);
            }
            this.pRecipe.processCommands.Clear();
            
            Info.ProcessCommand processCommand = new Info.ProcessCommand();
            
            for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
            {
                Info.Parameter parameter = new Info.Parameter((string)this.dataGridView1[0, i].Value, (string)this.dataGridView1[1, i].Value);
                processCommand.CCODE.Add(parameter);
            }

            this.pRecipe.processCommands.Add(processCommand);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void buttonDefaultSave_Click(object sender, EventArgs e)
        {
            Info.ProcessProgram dRecipe = new Info.ProcessProgram("default", Info.PPIDType.TYPE_1);

            Info.ProcessCommand processCommand = new Info.ProcessCommand();

            for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
            {
                Info.Parameter parameter = new Info.Parameter((string)this.dataGridView1[0, i].Value, (string)this.dataGridView1[1, i].Value);
                processCommand.CCODE.Add(parameter);
            }
            dRecipe.processCommands.Add(processCommand);

            System.IO.Stream dStream = System.IO.File.Create(mDefaultFilePath);
            mBinaryFormatter.Serialize(dStream, dRecipe);
            dStream.Close();

            System.Windows.Forms.MessageBox.Show("저장되었습니다.");
        }

        private void buttonDefaultLoad_Click(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(mDefaultFilePath))
            {
                System.Windows.Forms.MessageBox.Show("파일이 없습니다. 새로 저장해 주세요.");
                return;
            }
            this.dataGridView1.Rows.Clear();
            System.IO.Stream dStream = System.IO.File.Open(mDefaultFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Info.ProcessProgram dRecipe = (Info.ProcessProgram)mBinaryFormatter.Deserialize(dStream);
            dStream.Close();
            foreach (Info.Parameter item in dRecipe.processCommands[0].CCODE)
            {
                this.dataGridView1.Rows.Add(item.P_PARM_NAME, item.P_PARM);
            }
            System.Windows.Forms.MessageBox.Show("기본 서식을 불러왔습니다.");
        }

        private void PpidType1_Load(object sender, EventArgs e)
        {
            if (this.action == Info.Action.MODIFY)
            {
                this.textBoxId.Text = this.pRecipe.ID;
                this.dataGridView1.Rows.Clear();
                foreach (Info.Parameter item in pRecipe.processCommands[0].CCODE)
                {
                    this.dataGridView1.Rows.Add(item.P_PARM_NAME, item.P_PARM);
                }
            }
        }
    }
}
