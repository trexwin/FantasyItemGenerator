using System.Diagnostics.CodeAnalysis;

namespace FantasyItemGenerator.Data.Unparsed
{
    internal class Settings
    {
        public string[]? Tags { get; set; }
        public List<Word>? Dictionary { get; set; }
        public List<Item>? Items { get; set; }

        [MemberNotNull([nameof(Tags), nameof(Dictionary), nameof(Items)])]
        internal void ValidateSettings()
        {
            if (Tags == null)
                throw new Exception("No list of tags was specified");

            if (Dictionary == null)
                throw new Exception("No words were specified");
            if (Dictionary.Any(w => w.Name == null))
                throw new Exception("One or more words does not have a name");
            //if (input.Dictionary.Any(w => w.Tags == null))
            //    throw new Exception("One or more words does not have tags");

            if (Items == null)
                throw new Exception("No items were specified");
            if (Items.Any(i => i.Name == null))
                throw new Exception("One or more items do not have a name");
            if (Items.Any(i => i.Modifiers == null))
                throw new Exception("One or more items does not have any modifiers");
        }
    }
}
