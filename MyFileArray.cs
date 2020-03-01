using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_MergeSort
{
    class MyFileArray : DataArray
    {
        private string path;
        /// <summary>
        /// Generate a new file with data
        /// </summary>
        /// <param name="filename">file name to generate</param>
        /// <param name="n">number of elements</param>
        /// <param name="seed">seed used to generate the double</param>
        public MyFileArray(string filename, int n, int seed)
        {
            path = filename;
            (double,int)[] data = new (double,int)[n];
            length = n;
            Random rand = new Random(seed);
            for (int i = 0; i < length; i++)
            {
                double d = rand.NextDouble();
                int intas = rand.Next(1, 5);
                data[i] = (d, intas);
            }
            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename,
               FileMode.Create)))
                {
                    for (int j = 0; j < length; j++)
                    {
                        writer.Write(data[j].Item1);
                        writer.Write(data[j].Item2);
                    }
                        
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            this.fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
        }
        /// <summary>
        /// creates a copy of a specified filearray to a file
        /// </summary>
        /// <param name="filename">generates a new file here</param>
        /// <param name="original">the original filearray</param>
        public MyFileArray(string filename, MyFileArray original)
        {

           if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename,
               FileMode.Create)))
                {
                    for (int j = 0; j < original.length; j++)
                    {
                        writer.Write(original[j].Item1);
                        writer.Write(original[j].Item2);
                    }
                    this.length = original.length;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            this.fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
        }
        public FileStream fs { get; set; }
        public override (double, int) this[int index]
        {
            get
            {
                Byte[] data = new Byte[8];
                Byte[] dataint = new Byte[4];
                fs.Seek(12 * index, SeekOrigin.Begin);
                fs.Read(data, 0, 8);
                double resultd = BitConverter.ToDouble(data, 0);
                fs.Read(dataint, 0, 4);
                int resultint = BitConverter.ToInt32(dataint, 0);
                return (resultd, resultint);
            }
        }
        /// <summary>
        /// swap two elements in the file
        /// </summary>
        /// <param name="index1">array index of the first element</param>
        /// <param name="index2">array index of the second element</param>
        //public override void Swap(int index1, int index2)
        //{
        //    Byte[] data1 = new Byte[12];
        //    Byte[] data2 = new Byte[12];

        //    var a = this[index1];
        //    var b = this[index2];

        //    BitConverter.GetBytes(a.Item1).CopyTo(data1, 0);
        //    BitConverter.GetBytes(a.Item2).CopyTo(data1, 8);
        //    BitConverter.GetBytes(b.Item1).CopyTo(data2, 0);
        //    BitConverter.GetBytes(b.Item2).CopyTo(data2, 8);

        //    fs.Seek(12 * index1, SeekOrigin.Begin);
        //    fs.Write(data2, 0, 12);
        //    fs.Seek(12 * index2, SeekOrigin.Begin);
        //    fs.Write(data1, 0, 12);

        //}
        /// <summary>
        /// Replace a certain index in the file with chosen value
        /// </summary>
        /// <param name="index">index of the array element to replace</param>
        /// <param name="value">the new va;ie</param>
        public void Replace(int index, (double,int) value)
        {
            Byte[] data = new Byte[12];

            BitConverter.GetBytes(value.Item1).CopyTo(data, 0);
            BitConverter.GetBytes(value.Item2).CopyTo(data, 8);

            fs.Seek(12 * index, SeekOrigin.Begin);
            fs.Write(data, 0, 12);
        }
        public override void Print(int n)
        {
            if(!fs.CanRead) this.fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            for (int i = 0; i < n; i++)
                Console.Write("({0:F2}, {1})", this[i].Item1, this[i].Item2);
            Console.WriteLine();
            fs.Close();
        }
    }
}
