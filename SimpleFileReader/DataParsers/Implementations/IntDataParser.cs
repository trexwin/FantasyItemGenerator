using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleFileReader.DataParsers.Generics;

namespace SimpleFileReader.DataParsers.Implementations
{
    public class IntDataParser : BaseDataParser<int>
    {
        public override int Parse(string data)
            => int.Parse(data);
    }
}
