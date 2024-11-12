namespace FantasyItemGenerator.Model
{
    internal class SimpleModifier
    {
        public double Chance { get; set; }
        public string[] Prepend { get; set; }
        public string[] Append { get; set; }
        public string[][] Words { get; set; }

        public SimpleModifier(double chance, string[] prepend, string[] append, string[][] words)
        {
            Chance = chance;
            Prepend = prepend;
            Append = append;
            Words = words;
        }
    }
}
