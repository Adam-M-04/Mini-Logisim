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
        Color color;
        Func<bool[], bool> calc_function;
        List<Connection_point> start_points;
        Connection_point output_point;

        bool is_moving = false;
        Size mouseOffset;
        Point starting_location;

        public Gate_Template(string name, int inputs_number, Func<bool[], bool> calc_function)
        {
            type = Template_type.Logical_gate;
            this.inputs_number = inputs_number;
            this.calc_function = calc_function;
            real_height = inputs_number * 20 + 10;
            color = Color.Wheat;

            control_settings(name);
        }
        public Gate_Template(string name, List<Connection_point> start_points, Connection_point output_point, Color color)
        {
            type = Template_type.Custom_logical_gate;
            this.start_points = start_points;
            this.output_point = output_point;
            inputs_number = start_points.Count;
            real_height = inputs_number * 20 + 10;
            this.color = color;

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
            template.Font = new Font("Arial", 10, FontStyle.Regular);

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
                Point newLocation = e.Location - mouseOffset;

                if (template.Left + newLocation.X < Gates_manager.board.Left || 
                    template.Top + newLocation.Y < Gates_manager.board.Top) return;

                template.Left += newLocation.X;
                template.Top += newLocation.Y;
            }
        }

        public void Mouse_up_handler(object sender, MouseEventArgs e)
        {
            is_moving = false;

            if (template.Location.X < Gates_manager.board.Left || template.Location.X > Gates_manager.board.Width + Gates_manager.board.Left ||
                template.Location.Y < Gates_manager.board.Top || template.Location.Y > Gates_manager.board.Height + Gates_manager.board.Top) { template_to_default(); return; }

            Point location = template.Location;
            location.X = (int)Math.Round(location.X / 10.0) * 10 - Gates_manager.board.Left;
            location.Y = (int)Math.Round(location.Y / 10.0) * 10 - Gates_manager.board.Top;

            if (type == Template_type.Input_gate) tmp_gate_ref = new Input_gate(location);
            else if (type == Template_type.Output_gate) tmp_gate_ref = new Output_gate(location);
            else if (type == Template_type.Custom_logical_gate) tmp_gate_ref = new Logical_gate(template.Text, inputs_number, null, location, color, start_points, output_point);
            else tmp_gate_ref = new Logical_gate(template.Text, inputs_number, calc_function, location, color);
            Gates_manager.gates.Add(tmp_gate_ref);

            template_to_default();
        }

        void template_to_default()
        {
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
        Logical_gate,
        Custom_logical_gate
    }
}
