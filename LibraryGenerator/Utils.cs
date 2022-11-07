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
    }
}
