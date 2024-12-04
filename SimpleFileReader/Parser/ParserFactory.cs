using System.Reflection;

namespace SimpleFileReader.Parser
{
    public class ParserFactory
    {
        private Dictionary<string, IParser> _parsers;
        public ParserFactory() 
        {
            _parsers = new Dictionary<string, IParser>();
            var path = Path.Combine(Environment.CurrentDirectory, "Implementations");
            var assembly = Assembly.LoadFile(path);
            foreach(var type in assembly.GetTypes())
            {
                // Ignore if not a parser
                if (type.GetInterface(nameof(IParser)) == null)
                    continue;

                // Get an empty constructor and all supported extensions
                var constructor = type.GetConstructor([]);
                var extensions = type.GetCustomAttribute<SupportedExtensionsAttribute>();
                if(constructor != null && extensions != null)
                {
                    var parser = (IParser)constructor.Invoke(null);
                    foreach(var extension in extensions.FileExtensions)
                        _parsers.Add(extension, parser);
                }
            }
        }

        public IParser GetParser(string extension)
            => throw new NotImplementedException();
    }
}
