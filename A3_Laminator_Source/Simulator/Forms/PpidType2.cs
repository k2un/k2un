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
    public partial class PpidType2 : Form
    {
        public Info.ProcessProgram pRecipe = null;
        
        public PpidType2(List<string> recipes, Info.Action action)
        {
            if (recipes.Count <= 0)
            {
                throw new Exception("매핑 될 EQP PPID 리스트가 필요합니다.");
            }

            InitializeComponent();

            if (action == Info.Action.MODIFY)
            {
                this.textBoxId.Enabled = false; 
            }
            
            foreach (string item in recipes)
            {
                this.comboBoxEqpPpid.Items.Add(item);
            }
            this.comboBoxEqpPpid.SelectedIndex = 0;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBoxId.Text))
            {
                MessageBox.Show("HOST PPID 입력은 필수입니다.");
                return;
            }
            this.pRecipe = new Info.ProcessProgram(this.textBoxId.Text, Info.PPIDType.TYPE_2);
            Info.ProcessCommand dProcessCommand = new Info.ProcessCommand();
            //dProcessCommand.CCODE.Add(new Info.Parameter("SubPPID", (string)this.comboBoxEqpPpid.SelectedValue));
            dProcessCommand.CCODE.Add(new Info.Parameter("SubPPID", (string)this.comboBoxEqpPpid.SelectedItem));
            this.pRecipe.processCommands.Add(dProcessCommand);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }


    }
}
