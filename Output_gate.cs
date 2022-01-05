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
        public Connection_point point;

        public Output_gate(Point location): base("0", location)
        {
            point = new Connection_point(0, 10, point_type.Input, this);
            container.Controls.Add(point.point);

            // label styles
            label_gate.Height = 30;
            label_gate.Width = 30;
            label_gate.BackColor = Color.LightCoral;
            label_gate.Left = 5;

            // Container styles
            container.Height = label_gate.Height;
            container.Width = label_gate.Width + 5;

            label_gate.MouseMove += new MouseEventHandler(update_lines);
            label_gate.MouseUp += new MouseEventHandler(update_lines);


            menu_strip.Items.Add("Create gate");
            menu_strip.Items[0].Click += new EventHandler((sender, e) => {remove();});
            menu_strip.Items[1].Click += new EventHandler((sender, e) => {Form1.gate_creator.Open(this);});

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
            point.update_connection();
        }

        public void update_value(bool val)
        {
            label_gate.Text = val ? "1" : "0";
            label_gate.BackColor = val ? Color.Green : Color.LightCoral;
        }

        public void create_new_gate(string name, Color color)
        {
            List<Connection_point> start_points = new List<Connection_point>();
            point.search_for_start_points(start_points);
            if(start_points.Count == 0)
            {
                MessageBox.Show("Add at least one input gate to create a new gate");
                return;
            }
            Gates_manager.Add_gate_template(name, start_points, point, color);
            Gates_manager.Clear_board();
        }

        public void remove()
        {
            Gates_manager.gates.Remove(this);
            point.remove_connections();
            board.Controls.Remove(container);
        }

        public void show_gate_tree()
        {
            Gates_manager.board.Controls.Add(container);
            if (point.connection.Count > 0) point.connection[0].show_gate_tree();
        }
    }
}
