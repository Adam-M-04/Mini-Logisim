using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Symulator_ukladow_logicznych
{
    class Output_gate : Gate
    {
        public Connection_point point;

        public Output_gate(Panel p): base("0", p)
        {
            point = new Connection_point(0, 10, point_type.Input, this);
            container.Controls.Add(point.point);

            board.Controls.Add(container);
            container.Controls.Add(label_gate);

            // label styles
            label_gate.Height = 30;
            label_gate.Width = 30;
            label_gate.BackColor = Color.Pink;
            label_gate.Left = 5;

            // Container styles
            container.Height = label_gate.Height;
            container.Width = label_gate.Width + 5;

            label_gate.MouseMove += new MouseEventHandler(update_lines);
            label_gate.MouseUp += new MouseEventHandler(update_lines);

            menu_strip.Items[0].Click += new EventHandler((sender, e) => { remove(); });
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

        public void remove()
        {
            point.remove_connections();
            board.Controls.Remove(container);
        }
    }
}
