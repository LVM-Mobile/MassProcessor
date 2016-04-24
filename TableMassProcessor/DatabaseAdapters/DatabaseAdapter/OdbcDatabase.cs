using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Globalization;


namespace DatabaseAdapter
{
    public class OdbcDatabaseAdapter : BaseFileDatabaseAdapter, IDisposable
    {
        OdbcConnection inConn;        
        private string ConnectionString;
       
        
        private string basedir;
        private string extension;

       
        public Dictionary<string, int> GetFields(string tablename)
        {
            Dictionary<string, int> FieldsMap = new Dictionary<string, int>();
            OdbcDataReader reader;
            string sql = "SELECT * FROM " + NormTableName(tablename);
            OdbcCommand command = new OdbcCommand(sql, inConn);
            inConn.Open();
            reader = command.ExecuteReader(CommandBehavior.KeyInfo);
            DataTable schemaTable = reader.GetSchemaTable();
            //assign mappings to current table            
            Dictionary<string, int> fieldsmap = new Dictionary<string, int>();
            int fieldIndex = 0;
            foreach (DataRow myField in schemaTable.Rows)
            {
                string columnName = myField["ColumnName"].ToString();
                FieldsMap[columnName] = fieldIndex;
                fieldIndex++;
            }
            inConn.Close();
            return FieldsMap;
        }


        public OdbcDatabaseAdapter()
        {
            
            KnownFileTypes["Excel 2000/2003"] = "*.xls";
            KnownFileTypes["Excel 2000/2007"] = "*.xlsx";
            KnownFileTypes["DBase"] = "*.dbf";
            //KnownFileTypes["Text Table"] = "*.csv";
        }

        public void Connect()
        {
            //Check if FileName - is dir (usable for multiple dbf files processing)
            if (System.IO.Directory.Exists(FileName))
            {
                basedir = FileName;
                extension = ""; //take from dir OR ask
            }
            else
            {
                basedir = Path.GetDirectoryName(FileName);
                extension = Path.GetExtension(FileName);
            }

            //Test known formats
            switch (extension)
            {
                case ".dbf":
                    ConnectionString = @"Driver={Microsoft dBASE Driver (*.dbf)};DriverID=277;Dbq=" + basedir;
                    //ConnectionString = "Provider=VFPOLEDB.1;Data Source =" +filename+ ";";
                    //ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=dBase III;HDR=Yes";
                    //ConnectionString = "Provider=MSDASQL/SQLServer ODBC;Driver={Microsoft Visual FoxPro Driver};" + @"SourceType=DBF;SourceDB=" + filename + ";InternetTimeout=300000;Transact Updates=True;Exclusive=No;BackgroundFetch=Yes;Collate=Russian;Null=No;Deleted=No";
                    //ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=dBASE IV;User ID=Admin;Password=";
                    /*OdbcConnectionStringBuilder sb = new OdbcConnectionStringBuilder();
			
                    sb.Driver = "{Microsoft dBASE Driver (*.dbf)}";
                    sb.Add("DriverID","277");
                    sb.Add("Dbq", filename);
                    string ConnectionString = sb.ConnectionString;
                    */
                    break;
                case ".xls":
                    ConnectionString = @"Driver={Microsoft Excel Driver (*.xls)};DriverId=790;Dbq=" + FileName;
                    break;
                case ".xlsx":

                    //ConnectionString = Settings.Default.XLSX;
                    ConnectionString = @"DRIVER=Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb);ReadOnly=1;PageTimeout=5;maxbuffersize=2048;MaxScanRows=8;DriverId=1046;DBQ=" + FileName;
                    //ConnectionString = @"Dsn=Excel Files;dbq=" + filename + ";defaultdir=" + basedir + ";driverid=1046;maxbuffersize=2048;pagetimeout=5";
                    //                    @"DRIVER={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};ReadOnly=1;PageTimeout=5;MaxScanRows=8;FIL=excel 12.0;DriverId=1046;DBQ=" + filename;
                    //                    ConnectionString = @"Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm,*.xlsb)};DriverID=1046; READONLY=0;DBQ=" + filename;
                    //ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + "Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";
                    break;
                case ".csv":
                    //TODO:LumenWorks.Framework.IO.Csv.CsvReader();
                    break;

                default:
                    throw new Exception("unknown dataset type");
            }
            inConn = new OdbcConnection(ConnectionString);
        }

        public void Close()
        {
            inConn.Close();
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

        

        
        public void SetTable(string tablename)
        {
            throw new Exception("Not implemented");
        }

        public DbDataReader Execute(string sql)
        {
            OdbcCommand command = new OdbcCommand(sql, inConn);
            inConn.Open();
            OdbcDataReader reader;
            reader = command.ExecuteReader();
            return reader;
        }
/*        
        //Write Whole Table data
        public void Write(DataTable data)
        {
          //TODO :CSV Writer
            //Set list separator from This Machine regional Settings
            CsvWriter.columnDelimiter = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            StreamWriter writer = new StreamWriter(FileName);
            CsvWriter.WriteToStream(writer, data, true, false);
            writer.Close();
            writer = null;
        }
*/
        public void Write(DataTable table)
        {
            throw new Exception();             
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
        
        public int GetCount() {
            throw new Exception(); 
        }

        //Direct block
        
        public void FirstRecord() { throw new Exception(); }
        public IDataRecord CurrentRecord() { throw new Exception(); }
        public DataRow CurrentRow() { throw new Exception(); }
        public bool NextRecord() { throw new Exception(); }
        public void Write(string filename) { throw new Exception(); }
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
            return 0;//For statistics mainly
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
