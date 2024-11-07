using FantasyItemGenerator.Data;
using SimpleFileReader;
using SimpleFileReader.Implementations;

namespace FantasyItemGenerator
{
    public class ItemGenerator
    {
        private FileReaderFactory _readerFactory;

        protected Settings? Settings { get; set; }

        public ItemGenerator()
            => _readerFactory = new FileReaderFactory();

        public string? GenerateItem(string path)
        {
            if (Settings == null)
                return null;
            return string.Empty;
        }

        private void ReadSettings(string path)
        {
            var extension = Path.GetExtension(path);
            var reader = _readerFactory.GetFileReader<Settings>(extension);
            Settings = reader.ReadFile(path);
        }


    }
}
