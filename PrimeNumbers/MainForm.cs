using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeNumbers
{
    public partial class MainForm : Form
    {
        public WorkItem WorkItemValue
        {
            get {
                try
                {
                    return blockingCollection.Take();
                }
                catch (Exception)
                {

                    return null;
                }}
        }
        public int threadCountValue
        {
            get { return threadCount; }
        }
        public void closeThread (int threadNumber)
        {
            consumerThreads[threadNumber-1].Abort();
        }
        public void updateThreadNumber()
        {
            threadCount++;
        }
        public void updateLastFile(string name)
        {
            labelLast.Invoke(new MethodInvoker(() => {
                labelLast.Text = name;
            }));
        }
        public void updateFilesDone()
        {
            filesDone++;
            labelProcessed.Invoke(new MethodInvoker(() => {
                labelProcessed.Text = filesDone.ToString();
            }));
        }
        public void updateMinAndMax(int min, int max)
        {
            minValue = minValue > min ? min : minValue;
            maxValue = maxValue < max ? max : maxValue;
            labelMin.Invoke(new MethodInvoker(() => {
                labelMin.Text = minValue.ToString();
            }));
            labelMax.Invoke(new MethodInvoker(() => {
                labelMax.Text = maxValue.ToString();
            }));
        }
        private BlockingCollection<WorkItem> blockingCollection = new BlockingCollection<WorkItem>();
        private static int threadCount=0;
        private static int minValue;
        private static int maxValue;
        Thread[] consumerThreads = new Thread[9];
        private int filesDone = 0;
        public MainForm()
        {
            InitializeComponent();
        }

        private void FindNumbers_Load(object sender, EventArgs e)
        {
            minValue = int.MaxValue;
            maxValue = 0;
            labelMin.Text = minValue.ToString();
            labelMax.Text = maxValue.ToString();
            labelThreadCount.Text = "0";
        }
        public void Produce() {

            string[] fileArray = Directory.GetFiles(@"C:\Users\Rimgaudas\Desktop\Saugus_programavimas\3_uzd\files\");
            WorkItem workItem;
            foreach (string item in fileArray)
            {
                workItem = new WorkItem(item, readFile(item));
                blockingCollection.Add(workItem);
            }

        }
        private int[] readFile(string filename)
        {
            string[] records = File.ReadAllLines(filename);
            return  Array.ConvertAll<string, int>(records, new Converter<string, int>(i => int.Parse(i)));
        }

            private static bool isPrime(int number)
        {
            return Enumerable.Range(2, (int)Math.Sqrt(number) - 1)
                    .All(divisor => number % divisor != 0);
        }

        private void buttonIncrease_Click(object sender, EventArgs e)
        {
            if (threadCount < 9)
            {
                labelThreadCount.Text = (threadCount+1).ToString();
                consumerThreads[threadCount]= new Thread(() => ShowFormProgress());
                consumerThreads[threadCount].Start();

            }
        }

         void ShowFormProgress()
        {
            FormProgress f2 = new FormProgress(threadCount, this);
            f2.ShowDialog();
        }
        private void buttonDecrease_Click(object sender, EventArgs e)
        {
            if (threadCount>0)
            {
                threadCount--;
            }
            labelThreadCount.Text = threadCount.ToString();
        }

        private void FindNumbers_Shown(object sender, EventArgs e)
        {
            Task.Run(Produce).ConfigureAwait(false);

        }

      
        protected void setLabelValue(Label label, string value)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new MethodInvoker(() => {
                    label.Text = value;
                }));
            }
            else
            {
                label.Text = value;
            }
        }
        
       
        public void SetLabel(Label label, String text)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate () { SetLabel(label,text); });
                return;
            }
            label.Text = text;
        }




    }

}
