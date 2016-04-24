using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using RecordProcessor;

namespace TableProcessorNS
{
    public class ImportTimeSheet : BaseRecordProcessor
    {
        private Dictionary<string, int> counts;
        private static Dictionary<Field, bool> _knownFields = new Dictionary<Field, bool>();
        //Known fields for This class
        public override Dictionary<Field, bool> GetKnownFields()
        {
            return _knownFields;
        }

        public ImportTimeSheet()
        {
            if (KnownFields.Count == 0)
            {
                KnownFields[new Field("taskId", true, new string[] { "taskId" })] = true;
                KnownFields[new Field("projectId", true, new string[] { "projectId" })] = true;
                KnownFields[new Field("description", true, new string[] { "description" })] = true;
                KnownFields[new Field("startDate", true, new string[] { "startDate" })] = true;
                KnownFields[new Field("endDate", true, new string[] { "endDate" })] = true;
            }

           
        }


        public override bool ProcessRecord(IDataRecord record)
        {
            foreach (var field in KnownFields)
            {
                GetStringFieldValue(record, field.Key.Name);
            }

            //Put to Timesheet System

            return true;
        }

   
    }
}

