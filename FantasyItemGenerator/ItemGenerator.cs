using FantasyItemGenerator.Data.Parsed;
using FantasyItemGenerator.Data.Unparsed;
using FantasyItemGenerator.Parser;
using SimpleFileReader;

namespace FantasyItemGenerator
{
    public class ItemGenerator
    {
        private List<ParsedItem>? _items;
        private FileReaderFactory _readerFactory;
        private SettingsParser _settingsParser;

        public ItemGenerator()
        {
            _readerFactory = new FileReaderFactory();
            _settingsParser = new SettingsParser();
        }

        public string[] GenerateItem(int amount)
        {
            if (_items == null)
                return [];

            var result = new string[amount];
            for (int i = 0; i < amount; i++)
            {
                // Select a random item
                // Iterate through modifiers
                // Store final result
                result[i] = String.Empty;
            }

            return result;
        }

        public void ReadSettings(string path)
        {
            // Discard old settings
            _items = null;

            // Load new settings
            var extension = Path.GetExtension(path);
            var reader = _readerFactory.GetFileReader<Settings>(extension);
            var settings = reader.ReadFile(path);

            // Parse new settings
            _items = _settingsParser.Parse(settings);
        }

    }
}
