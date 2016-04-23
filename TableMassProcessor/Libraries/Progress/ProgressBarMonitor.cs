using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace LVMSoft.Progress
{
    public class ProgressBarMonitor: IProgressMonitor
    {
        ToolStripProgressBar _progress;
        ToolStripStatusLabel _label;

        public ProgressBarMonitor(ToolStripProgressBar progress, int max)
           :this(progress, null, max)
        {
            
        }

        public ProgressBarMonitor(ToolStripProgressBar progress, ToolStripStatusLabel label,int max)
        {
            //
            _progress = progress;
            _progress.Maximum = max; 
            _progress.Value = 0;

            _label = label;
            _label.Text = string.Empty;
            Application.DoEvents();
        }

        public void ReportProgress(int percentProgress)
        {
            _progress.Value = percentProgress;
        }

        public void ReportProgress(int percentProgress, object userState)
        {
            _progress.Value = percentProgress;
            if (_label != null)
            {
                if(userState is string){
                    _label.Text = (string)userState;
                }
            }
            Application.DoEvents();
        }
    }
}
