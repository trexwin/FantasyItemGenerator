using SimpleFileReader.Implementations;

namespace SimpleFileReader.Test.Mocks
{
    internal class TestFileReader<T> : BaseFileReader<T> where T : class, new()
    {
        public override T ReadFile(string path)
            => throw new NotImplementedException("TestFileReader is only used for testing the BaseFileReader");

        // Expose functions for testing
        internal new string RetrieveData(string path)
            => base.RetrieveData(path);

        internal new object CreateInstance(Type type)
            => base.CreateInstance(type);
    }
}
