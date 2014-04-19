namespace CompendiumImport.UI
{
    partial class ResultForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lboSuccess = new System.Windows.Forms.ListBox();
            this.lboWarnings = new System.Windows.Forms.ListBox();
            this.lboBugs = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Success :";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 185);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Warnings :";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 332);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Bugs :";
            // 
            // lboSuccess
            // 
            this.lboSuccess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lboSuccess.FormattingEnabled = true;
            this.lboSuccess.ItemHeight = 14;
            this.lboSuccess.Location = new System.Drawing.Point(82, 12);
            this.lboSuccess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lboSuccess.Name = "lboSuccess";
            this.lboSuccess.Size = new System.Drawing.Size(639, 158);
            this.lboSuccess.TabIndex = 3;
            // 
            // lboWarnings
            // 
            this.lboWarnings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lboWarnings.FormattingEnabled = true;
            this.lboWarnings.ItemHeight = 14;
            this.lboWarnings.Location = new System.Drawing.Point(82, 192);
            this.lboWarnings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lboWarnings.Name = "lboWarnings";
            this.lboWarnings.Size = new System.Drawing.Size(639, 144);
            this.lboWarnings.TabIndex = 4;
            // 
            // lboBugs
            // 
            this.lboBugs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lboBugs.FormattingEnabled = true;
            this.lboBugs.ItemHeight = 14;
            this.lboBugs.Location = new System.Drawing.Point(81, 342);
            this.lboBugs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lboBugs.Name = "lboBugs";
            this.lboBugs.Size = new System.Drawing.Size(639, 88);
            this.lboBugs.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(3, 401);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(71, 25);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.Location = new System.Drawing.Point(3, 370);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(71, 25);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(729, 440);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lboBugs);
            this.Controls.Add(this.lboWarnings);
            this.Controls.Add(this.lboSuccess);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResultForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ResultForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lboSuccess;
        private System.Windows.Forms.ListBox lboWarnings;
        private System.Windows.Forms.ListBox lboBugs;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
    }
}