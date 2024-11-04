namespace SimpleFileReader.Console.Data
{
    public class TestObject
    {
        public string text { get; set; }
        public int number { get; set; }
        public double decimalnumber { get; set; }

        public TestSubObject subobject { get; set; } = new TestSubObject();
    }
}
