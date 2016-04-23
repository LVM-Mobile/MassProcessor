using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecordProcessor;
using System.Data;

namespace TableProcessorNS
{
    public class ImportCalendars: BaseRecordProcessor
    {

        ERPEntities CalendarsModel;
        private static Dictionary<Field, bool> _knownFields = new Dictionary<Field, bool>();
        //Known fields for This class
        public override Dictionary<Field, bool> GetKnownFields()
        {
            return _knownFields;
        }

       //TODO: Move to adapter
       com.kayaposoft.Calendars.enrico cal;
       DataTable table = new DataTable();

       public ImportCalendars()
        { 
           //TODO: Move to adapter
           //Calendar service 
           com.kayaposoft.Calendars.enrico cal = new com.kayaposoft.Calendars.enrico();

           CalendarsModel = new ERPEntities();
 
           if(KnownFields.Count==0)
            {
                KnownFields[new Field("Country", true, new string[] { "Страна" })] = true;
                KnownFields[new Field("date", true, new string [] {"Дата"})] = true;
                KnownFields[new Field("localName", true, new string[] { "Локальное название" })] = false;
                KnownFields[new Field("englishName", true, new string[] { "Английское название" })] = false;
            }
        }
        
        public virtual void ProcessData(DataTable data)
         {
            var holidays = cal.getPublicHolidaysForYear(DateTime.Today.Year.ToString(), "RU", "");
            
            foreach(var holiday in holidays.publicHolidays)
            {
                Calendars newCal = new Calendars();
                newCal.StartTime = new DateTime(holiday.date.year, holiday.date.month, holiday.date.day);
                newCal.EndTime = newCal.StartTime.AddDays(1).AddMilliseconds(1);
                newCal.LocalName = holiday.localName;
                newCal.EnglishName = holiday.englishName;
                newCal.IsWorkDay = 2; //public holiday
            }
            CalendarsModel.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
         }

        public override bool ProcessRow(DataRow row)
        {
            return true;
        }
    }
}
