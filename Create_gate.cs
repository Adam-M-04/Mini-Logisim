using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_gate_simulator
{
    public partial class Create_gate : Form
    {
        public Output_gate caller = null;
        Random rnd = new Random();
        window_type type;

        public Create_gate()
        {
            InitializeComponent();
            Width = 280;
            
            select_color.Width = Gate_name_textbox.Width / 2 - 5;
            select_color.Left = Gate_name_textbox.Left;

            label_selected_color.Width = select_color.Width;
            label_selected_color.Left = select_color.Left + select_color.Width + 10;

            Random_color();
        }

        private void create_button_Click(object sender, EventArgs e)
        {
            if (Gate_name_textbox.Text.Length == 0)
            {
                MessageBox.Show("Enter a name for gate");
                return;
            }
            Close();
            if (type == window_type.Creating) caller.create_new_gate(Gate_name_textbox.Text, label_selected_color.BackColor);
            else Gates_manager.Save_edited_gate(Gate_name_textbox.Text, label_selected_color.BackColor);
        }

        public void Random_color()
        {
            label_selected_color.BackColor = Color.FromArgb(rnd.Next(0,255), rnd.Next(0, 255), rnd.Next(0, 255));
        }

        public void Clear()
        {
            Gate_name_textbox.Text = "";
        }

        private void select_color_Click(object sender, EventArgs e)
        {
            if(color_selector.ShowDialog() == DialogResult.OK)
            {
                label_selected_color.BackColor = color_selector.Color;
            }
        }

        public void Open(Output_gate caller)
        {
            type = window_type.Creating;
            this.caller = caller;
            create_button.Text = "Create";
            Random_color();
            Clear();
            ShowDialog();
        }

        public void Load_gate_settings()
        {
            if (Gates_manager.current_edited == null) return;
            type = window_type.Editing;
            create_button.Text = "Save";
            Gate_name_textbox.Text = Gates_manager.current_edited.name;
            label_selected_color.BackColor = Gates_manager.current_edited.color;
            ShowDialog();
        }
    }

    enum window_type
    {
        Creating,
        Editing
    }
}
