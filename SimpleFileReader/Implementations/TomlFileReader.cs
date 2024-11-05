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
    public class TomlFileReader<T> : BaseFileReader<T> where T : class, new()
    {
        // Simple limited toml reader implementation
        // Assumes all 'objects' are annotated with [], rather than having inline x.y keys
        // Limited support for data types, see SetFieldValue

        public override T ReadFile(string path)
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

        public T ParseInput(IEnumerable<string> input)
        {
            T result = (T)CreateInstance(typeof(T));
            object currentObject = result;

            foreach (string line in input)
            {
                if (line[0] == '[')
                {
                    if (line[1] == '[')
                    {
                        // Create list and object of requested type
                        string key = line.Substring(2, line.Length - 4);
                        currentObject = CreateNestedObject(result, key);

                        // Current object is a list, create new index
                        currentObject = CreateListObject(currentObject, key);
                    }
                    else
                    {
                        // Create object of requested type
                        string key = line.Substring(1, line.Length - 2);
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

        public object CreateNestedObject(object topObject, string key)
        {
            object result = topObject;
            var nesting = key.Split('.');
            foreach (string s in nesting)
            {
                var property = result.GetType().GetProperty(s);
                if (property == null)
                    throw new KeyNotFoundException($"Data malformatted, could not find property {s} of {key}");
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

        public object CreateListObject(object listObject, string key)
        {
            var typeArgs = listObject.GetType().GetGenericArguments();
            if (typeArgs.Length == 0)
                throw new KeyNotFoundException($"Data malformatted, {key} is not a generic");

            object result = CreateInstance(typeArgs[0]);

            // For now assume it has an add function
            var method = listObject.GetType().GetMethod("Add");
            if (method == null)
                throw new KeyNotFoundException($"Data malformatted, {key} does not have an add function");

            method.Invoke(listObject, [result]);

            return result;
        }

        public object SetFieldValue(object currentObject, string key, string value)
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
