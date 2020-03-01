using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Lab1_MergeSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;


            //Test_File_Array_List(seed);
            Benchmark(seed);
        }
        public static void Test_File_Array_List(int seed)
        {
            int n = 10000;
            string filename;
            filename = @"mydataarray.dat";
            MyFileArray myfilearray = new MyFileArray(filename, n, seed);
                Console.WriteLine("\n FILE ARRAY \n");
              // myfilearray.Print(n);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
                mergeSort3Way(myfilearray);
            stopwatch.Stop();
            Console.WriteLine("sorted:");
            //myfilearray.Print(n);
            Console.WriteLine("Number of data {0}", n);
            Console.WriteLine("Time: {0}", stopwatch.Elapsed);
            Console.WriteLine("Ticks: {0}", stopwatch.ElapsedTicks);
          
            

        }
        public static void Benchmark(int seed)
        {
            int n = 4000;
            string filename;
            filename = @"mydataarray.dat";
            Console.WriteLine("{0,-20} {1,20} {2,20}", "Number of data", "Time", "Ticks");
            for (int i=0; i<7;i++)
            {
                MyFileArray myfilearray = new MyFileArray(filename, n, seed);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                mergeSort3Way(myfilearray);
                stopwatch.Stop();
                myfilearray.fs.Close();
                Console.WriteLine("{0,-20} {1,20} {2,20}", n, stopwatch.Elapsed, stopwatch.ElapsedTicks);
                n *= 2;
               
                
            }
        }
        // Function  for 3-way merge sort process 
        public static void mergeSort3Way(MyFileArray gArray)
        {
            // if array of size is zero returns null 
            if (gArray == null)
                return;

            // creating duplicate of given array
            // copying elements of given array into 
            // duplicate array 
            MyFileArray fArray = new MyFileArray(@"mydataarraycopy.dat", gArray);

            // sort function 
             mergeSort3WayRec(fArray, 0, gArray.Length, gArray);
            // copy back elements of duplicate array 
            // to given array
            for (int i = 0; i < fArray.Length; i++)
            {
                gArray.Replace(i, fArray[i]);
            }
            fArray.fs.Close();
            File.Delete(@"mydataaarraycopy.dat");
        }
        public static void mergeSort3WayRec(MyFileArray gArray,
                  int low, int high, MyFileArray destArray)
        {
            // If array size is 1 then do nothing 
            if (high - low < 2)
                return;

            // Splitting array into 3 parts 
            int mid1 = low + ((high - low) / 3);
            int mid2 = low + 2 * ((high - low) / 3) + 1;

            // Sorting 3 arrays recursively 
            mergeSort3WayRec(destArray, low, mid1, gArray);
            mergeSort3WayRec(destArray, mid1, mid2, gArray);
            mergeSort3WayRec(destArray, mid2, high, gArray);

            // Merging the sorted arrays 
            merge(destArray, low, mid1, mid2, high, gArray);
        }

        /* Merge the sorted ranges [low, mid1), [mid1, 
           mid2) and [mid2, high) mid1 is first midpoint 
           index in overall range to merge mid2 is second 
           midpoint index in overall range to merge*/
        public static void merge(MyFileArray gArray, int low,
                               int mid1, int mid2, int high,
                                       MyFileArray destArray)
        {
            int i = low, j = mid1, k = mid2, l = low;

            // choose smaller of the smallest in the three ranges 
            while ((i < mid1) && (j < mid2) && (k < high))
            {
                if (gArray[i].CompareTo(gArray[j]) < 0)
                {
                    if (gArray[i].CompareTo(gArray[k]) < 0)
                        destArray.Replace(l++, gArray[i++]);

                    else
                        destArray.Replace(l++, gArray[k++]);
                }
                else
                {
                    if (gArray[j].CompareTo(gArray[k]) < 0)
                        destArray.Replace(l++, gArray[j++]);
                    else
                        destArray.Replace(l++, gArray[k++]);
                }
            }

            // case where first and second ranges have 
            // remaining values 
            while ((i < mid1) && (j < mid2))
            {
                if (gArray[i].CompareTo(gArray[j]) < 0)
                    destArray.Replace(l++, gArray[i++]);
                else
                    destArray.Replace(l++, gArray[j++]);
            }

            // case where second and third ranges have 
            // remaining values 
            while ((j < mid2) && (k < high))
            {
                if (gArray[j].CompareTo(gArray[k]) < 0)
                    destArray.Replace(l++, gArray[j++]);

                else
                    destArray.Replace(l++, gArray[k++]);
            }

            // case where first and third ranges have 
            // remaining values 
            while ((i < mid1) && (k < high))
            {
                if (gArray[i].CompareTo(gArray[k]) < 0)
                    destArray.Replace(l++, gArray[i++]);
                else
                    destArray.Replace(l++, gArray[k++]);
            }

            // copy remaining values from the first range 
            while (i < mid1)
                destArray.Replace(l++, gArray[i++]);

            // copy remaining values from the second range 
            while (j < mid2)
                destArray.Replace(l++, gArray[j++]);

            // copy remaining values from the third range 
            while (k < high)
                destArray.Replace(l++, gArray[k++]);
        }

    }
}
