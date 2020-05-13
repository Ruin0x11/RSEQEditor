namespace RSEQEditor
{
    partial class EventListForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Timestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Value1 = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.Value2 = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.Value3 = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.Value4 = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            this.Value5 = new DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Timestamp,
            this.Type,
            this.Value1,
            this.Value2,
            this.Value3,
            this.Value4,
            this.Value5});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 426);
            this.dataGridView1.TabIndex = 1;
            // 
            // Timestamp
            // 
            this.Timestamp.DataPropertyName = "Timestamp";
            this.Timestamp.HeaderText = "Timestamp";
            this.Timestamp.Name = "Timestamp";
            // 
            // Type
            // 
            this.Type.DataPropertyName = "_cmd";
            this.Type.HeaderText = "Command";
            this.Type.Name = "Type";
            // 
            // Value1
            // 
            this.Value1.DataPropertyName = "_value1";
            this.Value1.HeaderText = "Value1";
            this.Value1.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.Value1.Name = "Value1";
            // 
            // Value2
            // 
            this.Value2.DataPropertyName = "_value2";
            this.Value2.HeaderText = "Value2";
            this.Value2.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.Value2.Name = "Value2";
            // 
            // Value3
            // 
            this.Value3.DataPropertyName = "_value3";
            this.Value3.HeaderText = "Value3";
            this.Value3.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.Value3.Name = "Value3";
            // 
            // Value4
            // 
            this.Value4.DataPropertyName = "_value4";
            this.Value4.HeaderText = "Value4";
            this.Value4.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.Value4.Name = "Value4";
            // 
            // Value5
            // 
            this.Value5.DataPropertyName = "_value5";
            this.Value5.HeaderText = "Value5";
            this.Value5.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.Value5.Name = "Value5";
            // 
            // EventListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Name = "EventListForm";
            this.Text = "EventListForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Timestamp;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn Value1;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn Value2;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn Value3;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn Value4;
        private DataGridViewNumericUpDownElements.DataGridViewNumericUpDownColumn Value5;
    }
}