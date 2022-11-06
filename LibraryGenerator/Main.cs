using CppSharp;
using System;

try
{
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
