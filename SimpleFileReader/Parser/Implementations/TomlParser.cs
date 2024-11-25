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
                if (ch == '#')
                {
                    file = ConsumeComment(file);
                } 
                else if (char.IsWhiteSpace(ch))
                {
                    file = ConsumeWhitespace(file);
                }
                else if (ch == '"' || char.IsLetter(ch))
                {
                    var key = ReadKey(ref file);

                    file = ConsumeWhitespace(file);
                    
                    if(file.Length == 0 || file[0] != '=')
                        throw new Exception($"Key value pair not properly specified at line {_lines - file.Count('\n')}.");
                    else 
                        file = file.Slice(1);
                    
                    file = ConsumeWhitespace(file);

                    var val = ReadValue(ref file);

                    result.TryAdd(key, val);
                }
                else if(ch == '[')
                {
                    // Create new object
                    throw new NotImplementedException();
                }
                else
                {
                    throw new Exception($"Could not read line {_lines - file.Count('\n')}.");
                }

            }

            return result;
        }

        /// <summary>
        /// Reads the provided file as if it starts with a key
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected string ReadKey(ref ReadOnlySpan<char> file)
        {
            if (file.Length == 0)
                return string.Empty;

            // "..." = ...
            if (file[0] == '"') 
            {
                // Consume till second '"'
                file = file.Slice(1);
                var i = file.IndexOf('"');
                var key = file.Slice(0, i).ToString();
                file = file.Slice(i);
                return key;
            }
            // ... = ...
            else
            {
                // Consume till whitespace
                var i = Math.Min(file.IndexOf(' '), file.IndexOf('\t'));
                var key = file.Slice(0, i).ToString();
                file = file.Slice(i);
                return key;
            }
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

            // Array
            if (file[0] == '[')
            {
                // Consume till second '"'
                file = file.Slice(1);
                var i = file.IndexOf(']');
                var val = file.Slice(0, i).ToString();
                file = file.Slice(i);
                return val;
            }
            // Any other value
            else
            {
                // Consume till newline
                var i = file.IndexOf('\n');
                var val = file.Slice(0, i).ToString();
                file = file.Slice(i);
                return val;
            }
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
    }
}
