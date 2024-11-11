using SimpleFileReader.DataParsers.Generics;

namespace SimpleFileReader.DataParsers.Implementations
{
    public class StringArrDataParser : BaseDataParser<string[]>
    {
        public override string[] Parse(string data)
        {
            if (data.Length >= 2 && data.First() == '[' && data.Last() == ']')
            {
                var value = data.Substring(1, data.Length - 2).Trim();
                if (value.Length == 0)
                    return [];

                var values = value.Split(',').Select(s => s.Trim());
                if (values.All(s => s.Length >= 2 && s.First() == '"' && s.Last() == '"'))
                    return values.Select(s => s.Substring(1, s.Length - 2)).ToArray();
            }
            throw new FormatException($"{data} is not a correctly formatted string[]");
        }
    }
}
