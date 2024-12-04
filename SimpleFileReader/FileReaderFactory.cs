using SimpleFileReader.Implementations;

namespace SimpleFileReader
{
    public class FileReaderFactory
    {
        public IFileReader<T> GetFileReader<T>(string extension) where T : class, new()
            => extension switch
            {
                ".toml" => new TomlFileReader<T>(),
                _ => throw new KeyNotFoundException("No file reader associated with that extension")
            };
    }
}
