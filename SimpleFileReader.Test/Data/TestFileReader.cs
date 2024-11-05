using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleFileReader.Implementations;

namespace SimpleFileReader.Test.Data
{
    internal class TestFileReader<T> : BaseFileReader<T> where T : class, new()
    {
        public override T ReadFile(string path)
            => throw new NotImplementedException("TestFileReader is only used for testing the BaseFileReader");
    }
}
