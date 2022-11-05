using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LibGenerator
{
    public class PreGenerateHeaderHelper
    {
        public PreGenerateHeaderHelper(string headerPath)
        {
            HeaderPath = headerPath;
        }

        string HeaderPath { get; set; }

        const string PreGenerateHeaderPath = $@"{Global.CachePath}\PreGenerateHeader";
        public void Run()
        {
            if (Directory.Exists(PreGenerateHeaderPath))
                Directory.Delete(PreGenerateHeaderPath, true);
            Directory.CreateDirectory(PreGenerateHeaderPath);

            var inputHeader = File.Open(HeaderPath, FileMode.Open);
            var outputHeader = File.Create($@"{PreGenerateHeaderPath}\{Path.GetFileName(HeaderPath)}");

            if (inputHeader == null || outputHeader == null)
                return;

            StreamReader reader = new(inputHeader);
            StreamWriter writer = new(outputHeader);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (line == null)
                    continue;

                if (line.Contains("gsl::"))
                {
                    var rx = new Regex("class gsl::(.*?)<(.*?)(>+)");
                    var matches = rx.Matches(line);
                    foreach (Match match in matches)
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

            File.Copy($@"{PreGenerateHeaderPath}\{Path.GetFileName(HeaderPath)}", HeaderPath, true);
        }

        static string Handle_not_null(string line, Match match)
        {
            var groups = match.Groups;

            var ret = line.Substring(0, groups[0].Index);
            ret += groups[2].Value;

            if (groups[3].Length > 1)
                for (int i = 0; i < groups[3].Length - 1; i++)
                    ret += '>';

            ret += line.Substring(groups[0].Index + groups[0].Length);

            return ret;
        }

        static string Handle_span(string line, Match match)
        {
            var groups = match.Groups;

            if (groups[0].Value.Contains("std::string"))
            {
                var ret = line.Substring(0, groups[0].Index);

                ret += "std::string";

                if (groups[3].Length > 1)
                    for (int i = 0; i < groups[3].Length - 1; i++)
                        ret += '>';

                ret += line.Substring(groups[0].Index + groups[0].Length);

                return ret;
            }

            return line;
        }
    }
}
