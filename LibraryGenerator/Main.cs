using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibraryGenerator;

public class Program
{
    public static void Main()
    {
        string sdkPath = Path.Combine("SDK");

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

        List<string> IncludeDirs = new()
        {
            Path.GetFullPath(Path.Combine(sdkPath, "include")),
            Path.GetFullPath(Path.Combine(sdkPath, "include", "llapi","mc"))
        };
        #region FixDir
        {
            string[] tryToAdd =
            {
                Path.Combine("C:", "Program Files", "Microsoft Visual Studio", "2022", "Community", "VC", "Tools", "MSVC", "14.33.31629", "include")
            };
            IncludeDirs.AddRange(tryToAdd.Where(path => Directory.Exists(path)));
        }
        #endregion

        GeneratingUnit unit = new(
            Path.GetFullPath(Path.Combine(sdkPath, "include", "llapi", "mc", "AABB.hpp")),
            Path.GetFullPath(Path.Combine(sdkPath, "lib")),
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
