using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GUI
{
    public partial class WaitingForm : Form
    {
        public WaitingForm(string formName)
        {
            InitializeComponent(formName);
        }
        
        public static void InvokeWithWaitingForm(string formName, Action action)
        {
            
            WaitingForm waiting = new WaitingForm(formName);
            Thread thr = new Thread((ThreadStart)delegate()
            {
                action();
                waiting.Invoke((MethodInvoker)delegate()
                {
                    if (!waiting.IsDisposed)
                    {
                        waiting.Dispose();
                    }
                });
            });
            thr.IsBackground = true;
            thr.Start();
            if (!waiting.IsDisposed)
            {
                waiting.ShowDialog();
            }
        }
    }
}
