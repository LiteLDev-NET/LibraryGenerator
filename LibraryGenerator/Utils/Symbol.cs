namespace LibraryGenerator.Utils;

internal class Func
{
    private readonly List<string> _builder;
    internal Func()
    {
        _builder = new();
    }
    public override string ToString()
    {
        return string.Join('\n', GetLines());
    }

    internal List<string> GetLines()
    {
        List<string> cache = new(_builder);
        cache.Insert(0, $"{Helper.indent}{{");
        cache.Insert(0, $"{Helper.indent}{string.Empty} {string.Empty} {string.Empty}({string.Empty})");
        cache.Add($"{Helper.indent}}}");
        return cache;
    }
}
