using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Parser;
using LibraryGenerator;
using System;
using System.Collections.Generic;
using System.IO;

internal class GeneratingUnit : ILibrary
{
    // 模块名
    internal string Module { get; set; }
    // 文件名
    internal string CurrectFile { get; set; }
    //不需要解析的文件
    internal static readonly List<string> SkipFiles = new()
    {
        "Types.hpp",
        "AABB.hpp",
        "Vec3.hpp",
        "Vec2.hpp",
        "BoundingBox.hpp",
        "BlockInstance.hpp",
    };
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
        driver.ParserOptions.LanguageVersion = LanguageVersion.CPP20;
        // 转换类型
        driver.Options.GeneratorKind = GeneratorKind.CLI;
        // 预处理定义
        driver.ParserOptions.AddDefines("NDEBUG");
        driver.ParserOptions.AddDefines("LITELOADER_EXPORTS");
        driver.ParserOptions.AddDefines("WIN32_LEAN_AND_MEAN");
        driver.ParserOptions.AddDefines("_CRT_SECURE_NO_WARNINGS");
        driver.ParserOptions.AddDefines("_WINDOWS");
        driver.ParserOptions.AddDefines("_USRDLL");
        driver.ParserOptions.AddDefines("_AMD64_");
        driver.ParserOptions.AddDefines("NOMINMAX");

        Module module = driver.Options.AddModule(Module);
        module.Headers.Add(CurrectFile);

        // 引用目录
        module.IncludeDirs.Add(Path.Combine(Environment.CurrentDirectory, "SDK", "include"));
        foreach (string path in Utils.GetAllChildDir(Path.Combine(Environment.CurrentDirectory, "SDK", "include", "llapi")))
        {
            module.IncludeDirs.Add(path);
        }

        // 静态库
        module.LibraryDirs.Add(Path.Combine(Environment.CurrentDirectory, "SDK", "lib"));
        module.Libraries.Add("bedrock_server_api");
        module.Libraries.Add("bedrock_server_var");
        module.Libraries.Add("LiteLoader");
        module.Libraries.Add("SymDBHelper");

        // 输出目录
        string outputPath = Path.Combine(Environment.CurrentDirectory, "output", Module);
        if (!Directory.Exists(outputPath))
        {
            _ = Directory.CreateDirectory(outputPath);
        }
        driver.Options.OutputDir = outputPath;
    }
}
