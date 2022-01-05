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
        public Connection_point point;

        public bool value = false;

        public Input_gate(Point location) : base("0", location)
        {
            point = new Connection_point(25, 10, point_type.Output, this);
            container.Controls.Add(point.point);

            // label styles
            label_gate.Height = 30;
            label_gate.Width = 30;
            label_gate.BackColor = Color.Pink;
            label_gate.Left = 0;

            // Container styles
            container.Height = label_gate.Height;
            container.Width = label_gate.Width + 5;

            //draggable = new Gate_dragging(label_gate, container, board);

            label_gate.MouseMove += new MouseEventHandler(update_lines);
            label_gate.MouseUp += new MouseEventHandler(update_lines);
            label_gate.DoubleClick += new EventHandler(change_state);

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
            point.update_connection();
        }

        void change_state(object sender, EventArgs e)
        {
            value = !value;
            label_gate.Text = value ? "1" : "0";
            label_gate.BackColor = value ? Color.Green : Color.Pink;

            point.update_value(value);
        }

        public void remove()
        {
            Gates_manager.gates.Remove(this);
            point.update_value(false);
            point.remove_connections();
            board.Controls.Remove(container);
        }

        public void show_gate_tree()
        {
            Gates_manager.board.Controls.Add(container);
        }
    }
}
