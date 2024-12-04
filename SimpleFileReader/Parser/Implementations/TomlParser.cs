using SimpleFileReader.DataParsers.Implementations;
using SimpleFileReader.Helper;
using System.Dynamic;

namespace SimpleFileReader.Parser.Implementations
{
    [SupportedExtensions([".toml", ".tml"])]
    public class TomlParser : BaseParser
    {
        private int _lines = 0;

        public override Dictionary<string, object> Parse(string filecontent)
        {
            ReadOnlySpan<char> file = filecontent.AsSpan();
            _lines = file.Count('\n') + 1;
            
            return ReadObject([], ref file);
        }

        /// <summary>
        /// Functaionally reads the given input file into a dictionary
        /// </summary>
        /// <param name="file"></param>
        protected Dictionary<string, object> ReadObject(string[] level, ref ReadOnlySpan<char> file)
        {
            var res = new Dictionary<string, object>();
            while (file.Length > 0)
            {
                char ch = file[0];
                // Field
                if (ch == '"' || char.IsLetter(ch))
                {
                    var (keys, val) = ReadKeysValuePair(ref file);
                    var tmp = res;
                    for(int i = 0; i < keys.Length - 1; i++)
                    {
                        object? nestedObject;
                        if(tmp.TryGetValue(keys[i], out nestedObject) && nestedObject is Dictionary<string, object>)
                        {
                            tmp = (Dictionary<string, object>)nestedObject;
                        }
                        else
                        {
                            nestedObject = new Dictionary<string, object>();
                            tmp.Add(keys[i], nestedObject);
                            tmp = (Dictionary<string, object>)nestedObject;
                        }
                    }
                    tmp.TryAdd(keys.Last(), val);
                }
                // List or Object
                else if(ch == '[')
                {
                    var oldFile = file;
                    if (file.Length > 1 && file[1] == '[')
                    {
                        var name = ReadListKeys(ref file);
                        if (!name.SequenceEqual(level) && name.StartsWith(level))
                        {
                            object? listObject;
                            var newObject = ReadObject(name, ref file);
                            if (res.TryGetValue(name.Last(), out listObject) && listObject is List<object>)
                            {
                                List<object> list = (List<object>)listObject;
                                list.Add(newObject);
                            }
                            else
                            {
                                listObject = new List<object>() { newObject };
                                res.TryAdd(name.Last(), listObject);
                            }
                            continue;
                        }
                    }
                    else
                    {
                        var name = ReadObjectKeys(ref file);
                        if (name.StartsWith(level))
                        {
                            // Currently overwrite objects with same name
                            var newObject = ReadObject(name, ref file);
                            res.TryAdd(name.Last(), newObject);
                            continue;
                        }
                    }
                    // Unconsume key
                    file = oldFile;
                    return res;
                }
                // Comment
                else if (ch == '#')
                    file = ConsumeComment(file);
                // Whitespace
                else if (char.IsWhiteSpace(ch))
                    file = ConsumeSpaces(file);
                // Unknown
                else
                    throw new Exception($"Could not read line {_lines - file.Count('\n')}.");

                // Read new line if not end of document
                if(file.Length == 0 || (file.Length > 0 && file[0] == '\n') || (file.Length >= 1 && file[0] == '\r' && file[1] == '\n'))
                    file = ConsumeWhitespace(file);
                else
                    throw new Exception($"Improper ending of line {_lines - file.Count('\n')}.");
            }
            return res;
        }


        protected (string[], string) ReadKeysValuePair(ref ReadOnlySpan<char> file)
        {
            // ... = ...
            var keys = ReadKeys(ref file);
            file = ConsumeSpaces(file);
            if (file.Length == 0 || file[0] != '=')
                throw new Exception($"Key value pair not properly specified at line {_lines - file.Count('\n')}.");
            else
                file = file.Slice(1);

            file = ConsumeSpaces(file);
            var val = ReadValue(ref file);
            file = ConsumeSpaces(file);

            return (keys, val);
        }

        /// <summary>
        /// Attempts to read the start of the provided file as keys for a list.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected string[] ReadListKeys(ref ReadOnlySpan<char> file)
        {
            if (file.Length < 2 || file[0] != '[' || file[1] != '[')
                throw new Exception($"Invalid list selection on line {_lines - file.Count('\n')}");
            file = file.Slice(2);
            var name = ReadKeys(ref file);
            if (file.Length < 2 || file[0] != ']' || file[1] != ']')
                throw new Exception($"Invalid list selection on line {_lines - file.Count('\n')}");
            file = file.Slice(2);

            return name;
        }


