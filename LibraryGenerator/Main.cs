using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibraryGenerator;

public class Program
{
    public static void Main()
    {
        string sdkPath = Path.Combine(Global.CachePath, "SDK");

        if (!Directory.Exists(sdkPath))
        {
            string[] SDKFolders = new string[] {
                Path.Combine("E:", "Project", "LL.NET-LibraryGenerator", "SDK"),
                Path.Combine("A:", "Documents", "GitHub", "LiteLoaderSDK", "SDK")
            };
            foreach (string folder in SDKFolders)
            {
                if (Directory.Exists(folder))
                {
                    _ = Helper.CopyFolder(folder, sdkPath);
                    break;
                }
            }
        }
        #region Fix
        {
            string Global_h = Path.Combine(sdkPath, "Header", "Global.h");
            string[] lines = File.ReadAllLines(Global_h);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("#include \"third-party"))
                {
                    lines[i] = "//" + lines[i];
                }
            }
            File.WriteAllLines(Global_h, lines);
        }
        #endregion
        PreGenerateHeaderHelper helper = new(Path.Combine(sdkPath, "Header", "MC", "Actor.hpp"));

        helper.Run();

        List<string> IncludeDirs = new()
        {
                Path.GetFullPath(Path.Combine(sdkPath, "Header","MC"))
        };
        #region FixDir
        {
            string[] tryToAdd = new string[]
            {
                Path.Combine("C:", "Program Files", "Microsoft Visual Studio", "2022", "Community", "VC", "Tools", "MSVC", "14.33.31629", "include")
            };
            IncludeDirs.AddRange(tryToAdd.Where(path => Directory.Exists(path)));
        }
        #endregion

        GeneratingUnit unit = new(
            Path.GetFullPath(Path.Combine(sdkPath, "Header", "MC", "AABB.hpp")),
            Path.GetFullPath(Path.Combine(sdkPath, "Lib")),
            @$"output",
            "MC",
            "MODULE")
        {

            IncludeDirs = IncludeDirs.ToArray(),

            Libraries = new string[]
            {
                "bedrock_server_api.lib",
                "bedrock_server_var.lib",
                "LiteLoader.lib",
                "SymDBHelper.lib"
            },

            ValueTypes = new string[]
            {
                "AABB",
                "Vec3",
                "Vec2",
                "BoundingBox"
            }
        };

        unit.Run();
    }
}
