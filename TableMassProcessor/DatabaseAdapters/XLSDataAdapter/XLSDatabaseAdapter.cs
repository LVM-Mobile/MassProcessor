using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.in2bits.MyXls;
using org.in2bits.MyOle2;
using org.in2bits.MyOle2.Metadata;
using System.Data;
using System.IO;

using System.Data.Common;

namespace DatabaseAdapter
{
    public class XLSDatabaseAdapter: IDatabaseAdapter
    {
        private XlsDocument excelDoc;
        private Worksheet curSheet;
        int iRow;
        DataTable dataTable;

        public string FileName { get; set; }
        public Dictionary<string, string> KnownFileTypes
        { get { return knownFileTypes; } }
        private Dictionary<string, string> knownFileTypes = new Dictionary<string, string>();

        public XLSDatabaseAdapter()
        {
            knownFileTypes["Excel 2000/2003"] = "*.xls";
        }
        
        public void Connect()
        {
            Connect(FileName);
        }
        public void Connect(string filename)
        {
            excelDoc = new XlsDocument(filename);
        }

        public string[] GetTables()
        {
            Connect(FileName);
            List<string> list = new List<string>();
            foreach (var worksheet in excelDoc.Workbook.Worksheets)
            {
                list.Add(worksheet.Name);
            }
            return list.ToArray();
        }

        public Dictionary<string, int> GetFields(string filename, string tablename)
        {
            throw new Exception("Not implemented");
          /* var sheet = excelDoc.Workbook.Worksheets[tablename];
            
           return excelDoc.Workbook.Worksheets[tablename].Cells.
        */}
        public Dictionary<string, int> GetFields(string tablename) { throw new Exception(); }
        public void SetTable(string tablename)
        {
            curSheet = excelDoc.Workbook.Worksheets[tablename];
        }

        public DbDataReader Execute(string sql) { throw new Exception(); }

        public string NormTableName(string tablename)
        {
            /*TODO: Check if we need it
            tablename.Replace('.', '#');
            //Take from all
            if (tablename.Length == 0)
            {
                tablename = "*";
            }
            switch (tablename[tablename.Length - 1])
            {
                case '\'':
                    if (tablename[tablename.Length - 2] != '$')
                        tablename += '$';
                    break;
                case '$':
                    break; //add nothing
                default:
                    tablename += '$';
                    break;
            }
            tablename = "[" + tablename + "]";
             */
            return tablename;
        }

        public void Close() { throw new Exception(); }

        //Direct block
        public void FirstRecord() { throw new Exception(); }
        public IDataRecord CurrentRecord() { throw new Exception(); }
        public DataRow CurrentRow() { throw new Exception(); }
        public void UpdateRow(int ir, DataRow row) { throw new Exception(); }
        public bool NextRecord() { throw new Exception(); }
        public void Write(string filename) { throw new Exception(); }

        public void Write(DataTable table)
        {
            using (var file2Save = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                XlsDocument newDoc = new XlsDocument();
                Workbook wbk = newDoc.Workbook;
                Worksheet sht = wbk.Worksheets.Add("Result");

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    var col = table.Columns[i];
                    sht.Cells.Add(1, i + 1, col.ColumnName);
                }

                // заполняем результат работы
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    for (int ic = 0; ic < row.ItemArray.Length; ic++)
                    {
                        var cell = (row[ic] != null) ? row[ic] : string.Empty;
                        sht.Cells.Add(2 + i, 1 + ic, cell.ToString());
                    }
                }
                newDoc.Save(file2Save);
                file2Save.Close();
            }
            using (var file2Save = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                XlsDocument newDoc = new XlsDocument();
                Workbook wbk = newDoc.Workbook;
                Worksheet sht = wbk.Worksheets.Add("Result");

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    var col = table.Columns[i];
                    sht.Cells.Add(1, i + 1, col.ColumnName);
                }

                // заполняем результат работы
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    for (int ic = 0; ic < row.ItemArray.Length; ic++)
                    {
                        var cell = (row[ic] != null) ? row[ic] : string.Empty;
                        sht.Cells.Add(2 + i, 1 + ic, cell.ToString());
                    }
                }
                newDoc.Save(file2Save);
                file2Save.Close();
            }
        }


        public int GetRowsCount()
        {
            return excelDoc.Workbook.Worksheets[0].Rows.Count;
        }

        public void SeekRecord(int position)
        {
            FirstRecord();
            int iRow = 0;
            while (iRow < position)
            {
                NextRecord();
                iRow++;
            }
        }
       

      
    }
}
