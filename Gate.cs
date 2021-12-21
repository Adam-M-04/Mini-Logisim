using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Symulator_ukladow_logicznych
{
    public class Gate
    {
        public Label label_gate = new Label();
        protected Panel board, container = new Panel();
        protected ContextMenuStrip menu_strip;
        protected Gate_dragging draggable;

        public Gate(string text, Panel p)
        {
            board = p;

            // label styles
            label_gate.Text = text;
            label_gate.TextAlign = ContentAlignment.MiddleCenter;
            label_gate.BorderStyle = BorderStyle.FixedSingle;
            label_gate.Top = 0;

            // Container styles
            container.Tag = "gate";

            draggable = new Gate_dragging(label_gate, container, board);

            menu_strip = new ContextMenuStrip();
            menu_strip.Items.Add("Delete");

            container.ContextMenuStrip = menu_strip;
        }
    }
}
