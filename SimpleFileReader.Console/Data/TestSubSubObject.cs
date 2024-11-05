
namespace SimpleFileReader.Console.Data
{
    public class TestSubSubObject
    {

        public string name { get; set; }
        public string[] tags { get; set; }

        public string Print()
        {
            string result = $"My name is {name} and I have the tags {string.Join(',', tags)}\n";
            return result;
        }
    }
}