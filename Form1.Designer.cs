namespace FileCompare
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            panel1 = new Panel();
            btnLeftDir = new Button();
            lblAppName = new Label();
            panel2 = new Panel();
            panel3 = new Panel();
            btnCopyFromLeft = new Button();
            txtLeftDir = new TextBox();
            panel4 = new Panel();
            panelLeftHeader = new Panel();
            lvwLeftDir = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            panel5 = new Panel();
            panel6 = new Panel();
            btnRightDir = new Button();
            panel7 = new Panel();
            panel8 = new Panel();
            btnCopyFromRight = new Button();
            txtRightDir = new TextBox();
            panel10 = new Panel();
            panelRightHeader = new Panel();
            lvwRightDir = new ListView();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel6.SuspendLayout();
            panel8.SuspendLayout();
            panel10.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel1);
            splitContainer1.Panel1.Controls.Add(panel3);
            splitContainer1.Panel1.Controls.Add(panel4);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel6);
            splitContainer1.Panel2.Controls.Add(panel8);
            splitContainer1.Panel2.Controls.Add(panel10);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            splitContainer1.Size = new Size(800, 450);
            splitContainer1.SplitterDistance = 378;
            splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnLeftDir);
            panel1.Controls.Add(lblAppName);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 95);
            panel1.Name = "panel1";
            panel1.Size = new Size(378, 69);
            panel1.TabIndex = 0;
            // 
            // btnLeftDir
            // 
            btnLeftDir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLeftDir.Location = new Point(268, 29);
            btnLeftDir.Name = "btnLeftDir";
            btnLeftDir.Size = new Size(94, 29);
            btnLeftDir.TabIndex = 2;
            btnLeftDir.Text = ">>>";
            btnLeftDir.UseVisualStyleBackColor = true;
            // 
            // lblAppName
            // 
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("Segoe UI", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblAppName.ForeColor = Color.Red;
            lblAppName.Location = new Point(3, 9);
            lblAppName.Name = "lblAppName";
            lblAppName.Size = new Size(237, 50);
            lblAppName.TabIndex = 1;
            lblAppName.Text = "File Compare";
            lblAppName.Click += label1_Click;
            // 
            // panel2
            // 
            panel2.Location = new Point(0, 70);
            panel2.Name = "panel2";
            panel2.Size = new Size(375, 55);
            panel2.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnCopyFromLeft);
            panel3.Controls.Add(txtLeftDir);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(378, 95);
            panel3.TabIndex = 1;
            panel3.Paint += panel3_Paint;
            // 
            // btnCopyFromLeft
            // 
            btnCopyFromLeft.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCopyFromLeft.Location = new Point(268, 41);
            btnCopyFromLeft.Name = "btnCopyFromLeft";
            btnCopyFromLeft.Size = new Size(94, 29);
            btnCopyFromLeft.TabIndex = 4;
            btnCopyFromLeft.Text = "폴더 선택";
            btnCopyFromLeft.UseVisualStyleBackColor = true;
            btnCopyFromLeft.Click += btnCopyFromLeft_Click;
            // 
            // txtLeftDir
            // 
            txtLeftDir.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtLeftDir.Location = new Point(12, 41);
            txtLeftDir.Name = "txtLeftDir";
            txtLeftDir.Size = new Size(250, 27);
            txtLeftDir.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.Controls.Add(panelLeftHeader);
            panel4.Controls.Add(lvwLeftDir);
            panel4.Controls.Add(panel5);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(378, 450);
            panel4.TabIndex = 2;
            // 
            // panelLeftHeader
            // 
            panelLeftHeader.Location = new Point(0, 0);
            panelLeftHeader.Name = "panelLeftHeader";
            panelLeftHeader.Size = new Size(200, 100);
            panelLeftHeader.TabIndex = 0;
            // 
            // lvwLeftDir
            // 
            lvwLeftDir.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            lvwLeftDir.Dock = DockStyle.Fill;
            lvwLeftDir.FullRowSelect = true;
            lvwLeftDir.GridLines = true;
            lvwLeftDir.Location = new Point(0, 0);
            lvwLeftDir.Name = "lvwLeftDir";
            lvwLeftDir.Size = new Size(378, 450);
            lvwLeftDir.TabIndex = 5;
            lvwLeftDir.UseCompatibleStateImageBehavior = false;
            lvwLeftDir.View = View.Details;
            lvwLeftDir.SelectedIndexChanged += lvwLeftDir_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "이름";
            columnHeader1.Width = 220;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "크기";
            columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "수정일";
            columnHeader3.Width = 140;
            // 
            // panel5
            // 
            panel5.Location = new Point(385, -173);
            panel5.Name = "panel5";
            panel5.Size = new Size(412, 298);
            panel5.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.Controls.Add(btnRightDir);
            panel6.Controls.Add(panel7);
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(0, 95);
            panel6.Name = "panel6";
            panel6.Size = new Size(418, 69);
            panel6.TabIndex = 0;
            panel6.Paint += panel6_Paint;
            // 
            // btnRightDir
            // 
            btnRightDir.Location = new Point(6, 30);
            btnRightDir.Name = "btnRightDir";
            btnRightDir.Size = new Size(94, 29);
            btnRightDir.TabIndex = 6;
            btnRightDir.Text = "<<<";
            btnRightDir.UseVisualStyleBackColor = true;
            btnRightDir.Click += btnRightDir_Click;
            // 
            // panel7
            // 
            panel7.Location = new Point(0, 75);
            panel7.Name = "panel7";
            panel7.Size = new Size(418, 50);
            panel7.TabIndex = 0;
            // 
            // panel8
            // 
            panel8.Controls.Add(btnCopyFromRight);
            panel8.Controls.Add(txtRightDir);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(0, 0);
            panel8.Name = "panel8";
            panel8.Size = new Size(418, 95);
            panel8.TabIndex = 4;
            // 
            // btnCopyFromRight
            // 
            btnCopyFromRight.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCopyFromRight.Location = new Point(315, 42);
            btnCopyFromRight.Name = "btnCopyFromRight";
            btnCopyFromRight.Size = new Size(94, 29);
            btnCopyFromRight.TabIndex = 5;
            btnCopyFromRight.Text = "폴더선택";
            btnCopyFromRight.UseVisualStyleBackColor = true;
            btnCopyFromRight.Click += btnCopyFromRight_Click;
            // 
            // txtRightDir
            // 
            txtRightDir.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtRightDir.Location = new Point(6, 42);
            txtRightDir.Name = "txtRightDir";
            txtRightDir.Size = new Size(302, 27);
            txtRightDir.TabIndex = 7;
            // 
            // panel10
            // 
            panel10.Controls.Add(panelRightHeader);
            panel10.Controls.Add(lvwRightDir);
            panel10.Dock = DockStyle.Fill;
            panel10.Location = new Point(0, 0);
            panel10.Name = "panel10";
            panel10.Size = new Size(418, 450);
            panel10.TabIndex = 3;
            // 
            // panelRightHeader
            // 
            panelRightHeader.Location = new Point(0, 0);
            panelRightHeader.Name = "panelRightHeader";
            panelRightHeader.Size = new Size(200, 100);
            panelRightHeader.TabIndex = 0;
            // 
            // lvwRightDir
            // 
            lvwRightDir.Columns.AddRange(new ColumnHeader[] { columnHeader4, columnHeader5, columnHeader6 });
            lvwRightDir.Dock = DockStyle.Fill;
            lvwRightDir.FullRowSelect = true;
            lvwRightDir.GridLines = true;
            lvwRightDir.Location = new Point(0, 0);
            lvwRightDir.Name = "lvwRightDir";
            lvwRightDir.Size = new Size(418, 450);
            lvwRightDir.TabIndex = 6;
            lvwRightDir.UseCompatibleStateImageBehavior = false;
            lvwRightDir.View = View.Details;
            lvwRightDir.SelectedIndexChanged += listView2_SelectedIndexChanged;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "이름";
            columnHeader4.Width = 220;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "크기";
            columnHeader5.Width = 80;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "수정일";
            columnHeader6.Width = 140;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "FIle Compare v1.0";
            Load += Form1_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel10.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Panel panel4;
        private Panel panel5;
        private Panel panel3;
        private Panel panel1;
        private Panel panel2;
        private Panel panel6;
        private Panel panel7;
        private Panel panel10;
        private Panel panel8;
        private Label lblAppName;
        private Label lblLeftHeader;
        private Label lblRightHeader;
        private Panel panelLeftHeader;
        private Panel panelRightHeader;
        private ListView lvwLeftDir;
        private Button btnCopyFromLeft;
        private TextBox txtLeftDir;
        private Button btnLeftDir;
        private Button btnCopyFromRight;
        private TextBox txtRightDir;
        private ListView lvwRightDir;
        private Button btnRightDir;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
    }
}
