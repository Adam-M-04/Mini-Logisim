using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic_gate_simulator
{
    public partial class Starting_window : Form
    {
        public Starting_window()
        {
            InitializeComponent();
        }

        private void Close_handler(object sender, FormClosingEventArgs e)
        {
            if(Project_manager.project_path == null) Application.Exit();
        }

        private void Open_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON files (*.json)|*.json";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader reader = new StreamReader(dialog.OpenFile()))
                {
                    if (Project_manager.Load(reader.ReadToEnd()))
                    {
                        Project_manager.project_path = dialog.FileName;
                        Close();                       
                    }
                    else
                    {
                        MessageBox.Show("Invalid project file");
                    }
                }
            }
        }

        private void Create_button_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JSON files (*.json)|*.json";
            dialog.RestoreDirectory = true;
            dialog.Title = "Select where to save your project";
            dialog.FileName = Project_textbox.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Project_textbox.Text = dialog.FileName;

                Stream myStream;
                if ((myStream = dialog.OpenFile()) != null)
                {
                    Project_manager.project_path = dialog.FileName;
                    myStream.Close();
                    Close();
                }
            }
        }
    }
}
