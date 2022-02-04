using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_gate_simulator
{
    public class Gate_dragging
    {
        Panel panel;
        Size mouseOffset;
        bool is_dragged = false;
        Point starting_location;

        public Gate_dragging(Label label, Panel panel)
        {
            this.panel = panel;

            Add_Events(label);
        }

        void control_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                ((Label)sender).Cursor = Cursors.Hand;
                panel.BringToFront();

                mouseOffset = new Size(e.Location);

                starting_location = panel.Location;

                // turning on dragging
                is_dragged = true;
            }
        }
        void control_MouseUp(object sender, MouseEventArgs e)
        {
            if (true)
            {
                ((Label)sender).Cursor = Cursors.Default;
                panel.SendToBack();

                // turning off dragging
                is_dragged = false;

                if(Gates_manager.Is_overlapping(panel))
                {
                    panel.Location = starting_location;
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

                if (panel.Left + newLocationOffset.X < 0 || panel.Top + newLocationOffset.Y < 0 ) return;

                panel.Left += newLocationOffset.X;
                panel.Top += newLocationOffset.Y;
                panel.Left = (int)(Math.Round(panel.Left / 10.0) * 10);
                panel.Top = (int)(Math.Round(panel.Top / 10.0) * 10);
            }
        }

        public void Add_Events(Label label)
        {
            label.MouseDown += new MouseEventHandler(control_MouseDown);
            label.MouseUp += new MouseEventHandler(control_MouseUp);
            label.MouseMove += new MouseEventHandler(control_MouseMove);
        }
    }
}
