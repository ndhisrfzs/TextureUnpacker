using System;
using System.IO;

namespace TextureUnpacker
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("请将需要切割的png文件拖拽到exe上");
                Console.ReadKey();
                return;
            }

            string path = args[0];
            Console.WriteLine(Path.GetFullPath(path));
            Console.WriteLine(Path.GetDirectoryName(path));
            Console.WriteLine(Path.GetFileNameWithoutExtension(path));
            Console.WriteLine(Path.GetFileName(path));

            Core.LoadXml(Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".xml"));
            Core.LoadPng(path);
            Core.Cuts();

            Console.ReadKey();
        }
    }
}
