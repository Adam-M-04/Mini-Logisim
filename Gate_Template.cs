using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Symulator_ukladow_logicznych
{
    public class Gate_Template
    {
        Label template = new Label();
        Object tmp_gate_ref;
        Template_type type;
        int inputs_number, real_height, real_width = 80;
        Func<bool[], bool> calc_function;

        bool is_moving = false;
        Size mouseOffset;
        Point starting_location;

        public Gate_Template(string name, int inputs_number, Func<bool[], bool> calc_function)
        {
            this.type = Template_type.Logical_gate;
            this.inputs_number = inputs_number;
            this.calc_function = calc_function;
            real_height = inputs_number * 20 + 10;

            control_settings(name);
        }

        public Gate_Template(Template_type type)
        {
            this.type = type;
            control_settings(type == Template_type.Input_gate ? "Input" : "Output");
            real_height = 30;
            real_width = 30;
        }

        void control_settings(string name)
        {
            template.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            template.Top = 25;
            template.Left = Gates_manager.available_gates.Count * 90 + 10;
            template.Text = name;
            template.BackColor = Color.Wheat;
            template.Height = 50;
            template.Width = 80;
            template.TextAlign = ContentAlignment.MiddleCenter;
            template.BorderStyle = BorderStyle.FixedSingle;

            template.MouseDown += new MouseEventHandler(Mouse_down_handler);
            template.MouseMove += new MouseEventHandler(Mouse_move_handler);
            template.MouseUp += new MouseEventHandler(Mouse_up_handler);

            Gates_manager.gates_selector.Controls.Add(template);
        }

        public void Mouse_down_handler(object sender, MouseEventArgs e)
        {
            template.Cursor = Cursors.Hand;
            template.Height = real_height;
            template.Width = real_width;

            mouseOffset = new Size(e.Location);
            starting_location = template.Location;

            template.Parent = Gates_manager.form;

            template.BringToFront();
            is_moving = true;
        }

        public void Mouse_move_handler(object sender, MouseEventArgs e)
        {
            if (is_moving) 
            {
                Point newLocationOffset = e.Location - mouseOffset;

                if (template.Left + newLocationOffset.X < Gates_manager.board.Left || template.Top + newLocationOffset.Y < Gates_manager.board.Top) return;

                template.Left += newLocationOffset.X;
                template.Top += newLocationOffset.Y;
            }
        }

        public void Mouse_up_handler(object sender, MouseEventArgs e)
        {
            is_moving = false;

            Point location = template.Location;
            location.X = (int)Math.Round(location.X / 10.0) * 10 - Gates_manager.board.Left;
            location.Y = (int)Math.Round(location.Y / 10.0) * 10 - Gates_manager.board.Top;

            if (type == Template_type.Input_gate) tmp_gate_ref = new Input_gate(Gates_manager.board, location);
            else if (type == Template_type.Output_gate) tmp_gate_ref = new Output_gate(Gates_manager.board, location);
            else tmp_gate_ref = new Logical_gate(template.Text, Gates_manager.board, inputs_number, calc_function, location);
            Gates_manager.gates.Add(tmp_gate_ref);

            template.Cursor = Cursors.Default;
            template.Width = 80;
            template.Height = 50;
            template.Location = starting_location;
            template.Parent = Gates_manager.gates_selector;
            template.SendToBack();
        }
    }

    public enum Template_type
    {
        Input_gate,
        Output_gate,
        Logical_gate
    }
}
