using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAdapter;

namespace DatabaseAdapter
{
    public class EnricoCalendarAdapter: IDatabaseAdapter
    {
     
    public string  FileName
    {
	  get {
                return "http://kayaposoft.com/enrico/ws/v1.0/index.php?wsdl";
	    }

        set { throw new NotImplementedException();  }
	}

    public Dictionary<string,string>  KnownFileTypes
{
	get { throw new NotImplementedException(); }
}

public void  Connect()
{
 	
}

public void  Connect(string filename)
{
 	throw new NotImplementedException();
}

public string[]  GetTables()
{
 	throw new NotImplementedException();
}

public Dictionary<string,int>  GetFields(string tablename)
{
 	throw new NotImplementedException();
}

public System.Data.Common.DbDataReader  Execute(string sql)
{
 	throw new NotImplementedException();
}

public void  SetTable(string tablename)
{
 	throw new NotImplementedException();
}

public string  NormTableName(string tablename)
{
 	throw new NotImplementedException();
}

public void  Write(System.Data.DataTable table)
{
 	throw new NotImplementedException();
}

public void  Close()
{
 	throw new NotImplementedException();
}

public void  FirstRecord()
{
 	throw new NotImplementedException();
}

public void  SeekRecord(int position)
{
 	throw new NotImplementedException();
}

public System.Data.IDataRecord  CurrentRecord()
{
 	throw new NotImplementedException();
}

public System.Data.DataRow  CurrentRow()
{
 	throw new NotImplementedException();
}

public void  UpdateRow(int irow, System.Data.DataRow row)
{
 	throw new NotImplementedException();
}

public bool  NextRecord()
{
 	throw new NotImplementedException();
}

public void  Write(string filename)
{
 	throw new NotImplementedException();
}

public int  GetRowsCount()
{
 	throw new NotImplementedException();
}

        public void Clone(IDatabaseAdapter outDatabase)
        {
            throw new NotImplementedException();
        }
    }
}
