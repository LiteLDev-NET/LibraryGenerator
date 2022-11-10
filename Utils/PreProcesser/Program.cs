using PreProcesser;
using System;
using System.IO;

string input = Environment.CurrentDirectory;
if (!Directory.Exists(input))
{
    throw new FileNotFoundException(input);
}
DirectoryInfo directory = new(Path.Combine(input, "SDK", "include", "llapi"));
foreach (DirectoryInfo subDir in directory.GetDirectories())
{
    foreach (FileInfo file in subDir.GetFiles())
    {
        if(AFTER_EXTRA_Helper.RuningProcessFiles.ContainsKey(file.Name))
        {
            PreGenerateProcess handledFile = new(file.FullName);
            handledFile.Run();
        }
    }
}