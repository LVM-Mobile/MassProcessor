using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;

namespace DatabaseAdapter
{
    public class BaseFileDatabaseAdapter: IDatabaseAdapter
    {
       string _fileName;
       Dictionary<string, string> knownFileTypes = new Dictionary<string, string>();

        public string FileName
        {
            get { return _fileName; }
            set
            {
              _fileName = value;
            }
        }

        public Dictionary<string, string> KnownFileTypes
        {
            get { return knownFileTypes; }
        }


        virtual public void Connect(string filename)
        {
            //Set filename
            FileName = filename;
            Connect();
        }


        virtual public void Clone(IDatabaseAdapter OutputDatabase)
        {

            if (File.Exists(FileName) && OutputDatabase.FileName != string.Empty)
            {
                //Copy file to create new
                File.Copy(FileName, OutputDatabase.FileName, true);
            }
            else {
                //Directory base with file tables Like DBF
                if (Directory.Exists(FileName))
                {
                    Directory.CreateDirectory(OutputDatabase.FileName);
                    var files = Directory.GetFiles(FileName);
                    foreach (var fn in files)
                    {
                        string out_fn = Path.Combine(OutputDatabase.FileName, Path.GetFileName(fn));
                        if (File.Exists(fn))
                            File.Copy(fn, out_fn, true);
                    }
                }
            }
        }

        virtual public void Connect()
        {
            throw new NotImplementedException();
        }

        virtual public string[] GetTables()
        {
            throw new NotImplementedException();
        }

        virtual public Dictionary<string, int> GetFields(string tablename)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> GetFields(string filename, string tablename)
        {
            Connect(filename);
            return GetFields(tablename);
        }

        virtual public DbDataReader Execute(string sql)
        {
            throw new NotImplementedException();
        }

        virtual public void SetTable(string tablename)
        {
            throw new NotImplementedException();
        }

        virtual public string NormTableName(string tablename)
        {
            return tablename;
        }

        virtual public void Write(DataTable table)
        {
            throw new NotImplementedException();
        }

        virtual public void Close()
        {
            throw new NotImplementedException();
        }

        virtual public void FirstRecord()
        {
            throw new NotImplementedException();
        }

        virtual public void SeekRecord(int position)
        {
            throw new NotImplementedException();
        }

        virtual public IDataRecord CurrentRecord()
        {
            throw new NotImplementedException();
        }

        virtual public DataRow CurrentRow()
        {
            throw new NotImplementedException();
        }

        virtual public void UpdateRow(int irow, DataRow row)
        {
            throw new NotImplementedException();
        }

        virtual public bool NextRecord()
        {
            throw new NotImplementedException();
        }

        virtual public void Write(string filename)
        {
            throw new NotImplementedException();
        }

        virtual public int GetRowsCount()
        {
            throw new NotImplementedException();
        }
    }
}

