using LibraryGenerator;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PreProcesser;

internal class PreGenerateProcess
{
    public string CurrentFile { get; set; }

    public string FileName { get; set; }

    private bool _IsInside_AFTER_EXTRA;

    public PreGenerateProcess(string filePath)
    {
        CurrentFile = filePath;
        FileName = Path.GetFileName(filePath);
        _IsInside_AFTER_EXTRA = false;
    }

    public void Run()
    {
        int AFTER_EXTRA_startIndex = 0;

        string[] lines = File.ReadAllLines(CurrentFile);
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line is "#undef AFTER_EXTRA")
            {
                _IsInside_AFTER_EXTRA = false;
            }

            HandleInputLine(ref line);

            if (NeedToHandle() is true)
            {
                if (line is not null)
                {
                    if (_IsInside_AFTER_EXTRA && !line.StartsWith("//") && !string.IsNullOrWhiteSpace(line))
                    {
                        lines[i] = "// " + line;
                    }
                }
            }

            if (line is "#define AFTER_EXTRA")
            {
                _IsInside_AFTER_EXTRA = true;
                AFTER_EXTRA_startIndex = i;
            }
        }

        File.WriteAllLines(CurrentFile, lines.Take(AFTER_EXTRA_startIndex + 1));

        if (AFTER_EXTRA_Helper.RuningProcessFiles.ContainsKey(FileName))
            File.WriteAllLines(CurrentFile, AFTER_EXTRA_Helper.RuningProcessFiles[FileName]);

        File.WriteAllLines(CurrentFile, lines.Skip(AFTER_EXTRA_startIndex + 1));
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
                        "char const" => "std::string",
                        _ => charType
                    };
                    line = line.Replace($"class gsl::basic_string_span<{charType}, {rx.Groups["value"].Value}>", stringType);
                }
            }
            else if (line.Contains("gsl::span"))
            {
                Match rx = RegexHelper.gsl_basic_string_span_regex.Match(line);
                if (rx.Success)
                {
                    line = "// " + line;
                }
            }
        }
    }

    protected bool NeedToHandle()
    {
        return AFTER_EXTRA_Helper.RuningProcessFiles.ContainsKey(FileName);
    }

}

