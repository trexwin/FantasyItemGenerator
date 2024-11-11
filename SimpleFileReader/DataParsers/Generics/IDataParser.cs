using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader.DataParsers.Generics
{
    public interface IDataParser<T> : IDataParser
    {
        public new T Parse(string data);
    }
}
