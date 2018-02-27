using Shell32;
using System;
using System.IO;
using Id3Lib;
using Mp3Lib;

namespace sh_mp3
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\ttxy\desktop\music\";
            
            func0(path);

            Console.ReadKey();
        }

        private static void func0(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string f in files)
            {
                Mp3File mp3 = new Mp3File(f);
                TagHandler thd = mp3.TagHandler;
                string tmp = f.Substring(path.Length, f.Length - path.Length);
                string[] fn = tmp.Split('-');
                //Console.WriteLine(fn[0] + ":::" + fn[1]);
                if (fn.Length == 4)
                {
                    thd.Title = fn[3];
                    thd.Artist = fn[0] + fn[1] + fn[2];
                }
                else if (fn.Length == 3)
                {
                    thd.Title = fn[2];
                    thd.Artist = fn[0] + fn[1];
                }
                else if (fn.Length == 2)
                {
                    thd.Title = fn[1];
                    thd.Artist = fn[0];
                }
                else
                {
                    Console.WriteLine(tmp);
                }
                mp3.TagHandler = thd;
                mp3.Update();
            }
            Console.WriteLine("Finish");
        }

        private static void func1(string[] files)
        {
            ShellClass sh = new ShellClass();
            foreach (string f in files)
            {
                Folder dir = sh.NameSpace(Path.GetDirectoryName(f));
                FolderItem item = dir.ParseName(Path.GetFileName(f));
                string det = dir.GetDetailsOf(item, 21);
                Console.WriteLine(f + "-------" + det);
                Console.ReadKey();
            }
        }
    }
}
