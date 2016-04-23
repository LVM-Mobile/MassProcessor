using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PostModel;
using RecordProcessor;
using SmartGroup.Properties;

namespace TableProcessorNS
{
    public class SmartGrouping : BaseRecordProcessor
    {
        
        private static Dictionary<Field, bool> _knownFields = new Dictionary<Field, bool>();
        //Known fields for This class
        public override Dictionary<Field, bool> GetKnownFields()
        {
            return _knownFields; 
        }        
        
        /// <summary>
        /// Destinations by zipcode group, by weight splited to named groups
        /// </summary>
        Dictionary<string, PostalList> _fillingLists = new Dictionary<string, PostalList>();
        
        /// <summary>
        /// Already closed sending lists
        /// </summary>
        List<PostalList> _filledLists = new List<PostalList>();
        
        private double _massLimit = 0;

        public double MassLimit
        {
            get { return _massLimit; }
            set { _massLimit = value; }
        }

        public SmartGrouping()
        {
            //Set from stored settings
            MassLimit = Settings.Default.MassLimit;

            //set report template
            OutputTemplate = "SmartGroup.xls";

            if (KnownFields.Count == 0)
            {
                KnownFields[new Field("DepartamentField", true, new string[] { "цех"})] = true;
                KnownFields[new Field("MassField", true, new string[] { "вес", "масса" })] = true;
                KnownFields[new Field("ProductField", true, new string[] { "продукт" })] = true;
                KnownFields[new Field("AddressField", true, new string[] { "адрес" })] = true;
                KnownFields[new Field("ZipCodeField", true, new string[] { "Индекс" })] = true;
                KnownFields[new Field("RecipientField", true, new string[] { "Получатель" })] = true;                
            }
        }

        public override void Reset()
        {
            _fillingLists.Clear();
            _filledLists.Clear();
        }

        //Record procedures
        public override bool ProcessRecord(IDataRecord record)
        {
            string departament = GetStringFieldValue(record, "DepartamentField").Trim();
            string product = GetStringFieldValue(record, "ProductField").Trim();
            string address = GetStringFieldValue(record, "AddressField").Trim();
            string zipcode = GetStringFieldValue(record, "ZipCodeField").Trim();
            string recipient = GetStringFieldValue(record, "RecipientField").Trim();
            double mass = GetDoubleFieldValue(record, "MassField");
             
            Package package = new Package(new Product(product, mass));
            Destination dest = new Destination(new Address(zipcode, address), recipient, package);
            //If group is absent - add new
            if (!_fillingLists.ContainsKey(zipcode))
            {
                PostalList list = new PostalList(zipcode, departament, package);
                list.Destinations.Add(dest);
                _fillingLists[zipcode] =list;
            }
            else
            {
                PostalList list =_fillingLists[zipcode];
                //We may add dest to list
                if (MassLimit == 0 || list.Mass < MassLimit)
                {
                    list.Destinations.Add(dest);
                }
                else {
                    //Move sending list to closed
                    _filledLists.Add(list);

                    //Add dest to empty list
                    PostalList newlist = new PostalList(zipcode, departament, package);
                    newlist.Destinations.Add(dest);
                    _fillingLists[zipcode] = newlist;
                }
            }
            return true;
        }

        private static int CompareByZipCode(PostalList x, PostalList y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    int retval = x.Zipcode.Length.CompareTo(y.Zipcode.Length);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        //
                        return x.Zipcode.CompareTo(y.Zipcode);
                    }
                }
            }
        }

        //List 
        //-zipcode
        //-departament
        //--TableDest
        //---zipcode
        //---address
        //---recipient
        public override DataTable OutputData()
        {
            //close all sending lists 
            _filledLists.AddRange(_fillingLists.Values);
            _fillingLists.Clear();
            //Sort Lists by zipcode
            _filledLists.Sort(CompareByZipCode);

            DataTable outputTable = new DataTable();
            outputTable.Columns.Add("ZIPCODE");
            outputTable.Columns.Add("DEPARTAMENT");
            outputTable.Columns.Add("DESTCOUNT");
            outputTable.Columns.Add("PRODUCT");
            outputTable.Columns.Add(new DataColumn("TABLE_DESTINATIONS", typeof(DataTable)));
            
            //set output table definitions
            //Prepare output table for list
            foreach (PostalList list in _filledLists)
            {
                DataTable destsTable = new DataTable();
                destsTable.Columns.Add("ZIPCODE");
                destsTable.Columns.Add("ADDRESS");
                destsTable.Columns.Add("RECIPIENT");

                foreach (Destination destination in list.Destinations)
                {
                    destsTable.Rows.Add(new object[] { destination.ZipCode, destination.AddressString, destination.Recipient });
                }
                outputTable.Rows.Add(new object[] {list.Zipcode, list.Departament, list.Destinations.Count, list.Package.ProductsString, destsTable});
            }
            return outputTable;
        }
    }
}
