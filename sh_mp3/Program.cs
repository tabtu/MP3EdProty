using Shell32;
using System;
using System.IO;
using Id3Lib;
using Mp3Lib;
using NPinyin;
using System.Collections.Generic;
using System.Text;

namespace sh_mp3
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\ttxy\desktop\temp\t\";

            //func0(path);
            func1(path);
            //func2(path);

            Console.ReadKey();
        }

        private static void func1(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string f in files)
            {
                string tmp = f.Substring(path.Length, f.Length - path.Length);
                Console.WriteLine(tmp);
                string[] fn = tmp.Split('-');
                //tmp = ToDBC(tmp);

                // 添加后缀
                //File.Move(f, f + ".mp3");

                // 去除空格
                //string filename = fn[0].Trim() + " - " + fn[1].Trim();

                // 替换_到&
                //string fn = tmp.Replace("_", "&");

                // 去除括号
                //string[] fn = tmp.Split('(');
                //if (fn.Length > 1)
                //{
                //    string filename = fn[0].Trim();
                //    File.Move(f, path + filename);
                //}

                //更新mp3信息
                Mp3File mp3 = new Mp3File(f);
                TagHandler thd = mp3.TagHandler;
                //thd.Title = CapFirstLetter(Pinyin.GetPinyin(fn[1].Substring(0, fn[1].Length - 4).Trim()));
                //thd.Artist = CapFirstLetter(Pinyin.GetPinyin(fn[0].Trim()));
                thd.Title = fn[1].Substring(0, fn[1].Length - 4).Trim();
                thd.Artist = fn[0].Trim();
                thd.Year = "";
                thd.Album = "";
                thd.Disc = "";
                thd.Track = "";
                thd.Genre = "";
                mp3.TagHandler = thd;
                mp3.Update();
            }
            Console.WriteLine("Finish");
        }

        private static void func2(string path)
        {
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length - 1; i++)
            {
                for (int j = i + 1; j < files.Length; j++)
                {
                    string n1 = files[i].Split('-')[1].Trim();
                    string n2 = files[j].Split('-')[1].Trim();
                    if (n1.Substring(0, n1.Length - 4) == n2.Substring(0, n2.Length - 4))
                    {
                        Console.WriteLine(n1);
                    }
                }
            }
            Console.WriteLine("Finish");
        }

        private static void func0(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string f in files)
            {
                Console.WriteLine(f);
                Mp3File mp3 = new Mp3File(f);
                TagHandler thd = mp3.TagHandler;
                string artist = Pinyin.GetPinyin(thd.Artist);
                thd.Artist = CapFirstLetter(artist).Trim();
                string title = Pinyin.GetPinyin(thd.Title);
                thd.Title = CapFirstLetter(title).Trim();
                mp3.TagHandler = thd;
                mp3.Update();
            }
            Console.WriteLine("Finish");
        }

        private static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;
            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);
            return str.ToUpper();
        }

        private static string CapFirstLetter(string inpt)
        {
            string[] words = inpt.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = FirstLetterToUpper(words[i].Trim());
                //words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
            string result = "";
            foreach(string w in words)
            {
                result += (w + " ");
            }
            return result;
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

        /**/
        // /
        // / 转半角的函数(DBC case)
        // /
        // /任意字符串
        // /半角字符串
        // /
        // /全角空格为12288，半角空格为32
        // /其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        // /
        private static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
    }
}
