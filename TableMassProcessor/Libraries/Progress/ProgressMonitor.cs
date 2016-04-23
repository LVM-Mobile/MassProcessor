using System;
using System.Collections.Generic;
using System.Text;

namespace LVMSoft.Progress
{
    public interface IProgressMonitor
    {
        void ReportProgress(int percentProgress);
        void ReportProgress(int percentProgress, object userState);
    }
}
