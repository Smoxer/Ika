namespace IkariamBots.Forms
{
    partial class SelectUsers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectUsers));
            this.users = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.raidList = new System.Windows.Forms.ListView();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cities = new System.Windows.Forms.ListView();
            this.Clear = new System.Windows.Forms.Button();
            this.repeatRaid = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // users
            // 
            this.users.FullRowSelect = true;
            this.users.HideSelection = false;
            this.users.Location = new System.Drawing.Point(12, 29);
            this.users.MultiSelect = false;
            this.users.Name = "users";
            this.users.Size = new System.Drawing.Size(130, 97);
            this.users.TabIndex = 0;
            this.users.UseCompatibleStateImageBehavior = false;
            this.users.ItemActivate += new System.EventHandler(this.users_ItemActivate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "All users:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Raid list:";
            // 
            // raidList
            // 
            this.raidList.FullRowSelect = true;
            this.raidList.HideSelection = false;
            this.raidList.Location = new System.Drawing.Point(12, 149);
            this.raidList.MultiSelect = false;
            this.raidList.Name = "raidList";
            this.raidList.Size = new System.Drawing.Size(290, 97);
            this.raidList.TabIndex = 3;
            this.raidList.UseCompatibleStateImageBehavior = false;
            this.raidList.ItemActivate += new System.EventHandler(this.raidList_ItemActivate);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(227, 280);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(12, 280);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Change city:";
            // 
            // cities
            // 
            this.cities.FullRowSelect = true;
            this.cities.HideSelection = false;
            this.cities.Location = new System.Drawing.Point(162, 29);
            this.cities.MultiSelect = false;
            this.cities.Name = "cities";
            this.cities.Size = new System.Drawing.Size(140, 97);
            this.cities.TabIndex = 7;
            this.cities.UseCompatibleStateImageBehavior = false;
            this.cities.ItemActivate += new System.EventHandler(this.cities_ItemActivate);
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(118, 280);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(75, 23);
            this.Clear.TabIndex = 8;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // repeatRaid
            // 
            this.repeatRaid.AutoSize = true;
            this.repeatRaid.Location = new System.Drawing.Point(13, 253);
            this.repeatRaid.Name = "repeatRaid";
            this.repeatRaid.Size = new System.Drawing.Size(61, 17);
            this.repeatRaid.TabIndex = 9;
            this.repeatRaid.Text = "Repeat";
            this.repeatRaid.UseVisualStyleBackColor = true;
            // 
            // SelectUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 306);
            this.Controls.Add(this.repeatRaid);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.cities);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.raidList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.users);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(330, 331);
            this.Name = "SelectUsers";
            this.Text = "Raid list";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView users;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView raidList;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView cities;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.CheckBox repeatRaid;
    }
}