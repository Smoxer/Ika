namespace IkariamBots
{
    partial class PiracyHide
    {

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PiracyHide));
            users = new System.Windows.Forms.ListView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.startAllSimultaneous = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // users
            // 
            users.Dock = System.Windows.Forms.DockStyle.Fill;
            users.FullRowSelect = true;
            users.HideSelection = false;
            users.Location = new System.Drawing.Point(0, 0);
            users.Margin = new System.Windows.Forms.Padding(0);
            users.MultiSelect = false;
            users.Name = "users";
            users.Size = new System.Drawing.Size(519, 374);
            users.TabIndex = 1;
            users.UseCompatibleStateImageBehavior = false;
            users.ItemActivate += new System.EventHandler(users_ItemActivate);
            users.MouseClick += new System.Windows.Forms.MouseEventHandler(users_MouseClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.startAllSimultaneous, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 374);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(519, 22);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // startAllSimultaneous
            // 
            this.startAllSimultaneous.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startAllSimultaneous.Location = new System.Drawing.Point(0, 0);
            this.startAllSimultaneous.Margin = new System.Windows.Forms.Padding(0);
            this.startAllSimultaneous.Name = "startAllSimultaneous";
            this.startAllSimultaneous.Size = new System.Drawing.Size(519, 22);
            this.startAllSimultaneous.TabIndex = 3;
            this.startAllSimultaneous.Text = "Start All Simultaneous";
            this.startAllSimultaneous.UseVisualStyleBackColor = true;
            this.startAllSimultaneous.Click += new System.EventHandler(this.startAllSimultaneous_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(users, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.5F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(519, 396);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // PiracyHide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 396);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(535, 420);
            this.Name = "PiracyHide";
            this.Text = "Ikariam+ - Piracy Bot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PiracyHide_FormClosing);
            this.Load += new System.EventHandler(this.PiracyHide_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button startAllSimultaneous;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private static System.Windows.Forms.ListView users;
    }
}