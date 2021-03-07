using System;
using System.IO;

namespace FileConcat
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = @"C:\Temp\20210306";
            var dstPath = Path.Combine(basePath, "output.mp4");

            using (BinaryWriter sw = new BinaryWriter(File.OpenWrite(dstPath)))
            {
                for (int i = 1; i <= 5; i++)
                {
                    var path = string.Format("{0}\\{1}.mp4", basePath, i);
                    var bytes = File.ReadAllBytes(path);
                    sw.Write(bytes);
                }
            }
        }
    }
}
