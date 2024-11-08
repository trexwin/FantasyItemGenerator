using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyItemGenerator.Parser
{
    internal interface IParser<T, U>
    {
        public U Parse(T input);
    }
}
