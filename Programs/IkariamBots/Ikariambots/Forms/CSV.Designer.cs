namespace IkariamBots.Forms
{
    partial class CSV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CSV));
            this.users = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // users
            // 
            this.users.Dock = System.Windows.Forms.DockStyle.Fill;
            this.users.FullRowSelect = true;
            this.users.HideSelection = false;
            this.users.Location = new System.Drawing.Point(0, 0);
            this.users.MultiSelect = false;
            this.users.Name = "users";
            this.users.Size = new System.Drawing.Size(284, 261);
            this.users.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.users.TabIndex = 1;
            this.users.UseCompatibleStateImageBehavior = false;
            this.users.View = System.Windows.Forms.View.Details;
            // 
            // CSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.users);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CSV";
            this.Text = "CSV";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView users;
    }
}