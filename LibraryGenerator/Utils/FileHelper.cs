using System.Text.RegularExpressions;

namespace LibraryGenerator.Utils;

internal partial class FileHelper
{
    private readonly string _className;
    private readonly string _nameSpace;
    private readonly List<string> _builder;
    internal FileHelper(string nameSpace, string className)
    {
        _className = className;
        _nameSpace = nameSpace;
        _builder = new();
    }
    internal void WriteToFile()
    {
        List<string> cache = new(_builder);
        cache.Insert(0, "{");
        cache.Insert(0, $"public class {_className}");
        cache.Insert(0, string.Empty);
        cache.Insert(0, $"namespace {_nameSpace};");
        cache.Add("}");
        string dirPath = Path.Combine("Out", _nameSpace);
        if (!Directory.Exists(dirPath))
        {
            _ = Directory.CreateDirectory(dirPath);
        }
        File.WriteAllLines(Path.Combine(dirPath, $"{FileNameRegex().Match(_className).Value}.cs"), cache);
    }

    [GeneratedRegex("[a-zA-Z_]+?")]
    private static partial Regex FileNameRegex();
}
