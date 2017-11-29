namespace IkariamBots
{
    partial class AutoRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoRegister));
            this.log = new System.Windows.Forms.ListBox();
            this.register = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // log
            // 
            this.log.Dock = System.Windows.Forms.DockStyle.Top;
            this.log.FormattingEnabled = true;
            this.log.Location = new System.Drawing.Point(0, 0);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(284, 134);
            this.log.TabIndex = 0;
            this.log.SelectedIndexChanged += new System.EventHandler(this.log_SelectedIndexChanged);
            // 
            // register
            // 
            this.register.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.register.Location = new System.Drawing.Point(0, 170);
            this.register.Name = "register";
            this.register.Size = new System.Drawing.Size(284, 23);
            this.register.TabIndex = 1;
            this.register.Text = "Register";
            this.register.UseVisualStyleBackColor = true;
            this.register.Click += new System.EventHandler(this.register_Click);
            // 
            // AutoRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 193);
            this.Controls.Add(this.register);
            this.Controls.Add(this.log);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 232);
            this.Name = "AutoRegister";
            this.Text = "AutoRegister";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoRegister_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox log;
        private System.Windows.Forms.Button register;
    }
}