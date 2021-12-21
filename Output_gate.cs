using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Symulator_ukladow_logicznych
{
    class Output_gate
    {
        public Label label_gate = new Label();
        Panel board, container = new Panel();
        ContextMenuStrip menu_strip;

        public Connection_point point;
        Gate_dragging draggable;

        public Output_gate(Panel p)
        {
            board = p;

            point = new Connection_point(0, 10, point_type.Input, this);
            container.Controls.Add(point.point);

            board.Controls.Add(container);
            container.Controls.Add(label_gate);

            // label styles
            label_gate.Text = "0";
            label_gate.Height = 30;
            label_gate.Width = 30;
            label_gate.BackColor = Color.Pink;
            label_gate.TextAlign = ContentAlignment.MiddleCenter;
            label_gate.BorderStyle = BorderStyle.FixedSingle;
            label_gate.Left = 5;
            label_gate.Top = 0;

            // Container styles
            container.Height = label_gate.Height;
            container.Width = label_gate.Width + 5;
            container.Tag = "gate";

            draggable = new Gate_dragging(label_gate, container, board);

            label_gate.MouseMove += new MouseEventHandler(update_lines);
            label_gate.MouseUp += new MouseEventHandler(update_lines);
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
