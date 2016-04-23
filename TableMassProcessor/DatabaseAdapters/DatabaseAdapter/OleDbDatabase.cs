using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;


namespace DatabaseAdapter
{
    public class OleDbDatabaseAdapter : IDatabaseAdapter, IDisposable
    {
        OleDbConnection inConn;        
        private string ConnectionString;

        public string FileName { get; set; }
        public Dictionary<string, string> KnownFileTypes
        { get { return knownFileTypes; } }
        private Dictionary<string, string> knownFileTypes = new Dictionary<string, string>();
        
        private string basedir;
        private string extension;

        
        public OleDbDatabaseAdapter()
        {
            
            KnownFileTypes["Excel 2000/2003"] = "*.xls";
            KnownFileTypes["Excel 2000/2007"] = "*.xlsx";
            KnownFileTypes["DBase"] = "*.dbf";
//            KnownFileTypes["Text Table"] = "*.csv";
        }

        public void Connect()
        {
            Connect(FileName);
        }
        public void Close()
        {
            inConn.Close();
        }

        public void Connect(string filename)
        {
            //Check if FileName - is dir (usable for multiple dbf files processing)
            if (System.IO.Directory.Exists(filename))
            {
                basedir = FileName;
                extension = ""; //take from dir OR ask
            }
            else
            {
                basedir = Path.GetDirectoryName(filename);
                extension = Path.GetExtension(filename);
            }

            //Test known formats
            switch (extension)
            {
                case ".dbf":
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=dBASE IV;User ID=Admin;Password=";
                    break;
                case ".xls":
                    ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"{0}\";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"",  filename);
                    break;
                case ".xlsx":
                    ConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"{0}\";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";",  filename);
                    break;
                case ".csv":
            //        ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + "Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";
                    break;

                default:
                    throw new Exception("unknown dataset type");
            }
            inConn = new OleDbConnection(ConnectionString);
        }

        public string[] GetTables()
        {
            Connect(FileName);
            inConn.Open();
            List<string> tableNames = new List<string>();
            DataTable tablesSchema = inConn.GetSchema("Tables");
            foreach (DataRow tableInfo in tablesSchema.Rows)
            {
                tableNames.Add(tableInfo["TABLE_NAME"].ToString());
            } 
            inConn.Close();
            return tableNames.ToArray();
        }

        
        public Dictionary<string, int> GetFields(string tablename)
        {
            Connect(FileName);

            Dictionary<string, int> fieldsMap = new Dictionary<string, int>();
            OleDbDataReader reader;
            string sql = "SELECT * FROM " + NormTableName(tablename);
            OleDbCommand command = new OleDbCommand(sql, inConn);
            inConn.Open();
            reader = command.ExecuteReader(CommandBehavior.KeyInfo);
            DataTable schemaTable = reader.GetSchemaTable();
            //assign mappings to current table            
            Dictionary<string, int> fieldsmap = new Dictionary<string, int>();
            int fieldIndex = 0;
            foreach (DataRow myField in schemaTable.Rows)
            {
                string columnName = myField["ColumnName"].ToString();
                fieldsMap[columnName] = fieldIndex;
                fieldIndex++;
            }
            inConn.Close();
            return fieldsMap;
        }

        public void SetTable(string tablename)
        {
            throw new Exception("Not implemented");
        }

        public DbDataReader Execute(string sql)
        {
            OleDbCommand command = new OleDbCommand(sql, inConn);
            inConn.Open();
            DbDataReader reader;
            reader = command.ExecuteReader();
            return reader;
        }
        
        public string NormTableName(string tablename)
        {
            switch (extension)
            {
                default: break;
                case ".xlsx":
                case ".xls":
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
                    break;
            }
            return tablename;
        }

        //Direct block
        public void FirstRecord() { throw new Exception(); }
        public IDataRecord CurrentRecord() { throw new Exception(); }
        public bool NextRecord() { throw new Exception(); }
        public void Write(string filename) { throw new Exception(); }

        public void Write(DataTable data)
        {
            throw new Exception();
        }
        
        public DataRow CurrentRow()
        {
            throw new Exception("Not implemented");
        }
        
        public void UpdateRow(int irow, DataRow row)
        {
            throw new Exception("Not implemented");
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public int GetRowsCount()
        {
            throw new NotImplementedException();
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
