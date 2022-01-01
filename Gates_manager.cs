using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Symulator_ukladow_logicznych
{
    public static class Gates_manager
    {
        public static List<Gate_Template> available_gates = new List<Gate_Template>();
        public static List<Object> gates = new List<Object>();
        public static Form1 form;
        public static Panel board, gates_selector;

        public static void Add_gate_template(string name, int inputs_number, Func<bool[], bool> calc_function)
        {
            available_gates.Add(new Gate_Template(name, inputs_number, calc_function));
        }

        public static void Add_gate_template(string name, List<Connection_point> start_points, Connection_point output_point, Color color)
        {
            available_gates.Add(new Gate_Template(name, start_points, output_point, color));
        }

        public static void Add_gate_template(Template_type type)
        {
            available_gates.Add(new Gate_Template(type));
        }

        public static bool is_overlapping(Control to_check)
        {
            foreach (Control ctr in board.Controls)
            {
                if ((string)ctr.Tag != "gate") continue;
                if (to_check.Bounds.IntersectsWith(ctr.Bounds) && to_check != ctr) return true;
            }
            return false;
        }

        public static void Clear_board()
        {
            board.Controls.Clear();
            gates.Clear();
        }

    }
}
