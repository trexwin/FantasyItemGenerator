using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyItemGenerator.Data
{
    public class Word : IInitialsedCheck
    {
        public string? Name { get; set; }
        public string[]? Tags { get; set; }

        [MemberNotNullWhen(true, ["Name", "Tags"])]
        public bool IsInitialised()
            => Name != null && Tags != null;
    }
}
