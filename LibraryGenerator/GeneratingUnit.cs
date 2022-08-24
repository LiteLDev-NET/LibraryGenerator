using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Parser;
using System;
using System.IO;

internal class GeneratingUnit : ILibrary
{
    // 模块名
    internal string Module { get; set; }
    // 文件目录
    internal string InputPath { get; set; }
    // 文件名
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
        // C++版本
        driver.ParserOptions.LanguageVersion = LanguageVersion.CPP17_GNU;
        // 详细输出
        driver.Options.Verbose = true;
        driver.ParserOptions.Verbose = true;
        // 修复winnt.h报错
        driver.ParserOptions.AddDefines("_AMD64_");

        // 转换类型
        driver.Options.GeneratorKind = GeneratorKind.CLI;
        // 输出目录
        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output", Module);
        if (!Directory.Exists(outputPath))
        {
            _ = Directory.CreateDirectory(outputPath);
        }
        driver.Options.OutputDir = outputPath;

        Module module = driver.Options.AddModule(Module);
        module.Headers.Add(CurrectFile);

        // 引用目录
        module.IncludeDirs.Add(Path.Combine(Directory.GetCurrentDirectory(), "SDK", "Header"));

        // 静态库
        module.LibraryDirs.Add(Path.Combine(Directory.GetCurrentDirectory(), "SDK", "Lib"));
        module.Libraries.Add(Path.Combine(Directory.GetCurrentDirectory(), "SDK", "Lib", "bedrock_server_api.lib"));
        module.Libraries.Add(Path.Combine(Directory.GetCurrentDirectory(), "SDK", "Lib", "bedrock_server_var.lib"));
        module.Libraries.Add(Path.Combine(Directory.GetCurrentDirectory(), "SDK", "Lib", "LiteLoader.lib"));
        module.Libraries.Add(Path.Combine(Directory.GetCurrentDirectory(), "SDK", "Lib", "SymDBHelper.lib"));
    }

    public void SetupPasses(Driver driver)
    {
    }
    internal void Run()
    {
        foreach (FileInfo file in new DirectoryInfo(InputPath).GetFiles())
        {
            Console.WriteLine($"Doing {file.Name} to {Module}...");
            // 移除第三方引用
            Utils.FixInclude(file);
            if (file.Extension is ".h" or ".hpp")
            {
                CurrectFile = file.FullName;
                ConsoleDriver.Run(this);
            }
        }
    }
}
