using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FantasyItemGenerator.Data
{
    public class Item : IInitialsedCheck
    {
        public string? Name { get; set; }
        public List<Modifier>? Modifiers { get; set; }

        [MemberNotNullWhen(true, ["Name", "Modifiers"])]
        public bool IsInitialised()
            =>  Name != null && Modifiers != null;

        }
}