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
            values.output_points_number = Gates_manager.input_gate_points_number;
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
            Gates_manager.Default_templates();
            try
            {
                values = JsonConvert.DeserializeObject<ValuesToSave>(value);
                foreach(Gate_template_values template in values.templates_arr)
                {
                    if (!template.Restore()) return false;
                }
                if (values.editing_index == -1) values.board_state.Restore();
                else Gates_manager.available_gates[values.editing_index].edit_gate();
                Gates_manager.input_gate_points_number = values.output_points_number;
                foreach (ToolStripMenuItem tsmi in ((ToolStripMenuItem)Gates_manager.available_gates[0].menu_strip.Items[0]).DropDownItems)
                {
                    if (tsmi.Text == Gates_manager.input_gate_points_number.ToString()) tsmi.Checked = true;
                    else tsmi.Checked = false;
                }
                Gates_manager.available_gates[0].calculate_real_height();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public static void Open()
        {
            window.Clear();
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
        public byte output_points_number { get; set; }

        public ValuesToSave() { templates_arr = new Gate_template_values[0]; }
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
                if (gate.GetType().Name == "Logical_gate") gates.Add(new Gate_values(Gates_manager.available_gates.IndexOf(((Logical_gate)gate).template), ((Logical_gate)gate).container.Location, gate));
                if (gate.GetType().Name == "Output_gate") gates.Add(new Gate_values(1, ((Output_gate)gate).container.Location, gate));
                if (gate.GetType().Name == "Input_gate")
                {
                    Input_gate input_gate = (Input_gate)gate;
                    gates.Add(
                        new Gate_values(
                            0,
                            input_gate.container.Location, 
                            gate, 
                            (byte)input_gate.points.Count, 
                            Convert.ToInt32(input_gate.value_label.Text),
                            input_gate.negative_bit,
                            input_gate.values.Last()
                        )
                    );
                }
            }
            foreach (Object gate in Gates_manager.gates)
            {
                int curr_index = get_index(gate);
                int point_index = 0;
                if (gate.GetType().Name == "Logical_gate")
                {
                    Connection_point[] points = ((Logical_gate)gate).input_points;
                    for (int i=0; i < points.Length; ++i)
                    {
                        if (points[i].connection.Count <= 0) continue;
                        Object starting_gate = points[i].connection[0].p1.parent;
                        if (starting_gate.GetType().Name == "Input_gate") point_index = ((Input_gate)starting_gate).get_point_index(points[i].connection[0].p1);
                        connections.Add(new Connection(get_index(starting_gate), curr_index, i, point_index));
                    }
                }
                if (gate.GetType().Name == "Output_gate")
                {
                    if (((Output_gate)gate).point.connection.Count <= 0) continue;
                    Object starting_gate = ((Output_gate)gate).point.connection[0].p1.parent;
                    if (starting_gate.GetType().Name == "Input_gate") point_index = ((Input_gate)starting_gate).get_point_index(((Output_gate)gate).point.connection[0].p1);
                    connections.Add(new Connection(get_index(starting_gate), curr_index, 0, point_index));
                }
            }
        }

        public Output_gate Restore()
        {
            Output_gate output = null;
            foreach (Gate_values gate in gates)
            {
                if (gate.index == 0) Gates_manager.input_gate_points_number = gate.output_points_number;
                Gates_manager.available_gates[gate.index].Add_gate(gate.location);
                if (gate.index == 0) ((Input_gate)Gates_manager.gates[Gates_manager.gates.Count - 1]).set_value(gate.value, gate.negative_bit, gate.negative_bit_on);
                if (gate.index == 1) output = (Output_gate)Gates_manager.gates[Gates_manager.gates.Count - 1];
            }
            foreach (Connection conn in connections)
            {
                Connection_point p1 = Get_connection_point(Gates_manager.gates[conn.start_index], Gates_manager.gates[conn.start_index].GetType().Name == "Input_gate" ? conn.start_point_index : -1);
                Connection_point p2 = Get_connection_point(Gates_manager.gates[conn.end_index], conn.point_index);
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
                return ((Input_gate)gate).points[index];
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

        // Input gate properties
        public byte output_points_number { get; set; }
        public int value { get; set; }
        public bool negative_bit { get; set; }
        public bool negative_bit_on { get; set; }

        [JsonIgnore]
        public Object gate_ref;

        public Gate_values(int index, Point location, Object gate = null, byte opn = 1, int value = 0, bool nb = false, bool nbo = false)
        {
            this.index = index;
            this.location = location;
            gate_ref = gate;
            output_points_number = opn;
            this.value = value;
            negative_bit = nb;
            negative_bit_on = nbo;
        }
    }

    public class Connection
    {
        public int start_index { get; set; }
        public int end_index { get; set; }

        public int point_index { get; set; }
        public int start_point_index { get; set; }

        public Connection(int s_index, int e_index, int p_index, int start_point_index = 0)
        {
            start_index = s_index;
            end_index = e_index;
            point_index = p_index;
            this.start_point_index = start_point_index;
        }
    }

}
