﻿
namespace SimpleFileReader.Parser
{
    public interface IParser
    {
        /// <summary>
        /// Retrieve the contents of the given path as a string.
        /// </summary>
        /// <param name="filepath">Path to file where contents is read.</param>
        /// <returns></returns>
        public string Load(string filepath);

        /// <summary>
        /// Parses the given file content into a dynamic object.
        /// </summary>
        /// <param name="filecontent"></param>
        /// <returns></returns>
        public Dictionary<string, object> Parse(string filecontent);


        /// <summary>
        /// Load in the given file and immediatly attempts to parse it.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public Dictionary<string, object> LoadAndParse(string filepath);
    }
}
