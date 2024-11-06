using System.Reflection;

namespace SimpleFileReader.DataParsers.Generics
{
    public abstract class BaseDataParser<T> : IDataParser<T>
    {
        public abstract T Parse(string data);
        
        object? IDataParser.Parse(string data)
            => Parse(data);

        public virtual void ParseAndUpdate(object parent, PropertyInfo property, string data)
            => property.SetValue(parent, Parse(data));

    }
}
