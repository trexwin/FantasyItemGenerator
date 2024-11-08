using FantasyItemGenerator.Data;
using SimpleFileReader;
using SimpleFileReader.Implementations;
using System.Collections.Generic;

namespace FantasyItemGenerator
{
    public class ItemGenerator
    {
        private static Random Random = new Random();

        private FileReaderFactory _readerFactory;

        protected Settings? Settings { get; set; }

        public ItemGenerator()
            => _readerFactory = new FileReaderFactory();

        public string[] GenerateItem(int amount)
        {
            if (amount < 1 || Settings == null || !Settings.IsInitialised())
                return [];

            var res = new string[amount];
            for (int i = 0; i < amount; i++)
            {
                // Retrieve random item
                int rand = Random.Next(Settings.Items.Count);
                var item = Settings.Items[rand];
                if (item.IsInitialised())
                {
                    res[i] = item.Name;
                    // Check prepend and append, count @'s and use tags to find suitable materials
                }
                else
                { throw new Exception("GenerateItem called when an item was not initialised"); }
            }

            return res;
        }

        public void ReadSettings(string path)
        {
            Settings = null;
            var extension = Path.GetExtension(path);
            var reader = _readerFactory.GetFileReader<Settings>(extension);
            var settings = reader.ReadFile(path);
            if (settings.TagsValid())
                throw new Exception("Tags used that are not previously specified");
            Settings = settings;
        }

    }
}
