using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_gate_simulator
{
    public class Input_gate : Gate
    {
        public List<Connection_point> points = new List<Connection_point>();
        public List<Label> labels = new List<Label>();
        public Label value_label = new Label();

        public List<bool> values = new List<bool>();
        string text = null;
        public bool negative_bit = false;

        public Input_gate(Point location) : base("0", location)
        {
            for(int i=0; i<Gates_manager.input_gate_points_number; ++i)
            {
                Connection_point cp = new Connection_point(25, 30 + 20 * i, point_type.Output, this);
                points.Add(cp);
                container.Controls.Add(cp.point);

                values.Add(false);

                // label styles
                Label new_label = new Label();
                new_label.Text = "0";
                new_label.Height = 20;
                new_label.Width = 30;
                new_label.BackColor = Color.Pink;
                new_label.TextAlign = ContentAlignment.MiddleCenter;
                new_label.BorderStyle = BorderStyle.FixedSingle;
                new_label.Left = 0;
                new_label.Top = i * 20 + 25;

                new_label.DoubleClick += new EventHandler((sender, e)=> { change_state(new_label); });
                labels.Add(new_label);
                container.Controls.Add(new_label);

                draggable.Add_Events(new_label);
                new_label.MouseMove += new MouseEventHandler(update_lines);
                new_label.MouseUp += new MouseEventHandler(update_lines);
            }

            // Value label styles
            value_label.Text = "0";
            value_label.TextAlign = ContentAlignment.MiddleCenter;
            value_label.BorderStyle = BorderStyle.FixedSingle;
            value_label.Left = 0;
            value_label.Top = 0;
            value_label.Width = 30;
            value_label.Height = 30;
            draggable.Add_Events(value_label);

            // Container styles
            container.Height = Gates_manager.input_gate_points_number * 20 + 30;
            container.Width = labels[0].Width + 5;

            value_label.MouseMove += new MouseEventHandler(update_lines);
            value_label.MouseUp += new MouseEventHandler(update_lines);

            
            menu_strip.Items[0].Click += new EventHandler((sender, e) => { remove(); });
            if(points.Count > 1)
            {
                menu_strip.ShowCheckMargin = true;
                menu_strip.Items.Add("Sign bit");
                menu_strip.Items[1].Click += new EventHandler((sender, e) => { 
                    ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked; 
                    negative_bit = ((ToolStripMenuItem)sender).Checked;
                    calculate_value();
                });
            }

            container.Controls.Add(value_label);
            board.Controls.Add(container);

            if (Gates_manager.Is_overlapping(container))
            {
                remove();
                return;
            }
        }

        void update_lines(object sender, MouseEventArgs e)
        {
            foreach(Connection_point cp in points) cp.update_connection();
        }

        void change_state(Label label)
        {
            int index = labels.IndexOf(label);
            values[index] = !values[index];
            label.Text = values[index] ? "1" : "0";
            label.BackColor = values[index] ? Color.Green : Color.Pink;
            calculate_value();

            points[index].update_value(values[index]);
        }
        void set_state(int index)
        {
            Label label = labels[index];
            label.Text = values[index] ? "1" : "0";
            label.BackColor = values[index] ? Color.Green : Color.Pink;
            points[index].update_value(values[index]);
        }

        public void set_value(int val, bool negative_bit, bool negative_bit_on = false)
        {   
            if(values.Count == 1)
            {
                values[0] = Convert.ToBoolean(val % 2);
                set_state(0);
                calculate_value();
                return;
            }
            this.negative_bit = negative_bit;
            ((ToolStripMenuItem)menu_strip.Items[1]).Checked = negative_bit;
            if (val < -127 || val > 256) return;
            for(int i=0; i<points.Count - 1; ++i)
            {
                values[i] = Convert.ToBoolean(val % 2);
                set_state(i);
                val /= 2;
            }
            if (negative_bit)
            {
                if (negative_bit_on)
                {
                    values[points.Count - 1] = true;
                    set_state(points.Count - 1);
                }
            }
            else
            {
                values[points.Count - 1] = Convert.ToBoolean(val % 2);
                set_state(points.Count - 1);
            }
            calculate_value();
        }

        public void remove()
        {
            Gates_manager.gates.Remove(this);
            foreach (Connection_point cp in points) cp.update_value(false);
            foreach (Connection_point cp in points) cp.remove_connections();
            board.Controls.Remove(container);
        }

        public void show_gate_tree()
        {
            Gates_manager.board.Controls.Add(container);
        }

        public void Get_gates_and_connections(List<Gate_values> gates_arr, List<Connection> connections_arr, int prev_index, int point_index, Connection_point point)
        {
            int curr_index = Gates_manager.Index_of_gate(this, gates_arr);
            if (curr_index == -1)
            {
                gates_arr.Add(new Gate_values(0, container.Location, this, (byte)points.Count, Convert.ToSByte(value_label.Text), negative_bit, values[values.Count - 1]));
                curr_index = gates_arr.Count - 1;
            }
            connections_arr.Add(new Connection(curr_index, prev_index, point_index, points.IndexOf(point)));
        }

        void calculate_value()
        {
            int value = 0;
            for (int i = 0; i < values.Count - 1; ++i) value += Convert.ToInt32(values[i]) * (int)Math.Pow(2, i);
            if(negative_bit)
            {
                if (values[values.Count - 1]) value = -value;
            }
            else value += Convert.ToInt32(values[values.Count - 1]) * (int)Math.Pow(2, values.Count - 1);
            value_label.Text = value.ToString();
        }

        public int get_point_index(Connection_point point)
        {
            return points.IndexOf(point);
        }

        public void Name_hidden(bool val)
        {
            /*if (val) label_gate.Text = "";                                                                                DO PRZEROBIENIA
            else
            {
                if (text != null) label_gate.Text = text;
                else text = value ? "1" : "0";
            }*/
        }
    }
}
