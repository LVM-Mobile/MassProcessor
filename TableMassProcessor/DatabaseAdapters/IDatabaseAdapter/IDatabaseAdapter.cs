using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;


namespace DatabaseAdapter
{
    public interface IDatabaseAdapter
    {
        string FileName { get; set; }
        Dictionary<string, string> KnownFileTypes { get; }
      
        /// <summary>
        /// Clone complete database
        /// </summary>
        /// <param name="outDatabase"></param>
        void Clone(IDatabaseAdapter outDatabase );
        //TODO: void Restore();
        void Connect();
        void Connect(string filename);
        string[] GetTables();
        Dictionary<string, int> GetFields(string tablename);
        //use reader technic
        DbDataReader Execute(string sql);
        //Alternative 
        void SetTable(string tablename);

        string NormTableName(string tablename);
        void Write(DataTable table);
        void Close();

        //Direct block
        
        void FirstRecord();
        void SeekRecord(int position);
        IDataRecord CurrentRecord();
        DataRow CurrentRow();
        void UpdateRow(int irow, DataRow row);
        bool NextRecord();
        void Write(string filename);
        int GetRowsCount();

      
    }
}
