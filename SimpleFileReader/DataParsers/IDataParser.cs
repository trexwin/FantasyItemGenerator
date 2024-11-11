using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader.DataParsers
{
    public interface IDataParser
    {
        public object? Parse(string data);

        public void ParseAndUpdate(object parent, PropertyInfo property, string data);
    }
}
