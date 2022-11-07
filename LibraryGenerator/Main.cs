using CppSharp;
using LibraryGenerator;
using System;

try
{
    var handledFile = new PreGenerateProcess(args[1]);
    handledFile.Run();

    ConsoleDriver.Run(new GeneratingUnit()
    {
        Module = args[0],
        CurrectFile = args[1]
    });
}
catch (NullReferenceException ex)
{
    Console.WriteLine(ex);
}
