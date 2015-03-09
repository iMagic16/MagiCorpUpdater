namespace MagicCorpUpdate_Packager
{
    partial class FrmMUPackage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMUPackage));
            this.BtnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.GpPackager = new System.Windows.Forms.GroupBox();
            this.TxtFilesToUpload = new System.Windows.Forms.TextBox();
            this.LblValid = new System.Windows.Forms.Label();
            this.TxtProgName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtPasswd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtUsername = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtHostname = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BtnSubmit = new System.Windows.Forms.Button();
            this.GpLogin = new System.Windows.Forms.GroupBox();
            this.LblVersion = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.GpPackager.SuspendLayout();
            this.GpLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Location = new System.Drawing.Point(407, 45);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(75, 20);
            this.BtnBrowse.TabIndex = 7;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File to upload: ";
            // 
            // GpPackager
            // 
            this.GpPackager.Controls.Add(this.TxtProgName);
            this.GpPackager.Controls.Add(this.label2);
            this.GpPackager.Controls.Add(this.LblValid);
            this.GpPackager.Controls.Add(this.TxtFilesToUpload);
            this.GpPackager.Controls.Add(this.BtnBrowse);
            this.GpPackager.Controls.Add(this.label1);
            this.GpPackager.Enabled = false;
            this.GpPackager.Location = new System.Drawing.Point(12, 151);
            this.GpPackager.Name = "GpPackager";
            this.GpPackager.Size = new System.Drawing.Size(562, 77);
            this.GpPackager.TabIndex = 2;
            this.GpPackager.TabStop = false;
            this.GpPackager.Text = "Packager";
            // 
            // TxtFilesToUpload
            // 
            this.TxtFilesToUpload.Location = new System.Drawing.Point(88, 45);
            this.TxtFilesToUpload.Name = "TxtFilesToUpload";
            this.TxtFilesToUpload.Size = new System.Drawing.Size(313, 20);
            this.TxtFilesToUpload.TabIndex = 6;
            // 
            // LblValid
            // 
            this.LblValid.AutoSize = true;
            this.LblValid.Location = new System.Drawing.Point(489, 49);
            this.LblValid.Name = "LblValid";
            this.LblValid.Size = new System.Drawing.Size(63, 13);
            this.LblValid.TabIndex = 3;
            this.LblValid.Text = "VAL/INVAL";
            // 
            // TxtProgName
            // 
            this.TxtProgName.Location = new System.Drawing.Point(88, 19);
            this.TxtProgName.Name = "TxtProgName";
            this.TxtProgName.Size = new System.Drawing.Size(313, 20);
            this.TxtProgName.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Program Name:";
            // 
            // TxtPasswd
            // 
            this.TxtPasswd.Location = new System.Drawing.Point(88, 79);
            this.TxtPasswd.Name = "TxtPasswd";
            this.TxtPasswd.Size = new System.Drawing.Size(313, 20);
            this.TxtPasswd.TabIndex = 3;
            this.TxtPasswd.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Password: ";
            // 
            // TxtUsername
            // 
            this.TxtUsername.Location = new System.Drawing.Point(88, 53);
            this.TxtUsername.Name = "TxtUsername";
            this.TxtUsername.Size = new System.Drawing.Size(313, 20);
            this.TxtUsername.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Username: ";
            // 
            // TxtHostname
            // 
            this.TxtHostname.Location = new System.Drawing.Point(88, 27);
            this.TxtHostname.Name = "TxtHostname";
            this.TxtHostname.Size = new System.Drawing.Size(313, 20);
            this.TxtHostname.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Hostname:";
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.Enabled = false;
            this.BtnSubmit.Location = new System.Drawing.Point(12, 234);
            this.BtnSubmit.Name = "BtnSubmit";
            this.BtnSubmit.Size = new System.Drawing.Size(562, 54);
            this.BtnSubmit.TabIndex = 8;
            this.BtnSubmit.Text = "Submit";
            this.BtnSubmit.UseVisualStyleBackColor = true;
            // 
            // GpLogin
            // 
            this.GpLogin.Controls.Add(this.button1);
            this.GpLogin.Controls.Add(this.label5);
            this.GpLogin.Controls.Add(this.TxtHostname);
            this.GpLogin.Controls.Add(this.TxtPasswd);
            this.GpLogin.Controls.Add(this.label3);
            this.GpLogin.Controls.Add(this.label4);
            this.GpLogin.Controls.Add(this.TxtUsername);
            this.GpLogin.Location = new System.Drawing.Point(12, 12);
            this.GpLogin.Name = "GpLogin";
            this.GpLogin.Size = new System.Drawing.Size(562, 133);
            this.GpLogin.TabIndex = 13;
            this.GpLogin.TabStop = false;
            this.GpLogin.Text = "Login Credentials";
            // 
            // LblVersion
            // 
            this.LblVersion.AutoSize = true;
            this.LblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblVersion.Location = new System.Drawing.Point(549, 291);
            this.LblVersion.Name = "LblVersion";
            this.LblVersion.Size = new System.Drawing.Size(25, 13);
            this.LblVersion.TabIndex = 14;
            this.LblVersion.Text = "100";
            this.LblVersion.Click += new System.EventHandler(this.LblVersion_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(88, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(313, 22);
            this.button1.TabIndex = 4;
            this.button1.Text = "Verify Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmMUPackage
            // 
            this.AcceptButton = this.BtnSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 309);
            this.Controls.Add(this.LblVersion);
            this.Controls.Add(this.GpLogin);
            this.Controls.Add(this.BtnSubmit);
            this.Controls.Add(this.GpPackager);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMUPackage";
            this.Text = "MUPackage";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GpPackager.ResumeLayout(false);
            this.GpPackager.PerformLayout();
            this.GpLogin.ResumeLayout(false);
            this.GpLogin.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GpPackager;
        private System.Windows.Forms.TextBox TxtHostname;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtUsername;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtPasswd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtProgName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LblValid;
        private System.Windows.Forms.TextBox TxtFilesToUpload;
        private System.Windows.Forms.Button BtnSubmit;
        private System.Windows.Forms.GroupBox GpLogin;
        private System.Windows.Forms.Label LblVersion;
        private System.Windows.Forms.Button button1;
    }
}

