namespace SPC.LDAP.ProfileSync.WinForm
{
    partial class FrmSearchDomain
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtDomainUrl = new System.Windows.Forms.TextBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.DdBaseDN = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtRootOU = new System.Windows.Forms.TextBox();
            this.TxtSearchFilter = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnBaseDN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Domain Url";
            // 
            // TxtDomainUrl
            // 
            this.TxtDomainUrl.Location = new System.Drawing.Point(16, 30);
            this.TxtDomainUrl.Name = "TxtDomainUrl";
            this.TxtDomainUrl.Size = new System.Drawing.Size(549, 20);
            this.TxtDomainUrl.TabIndex = 1;
            this.TxtDomainUrl.Text = "ldap.spc.com";
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(571, 146);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 23);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "Search";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(-1, 175);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(8);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(841, 357);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // DdBaseDN
            // 
            this.DdBaseDN.FormattingEnabled = true;
            this.DdBaseDN.Location = new System.Drawing.Point(16, 108);
            this.DdBaseDN.Name = "DdBaseDN";
            this.DdBaseDN.Size = new System.Drawing.Size(630, 21);
            this.DdBaseDN.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Base DN";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Root OU";
            // 
            // TxtRootOU
            // 
            this.TxtRootOU.Location = new System.Drawing.Point(16, 69);
            this.TxtRootOU.Name = "TxtRootOU";
            this.TxtRootOU.Size = new System.Drawing.Size(549, 20);
            this.TxtRootOU.TabIndex = 7;
            // 
            // TxtSearchFilter
            // 
            this.TxtSearchFilter.Location = new System.Drawing.Point(16, 148);
            this.TxtSearchFilter.Name = "TxtSearchFilter";
            this.TxtSearchFilter.Size = new System.Drawing.Size(549, 20);
            this.TxtSearchFilter.TabIndex = 8;
            this.TxtSearchFilter.Text = "(&(objectClass=person)(objectClass=user))";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Search Filter";
            // 
            // BtnBaseDN
            // 
            this.BtnBaseDN.Location = new System.Drawing.Point(571, 67);
            this.BtnBaseDN.Name = "BtnBaseDN";
            this.BtnBaseDN.Size = new System.Drawing.Size(112, 23);
            this.BtnBaseDN.TabIndex = 10;
            this.BtnBaseDN.Text = "Update Base DN";
            this.BtnBaseDN.UseVisualStyleBackColor = true;
            this.BtnBaseDN.Click += new System.EventHandler(this.BtnBaseDN_Click);
            // 
            // FrmSearchDomain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 532);
            this.Controls.Add(this.BtnBaseDN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TxtSearchFilter);
            this.Controls.Add(this.TxtRootOU);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DdBaseDN);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.TxtDomainUrl);
            this.Controls.Add(this.label1);
            this.Name = "FrmSearchDomain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FrmSearchDomain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtDomainUrl;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox DdBaseDN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtRootOU;
        private System.Windows.Forms.TextBox TxtSearchFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnBaseDN;
    }
}

