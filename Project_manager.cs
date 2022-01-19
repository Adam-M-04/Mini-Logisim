using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_gate_simulator
{
    public static class Project_manager
    {
        public static ValuesToSave values = new ValuesToSave();
        public static Starting_window window = new Starting_window();
        public static string project_path = null;

        public static void Save()
        {
            values.templates_arr = new Gate_template_values[Gates_manager.available_gates.Count - 4];
            for (int i = 4; i < Gates_manager.available_gates.Count; ++i) values.templates_arr[i - 4] = new Gate_template_values(Gates_manager.available_gates[i]);
            values.board_state = new Board_values();
            values.editing_index = Gates_manager.available_gates.IndexOf(Gates_manager.current_edited);

            using(StreamWriter sw = new StreamWriter(project_path))
            {
                try
                {
                    sw.Write(JsonConvert.SerializeObject(values));
                }
                catch(Exception e)
                {
                    MessageBox.Show("An error occured while saving project");
                }
            }           
        }

        public static bool Load(string value)
        {
            try
            {
                values = JsonConvert.DeserializeObject<ValuesToSave>(value);
                foreach(Gate_template_values template in values.templates_arr)
                {
                    if (!template.Restore()) return false;
                }
                if (values.editing_index == -1) values.board_state.Restore();
                else Gates_manager.available_gates[values.editing_index].edit_gate();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public static void Open()
        {
            window.ShowDialog();
        }
    }

    public class Gate_template_values
    {
        public Color color { get; set; }
        public string name { get; set; }
        public Board_values structure { get; set; }

        public Gate_template_values(){}
        public Gate_template_values(Gate_Template gt)
        {
            color = gt.color;
            name = gt.name;
            structure = gt.Get_gates_and_connections();
        }

        public bool Restore()
        {
            Output_gate output_gate = structure.Restore();
            if (output_gate == null) return false;
            output_gate.create_new_gate(name, color);
            return true;
        }
    }

    public class ValuesToSave
    {
        public Gate_template_values[] templates_arr { get; set; }
        public Board_values board_state { get; set; }
        public int editing_index { get; set; }
    }

    public class Board_values
    {
        public List<Gate_values> gates { get; set; }
        public List<Connection> connections { get; set; }

        public Board_values(List<Gate_values> gates, List<Connection> conn) { this.gates = gates; connections = conn; }

        public Board_values()
        {
            gates = new List<Gate_values>();
            connections = new List<Connection>();
            foreach (Object gate in Gates_manager.gates)
            {
                if (gate.GetType().Name == "Logical_gate") gates.Add(new Gate_values(((Logical_gate)gate).index, ((Logical_gate)gate).container.Location, gate));
                if (gate.GetType().Name == "Output_gate") gates.Add(new Gate_values(1, ((Output_gate)gate).container.Location, gate));
                if (gate.GetType().Name == "Input_gate") gates.Add(new Gate_values(0, ((Input_gate)gate).container.Location, gate));
            }
            foreach (Object gate in Gates_manager.gates)
            {
                int curr_index = get_index(gate);
                if (gate.GetType().Name == "Logical_gate")
                {
                    Connection_point[] points = ((Logical_gate)gate).input_points;
                    for (int i=0; i < points.Length; ++i)
                    {
                        if(points[i].connection.Count > 0) connections.Add(new Connection(get_index(points[i].connection[0].p1.parent), curr_index, i));
                    }
                }
                if (gate.GetType().Name == "Output_gate")
                {
                    if(((Output_gate)gate).point.connection.Count > 0) connections.Add(new Connection(get_index(((Output_gate)gate).point.connection[0].p1.parent), curr_index, 0));
                }
            }
        }

        public Output_gate Restore()
        {
            Output_gate output = null;
            foreach (Gate_values gate in gates)
            {
                Gates_manager.available_gates[gate.index].Add_gate(gate.location);
                if (gate.index == 1) output = (Output_gate)Gates_manager.gates[Gates_manager.gates.Count - 1];
            }
            foreach (Connection conn in connections)
            {
                Connection_point p1 = Get_connection_point(Gates_manager.gates[conn.start_index]), p2 = Get_connection_point(Gates_manager.gates[conn.end_index], conn.point_index);
                Line_connection new_connection = new Line_connection(p1, p2);
                p1.add_connection(new_connection);
                p2.add_connection(new_connection);
            }
            return output;
        }
        Connection_point Get_connection_point(Object gate, int index = -1)
        {
            if (gate.GetType().Name == "Logical_gate")
            {
                if (index == -1) return ((Logical_gate)gate).output_point;
                else return ((Logical_gate)gate).input_points[index];
            }
            if (gate.GetType().Name == "Output_gate")
            {
                return ((Output_gate)gate).point;
            }
            if (gate.GetType().Name == "Input_gate")
            {
                return ((Input_gate)gate).point;
            }
            return null;
        }

        int get_index(Object gate)
        {
            for(int i=0; i<gates.Count; ++i)
            {
                if (gates[i].gate_ref == gate) return i;
            }
            return -1;
        }
    }

    public class Gate_values
    {
        public int index { get; set; }
        public Point location { get; set; }

        [JsonIgnore]
        public Object gate_ref;

        public Gate_values(int index, Point location, Object gate = null)
        {
            this.index = index;
            this.location = location;
            gate_ref = gate;
        }
    }

    public class Connection
    {
        public int start_index { get; set; }
        public int end_index { get; set; }

        public int point_index { get; set; }

        public Connection(int s_index, int e_index, int p_index)
        {
            start_index = s_index;
            end_index = e_index;
            point_index = p_index;
        }
    }

}
