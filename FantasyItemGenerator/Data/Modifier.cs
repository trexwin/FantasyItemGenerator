using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyItemGenerator.Data
{
    internal class Modifier
    {
        public string? Prepend { get; set; }
        public string? Append { get; set; }
        public string[]? Tags { get; set; } // Expand to also parse input strings
        public double Chance { get; set; }

    }
}
