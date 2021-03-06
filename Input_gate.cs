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
        private const int bit_sign_index_in_menustrip = 2;
        public List<Connection_point> points = new List<Connection_point>();
        public List<Label> labels = new List<Label>();
        public Label value_label = new Label(), name_label = new Label();
        static Name_selector name_selector = new Name_selector();

        public List<bool> values = new List<bool>();
        public string text = "";
        public bool negative_bit = false;

        public Input_gate(Point location) : base("0", location)
        {
            for(int i=0; i<Gates_manager.input_gate_points_number; ++i)
            {
                Connection_point cp = new Connection_point(30, 30 + 20 * i, point_type.Output, this);
                points.Add(cp);
                container.Controls.Add(cp.point);

                values.Add(false);

                // label styles
                Label new_label = new Label();
                new_label.Text = "0";
                new_label.Height = 20;
                new_label.Width = 35;
                new_label.BackColor = Color.FromArgb(164, 36, 59);
                new_label.ForeColor = Color.White;
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
            value_label.Width = 35;
            value_label.Height = 30;
            draggable.Add_Events(value_label);

            // Name label styles
            name_label.Text = text;
            name_label.TextAlign = ContentAlignment.MiddleCenter;
            name_label.Left = 0;
            name_label.Width = 40;
            name_label.Height = 30;
            name_label.Font = new Font("Arial", 7);

            // Container styles
            container.Height = Gates_manager.input_gate_points_number * 20 + 55;
            container.Width = labels[0].Width + 5;

            name_label.Top = container.Height - 25;

            value_label.MouseMove += new MouseEventHandler(update_lines);
            value_label.MouseUp += new MouseEventHandler(update_lines);

            
            menu_strip.Items[0].Click += new EventHandler((sender, e) => { remove(); });

            menu_strip.Items.Add("Change name");
            menu_strip.Items[1].Click += new EventHandler((sender, e) => { name_selector.Open(this); });

            if (points.Count > 1)
            {
                menu_strip.ShowCheckMargin = true;
                menu_strip.Items.Add("Sign bit");
                menu_strip.Items[bit_sign_index_in_menustrip].Click += new EventHandler((sender, e) => { 
                    ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked; 
                    negative_bit = ((ToolStripMenuItem)sender).Checked;
                    calculate_value();
                });
            }

            container.Controls.Add(value_label);
            container.Controls.Add(name_label);
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
            label.BackColor = values[index] ? Color.Green : Color.FromArgb(164, 36, 59);
            calculate_value();

            points[index].update_value(values[index]);
        }
        void set_state(int index)
        {
            Label label = labels[index];
            label.Text = values[index] ? "1" : "0";
            label.BackColor = values[index] ? Color.Green : Color.FromArgb(164, 36, 59);
            points[index].update_value(values[index]);
        }

        public void set_name(string text)
        {
            this.text = text;
            name_label.Text = Form1.form.hideNamesToolStripMenuItem.Checked ? "" : text;
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
            ((ToolStripMenuItem)menu_strip.Items[bit_sign_index_in_menustrip]).Checked = negative_bit;
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
            name_label.Text = Form1.form.hideNamesToolStripMenuItem.Checked ? "" : text;
            Gates_manager.board.Controls.Add(container);
        }

        public void Get_gates_and_connections(List<Gate_values> gates_arr, List<Connection> connections_arr, int prev_index, int point_index, Connection_point point)
        {
            int curr_index = Gates_manager.Index_of_gate(this, gates_arr);
            if (curr_index == -1)
            {
                gates_arr.Add(new Gate_values(0, container.Location, this, (byte)points.Count, Convert.ToSByte(value_label.Text), negative_bit, values.Last(), text));
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
                if (values.Last()) value = -value;
            }
            else value += Convert.ToInt32(values.Last()) * (int)Math.Pow(2, values.Count - 1);
            value_label.Text = value.ToString();
        }

        public int get_point_index(Connection_point point)
        {
            return points.IndexOf(point);
        }

        public void Name_hidden(bool val)
        {
            name_label.Text = val ? "" : text;
        }
    }
}
