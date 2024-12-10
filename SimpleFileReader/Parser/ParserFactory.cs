using System.Reflection;

namespace SimpleFileReader.Parser
{
    public class ParserFactory
    {
        private Dictionary<string, IParser> _parsers;
        public ParserFactory() 
        {
            _parsers = new Dictionary<string, IParser>();

            // Retrieve associated assembly
            var assembly = Assembly.GetAssembly(typeof(IParser));

            if (assembly == null)
                throw new Exception($"Could not locate assembly of \"{typeof(IParser).Name}\"");

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

        public IParser? GetParser(string extension)
            => _parsers.ContainsKey(extension) ? _parsers[extension] : null;
    }
}
