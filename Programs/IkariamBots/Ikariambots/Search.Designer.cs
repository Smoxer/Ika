namespace IkariamBots
{
    partial class Search
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Search));
            this.value = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.allyOption = new System.Windows.Forms.RadioButton();
            this.userNameOption = new System.Windows.Forms.RadioButton();
            this.cityNameOption = new System.Windows.Forms.RadioButton();
            this.custom = new System.Windows.Forms.RadioButton();
            this.close = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.log = new System.Windows.Forms.ListBox();
            this.useCache = new System.Windows.Forms.CheckBox();
            this.speed = new System.Windows.Forms.Label();
            this.simultaneously = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simultaneously)).BeginInit();
            this.SuspendLayout();
            // 
            // value
            // 
            this.value.Location = new System.Drawing.Point(169, 40);
            this.value.Name = "value";
            this.value.Size = new System.Drawing.Size(100, 20);
            this.value.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search by";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.allyOption, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.userNameOption, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cityNameOption, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.custom, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(74, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(84, 103);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // allyOption
            // 
            this.allyOption.AutoSize = true;
            this.allyOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allyOption.Location = new System.Drawing.Point(3, 3);
            this.allyOption.Name = "allyOption";
            this.allyOption.Size = new System.Drawing.Size(78, 19);
            this.allyOption.TabIndex = 0;
            this.allyOption.TabStop = true;
            this.allyOption.Text = "Ally";
            this.allyOption.UseVisualStyleBackColor = true;
            this.allyOption.CheckedChanged += new System.EventHandler(this.allyOption_CheckedChanged);
            // 
            // userNameOption
            // 
            this.userNameOption.AutoSize = true;
            this.userNameOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userNameOption.Location = new System.Drawing.Point(3, 28);
            this.userNameOption.Name = "userNameOption";
            this.userNameOption.Size = new System.Drawing.Size(78, 19);
            this.userNameOption.TabIndex = 1;
            this.userNameOption.TabStop = true;
            this.userNameOption.Text = "User name";
            this.userNameOption.UseVisualStyleBackColor = true;
            this.userNameOption.CheckedChanged += new System.EventHandler(this.userNameOption_CheckedChanged);
            // 
            // cityNameOption
            // 
            this.cityNameOption.AutoSize = true;
            this.cityNameOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cityNameOption.Location = new System.Drawing.Point(3, 53);
            this.cityNameOption.Name = "cityNameOption";
            this.cityNameOption.Size = new System.Drawing.Size(78, 19);
            this.cityNameOption.TabIndex = 2;
            this.cityNameOption.TabStop = true;
            this.cityNameOption.Text = "City name";
            this.cityNameOption.UseVisualStyleBackColor = true;
            this.cityNameOption.CheckedChanged += new System.EventHandler(this.cityNameOption_CheckedChanged);
            // 
            // custom
            // 
            this.custom.AutoSize = true;
            this.custom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.custom.Location = new System.Drawing.Point(3, 78);
            this.custom.Name = "custom";
            this.custom.Size = new System.Drawing.Size(78, 22);
            this.custom.TabIndex = 3;
            this.custom.TabStop = true;
            this.custom.Text = "Custom";
            this.custom.UseVisualStyleBackColor = true;
            this.custom.CheckedChanged += new System.EventHandler(this.custom_CheckedChanged);
            // 
            // close
            // 
            this.close.Dock = System.Windows.Forms.DockStyle.Fill;
            this.close.Location = new System.Drawing.Point(3, 3);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(121, 23);
            this.close.TabIndex = 3;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // start
            // 
            this.start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.start.Location = new System.Drawing.Point(130, 3);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(121, 23);
            this.start.TabIndex = 4;
            this.start.Text = "Search";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.close, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.start, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(15, 145);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(254, 29);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(15, 302);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(254, 23);
            this.progress.TabIndex = 6;
            // 
            // log
            // 
            this.log.FormattingEnabled = true;
            this.log.Location = new System.Drawing.Point(15, 180);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(254, 95);
            this.log.TabIndex = 7;
            // 
            // useCache
            // 
            this.useCache.AutoSize = true;
            this.useCache.Location = new System.Drawing.Point(15, 122);
            this.useCache.Name = "useCache";
            this.useCache.Size = new System.Drawing.Size(150, 17);
            this.useCache.TabIndex = 8;
            this.useCache.Text = "Use from cache if possible";
            this.useCache.UseVisualStyleBackColor = true;
            // 
            // speed
            // 
            this.speed.AutoSize = true;
            this.speed.Location = new System.Drawing.Point(15, 282);
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(44, 13);
            this.speed.TabIndex = 9;
            this.speed.Text = "Speed: ";
            // 
            // simultaneously
            // 
            this.simultaneously.Location = new System.Drawing.Point(169, 119);
            this.simultaneously.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.simultaneously.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.simultaneously.Name = "simultaneously";
            this.simultaneously.Size = new System.Drawing.Size(100, 20);
            this.simultaneously.TabIndex = 10;
            this.simultaneously.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "simultaneously";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Location = new System.Drawing.Point(169, 12);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(23, 13);
            this.title.TabIndex = 12;
            this.title.Text = "title";
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 331);
            this.Controls.Add(this.title);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.simultaneously);
            this.Controls.Add(this.speed);
            this.Controls.Add(this.useCache);
            this.Controls.Add(this.log);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.value);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 336);
            this.Name = "Search";
            this.Text = "Search";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.simultaneously)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox value;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton allyOption;
        private System.Windows.Forms.RadioButton userNameOption;
        private System.Windows.Forms.RadioButton cityNameOption;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.ListBox log;
        private System.Windows.Forms.CheckBox useCache;
        private System.Windows.Forms.Label speed;
        private System.Windows.Forms.NumericUpDown simultaneously;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton custom;
        private System.Windows.Forms.Label title;
    }
}