using LibraryGenerator;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PreProcesser;

internal class PreGenerateProcess
{
    public string CurrentFile { get; set; }

    private bool _IsInside_AFTER_EXTRA;

    public PreGenerateProcess(string filePath)
    {
        CurrentFile = filePath;
        _IsInside_AFTER_EXTRA = false;
    }

    public void Run()
    {
        FileInfo fileInfo = new(CurrentFile);
        string handledFilePath = Path.Combine(CacheDir, fileInfo.Name);

        FileStream outputFile = File.Create(handledFilePath);
        FileStream inputFile = File.Open(CurrentFile, FileMode.Open);
        StreamReader reader = new(inputFile);
        StreamWriter writer = new(outputFile);

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            if (line is "#undef AFTER_EXTRA")
            {
                _IsInside_AFTER_EXTRA = false;
            }

            HandleInputLine(ref line);
            if (line is not null)
            {
                if (_IsInside_AFTER_EXTRA && !line.StartsWith("//") && !string.IsNullOrWhiteSpace(line))
                {
                    writer.Write("// ");
                }

                writer.WriteLine(line);
            }

            if (line is "#define AFTER_EXTRA")
            {
                _IsInside_AFTER_EXTRA = true;
            }

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
        string cacheDir = Path.Combine(Environment.CurrentDirectory, "cache");
        if (!Directory.Exists(cacheDir))
        {
            _ = Directory.CreateDirectory(cacheDir);
        }

        CacheDir = cacheDir;
    }

    protected enum LineType
    {
        std_optional,
        gsl_not_null,
    }

    protected void HandleInputLine(ref string line)
    {

        if (_IsInside_AFTER_EXTRA)
        {
            return;
        }

        if (line.Contains("gsl::"))
        {
            if (line.Contains("gsl::not_null"))
            {
                Match rx = RegexHelper.gsl_not_null_regex.Match(line);
                if (rx.Success)
                {
                    string classType = rx.Groups["class_type"].Value;
                    line = line.Replace($"class gsl::not_null<{classType}>", classType);
                }
            }
            else if (line.Contains("gsl::basic_string_span"))
            {
                Match rx = RegexHelper.gsl_basic_string_span_regex.Match(line);
                if (rx.Success)
                {
                    string charType = rx.Groups["char_type"].Value;
                    string stringType = charType switch
                    {
                        "char" => "std::string",
                        "wchar_t" => "std::wstring",
                        _ => charType
                    };
                    line = line.Replace($"class gsl::basic_string_span<{charType}, {rx.Groups["value"].Value}>", stringType);
                }
            }
        }
    }


}

