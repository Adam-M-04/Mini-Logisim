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
        public static List<Logical_gate> gates = new List<Logical_gate>();
        public Form1()
        {
            InitializeComponent();
            form = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gates.Add(new Logical_gate("And", board, 2, (values) => { return values[0] && values[1]; }));
            gates.Add(new Logical_gate("Not", board, 1, (values) => { return !values[0]; }));

            Input_gate ig = new Input_gate(board);
            Output_gate og = new Output_gate(board);
        }

    }
}
