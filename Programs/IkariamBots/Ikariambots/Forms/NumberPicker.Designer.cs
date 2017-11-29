namespace IkariamBots.Forms
{
    partial class NumberPicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NumberPicker));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Trian = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Crew = new System.Windows.Forms.TrackBar();
            this.crewLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Crew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Trian, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Cancel, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 79);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 31);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Trian
            // 
            this.Trian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Trian.Location = new System.Drawing.Point(0, 0);
            this.Trian.Margin = new System.Windows.Forms.Padding(0);
            this.Trian.Name = "Trian";
            this.Trian.Size = new System.Drawing.Size(142, 31);
            this.Trian.TabIndex = 0;
            this.Trian.Text = "Train";
            this.Trian.UseVisualStyleBackColor = true;
            this.Trian.Click += new System.EventHandler(this.Trian_Click);
            // 
            // Cancel
            // 
            this.Cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Cancel.Location = new System.Drawing.Point(142, 0);
            this.Cancel.Margin = new System.Windows.Forms.Padding(0);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(142, 31);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Crew
            // 
            this.Crew.Dock = System.Windows.Forms.DockStyle.Top;
            this.Crew.Location = new System.Drawing.Point(0, 0);
            this.Crew.Name = "Crew";
            this.Crew.Size = new System.Drawing.Size(284, 45);
            this.Crew.TabIndex = 1;
            this.Crew.Scroll += new System.EventHandler(this.Crew_Scroll);
            // 
            // crewLabel
            // 
            this.crewLabel.AutoSize = true;
            this.crewLabel.Location = new System.Drawing.Point(53, 48);
            this.crewLabel.Name = "crewLabel";
            this.crewLabel.Size = new System.Drawing.Size(56, 13);
            this.crewLabel.TabIndex = 2;
            this.crewLabel.Text = "crewLabel";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::IkariamPlus.Resources.piratecrew;
            this.pictureBox1.InitialImage = global::IkariamPlus.Resources.piratecrew;
            this.pictureBox1.Location = new System.Drawing.Point(12, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 22);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // NumberPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 110);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.crewLabel);
            this.Controls.Add(this.Crew);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(300, 149);
            this.MinimumSize = new System.Drawing.Size(300, 149);
            this.Name = "NumberPicker";
            this.Text = "Train crew";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Crew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Trian;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TrackBar Crew;
        private System.Windows.Forms.Label crewLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}