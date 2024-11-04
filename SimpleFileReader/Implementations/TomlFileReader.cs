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


        private bool _currentIsList = false;
        private string[] _currentSelector = [];

        public T ReadFile(string path)
        {
            // Retrieve all non-empty lines, remove comments
            string input = RetrieveData(path);
            var inputArr = input.Split(Environment.NewLine, StringSplitOptions.None)
                                .Select(s => { var i = s.IndexOf('#'); return i < 0 ? s : s.Substring(0, i); })
                                .Select(s => s.Trim())
                                .Where(s => s.Length > 0);

            // Create base object
            T result = CreateInstance();

            // Go line by line
            foreach (string line in inputArr)
            {
                ReadLine(line, ref result);
            }

            return result;
        }

        private string RetrieveData(string path)
        {

            if (!File.Exists(path))
            {
                return File.ReadAllText(path);

            }
            throw new FileNotFoundException($"Could not retrieve file at {path}");
        }

        private T CreateInstance()
        {
            var constructors = typeof(T).GetConstructor([]);
            if (constructors != null)
                return (T)constructors.Invoke([]);
            throw new Exception("No default constructor provided");
        }

        private void ReadLine(string line, ref T result)
        {

            if (line[0] == '[')
            {
                int index = line.IndexOf(']');
                _currentIsList = line[1] == '[';
                _currentSelector = line.Substring(_currentIsList ? 2 : 1, index).Split('.');
            } 
            else
            {
                int index = line.IndexOf('=');
                var key = line.Substring(0, index).Trim();
                var val = line.Substring(index + 1).Trim();

                object curObject = GetCurrentObject(result);

                var property = curObject.GetType().GetProperty(key);
                if (property == null)
                    throw new KeyNotFoundException($"Data malformatted, could not find property {key}");
                // Cast val to proper type, for now only strings
                property.SetValue(result, ReadVal(key, val));
            }
        }
        private object GetCurrentObject(in T start)
        {
            object result = start;
            foreach(string s in _currentSelector)
            {
                var property= result.GetType().GetProperty(s);
                if (property == null)
                    throw new KeyNotFoundException($"Data malformatted, could not find property {s}");
                var value = property.GetValue(result);
                if (value == null)
                    throw new NullReferenceException("Default constructor does not initialise all objects");
                result = value;
            }
            return result;
        }

        private object ReadVal(string key, string val)
        {
            if (val == "")
                throw new Exception($"Data malformatted, {key} does not have a value");

            object result;
            switch (val[0])
            {
                case '"':
                    result = val.Substring(1, val.Length - 2);
                    break;
                case '[':
                    // Only handle inline string arrays
                    var arr = val.Substring(1, val.Length - 2)
                                 .Split(',')
                                 .Select(s => s.Trim())
                                 .Select(s => s.Substring(1, s.Length - 2))
                                 .ToArray();
                    result = arr ?? [];
                    break;
                default:
                    // Number
                    if(val.Contains('.'))
                    {
                        result = double.Parse(val);
                    }
                    else
                    {
                        result = int.Parse(val);
                    }
                    break;
            }

            return result;
        }
    }

}
