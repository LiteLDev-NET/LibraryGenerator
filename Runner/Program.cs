﻿using System;
using System.Diagnostics;
using System.IO;

foreach (FileInfo file in new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "SDK", "include", "llapi", "mc")).GetFiles())
{
    if (file.Extension is ".h" or ".hpp" || !File.Exists(Path.Combine(Environment.CurrentDirectory, "output", "Minecraft", Path.ChangeExtension(file.Name, ".h"))))
    {
        Process.Start(args[0], $"Minecraft {file.FullName}").WaitForExit();
    }
}

foreach (FileInfo file in new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "SDK", "include", "llapi")).GetFiles())
{
    if (file.Extension is ".h" or ".hpp" || !File.Exists(Path.Combine(Environment.CurrentDirectory, "output", "LiteLoader", Path.ChangeExtension(file.Name, ".h"))))
    {
        Process.Start(args[0], $"LiteLoader {file.FullName}").WaitForExit();
    }
}

foreach (FileInfo file in new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "SDK", "include", "llapi", "perm")).GetFiles())
{
    if (file.Extension is ".h" or ".hpp" || !File.Exists(Path.Combine(Environment.CurrentDirectory, "output", "Permission", Path.ChangeExtension(file.Name, ".h"))))
    {
        Process.Start(args[0], $"Permission {file.FullName}").WaitForExit();
    }
}