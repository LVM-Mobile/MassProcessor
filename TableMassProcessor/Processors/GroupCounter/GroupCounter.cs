using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using RecordProcessor;

namespace TableProcessorNS
{
    public class GroupCounter: BaseRecordProcessor
    {
        private Dictionary<string, int> counts;
        private static Dictionary<Field, bool> _knownFields = new Dictionary<Field, bool>();
        //Known fields for This class
        public override Dictionary<Field, bool> GetKnownFields()
        {
            return _knownFields;
        }        
                
        public GroupCounter()
        {
            if(KnownFields.Count==0)
            {
                KnownFields[new Field("GroupField", true, new string [] {"»ндекс"})] = true;
                KnownFields[new Field("CountField", true, new string[] { "кол-во на о/св" })] = false;
            }

            counts = new Dictionary<string, int>();
        }

        public override void Reset()
        {
            counts.Clear();
        }

        public override bool ProcessRecord(IDataRecord record)
        {
            string name = GetStringFieldValue(record, "GroupField");
            int count = GetIntFieldValue(record, "CountField");
            
            //Calculate 
            if(counts.ContainsKey(name))
             {
              counts[name] += count;
             }   
            else{
                counts[name] = count;
            }
            return true;
        }

        public override DataTable OutputData()
        {
         DataTable data = new DataTable();
         data.Columns.Add("GroupField", typeof(string));
         data.Columns.Add("CountField", typeof(int));

         foreach (KeyValuePair<string, int> row in counts)
         {
             data.Rows.Add(row.Key, row.Value);
         }
            return data;
        }
    }
}

