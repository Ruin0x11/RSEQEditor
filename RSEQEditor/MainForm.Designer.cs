namespace RSEQEditor
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.positionIndicator1 = new RSEQEditor.PositionIndicator();
            this.pianoRoll1 = new RSEQEditor.PianoRoll();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewNumericUpDownColumn1 = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.dataGridViewNumericUpDownColumn2 = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.dataGridViewNumericUpDownColumn3 = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.dataGridViewNumericUpDownColumn4 = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionIndicator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pianoRoll1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.positionIndicator1);
            this.panel1.Controls.Add(this.vScrollBar1);
            this.panel1.Controls.Add(this.hScrollBar1);
            this.panel1.Controls.Add(this.pianoRoll1);
            this.panel1.Location = new System.Drawing.Point(12, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 629);
            this.panel1.TabIndex = 2;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(0, 52);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 80);
            this.vScrollBar1.TabIndex = 2;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
            this.vScrollBar1.Enter += new System.EventHandler(this.vScrollBar1_Enter);
            this.vScrollBar1.Resize += new System.EventHandler(this.vScrollBar1_Resize);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(18, 324);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(80, 17);
            this.hScrollBar1.TabIndex = 1;
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            this.hScrollBar1.Enter += new System.EventHandler(this.hScrollBar1_Enter);
            this.hScrollBar1.Resize += new System.EventHandler(this.hScrollBar1_Resize);
            // 
            // positionIndicator1
            // 
            this.positionIndicator1.Location = new System.Drawing.Point(20, 4);
            this.positionIndicator1.Name = "positionIndicator1";
            this.positionIndicator1.Size = new System.Drawing.Size(753, 50);
            this.positionIndicator1.TabIndex = 3;
            this.positionIndicator1.TabStop = false;
            // 
            // pianoRoll1
            // 
            this.pianoRoll1.Location = new System.Drawing.Point(20, 52);
            this.pianoRoll1.Name = "pianoRoll1";
            this.pianoRoll1.Size = new System.Drawing.Size(753, 318);
            this.pianoRoll1.TabIndex = 0;
            this.pianoRoll1.TabStop = false;
            this.pianoRoll1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pianoRoll1_MouseMove);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Timestamp";
            this.dataGridViewTextBoxColumn1.HeaderText = "Timestamp";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewNumericUpDownColumn1
            // 
            this.dataGridViewNumericUpDownColumn1.DataPropertyName = "_value1";
            this.dataGridViewNumericUpDownColumn1.HeaderText = "Value1";
            this.dataGridViewNumericUpDownColumn1.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.dataGridViewNumericUpDownColumn1.Name = "dataGridViewNumericUpDownColumn1";
            // 
            // dataGridViewNumericUpDownColumn2
            // 
            this.dataGridViewNumericUpDownColumn2.DataPropertyName = "_value2";
            this.dataGridViewNumericUpDownColumn2.HeaderText = "Value2";
            this.dataGridViewNumericUpDownColumn2.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.dataGridViewNumericUpDownColumn2.Name = "dataGridViewNumericUpDownColumn2";
            // 
            // dataGridViewNumericUpDownColumn3
            // 
            this.dataGridViewNumericUpDownColumn3.DataPropertyName = "_value3";
            this.dataGridViewNumericUpDownColumn3.HeaderText = "Value3";
            this.dataGridViewNumericUpDownColumn3.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.dataGridViewNumericUpDownColumn3.Name = "dataGridViewNumericUpDownColumn3";
            // 
            // dataGridViewNumericUpDownColumn4
            // 
            this.dataGridViewNumericUpDownColumn4.DataPropertyName = "_value4";
            this.dataGridViewNumericUpDownColumn4.HeaderText = "Value4";
            this.dataGridViewNumericUpDownColumn4.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.dataGridViewNumericUpDownColumn4.Name = "dataGridViewNumericUpDownColumn4";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 668);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.LocationChanged += new System.EventHandler(this.MainForm_LocationChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.positionIndicator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pianoRoll1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn1;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn2;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn3;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private PianoRoll pianoRoll1;
        private PositionIndicator positionIndicator1;
    }
}

