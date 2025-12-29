using NUnit.Framework;
using TagCloudGenerator.Infrastructure.Analyzers;

namespace TagCloudGeneratorTests
{
    public class WordsFrequencyAnalyzerTests
    {
        private WordsFrequencyAnalyzer frequencyAnalyzer  = new WordsFrequencyAnalyzer();

        [Test]
        public void Analyze_EmptyInput_ReturnsEmpty_Test()
        {
            var words = new List<string>();
            var result = frequencyAnalyzer.Analyze(words);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Analyze_SingleWord_ReturnsOneItemWithFrequencyOne_Test()
        {
            var words = new List<string> { "hello" };
            var result = frequencyAnalyzer.Analyze(words).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Key, Is.EqualTo("hello"));
            Assert.That(result[0].Value, Is.EqualTo(1));
        }

        [Test]
        public void Analyze_MultipleWords_ReturnsCorrectFrequencies_Test()
        {
            var words = new List<string> { "hello", "world", "hello", "test", "world", "hello" };
            var result = frequencyAnalyzer.Analyze(words).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Key, Is.EqualTo("hello"));
            Assert.That(result[0].Value, Is.EqualTo(3));
            Assert.That(result[1].Key, Is.EqualTo("world"));
            Assert.That(result[1].Value, Is.EqualTo(2));
            Assert.That(result[2].Key, Is.EqualTo("test"));
            Assert.That(result[2].Value, Is.EqualTo(1));
        }

        [Test]
        public void Analyze_CaseSensitive_ReturnsSeparateItems_Test()
        {
            var words = new List<string> { "Hello", "hello", "HELLO" };
            var result = frequencyAnalyzer.Analyze(words).ToList();
            Assert.That(result.Count, Is.EqualTo(3));
        }
    }
}