using CppSharp;
using CppSharp.AST;
using CppSharp.Parser;
using CppSharp.Generators;

namespace LibGenerator
{
    public class GeneratingUnit : ILibrary
    {
        const string CppSharpOutputPath = @$"{Global.CachePath}\GeneratedFile";
        public GeneratingUnit(string sourceFilePath, string? libPath, string outputDir, string outputNamespace, string moduleName)
        {
            SourceFile = sourceFilePath;
            LibPath = libPath;
            OutputDir = outputDir;
            OutputNamespace = outputNamespace;
            ModuleName = moduleName;
        }

        string SourceFile { get; set; }
        string? LibPath { get; set; }
        string OutputDir { get; set; }
        string OutputNamespace { get; set; }
        string ModuleName { get; set; }
        public string[]? ValueTypes { get; set; }
        public string[]? Libraries { get; set; }
        public string[]? IncludeDirs { get; set; }

        public void Postprocess(Driver driver, ASTContext ctx)
        {
            if (ValueTypes == null)
                return;

            foreach (var type in ValueTypes)
            {
                ctx.SetClassAsValueType(type);
            }
        }

        public void Preprocess(Driver driver, ASTContext ctx)
        {
        }

        public void Setup(Driver driver)
        {
            var parserOptions = driver.ParserOptions;
            var options = driver.Options;
            var module = options.AddModule(ModuleName);


            parserOptions.SetupMSVC();
            parserOptions.LanguageVersion = LanguageVersion.CPP17;
            parserOptions.Verbose = true;


            options.GeneratorKind = GeneratorKind.CLI;
            options.GenerateClassTemplates = true;
            options.GenerateFunctionTemplates = true;
            options.OutputDir = CppSharpOutputPath;


            module.OutputNamespace = OutputNamespace;
            if (LibPath != null)
            {
                module.LibraryDirs.Add(LibPath);

                if (Libraries != null)
                    foreach (var lib in Libraries)
                        module.Libraries.Add(lib);

            }
            if (IncludeDirs != null)
            {
                foreach (var includeDir in IncludeDirs)
                    module.IncludeDirs.Add(includeDir);
            }
            module.Headers.Add(SourceFile);
        }

        public void SetupPasses(Driver driver)
        {
        }

        public void Run()
        {
            if (Directory.Exists(CppSharpOutputPath))
                Directory.Delete(CppSharpOutputPath, true);
            Directory.CreateDirectory(CppSharpOutputPath);

            ConsoleDriver.Run(this);

            if (!Directory.Exists(OutputDir))
                Directory.CreateDirectory(OutputDir);

            var outputFileName = Path.GetFileNameWithoutExtension(SourceFile);

            File.Copy(Path.Combine(CppSharpOutputPath, outputFileName + ".cpp"), Path.Combine(OutputDir, outputFileName + ".cpp"), true);
            File.Copy(Path.Combine(CppSharpOutputPath, outputFileName + ".h"), Path.Combine(OutputDir, outputFileName + ".h"), true);
        }
    }

    //public class Generator : ILibrary
    //{
    //    public static void Main()
    //    {
    //        ConsoleDriver.Run(new Generator(@"E:\Project\LL.NET-LibraryGenerator\output"));
    //    }
    //    string OutputDir { get; set; }
    //    public Generator(string outputDir)
    //    {
    //        OutputDir = outputDir;
    //    }
    //    public static void Run(string outputDir)
    //    {
    //        ConsoleDriver.Run(new Generator(outputDir));
    //    }
    //    public void Postprocess(Driver driver, ASTContext ctx)
    //    {
    //        ctx.SetClassAsValueType("Vec3");
    //        ctx.SetClassAsValueType("Vec2");
    //        ctx.SetClassAsValueType("AABB");
    //        ctx.SetClassAsValueType("BoundingBox");

    //    }

    //    public void Preprocess(Driver driver, ASTContext ctx)
    //    {
    //    }

    //    public void Setup(Driver driver)
    //    {
    //        driver.ParserOptions.SetupMSVC(VisualStudioVersion.Latest);
    //        Console.WriteLine(driver.ParserOptions.ClangVersion);
    //        driver.ParserOptions.LanguageVersion = CppSharp.Parser.LanguageVersion.CPP17;
    //        var options = driver.Options;
    //        options.GeneratorKind = CppSharp.Generators.GeneratorKind.CLI;
    //        options.GenerateClassTemplates = true;
    //        options.GenerateFunctionTemplates = true;
    //        options.OutputDir = OutputDir;
    //        var module = options.AddModule("QAQ");
    //        module.OutputNamespace = "MC";
    //        module.IncludeDirs.Add(@"E:\Project\LL.NET-LibraryGenerator\SDK\Header\MC");
    //        module.Headers.Add(@"E:\Project\LL.NET-LibraryGenerator\SDK\Header\MC"+@"\"+"Actor.hpp"/*Directory.GetFiles(@"F:\Desktop\PluginTemplate-master\LibGenerator\bin\Debug\net6.0\SDK\Header\MC").Skip(32).Take(1)*/);
    //        //module.Headers.Add("Types.hpp");
    //        module.LibraryDirs.Add(@"E:\Project\LL.NET-LibraryGenerator\SDK\Lib");
    //        module.Libraries.Add("bedrock_server_api.lib");
    //        module.Libraries.Add("bedrock_server_var.lib");
    //        module.Libraries.Add("LiteLoader.lib");
    //        module.Libraries.Add("SymDBHelper.lib");
    //    }

    //    public void SetupPasses(Driver driver)
    //    {
    //    }
    //}
}