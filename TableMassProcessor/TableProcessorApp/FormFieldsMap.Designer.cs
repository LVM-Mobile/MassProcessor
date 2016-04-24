namespace TableProcessorApp
{
    partial class FormFieldsMap
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
            this.dataGridViewFieldsMap = new System.Windows.Forms.DataGridView();
            this.TargerField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inputField = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFieldsMap)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewFieldsMap
            // 
            this.dataGridViewFieldsMap.AllowUserToAddRows = false;
            this.dataGridViewFieldsMap.AllowUserToDeleteRows = false;
            this.dataGridViewFieldsMap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewFieldsMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFieldsMap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TargerField,
            this.inputField});
            this.dataGridViewFieldsMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFieldsMap.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewFieldsMap.Name = "dataGridViewFieldsMap";
            this.dataGridViewFieldsMap.RowTemplate.Height = 24;
            this.dataGridViewFieldsMap.Size = new System.Drawing.Size(589, 362);
            this.dataGridViewFieldsMap.TabIndex = 0;
            this.dataGridViewFieldsMap.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFieldsMap_CellContentClick);
            // 
            // TargerField
            // 
            this.TargerField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.TargerField.Frozen = true;
            this.TargerField.HeaderText = "Field";
            this.TargerField.Name = "TargerField";
            this.TargerField.ReadOnly = true;
            this.TargerField.Width = 52;
            // 
            // inputField
            // 
            this.inputField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.inputField.HeaderText = "Input field";
            this.inputField.Items.AddRange(new object[] {
            "11",
            "22",
            "33"});
            this.inputField.Name = "inputField";
            this.inputField.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.inputField.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 331);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(589, 31);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(3, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(84, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FormFieldsMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 362);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataGridViewFieldsMap);
            this.Name = "FormFieldsMap";
            this.Text = "FormFieldsMap";
            this.Load += new System.EventHandler(this.FormFieldsMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFieldsMap)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewFieldsMap;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargerField;
        private System.Windows.Forms.DataGridViewComboBoxColumn inputField;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}