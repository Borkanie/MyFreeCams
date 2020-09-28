namespace Traffic_Booster
{
    partial class Consola_Form
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
            this.Hide_button = new System.Windows.Forms.Button();
            this.textbox = new System.Windows.Forms.TextBox();
            this.listbox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Hide_button
            // 
            this.Hide_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Hide_button.Location = new System.Drawing.Point(792, 386);
            this.Hide_button.Name = "Hide_button";
            this.Hide_button.Size = new System.Drawing.Size(104, 31);
            this.Hide_button.TabIndex = 7;
            this.Hide_button.Text = "Hide";
            this.Hide_button.UseVisualStyleBackColor = true;
            this.Hide_button.Click += new System.EventHandler(this.Hide_button_Click);
            // 
            // textbox
            // 
            this.textbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textbox.Location = new System.Drawing.Point(12, 386);
            this.textbox.Name = "textbox";
            this.textbox.Size = new System.Drawing.Size(774, 31);
            this.textbox.TabIndex = 5;
            // 
            // listbox
            // 
            this.listbox.Dock = System.Windows.Forms.DockStyle.Left;
            this.listbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listbox.FormattingEnabled = true;
            this.listbox.ItemHeight = 20;
            this.listbox.Location = new System.Drawing.Point(0, 0);
            this.listbox.MaximumSize = new System.Drawing.Size(1024, 370);
            this.listbox.Name = "listbox";
            this.listbox.Size = new System.Drawing.Size(884, 370);
            this.listbox.TabIndex = 4;
            // 
            // Consola_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 429);
            this.Controls.Add(this.Hide_button);
            this.Controls.Add(this.textbox);
            this.Controls.Add(this.listbox);
            this.KeyPreview = true;
            this.Name = "Consola_Form";
            this.Text = "Consola";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Consola_Form_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Consola_Form_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Hide_button;
        private System.Windows.Forms.TextBox textbox;
        private System.Windows.Forms.ListBox listbox;
    }
}