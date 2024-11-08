using FantasyItemGenerator.Data.Parsed;
using FantasyItemGenerator.Data.Unparsed;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyItemGenerator.Parser
{
    internal class SettingsParser : IParser<Settings, List<ParsedItem>>
    {
        public List<ParsedItem> Parse(Settings input)
        {
            // Validate settings object is properly setup
            input.ValidateSettings();

            // Start parsing each item into a ParsedItem
            var res = new List<ParsedItem>();
            foreach (var item in input.Items)
            {
                string name = item.Name ?? string.Empty;
                if (item.Modifiers != null && item.Modifiers.Count > 0)
                {
                    var parsedModifiers = new ParsedModifier[item.Modifiers.Count];
                    for (int i = 0; i < parsedModifiers.Length; i++)
                    {
                        var modifier = item.Modifiers[i];
                        var prepend = modifier.Prepend?.Split('@') ?? [];
                        var append = modifier.Append?.Split('@') ?? [];

                        int numWords = modifier.Prepend?.Count(c => c == '@') ?? 0 +
                                       modifier.Append?.Count(c => c == '@') ?? 0;

                        // Compare numWords with actual input tags
                        // For all input  tags:
                        // - Parse input string into a delegate to check if word matches
                        // - Fill in words that match delegate (predicate really)

                        var words = new string[numWords][];

                        parsedModifiers[i] = new ParsedModifier(modifier.Chance, prepend, append, words);
                    }
                    res.Add(new ParsedItem(name, parsedModifiers));
                }
                else
                {
                    res.Add(new ParsedItem(name, []));
                }
            }
            return res;
        }
    }
}
