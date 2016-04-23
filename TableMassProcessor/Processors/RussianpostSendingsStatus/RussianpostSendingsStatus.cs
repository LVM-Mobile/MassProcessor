using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.IO;
using System.Collections;
using RecordProcessor;
using RussianpostSendingsStatus.RussianpostVOH12;


namespace TableProcessorNS
{
    public class RussianpostSendingsStatus: BaseRecordProcessor
    {
       
        private static Dictionary<Field, bool> _knownFields = new Dictionary<Field, bool>();
        //Known fields for This class
        public override Dictionary<Field, bool> GetKnownFields()
        {
            return _knownFields;
        }

        public RussianpostSendingsStatus()
        {
            if(KnownFields.Count==0)
            {
                KnownFields[new Field("Barcode", true, new string [] {"Штрихкод"})] = true;
                KnownFields[new Field("Status", true, new string[] { "Статус" })] = false;
                KnownFields[new Field("Date", false, new string[] { "Дата/Время" })] = false;
            }

        }


        public override bool ProcessRow(DataRow row)
        {
            string barcode = GetStringFieldValue(row, "Barcode");
            using (var ws = new OperationHistory12())
            {
                OperationHistoryRequest request = new OperationHistoryRequest();
                request.Barcode = barcode;
                request.Language = "RUS";
                request.MessageType = 0;
                
                AuthorizationHeader ah = new AuthorizationHeader();
                ah.login = "rpost";
                ah.password = "eJbEvDGwNB";
                ah.mustUnderstand = true;

                var Values = ws.getOperationHistory(request, ah);
               

                OperationHistoryRecord latestOperation = null;
                bool IsReturned = false;
                foreach (var Value in Values)
                {
                    if (Value.OperationParameters.OperType.Name == "Возврат") 
                        IsReturned = true;
                    if (latestOperation == null || latestOperation.OperationParameters.OperDate < Value.OperationParameters.OperDate)
                    { latestOperation = Value; }

                }

                if (latestOperation != null)
                {
                    string lastStatus = string.Empty;
                    if (IsReturned)
                        lastStatus = "Возврат!!!";

                    lastStatus += latestOperation.OperationParameters.OperType.Name + ":" + latestOperation.OperationParameters.OperAttr.Name;
                    DateTime latestDate = latestOperation.OperationParameters.OperDate;
                    SetStringFieldValue(row, "Status", lastStatus);
                    if (Fieldsmap.ContainsKey("Date"))
                    {
                        SetStringFieldValue(row, "Date", latestDate.ToString("yyyy.MM.dd H:mm:ss"));
                    }
                }

            }
            return true;
        }
    }
}

