using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

internal class Utils
{
    private static readonly List<string> fixedFiles = new();
    internal static void FixInclude(FileInfo fileInfo)
    {
        if (fixedFiles.Contains(fileInfo.FullName))
        {
            return;
        }
        string[] lines = File.ReadAllLines(fileInfo.FullName);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("#include \"third-party"))
            {
                Console.WriteLine($"Fixing {fileInfo.Name}...");
                lines[i] = $"// {lines[i]}  // Fixed by LibraryGenerator";
            }
        }
        File.WriteAllLines(fileInfo.FullName, lines);
        fixedFiles.Add(fileInfo.FullName);
        for (int i = 0; i < lines.Length; i++)
        {
            MatchCollection matches = Regex.Matches(lines[i], "^#include \"(.+?)\"");
            foreach (Match match in matches)
            {
                string path = Path.Combine(fileInfo.DirectoryName, match.Groups[1].Value);
                if (!File.Exists(path))
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "SDK", "Header", match.Groups[1].Value);
                }
                FixInclude(new FileInfo(path));
            }
        }
    }
}
