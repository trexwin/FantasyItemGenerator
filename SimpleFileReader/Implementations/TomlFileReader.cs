using Microsoft.VisualBasic;
using SimpleFileReader.DataParsers.Implementations;
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
    // Simple limited toml reader implementation
    // Assumes all 'objects' are annotated with [], rather than having inline x.y keys
    // Assumes that any # starts a comment, even within a string
    // Limited support for data types, see the DataParsers

    public class TomlFileReader<T> : BaseFileReader<T> where T : class, new()
    {

        private ObjectDataParser _parser;

        public TomlFileReader()
            => _parser = new ObjectDataParser();

        public override T ReadFile(string path)
        {
            // Retrieve all non-empty lines, remove comments
            string input = RetrieveData(path);
            var inputArr = input.Split(Environment.NewLine)
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
                        currentObject = CreateListItem(currentObject, key);
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
                    if (index < 0)
                        throw new FormatException($"Data malformatted, could not find =");

                    var key = line.Substring(0, index).Trim();
                    var val = line.Substring(index + 1).Trim();

                    if (key == "" || val == "")
                        throw new FormatException($"Data malformatted, must have a key and a value in \"{key} = {val}\"");

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
                // If a list, first get last or new item from it
                if (result is IEnumerable<object>) // Works because of Covariance
                    result = Enumerable.LastOrDefault((IEnumerable<object>)result) ?? CreateListItem(result, s);

                // Retrieve property with name s or create it if needed
                var property = result.GetType().GetProperty(s);
                if (property == null)
                    throw new FormatException($"Data malformatted, could not find property {s} of {key}");
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

        public object CreateListItem(object listObject, string key)
        {
            var typeArgs = listObject.GetType().GetGenericArguments();
            if (typeArgs.Length == 0)
                throw new FormatException($"Data malformatted, {key} is not a generic");

            object result = CreateInstance(typeArgs[0]);

            // For now assume it has an add function
            var method = listObject.GetType().GetMethod("Add");
            if (method == null)
                throw new FormatException($"Data malformatted, {key} does not have an Add function");

            method.Invoke(listObject, [result]);

            return result;
        }

        public object SetFieldValue(object currentObject, string key, string value)
        {
            var objectType = currentObject.GetType();
            var property = objectType.GetProperty(key);
            if (property == null)
                throw new FormatException($"Data malformatted, could not find property {key} for {objectType.FullName}");

            _parser.ParseAndUpdate(currentObject, property, value);

            return currentObject;
        }
    }

}
