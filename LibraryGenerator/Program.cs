using LibraryGenerator.Utils;
using System.Text.Json;
using System.Text.RegularExpressions;

string originalDataPath = Console.ReadLine();
string originalDataStr = File.ReadAllText(originalDataPath);
OriginalData originalData = JsonSerializer.Deserialize<OriginalData>(originalDataStr);
foreach ((string className, OriginalData.Class classData) in originalData.Classes)
{
    FileHelper helper = new("Minecraft", className);
    // Doing...
    string dirPath = Path.Combine("Out", "Minecraft");
    if (!Directory.Exists(dirPath))
    {
        _ = Directory.CreateDirectory(dirPath);
    }
    File.WriteAllText(Path.Combine(dirPath, $"{Project.FileNameRegex().Match(className).Value}.cs"), helper.ToString());
}

internal partial class Project
{
    [GeneratedRegex("[\\w]+")]
    internal static partial Regex FileNameRegex();
}
