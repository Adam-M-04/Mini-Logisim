using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Symulator_ukladow_logicznych
{
    public class Logical_gate
    {
        public Label label_gate = new Label();
        Panel board, panel_gate = new Panel();
        ContextMenuStrip menu_strip;

        int inputs_number;
        bool[] input_values;
        public Connection_point[] input_points;
        public Connection_point output_point;
        Gate_dragging draggable;
        Func<bool[], bool> calculate_output;

        public Logical_gate(string text, Panel p, int inputs_number, Func<bool[], bool> calculate_output)
        {
            board = p;
            this.calculate_output = calculate_output;
            this.inputs_number = inputs_number;

            input_values = new bool[inputs_number];

            // Adding and styling inputs of the gate
            input_points = new Connection_point[inputs_number];
            for (int i = 0; i < input_points.Length; ++i)
            {
                input_points[i] = new Connection_point(0, i * 20 + 10, point_type.Input, this);
                panel_gate.Controls.Add(input_points[i].point);
            }

            output_point = new Connection_point(80, inputs_number * 10, point_type.Output, this);
            panel_gate.Controls.Add(output_point.point);


            board.Controls.Add(panel_gate);
            panel_gate.Controls.Add(label_gate);


            // label styles
            label_gate.Text = text;
            label_gate.Height = inputs_number * 20 + 10;
            label_gate.Width = 80;
            label_gate.BackColor = Color.Orange;
            label_gate.TextAlign = ContentAlignment.MiddleCenter;
            label_gate.BorderStyle = BorderStyle.FixedSingle;
            label_gate.Left = 5;
            label_gate.Top = 0;

            // Panel styles
            panel_gate.Height = label_gate.Height;
            panel_gate.Width = label_gate.Width + 10;
            panel_gate.Tag = "gate";

            draggable = new Gate_dragging(label_gate, panel_gate, board);
            label_gate.MouseMove += new MouseEventHandler(update_lines);
            label_gate.MouseUp += new MouseEventHandler(update_lines);

            menu_strip = new ContextMenuStrip();
            menu_strip.Items.Add("Delete");
            menu_strip.Items[0].Click += new EventHandler((sender, e) => { remove(); });

            panel_gate.ContextMenuStrip = menu_strip;
        }

        void update_lines(object sender, MouseEventArgs e)
        {
            foreach (Connection_point p in input_points) p.update_connection();
            output_point.update_connection();
        }

        public void update_value()
        {
            for (int i = 0; i < input_points.Length; ++i) input_values[i] = input_points[i].value;
            output_point.update_value(calculate_output(input_values));
        }

        public bool check_for_connection(Logical_gate lg)
        {
            if (this == lg) return true;
            return output_point.check_for_connection(lg);
        }

        public void remove()
        {
            Form1.gates.Remove(this);
            foreach (Connection_point cp in input_points) cp.remove_connections();
            output_point.remove_connections();
            board.Controls.Remove(panel_gate);
        }
    }
}
