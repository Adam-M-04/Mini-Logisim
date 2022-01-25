using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_gate_simulator
{
    public static class Gates_manager
    {
        public static List<Gate_Template> available_gates = new List<Gate_Template>();
        public static List<Object> gates = new List<Object>();
        public static Form1 form;
        public static Panel board, gates_selector;
        public static Gate_Template current_edited = null;

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

        public static bool Is_overlapping(Control to_check)
        {
            foreach (Control ctr in board.Controls)
            {
                if ((string)ctr.Tag != "gate") continue;
                if (to_check.Bounds.IntersectsWith(ctr.Bounds) && to_check != ctr) return true;
            }
            return false;
        }

        public static void Context_menu_options(bool val)
        {
            foreach (Gate_Template gt in available_gates) if(gt.menu_strip.Items.Count > 0) gt.menu_strip.Items[0].Enabled = val;
            Form1.menu.Items[2].Visible = !val;
            ((Output_gate)current_edited.output_point.parent).menu_strip.Items[0].Enabled = val;
            ((Output_gate)current_edited.output_point.parent).menu_strip.Items[1].Enabled = val;
        }

        public static void Gates_Enabled(bool val, Gate_Template starting_gate = null)
        {
            int i = 0;
            if(!val) while (available_gates[i] != starting_gate && i < available_gates.Count) ++i;
            while (i < available_gates.Count) available_gates[i++].Disabled(!val);
            available_gates[1].Disabled(!val);
            Context_menu_options(val);
        }

        public static void Save_edited_gate(string new_name, Color new_color)
        {
            List<Connection_point> start_points = new List<Connection_point>();
            current_edited.output_point.search_for_start_points(start_points);
            if (start_points.Count == 0)
            {
                MessageBox.Show("Add at least one input gate to create a new gate");
                return;
            }

            current_edited.set_style(new_name, new_color);
            current_edited.calculate_input_points(start_points);
            Clear_board();
            Gates_Enabled(true);
            current_edited = null;
        }

        public static void Default_templates()
        {
            Clear_board();
            for (int i= available_gates.Count-1; i>=0; --i) available_gates[i].remove();
            Add_gate_template(Template_type.Input_gate);
            Add_gate_template(Template_type.Output_gate);
            Add_gate_template("And", 2, (values) => { return values[0] && values[1]; });
            Add_gate_template("Not", 1, (values) => { return !values[0]; });
        }

        public static void Clear_board()
        {
            board.Controls.Clear();
            gates.Clear();
        }
        public static int Index_of_gate(Object gate, List<Gate_values> gates_arr)
        {
            for (int i=0; i<gates_arr.Count; ++i)
            {
                if (gates_arr[i].gate_ref == gate) return i;
            }
            return -1;
        }
    }
}
