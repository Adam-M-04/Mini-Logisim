using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Symulator_ukladow_logicznych
{
    public partial class Form1 : Form
    {
        public static Form1 form;
        public static Create_gate gate_creator = new Create_gate();
        
        public Form1()
        {
            InitializeComponent();
            form = this;

            board.Left = 10;
            board.Top = 30;
            board.Width = form.Width - 35;
            board.Height = form.Height - 170;

            gates_selector_panel.Left = 10;
            gates_selector_panel.Width = form.Width - 35;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Gates_manager.form = this;
            Gates_manager.board = board;
            Gates_manager.gates_selector = gates_selector_panel;

            Gates_manager.Add_gate_template(Template_type.Input_gate);
            Gates_manager.Add_gate_template(Template_type.Output_gate);
            Gates_manager.Add_gate_template("And", 2, (values) => { return values[0] && values[1]; });
            Gates_manager.Add_gate_template("Not", 1, (values) => { return !values[0]; });
        }

        private void resize_handler(object sender, EventArgs e)
        {
            board.Width = form.Width - 35;
            board.Height = form.Height - 150;

            gates_selector_panel.Width = form.Width - 35;
        }
    }
}