        /// <summary>
        /// Attempts to read the start of the provided file as keys for an object.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected string[] ReadObjectKeys(ref ReadOnlySpan<char> file)
        {
            if (file.Length == 0 || file[0] != '[')
                throw new Exception($"Invalid object selection on line {_lines - file.Count('\n')}");
            file = file.Slice(1);
            var name = ReadKeys(ref file);
            if (file.Length == 0 || file[0] != ']')
                throw new Exception($"Invalid object selection on line {_lines - file.Count('\n')}");
            file = file.Slice(1);

            return name;
        }


        /// <summary>
        /// Reads the provided file as if it starts with a one or more keys.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected string[] ReadKeys(ref ReadOnlySpan<char> file)
        {
            List<string> keys = new List<string>() { ReadKey(ref file) };
            while (file[0] == '.')
            {
                file = file.Slice(1);
                keys.Add(ReadKey(ref file));
            }
            return keys.ToArray();
        }

        /// <summary>
        /// Reads the provided file as if it starts with a single key.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected string ReadKey(ref ReadOnlySpan<char> file)
        {
            if (file.Length == 0)
                return string.Empty;

            // "..." = ...
            if (file[0] == '"') 
                return ReadString(ref file);
            // ... = ...
            // Consume till whitespace, =, ] or .
            var i = file.IndexOfAny([' ', '\t', '=', ']', '.']);
            var key = file.Slice(0, i).Trim().ToString();
            if (key.Contains('\n'))
                throw new Exception($"Key contains new line at line {_lines - file.Count('\n')}.");
            file = file.Slice(i);
            return key;
        }

        /// <summary>
        /// Reads the provided file as if it starts with a key
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected string ReadValue(ref ReadOnlySpan<char> file)
        {
            if (file.Length == 0)
                return string.Empty;

            string res;
            if (file[0] == '[')
            {
                // Consume till second ']'
                file = file.Slice(1);
                res = "[";
                while (file[0] != ']')
                {
                    file = ConsumeWhitespace(file);
                    res += ReadValue(ref file);
                    file = ConsumeWhitespace(file);
                    if (file[0] == ',')
                    {
                        file = file.Slice(1);
                        res += ",";
                    }
                }
                file = file.Slice(1);
                res += "]";
                Console.WriteLine(res);
            }
            else if (file[0] == '"')
            {
                res = '"' + ReadString(ref file) + '"';
            }
            else
            {
                // Read till next non letter or digit
                for(res = string.Empty; 
                    file.Length > 0 && (char.IsLetterOrDigit(file[0]) || file[0] == '.'); 
                    file = file.Slice(1))
                {
                    res += file[0];
                }
            }

            return res;
        }

        /// <summary>
        /// Reads a string between two quotation marks.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>The read string or an empty string if no opening quotation mark was found.</returns>
        /// <exception cref="Exception"></exception>
        protected string ReadString(ref ReadOnlySpan<char> file)
        {
            if (file[0] == '"')
            {
                var tmpFile = file.Slice(1);
                while (tmpFile.Length > 0)
                {
                    if (tmpFile[0] == '"')
                    {
                        // Extract string
                        var val = file.Slice(1, file.Length - tmpFile.Length - 1).ToString();
                        file = tmpFile.Slice(1);
                        return val;
                    }
                    else if (tmpFile[0] == '\n')
                        break;
                    else if (tmpFile[0] == '\\' && tmpFile.Length >= 2)
                        tmpFile = tmpFile.Slice(2);
                    else
                        tmpFile = tmpFile.Slice(1);
                }
                throw new Exception($"Could not read string at line {_lines - file.Count('\n')}.");
            }
            return string.Empty;
        }

        /// <summary>
        /// Consume characters till we encounter a new line
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected ReadOnlySpan<char> ConsumeComment(ReadOnlySpan<char> file)
        {
            var i = file.IndexOf('\n');
            return i == -1 ? ReadOnlySpan<char>.Empty : file.Slice(i);
        }

        /// <summary>
        /// Consume whitespace characters from the start
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected ReadOnlySpan<char> ConsumeWhitespace(ReadOnlySpan<char> file)
            => file.TrimStart();

        /// <summary>
        /// Similair to ConsumeWhitespace, but does not consume new lines
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected ReadOnlySpan<char> ConsumeSpaces(ReadOnlySpan<char> file)
        {
            while (file.Length > 0 && (file[0] == ' ' || file[0] == '\t'))
                file = file.Slice(1);
            return file;
        }
    }
}
