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
    public partial class Form1 : Form
    {
        public static Form1 form;
        public static Create_gate gate_creator = new Create_gate();
        public static MenuStrip menu;
        HelpWindow help_window = new HelpWindow();

        public Form1()
        {
            InitializeComponent();
            
            form = this;
            menu = menuStrip;
            menu.Items[2].Click += new EventHandler((sender, e) => { gate_creator.Load_gate_settings(); });

            board.Left = 10;
            board.Top = 30;
            board.Width = form.Width - 35;
            board.Height = form.Height - 170;

            gates_selector_panel.Left = 10;
            gates_selector_panel.Width = form.Width - 35;            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Gates_manager.form = this;
            Gates_manager.board = board;
            Gates_manager.gates_selector = gates_selector_panel;
            Gates_manager.Default_templates();

            Hide();
            Project_manager.window.ProjectTitle = ProjectTitle;
            Project_manager.Open();
        }

        private void resize_handler(object sender, EventArgs e)
        {
            board.Width = form.Width - 35;
            board.Height = form.Height - 170;

            gates_selector_panel.Width = form.Width - 35;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project_manager.Save();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project_manager.Open();
        }

        private void hideNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(Object gate in Gates_manager.gates)
            {
                if (gate.GetType().Name == "Logical_gate") ((Logical_gate)gate).Name_hidden(hideNamesToolStripMenuItem.Checked);
                if (gate.GetType().Name == "Input_gate") ((Input_gate)gate).Name_hidden(hideNamesToolStripMenuItem.Checked);
            }
            if (Gates_manager.current_edited != null) Gates_manager.current_edited.Names_hidden(hideNamesToolStripMenuItem.Checked);
        }

        private void clearBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Gates_manager.current_edited != null)
            {
                Gates_manager.Context_menu_options(true);
                Gates_manager.Gates_Enabled(true);
                Gates_manager.current_edited = null;
            }
            Gates_manager.Clear_board();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help_window.ShowDialog();
        }
    }
}
