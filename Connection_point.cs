using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Symulator_ukladow_logicznych
{
    public class Connection_point
    {
        public Label point;
        public point_type type;
        public List<Line_connection> connection = new List<Line_connection>();
        Line_connection tmp_to_remove = null;

        Object parent;

        public bool value = false;

        public Connection_point(int left, int top, point_type type, Object parent)
        {
            this.parent = parent;
            this.type = type;
            point = new Label();
            point.Width = 10;
            point.Height = 10;
            point.BackColor = Color.Black;
            point.Left = left;
            point.Top = top;

            point.MouseDown += new MouseEventHandler(point_mouse_down);
            point.AllowDrop = true;
            point.DragEnter += new DragEventHandler(OnDragEnter);
            point.DragLeave += new EventHandler(OnDragLeave);
            point.DragDrop += new DragEventHandler(OnDragDrop);
        }


        void point_mouse_down(object sender, MouseEventArgs e)
        {
            point.DoDragDrop(this, DragDropEffects.All);
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            tmp_to_remove = null;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            Connection_point event_connection_point = (Connection_point)e.Data.GetData(typeof(Connection_point));
            if (this == event_connection_point) return;
            if (type == event_connection_point.type) return;
            if (point.Parent == event_connection_point.point.Parent) return;

            if(parent.GetType().Name == "Logical_gate" && event_connection_point.parent.GetType().Name == "Logical_gate")
            {
                if (type == point_type.Input) 
                {
                    if (check_for_connection((Logical_gate)event_connection_point.parent)) return; 
                }
                else 
                {
                    if (event_connection_point.check_for_connection((Logical_gate)parent)) return; 
                }
            }
            

            e.Effect = DragDropEffects.All;

            Line_connection new_connection;
            if (type == point_type.Input) new_connection = new Line_connection(event_connection_point, this);
            else new_connection = new Line_connection(this, event_connection_point);

            add_connection(new_connection);
            event_connection_point.add_connection(new_connection);

            tmp_to_remove = new_connection;
        }

        private void OnDragLeave(object sender, EventArgs e)
        {
            if (tmp_to_remove == null) return;
            tmp_to_remove.remove();
            tmp_to_remove = null;
        }

        public void add_connection(Line_connection conn)
        {
            if(type == point_type.Input)
            {
                if(connection.Count > 0) connection[0].remove();
                connection.Add(conn);
            }
            else
            {
                connection.Add(conn);
            }
        }

        public void update_connection()
        {
            foreach(Line_connection conn in connection) conn.create_connection();
        }

        public void remove_connections()
        {
            while (connection.Count > 0) connection[0].remove();
        }

        public void update_value(bool val)
        {
            value = val;
            if(type == point_type.Output)
            {
                foreach(Line_connection lc in connection)
                {
                    lc.update_value(val);
                }
            }
            else
            {
                if (parent.GetType().Name == "Logical_gate") ((Logical_gate)parent).update_value();
                if (parent.GetType().Name == "Output_gate") ((Output_gate)parent).update_value(value);
            }
        }

        public bool check_for_connection(Logical_gate lg)
        {
            if (type == point_type.Input)
            {
                if (parent.GetType().Name == "Output_gate") return false;
                else return ((Logical_gate)parent).check_for_connection(lg);
            }
            else
            {
                foreach (Line_connection lc in connection) if (lc.check_for_connection(lg)) return true;
                return false;
            }
        }
    }

    public enum point_type
    {
        Input, 
        Output
    }
}
