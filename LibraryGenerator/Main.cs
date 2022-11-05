using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGenerator;

namespace Process
{
    public class Program
    {
        public static void Main()
        {
            string sdkPath = Path.Combine(Global.CachePath, "SDK");

            if (!Directory.Exists(sdkPath))
            {
                var SDKFolders = new string[] {
                    @"E:\Project\LL.NET-LibraryGenerator\SDK",
                    @"A:\Documents\GitHub\LiteLoaderSDK\SDK"
                };
                foreach (var folder in SDKFolders)
                {
                    if (Directory.Exists(folder))
                    {
                        Helper.CopyFolder(folder, sdkPath);
                        break;
                    }
                }
            }
            #region Fix
            {
                var Global_h = Path.Combine(sdkPath, "Header", "Global.h");
                var lines = File.ReadAllLines(Global_h);
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
            var helper = new PreGenerateHeaderHelper(Path.Combine(sdkPath, "Header", "MC", "Actor.hpp"));

            helper.Run();

            var IncludeDirs = new List<string>
                {
                    Path.GetFullPath(Path.Combine(sdkPath, "Header","MC"))
                };
            #region FixDir
            {
                string[] tryToAdd = new string[]
                {
                    @"C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Tools\MSVC\14.32.31326\include"
                };
                IncludeDirs.AddRange(tryToAdd.Where(path => Directory.Exists(path)));
            }
            #endregion

            var unit = new GeneratingUnit(
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
}
