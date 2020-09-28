namespace Traffic_Booster
{
    partial class CodeGeneratorForm
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
            this.LstbModel = new System.Windows.Forms.ListBox();
            this.BrnChooseFile = new System.Windows.Forms.Button();
            this.BtnGetCodes = new System.Windows.Forms.Button();
            this.TxtProxy = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LstbModel
            // 
            this.LstbModel.FormattingEnabled = true;
            this.LstbModel.Location = new System.Drawing.Point(12, 12);
            this.LstbModel.Name = "LstbModel";
            this.LstbModel.Size = new System.Drawing.Size(212, 225);
            this.LstbModel.TabIndex = 0;
            // 
            // BrnChooseFile
            // 
            this.BrnChooseFile.Location = new System.Drawing.Point(12, 273);
            this.BrnChooseFile.Name = "BrnChooseFile";
            this.BrnChooseFile.Size = new System.Drawing.Size(75, 23);
            this.BrnChooseFile.TabIndex = 1;
            this.BrnChooseFile.Text = "Choose File";
            this.BrnChooseFile.UseVisualStyleBackColor = true;
            this.BrnChooseFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnGetCodes
            // 
            this.BtnGetCodes.Location = new System.Drawing.Point(149, 273);
            this.BtnGetCodes.Name = "BtnGetCodes";
            this.BtnGetCodes.Size = new System.Drawing.Size(75, 23);
            this.BtnGetCodes.TabIndex = 2;
            this.BtnGetCodes.Text = "Get Codes";
            this.BtnGetCodes.UseVisualStyleBackColor = true;
            this.BtnGetCodes.Click += new System.EventHandler(this.button2_Click);
            // 
            // TxtProxy
            // 
            this.TxtProxy.Location = new System.Drawing.Point(12, 243);
            this.TxtProxy.Name = "TxtProxy";
            this.TxtProxy.Size = new System.Drawing.Size(212, 20);
            this.TxtProxy.TabIndex = 3;
            // 
            // CodeGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 314);
            this.Controls.Add(this.TxtProxy);
            this.Controls.Add(this.BtnGetCodes);
            this.Controls.Add(this.BrnChooseFile);
            this.Controls.Add(this.LstbModel);
            this.Name = "CodeGeneratorForm";
            this.Text = "CodeGeneratorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodeGeneratorForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LstbModel;
        private System.Windows.Forms.Button BrnChooseFile;
        private System.Windows.Forms.Button BtnGetCodes;
        private System.Windows.Forms.TextBox TxtProxy;
    }
}