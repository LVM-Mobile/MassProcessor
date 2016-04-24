using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using DotNetDBF;

namespace DatabaseAdapter
{
    public class DBFDatabaseAdapter: BaseFileDatabaseAdapter, IDisposable
    {
        DBFReader dbf;
        DBFWriter dbfWriter;

        object[] currentRow;
        DataTable dataTable;
        
        public DBFDatabaseAdapter()
        {
            KnownFileTypes["DBF"] = "*.dbf";
        }

        public string FileName
        {
            get { return  base.FileName; }
            set {
                var filename = value;
                if (filename.ToLower().Substring(filename.Length - 4) == ".dbf" || File.Exists(filename))
                 base.FileName = Path.GetDirectoryName(filename);
                else
                    base.FileName = filename; 
             }
        }

     

        public void Connect()
        {
            if (!Directory.Exists(FileName))
                throw new Exception("Directory does not exists " + FileName);
        }

       
        public string[] GetTables()
        {
           var list=  Directory.GetFiles(FileName, "*.dbf");
           var tables = new List<string>();
            foreach (var fn in list)
            {
                tables.Add(Path.GetFileNameWithoutExtension(fn));
           
           }
            return tables.ToArray();
        }

       
        public Dictionary<string, int> GetFields(string tablename)
        {
            Connect(FileName);
            SetTable(tablename);
            Dictionary<string, int> fields = new Dictionary<string, int>();
            dataTable = new DataTable(tablename);
            int i = 0;
            foreach (var field in dbf.Fields)
            {
                fields[field.Name] = i++;
                dataTable.Columns.Add(field.Name);
            }
            return fields;
        }

        public System.Data.Common.DbDataReader Execute(string sql)
        {
            throw new NotImplementedException();
        }

        public void SetTable(string tablename)
        {
            //Oped table file
            string filename = Path.Combine(FileName, tablename + ".dbf");

            dbf = new DotNetDBF.DBFReader(filename);
            dbf.CharEncoding = Encoding.GetEncoding("cp866");
        }

        public void Write(System.Data.DataTable table)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            dbf.Close();
        }

        public void FirstRecord()
        {
            Connect();
        }
        
        public System.Data.IDataRecord CurrentRecord()
        {

            throw new Exception("Not implemented");
        }

        public System.Data.DataRow CurrentRow()
        {
           
            DataRow row = dataTable.NewRow();

            for (int ic = 0; ic < currentRow.Length; ic++)
            {
                row[ic] = currentRow[ic];
            }
            return row;
        }

        public void UpdateRow(int irow, System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        
        public bool NextRecord()
        {
            currentRow = dbf.NextRecord();
            return currentRow.Length > 0;
        }

        public void Write(string filename)
        {
            throw new NotImplementedException();
        }

        public int GetRowsCount()
        {
            return dbf.RecordCount;
        }

        public void Dispose()
        {
            dbf.Dispose();
        }

        public void SeekRecord(int position)
        {
            FirstRecord();
            int iRow = 0;
            while(iRow < position){
                NextRecord();
                iRow++;
            }
        }
       
    }
}
