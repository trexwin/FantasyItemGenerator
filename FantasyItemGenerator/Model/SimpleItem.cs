namespace FantasyItemGenerator.Model
{
    internal class SimpleItem
    {
        public string Name { get; set; }

        public SimpleModifier[] Modifiers { get; set; }

        public SimpleItem(string name, SimpleModifier[] modifiers)
        {
            Name = name;
            Modifiers = modifiers;
        }
    }
}
