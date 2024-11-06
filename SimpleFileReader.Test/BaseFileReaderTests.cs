using SimpleFileReader.Implementations;
using SimpleFileReader.Test.Data;
using SimpleFileReader.Test.Mocks;

namespace SimpleFileReader.Test
{
    public class BaseFileReaderTests
    {
        private BaseFileReader<SimpleObject<object>> _reader;
        
        [SetUp]
        public void Setup()
        {
            // Arrange
            _reader = new TestFileReader<SimpleObject<object>>();
        }
        
        [Test]
        public void CreateInstanceNoConstructorDoesNotThrow()
        {
            // Act
            TestDelegate action = () => _reader.CreateInstance(typeof(NoConstructor));

            // Assert
            Assert.DoesNotThrow(action);
        }

        [Test]
        public void CreateInstanceNoConstructorCorrectType()
        {
            // Act
            var res = _reader.CreateInstance(typeof(NoConstructor));

            // Assert
            Assert.True(res is NoConstructor);
        }

        [Test]
        public void CreateInstanceHasConstructorThrows()
        {
            // Act
            TestDelegate action = () => _reader.CreateInstance(typeof(HasConstructor));

            // Assert
            Assert.Throws<Exception>(action);
        }

        [Test]
        public void RetrieveDataFileDoesNotThrow()
        {
            // Act
            TestDelegate action = () => _reader.RetrieveData(Path.Combine(Environment.CurrentDirectory, @"Data\Text.txt"));

            // Assert
            Assert.DoesNotThrow(action);
        }

        [Test]
        public void RetrieveDataFileIsCorrect()
        {
            // Act
            string text = _reader.RetrieveData(Path.Combine(Environment.CurrentDirectory, @"Data\Text.txt"));

            // Assert
            Assert.That(text, Is.EqualTo("0123456789"));
        }


        [Test]
        public void RetrieveDataNoFileThrows()
        {
            // Act
            TestDelegate action = () => _reader.RetrieveData("");

            // Assert
            Assert.Throws<FileNotFoundException>(action);
        }
    }
}