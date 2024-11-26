using System.Dynamic;

namespace SimpleFileReader.Parser.Implementations
{
    [SupportedExtensions([".toml", ".tml"])]
    public class TomlParser : BaseParser
    {
        private int _lines = 0;

        public override dynamic Parse(string filecontent)
        {
            var res = new ExpandoObject();
            ReadOnlySpan<char> file = filecontent.AsSpan();
            _lines = file.Count('\n');
            
            return ReadFile(res, file);
        }

        /// <summary>
        /// Functaionally reads the given input file character by character into an ExpandoObject
        /// </summary>
        /// <param name="result"></param>
        /// <param name="file"></param>
        protected dynamic ReadFile(ExpandoObject result, ReadOnlySpan<char> file)
        {
            while (file.Length > 0)
            {
                char ch = file[0];
                if (ch == '"' || char.IsLetter(ch))
                {
                    var (key, val) = ReadKeyValuePair(ref file);
                    result.TryAdd(key, val);
                }
                else if (ch == '[')
                {
                    if (file.Length >= 2 && file[1] == '[')
                    { /* in list */  }
                    else
                    {
                        var name = ReadKey(ref file);
                        if (file.Length == 0 || file[0] != ']')
                            throw new Exception($"Invalid object selection on line {_lines - file.Count('\n')}");

                        var newObject = new ExpandoObject();
                        result.TryAdd(name, newObject);
                        // Add newObject to some sort of stack
                    }
                    throw new NotImplementedException();
                }
                else if (ch == '#')
                {
                    file = ConsumeComment(file);
                } 
                else if (char.IsWhiteSpace(ch))
                {
                    file = ConsumeSpaces(file);
                }
                else
                {
                    throw new Exception($"Could not read line {_lines - file.Count('\n')}.");
                }

                if (file.Length > 0 && file[0] != '\n')
                    throw new Exception($"Improper ending of line {_lines - file.Count('\n')}.");
                file = ConsumeWhitespace(file);
            }
            return result;
        }


        protected (string, string) ReadKeyValuePair(ref ReadOnlySpan<char> file)
        {
            // ... = ...
            var key = ReadKey(ref file);
            file = ConsumeSpaces(file);
            if (file.Length == 0 || file[0] != '=')
                throw new Exception($"Key value pair not properly specified at line {_lines - file.Count('\n')}.");
            else
                file = file.Slice(1);

            file = ConsumeSpaces(file);
            var val = ReadValue(ref file);
            file = ConsumeSpaces(file);

            return (key, val);
        }

        /// <summary>
        /// Reads the provided file as if it starts with a key.
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
            // Consume till whitespace, = or ]
            var i = file.IndexOfAny([' ', '\t', '=', ']']);
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
            }
            else if (file[0] == '"')
            {
                res = '"' + ReadString(ref file) + '"';
            }
            else
            {
                // Read till next whitespace
                for(res = string.Empty; !char.IsWhiteSpace(file[0]); file = file.Slice(1))
                    res += file[0];
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
                        var val = file.Slice(0, file.Length - tmpFile.Length).ToString();
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
