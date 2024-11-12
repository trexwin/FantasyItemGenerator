using FantasyItemGenerator.Model;
using FantasyItemGenerator.Model.Input;

namespace FantasyItemGenerator.Transform
{
    internal class ModifierTransformer : ITransformer<Modifier, SimpleModifier>
    {
        private List<Word> _dictionary;

        public ModifierTransformer(List<Word> dictionary) 
            => _dictionary = dictionary;

        public SimpleModifier Transform(Modifier input)
        {
            // ToDo: Clean up a bit
            var prepend = input.Prepend?.Split('@') ?? [];
            var append  = input.Append?.Split('@') ?? [];

            int numWords = input.Prepend?.Count(c => c == '@') ?? 0 +
                           input.Append?.Count(c => c == '@') ?? 0;

            if ((input.Tags == null && numWords > 0) ||
                (input.Tags != null && numWords != input.Tags.Length))
            {
                throw new Exception("Amount of tags for modifier does not match amount of required tags");
            }
            else
            {
                var words = new string[numWords][];
                if (input.Tags != null)
                {
                    for (int i = 0; i < numWords; i++)
                    {
                        var tagReqs = input.Tags[i].Split('|').Select(t => t.Split('&'));
                        var matchedWords =
                            _dictionary.Where(
                                w => tagReqs.Any(
                                    tags => !tags.Except(w.Tags ?? []).Any()
                                )
                            ).Select(w => w.Name ?? string.Empty).ToArray();
                        words[i] = matchedWords;
                    }

                }
                return new SimpleModifier(input.Chance, prepend, append, words);
            }
        }
    }
}
