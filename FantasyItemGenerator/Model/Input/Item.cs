namespace FantasyItemGenerator.Model.Input
{
    internal class Item
    {
        public string Name { get; set; } = string.Empty;
        public Modifier[] Modifiers { get; set; } = [];
    }
}