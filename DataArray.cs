using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_MergeSort
{
   abstract class DataArray
    {
        protected int length;
        public int Length { get { return length; } }
        public abstract (double, int) this[int index] { get; }
        //public abstract void Swap(int index1, int index2);
        public abstract void Print(int n);

    }
}
