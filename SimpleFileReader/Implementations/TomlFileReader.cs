using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader.Implementations
{
    public class TomlFileReader<T> : IFileReader<T> where T : class, new()
    {
        // Simple naive implementation
        // Assumes all 'objects' are annotated with [], rather than having inline x.y keys
        // Only support inline string arrays with the ["x", "y", "z"] notation
        public T ReadFile(string path)
        {
            // Retrieve all non-empty lines, remove comments
            string input = RetrieveData(path);
            var inputArr = input.Split(Environment.NewLine, StringSplitOptions.None)
                                .Select(s => { var i = s.IndexOf('#'); return i < 0 ? s : s.Substring(0, i); })
                                .Select(s => s.Trim())
                                .Where(s => s.Length > 0);

            T result = ParseInput(inputArr);

            return result;

        }

        private string RetrieveData(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            throw new FileNotFoundException($"Could not retrieve file at {path}");
        }

        private object CreateInstance(Type type)
        {
            var constructors = type.GetConstructor([]);
            if (constructors != null)
                return constructors.Invoke([]);
            throw new Exception($"No default constructor provided for {type.FullName}");
        }

        private T ParseInput(IEnumerable<string> input)
        {
            T result = (T)CreateInstance(typeof(T));
            object currentObject = result;

            foreach (string line in input)
            {
                if (line[0] == '[')
                {
                    int index = line.IndexOf(']');
                    if (line[1] == '[')
                    {
                        // Create array
                        throw new NotImplementedException();
                    }
                    else
                    {
                        // Create object of requested type
                        string key = line.Substring(1, index - 1);
                        currentObject = CreateNestedObject(result, key);
                    }
                }
                else
                {
                    // Field value
                    int index = line.IndexOf('=');
                    var key = line.Substring(0, index).Trim();
                    var val = line.Substring(index + 1).Trim();

                    if (key == "" || val == "")
                        throw new Exception($"Data malformatted, must have a key and a value in \"{key} = {val}\"");

                    currentObject = SetFieldValue(currentObject, key, val);
                }
            }
            return result;
        }

        private object CreateNestedObject(object topObject, string key)
        {
            object result = topObject;
            var nesting = key.Split('.');
            foreach (string s in nesting)
            {
                var property = result.GetType().GetProperty(s);
                if (property == null)
                    throw new KeyNotFoundException($"Data malformatted, could not find property {s}");
                var value = property.GetValue(result);
                if (value == null)
                {
                    value = CreateInstance(property.PropertyType);
                    property.SetValue(result, value);
                }
                result = value;
            }
            return result;
        }

        private object SetFieldValue(object currentObject, string key, string value)
        {
            var objectType = currentObject.GetType();
            var property = objectType.GetProperty(key);
            if (property == null)
                throw new KeyNotFoundException($"Data malformatted, could not find property {key} for {objectType.FullName}");

            if (property.PropertyType == typeof(string))
            {
                if (value.First () == '"' && value.Last() == '"')
                { property.SetValue(currentObject, value.Substring(1, value.Length - 2)); }
                else
                { throw new Exception($"Data malformatted, {key} requires a string"); }
            }
            else if (property.PropertyType == typeof(int))
            {
                try { property.SetValue(currentObject, int.Parse(value)); }
                catch { throw new Exception($"Data malformatted, {key} requires an int"); }

            }
            else if (property.PropertyType == typeof(double))
            {
                try { property.SetValue(currentObject, double.Parse(value)); }
                catch { throw new Exception($"Data malformatted, {key} requires a double"); }
            }
            else if (property.PropertyType == typeof(string[]))
            {
                if (value.First() == '[' && value.Last() == ']')
                {
                    var values = value.Substring(1, value.Length - 2)
                                      .Split(',')
                                      .Select(s => s.Trim());

                    if (values.All(s => s.First() == '"' && s.Last() == '"'))
                    { 
                        property.SetValue(currentObject, 
                                          values.Select(s => s.Substring(1, s.Length - 2)).ToArray()); 
                    }
                    else
                    { throw new Exception($"Data malformatted, {key} requires a string[]"); }
                }
                else
                { throw new Exception($"Data malformatted, {key} requires a string[]"); }
            }

            return currentObject;
        }
    }

}
