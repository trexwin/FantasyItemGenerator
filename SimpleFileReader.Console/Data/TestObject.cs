namespace SimpleFileReader.Console.Data
{
    public class TestObject
    {
        public string text { get; set; }
        public int number { get; set; }
        public double decimalnumber { get; set; }

        public TestSubObject subobject { get; set; }

        public List<TestSubSubObject> subsubobjects { get; set; }

        public string Print()
        {
            string result = $"My text is \"{text}\".\n";
            result += $"My number is {number}.\n";
            result += $"My decimal is {decimalnumber}.\n";
            result += $"I have a SubObject.\n" + subobject.Print();
            result += $"I have alist of subsubobjects.\n";
            foreach (var s in subsubobjects)
            {
                result += s.Print();
            }

            return result;
        }
    }
}
