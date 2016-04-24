using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace RecordProcessor
{
    public class BaseRecordProcessor : IRecordProcessor
    {
        IDictionary<string, object> _parameters = new Dictionary<string, object>();

        private string _OutputTemplate = null;

        public string OutputTemplate
        {
            get {return _OutputTemplate;}
            set {_OutputTemplate = value;}
        }

        string _tablename;
        Dictionary<string, int> _fieldsmap;
        
        public Dictionary<string, int> Fieldsmap
        {
            get { return _fieldsmap; }
            set { _fieldsmap = value; }
        }

        public BaseRecordProcessor()
        {
            _tablename = "";
            _fieldsmap = new Dictionary<string, int>();
        }

        public void setFieldsMap(Dictionary<string, int> fieldsmap)
        {
            _fieldsmap = fieldsmap;
        }
        
        private int GetFieldIdByName(string name)
        {
         int iDepartamentField;

         if (Fieldsmap.TryGetValue(name, out iDepartamentField) == false)
             throw new Exception("Required " + name + "field are not mapped");
         return iDepartamentField;
        }
        
        public void SetStringFieldValue(DataRow row, string name, string Value)
        {
            row[GetFieldIdByName(name)] = Value;
        }
        
        
        public string GetStringFieldValue(DataRow row, string name)
        {
            int iDepartamentField = Fieldsmap[name]; 
            return (row.IsNull(iDepartamentField)) ? "" : row[iDepartamentField].ToString();
        }
        
        public string GetStringFieldValue(IDataRecord record, string name)
        {
            int iDepartamentField = Fieldsmap[name];
            return (record.IsDBNull(iDepartamentField)) ? "" : record.GetValue(iDepartamentField).ToString();
        }
        
        public int GetIntFieldValue(IDataRecord record, string name)
        {
            int iCountField = Fieldsmap[name];
            int count;
            if (record.IsDBNull(iCountField))
                count = 0;
            else
            {
                Int32.TryParse(record.GetValue(iCountField).ToString(), out count);
            }
            return count;
        }

        public double GetDoubleFieldValue(IDataRecord record, string name)
        {
            int iCountField = Fieldsmap[name];
            double value;
            if (record.IsDBNull(iCountField))
                value = 0;
            else
            {
                Double.TryParse(record.GetValue(iCountField).ToString(), out value);
            }
            return value;
        }               

        #region IRecordProcessor Members
        public virtual void Reset()
        {

        }

        public virtual bool ProcessRecord(IDataRecord row)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool ProcessRow(DataRow row)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void ProcessData(DataTable data)
        {
           foreach(DataRow row in data.Rows)
           {
            ProcessRow(row);
           }
        }

        public virtual DataTable OutputData()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IDictionary<string, object> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public void setDatasetName(string tablename)
        {
            _tablename = tablename;
        }
        /// <summary>
        /// Retur all table fields by default
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetSelectStatements()
        {
            string sql = "SELECT * FROM " + _tablename;
            string[] inSQL = { sql };
            return inSQL;
        }
        public Dictionary<Field, bool> KnownFields
        {
            get { return GetKnownFields(); }
        }

        public virtual Dictionary<Field, bool> GetKnownFields()
        {
            throw new Exception("KnownFields must be defined in module class");
        }
        #endregion
    }
}
