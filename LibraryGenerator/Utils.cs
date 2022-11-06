using System;
using System.Collections.Generic;
using System.IO;

namespace LibraryGenerator
{
    internal static class Utils
    {
        internal static List<string> GetAllChildDir(string path)
        {
            List<string> list = new()
            {
                path
            };
            foreach (string path1 in Directory.GetDirectories(path))
            {
                list.Add(path1);
                List<string> temp = GetAllChildDir(path1);
                if (temp.Count <= 0)
                {
                    break;
                }
                list.AddRange(temp);
            }
            return list;
        }

        static object _lock = new();

        internal static void ColorWriteLine(ConsoleColor foreground, ConsoleColor background, string format, params object[] args)
        {
            lock (_lock)
            {
                var currentForeground = Console.ForegroundColor;
                var currentBackground = Console.BackgroundColor;

                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;

                Console.WriteLine(string.Format(format, args));

                Console.ForegroundColor = currentForeground;
                Console.BackgroundColor = currentBackground;
            }
        }

        internal static void ColorWriteLine(ConsoleColor foreground, string format, params object[] args)
        {
            ColorWriteLine(foreground, ConsoleColor.Black, format, args);
        }
    }
}
