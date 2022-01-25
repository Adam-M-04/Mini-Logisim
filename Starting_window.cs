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
        public Label ProjectTitle;
        public Starting_window()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            Project_textbox.Text = "";
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
                        Set_title(Path.GetFileNameWithoutExtension(dialog.FileName));
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
                Stream myStream;
                if ((myStream = dialog.OpenFile()) != null)
                {
                    Set_title(Path.GetFileNameWithoutExtension(dialog.FileName));
                    Project_manager.project_path = dialog.FileName;
                    Gates_manager.Default_templates();
                    myStream.Close();
                    Close();
                    Project_manager.Save();
                }
            }
        }

        private void Set_title(string title)
        {
            ProjectTitle.Text = title;
            ProjectTitle.Left = Form1.form.Width - ProjectTitle.Width - 20;
        }
    }
}
