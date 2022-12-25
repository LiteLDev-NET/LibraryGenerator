namespace LibraryGenerator.Utils;

internal class FileHelper
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
    public static FileHelper operator +(FileHelper helper, Func symbol)
    {
        // Doing...
        return helper;
    }
    public override string ToString()
    {
        return string.Join('\n', GetLines());
    }

    internal List<string> GetLines()
    {
        List<string> cache = new(_builder);
        cache.Insert(0, "{");
        cache.Insert(0, $"public class {_className}");
        cache.Insert(0, string.Empty);
        cache.Insert(0, $"namespace {_nameSpace};");
        cache.Add("}");
        return cache;
    }
}
