using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader.Console.Data
{
    internal class TestObject : IPrintable
    {
        public BasicObject basic { get; set; }
        public ListObject<BasicObject> basiclist { get; set; }
        public ListObject<ListObject<BasicObject>> nestedlist { get; set; }

        public string Print()
        {
            var res = "Test object:\n";
            res += basic.Print();
            res += basiclist.Print();
            res += nestedlist.Print();
            return res;
        }
    }
}
