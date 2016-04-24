using System;
using System.Collections.Generic;
using System.Data;

namespace RecordProcessor
{
	public interface IRecordProcessor
	{
        /// <summary>
        /// Reset processor
        /// </summary>
        void Reset();
        /// <summary>
        /// set working table 
        /// </summary>
        /// <param name="tablename">table name</param>
        void setDatasetName(string tablename);
        /// <summary>
        /// Set fields mapping
        /// </summary>
        /// <param name="fieldsmap">dictionary field name to field index in working dataset</param>
        void setFieldsMap(Dictionary<string, int> fieldsmap);
        /// <summary>
        /// Select statements to process input data
        /// </summary>
        /// <returns></returns>
        string[] GetSelectStatements();
        
        /// <summary>
        /// fields for processing. FieldName-IsRequired
        /// </summary>
        Dictionary<Field, bool> KnownFields { get; }
        /// <summary>
        /// Every module have to define own function
        /// </summary>
        /// <returns></returns>
        Dictionary<Field, bool> GetKnownFields();
        
        /// <summary>
        /// Input/Output parameters by name
        /// </summary>
        IDictionary<string, object> Parameters {get; set;}
        
        /// <summary>
        /// Process record
        /// </summary>
        /// <param name="row">input record</param>
		bool ProcessRecord(IDataRecord record);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        bool ProcessRow(DataRow row);
        /// <summary>
        /// Process all data
        /// </summary>
        /// <param name="data">table to process</param>
        void ProcessData(DataTable data);
        
        /// <summary>
        /// Return processed data table. See also Parameters 
        /// </summary>
        /// <returns></returns>
        DataTable OutputData();
        /// <summary>
        /// Output template name. If empty write table, else make retport
        /// </summary>
        string OutputTemplate { get;set;}

        
    }
}
