using SimpleFileReader.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
