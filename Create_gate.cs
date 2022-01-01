using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Symulator_ukladow_logicznych
{
    public partial class Create_gate : Form
    {
        public Output_gate caller = null;
        Random rnd = new Random();

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
            caller.create_new_gate(Gate_name_textbox.Text, label_selected_color.BackColor);
        }

        public void Random_color()
        {
            label_selected_color.BackColor = Color.FromArgb(rnd.Next(0,255), rnd.Next(0, 255), rnd.Next(0, 255));
        }

        private void select_color_Click(object sender, EventArgs e)
        {
            if(color_selector.ShowDialog() == DialogResult.OK)
            {
                label_selected_color.BackColor = color_selector.Color;
            }
        }
    }
}
