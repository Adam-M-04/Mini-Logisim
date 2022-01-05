using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Logic_gate_simulator
{
    public class Logical_gate : Gate
    {
        int inputs_number;
        bool[] input_values;
        public Connection_point[] input_points;
        public Connection_point output_point;
        List<Connection_point> custom_gate_inputs;
        Connection_point custom_gate_output;
        Func<bool[], bool> calculate_output;

        public Logical_gate(string text, int inputs_number, Func<bool[], bool> calculate_output, Point location, Color color, List<Connection_point> custom_gate_inputs = null, Connection_point custom_gate_output = null): base(text, location)
        {
            this.calculate_output = calculate_output;
            this.inputs_number = inputs_number;
            this.custom_gate_inputs = custom_gate_inputs;
            this.custom_gate_output = custom_gate_output;

            input_values = new bool[inputs_number];

            // Adding and styling inputs of the gate
            input_points = new Connection_point[inputs_number];
            for (int i = 0; i < input_points.Length; ++i)
            {
                input_points[i] = new Connection_point(0, i * 20 + 10, point_type.Input, this);
                container.Controls.Add(input_points[i].point);
            }

            output_point = new Connection_point(80, inputs_number * 10, point_type.Output, this);
            container.Controls.Add(output_point.point);

            // label styles
            label_gate.Height = inputs_number * 20 + 10;
            label_gate.Width = 80;
            label_gate.BackColor = color;
            label_gate.Left = 5;

            // Container styles
            container.Height = label_gate.Height;
            container.Width = label_gate.Width + 10;

            label_gate.MouseMove += new MouseEventHandler(update_lines);
            label_gate.MouseUp += new MouseEventHandler(update_lines);

            menu_strip.Items[0].Click += new EventHandler((sender, e) => { remove(); });

            board.Controls.Add(container);
            container.Controls.Add(label_gate);

            if (Gates_manager.Is_overlapping(container))
            {
                remove();
                return;
            }
        }

        void update_lines(object sender, MouseEventArgs e)
        {
            foreach (Connection_point p in input_points) p.update_connection();
            output_point.update_connection();
        }

        public void update_value()
        {
            for (int i = 0; i < input_points.Length; ++i) input_values[i] = input_points[i].value;
            bool new_val;
            if(calculate_output != null)
            {
                new_val = calculate_output(input_values);
            }
            else
            {
                for (int i = 0; i < custom_gate_inputs.Count; ++i) custom_gate_inputs[i].update_value(input_values[i]);
                new_val = custom_gate_output.value;
            }
            output_point.update_value(new_val);
        }

        public bool check_for_connection(Logical_gate lg)
        {
            if (this == lg) return true;
            return output_point.check_for_connection(lg);
        }

        public void remove()
        {
            Gates_manager.gates.Remove(this);
            foreach (Connection_point cp in input_points) cp.remove_connections();
            output_point.update_value(false);
            output_point.remove_connections();
            board.Controls.Remove(container);
        }

        public void search_for_start_points(List<Connection_point> points)
        {
            foreach (Connection_point p in input_points) p.search_for_start_points(points);
        }

        public void show_gate_tree()
        {
            Gates_manager.board.Controls.Add(container);
            foreach (Connection_point cp in input_points)
            {
                if (cp.connection.Count > 0) cp.connection[0].show_gate_tree();
            }
        }
    }
}
