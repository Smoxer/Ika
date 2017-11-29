namespace Launcher
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.login = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.userName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.country = new System.Windows.Forms.ComboBox();
            this.password = new System.Windows.Forms.TextBox();
            this.server = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.proxy = new System.Windows.Forms.TextBox();
            this.torr = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ua = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.register = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.newEmail = new System.Windows.Forms.TextBox();
            this.autoActive = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.newTor = new System.Windows.Forms.CheckBox();
            this.newProxy = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.newUserName = new System.Windows.Forms.TextBox();
            this.newPassword = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.newUA = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.invite = new System.Windows.Forms.TextBox();
            this.howmany = new System.Windows.Forms.NumericUpDown();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.SelectFile = new System.Windows.Forms.Button();
            this.ExportUsers = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.users = new System.Windows.Forms.ListView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.news = new System.Windows.Forms.WebBrowser();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.captchaPwd = new System.Windows.Forms.Label();
            this.apiSource = new System.Windows.Forms.TextBox();
            this.apiKey = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.captcha = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pushBulletToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accessTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getAccessTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eMailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.howmany)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // login
            // 
            this.login.AutoSize = true;
            this.login.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.login.Location = new System.Drawing.Point(3, 245);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(361, 23);
            this.login.TabIndex = 0;
            this.login.Text = "Login";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "User name";
            // 
            // userName
            // 
            this.userName.Location = new System.Drawing.Point(145, 3);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(124, 20);
            this.userName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Server";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Country";
            // 
            // country
            // 
            this.country.FormattingEnabled = true;
            this.country.Location = new System.Drawing.Point(145, 55);
            this.country.Name = "country";
            this.country.Size = new System.Drawing.Size(124, 21);
            this.country.TabIndex = 9;
            this.country.SelectedIndexChanged += new System.EventHandler(this.country_SelectedIndexChanged);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(145, 29);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(124, 20);
            this.password.TabIndex = 10;
            // 
            // server
            // 
            this.server.FormattingEnabled = true;
            this.server.Location = new System.Drawing.Point(145, 82);
            this.server.Name = "server";
            this.server.Size = new System.Drawing.Size(124, 21);
            this.server.TabIndex = 11;
            this.server.SelectedIndexChanged += new System.EventHandler(this.server_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.42857F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.57143F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.userName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.password, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.ua, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.server, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.country, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(361, 236);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.36044F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.63957F));
            this.tableLayoutPanel5.Controls.Add(this.proxy, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.torr, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(142, 106);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(219, 26);
            this.tableLayoutPanel5.TabIndex = 13;
            // 
            // proxy
            // 
            this.proxy.Dock = System.Windows.Forms.DockStyle.Left;
            this.proxy.Location = new System.Drawing.Point(3, 3);
            this.proxy.Name = "proxy";
            this.proxy.Size = new System.Drawing.Size(123, 20);
            this.proxy.TabIndex = 14;
            // 
            // torr
            // 
            this.torr.AutoSize = true;
            this.torr.Location = new System.Drawing.Point(132, 3);
            this.torr.Name = "torr";
            this.torr.Size = new System.Drawing.Size(42, 17);
            this.torr.TabIndex = 15;
            this.torr.Text = "Tor";
            this.torr.UseVisualStyleBackColor = true;
            this.torr.CheckedChanged += new System.EventHandler(this.torr_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Proxy";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "User Agent";
            // 
            // ua
            // 
            this.ua.Location = new System.Drawing.Point(145, 135);
            this.ua.Name = "ua";
            this.ua.Size = new System.Drawing.Size(124, 20);
            this.ua.TabIndex = 19;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 93);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(375, 297);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 13;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.login);
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(367, 271);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Login";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.register);
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(367, 271);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Register";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // register
            // 
            this.register.AutoSize = true;
            this.register.Dock = System.Windows.Forms.DockStyle.Top;
            this.register.Location = new System.Drawing.Point(3, 247);
            this.register.Name = "register";
            this.register.Size = new System.Drawing.Size(361, 23);
            this.register.TabIndex = 1;
            this.register.Text = "Register";
            this.register.UseVisualStyleBackColor = true;
            this.register.Click += new System.EventHandler(this.register_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.43F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.57F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label14, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.newUserName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.newPassword, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.newUA, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.invite, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.howmany, 1, 6);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28428F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28428F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28428F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28428F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28428F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28428F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.29429F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(361, 244);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel6.Controls.Add(this.newEmail, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.autoActive, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(142, 68);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(219, 34);
            this.tableLayoutPanel6.TabIndex = 16;
            // 
            // newEmail
            // 
            this.newEmail.Location = new System.Drawing.Point(3, 3);
            this.newEmail.Name = "newEmail";
            this.newEmail.Size = new System.Drawing.Size(124, 20);
            this.newEmail.TabIndex = 14;
            // 
            // autoActive
            // 
            this.autoActive.AutoSize = true;
            this.autoActive.Location = new System.Drawing.Point(139, 3);
            this.autoActive.Name = "autoActive";
            this.autoActive.Size = new System.Drawing.Size(77, 17);
            this.autoActive.TabIndex = 15;
            this.autoActive.Text = "Active mail";
            this.autoActive.UseVisualStyleBackColor = true;
            this.autoActive.CheckedChanged += new System.EventHandler(this.autoActive_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.newTor);
            this.panel2.Controls.Add(this.newProxy);
            this.panel2.Location = new System.Drawing.Point(145, 105);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(178, 25);
            this.panel2.TabIndex = 14;
            // 
            // newTor
            // 
            this.newTor.AutoSize = true;
            this.newTor.Dock = System.Windows.Forms.DockStyle.Right;
            this.newTor.Location = new System.Drawing.Point(136, 0);
            this.newTor.Name = "newTor";
            this.newTor.Size = new System.Drawing.Size(42, 25);
            this.newTor.TabIndex = 22;
            this.newTor.Text = "Tor";
            this.newTor.UseVisualStyleBackColor = true;
            this.newTor.CheckedChanged += new System.EventHandler(this.newTor_CheckedChanged);
            // 
            // newProxy
            // 
            this.newProxy.Dock = System.Windows.Forms.DockStyle.Left;
            this.newProxy.Location = new System.Drawing.Point(0, 0);
            this.newProxy.Name = "newProxy";
            this.newProxy.Size = new System.Drawing.Size(124, 20);
            this.newProxy.TabIndex = 21;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 204);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(130, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "How many users to create";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 136);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "User Agent";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "New password";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "New user name";
            // 
            // newUserName
            // 
            this.newUserName.Location = new System.Drawing.Point(145, 3);
            this.newUserName.Name = "newUserName";
            this.newUserName.Size = new System.Drawing.Size(124, 20);
            this.newUserName.TabIndex = 5;
            // 
            // newPassword
            // 
            this.newPassword.Location = new System.Drawing.Point(145, 37);
            this.newPassword.Name = "newPassword";
            this.newPassword.PasswordChar = '*';
            this.newPassword.Size = new System.Drawing.Size(124, 20);
            this.newPassword.TabIndex = 12;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 68);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "New EMail";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(129, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Proxy (Leave empty if not)";
            // 
            // newUA
            // 
            this.newUA.Location = new System.Drawing.Point(145, 139);
            this.newUA.Name = "newUA";
            this.newUA.Size = new System.Drawing.Size(124, 20);
            this.newUA.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 170);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 13);
            this.label13.TabIndex = 24;
            this.label13.Text = "Invite URL";
            // 
            // invite
            // 
            this.invite.Location = new System.Drawing.Point(145, 173);
            this.invite.Name = "invite";
            this.invite.Size = new System.Drawing.Size(124, 20);
            this.invite.TabIndex = 27;
            // 
            // howmany
            // 
            this.howmany.Location = new System.Drawing.Point(145, 207);
            this.howmany.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.howmany.Name = "howmany";
            this.howmany.Size = new System.Drawing.Size(120, 20);
            this.howmany.TabIndex = 28;
            this.howmany.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel7);
            this.tabPage3.Controls.Add(this.users);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(367, 271);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Massive piracy use";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel8, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.button2, 0, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 210);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(367, 61);
            this.tableLayoutPanel7.TabIndex = 22;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.SelectFile, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.ExportUsers, 2, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(367, 30);
            this.tableLayoutPanel8.TabIndex = 23;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SelectFile
            // 
            this.SelectFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectFile.Location = new System.Drawing.Point(125, 3);
            this.SelectFile.Name = "SelectFile";
            this.SelectFile.Size = new System.Drawing.Size(116, 24);
            this.SelectFile.TabIndex = 1;
            this.SelectFile.Text = "Import users file";
            this.SelectFile.UseVisualStyleBackColor = true;
            this.SelectFile.Click += new System.EventHandler(this.SelectFile_Click);
            // 
            // ExportUsers
            // 
            this.ExportUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExportUsers.Location = new System.Drawing.Point(247, 3);
            this.ExportUsers.Name = "ExportUsers";
            this.ExportUsers.Size = new System.Drawing.Size(117, 24);
            this.ExportUsers.TabIndex = 2;
            this.ExportUsers.Text = "Export users file";
            this.ExportUsers.UseVisualStyleBackColor = true;
            this.ExportUsers.Click += new System.EventHandler(this.ExportUsers_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(3, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(361, 25);
            this.button2.TabIndex = 1;
            this.button2.Text = "Login";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // users
            // 
            this.users.Dock = System.Windows.Forms.DockStyle.Top;
            this.users.FullRowSelect = true;
            this.users.HideSelection = false;
            this.users.Location = new System.Drawing.Point(0, 0);
            this.users.Name = "users";
            this.users.Size = new System.Drawing.Size(367, 207);
            this.users.TabIndex = 21;
            this.users.UseCompatibleStateImageBehavior = false;
            this.users.ItemActivate += new System.EventHandler(this.users_ItemActivate);
            this.users.MouseClick += new System.Windows.Forms.MouseEventHandler(this.users_MouseClick);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.news);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(367, 271);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "News";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // news
            // 
            this.news.Dock = System.Windows.Forms.DockStyle.Fill;
            this.news.Location = new System.Drawing.Point(0, 0);
            this.news.MinimumSize = new System.Drawing.Size(20, 20);
            this.news.Name = "news";
            this.news.Size = new System.Drawing.Size(367, 271);
            this.news.TabIndex = 0;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 1;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel10, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.85787F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.14214F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(375, 390);
            this.tableLayoutPanel9.TabIndex = 14;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.53333F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.46667F));
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.captcha, 1, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(375, 93);
            this.tableLayoutPanel10.TabIndex = 14;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.60839F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.39161F));
            this.tableLayoutPanel3.Controls.Add(this.captchaPwd, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.apiSource, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.apiKey, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(286, 93);
            this.tableLayoutPanel3.TabIndex = 13;
            // 
            // captchaPwd
            // 
            this.captchaPwd.AutoSize = true;
            this.captchaPwd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captchaPwd.Location = new System.Drawing.Point(3, 46);
            this.captchaPwd.Name = "captchaPwd";
            this.captchaPwd.Size = new System.Drawing.Size(112, 47);
            this.captchaPwd.TabIndex = 21;
            this.captchaPwd.Text = "Captcha source or password";
            // 
            // apiSource
            // 
            this.apiSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.apiSource.Location = new System.Drawing.Point(121, 49);
            this.apiSource.Name = "apiSource";
            this.apiSource.PasswordChar = '*';
            this.apiSource.Size = new System.Drawing.Size(162, 20);
            this.apiSource.TabIndex = 23;
            // 
            // apiKey
            // 
            this.apiKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.apiKey.Location = new System.Drawing.Point(121, 3);
            this.apiKey.Name = "apiKey";
            this.apiKey.Size = new System.Drawing.Size(162, 20);
            this.apiKey.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 46);
            this.label6.TabIndex = 19;
            this.label6.Text = "Captcha key or name";
            // 
            // captcha
            // 
            this.captcha.Dock = System.Windows.Forms.DockStyle.Top;
            this.captcha.FormattingEnabled = true;
            this.captcha.Items.AddRange(new object[] {
            "9KW",
            "DeCaptcher",
            "DeathByCaptcha",
            "Manual"});
            this.captcha.Location = new System.Drawing.Point(289, 3);
            this.captcha.Name = "captcha";
            this.captcha.Size = new System.Drawing.Size(83, 21);
            this.captcha.TabIndex = 14;
            this.captcha.SelectedIndexChanged += new System.EventHandler(this.captcha_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.pushBulletToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(375, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageToolStripMenuItem,
            this.licenseToolStripMenuItem,
            this.sendLogToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.languageToolStripMenuItem.Text = "Language";
            // 
            // licenseToolStripMenuItem
            // 
            this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
            this.licenseToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.licenseToolStripMenuItem.Text = "License";
            this.licenseToolStripMenuItem.Click += new System.EventHandler(this.licenseToolStripMenuItem_Click);
            // 
            // sendLogToolStripMenuItem
            // 
            this.sendLogToolStripMenuItem.Name = "sendLogToolStripMenuItem";
            this.sendLogToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.sendLogToolStripMenuItem.Text = "Send log";
            this.sendLogToolStripMenuItem.Click += new System.EventHandler(this.sendLogToolStripMenuItem_Click);
            // 
            // pushBulletToolStripMenuItem
            // 
            this.pushBulletToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accessTokenToolStripMenuItem,
            this.getAccessTokenToolStripMenuItem});
            this.pushBulletToolStripMenuItem.Name = "pushBulletToolStripMenuItem";
            this.pushBulletToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.pushBulletToolStripMenuItem.Text = "Pushbullet";
            // 
            // accessTokenToolStripMenuItem
            // 
            this.accessTokenToolStripMenuItem.Name = "accessTokenToolStripMenuItem";
            this.accessTokenToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.accessTokenToolStripMenuItem.Text = "Set Access token";
            this.accessTokenToolStripMenuItem.Click += new System.EventHandler(this.accessTokenToolStripMenuItem_Click);
            // 
            // getAccessTokenToolStripMenuItem
            // 
            this.getAccessTokenToolStripMenuItem.Name = "getAccessTokenToolStripMenuItem";
            this.getAccessTokenToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.getAccessTokenToolStripMenuItem.Text = "Get Access token";
            this.getAccessTokenToolStripMenuItem.Click += new System.EventHandler(this.getAccessTokenToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eMailToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // eMailToolStripMenuItem
            // 
            this.eMailToolStripMenuItem.Name = "eMailToolStripMenuItem";
            this.eMailToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.eMailToolStripMenuItem.Text = "EMail";
            this.eMailToolStripMenuItem.Click += new System.EventHandler(this.eMailToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 414);
            this.Controls.Add(this.tableLayoutPanel9);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(391, 429);
            this.Name = "Form1";
            this.Text = "Ikariam+ launcher";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.howmany)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button login;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox userName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox country;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.ComboBox server;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox newUserName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox newPassword;
        private System.Windows.Forms.TextBox newEmail;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button register;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox newProxy;
        private System.Windows.Forms.TextBox newUA;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox invite;
        private System.Windows.Forms.NumericUpDown howmany;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox newTor;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView users;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Button SelectFile;
        private System.Windows.Forms.Button ExportUsers;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TextBox proxy;
        private System.Windows.Forms.CheckBox torr;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ua;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.TextBox apiSource;
        private System.Windows.Forms.TextBox apiKey;
        private System.Windows.Forms.Label captchaPwd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.WebBrowser news;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pushBulletToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accessTokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getAccessTokenToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.CheckBox autoActive;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eMailToolStripMenuItem;
        private System.Windows.Forms.ComboBox captcha;
    }
}

