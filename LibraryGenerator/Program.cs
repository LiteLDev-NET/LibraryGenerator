using LibraryGenerator.Utils;
using System.Text.Json;

string originalDataPath = Console.ReadLine();
string originalDataStr = File.ReadAllText(originalDataPath);
OriginalData originalData = JsonSerializer.Deserialize<OriginalData>(originalDataStr);
foreach (KeyValuePair<string, OriginalData.Class> @class in originalData.Classes)
{
    FileHelper helper = new("Minecraft", @class.Key);
    // Doing...
    helper.WriteToFile();
}
