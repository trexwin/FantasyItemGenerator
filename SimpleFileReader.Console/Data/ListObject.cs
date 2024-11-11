using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader.Console.Data
{
    internal class ListObject<T>: IPrintable where T : IPrintable
    {
        public string name {  get; set; }
        public List<T> list { get; set; } = new List<T>();

        public string Print()
        {
            var res = $"List {name} with {list.Count} values:\n";
            foreach (var item in list)
                res += item.Print();
            return res;
        }
    }
}
