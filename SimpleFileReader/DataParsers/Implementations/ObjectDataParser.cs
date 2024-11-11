using System.Reflection;
using SimpleFileReader.DataParsers.Generics;

namespace SimpleFileReader.DataParsers.Implementations
{
    public class ObjectDataParser : BaseDataParser<object>
    {
        private Dictionary<Type, IDataParser> _parsers;
        public ObjectDataParser() 
        {
            _parsers = new Dictionary<Type, IDataParser>()
            {
                {typeof(int), new IntDataParser() },
                {typeof(double), new DoubleDataParser() },
                {typeof(string), new StringDataParser() },
                {typeof(string[]), new StringArrDataParser() }
            };
        }

        public override object Parse(string data)
            => throw new NotImplementedException("ObjectDataParser can not direclty parse");

        public override void ParseAndUpdate(object parent, PropertyInfo property, string data)
            => _parsers[property.PropertyType].ParseAndUpdate(parent, property, data);
    }
}
