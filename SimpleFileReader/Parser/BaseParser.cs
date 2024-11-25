
namespace SimpleFileReader.Parser
{
    public abstract class BaseParser : IPraser
    {
        public string Load(string filepath)
        {
            if (File.Exists(filepath))
                return File.ReadAllText(filepath);
            throw new FileNotFoundException($"Could not retrieve file at \"{filepath}\"");
        }

        public abstract dynamic Parse(string filecontent);

        /// <summary>
        /// Load in the given file and immediatly attempts to parse it.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public dynamic LoadAndParse(string filepath)
            => Parse(Load(filepath));
    }
}
