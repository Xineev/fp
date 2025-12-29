using NUnit.Framework;
using TagCloudGenerator.Infrastructure.Filters;

namespace TagCloudGeneratorTests
{
    public class BoringWordsFilterTests
    {
        private BoringWordsFilter filter = new BoringWordsFilter();

        [Test]
        public void Filter_EmptyInput_ReturnsEmpty()
        {
            var words = new List<string>();
            var result = filter.Filter(words);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Filter_RemovesBoringWords_Test()
        {
            var words = new List<string> { "hello", "in", "world", "a", "test" };
            var result = filter.Filter(words).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result, Contains.Item("hello"));
            Assert.That(result, Contains.Item("world"));
            Assert.That(result, Contains.Item("test"));
            Assert.That(result, Does.Not.Contain("in"));
            Assert.That(result, Does.Not.Contain("a"));
        }

        [Test]
        public void Filter_AllBoringWords_ReturnsEmpty_Test()
        {
            var words = new List<string>  { "in", "a", "for", "on" };
            var result = filter.Filter(words);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Filter_NoBoringWords_ReturnsAllWords_Test()
        {
            var words = new List<string> { "hello", "world", "test" };
            var result = filter.Filter(words).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result, Is.EquivalentTo(words));
        }

        [Test]
        public void ShouldInclude_ReturnsFalseForBoringWords_Test()
        {
            var boringWords = new List<string> { "in", "it", "a", "as", "for", "of", "on" };
            foreach (var word in boringWords)
            {
                Assert.That(filter.ShouldInclude(word), Is.False);
            }
        }

        [Test]
        public void ShouldInclude_ReturnsTrueForNormalWords_Test()
        {
            var normalWords = new List<string> { "hello", "world", "computer", "programming", "test" };
            foreach (var word in normalWords)
            {
                Assert.That(filter.ShouldInclude(word), Is.True);
            }
        }

        [Test]
        public void Filter_PreservesOrder_Test()
        {
            var words = new List<string> { "hello", "in", "world", "a", "test", "for" };
            var result = filter.Filter(words).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0], Is.EqualTo("hello"));
            Assert.That(result[1], Is.EqualTo("world"));
            Assert.That(result[2], Is.EqualTo("test"));
        }
    }
}
