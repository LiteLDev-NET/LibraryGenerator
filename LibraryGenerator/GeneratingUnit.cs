using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Parser;
using System;
using System.IO;

internal class GeneratingUnit : ILibrary
{
    internal string Module { get; set; }
    internal string InputPath { get; set; }
    private string CurrectFile { get; set; }
    internal GeneratingUnit()
    {
    }

    public void Postprocess(Driver driver, ASTContext ctx)
    {
    }

    public void Preprocess(Driver driver, ASTContext ctx)
    {
    }

    public void Setup(Driver driver)
    {
        driver.ParserOptions.LanguageVersion = LanguageVersion.CPP17_GNU;
        driver.ParserOptions.Verbose = true;

        driver.Options.GeneratorKind = GeneratorKind.CLI;
        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output", Module);
        if (!Directory.Exists(outputPath))
        {
            _ = Directory.CreateDirectory(outputPath);
        }
        driver.Options.OutputDir = outputPath;

        Module module = driver.Options.AddModule(Module);
        module.Headers.Add(CurrectFile);
    }

    public void SetupPasses(Driver driver)
    {
    }
    internal void Run()
    {
        foreach (FileInfo file in new DirectoryInfo(InputPath).GetFiles())
        {
            Console.WriteLine($"Doing {file.Name} to {Module}...");
            Utils.FixInclude(file);
            if (file.Extension is ".h" or ".hpp")
            {
                CurrectFile = file.FullName;
                try
                {
                    ConsoleDriver.Run(this);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
