using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DatabaseAdapter
{
    public class DatabaseAdapterFactory
    {
        static Dictionary<string, string> knownFileTypes = new Dictionary<string, string>()
        {
              {"Excel 2000/2003", "*.xls" },
              {"Excel 2007/2012", "*.xlsx" },
              {"DBF", "*.dbf" },
              {"XML", "*.xml" },
              {"Web services", "*.url" }

        };

        static Dictionary<string, string> knownOutputFileTypes = new Dictionary<string, string>()
        {
              {"Excel 2000/2003", "*.xls" },
              {"Excel 2007/2012", "*.xlsx" },
              {"DBF", "*.dbf" },
              {"XML", "*.xml" },
              {"Web services", "*.url" }

        };

        public static Dictionary<string, string> KnownOutputFileTypes
        {
            get { return knownOutputFileTypes; }
        }

        public static Dictionary<string, string> KnownFileTypes
        {
            get { return knownFileTypes; }
        }

        public static IDatabaseAdapter CreateReader(string extension, bool EditMode)
        {
            if (EditMode)
            {
                switch (extension.ToLower())
                {
                    case ".xls":
                        return new XLSDatabaseAdapter();
                    case ".xlsx":
                        return new XLSXDatabaseAdapter();
                    case ".csv":
                        return new OleDbDatabaseAdapter();
                    case ".dbf":
                        return new DBFDatabaseAdapter();
                    case ".xml":
                        return new XmlDatabaseAdapter();
                }
                throw new Exception("Database read adapter is not found for " + extension.ToLower());
            }
            return new OleDbDatabaseAdapter(); //was OdbcDatabaseAdapter
            
        }

        public static IDatabaseAdapter CreateWriter(string extension)
        {
                switch (extension.ToLower())
                {
                    case ".xls":
                        return new XLSDatabaseAdapter();
                    case ".xlsx":
                        return new XLSXDatabaseAdapter();
                    case ".csv":
                        return new OleDbDatabaseAdapter();
                    case ".dbf":
                        return new DBFDatabaseAdapter();
                   // case ".xml":
                   //      return new XmlDatabaseAdapter();
                   
                }
                throw new Exception("Database write adapter is not found for "+ extension.ToLower());
        }

    }
}
