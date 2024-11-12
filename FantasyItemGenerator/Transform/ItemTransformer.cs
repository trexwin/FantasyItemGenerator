﻿using FantasyItemGenerator.Model;
using FantasyItemGenerator.Model.Input;

namespace FantasyItemGenerator.Transform
{
    internal class ItemTransformer : ITransformer<Item, SimpleItem>
    {
        private ModifierTransformer _modifierTransformer;

        public ItemTransformer(List<Word> dictionary)
            => _modifierTransformer = new ModifierTransformer(dictionary);

        public SimpleItem Transform(Item input)
        {
            string name = input.Name ?? string.Empty;
            var parsedModifiers = input.Modifiers?.Select(_modifierTransformer.Transform).ToArray() ?? [];
            return new SimpleItem(name, parsedModifiers);
        }
    }
}
