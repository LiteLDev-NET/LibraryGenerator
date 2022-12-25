using LibraryGenerator.Utils;
using System.Text.Json;

string originalDataPath = Console.ReadLine();
string originalDataStr = File.ReadAllText(originalDataPath);
OriginalData originalData = JsonSerializer.Deserialize<OriginalData>(originalDataStr);
foreach (KeyValuePair<string, Dictionary<string, List<OriginalData.Item>>> @class in originalData.classes)
{
    FileHelper helper = new("Minecraft", @class.Key);
    foreach (KeyValuePair<string, List<OriginalData.Item>> type in @class.Value)
    {
        foreach (OriginalData.Item item in type.Value)
        {
            // Doing...
        }
    }
    helper.WriteToFile();
}
