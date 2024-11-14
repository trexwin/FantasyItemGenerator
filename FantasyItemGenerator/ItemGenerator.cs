using FantasyItemGenerator.Model;
using FantasyItemGenerator.Model.Input;
using FantasyItemGenerator.Transform;
using SimpleFileReader;
using System;

namespace FantasyItemGenerator
{
    public class ItemGenerator
    {
        private Random _random;
        private List<SimpleItem>? _items;
        private FileReaderFactory _readerFactory;
        private SettingsTransformer _settingsTransformer;

        public ItemGenerator()
        {
            _random = new Random();
            _readerFactory = new FileReaderFactory();
            _settingsTransformer = new SettingsTransformer();
        }

        public string[] GetExampleFiles()
            => Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, @"Examples"), "*.*", SearchOption.TopDirectoryOnly);

        public string[]? GenerateItem(int amount)
        {
            if (_items == null)
                return null;

            var result = new string[amount];
            for (int i = 0; i < amount; i++)
            {
                var item = _items[_random.Next(_items.Count)];
                var res = item.Name;
                foreach (var modifier in item.Modifiers)
                {
                    var val = _random.NextDouble();
                    if (val < modifier.Chance)
                    {
                        // Retrieve all words
                        var words = modifier.Words.Select(w => w[_random.Next(w.Length)]);

                        // interweave lists and result
                        if (modifier.Prepend.Any())
                        {
                            var preWords = words.Take(modifier.Prepend.Length);
                            var pre = modifier.Prepend.Zip(preWords, (f, s) => f + s).Append(modifier.Prepend.Last());
                            res = string.Join(null, pre) + res;
                        }
                        if (modifier.Append.Any())
                        {
                            var appWords = words.Take(modifier.Append.Length);
                            var app = modifier.Append.Zip(appWords, (f, s) => f + s).Append(modifier.Append.Last());
                            res += string.Join(null, app);
                        }
                    }
                }
                result[i] = res;
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
            _items = _settingsTransformer.Transform(settings);
        }

    }
}
