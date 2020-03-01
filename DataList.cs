using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_MergeSort
{
    abstract class DataList
    {
        protected int length;
        public int Length { get { return length; } }
        public abstract (double, int) Head();
        public abstract (double, int) Next();
        public abstract void Swap(double a, double b);
        public void Print(int n)
        {
           
        Console.Write(" {0:F5} ", Head());
            for
           (int i = 1; i < n; i++)
                Console.Write(" {0:F5} ", Next());
            Console.WriteLine();

        }

    }
}
