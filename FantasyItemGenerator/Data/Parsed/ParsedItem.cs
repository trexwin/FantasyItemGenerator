
using FantasyItemGenerator.Data.Unparsed;

namespace FantasyItemGenerator.Data.Parsed
{
    internal class ParsedItem
    {
        public string Name { get; set; }

        public ParsedModifier[] Modifiers { get; set; }

        public ParsedItem(string name, ParsedModifier[] modifiers)
        {
            Name = name;
            Modifiers = modifiers;
        }
    }
}
