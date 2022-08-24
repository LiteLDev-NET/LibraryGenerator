using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

internal class Utils
{
    // 已修复列表
    private static readonly List<string> fixedFiles = new();
    internal static void FixInclude(FileInfo fileInfo)
    {
        // 跳过已修复
        if (fixedFiles.Contains(fileInfo.FullName))
        {
            return;
        }
        string[] lines = File.ReadAllLines(fileInfo.FullName);
        // 注释引用
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("#include \"third-party"))
            {
                Console.WriteLine($"Fixing line {i} in {fileInfo.Name}...");
                lines[i] = $"// {lines[i]}  // Fixed by LibraryGenerator";
            }
        }
        // 回写
        File.WriteAllLines(fileInfo.FullName, lines);
        fixedFiles.Add(fileInfo.FullName);
        foreach (string line in lines)
        {
            MatchCollection matches = Regex.Matches(line, "^#include \"(.+?)\"");
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
