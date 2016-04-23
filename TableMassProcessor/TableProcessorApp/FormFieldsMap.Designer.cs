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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.TargerField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inputField = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFieldsMap)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewFieldsMap
            // 
            this.dataGridViewFieldsMap.AllowUserToAddRows = false;
            this.dataGridViewFieldsMap.AllowUserToDeleteRows = false;
            this.dataGridViewFieldsMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFieldsMap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TargerField,
            this.inputField});
            this.dataGridViewFieldsMap.Location = new System.Drawing.Point(16, 15);
            this.dataGridViewFieldsMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridViewFieldsMap.Name = "dataGridViewFieldsMap";
            this.dataGridViewFieldsMap.RowTemplate.Height = 24;
            this.dataGridViewFieldsMap.Size = new System.Drawing.Size(527, 337);
            this.dataGridViewFieldsMap.TabIndex = 0;
            this.dataGridViewFieldsMap.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFieldsMap_CellContentClick);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(153, 359);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(261, 359);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // TargerField
            // 
            this.TargerField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.TargerField.Frozen = true;
            this.TargerField.HeaderText = "Field";
            this.TargerField.Name = "TargerField";
            this.TargerField.ReadOnly = true;
            this.TargerField.Width = 63;
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
            // FormFieldsMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 396);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridViewFieldsMap);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormFieldsMap";
            this.Text = "FormFieldsMap";
            this.Load += new System.EventHandler(this.FormFieldsMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFieldsMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewFieldsMap;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargerField;
        private System.Windows.Forms.DataGridViewComboBoxColumn inputField;

    }
}