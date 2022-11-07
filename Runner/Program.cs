using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

Dictionary<string, FileInfo[]> files = new()
{
    {
        "Minecraft",
        new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "SDK", "include", "llapi", "mc")).GetFiles("*.h*")
    },
    {
        "LiteLoader",
        new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "SDK", "include", "llapi")).GetFiles("*.h*")
    },
    {
        "Permission",
        new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "SDK", "include", "llapi", "perm")).GetFiles("*.h*")
    }
};
foreach (KeyValuePair<string, FileInfo[]> file in files)
{
    List<Task> tasks = new();
    int index = 0;
    for (int i = 0; i < Environment.ProcessorCount; i++)
    {
        Task task = new(() =>
        {
            while (file.Value.Length > index)
            {
                int localIndex = index++;
                if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "output", file.Key, Path.ChangeExtension(file.Value[localIndex].Name, ".h"))))
                {
                    Process.Start(args[0], $"{file.Key} {file.Value[localIndex].FullName}").WaitForExit();
                }
            }
        });
        task.Start();
        tasks.Add(task);
    }
    foreach (Task task in tasks)
    {
        task.Wait();
    }
}