using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader.Implementations
{
    public abstract class BaseFileReader<T> : IFileReader<T> where T : class, new()
    {
        public abstract T ReadFile(string path);

        public string RetrieveData(string path)
        {
            if (File.Exists(path))
                return File.ReadAllText(path);
            throw new FileNotFoundException($"Could not retrieve file at {path}");
        }

        public object CreateInstance(Type type)
        {
            var constructors = type.GetConstructor([]);
            if (constructors != null)
                return constructors.Invoke([]);
            throw new Exception($"No default constructor provided for {type.FullName}");
        }
    }
}
