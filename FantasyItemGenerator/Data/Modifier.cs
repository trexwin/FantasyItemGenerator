using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyItemGenerator.Data
{
    public class Modifier : IInitialsedCheck
    {
        public string? Prepend { get; set; }
        public string? Append { get; set; }
        public string[]? Tags { get; set; } // Expand to also parse input strings
        public double Chance { get; set; }

        [MemberNotNullWhen(true, "Tags")]
        public bool IsInitialised()
            => (Prepend != null || Append != null) && 
               Tags != null && Chance > 0;
    }
}
