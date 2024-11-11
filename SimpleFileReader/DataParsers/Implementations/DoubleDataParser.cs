using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleFileReader.DataParsers.Generics;

namespace SimpleFileReader.DataParsers.Implementations
{
    public class DoubleDataParser : BaseDataParser<double>
    {
        public override double Parse(string data)
            => double.Parse(data);
    }
}
