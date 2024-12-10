using System.Diagnostics.CodeAnalysis;

namespace FantasyItemGenerator.Model.Input
{
    internal class Settings
    {
        public string[] Tags { get; set; } = [];
        public Word[] Dictionary { get; set; } = [];
        public Item[] Items { get; set; } = [];
    }
}
