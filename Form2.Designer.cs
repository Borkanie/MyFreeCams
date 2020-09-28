namespace Traffic_Booster
{
    partial class Recaptcha_Key_Form
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
            this.recaptcha_textbox = new System.Windows.Forms.TextBox();
            this.recaptcha_label = new MetroFramework.Controls.MetroLabel();
            this.cancel_button = new MetroFramework.Controls.MetroButton();
            this.done_button = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // recaptcha_textbox
            // 
            this.recaptcha_textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recaptcha_textbox.Location = new System.Drawing.Point(191, 34);
            this.recaptcha_textbox.Name = "recaptcha_textbox";
            this.recaptcha_textbox.Size = new System.Drawing.Size(448, 31);
            this.recaptcha_textbox.TabIndex = 15;
            // 
            // recaptcha_label
            // 
            this.recaptcha_label.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.recaptcha_label.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.recaptcha_label.Location = new System.Drawing.Point(12, 12);
            this.recaptcha_label.Name = "recaptcha_label";
            this.recaptcha_label.Size = new System.Drawing.Size(173, 53);
            this.recaptcha_label.TabIndex = 16;
            this.recaptcha_label.Text = "Put your recaptcha \r\nkey in here:";
            // 
            // cancel_button
            // 
            this.cancel_button.BackColor = System.Drawing.Color.Red;
            this.cancel_button.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.cancel_button.Location = new System.Drawing.Point(437, 87);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(119, 29);
            this.cancel_button.TabIndex = 17;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseCustomBackColor = true;
            this.cancel_button.UseCustomForeColor = true;
            this.cancel_button.UseSelectable = true;
            this.cancel_button.UseStyleColors = true;
            this.cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // done_button
            // 
            this.done_button.BackColor = System.Drawing.Color.Green;
            this.done_button.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.done_button.Location = new System.Drawing.Point(78, 87);
            this.done_button.Name = "done_button";
            this.done_button.Size = new System.Drawing.Size(119, 29);
            this.done_button.Style = MetroFramework.MetroColorStyle.Green;
            this.done_button.TabIndex = 18;
            this.done_button.Text = "Done";
            this.done_button.UseCustomBackColor = true;
            this.done_button.UseCustomForeColor = true;
            this.done_button.UseSelectable = true;
            this.done_button.UseStyleColors = true;
            this.done_button.Click += new System.EventHandler(this.Done_button_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 133);
            this.Controls.Add(this.done_button);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.recaptcha_label);
            this.Controls.Add(this.recaptcha_textbox);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox recaptcha_textbox;
        private MetroFramework.Controls.MetroLabel recaptcha_label;
        private MetroFramework.Controls.MetroButton cancel_button;
        private MetroFramework.Controls.MetroButton done_button;
    }
}