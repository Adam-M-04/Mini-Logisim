
namespace Logic_gate_simulator
{
    partial class Starting_window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Starting_window));
            this.label1 = new System.Windows.Forms.Label();
            this.Create_button = new System.Windows.Forms.Button();
            this.Open_button = new System.Windows.Forms.Button();
            this.Project_textbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(78, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(365, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "Logic Gate Simulator";
            // 
            // Create_button
            // 
            this.Create_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.Create_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Create_button.Location = new System.Drawing.Point(12, 184);
            this.Create_button.Name = "Create_button";
            this.Create_button.Size = new System.Drawing.Size(252, 63);
            this.Create_button.TabIndex = 1;
            this.Create_button.Text = "Create";
            this.Create_button.UseVisualStyleBackColor = false;
            this.Create_button.Click += new System.EventHandler(this.Create_button_Click);
            // 
            // Open_button
            // 
            this.Open_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.Open_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Open_button.Location = new System.Drawing.Point(270, 184);
            this.Open_button.Name = "Open_button";
            this.Open_button.Size = new System.Drawing.Size(252, 63);
            this.Open_button.TabIndex = 2;
            this.Open_button.Text = "Open";
            this.Open_button.UseVisualStyleBackColor = false;
            this.Open_button.Click += new System.EventHandler(this.Open_button_Click);
            // 
            // Project_textbox
            // 
            this.Project_textbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.Project_textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Project_textbox.Location = new System.Drawing.Point(12, 128);
            this.Project_textbox.MaxLength = 50;
            this.Project_textbox.Name = "Project_textbox";
            this.Project_textbox.Size = new System.Drawing.Size(510, 38);
            this.Project_textbox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(22, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(478, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "Enter name for new project or open existing";
            // 
            // Starting_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(181)))), ((int)(((byte)(189)))));
            this.ClientSize = new System.Drawing.Size(533, 259);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Project_textbox);
            this.Controls.Add(this.Open_button);
            this.Controls.Add(this.Create_button);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Starting_window";
            this.Text = "Logic Gate Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Close_handler);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Create_button;
        private System.Windows.Forms.Button Open_button;
        private System.Windows.Forms.TextBox Project_textbox;
        private System.Windows.Forms.Label label2;
    }
}