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
    public partial class MainView : Form
    {
        PPIDCreater pc = new PPIDCreater();
        PpidManager ppidManager = new PpidManager();

        public event Info.EventHandlerProcessProgram recipeStateChanged = delegate(Info.ProcessProgram recipe, Info.Action action) { };

        public MainView()
        {
            InitializeComponent();
            ppidManager.recipeStateChanged += new Info.EventHandlerProcessProgram(ppidManager_recipeStateChanged);
        }

        void ppidManager_recipeStateChanged(Info.ProcessProgram recipe, Info.Action action)
        {
            this.recipeStateChanged(recipe, action);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ppidManager.Show();
            this.ppidManager.BringToFront();
        }
    }
}
