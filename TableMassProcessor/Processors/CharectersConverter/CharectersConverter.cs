using System;
using System.Collections.Generic;
using System.Text;
using RecordProcessor;
using System.IO;
using System.Data;
using System.Reflection;

namespace CharectersConverter
{
    public class CharectersConverter : BaseRecordProcessor
    {
        private int ConvertedCount;
        private int unknownCount;

        private Dictionary<char, char> ConvertPairsDic = new Dictionary<char,char>();
        private Dictionary<Field, bool> _knownFields = new Dictionary<Field, bool>();
        //Known fields for This class
        public override Dictionary<Field, bool> GetKnownFields()
        {
            return _knownFields;
        }

        public CharectersConverter()
        {
            if(KnownFields.Count==0)
            {
                KnownFields[new Field("InputField", true, new string[] { "InputField" })] = true;
                KnownFields[new Field("OutputField", true, new string[] { "OutputField" })] = true;
                
                string ModuleDir = Path.GetDirectoryName(new System.Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
                string Filename =  Path.GetFullPath(Path.Combine(ModuleDir, "Data/Cyr2Lat.txt"));
                string[] lines = File.ReadAllLines(Filename);
                foreach (var line in lines)
                {
                    ConvertPairsDic.Add(line[0], line[1]);
                }
            }

        }

        public override void Reset()
        {
            ConvertedCount = 0;
            unknownCount = 0;
        }

        public override bool ProcessRow(DataRow row)
        {
            string Field = GetStringFieldValue(row, "InputField");
            int convCount = 0;
            string Value = ConvertString(Field, out convCount, out unknownCount);
            if(convCount>0){
                ConvertedCount += convCount;
                SetStringFieldValue(row, "OutputField", Value);
            }
            if (unknownCount>0)
            {
                SetStringFieldValue(row, "OutputField", "unknown non-latin");
            }
            return convCount > 0 || unknownCount > 0;
        }

        
        string ConvertString(string Text, out int ConvertedCount, out int unknownCount)
        {
            StringBuilder outText = new StringBuilder();
            ConvertedCount = 0;
            unknownCount = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                char inChar = Text[i];
                char outChar = inChar;

                //check if any non latin characters found
                if (inChar > 255)
                { //Non latin char
                    if (ConvertPairsDic.ContainsKey(Text[i]))
                    {
                        outChar = ConvertPairsDic[inChar];
                        if (ConvertedCount == 0)
                            //logsb.AppendLine(string.Format("Line{0}: {1}", il, Text));

                        //logsb.AppendLine(string.Format("   {0} (0x{1:X}) to {2} (0x{3:X})", inChar, Convert.ToInt32(inChar), outChar, Convert.ToInt32(outChar)));
                        ConvertedCount++;
                    }
                    else
                    {
                        unknownCount++;
//                        if (ConvertedCount == 0)
//                            logsb.AppendLine(string.Format("Line{0}: {1}", il, Text));
//                        logsb.AppendLine(string.Format("   {0} (0x{1:X} Unknown non-latin character)", inChar, Convert.ToInt32(inChar), il));
                        //leave unknown char as is
                    }
                }
                outText.Append(outChar);
            }

            return outText.ToString().Trim();
        }
    }
}
