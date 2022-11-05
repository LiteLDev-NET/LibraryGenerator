using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LibraryGenerator;

public class PreGenerateHeaderHelper
{
    public PreGenerateHeaderHelper(string headerPath)
    {
        HeaderPath = headerPath;
    }

    private string HeaderPath { get; set; }

    private readonly string PreGenerateHeaderPath = Path.Combine(Global.CachePath, "PreGenerateHeader");
    public void Run()
    {
        if (Directory.Exists(PreGenerateHeaderPath))
        {
            Directory.Delete(PreGenerateHeaderPath, true);
        }

        _ = Directory.CreateDirectory(PreGenerateHeaderPath);

        FileStream inputHeader = File.Open(HeaderPath, FileMode.Open);
        string path = Path.Combine(PreGenerateHeaderPath, Path.GetFileName(HeaderPath));
        FileStream outputHeader = File.Create(path);

        if (inputHeader is null || outputHeader is null)
        {
            return;
        }

        StreamReader reader = new(inputHeader);
        StreamWriter writer = new(outputHeader);

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();

            if (line is null)
            {
                continue;
            }

            if (line.Contains("gsl::"))
            {
                Regex rx = new("class gsl::(.*?)<(.*?)(>+)");
                MatchCollection matches = rx.Matches(line);
                foreach (Match match in matches.Cast<Match>())
                {
                    line = match.Groups[1].Value switch
                    {
                        "not_null" => Handle_not_null(line, match),
                        "span" => Handle_span(line, match),
                        _ => line
                    };


                }
            }

            writer.WriteLine(line);
        }

        reader.Close();
        writer.Close();

        File.Copy(path, HeaderPath, true);
    }

    private static string Handle_not_null(string line, Match match)
    {
        GroupCollection groups = match.Groups;

        string ret = line[..groups[0].Index];
        ret += groups[2].Value;

        if (groups[3].Length > 1)
        {
            for (int i = 0; i < groups[3].Length - 1; i++)
            {
                ret += '>';
            }
        }

        ret += line[(groups[0].Index + groups[0].Length)..];

        return ret;
    }

    private static string Handle_span(string line, Match match)
    {
        GroupCollection groups = match.Groups;

        if (groups[0].Value.Contains("std::string"))
        {
            string ret = line[..groups[0].Index];

            ret += "std::string";

            if (groups[3].Length > 1)
            {
                for (int i = 0; i < groups[3].Length - 1; i++)
                {
                    ret += '>';
                }
            }

            ret += line[(groups[0].Index + groups[0].Length)..];

            return ret;
        }

        return line;
    }
}
