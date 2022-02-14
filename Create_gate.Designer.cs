
namespace Logic_gate_simulator
{
    partial class Create_gate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Create_gate));
            this.Gate_name_textbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.color_selector = new System.Windows.Forms.ColorDialog();
            this.create_button = new System.Windows.Forms.Button();
            this.label_selected_color = new System.Windows.Forms.Label();
            this.select_color = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Gate_name_textbox
            // 
            this.Gate_name_textbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.Gate_name_textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Gate_name_textbox.Location = new System.Drawing.Point(15, 77);
            this.Gate_name_textbox.MaxLength = 10;
            this.Gate_name_textbox.Name = "Gate_name_textbox";
            this.Gate_name_textbox.Size = new System.Drawing.Size(324, 38);
            this.Gate_name_textbox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(127, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(134, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "Color";
            // 
            // create_button
            // 
            this.create_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.create_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.create_button.Location = new System.Drawing.Point(15, 297);
            this.create_button.Name = "create_button";
            this.create_button.Size = new System.Drawing.Size(327, 62);
            this.create_button.TabIndex = 3;
            this.create_button.Text = "Create";
            this.create_button.UseVisualStyleBackColor = false;
            this.create_button.Click += new System.EventHandler(this.create_button_Click);
            // 
            // label_selected_color
            // 
            this.label_selected_color.Location = new System.Drawing.Point(187, 204);
            this.label_selected_color.Name = "label_selected_color";
            this.label_selected_color.Size = new System.Drawing.Size(152, 51);
            this.label_selected_color.TabIndex = 4;
            // 
            // select_color
            // 
            this.select_color.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.select_color.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.select_color.Location = new System.Drawing.Point(15, 204);
            this.select_color.Name = "select_color";
            this.select_color.Size = new System.Drawing.Size(148, 51);
            this.select_color.TabIndex = 5;
            this.select_color.Text = "Select";
            this.select_color.UseVisualStyleBackColor = false;
            this.select_color.Click += new System.EventHandler(this.select_color_Click);
            // 
            // Create_gate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(181)))), ((int)(((byte)(189)))));
            this.ClientSize = new System.Drawing.Size(351, 371);
            this.Controls.Add(this.select_color);
            this.Controls.Add(this.label_selected_color);
            this.Controls.Add(this.create_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Gate_name_textbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Create_gate";
            this.ShowInTaskbar = false;
            this.Text = "Create New Gate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Gate_name_textbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog color_selector;
        private System.Windows.Forms.Button create_button;
        private System.Windows.Forms.Label label_selected_color;
        private System.Windows.Forms.Button select_color;
    }
}