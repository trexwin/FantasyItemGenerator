using FantasyItemGenerator.Model.Input;

namespace FantasyItemGenerator.Transform
{
    internal class TagTransformer : ITransformer<string, Predicate<Word>>
    {
        public Predicate<Word> Transform(string input)
        {
            var requirements = input.Split('|').Select(t => t.Split('&'));
            // If any of the tag arrays are empty after except, an array is a subset of word.Tags
            return (word) => requirements.Any(tags => !tags.Except(word.Tags ?? []).Any());
        }
    }
}
