using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_gate_simulator
{
    public partial class Name_selector : Form
    {
        Input_gate caller = null;
        public Name_selector()
        {
            InitializeComponent();
        }

        public void Open(Input_gate caller)
        {
            this.caller = caller;
            textBox_Name.Text = caller.text;
            ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            caller.set_name(textBox_Name.Text);
            textBox_Name.Text = "";
            Close();
        }
    }
}
