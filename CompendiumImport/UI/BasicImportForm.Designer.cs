namespace CompendiumImport.UI
{
    partial class BasicImportForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BasicImportForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.btnAddTo = new System.Windows.Forms.Button();
            this.cboLibraries = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSingleSourceOnly = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnUncheckInAnother = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.lvwResults = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMainRole = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GroupRole = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSearch = new System.Windows.Forms.Button();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLoading = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkHideDuplicateInLibrary = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblInfo,
            this.lblProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 422);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(734, 22);
            this.statusStrip1.TabIndex = 53;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = false;
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(350, 17);
            // 
            // lblProgress
            // 
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(200, 16);
            // 
            // btnAddTo
            // 
            this.btnAddTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddTo.Enabled = false;
            this.btnAddTo.Location = new System.Drawing.Point(2, 362);
            this.btnAddTo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAddTo.Name = "btnAddTo";
            this.btnAddTo.Size = new System.Drawing.Size(100, 25);
            this.btnAddTo.TabIndex = 47;
            this.btnAddTo.Text = "Add To";
            this.btnAddTo.UseVisualStyleBackColor = true;
            this.btnAddTo.Click += new System.EventHandler(this.btnAddTo_Click);
            this.btnAddTo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddTo_MouseUp);
            // 
            // cboLibraries
            // 
            this.cboLibraries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLibraries.DisplayMember = "Name";
            this.cboLibraries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLibraries.FormattingEnabled = true;
            this.cboLibraries.Location = new System.Drawing.Point(110, 364);
            this.cboLibraries.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboLibraries.Name = "cboLibraries";
            this.cboLibraries.Size = new System.Drawing.Size(613, 21);
            this.cboLibraries.TabIndex = 46;
            this.cboLibraries.ValueMember = "ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "Select :";
            // 
            // btnSingleSourceOnly
            // 
            this.btnSingleSourceOnly.Location = new System.Drawing.Point(256, 32);
            this.btnSingleSourceOnly.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSingleSourceOnly.Name = "btnSingleSourceOnly";
            this.btnSingleSourceOnly.Size = new System.Drawing.Size(127, 25);
            this.btnSingleSourceOnly.TabIndex = 52;
            this.btnSingleSourceOnly.Text = "Multiple sources";
            this.toolTip1.SetToolTip(this.btnSingleSourceOnly, "Uncheck items present in several sources");
            this.btnSingleSourceOnly.UseVisualStyleBackColor = true;
            this.btnSingleSourceOnly.Click += new System.EventHandler(this.btnSingleSourceOnly_Click);
            // 
            // btnUncheckInAnother
            // 
            this.btnUncheckInAnother.Location = new System.Drawing.Point(391, 31);
            this.btnUncheckInAnother.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUncheckInAnother.Name = "btnUncheckInAnother";
            this.btnUncheckInAnother.Size = new System.Drawing.Size(155, 25);
            this.btnUncheckInAnother.TabIndex = 56;
            this.btnUncheckInAnother.Text = "Present in another library";
            this.toolTip1.SetToolTip(this.btnUncheckInAnother, "Uncheck items present in another library");
            this.btnUncheckInAnother.UseVisualStyleBackColor = true;
            this.btnUncheckInAnother.Click += new System.EventHandler(this.btnUncheckPresentInAnotherLibrary_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(130, 33);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(53, 25);
            this.btnSelectAll.TabIndex = 44;
            this.btnSelectAll.Text = "All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(62, 33);
            this.btnSelectNone.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(60, 25);
            this.btnSelectNone.TabIndex = 43;
            this.btnSelectNone.Text = "None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // lvwResults
            // 
            this.lvwResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwResults.CheckBoxes = true;
            this.lvwResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colLevel,
            this.colMainRole,
            this.GroupRole,
            this.colSource});
            this.lvwResults.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwResults.Location = new System.Drawing.Point(7, 64);
            this.lvwResults.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lvwResults.Name = "lvwResults";
            this.lvwResults.ShowItemToolTips = true;
            this.lvwResults.Size = new System.Drawing.Size(717, 292);
            this.lvwResults.TabIndex = 42;
            this.lvwResults.UseCompatibleStateImageBehavior = false;
            this.lvwResults.View = System.Windows.Forms.View.Details;
            this.lvwResults.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvwResults_ItemChecked);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 263;
            // 
            // colLevel
            // 
            this.colLevel.Text = "Level";
            this.colLevel.Width = 45;
            // 
            // colMainRole
            // 
            this.colMainRole.Text = "Main Role";
            this.colMainRole.Width = 132;
            // 
            // GroupRole
            // 
            this.GroupRole.Text = "Group Role";
            this.GroupRole.Width = 86;
            // 
            // colSource
            // 
            this.colSource.Text = "Source";
            this.colSource.Width = 163;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(621, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 25);
            this.btnSearch.TabIndex = 41;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cboSource
            // 
            this.cboSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSource.DisplayMember = "Name";
            this.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Location = new System.Drawing.Point(62, 4);
            this.cboSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(551, 21);
            this.cboSource.TabIndex = 40;
            this.cboSource.ValueMember = "Value";
            this.cboSource.SelectedIndexChanged += new System.EventHandler(this.cboSource_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Source : ";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPassword.Location = new System.Drawing.Point(490, 392);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(174, 20);
            this.txtPassword.TabIndex = 51;
            // 
            // txtLogin
            // 
            this.txtLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtLogin.Location = new System.Drawing.Point(110, 392);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(282, 20);
            this.txtLogin.TabIndex = 50;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(408, 397);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "DDI Password";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 397);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "DDI Login";
            // 
            // lblLoading
            // 
            this.lblLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoading.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoading.Location = new System.Drawing.Point(0, 0);
            this.lblLoading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(734, 444);
            this.lblLoading.TabIndex = 54;
            this.lblLoading.Text = "Loading filter data for the first time..";
            this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoading.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(191, 38);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 55;
            this.label5.Text = "Uncheck :";
            // 
            // chkHideDuplicateInLibrary
            // 
            this.chkHideDuplicateInLibrary.AutoSize = true;
            this.chkHideDuplicateInLibrary.Checked = true;
            this.chkHideDuplicateInLibrary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHideDuplicateInLibrary.Location = new System.Drawing.Point(553, 33);
            this.chkHideDuplicateInLibrary.Name = "chkHideDuplicateInLibrary";
            this.chkHideDuplicateInLibrary.Size = new System.Drawing.Size(141, 17);
            this.chkHideDuplicateInLibrary.TabIndex = 57;
            this.chkHideDuplicateInLibrary.Text = "Hide Duplicate in Library";
            this.chkHideDuplicateInLibrary.UseVisualStyleBackColor = true;
            this.chkHideDuplicateInLibrary.Visible = false;
            // 
            // BasicImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 444);
            this.Controls.Add(this.chkHideDuplicateInLibrary);
            this.Controls.Add(this.btnUncheckInAnother);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnAddTo);
            this.Controls.Add(this.cboLibraries);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSingleSourceOnly);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.lvwResults);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.cboSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblLoading);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BasicImportForm";
            this.Text = "BasicImportForm";
            this.Activated += new System.EventHandler(this.BasicImportForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BasicImportForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BasicImportForm_FormClosed);
            this.Load += new System.EventHandler(this.BasicImportForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblInfo;
        private System.Windows.Forms.Button btnAddTo;
        private System.Windows.Forms.ComboBox cboLibraries;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSingleSourceOnly;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colLevel;
        private System.Windows.Forms.ColumnHeader colMainRole;
        private System.Windows.Forms.ColumnHeader GroupRole;
        private System.Windows.Forms.ColumnHeader colSource;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.Label label5;
        protected System.Windows.Forms.ListView lvwResults;
        private System.Windows.Forms.Button btnUncheckInAnother;
        private System.Windows.Forms.CheckBox chkHideDuplicateInLibrary;
        protected System.Windows.Forms.ComboBox cboSource;
        protected System.Windows.Forms.ToolStripProgressBar lblProgress;
        protected System.Windows.Forms.TextBox txtPassword;
        protected System.Windows.Forms.TextBox txtLogin;
    }
}