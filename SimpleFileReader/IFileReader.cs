using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader
{
    public interface IFileReader<T> where T : class, new()
    {
        public T ReadFile(string path);

    }
}
