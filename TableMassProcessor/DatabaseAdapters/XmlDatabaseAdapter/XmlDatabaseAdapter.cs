using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DatabaseAdapter;

namespace DatabaseAdapter
{
    public class XmlDatabaseAdapter: BaseFileDatabaseAdapter
    {
      
        XDocument doc;
        IEnumerable<XElement> data;
        int iRow;
        DataTable dataTable;
        
    
        public XmlDatabaseAdapter()
        {
            KnownFileTypes["XML documents"] = "*.xml";
        }

       
         override public void Connect()
        {
            doc = XDocument.Load(FileName);
            //XML Zero level node is root
            data = doc.Elements().First().Elements();
        }

      
        // Set filter - projects, tasks,
        override public string[] GetTables()
        {
            Connect(FileName);
            List<string> tables = new List<string>();
           
            //First level nodes =  table rows 
            foreach (var table in data)
            {
                string tableName = table.Name.LocalName;
                if(!tables.Contains(tableName))
                  tables.Add(tableName);
            }

            return tables.ToArray();
        }

        override public Dictionary<string, int> GetFields(string tablename)
        {
            //XML second level nodes = fields
            Dictionary<string, int> columnNames = new Dictionary<string, int>();
            var table = from d in data where d.Name == tablename select d;
            var rows = from t in table select t.Elements();
            var fields = rows.First().Elements();
            int i = 0;
            foreach (var fieldNode in fields)
            {
                string fieldName = fieldNode.Name.LocalName;
                columnNames[fieldName] = i++;

            }
            return columnNames;
        }

      
        override public void SetTable(string tablename)
        {
            //Set variables
            dataTable = new DataTable(tablename);
          
            Dictionary<string, int> fields = GetFields(tablename);
            foreach (var field in fields.Keys)
            {
                dataTable.Columns.Add(field);
            }
        }

     
        override public void Close()
        {
           
        }

         override public void FirstRecord()
        {
            iRow = 0; 
        }

         override public void SeekRecord(int position)
        {
            iRow = position;
           
        }

         override public DataRow CurrentRow()
        {
            DataRow row = dataTable.NewRow();

            for (int ic = 0; ic < dataTable.Columns.Count; ic++)
            {
                var colName = dataTable.Columns[ic].ColumnName;
                var dataRow = data.ElementAt(iRow);
                row[ic] = dataRow.Element(colName).Value;
            }
            return row;
        }

         override public bool NextRecord()
        {
            iRow++;
            return iRow < data.Count();
        }

     
         override public int GetRowsCount()
        {
            return data.Count();
        }
    }
}
