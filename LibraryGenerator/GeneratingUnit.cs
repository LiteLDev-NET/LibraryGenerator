using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Parser;
using System.IO;

namespace LibraryGenerator;

public class GeneratingUnit : ILibrary
{
    private readonly string CppSharpOutputPath = Path.Combine(Global.CachePath, "GeneratedFile");
    public GeneratingUnit(string sourceFilePath, string libPath, string outputDir, string outputNamespace, string moduleName)
    {
        SourceFile = sourceFilePath;
        LibPath = libPath;
        OutputDir = outputDir;
        OutputNamespace = outputNamespace;
        ModuleName = moduleName;
    }

    private string SourceFile { get; set; }
    private string LibPath { get; set; }
    private string OutputDir { get; set; }
    private string OutputNamespace { get; set; }
    private string ModuleName { get; set; }
    public string[] ValueTypes { get; set; }
    public string[] Libraries { get; set; }
    public string[] IncludeDirs { get; set; }

    public void Postprocess(Driver driver, ASTContext ctx)
    {
        if (ValueTypes is null)
        {
            return;
        }

        foreach (string type in ValueTypes)
        {
            ctx.SetClassAsValueType(type);
        }
    }

    public void Preprocess(Driver driver, ASTContext ctx)
    {
    }

    public void Setup(Driver driver)
    {
        ParserOptions parserOptions = driver.ParserOptions;
        DriverOptions options = driver.Options;
        Module module = options.AddModule(ModuleName);


        parserOptions.SetupMSVC();
        parserOptions.LanguageVersion = LanguageVersion.CPP17;
        parserOptions.Verbose = true;


        options.GeneratorKind = GeneratorKind.CLI;
        options.GenerateClassTemplates = true;
        options.GenerateFunctionTemplates = true;
        options.OutputDir = CppSharpOutputPath;


        module.OutputNamespace = OutputNamespace;
        if (LibPath is not null)
        {
            module.LibraryDirs.Add(LibPath);

            if (Libraries is not null)
            {
                foreach (string lib in Libraries)
                {
                    module.Libraries.Add(lib);
                }
            }
        }
        if (IncludeDirs is not null)
        {
            foreach (string includeDir in IncludeDirs)
            {
                module.IncludeDirs.Add(includeDir);
            }
        }
        module.Headers.Add(SourceFile);
    }

    public void SetupPasses(Driver driver)
    {
    }

    public void Run()
    {
        if (Directory.Exists(CppSharpOutputPath))
        {
            Directory.Delete(CppSharpOutputPath, true);
        }

        _ = Directory.CreateDirectory(CppSharpOutputPath);

        ConsoleDriver.Run(this);

        if (!Directory.Exists(OutputDir))
        {
            _ = Directory.CreateDirectory(OutputDir);
        }

        string outputFileName = Path.GetFileNameWithoutExtension(SourceFile);

        File.Copy(Path.Combine(CppSharpOutputPath, outputFileName + ".cpp"), Path.Combine(OutputDir, outputFileName + ".cpp"), true);
        File.Copy(Path.Combine(CppSharpOutputPath, outputFileName + ".h"), Path.Combine(OutputDir, outputFileName + ".hpp"), true);
    }
}