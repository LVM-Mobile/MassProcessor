using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RecordProcessor;
using TableProcessorNS;

namespace TableProcessorApp
{
    public partial class FormFieldsMap : Form
    {
        /// <summary>
        /// SQL Fields name-db index
        /// </summary>
        FieldsMap map = new FieldsMap();

        public Dictionary<Field, bool> targetFields
        {
            get { return map.targetFields; }
            set { map.targetFields  = value;}
        }

        public Dictionary<string, int> inputFields
        {
            get { return map.inputFields; }
            set { map.inputFields = value; }
        }

        public Dictionary<string, string> Target2InputFields
        {
            get { return map.Target2InputFields; }
            set { map.Target2InputFields = value; }
        }                

        public FormFieldsMap()
        {
            InitializeComponent();
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void FormFieldsMap_Load(object sender, EventArgs e)
        {
            DataGridViewComboBoxColumn inputFieldColumn = new DataGridViewComboBoxColumn();
            inputFieldColumn.Name = "inputField";

            //Set selection range to input fields
            dataGridViewFieldsMap.Columns.Remove("inputField");
            foreach (KeyValuePair<string, int> fieldDef in inputFields)
            {
                inputFieldColumn.Items.Add(fieldDef.Key);
            }
            dataGridViewFieldsMap.Columns.Add(inputFieldColumn);

            map.AutoMap();
            
            setMapping();
        }

        

        void setMapping()
        {
            //set list of needed fields
            dataGridViewFieldsMap.Rows.Clear();
            foreach(KeyValuePair<Field, bool> pair in map.targetFields)
            {
                string targetFieldName = pair.Key.Name;
                if(pair.Key.IsRequired)
                    targetFieldName += "*";
                string inputFieldName = "";
                //Link output field to input field
                if (Target2InputFields.ContainsKey(pair.Key.Name))
                    inputFieldName = Target2InputFields[pair.Key.Name];
                
                dataGridViewFieldsMap.Rows.Add(new object[] { targetFieldName, inputFieldName});
            }

        }

        public void getMapping()
        {
          foreach(DataGridViewRow row in dataGridViewFieldsMap.Rows)
           {
               if (row.Cells[1].Value != null)
               {
                   string inputFiledName = row.Cells[1].Value.ToString();
                   string targetFieldName = row.Cells[0].Value.ToString();
                   
                   if(targetFieldName[targetFieldName.Length -1] =='*')
                       targetFieldName = targetFieldName.Substring(0, targetFieldName.Length - 1);

                   Target2InputFields[targetFieldName] = inputFiledName;
               }
           }
        }

        private void dataGridViewFieldsMap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}