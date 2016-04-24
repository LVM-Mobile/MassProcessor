using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Reflection;
using TableProcessorNS;
using RecordProcessor;
using LVMSoft.Progress;
using System.Text;
using DatabaseAdapter;


namespace TableProcessorApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FormTableProcessor : System.Windows.Forms.Form
	{
        TableProcessorNS.TableProcessor tp;
        private string modulesdir;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonStart;
        private SaveFileDialog saveFileDialog1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripProgressBar toolStripProgressBar1;
        private GroupBox groupBoxExtract;
        private CheckBox CheckBoxEditMode;
        private ComboBox comboBoxTableName;
        private Button buttonOpen;
        private Label label3;
        private TextBox textBoxInputFileName;
        private Label label2;
        private GroupBox groupBox1;
        private Button buttonMapFields;
        private TextBox textBoxFieldsMapping;
        private Label label4;
        private Label label1;
        private ComboBox comboBoxProcessor;
        private GroupBox groupBox2;
        private Button buttonSave;
        private Label label10;
        private TextBox textBoxOutputFileName;
        private Label label5;
        private TextBox textBoxOffset;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormTableProcessor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            tp = new TableProcessorNS.TableProcessor();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonStart = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxExtract = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxOffset = new System.Windows.Forms.TextBox();
            this.CheckBoxEditMode = new System.Windows.Forms.CheckBox();
            this.comboBoxTableName = new System.Windows.Forms.ComboBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxInputFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonMapFields = new System.Windows.Forms.Button();
            this.textBoxFieldsMapping = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxProcessor = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxOutputFileName = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.groupBoxExtract.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "All Known files|*.xls;*.xlsx;*.csv;*.dbf";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(12, 269);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(681, 24);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.Text = "Start";
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 301);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(705, 22);
            this.statusStrip1.TabIndex = 21;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBoxExtract
            // 
            this.groupBoxExtract.Controls.Add(this.label5);
            this.groupBoxExtract.Controls.Add(this.textBoxOffset);
            this.groupBoxExtract.Controls.Add(this.CheckBoxEditMode);
            this.groupBoxExtract.Controls.Add(this.comboBoxTableName);
            this.groupBoxExtract.Controls.Add(this.buttonOpen);
            this.groupBoxExtract.Controls.Add(this.label3);
            this.groupBoxExtract.Controls.Add(this.textBoxInputFileName);
            this.groupBoxExtract.Controls.Add(this.label2);
            this.groupBoxExtract.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxExtract.Location = new System.Drawing.Point(0, 0);
            this.groupBoxExtract.Name = "groupBoxExtract";
            this.groupBoxExtract.Size = new System.Drawing.Size(705, 131);
            this.groupBoxExtract.TabIndex = 23;
            this.groupBoxExtract.TabStop = false;
            this.groupBoxExtract.Text = "Extract";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Begin processing from row";
            // 
            // textBoxOffset
            // 
            this.textBoxOffset.Location = new System.Drawing.Point(184, 92);
            this.textBoxOffset.Name = "textBoxOffset";
            this.textBoxOffset.Size = new System.Drawing.Size(84, 20);
            this.textBoxOffset.TabIndex = 29;
            // 
            // CheckBoxEditMode
            // 
            this.CheckBoxEditMode.AutoSize = true;
            this.CheckBoxEditMode.Checked = true;
            this.CheckBoxEditMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxEditMode.Location = new System.Drawing.Point(184, 16);
            this.CheckBoxEditMode.Name = "CheckBoxEditMode";
            this.CheckBoxEditMode.Size = new System.Drawing.Size(73, 17);
            this.CheckBoxEditMode.TabIndex = 28;
            this.CheckBoxEditMode.Text = "Edit mode";
            this.CheckBoxEditMode.UseVisualStyleBackColor = true;
            // 
            // comboBoxTableName
            // 
            this.comboBoxTableName.FormattingEnabled = true;
            this.comboBoxTableName.Location = new System.Drawing.Point(184, 65);
            this.comboBoxTableName.Name = "comboBoxTableName";
            this.comboBoxTableName.Size = new System.Drawing.Size(464, 21);
            this.comboBoxTableName.TabIndex = 27;
            this.comboBoxTableName.SelectedIndexChanged += new System.EventHandler(this.comboBoxTableName_SelectedIndexChanged);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(654, 40);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(39, 19);
            this.buttonOpen.TabIndex = 26;
            this.buttonOpen.Text = "...";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 24);
            this.label3.TabIndex = 25;
            this.label3.Text = "Selected table";
            // 
            // textBoxInputFileName
            // 
            this.textBoxInputFileName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxInputFileName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.textBoxInputFileName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TableProcessorApp.Properties.Settings.Default, "InputDB", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxInputFileName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxInputFileName.Location = new System.Drawing.Point(184, 39);
            this.textBoxInputFileName.Name = "textBoxInputFileName";
            this.textBoxInputFileName.Size = new System.Drawing.Size(464, 20);
            this.textBoxInputFileName.TabIndex = 24;
            this.textBoxInputFileName.Text = global::TableProcessorApp.Properties.Settings.Default.InputDB;
            this.textBoxInputFileName.TextChanged += new System.EventHandler(this.textBoxInputFileName_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 24);
            this.label2.TabIndex = 23;
            this.label2.Text = "Database path (tables Dir/File)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonMapFields);
            this.groupBox1.Controls.Add(this.textBoxFieldsMapping);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxProcessor);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 131);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(705, 87);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transform";
            // 
            // buttonMapFields
            // 
            this.buttonMapFields.Location = new System.Drawing.Point(654, 55);
            this.buttonMapFields.Name = "buttonMapFields";
            this.buttonMapFields.Size = new System.Drawing.Size(39, 19);
            this.buttonMapFields.TabIndex = 26;
            this.buttonMapFields.Text = "...";
            this.buttonMapFields.UseVisualStyleBackColor = true;
            this.buttonMapFields.Click += new System.EventHandler(this.buttonMapFields_Click);
            // 
            // textBoxFieldsMapping
            // 
            this.textBoxFieldsMapping.Location = new System.Drawing.Point(184, 54);
            this.textBoxFieldsMapping.Name = "textBoxFieldsMapping";
            this.textBoxFieldsMapping.Size = new System.Drawing.Size(464, 20);
            this.textBoxFieldsMapping.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 24);
            this.label4.TabIndex = 23;
            this.label4.Text = "Fields to process";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 22;
            this.label1.Text = "Processor module";
            // 
            // comboBoxProcessor
            // 
            this.comboBoxProcessor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProcessor.Location = new System.Drawing.Point(184, 27);
            this.comboBoxProcessor.Name = "comboBoxProcessor";
            this.comboBoxProcessor.Size = new System.Drawing.Size(464, 21);
            this.comboBoxProcessor.TabIndex = 21;
            this.comboBoxProcessor.SelectedIndexChanged += new System.EventHandler(this.comboBoxProcessor_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonSave);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.textBoxOutputFileName);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(0, 218);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(705, 45);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Load";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(654, 18);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(39, 19);
            this.buttonSave.TabIndex = 20;
            this.buttonSave.Text = "...";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(12, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(166, 20);
            this.label10.TabIndex = 19;
            this.label10.Text = "Database path (tables Dir/File)";
            // 
            // textBoxOutputFileName
            // 
            this.textBoxOutputFileName.Location = new System.Drawing.Point(184, 18);
            this.textBoxOutputFileName.Name = "textBoxOutputFileName";
            this.textBoxOutputFileName.Size = new System.Drawing.Size(464, 19);
            this.textBoxOutputFileName.TabIndex = 18;
            this.textBoxOutputFileName.Text = "f:\\TableProcessor\\GroupCounter\\tests\\testdata2.csv";
            this.textBoxOutputFileName.TextChanged += new System.EventHandler(this.textBoxOutputFileName_TextChanged);
            // 
            // FormTableProcessor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(705, 323);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxExtract);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonStart);
            this.Name = "FormTableProcessor";
            this.Text = "Table Mass Processor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBoxExtract.ResumeLayout(false);
            this.groupBoxExtract.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FormTableProcessor());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{


            //Take processor plugins
            modulesdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"modules");
			
            comboBoxProcessor.Items.Clear();
			string [] files = Directory.GetFiles(modulesdir, "*.dll");
			
            Dictionary<string,string> modulenames = new Dictionary<string,string>();
            foreach(string path in files)
			{
               try{
                var ass = Assembly.LoadFrom(path);
                AssemblyTitleAttribute title_assembly = ass.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute;
               
                modulenames[title_assembly.Title] =  path;
               }catch(Exception ex)
                {

                }
            }

            comboBoxProcessor.DisplayMember = "Key";
            comboBoxProcessor.ValueMember = "Value";

            comboBoxProcessor.DataSource = new BindingSource(modulenames, null);
            
		   
            //Autoselect if just one choose
            if (comboBoxProcessor.Items.Count == 1)
            {
                comboBoxProcessor.SelectedIndex = 0;
            }
            //Check and autofill table fields if table exists
            textBoxInputFileName_TextChanged(this, new EventArgs());
		}

		
		private void buttonStart_Click(object sender, System.EventArgs e)
		{
            buttonStart.Enabled = false;
            try
            {
                string filename = textBoxInputFileName.Text.Trim();
                string outfilename = textBoxOutputFileName.Text.Trim();
                //extract settings
                tp.InputDatabase = DatabaseAdapterFactory.CreateReader(Path.GetExtension(filename), CheckBoxEditMode.Checked);

                tp.InputDatabase.FileName = filename;
                //transform settings
                tp.tableName = comboBoxTableName.Text;
                tp.InputFieldNamesMap = TableProcessor.DeserializeFieldsMap( textBoxFieldsMapping.Text );

                //load settings 

                //If Edit mode - copy original and open for wrtiting
                if (CheckBoxEditMode.Checked)
                {
                    tp.OutputDatabase = DatabaseAdapterFactory.CreateWriter(Path.GetExtension(outfilename));
                    tp.ProcessMode = ProcessMode.pmEdit;
                }

                tp.OutputDatabase.FileName = outfilename;

                tp.Progress = new ProgressBarMonitor(toolStripProgressBar1, toolStripStatusLabel1, 100);
                
                int.TryParse(textBoxOffset.Text, out tp.ProcessOffset);
              
                tp.Process();
                
                string Msg = Path.GetFileName(tp.InputDatabase.FileName) + " processing complete";
                MessageBox.Show(this, Msg, "Results");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message +" "+ ex.StackTrace, "Error");
            }
            buttonStart.Enabled = true;
		}

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxInputFileName_TextChanged(object sender, EventArgs e)
        {

            string filename = textBoxInputFileName.Text.Trim();

            if (File.Exists(filename))
            {
                //Create input database adapter
                tp.InputDatabase = DatabaseAdapterFactory.CreateReader(Path.GetExtension(filename), CheckBoxEditMode.Checked);
                tp.InputDatabase.FileName = filename;

                textBoxInputFileName.ForeColor = System.Drawing.SystemColors.WindowText;
                //Fill tables list
                comboBoxTableName.Items.Clear();

                comboBoxTableName.Items.AddRange(tp.InputDatabase.GetTables());
                if (comboBoxTableName.Items.Count == 1)
                {
                    comboBoxTableName.SelectedIndex= 0;
                }
                else {
                    comboBoxTableName.Text = "";
                }
            }
            else {
                textBoxInputFileName.ForeColor = Color.Red;
            }
        }

        
        private void buttonSave_Click(object sender, EventArgs e)
        {
            //TODO: Take output data types from acceptable DataAdapters
            saveFileDialog1.Filter = "Comma separated text(CSV)|*.csv|Excel (2000/2003)|*.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxOutputFileName.Text = saveFileDialog1.FileName;
            }
        }

        //Make filter
        string MakeFileTypesFilter(Dictionary<string, string> types)
        {
            //Auto Make All Known Files
            StringBuilder sbExt = new StringBuilder();
            foreach(string ext in types.Values)
            {
                if(sbExt.Length>0)
                   sbExt.Append(';');
                sbExt.Append(ext);
            }

            StringBuilder sb = new StringBuilder("All Known Files|" + sbExt.ToString());
            foreach(KeyValuePair<string, string> pair in types)
            {
                if(sb.Length>0)
                    sb.Append('|');
              sb.Append(pair.Key);
              sb.Append('|');
              sb.Append(pair.Value);
            }
            return sb.ToString();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.Filter = MakeFileTypesFilter(DatabaseAdapterFactory.KnownFileTypes);

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxInputFileName.Text = openFileDialog1.FileName;
                //Autogenerate output filename
                textBoxOutputFileName.Text = Path.GetDirectoryName(textBoxInputFileName.Text) +"\\"+ Path.GetFileNameWithoutExtension(textBoxInputFileName.Text) + "_out" + Path.GetExtension(textBoxInputFileName.Text);
            }
        }

        private void buttonMapFields_Click(object sender, EventArgs e)
        {
            if (comboBoxTableName.SelectedIndex == -1)
                MessageBox.Show("Select source table first");

            FormFieldsMap mapForm = new FormFieldsMap();
            mapForm.targetFields = tp.GetTransformFields();
            mapForm.inputFields = tp.InputDatabase.GetFields(tp.tableName);
            //apply current settings
            //DeserializeFieldsMap(textBoxFieldsMapping.Text);
            if (mapForm.ShowDialog()==DialogResult.OK)
            {
                //Update fields mapping from UI 
                mapForm.getMapping();

                tp.InputFieldNamesMap.Clear();
                foreach(var pair in mapForm.Target2InputFields)
                {
                    //Save as Inut field to target field map
                    tp.InputFieldNamesMap.Add(new KeyValuePair<string, string>(pair.Value, pair.Key));
                }
                textBoxFieldsMapping.Text = TableProcessor.SerializeFieldsMap(tp.InputFieldNamesMap);
            }
        }

        private void comboBoxTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            tp.tableName = comboBoxTableName.Text;
        }

        private void textBoxOutputFileName_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxProcessor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProcessor.SelectedIndex !=0)
            {
               tp.SetRecordProcessor((string)comboBoxProcessor.SelectedValue);
                //Clear fields mappings
               textBoxFieldsMapping.Text = "";
            }
        }


	}
}
