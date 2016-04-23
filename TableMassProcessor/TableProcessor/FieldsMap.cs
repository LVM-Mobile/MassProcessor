using System;
using System.Collections.Generic;
using System.Text;
using RecordProcessor;

namespace TableProcessorNS
{
    public class FieldsMap
    {
        /// <summary>
        /// Input fields
        /// </summary>
        public Dictionary<string, int> inputFields;
        /// <summary>
        /// Transfer fields
        /// </summary>
        public Dictionary<Field, bool> targetFields;
        
        /// <summary>
        /// Map from 
        /// </summary>
        public Dictionary<string, string> Target2InputFields = new Dictionary<string, string>();

        public void AutoMap()
        {
            //For each target field try to find input field by one of aliases
            foreach (KeyValuePair<Field, bool> outFieldDef in targetFields)
            {
                //Find input field by name
                foreach (KeyValuePair<string, int> inFieldDef in inputFields)
                    
                    //Check case insensitive if present in list
                    if (outFieldDef.Key.Aliases.Contains(inFieldDef.Key.ToLower()))
                        Target2InputFields[outFieldDef.Key.Name] = inFieldDef.Key;
            }
        }


    }
}
