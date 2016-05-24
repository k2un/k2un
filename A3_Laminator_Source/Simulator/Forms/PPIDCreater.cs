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
    public partial class PPIDCreater : Form
    {
        readonly string savePath = System.Windows.Forms.Application.StartupPath + "\\simulator";
        Dictionary<string, Info.ProcessProgram> ppidHash = new Dictionary<string, Info.ProcessProgram>();

        public PPIDCreater()
        {
            InitializeComponent();
            System.IO.Directory.CreateDirectory(savePath);
            this.openFileDialog1.InitialDirectory = savePath;
            this.comboBoxType.SelectedIndex = 0;

            subLoadingFiles();
        }

        /// <summary>
        /// 바이너리 파일들을 읽어 this.ppidHash를 초기화한다.
        /// </summary>
        void subLoadingFiles()
        {
            string[] dFiles = System.IO.Directory.GetFiles(this.savePath);   // GetFiles의 리턴값은 전체 경로를 포함한다
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter dBinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            foreach (string item in dFiles)
            {
                System.IO.FileStream dFileStream = System.IO.File.Open(item, System.IO.FileMode.Open);
                Info.ProcessProgram dProcessProgram = (Info.ProcessProgram)dBinaryFormatter.Deserialize(dFileStream);
                dFileStream.Close();
                this.ppidHash.Add(dProcessProgram.ID, dProcessProgram);
            }
        }

        void subSave(Info.ProcessProgram dProcessProgram)
        {
            this.ppidHash.Add(dProcessProgram.ID, dProcessProgram);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter dBinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.FileStream dFileStream = System.IO.File.Create(this.savePath + "\\" + dProcessProgram.ID + ".ppid");
            dBinaryFormatter.Serialize(dFileStream, dProcessProgram);
            dFileStream.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                Info.ProcessProgram PP = new Info.ProcessProgram(this.textBoxID.Text, (Info.PPIDType)this.comboBoxType.SelectedIndex);
                Info.ProcessCommand PC = new Info.ProcessCommand();
                for (int i = 0; i < this.dataGridViewBody.RowCount - 1; i++)
                {
                    Info.Parameter P = new Info.Parameter(this.dataGridViewBody.Rows[i].Cells[0].Value.ToString(), this.dataGridViewBody.Rows[i].Cells[1].Value.ToString());
                    PC.CCODE.Add(P);
                }
                PP.processCommands.Add(PC);
                this.subSave(PP);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = this.openFileDialog1.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.Cancel)
                    return;
                string fileName = this.openFileDialog1.SafeFileName;
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.FileStream fs = System.IO.File.Open(this.savePath + "\\" + fileName, System.IO.FileMode.Open);
                Info.ProcessProgram PP = (Info.ProcessProgram)bf.Deserialize(fs);
                fs.Close();

                this.textBoxID.Text = PP.ID;
                this.comboBoxType.SelectedIndex = (int)PP.TYPE;
                this.dataGridViewBody.Rows.Clear();
                foreach (Info.Parameter p in PP.processCommands[0].CCODE)
                {
                    this.dataGridViewBody.Rows.Add(p.P_PARM_NAME, p.P_PARM);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        
    }

    

}
