using System;
using System.Collections.Generic;
using System.Text;
using DataTable = System.Data.DataTable;
using Microsoft.Office.Interop.Excel;
using System.Data;

namespace OfficeReporting
{
    public class ExcelInterop : IDisposable
    {
        //Excel interop
        private Application excelApp;
        private Workbook theWorkbook = null;
        private Worksheet templateWorkSheet = null;
        private Worksheet newWorkSheet = null;
        //new report PAGE offset
        private int NewPageOffset = 0;
        
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();
        private Dictionary<string, DataTable> _dataSources = new Dictionary<string, DataTable>();
        
        /// <summary>
        /// Report parameters
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get { return _parameters; }

            set
            {
                _parameters = value;
            }
        }
        /// <summary>
        /// Report data tables
        /// </summary>        
        public Dictionary<string, DataTable> DataSources
        {
            get { return _dataSources; }
            set { _dataSources = value; }
        }

        public ExcelInterop()
        {
             excelApp = new Application();
        }

        public void SetReport(string reportFilename)
        {
            theWorkbook = excelApp.Workbooks.Open(reportFilename, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", true, false, 0, true,true,false);
            //take template worksheet
            templateWorkSheet = (Worksheet)theWorkbook.Worksheets[1];
        }

        public void SaveXLS(string filename)
        {
            excelApp.ActiveWorkbook.SaveAs(filename, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing,
Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
Type.Missing, Type.Missing, Type.Missing, Type.Missing,Type.Missing);
        }



        /// <summary>
        /// Bind report fields from Datasource 
        /// </summary>
        private void BindData()
        {
            if (theWorkbook == null)
                throw new Exception("Report template is not set");

            ///Delta Row count for current page
            //Try to find 
            if (newWorkSheet == null)
            {
                //Create new worksheet
                newWorkSheet = (Worksheet)theWorkbook.Worksheets.Add(Type.Missing, Type.Missing, 1, XlSheetType.xlWorksheet);                
                newWorkSheet.Name = "output";
            }
            
            //Find template PAGE Range
            Range pagerange = templateWorkSheet.get_Range("PAGE", Type.Missing);  
            ///Empty Worksheet already have 1 row
            //NewPageOffset = newWorkSheet.UsedRange.Count;

            //if its not first line force page break
            if (NewPageOffset != 0)
            {
                newWorkSheet.HPageBreaks.Add(newWorkSheet.get_Range(string.Format("A{0}", NewPageOffset+1), Type.Missing));
            }
            else { 
             //TODO: set columns width first time
                    for (int icol = 1; icol <= pagerange.Count; icol++)
                    {
                        ((Range)newWorkSheet.Cells[1, icol]).EntireColumn.ColumnWidth = ((Range)pagerange.Cells[1, icol]).EntireColumn.ColumnWidth;
                    }
            }

            //Copy template PAGE contents
            Range newrange = newWorkSheet.get_Range(string.Format("A{0}", NewPageOffset+1), string.Format("A{0}", NewPageOffset+1));
            
            //Move selection to end 
            pagerange.Copy(newrange);

            int DeltaCount = FillReportFields();
            //Store new page offset
            NewPageOffset += pagerange.Rows.Count + DeltaCount;
        }

        /// <summary>
        /// Get parameters required for report 
        /// </summary>
        /// <returns></returns>
        int FillReportFields()
        {
            int CountDelta = 0;

            foreach (Name named in theWorkbook.Names)
            {
                if (!named.Visible)
                    continue;
                string name = named.Name;
                //ignore PAGE
                if (name == "PAGE")
                    continue;

                string[] name_parts = name.Split('_');                
                //fill parameter
                if (name_parts.Length == 1)
                {
                    FillParameter(name);
                }
                else
                {
                    //Check if name is table type
                    if (name.StartsWith("TABLE_") && name_parts.Length == 2)
                    {
                        //check if table_name (not table field name)
                        CountDelta += FillTable(name, DataSources[name]);
                    }
                    else
                    {
                        //skip table fields 
                        //TODO: Take table fields before fill table
                        continue;
                    }
                }
            }
            //Calc Next Page offset
            return CountDelta;
        }
        /// <summary>
        /// Set parameter value in output excel sheet from parameter in datasource
        /// </summary>
        /// <param name="name"></param>
        void FillParameter(string name)
        {
            if (!Parameters.ContainsKey(name))
            {
                throw new Exception("Parameters have value for key " + name);
            }

            Range newrange = TemplateRangeToOutputRange(name);
            
            newrange.Value2 = Parameters[name];
        }

        /// <summary>
        /// Fill table from Datasource 
        /// </summary>
        /// <param name="name">table name from template</param>
        /// <param name="datatable">selected data table with values</param>
        /// <returns>Delta offset equal to table row numbers - template rows number</returns>
        int FillTable(string name, DataTable datatable)
        {            
            //Find by range name DESTTABLE
            Range tableRange = templateWorkSheet.get_Range(name, Type.Missing);

            int StartTableRow = NewPageOffset + tableRange.Row;

            //Copy formatting

            Range newTableRange = newWorkSheet.get_Range(string.Format("A{0}", StartTableRow), string.Format("A{0}", StartTableRow)).EntireRow;
            //Copy formating row copied from template 
            int CountDelta = 0;
            
            for (int i = 1; i < datatable.Rows.Count; i++)
            {
                newTableRange.Insert(XlInsertShiftDirection.xlShiftDown, StartTableRow + i + 1);
                newTableRange.Copy(newTableRange.get_Offset(-1, 0));
                CountDelta++;
            }

            //Get template ranges for fields
            List<Range> fieldRanges = new List<Range>(datatable.Columns.Count);
            foreach (DataColumn dataColumn in datatable.Columns)
            {
                //translate range
                Range fieldRange = TemplateRangeToOutputRange(name + '_' +dataColumn.ColumnName);
                fieldRanges.Add(fieldRange);
            }

            //Fill Values            
            for (int irow = 0; irow < datatable.Rows.Count; irow++)
            {
                DataRow row = datatable.Rows[irow];
                //Copy field values
                for (int icol = 0; icol < fieldRanges.Count; icol++)
                {
                    Range fieldRange = fieldRanges[icol];
                    //Offset to N row of data rows Count
                    fieldRange.get_Offset(irow, 0).Value2 = row[icol];
                }
            }
            return CountDelta;
        }

        /// <summary>
        /// Get range in output worksheet
        /// </summary>
        /// <returns></returns>
        private Range TemplateRangeToOutputRange(string name)
        {
            Range parameterRange = templateWorkSheet.get_Range(name, Type.Missing);

            Range newrange = (Range)newWorkSheet.Cells[NewPageOffset + parameterRange.Row, parameterRange.Column];
            //Expand range
            return newrange.get_Resize(parameterRange.Rows.Count, parameterRange.Rows.Columns.Count);
        }

        public void PrepareReport()
        {
            BindData();
//            reportViewer1.RefreshReport();
        }

        public bool Visible
        {
            get { return excelApp.Visible; }
            set { excelApp.Visible = value; }
        }

        #region IDisposable Members

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            excelApp.Quit();
        }
        #endregion
    }
}
