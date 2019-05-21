using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Implementation
{

    public partial class TaskWorker
    {
        private BackgroundWorker backgroundWorker = null;

        private void doByWorker(DoWorkEventHandler work, object argument, RunWorkerCompletedEventHandler reaction)
        {
            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.DoWork += work;
            this.backgroundWorker.RunWorkerCompleted += reaction;
            this.backgroundWorker.RunWorkerAsync(argument);
        }

    }
}
