using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using TableProcessorNS;
using RecordProcessor;
using Progress;
using DatabaseAdapter;
using System.Threading;
using System.Reflection;


namespace TableProcessorWebApp
{
    public partial class WebFormInputFile : System.Web.UI.Page
    {
        public WebFormInputFile()
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
              Wizard1.ActiveStepIndex = 0;
              ResetProgress();
            }
             
        }
        void ResetProgress()
        {
            var progress = (BaseProgress)Session["progress"];

            if (progress != null)
                progress.ReportProgress(0, "");
        }

        void UplodFile()
        {
            if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
            {
                string fn = System.IO.Path.GetFileName(File1.PostedFile.FileName);
                string sessionDir = Path.Combine(Server.MapPath("Data"), Session.SessionID);
                Directory.CreateDirectory(sessionDir);
                string SaveLocation = Path.Combine(sessionDir, fn);
                try
                {
                    File1.PostedFile.SaveAs(SaveLocation);
                    Response.Write("The file has been uploaded.");
                    Session["InputFileName"] = SaveLocation;

                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                    //Note: Exception.Message returns detailed message that describes the current exception. 
                    //For security reasons, we do not recommend you return Exception.Message to end users in 
                    //production environments. It would be better just to put a generic error message. 
                }
            }
            else
            {
                Response.Write("Please select a file to upload.");
            }
        }
        void FillTables()
        {
            //OleDbDatabaseAdapter inputDatabase = new OleDbDatabaseAdapter();
            string infilename = Session["InputFileName"].ToString();
            IDatabaseAdapter inputDatabase = DatabaseAdapterFactory.CreateReader(Path.GetExtension(infilename), CheckBoxEditMode.Checked);
            inputDatabase.FileName = infilename;
         
            string[] tables = inputDatabase.GetTables();

            CheckBoxListTables.Items.Clear();
            foreach (string tablename in tables)
            {
                CheckBoxListTables.Items.Add(tablename);
            }

            if (CheckBoxListTables.Items.Count == 1)
            {
                CheckBoxListTables.Items[0].Selected = true;
            }
        }

        void FillModules()
        {
           CheckBoxListModules.Items.Clear();
           string modulesdir = Server.MapPath("Modules");

            List<string> modulenames = new List<string>();
            foreach(var module in Properties.Settings.Default.Modules)
                modulenames.Add(module);

            var processors = TableProcessorNS.TableProcessor.GetAvailableProcessors(modulesdir, modulenames);
               
            foreach (var processor in processors)
            {
               AssemblyTitleAttribute title_assembly = processor.Value.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute;
               CheckBoxListModules.Items.Add(new ListItem(title_assembly.Title, processor.Key));
            }
        }

        void FillMapFields()
        {
            string filename = Session["InputFileName"].ToString();
            //input fields
            IDatabaseAdapter inputDatabase = DatabaseAdapterFactory.CreateReader(Path.GetExtension(filename), CheckBoxEditMode.Checked);
            List<string> tables = (List<string>)Session["tables"];

            inputDatabase.Connect(filename);
            Dictionary<string, int> fields = inputDatabase.GetFields(tables[0]);

            //target fields
            TableProcessorNS.TableProcessor tp = new TableProcessorNS.TableProcessor();
            List<string> modules = (List<string>)Session["Modules"];
            var processorFileName = Server.MapPath(Path.Combine("Modules", modules[0]));
            
            tp.SetRecordProcessor(processorFileName);
           
            Dictionary<Field, bool> transformFields = tp.GetTransformFields();
            
            DataTable transformFieldsTable = new DataTable();
            transformFieldsTable = new DataTable();
            transformFieldsTable.Columns.Add("targetField");
            transformFieldsTable.Columns.Add("inputField");
            transformFieldsTable.Columns["targetField"].ReadOnly = true;

            foreach (KeyValuePair<Field, bool> row in transformFields)
            {
                transformFieldsTable.Rows.Add(row.Key.Name, "");
            }

            Repeater1.DataSource = transformFieldsTable;
            Repeater1.DataBind();
        }
        
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    {
                        // Find secondary DDL
                        DropDownList DropDownList2 = e.Item.FindControl("DropDownList2") as DropDownList;
                        if (DropDownList2 != null)
                        {
                            string filename = Session["InputFileName"].ToString();
                            //input fields
                            IDatabaseAdapter inputDatabase = DatabaseAdapterFactory.CreateReader(Path.GetExtension(filename), CheckBoxEditMode.Checked);            
                            List<string> tables = (List<string>)Session["tables"];

                            inputDatabase.Connect(Session["InputFileName"].ToString());
                            Dictionary<string, int> fields = inputDatabase.GetFields(tables[0]);
                            
                            DropDownList2.Items.Clear();
                            DropDownList2.Items.Add("");
                            foreach (KeyValuePair<string, int> row in fields)
                            {
                                var item = new ListItem(row.Key);
                                //Set field if it present in input document
                                DropDownList2.Items.Add(item);
                            }
                        }
                        break;
                    }
            }
        }


        void SetTables()
        {
            List<string> tables = new List<string>();
          foreach (ListItem item in CheckBoxListTables.Items)
           {
               if (item.Selected)
               {
                    tables.Add(item.Text);
               }
           }
           Session["tables"] = tables;
        }

        void SetMappings()
        {
            List<KeyValuePair<string, string>> map = new List<KeyValuePair<string, string>>();
            //Fields to map
            foreach (RepeaterItem row in Repeater1.Items)
            {
                Label label = (Label)row.FindControl("Label1");
                DropDownList list = (DropDownList)row.FindControl("DropDownList2");
                map.Add(new KeyValuePair<string,string>(list.SelectedValue, label.Text));
            }
            Session["Mappings"] = TableProcessor.SerializeFieldsMap(map);
        }

        protected void Wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            Session["progress"] = new BaseProgress();
            SetMappings();
            //Start Processing 
            TableProcessorNS.TableProcessor tp = new TableProcessorNS.TableProcessor();
            tp.Progress = (BaseProgress)Session["progress"];

            string inputFileName = Session["InputFileName"].ToString().Trim();
            string outputFileName = Path.Combine(Path.GetDirectoryName(inputFileName), TextBoxOutputFileName.Text);

            tp.InputDatabase = DatabaseAdapterFactory.CreateReader(Path.GetExtension(inputFileName), CheckBoxEditMode.Checked);
            tp.InputDatabase.FileName = inputFileName;
            tp.InputDatabase.Connect(inputFileName);
            List<string> tables = (List<string>)Session["tables"];
            tp.tableName = tables[0];
            List<string> modules = (List<string>)Session["Modules"];
            tp.SetRecordProcessor(Server.MapPath(Path.Combine("Modules",modules[0])));

            tp.InputFieldNamesMap = TableProcessor.DeserializeFieldsMap((string)Session["Mappings"]);

            

            //If Edit mode - copy original and open for wrtiting
            if (CheckBoxEditMode.Checked)
            {
                tp.OutputDatabase = DatabaseAdapterFactory.CreateWriter(Path.GetExtension(outputFileName));
                tp.ProcessMode = ProcessMode.pmEdit;
            }

            tp.OutputDatabase.FileName = outputFileName;

            Thread run = new Thread( new ThreadStart( tp.Process ));
            run.Start();
            Response.Write("Process started");
        }

        void SetProcessors()
        { 
          List<string> modules = new List<string>();
          foreach(ListItem item in CheckBoxListModules.Items)
           {
               if (item.Selected)
               {
                    modules.Add(item.Value);
               }
           }
           Session["Modules"] = modules;
        }

        protected void Wizard1_ActiveStepChanged(object sender, EventArgs e)
        {
            ResetProgress();
            if (Wizard1.ActiveStepIndex != 1)
            {
                if (Session["InputFileName"] == null)
                {
                    Response.Write("Upload file first");
                    Wizard1.ActiveStepIndex = 0;
                }
            }

            switch(Wizard1.ActiveStepIndex)
            {
                case 1:
                    //uplod file(s)
                    UplodFile();
                    //Show step2 tables form
                    FillTables();
                    break;
                case 2:
                    SetTables();
                    //Select Processing module(s) 
                    FillModules();
                    break;
                case 3:
                    SetProcessors();
                    //Select fields relations
                    FillMapFields();
                    break;
                case 4:
                    SetMappings();
                    FillOutputFileName();
                    
                    break;
                    
            }

        }

        void FillOutputFileName()
        {
            string inputFilename = Session["InputFileName"].ToString();

            SessionID.Value = Session.SessionID; 
            var filename = Path.GetFileNameWithoutExtension(inputFilename) + "_out" + 
                Path.GetExtension(inputFilename);
            TextBoxOutputFileName.Text = filename;
            
            /*var Url = 
                "Download.ashx?fn=" +filename  + 
                                 "&sessionid=" +SessionID.Value;
            DownloadUrl.NavigateUrl = Url;
             */
        }

        protected void CheckBoxEditMode_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void HiddenField1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
