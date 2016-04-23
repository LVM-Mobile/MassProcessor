using System;
using System.Collections.Generic;
using System.Text;
using PostModel;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace TableProcessorNS.ReportOutput
{
    public class ReportOutput
    {
        ReportViewer reportViewer1;
        IList<Destination> _destinations;

        public IList<Destination> Destinations
        {
            get { return _destinations; }
            set { _destinations = value; }
        }
        
        public ReportOutput()
        {
            reportViewer1 = new ReportViewer();
        }

        public void SetReport(string reportFilename)
        {
            reportViewer1.Reset();
            reportViewer1.ProcessingMode = ProcessingMode.Local;
            reportViewer1.LocalReport.EnableExternalImages = true; //enable background

            reportViewer1.LocalReport.ExecuteReportInCurrentAppDomain( 
            System.Reflection.Assembly.GetExecutingAssembly().Evidence);

            reportViewer1.LocalReport.EnableHyperlinks = true;

            reportViewer1.LocalReport.ReportPath = reportFilename;

            using (Stream strm = new FileStream(reportFilename, FileMode.Open, FileAccess.Read))
                {
                    reportViewer1.LocalReport.LoadReportDefinition(strm);
                    strm.Close();
                }
                //reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
        }

        public void SaveXLS(string filename)
        {
            string deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";
            string mimeType;
            string encoding;
            string extension;
            string[] streams;
            Warning[] warnings;                        
            byte[] bytes;      
    
            bytes = this.reportViewer1.LocalReport.Render("Excel", deviceInfo, out mimeType, out encoding, out extension, out streams, out warnings);
            //Save file
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
        }

        public Dictionary<string,string> Parameters
        {
            set{
                List<ReportParameter> parameters =new List<ReportParameter>(value.Count);
                foreach (KeyValuePair<string, string> strParameter in value)
                {
                    parameters.Add(new ReportParameter(strParameter.Key, strParameter.Value));
                }
                this.reportViewer1.LocalReport.SetParameters(parameters);
            }
        }

        private void BindData()
        {
            //Set all required data sources
            IList<string> requiredDataSources = reportViewer1.LocalReport.GetDataSourceNames();

            reportViewer1.LocalReport.DataSources.Clear();
            foreach (string dsName in requiredDataSources)
            {
                ReportDataSource reportDataSource = new ReportDataSource();
                switch (dsName)
                {
                    //Get categories dynamic
                    case "PostModel_Destination":
                        reportDataSource.Name = dsName;
                        reportDataSource.Value = Destinations;
                        break;
                }
                if (reportDataSource.Name.Length > 0)
                {
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                }

            }
        }
        
        public void PrepareReport()
        {
            BindData();
            reportViewer1.RefreshReport();
        }

    }
}
