using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumbers
{
    public class WorkItem
    {
        public string fileName;
        public int[] numbers;

        public WorkItem(string fileName, int [] numbers) {
            this.fileName = fileName;
            this.numbers = numbers;
        }
    }
}
