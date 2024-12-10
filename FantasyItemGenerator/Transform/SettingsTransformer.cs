using FantasyItemGenerator.Model;
using FantasyItemGenerator.Model.Input;

namespace FantasyItemGenerator.Transform
{
    internal class SettingsTransformer : ITransformer<Settings, List<SimpleItem>>
    {
        public List<SimpleItem> Transform(Settings input)
        {
            var itemTransformer = new ItemTransformer(input.Dictionary);
            return input.Items.Select(itemTransformer.Transform).ToList();
        }
    }
}
