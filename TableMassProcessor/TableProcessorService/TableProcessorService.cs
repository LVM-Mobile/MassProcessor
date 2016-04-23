using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace TableProcessorService
{
    public partial class TableProcessorService : ServiceBase
    {
        public TableProcessorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
           //Run task
            var tp = new TableProcessorNS.TableProcessor();
           
        }

        protected override void OnStop()
        {
        }
    }
}
