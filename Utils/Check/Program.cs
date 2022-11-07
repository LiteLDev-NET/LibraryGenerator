using System;
using System.Collections.Generic;
using System.IO;

string input = Console.ReadLine();
if (!Directory.Exists(input))
{
    throw new FileNotFoundException(input);
}
string outPutPath = Path.Combine(input, "output");
Dictionary<string, FileInfo[]> files = new()
{
    {
        "Minecraft",
        new DirectoryInfo(Path.Combine(input, "SDK", "include", "llapi", "mc")).GetFiles("*.h*")
    },
    {
        "LiteLoader",
        new DirectoryInfo(Path.Combine(input, "SDK", "include", "llapi")).GetFiles("*.h*")
    },
    {
        "Permission",
        new DirectoryInfo(Path.Combine(input, "SDK", "include", "llapi", "perm")).GetFiles("*.h*")
    }
};

foreach (KeyValuePair<string, FileInfo[]> file in files)
{
    foreach (FileInfo fileInfo in file.Value)
    {
        if (!File.Exists(Path.Combine(outPutPath, file.Key, Path.ChangeExtension(fileInfo.Name, ".h"))))
        {
            File.AppendAllText("unconvertedFiles.log", $"{fileInfo.Name}\n");
        }
    }
}