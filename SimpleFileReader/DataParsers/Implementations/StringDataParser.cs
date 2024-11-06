using SimpleFileReader.DataParsers.Generics;

namespace SimpleFileReader.DataParsers.Implementations
{
    internal class StringDataParser : BaseDataParser<string>
    {
        public override string Parse(string data)
        {
            if (data.First() == '"' && data.Last() == '"')
                return data.Substring(1, data.Length - 2);
            throw new FormatException($"{data} is not a correctly formatted string");
        }
    }
}
