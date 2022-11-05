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
    #region Unused
    public void Postprocess(Driver driver, ASTContext ctx)
    {
    }

    public void Preprocess(Driver driver, ASTContext ctx)
    {
    }

    public void SetupPasses(Driver driver)
    {
    }
    #endregion
    public void Setup(Driver driver)
    {
        // C++版本
        driver.ParserOptions.LanguageVersion = LanguageVersion.CPP17;
        // 详细输出
        driver.Options.Verbose = true;
        driver.ParserOptions.Verbose = true;
        // 预处理定义
        driver.ParserOptions.AddDefines("NDEBUG");
        driver.ParserOptions.AddDefines("LITELOADER_EXPORTS");
        driver.ParserOptions.AddDefines("WIN32_LEAN_AND_MEAN");
        driver.ParserOptions.AddDefines("_CRT_SECURE_NO_WARNINGS");
        driver.ParserOptions.AddDefines("_WINDOWS");
        driver.ParserOptions.AddDefines("_USRDLL");
        driver.ParserOptions.AddDefines("_AMD64_");
        driver.ParserOptions.AddDefines("NOMINMAX");
        driver.ParserOptions.AddDefines("%(PreprocessorDefinitions)");
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
        module.IncludeDirs.Add(Path.Combine(Directory.GetCurrentDirectory(), "SDK", "include"));

        // 静态库
        module.LibraryDirs.Add(Path.Combine(Directory.GetCurrentDirectory(), "SDK", "lib"));
        module.Libraries.Add("bedrock_server_api");
        module.Libraries.Add("bedrock_server_var");
        module.Libraries.Add("LiteLoader");
        module.Libraries.Add("SymDBHelper");
    }

    internal void Run()
    {
        foreach (FileInfo file in new DirectoryInfo(InputPath).GetFiles())
        {
            Console.WriteLine($"Doing {file.Name} to {Module}...");
            if (file.Extension is ".h" or ".hpp")
            {
                CurrectFile = file.FullName;
                ConsoleDriver.Run(this);
            }
        }
    }
}
