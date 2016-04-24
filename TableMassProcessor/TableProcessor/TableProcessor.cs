using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Collections.Generic;
using RecordProcessor;
using LVMSoft.Progress;
//using OfficeReporting;
using DatabaseAdapter;

namespace TableProcessorNS
{
    public enum ProcessMode { pmEdit, pmETL };

    public class TableProcessor
    {
        
        private IProgressMonitor _progress;

        public int ProcessOffset;

        public ProcessMode ProcessMode;
        public IProgressMonitor Progress
        {
            get { return _progress; }
            set { _progress = value; }
        }
        /// <summary>
        /// Input database to "Extract" data
        /// </summary>
        private IDatabaseAdapter inputDatabase;
        
        public IDatabaseAdapter InputDatabase
        {
            get{return inputDatabase;}
            set { inputDatabase = value; }
        }

            

        /// <summary>
        /// Selected table to "Transform" data from inputDatabase
        /// </summary>
        public string tableName {get;set;}
        /// <summary>
        /// Record processor interface to process records
        /// </summary>
        IRecordProcessor rp;
        /// <summary>
        /// Fields mapping received from user
        /// </summary>
        private List<KeyValuePair<string, string>> inputFieldNamesMap = new List<KeyValuePair<string, string>>();

        public List<KeyValuePair<string, string>> InputFieldNamesMap
        {
            get { return inputFieldNamesMap; }
            set { inputFieldNamesMap = value; }
        }
        /// <summary>
        /// output database to "Load" output data
        /// </summary>
        private IDatabaseAdapter outputDatabase;

        public IDatabaseAdapter OutputDatabase
        {
            get {
                if (outputDatabase == null)
                    return outputDatabase;
                return outputDatabase; 
            }
            set { outputDatabase = value; }
        }

        public TableProcessor()
        {
            ProcessOffset = 0;
        }
        
		public string getRecordProcessorName(string filename){
			return rp.GetType().Name;
		}

		public void SetRecordProcessor(string filename)
		{
			if(rp != null && rp.GetType().Name == Path.GetFileNameWithoutExtension(filename))
			{
				 return; //already loaded
			}
			rp = TableProcessor.LoadRecordProcessor(filename);
		}

        private void SetRecordProcessor(Assembly irp)
        {
            rp = TableProcessor.LoadRecordProcessor(irp);
        }
		
        public static IRecordProcessor LoadRecordProcessor(string filename)
		{
            Assembly Dynamic = Assembly.LoadFrom(filename);
			return LoadRecordProcessor( Dynamic);
		}

        public static IRecordProcessor LoadRecordProcessor(Assembly Dynamic)
        {
            Type TypeofIRecordProcessor = typeof(IRecordProcessor);
            foreach (Type Exported in Dynamic.GetExportedTypes())
            {
                if (TypeofIRecordProcessor.IsAssignableFrom(Exported))
                {
                    //return (IRecordProcessor)Activator.CreateInstance(InputElementClass);
                    ConstructorInfo Constructor = Exported.GetConstructor(System.Type.EmptyTypes);
                    if (Constructor == null)
                        throw new Exception("Failed to bind " + Dynamic.FullName + ". Classes has no proper Constructor");

                    // Type supports IMatch and has an no-parameter constructor
                    IRecordProcessor Instance = (IRecordProcessor)Constructor.Invoke(System.Type.EmptyTypes);
                    return Instance;
                }
            }
            throw new Exception("Failed to bind " + Dynamic.FullName + ". No acceptable Classes found");
        }

