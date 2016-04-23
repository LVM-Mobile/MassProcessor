using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using OfficeOpenXml;
using System.IO;

namespace DatabaseAdapter
{
    public class XLSXDatabaseAdapter : IDatabaseAdapter, IDisposable
    {
        ExcelPackage data;
        ExcelWorksheet curWorksheet;
        int iRow;
        DataTable dataTable;

        public XLSXDatabaseAdapter()
        {
         knownFileTypes["Excel 2007/2012"] = "*.xlsx";
        }

        public string FileName { get; set; }
        public Dictionary<string, string> KnownFileTypes
        { get { return knownFileTypes; } }
        private Dictionary<string, string> knownFileTypes = new Dictionary<string, string>();

        public void Connect() {
            Connect(FileName);
        }
        
        public void Connect(string filename) {
            FileInfo info = new FileInfo(filename);
            if (!info.Exists)
                throw new Exception(string.Format("File {0} is not exists", filename));

            data = new ExcelPackage(info, true); 
        }

       
        public string[] GetTables() { 
            Connect(FileName);
            List<string> tables = new List<string>();
            foreach(var sheet in data.Workbook.Worksheets)
                tables.Add(sheet.Name);
            return tables.ToArray();
        }

        public Dictionary<string, int> GetFields(string filename, string tablename) 
        {
            Connect(filename);
            return GetFields(tablename);
        }

        public Dictionary<string, int> GetFields(string tablename) 
        {
            Dictionary<string, int> columnNames = new Dictionary<string, int>();
            ExcelWorksheet sheet = data.Workbook.Worksheets[tablename];

            foreach (var firstRowCell in sheet.Cells[sheet.Dimension.Start.Row, sheet.Dimension.Start.Column, 1, sheet.Dimension.End.Column])
            {
                columnNames[firstRowCell.Text] = firstRowCell.Start.Column-1;//Index starting from 0
            }
            return columnNames;
        }

        public void SetTable(string tablename)
        {
            //Set variables
            dataTable = new DataTable(tablename);
            curWorksheet = data.Workbook.Worksheets[tablename];

            Dictionary<string, int> fields = GetFields(tablename);
            foreach (var field in fields.Keys)
            {
                dataTable.Columns.Add(field);
            }
        }

        public DbDataReader Execute(string sql) { 
            throw new Exception("No suitable for this kind of adapter");
        }
        
        
        public string NormTableName(string tablename) 
        {
            return tablename;
        }
        
        public void Close() 
        {
            Dispose();
        }
        
        //Direct block
        public void FirstRecord() {
            iRow = 1; //Skip first row as Header            
        }
        
        public IDataRecord CurrentRecord() {
            throw new Exception("Not implemented");
        }

        public int GetRowsCount()
        {
            return curWorksheet.Dimension.End.Row;
        }
        
        public DataRow CurrentRow()
        {
            DataRow row = dataTable.NewRow();
            
            for (int ic = 0; ic < dataTable.Columns.Count; ic++)
            {
                row[ic] = curWorksheet.Cells[iRow, ic+1].Value;
            }
            return row;
        }

        public void UpdateRow(int ir, DataRow row)
        {
            for (int ic = 0; ic < row.ItemArray.Length; ic++)
            {
                curWorksheet.Cells[ir, ic + 1].Value = row[ic];
            }            
        }

        public bool NextRecord() {
            if (iRow >= curWorksheet.Dimension.End.Row) return false;
            iRow++;
            return true;
        }
        
        public void Write(string filename) { 
            FileInfo fi = new FileInfo(filename);
            data.SaveAs(fi);
        }

        public void Write(DataTable table)
        {
            throw new Exception("Not implemented");
        }

        public void Dispose()
        {
            data.Dispose();
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
