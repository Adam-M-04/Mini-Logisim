using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_gate_simulator
{
    public class Output_gate : Gate
    {
        private const int bit_sign_index_in_menustrip = 2;
        public List<Connection_point> points = new List<Connection_point>();
        public List<Label> labels = new List<Label>();
        public Label value_label = new Label(), name_label = new Label();
        static Name_selector name_selector = new Name_selector();

        public string text = "";
        public bool negative_bit = false;

        public Output_gate(Point location): base("0", location)
        {
            for (int i = 0; i < Gates_manager.output_gate_points_number; ++i)
            {
                Connection_point cp = new Connection_point(0, 30 + 20 * i, point_type.Input, this);
                points.Add(cp);
                container.Controls.Add(cp.point);

                // label styles
                Label new_label = new Label();
                new_label.Text = "0";
                new_label.Height = 20;
                new_label.Width = 35;
                new_label.BackColor = Color.FromArgb(164, 36, 59);
                new_label.ForeColor = Color.White;
                new_label.TextAlign = ContentAlignment.MiddleCenter;
                new_label.BorderStyle = BorderStyle.FixedSingle;
                new_label.Left = 5;
                new_label.Top = i * 20 + 25;
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
            value_label.Left = 5;
            value_label.Top = 0;
            value_label.Width = 35;
            value_label.Height = 30;
            draggable.Add_Events(value_label);

            // Name label styles
            name_label.Text = text;
            name_label.TextAlign = ContentAlignment.MiddleCenter;
            name_label.Left = 5;
            name_label.Width = 35;
            name_label.Height = 30;
            name_label.Font = new Font("Arial", 7);

            // Container styles
            container.Height = Gates_manager.output_gate_points_number * 20 + 55;
            container.Width = 40;

            name_label.Top = container.Height - 25;

            value_label.MouseMove += new MouseEventHandler(update_lines);
            value_label.MouseUp += new MouseEventHandler(update_lines);

            menu_strip.Items[0].Click += new EventHandler((sender, e) => {remove();});

            menu_strip.Items.Add("Change name");
            menu_strip.Items[1].Click += new EventHandler((sender, e) => { name_selector.Open(this); });

            if (points.Count > 1)
            {
                menu_strip.ShowCheckMargin = true;
                menu_strip.Items.Add("Sign bit");
                menu_strip.Items[bit_sign_index_in_menustrip].Click += new EventHandler((sender, e) => {
                    //((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
                    //negative_bit = ((ToolStripMenuItem)sender).Checked;
                    //calculate_value();
                    set_negative_bit(!((ToolStripMenuItem)sender).Checked);
                });
            }
            else
            {
                menu_strip.Items.Add("Create gate");
                menu_strip.Items[2].Click += new EventHandler((s, e) => { Form1.gate_creator.Open(this); });
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
            foreach (Connection_point cp in points) cp.update_connection();
        }

        public void update_value(bool val, Connection_point cp)
        {
            int index = points.IndexOf(cp);
            labels[index].Text = val ? "1" : "0";
            labels[index].BackColor = val ? Color.Green : Color.FromArgb(164, 36, 59);
            calculate_value();
        }

        public void set_negative_bit(bool val)
        {
            negative_bit = val;
            ((ToolStripMenuItem)menu_strip.Items[bit_sign_index_in_menustrip]).Checked = val;
            calculate_value();
        }

        void calculate_value()
        {
            int value = 0;
            for (int i = 0; i < points.Count - 1; ++i) value += Convert.ToInt32(points[i].value) * (int)Math.Pow(2, i);
            if (negative_bit)
            {
                if (points.Last().value) value = -value;
            }
            else value += Convert.ToInt32(points.Last().value) * (int)Math.Pow(2, points.Count - 1);
            value_label.Text = value.ToString();
        }

        public void create_new_gate(string name, Color color)
        {
            List<Connection_point> start_points = new List<Connection_point>();
            points[0].search_for_start_points(start_points);
            if(start_points.Count == 0)
            {
                MessageBox.Show("Add at least one input gate to create a new gate");
                return;
            }
            Gates_manager.Add_gate_template(name, start_points, points[0], color);
            Gates_manager.Clear_board();
        }

        public void remove()
        {
            Gates_manager.gates.Remove(this);
            foreach (Connection_point cp in points) cp.remove_connections();
            board.Controls.Remove(container);
        }

        public void show_gate_tree()
        {
            name_label.Text = Form1.form.hideNamesToolStripMenuItem.Checked ? "" : text;
            Gates_manager.board.Controls.Add(container);
            if (points[0].connection.Count > 0) points[0].connection[0].show_gate_tree();
        }

        public void Get_gates_and_connections(List<Gate_values> gates_arr, List<Connection> connections_arr)
        {
            gates_arr.Add(new Gate_values(1, container.Location, this, Convert.ToByte(points.Count), 0, negative_bit, false, text));
            if (points[0].connection.Count > 0) points[0].connection[0].Get_gates_and_connections(gates_arr, connections_arr, gates_arr.Count - 1, 0);
        }

        internal void set_name(string text)
        {
            this.text = text;
            name_label.Text = Form1.form.hideNamesToolStripMenuItem.Checked ? "" : text;
        }
    }
}
