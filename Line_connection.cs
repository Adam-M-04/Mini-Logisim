using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Logic_gate_simulator
{
    public class Line_connection
    {
        public Connection_point p1, p2;
        private const int line_size = 6;
        Label start_point, end_point;
        ContextMenuStrip menu_strip = new ContextMenuStrip();

        Label[] connection_line;

        public bool value = false;

        public Line_connection(Connection_point p1, Connection_point p2)
        {
            this.p1 = p1;
            this.p2 = p2;

            start_point = p1.point;
            end_point = p2.point;

            menu_strip.ShowImageMargin = false;
            menu_strip.Items.Add("Delete");
            menu_strip.Items[0].Click += new EventHandler((sender, e)=> { remove(); });

            initialize_array();
            create_connection();

            update_value(p1.value);
        }

        public void create_connection()
        {
            if(get_panel(start_point).Left + start_point.Left + 20 < get_panel(end_point).Left + end_point.Left)
            {
                int width = Math.Abs(get_panel(end_point).Left - get_panel(start_point).Right);
                int higher_point = Math.Min(start_point.Top + get_panel(start_point).Top, end_point.Top + get_panel(end_point).Top) + 2;

                connection_line[0].Visible = false;
                connection_line[4].Visible = false;

                connection_line[1].Height = line_size;
                connection_line[1].Width = width / 2;
                connection_line[1].Left = get_panel(start_point).Right;
                connection_line[1].Top = start_point.Top + get_panel(start_point).Top + 2;

                connection_line[2].Width = line_size;
                connection_line[2].Height = Math.Abs(get_panel(end_point).Top + end_point.Top - get_panel(start_point).Top - start_point.Top) + 6;
                connection_line[2].Left = get_panel(start_point).Right + connection_line[1].Width;
                connection_line[2].Top = higher_point;

                connection_line[3].Height = line_size;
                connection_line[3].Width = width / 2;
                connection_line[3].Left = get_panel(start_point).Right + connection_line[3].Width;
                connection_line[3].Top = end_point.Top + get_panel(end_point).Top + 2;
            }
            else
            {
                connection_line[0].Visible = true;
                connection_line[0].Height = line_size;
                connection_line[0].Width = 10;
                connection_line[0].Left = get_panel(start_point).Right;
                connection_line[0].Top = start_point.Top + get_panel(start_point).Top + 2;

                connection_line[4].Visible = true;
                connection_line[4].Height = line_size;
                connection_line[4].Width = 10;
                connection_line[4].Left = get_panel(end_point).Left - 10;
                connection_line[4].Top = get_panel(end_point).Top + end_point.Top + 2;

                connection_line[1].Width = line_size;
                connection_line[3].Width = line_size;
                connection_line[2].Height = line_size;

                if(get_panel(start_point).Bottom + 10 < get_panel(end_point).Top)
                {
                    int height = get_panel(end_point).Top + end_point.Top - get_panel(start_point).Top - start_point.Top;

                    connection_line[1].Height = height / 2;
                    connection_line[1].Left = get_panel(start_point).Right + 4;
                    connection_line[1].Top = connection_line[0].Top;

                    connection_line[3].Height = height / 2;
                    connection_line[3].Left = get_panel(end_point).Left - 10;
                    connection_line[3].Top = connection_line[4].Bottom - connection_line[3].Height;

                    connection_line[2].Top = connection_line[1].Top + height / 2;
                }
                else if(get_panel(start_point).Top > get_panel(end_point).Bottom + 10)
                {
                    int height = get_panel(start_point).Top + start_point.Top - get_panel(end_point).Top - end_point.Top;

                    connection_line[1].Height = height / 2;
                    connection_line[1].Left = get_panel(start_point).Right + 4;
                    connection_line[1].Top = connection_line[0].Bottom - connection_line[1].Height;

                    connection_line[3].Height = height / 2;
                    connection_line[3].Left = get_panel(end_point).Left - 10;
                    connection_line[3].Top = connection_line[4].Top;

                    connection_line[2].Top = connection_line[3].Top + height / 2;
                }
                else
                {
                    connection_line[1].Height = get_panel(start_point).Height - start_point.Top + 10;
                    connection_line[1].Left = get_panel(start_point).Right + 4;
                    connection_line[1].Top = connection_line[0].Top;
                    
                    connection_line[3].Height = get_panel(end_point).Height - end_point.Top + 10;
                    connection_line[3].Left = get_panel(end_point).Left - 10;
                    connection_line[3].Top = end_point.Top + get_panel(end_point).Top + 2;
                    
                    
                    connection_line[2].Top = Math.Max(connection_line[1].Bottom, connection_line[3].Bottom);

                    if (connection_line[1].Bottom > connection_line[3].Bottom) connection_line[3].Height += connection_line[1].Bottom - connection_line[3].Bottom + 6;
                    else connection_line[1].Height += connection_line[3].Bottom - connection_line[1].Bottom + 6;
                }

                connection_line[2].Width = connection_line[1].Right - connection_line[3].Left;
                connection_line[2].Left = connection_line[3].Left;
            }
        }

        void initialize_array()
        {
            connection_line = new Label[5];
            for (int i = 0; i < 5; ++i)
            {
                connection_line[i] = new Label();
                connection_line[i].BackColor = Color.Gray;
                connection_line[i].ContextMenuStrip = menu_strip;
            }
            display();
        }

        private Panel get_panel(Label control)
        {
            return (Panel)control.Parent;
        }

        public void remove()
        {
            foreach (Label l in connection_line) Gates_manager.board.Controls.Remove(l);
            p1.connection.Remove(this);
            p2.connection.Remove(this);
            p2.update_value(false);
        }

        public void display()
        {
            foreach (Label l in connection_line) Gates_manager.board.Controls.Add(l);
        }

        public void update_value(bool val)
        {
            value = val;
            foreach (Label l in connection_line) l.BackColor = value ? Color.Green : Color.Gray;
            p2.update_value(value);
        }

        public bool check_for_connection(Logical_gate lg)
        {
            return p2.check_for_connection(lg);
        }

        public bool Check_if_contains_gate(Gate_Template template)
        {
            if (p1.parent.GetType().Name == "Logical_gate") return ((Logical_gate)p1.parent).Check_if_contains_gate(template);
            return false;
        }

        public void search_for_start_points(List<Connection_point> points)
        {
            p1.search_for_start_points(points);
        }

        public void show_gate_tree()
        {
            display();
            if (p1.parent.GetType().Name == "Logical_gate") ((Logical_gate)p1.parent).show_gate_tree();
            else ((Input_gate)p1.parent).show_gate_tree();
        }

        public void Get_gates_and_connections(List<Gate_values> gates_arr, List<Connection> connections_arr, int prev_index, int point_index)
        {
            if (p1.parent.GetType().Name == "Logical_gate") ((Logical_gate)p1.parent).Get_gates_and_connections(gates_arr, connections_arr, prev_index, point_index);
            else ((Input_gate)p1.parent).Get_gates_and_connections(gates_arr, connections_arr, prev_index, point_index, p1);
        }

        public void Names_hidden(bool val)
        {
            if (p1.parent.GetType().Name == "Logical_gate") ((Logical_gate)p1.parent).Name_hidden(val);
            else ((Input_gate)p1.parent).Name_hidden(val);
        }
    }
}
