namespace FantasyItemGenerator.Model.Input
{
    internal class Modifier
    {
        public string? Prepend { get; set; }
        public string? Append { get; set; }
        public string[]? Tags { get; set; }
        public double Chance { get; set; }
    }
}