        public static Dictionary<string, Assembly> GetAvailableProcessors(string dir, IEnumerable<string> modules)
        {
            Dictionary<string, Assembly> list = new Dictionary<string, Assembly>();
            foreach (var modulename in modules)
            {
                try
                {
                    var ass = Assembly.LoadFrom(Path.Combine(dir, modulename));
                    list[modulename]= ass;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return list;
        }

        public void Process()
		{
			Process(rp);
		}

        void MapFields(IRecordProcessor rp)
        {
            Dictionary<string, int> inputFields = inputDatabase.GetFields(tableName);
            Dictionary<string, int> fieldsMap = new Dictionary<string, int>();

            foreach (KeyValuePair<string, string> fieldpair in inputFieldNamesMap)
            {
                string transferColName = fieldpair.Value;
                if (fieldpair.Key.Length >0)
                {
                    int fieldIndex = inputFields[fieldpair.Key];
                    fieldsMap[transferColName] = fieldIndex;
                }
            }
            rp.setFieldsMap(fieldsMap);
        }

		public void Process(IRecordProcessor rp)
        {            
            //Try to copy original to output database
            if (ProcessMode == ProcessMode.pmEdit)
            {
                ProcessEditMode(rp);
            }
            else {
                ProcessETLMode(rp);
            }
        }
        
        public void ProcessETLMode(IRecordProcessor rp)
        {
            inputDatabase.Connect();
            MapFields(rp);

            rp.setDatasetName(inputDatabase.NormTableName(tableName));
            

            string[] sqls = rp.GetSelectStatements();

            if (Progress != null)
                Progress.ReportProgress(0);
			foreach(string sql in sqls)
				{
                   try
                    {
                        DbDataReader reader = inputDatabase.Execute(sql);
                        int count=1;
                        rp.Reset();
                        //Extract
                        while (reader.Read())
                        {
                            //Transform
                            rp.ProcessRecord(reader);
                            if (Progress != null)
                                Progress.ReportProgress(0, "Read records: " + count.ToString());
                            count++;

                        }
                        reader.Close();
                        reader = null;

                        //Load
                        if (outputDatabase.FileName != null)
                        {
                          //if output is file - Write one
                          WriteData();
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
					// Call Close when done reading.
				}
                inputDatabase.Close();
        }

        public void ProcessEditMode(IRecordProcessor rp)
        {
            inputDatabase.Connect();
            outputDatabase.Connect();
            MapFields(rp);
            rp.setDatasetName(inputDatabase.NormTableName(tableName));
            
            if (Progress != null)
                Progress.ReportProgress(0);
                try
                {


                    int count = ProcessOffset;
                    int processedCount = 0; 
                    int totalCount;

                    rp.Reset();
                    
                    //Extract
                    inputDatabase.SetTable(tableName);
                    outputDatabase.SetTable(tableName);
                    
                    totalCount = inputDatabase.GetRowsCount();

                    if(ProcessOffset > 0)
                        inputDatabase.SeekRecord(ProcessOffset);
                    else
                        inputDatabase.FirstRecord();
                    
                    //!! Skip first line as it Header
                    while (inputDatabase.NextRecord()){
                        DataRow row = inputDatabase.CurrentRow();

                        //Transform
                        if(rp.ProcessRow(row))
                        {
                            //Load if row changed
                            outputDatabase.UpdateRow(count+1, row);

                            if (Progress != null)
                                Progress.ReportProgress((processedCount * 100) / (totalCount - ProcessOffset), string.Format("Records: Read {0} (Processed {1}) of {2}", count, processedCount, totalCount));
               
                            processedCount++;
                        }
                        count++;

                           }

                    //Load
                    if (outputDatabase.FileName != null)
                    {
                        outputDatabase.Write(outputDatabase.FileName);
                    }
                    //Set complete
                    Progress.ReportProgress(100, string.Format("Read records: {0}, Processed {1}", count, processedCount));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                // Call Close when done reading.
        
            inputDatabase.Close();
            outputDatabase.Close();
        }

        public void WriteData()
        {
            DataTable data = rp.OutputData();
            //we have sometihng to write after all
            if (data != null && outputDatabase.FileName != null)
            {
                /*TODO: Make output filters
                if (rp.OutputTemplate!=null){
                    MakeReport(outputDatabase.FileName, rp.OutputTemplate, data);
                }
                else
            */    
            {
                    //Write raw output
                    outputDatabase.Write(data);
                }
            }
        }

        public Dictionary<Field, bool> GetTransformFields()
        {
            return rp.KnownFields;
        }

 /*       public void MakeReport(string filename, string templateName, DataTable outputData)
        {
            string templateFileName = AppDomain.CurrentDomain.BaseDirectory + "reports/" + templateName;
            ExcelInterop output = new ExcelInterop();
            //Select report template
            output.SetReport(templateFileName);
            
            if (Progress != null)
                Progress.ReportProgress(0);
            int count = 0;
            //Prepare output table for list
            foreach (DataRow row in outputData.Rows)
            {
                object[] cells = row.ItemArray;
                for(int icol=0; icol <cells.Length;icol++)
                {
                    string colName = outputData.Columns[icol].ColumnName;
                    object cell = cells[icol];
                    if(cell is DataTable)
                    {
                        output.DataSources[colName] = (DataTable)cell;
                    }
                    else{
                        output.Parameters[colName] = cell;
                    }                   
                }
                //Render data to report
                output.PrepareReport();

                count++;
                if (Progress != null)
                    Progress.ReportProgress(count *100/outputData.Rows.Count, string.Format("Make report: {0}/{1}",count, outputData.Rows.Count));
            }
            //Save report
            output.SaveXLS(filename);
            //Close Excel
            output.Dispose();
            //We dont need to write raw data            
        }
*/
        public static List< KeyValuePair<string, string>> DeserializeFieldsMap(string listString)
        {
            //parse field mappings
            string[] fields = (listString.Length>0)?listString.Split(','):new string[0];
            List<KeyValuePair<string, string>> fieldsmap = new List<KeyValuePair<string, string>>();
            foreach (string field in fields)
            {
                string[] keyval = field.Split('=');
                fieldsmap.Add(new KeyValuePair<string,string>(keyval[0], keyval[1]));
            }
            return fieldsmap;
        }

        public static string SerializeFieldsMap(List<KeyValuePair<string, string>> fieldsmap)
        {
            string listString = "";

            foreach (KeyValuePair<string, string> fieldPair in fieldsmap)
            {
                if (listString.Length > 0)
                    listString += ',';
                listString += fieldPair.Key + '=' + fieldPair.Value;
            }
            return listString;
        }

        

    }
}
