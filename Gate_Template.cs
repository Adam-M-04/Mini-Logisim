using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_gate_simulator
{
    public class Gate_Template
    {
        public Label template = new Label();
        Object tmp_gate_ref;
        public Template_type type;
        public int inputs_number, real_height, real_width = 80;
        public Color color;
        public string name;
        Func<bool[], bool> calc_function;
        List<Connection_point> start_points;
        public Connection_point output_point;
        public ContextMenuStrip menu_strip = new ContextMenuStrip();

        bool disabled = false;
        bool is_moving = false;
        Size mouseOffset;
        Point starting_location;

        public Gate_Template(string name, int inputs_number, Func<bool[], bool> calc_function)
        {
            type = Template_type.Logical_gate;
            this.calc_function = calc_function;
            calculate_real_height(inputs_number);
            set_style(name, name == "And" ? Color.FromArgb(42,157,143) : Color.FromArgb(231,111,81));
            control_settings();
        }

        public Gate_Template(string name, List<Connection_point> start_points, Connection_point output_point, Color color)
        {
            type = Template_type.Custom_logical_gate;
            this.output_point = output_point;
            set_style(name, color);
            calculate_real_height(start_points);

            menu_strip.ShowImageMargin = false;
            menu_strip.Items.Add("Edit");
            menu_strip.Items.Add("Delete");
            menu_strip.Items[0].Click += new EventHandler((sender, e) => { edit_gate(); });
            menu_strip.Items[1].Click += new EventHandler((sender, e) => { remove(); });
            template.ContextMenuStrip = menu_strip;

            control_settings();
        }

        public Gate_Template(Template_type type)
        {
            this.type = type;
            set_style(type == Template_type.Input_gate ? "Input" : "Output");

            menu_strip.ShowImageMargin = false;
            ToolStripMenuItem item = new ToolStripMenuItem("Output points");
            for(int i=0; i<4; ++i)
            {
                ToolStripMenuItem subitem = new ToolStripMenuItem(Math.Pow(2,i).ToString());
                item.DropDownItems.Add(subitem);
                item.DropDownItems[i].Click += new EventHandler((s, e)=> 
                {
                    foreach (ToolStripMenuItem tsmi in item.DropDownItems) tsmi.Checked = false;
                    ((ToolStripMenuItem)s).Checked = true;
                    if(type == Template_type.Input_gate) Gates_manager.input_gate_points_number = Convert.ToByte(((ToolStripMenuItem)s).Text);
                    if (type == Template_type.Output_gate) Gates_manager.output_gate_points_number = Convert.ToByte(((ToolStripMenuItem)s).Text);
                    calculate_real_height();
                });
            }
            ((ToolStripMenuItem)item.DropDownItems[0]).Checked = true;
            menu_strip.Items.Add(item);
            template.ContextMenuStrip = menu_strip;

            control_settings();
            calculate_real_height();
            real_width = 35;
        }

        public void calculate_real_height(List<Connection_point> start_points)
        {
            this.start_points = start_points;
            inputs_number = start_points.Count;
            real_height = inputs_number * 20 + 10;
        }
        public void calculate_real_height(int inputs_number)
        {
            this.inputs_number = inputs_number;
            real_height = inputs_number * 20 + 10;
        }
        public void calculate_real_height()
        {
            real_height = (type == Template_type.Input_gate ? Gates_manager.input_gate_points_number : Gates_manager.output_gate_points_number) * 20 + 55;
        }

        public void set_style(string name, Color color)
        {
            this.name = name;
            if(color != null) this.color = color;
            template.Text = name;
        }

        public void set_style(string name)
        {
            this.name = name;
            template.Text = name;
        }

        public void set_starting_location()
        {
            starting_location = template.Location;
        }

        void control_settings()
        {
            template.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            template.Top = Gates_manager.available_gates.Count > 0 ? Gates_manager.available_gates[0].template.Top : 25;
            template.Left = Gates_manager.available_gates.Count > 0 ? Gates_manager.available_gates.Last().template.Right + 10 : 10;
            template.Text = name;
            template.BackColor = Color.FromArgb(233,236,239);
            template.Height = 50;
            template.Width = 80;
            template.TextAlign = ContentAlignment.MiddleCenter;
            template.BorderStyle = BorderStyle.FixedSingle;
            template.Font = new Font("Arial", 10, FontStyle.Regular);

            template.MouseDown += new MouseEventHandler(Mouse_down_handler);
            template.MouseMove += new MouseEventHandler(Mouse_move_handler);
            template.MouseUp += new MouseEventHandler(Mouse_up_handler);

            Gates_manager.gates_selector.Controls.Add(template);
            set_starting_location();
        }

        public void Mouse_down_handler(object sender, MouseEventArgs e)
        {
            if (disabled) return;
            if (e.Button == MouseButtons.Left)
            {
                set_starting_location();
                Gates_manager.gates_selector.SuspendLayout();
                template.Cursor = Cursors.Hand;
                template.Height = real_height;
                template.Width = real_width;

                mouseOffset = new Size(e.Location);

                template.Parent = Gates_manager.form;
                
                template.BringToFront();
                is_moving = true;
            }
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
            if (disabled) return;
            if (e.Button == MouseButtons.Right) return;
            is_moving = false;

            if (template.Location.X < Gates_manager.board.Left || template.Location.X > Gates_manager.board.Width + Gates_manager.board.Left ||
                template.Location.Y < Gates_manager.board.Top || template.Location.Y > Gates_manager.board.Height + Gates_manager.board.Top)
            { 
                template_to_default();
                Gates_manager.gates_selector.ResumeLayout();
                return; 
            }

            Point location = template.Location;
            location.X = (int)Math.Round(location.X / 10.0) * 10 - Gates_manager.board.Left;
            location.Y = (int)Math.Round(location.Y / 10.0) * 10 - Gates_manager.board.Top;
            Add_gate(location);
            template_to_default();
            Gates_manager.gates_selector.ResumeLayout();
        }

        public void Add_gate(Point location)
        {
            if (type == Template_type.Input_gate) tmp_gate_ref = new Input_gate(location);
            else if (type == Template_type.Output_gate) tmp_gate_ref = new Output_gate(location);
            else if (type == Template_type.Custom_logical_gate) tmp_gate_ref = new Logical_gate(template.Text, inputs_number, null, location, color, this, start_points, output_point);
            else tmp_gate_ref = new Logical_gate(template.Text, inputs_number, calc_function, location, color, this);

            if (tmp_gate_ref.GetType().Name == "Logical_gate") ((Logical_gate)tmp_gate_ref).Name_hidden(Form1.form.hideNamesToolStripMenuItem.Checked);
            if (tmp_gate_ref.GetType().Name == "Input_gate") ((Input_gate)tmp_gate_ref).Name_hidden(Form1.form.hideNamesToolStripMenuItem.Checked);

            Gates_manager.gates.Add(tmp_gate_ref);
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

        public void edit_gate()
        {
            Gates_manager.current_edited = this;
            Gates_manager.Gates_Enabled(false, this);
            Gates_manager.Clear_board();
            ((Output_gate)output_point.parent).show_gate_tree();
        }

        public void Disabled(bool val)
        {
            disabled = val;
            template.BackColor = val ? Color.FromArgb(52, 58, 64) : Color.FromArgb(233, 236, 239);
        }

        public void remove()
        {
            int i = Math.Min(Gates_manager.available_gates.IndexOf(this) + 1, 4);
            while (i < Gates_manager.available_gates.Count && i > 3)
            {
                if (Gates_manager.available_gates[i].Check_if_contains_gate(this))
                {
                    MessageBox.Show("This logical gate cannot be deleted because it is in use in another logical gate");
                    return;
                }
                ++i;
            }
            i = 0;

            if(this == Gates_manager.current_edited) 
            { 
                Gates_manager.Context_menu_options(true); 
                Gates_manager.Gates_Enabled(true); 
                Gates_manager.Clear_board();
                Gates_manager.current_edited = null;
            }
            while (Gates_manager.available_gates.Count > i)
            {
                if(Gates_manager.available_gates[i] == this)
                {
                    Gates_manager.available_gates.RemoveAt(i);
                    Gates_manager.gates_selector.Controls.Remove(template);
                    break;
                }
                ++i;
            }
            while (Gates_manager.available_gates.Count > i)
            {
                Gates_manager.available_gates[i].template.Left -= 90;
                Gates_manager.available_gates[i++].set_starting_location();
            }
            for(i=0; i<Gates_manager.gates.Count; ++i)
            {
                if(Gates_manager.gates[i].GetType().Name == "Logical_gate")
                {
                    Logical_gate gate = (Logical_gate)Gates_manager.gates[i];
                    if (gate.template == this)
                    {
                        gate.remove();
                        i--;
                    }
                }
            }    
        }

        public bool Check_if_contains_gate(Gate_Template template)
        {
            return output_point.connection[0].Check_if_contains_gate(template);
        }

        public Board_values Get_gates_and_connections()
        {
            List<Gate_values> gates_arr = new List<Gate_values>();
            List<Connection> connections_arr = new List<Connection>();
            ((Output_gate)output_point.parent).Get_gates_and_connections(gates_arr, connections_arr);

            return new Board_values(gates_arr, connections_arr);
        }

        public void Names_hidden(bool val)
        {
            output_point.connection[0].Names_hidden(val);
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
