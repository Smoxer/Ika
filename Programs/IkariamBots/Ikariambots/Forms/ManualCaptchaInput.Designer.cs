namespace IkariamBots.Forms
{
    partial class ManualCaptchaInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManualCaptchaInput));
            this.captcha = new System.Windows.Forms.PictureBox();
            this.captchaValue = new System.Windows.Forms.TextBox();
            this.send = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.captcha)).BeginInit();
            this.SuspendLayout();
            // 
            // captcha
            // 
            this.captcha.Dock = System.Windows.Forms.DockStyle.Top;
            this.captcha.Location = new System.Drawing.Point(0, 0);
            this.captcha.Name = "captcha";
            this.captcha.Size = new System.Drawing.Size(284, 60);
            this.captcha.TabIndex = 0;
            this.captcha.TabStop = false;
            // 
            // captchaValue
            // 
            this.captchaValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.captchaValue.Location = new System.Drawing.Point(0, 60);
            this.captchaValue.Name = "captchaValue";
            this.captchaValue.Size = new System.Drawing.Size(284, 20);
            this.captchaValue.TabIndex = 1;
            // 
            // send
            // 
            this.send.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.send.Location = new System.Drawing.Point(0, 82);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(284, 23);
            this.send.TabIndex = 2;
            this.send.Text = "Send captcha";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // ManualCaptchaInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 105);
            this.Controls.Add(this.send);
            this.Controls.Add(this.captchaValue);
            this.Controls.Add(this.captcha);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 144);
            this.Name = "ManualCaptchaInput";
            this.Text = "Captcha detected";
            ((System.ComponentModel.ISupportInitialize)(this.captcha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox captcha;
        private System.Windows.Forms.TextBox captchaValue;
        private System.Windows.Forms.Button send;
    }
}