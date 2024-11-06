using SimpleFileReader.DataParsers.Implementations;
using SimpleFileReader.Test.Data;
using System.Collections;
using System.Reflection;

namespace SimpleFileReader.Test
{
    [TestFixtureSource(typeof(FixtureArgs))]
    public class ObjectDataParserTests<T>
    {
        private SimpleObject<T> _object;
        private string _correctValue;
        private T _expectedValue;
        private string _faultyValue;

        private ObjectDataParser _parser;
        private PropertyInfo _propertyInfo;

        public ObjectDataParserTests(SimpleObject<T> objectArg,
                                     string correctValue, 
                                     T expectedValue, 
                                     string faultyValue) 
        {
            _object = objectArg;
            _correctValue = correctValue;
            _expectedValue = expectedValue;
            _faultyValue = faultyValue;
        }

        [SetUp]
        public void Setup()
        {
            _parser = new ObjectDataParser();
            _propertyInfo = _object.GetType().GetProperty("Value") ?? throw new Exception("SimpleObject should have Value property");
        }

        [Test]
        public void ObjectDataParserParseThrows()
        {
            // Act
            TestDelegate action = () => _parser.Parse(_correctValue);

            // Assert
            Assert.Throws<NotImplementedException>(action);
        }

        [Test]
        public void ObjectDataParserParseAndUpdateDoesNotThrow()
        {
            // Act
            TestDelegate action = () => _parser.ParseAndUpdate(_object, _propertyInfo, _correctValue);

            // Assert
            Assert.DoesNotThrow(action);
        }

        [Test]
        public void ObjectDataParserParseAndUpdateCorrect()
        {
            // Act
            _parser.ParseAndUpdate(_object, _propertyInfo, _correctValue);

            // Assert
            Assert.That(_expectedValue, Is.EqualTo(_object.Value));
        }

        [Test]
        public void ObjectDataParserParseAndUpdateThrows()
        {
            // Act
            TestDelegate action = () => _parser.ParseAndUpdate(_object, _propertyInfo, _faultyValue);

            // Assert
            Assert.Throws<FormatException>(action);
        }
    }

    class FixtureArgs : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            // Return type, correct value, expected value, faulty value
            yield return new object[] { typeof(int), new SimpleObject<int>(), "1", 1, ""};
            yield return new object[] { typeof(int), new SimpleObject<int>(), "0", 0, "a" };
            yield return new object[] { typeof(int), new SimpleObject<int>(), "-1", -1, "1.0" };
            yield return new object[] { typeof(int), new SimpleObject<int>(), "1000", 1000, "1,00" };

            yield return new object[] { typeof(double), new SimpleObject<double>(), "1.1", 1.1, "" };
            yield return new object[] { typeof(double), new SimpleObject<double>(), "0", 0, "a" };
            yield return new object[] { typeof(double), new SimpleObject<double>(), "-1.1", -1.1, "1.a" };
            yield return new object[] { typeof(double), new SimpleObject<double>(), "1234.5", 1234.5, "@.0" };

            yield return new object[] { typeof(string), new SimpleObject<string>(), "\"test\"", "test", "" };
            yield return new object[] { typeof(string), new SimpleObject<string>(), "\"\"", "", "\"" };

            yield return new object[] { typeof(string[]), new SimpleObject<string[]>(), "[\"test\"]", new string[] { "test" }, "" };
            yield return new object[] { typeof(string[]), new SimpleObject<string[]>(), "[]", new string[0], "[" };
            yield return new object[] { typeof(string[]), new SimpleObject<string[]>(), "[\"test\",\"text\"]", new string[] { "test", "text" }, "[\"test]" };
        }

    }
}
