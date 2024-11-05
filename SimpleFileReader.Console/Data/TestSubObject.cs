using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader.Console.Data
{
    public class TestSubObject
    {
        public TestSubSubObject subsub { get; set; }

        public string Print()
        {
            return $"I'm a Subobject.\n" + subsub.Print();
        }
    }
}
