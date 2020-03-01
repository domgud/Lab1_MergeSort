using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab1_MergeSort
{
    class MyFileList : DataList
    {
        int prevNode;
        public int currentNode { get; set; }
        public int nextNode { get; set; }
        public string filename { get; set; }
        public MyFileList(string filename, int n, int seed)
        {
            length = n;
            this.filename = filename;
            Random rand = new Random(seed);
            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename,
               FileMode.Create)))
                {
                    writer.Write(4);
                    for (int j = 0; j < length; j++)
                    {
                        writer.Write(rand.NextDouble());
                        writer.Write(rand.Next(1, 5));
                        writer.Write((j + 1) * 16 + 4);
                    }
                }
                this.fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public FileStream fs { get; set; }
        public override (double,int) Head()
        {
            Byte[] data = new Byte[16];
            fs.Seek(0, SeekOrigin.Begin);
            fs.Read(data, 0, 4);
            currentNode = BitConverter.ToInt32(data, 0);
            prevNode = -1;
            fs.Seek(currentNode, SeekOrigin.Begin);
            fs.Read(data, 0, 16);
            double result = BitConverter.ToDouble(data, 0);
            int iresult = BitConverter.ToInt32(data, 8);
            nextNode = BitConverter.ToInt32(data, 12);
            return (result, iresult);
        }
        public override (double,int) Next()
        {
            Byte[] data = new Byte[16];
            fs.Seek(nextNode, SeekOrigin.Begin);
       
            fs.Read(data, 0, 16);
            prevNode = currentNode;
            currentNode = nextNode;
            double result = BitConverter.ToDouble(data, 0);
            int iresult = BitConverter.ToInt32(data, 8);
            nextNode = BitConverter.ToInt32(data, 12);
            return (result, iresult);

        }
        public override void Swap(double a, double b)

        {
            Byte[] data;
            fs.Seek(prevNode, SeekOrigin.Begin);
            data = BitConverter.GetBytes(a);
            fs.Write(data, 0, 8);
            fs.Seek(currentNode, SeekOrigin.Begin);
            data = BitConverter.GetBytes(b);
            fs.Write(data, 0, 8);

        }
        public MyFileList(string filename, MyFileList original)
        {
            length = original.length;
            this.filename = filename;
            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename,
               FileMode.Create)))
                {
                    writer.Write(4);
                    writer.Write(original.Head().Item1);
                    writer.Write(original.Head().Item2);
                    writer.Write(20);
                    for (int j = 1; j < length; j++)
                    {
                        original.Next();
                        writer.Write(original.CurrentData(original.currentNode).Item1);
                        writer.Write(original.CurrentData(original.currentNode).Item2);
                        writer.Write(original.nextNode);
                    }
                }
                this.fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public (double, int) CurrentData(int node)
        {
            Byte[] data = new Byte[12];
            fs.Seek(node, SeekOrigin.Begin);
            fs.Read(data, 0, 12); 
            double result = BitConverter.ToDouble(data, 0);
            int iresult = BitConverter.ToInt32(data, 8);
            return (result, iresult);
        }
        // Function  for 3-way merge sort process 
        public  void mergeSort3Way()
        {
            // if array of size is zero returns null 
            if (this == null)
                return;
            GetReady();
            // creating duplicate of given array
            // copying elements of given array into 
            // duplicate array 
            MyFileList fArray = new MyFileList(@"mydatalistcopy.dat", this);
            //// sort function 
            mergeSort3WayRec(fArray, 0, this.length, this);
            //// copy back elements of duplicate array 
            //// to given array
            for (int i = 0; i < fArray.Length; i++)
            {
                this.Replace(i, fArray, i);
            }
            fArray.fs.Close();
            File.Delete(@"mydatalistcopy.dat");
        }
        public static void mergeSort3WayRec(MyFileList gArray,
                  int low, int high, MyFileList destArray)
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
        public static void merge(MyFileList gArray, int low,
                               int mid1, int mid2, int high,
                                       MyFileList destArray)
        {
            int i = low, j = mid1, k = mid2, l = low;

            // choose smaller of the smallest in the three ranges 
            while ((i < mid1) && (j < mid2) && (k < high))
            {
                if (gArray.Nodedata(i).CompareTo(gArray.Nodedata(j)) < 0)
                {
                    if (gArray.Nodedata(i).CompareTo(gArray.Nodedata(k)) < 0)
                        destArray.Replace(l++, gArray, i++);

                    else
                        destArray.Replace(l++, gArray, k++);
                }
                else
                {
                    if (gArray.Nodedata(j).CompareTo(gArray.Nodedata(k)) < 0)
                        destArray.Replace(l++, gArray, j++);
                    else
                        destArray.Replace(l++, gArray, k++);
                }
            }

            // case where first and second ranges have 
            // remaining values 
            while ((i < mid1) && (j < mid2))
            {
                if (gArray.Nodedata(i).CompareTo(gArray.Nodedata(j)) < 0)
                    destArray.Replace(l++, gArray, i++);
                else
                    destArray.Replace(l++, gArray, j++);
            }

            // case where second and third ranges have 
            // remaining values 
            while ((j < mid2) && (k < high))
            {
                if (gArray.Nodedata(j).CompareTo(gArray.Nodedata(k)) < 0)
                    destArray.Replace(l++, gArray, j++);

                else
                    destArray.Replace(l++, gArray, k++);
            }

            // case where first and third ranges have 
            // remaining values 
            while ((i < mid1) && (k < high))
            {
                if (gArray.Nodedata(i).CompareTo(gArray.Nodedata(k)) < 0)
                    destArray.Replace(l++, gArray, i++);
                else
                    destArray.Replace(l++, gArray, k++);
            }

            // copy remaining values from the first range 
            while (i < mid1)
                destArray.Replace(l++, gArray, i++);

            // copy remaining values from the second range 
            while (j < mid2)
                destArray.Replace(l++, gArray, j++);

            // copy remaining values from the third range 
            while (k < high)
                destArray.Replace(l++, gArray, k++);
        }
        public void Replace(int index, MyFileList listas, int index2)
        {
            Byte[] data = new Byte[12];

            listas.fs.Seek(index2*16+4, SeekOrigin.Begin);
            listas.fs.Read(data, 0, 12);

            fs.Seek(index*16+4, SeekOrigin.Begin);
            fs.Write(data, 0, 12);
        }
        public (double, int) Nodedata(int i)
        {
            return CurrentData(i*16+4);
        }
        public void GetReady()
        {
            for (int i=0; i< length; i++)
            {
                fs.Seek(i * 16 + 16, SeekOrigin.Begin);
                Byte[] pointer = new Byte[4];
                BitConverter.GetBytes(i * 16 + 20).CopyTo(pointer, 0);

                fs.Write(pointer, 0, 4);
            }
        }

    }
}
