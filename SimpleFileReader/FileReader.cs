using SimpleFileReader.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader
{
    public class FileReader<T> : IFileReader<T> where T : class, new()
    {
        private ParserFactory _factory;

        public FileReader()
            => _factory = new ParserFactory();

        public T ReadFile(string path)
        {
            var extension = Path.GetExtension(path);
            var parser = _factory.GetParser(extension) ?? throw new Exception($"No file reader associated with the extension \"{extension}\".");

            // Parse into dictionary
            var inputdict = parser.LoadAndParse(path);

            // Convert dictionary into T
            T res = (T)ConvertObject(typeof(T), inputdict);

            return res;
        }

        public object ConvertObject(Type type, Dictionary<string, object> input)
        {
            var res = CreateInstance(type);
            var props = type.GetProperties(BindingFlags.Public|BindingFlags.Instance);
            foreach (var prop in props)
            {
                var name = prop.Name;
                if (input.ContainsKey(name))
                {
                    var val = input[name];

                    // object can either be an array, dictionary or string
                    object obj;
                    if (val is Dictionary<string, object?>) // nested object
                        obj = ConvertObject(prop.PropertyType, (Dictionary<string, object>)val);
                    else if (val is List<object?>) // array
                        obj = ConvertList(prop.PropertyType, name, (List<object?>)val);
                    else if (val is string) // int, string or double
                        obj = ConvertString(prop.PropertyType, name, (string)val);
                    else
                        throw new NotImplementedException("Parser gave unexpected type.");
                    prop.SetValue(res, obj);
                } 
                /* else default value */
            }
            return res;
        }

        protected object ConvertList(Type target, string name, List<object?> list)
        {
            var elem = target.GetElementType() ?? throw new Exception($"\"{name}\" is not an array but value is.");
            var res = (object[])Array.CreateInstance(elem, list.Count);

            for (var i = 0; i < list.Count; i++)
            {
                object val = list[i] ?? throw new Exception($"\"{name}\"'s array values has a null value.");
                
                if (list[i] is string)
                    res[i] = ConvertString(elem, $"{name}:{i}", (string)val);
                else if (list[i] is Dictionary<string, object>)
                    res[i] = ConvertObject(elem, (Dictionary<string, object>)val);
                else
                    throw new Exception($"\"{name}\"'s array values are not correct.");
            }
            return res;
        }

        protected object ConvertString(Type target, string name, string input)
        {
            if (target == typeof(string))
                return input;
            else if (target == typeof(int))
                return int.Parse(input);
            else if (target == typeof(double))
                return double.Parse(input);
            else
                throw new NotImplementedException($"No conversion for type \"{target.Name}\" on key \"{name}\".");
        }

        protected object CreateInstance(Type type)
            => type.GetConstructor([])?.Invoke([]) 
                   ?? throw new Exception($"No default constructor provided for \"{type.FullName}\"");
    }
}
