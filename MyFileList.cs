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
        public MyFileList(string filename, int n, int seed)
        {
            length = n;
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

    }
}
