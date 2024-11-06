using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader.Console.Data
{
    internal class BasicObject : IPrintable
    {
        public string text { get; set; }
        public string[] texts { get; set; }
        public int number { get; set; }
        public double decnumber { get; set; }


        public string Print()
            => $"Basic=text:\"{text}\",texts:[\"{String.Join("\", \"", texts)}\"],number:{number},decnumber:{decnumber}\n";
    }
}
