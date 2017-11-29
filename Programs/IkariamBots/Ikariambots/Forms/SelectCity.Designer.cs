namespace IkariamBots.Forms
{
    partial class SelectCity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectCity));
            this.citiesList = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // citiesList
            // 
            this.citiesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.citiesList.FullRowSelect = true;
            this.citiesList.HideSelection = false;
            this.citiesList.Location = new System.Drawing.Point(0, 0);
            this.citiesList.MultiSelect = false;
            this.citiesList.Name = "citiesList";
            this.citiesList.Size = new System.Drawing.Size(373, 241);
            this.citiesList.TabIndex = 0;
            this.citiesList.UseCompatibleStateImageBehavior = false;
            this.citiesList.ItemActivate += new System.EventHandler(this.citiesList_ItemActivate);
            this.citiesList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.citiesList_MouseClick);
            // 
            // SelectCity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 241);
            this.Controls.Add(this.citiesList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectCity";
            this.Text = "Select city";
            this.Load += new System.EventHandler(this.SelectCity_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView citiesList;
    }
}