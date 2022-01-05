using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Logic_gate_simulator
{
    public class Gate
    {
        public Label label_gate = new Label();
        public Panel board, container = new Panel();
        public ContextMenuStrip menu_strip = new ContextMenuStrip();
        public Gate_dragging draggable;

        public Gate(string text, Point location)
        {
            board = Gates_manager.board;

            // label styles
            label_gate.Text = text;
            label_gate.TextAlign = ContentAlignment.MiddleCenter;
            label_gate.BorderStyle = BorderStyle.FixedSingle;
            label_gate.Top = 0;
            label_gate.Font = new Font("Arial", 10, FontStyle.Regular);

            // Container styles
            container.Location = location;
            container.Tag = "gate";

            draggable = new Gate_dragging(label_gate, container, board);

            menu_strip.Items.Add("Delete");
            menu_strip.ShowImageMargin = false;

            container.ContextMenuStrip = menu_strip;
        }
    }
}
