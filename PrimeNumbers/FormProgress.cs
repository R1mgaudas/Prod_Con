using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeNumbers
{
    public partial class FormProgress : Form
    {
        WorkItem workItem;
        int threadNumber;
        MainForm mainForm;
        int minValue = int.MaxValue;
        int maxValue = int.MinValue;
        string name;
        public FormProgress(int threadNumberFormMainForm, MainForm mainFormFrom)
        {
            threadNumber = threadNumberFormMainForm;
            mainForm = mainFormFrom;
            this.Location = new Point(435, (threadNumber) * 90);
            mainForm.updateThreadNumber();
            threadNumber++;
            InitializeComponent();
        }

        private void DoWork()
        {
            while (true)
            {
                if (mainForm.threadCountValue < threadNumber)
                {
                    mainForm.closeThread(threadNumber);
                }
                else
                {
                    workItem = mainForm.WorkItemValue;
                    if (workItem == null)
                    {
                        mainForm.closeThread(threadNumber);
                    }
                    Consume();
                }
            }

        }
        public void updateValuesBeforeLoop ()
        {
            progressBar.Maximum = workItem.numbers.Length;
            progressBar.Value = 0;
            name= workItem.fileName.Replace(@"Users\Rimgaudas\Desktop\Saugus_programavimas\3_uzd\files", "..");
        }
        public void updateValuesInLoop()
        {
            progressBar.Value++;
            this.Text = $"{name} ({progressBar.Value})";

        }
        public void updateValuesAfterLoop()
        {
            mainForm.updateFilesDone();
            mainForm.updateLastFile(name);
            mainForm.updateMinAndMax(minValue,maxValue);
        }
        public void Consume()
        {
            updateValuesBeforeLoop();
            for (int i = 0; i < workItem.numbers.Length; i++)
            {
                if (IsPrime(workItem.numbers[i]))
                {
                    maxValue = maxValue < workItem.numbers[i] ? workItem.numbers[i] : maxValue;
                    minValue = minValue > workItem.numbers[i] ? workItem.numbers[i] : minValue;
                }
                updateValuesInLoop();
            }
            updateValuesAfterLoop();

        }
        private static bool IsPrime(int n)
        {
            if (n > 1)
            {
                return Enumerable.Range(1, n).Where(x => n % x == 0)
                    .SequenceEqual(new[] { 1, n });
            }

            return false;
        }
        private void FormProgress_Shown(object sender, EventArgs e)
        {
            DoWork();
        }
    }
}
