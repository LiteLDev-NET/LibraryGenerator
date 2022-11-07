using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryGenerator;

internal class PreGenerateProcess
{
    public string CurrentFile { get; set; }

    private bool IsInside_AFTER_EXTRA;

    public PreGenerateProcess(string filePath)
    {
        CurrentFile = filePath;
        IsInside_AFTER_EXTRA = false;
    }

    public void Run()
    {
        var fileInfo = new FileInfo(CurrentFile);
        var handledFilePath = Path.Combine(CacheDir, fileInfo.Name);

        var outputFile = File.Create(handledFilePath);
        var inputFile = File.Open(CurrentFile, FileMode.Open);
        var reader = new StreamReader(inputFile);
        var writer = new StreamWriter(outputFile);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            HandleInputLine(ref line);

            if (line != null && !IsInside_AFTER_EXTRA)
                writer.WriteLine(line);
        }

        reader.Close();
        writer.Close();
        inputFile.Close();
        outputFile.Close();

        File.Copy(handledFilePath, CurrentFile, true);
        File.Delete(handledFilePath);
    }

    /// <summary>
    /// 缓存目录
    /// </summary>
    public static string CacheDir { get; private set; }

    static PreGenerateProcess()
    {
        try
        {
            var cacheDir = Path.Combine(Environment.CurrentDirectory, "cache");
            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);
            CacheDir = cacheDir;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    protected enum LineType
    {
        std_optional,
        gsl_not_null,
    }

    protected void HandleInputLine(ref string line)
    {
        if (line == "#define AFTER_EXTRA")
        {
            IsInside_AFTER_EXTRA = true;
            return;
        }

        if (line == "#undef AFTER_EXTRA")
        {
            IsInside_AFTER_EXTRA = false;
            return;
        }

        if (IsInside_AFTER_EXTRA)
            return;

        if (line.Contains("gsl::"))
        {
            if (line.Contains("gsl::not_null"))
            {
                var rx = RegexHelper.gsl_not_null_regex.Match(line);
                if (rx.Success)
                {
                    var classType = rx.Groups["class_type"].Value;
                    line = line.Replace($"class gsl::not_null<{classType}>", classType);
                }
            }
            else if (line.Contains("gsl::basic_string_span"))
            {
                var rx = RegexHelper.gsl_basic_string_span_regex.Match(line);
                if (rx.Success)
                {
                    var charType = rx.Groups["char_type"].Value;
                    var stringType = charType switch
                    {
                        "char" => "std::string",
                        "wchar_t" => "std::wstring",
                        _ => throw new Exception()
                    };
                    line = line.Replace($"class gsl::basic_string_span<{charType}, {rx.Groups["value"].Value}>", stringType);
                }
            }
        }
    }


}

