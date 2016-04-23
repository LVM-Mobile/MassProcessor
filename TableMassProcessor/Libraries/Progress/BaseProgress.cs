using System;
using System.Collections.Generic;
using System.Text;
using LVMSoft.Progress;

namespace Progress
{
    public class BaseProgress : IProgressMonitor
    {
        public int Value { get; set; }
        public string Label { get; set; }

        public BaseProgress()
        {
            Value = 0;
            Label = string.Empty;
        }

        public void ReportProgress(int percentProgress)
        {
                Value = percentProgress;
        }

        public void ReportProgress(int percentProgress, object userState)
        {
            ReportProgress(percentProgress);
            if(userState is string)
             {
                lock (Label)
                {
                    Label = userState.ToString();
                }
             }
        }
    }
}
