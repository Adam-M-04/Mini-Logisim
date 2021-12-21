using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Symulator_ukladow_logicznych
{
    public class Gate_dragging
    {
        Label label;
        Panel panel, board;
        Size mouseOffset;
        bool is_dragged = false;
        Point starting_location;

        public Gate_dragging(Label label, Panel panel, Panel board)
        {
            this.label = label;
            this.panel = panel;
            this.board = board;

            label.MouseDown += new MouseEventHandler(control_MouseDown);
            label.MouseUp += new MouseEventHandler(control_MouseUp);
            label.MouseMove += new MouseEventHandler(control_MouseMove);
        }

        void control_MouseDown(object sender, MouseEventArgs e)
        {
            label.Cursor = Cursors.Hand;

            mouseOffset = new Size(e.Location);
            // turning on dragging
            is_dragged = true;

            starting_location = panel.Location;
        }
        void control_MouseUp(object sender, MouseEventArgs e)
        {
            label.Cursor = Cursors.Default;

            // turning off dragging
            is_dragged = false;

            foreach (Control ctr in board.Controls)
            {
                if ((string)ctr.Tag != "gate") return;
                if (panel.Bounds.IntersectsWith(ctr.Bounds) && this.panel != ctr)
                {
                    panel.Location = starting_location;
                    return;
                }
            }
        }
        void control_MouseMove(object sender, MouseEventArgs e)
        {
            // only if dragging is turned on
            if (is_dragged == true)
            {
                // calculations of control's new position
                Point newLocationOffset = e.Location - mouseOffset;

                if (panel.Left + newLocationOffset.X < 0 || panel.Right + newLocationOffset.X > board.Width
                    || panel.Top + newLocationOffset.Y < 0 || panel.Bottom + newLocationOffset.Y > board.Height) return;

                panel.Left += newLocationOffset.X;
                panel.Top += newLocationOffset.Y;
                panel.Left = (int)(Math.Round(panel.Left / 10.0) * 10);
                panel.Top = (int)(Math.Round(panel.Top / 10.0) * 10);
            }
        }
    }
}
