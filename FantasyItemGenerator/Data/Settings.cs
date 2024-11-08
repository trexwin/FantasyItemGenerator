using System.Diagnostics.CodeAnalysis;

namespace FantasyItemGenerator.Data
{
    public class Settings : IInitialsedCheck
    {
        public string[]? Tags { get; set; }
        public List<Word>? Dictionary { get; set; }
        public List<Item>? Items { get; set; }

        [MemberNotNullWhen(true, ["Tags", "Dictionary", "Items"])]
        public bool TagsValid()
        {
            if (IsInitialised())
            {
                var dictionaryOk
                    = Dictionary.All(w => w.IsInitialised() && 
                                          w.Tags.All(t => Tags.Contains(t)));
                var itemsOk
                    = Items.All(i => i.IsInitialised() && 
                                     i.Modifiers.All(m => m.IsInitialised() && 
                                                          m.Tags.All(t => Tags.Contains(t))));

                return dictionaryOk && itemsOk;
            }
            return false;
        }


        [MemberNotNullWhen(true, ["Tags", "Dictionary", "Items"])]
        public bool IsInitialised()
            => Tags != null && Dictionary != null && Items != null;
    }
}
