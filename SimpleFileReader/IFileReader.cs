
namespace SimpleFileReader
{
    public interface IFileReader<T> where T : class, new()
    {
        public T ReadFile(string path);

    }
}
