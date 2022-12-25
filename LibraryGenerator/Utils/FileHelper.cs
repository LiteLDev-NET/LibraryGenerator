namespace LibraryGenerator.Utils
{
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
        internal void WriteToFile()
        {
            List<string> cache = new(_builder);
            cache.Insert(0, "{");
            cache.Insert(0, $"public class {_className}");
            cache.Insert(0, $"namespace {_nameSpace};");
            cache.Add("}");
            cache.Add(string.Empty);
            File.WriteAllLines(Path.Combine(_nameSpace, $"{_className}.cs"), cache);
        }
    }
}
