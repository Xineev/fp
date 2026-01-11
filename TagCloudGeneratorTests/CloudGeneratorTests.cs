using Moq;
using NUnit.Framework;
using System.Drawing;
using System.Globalization;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Core.Services;
using TagCloudGenerator.Infrastructure;

namespace TagCloudGeneratorTests
{
    public class CloudGeneratorTests
    {
        private Mock<ITagCloudAlgorithm> algorithmMock;
        private Mock<IFilter> filterMock;
        private Mock<IAnalyzer> analyzerMock;
        private Mock<IRenderer> rendererMock;
        private Mock<IFontSizeCalculator> fontSizeCalculatorMock;
        private Mock<ITextMeasurer> textMeasurerMock;
        private Mock<ISorter> sortererMock;
        private CloudGenerator cloudGenerator;

        [SetUp]
        public void Setup()
        {
            algorithmMock = new Mock<ITagCloudAlgorithm>();
            filterMock = new Mock<IFilter>();
            analyzerMock = new Mock<IAnalyzer>();
            rendererMock = new Mock<IRenderer>();
            fontSizeCalculatorMock = new Mock<IFontSizeCalculator>();
            textMeasurerMock = new Mock<ITextMeasurer>();
            sortererMock = new Mock<ISorter>();

            cloudGenerator = new CloudGenerator(
                algorithmMock.Object,
                analyzerMock.Object,
                rendererMock.Object,
                fontSizeCalculatorMock.Object,
                textMeasurerMock.Object,
                sortererMock.Object);
        }

        [Test]
        public void Generate_EmptyWords_ReturnsSuccessfulResult_Test()
        {
            var words = new List<string> { };
            var filteredWords = new List<string> { };

            var analyzed = new WordsWithFrequency
            {
                WordsWithFreq = new List<WordFrequencyData> { },
                MaxFreq = 0,
                MinFreq = 0
            };

            var sorted = analyzed;

            filterMock
                .Setup(f => f.ShouldInclude(It.IsAny<string>()))
                .Returns(true);

            analyzerMock
                .Setup(a => a.Analyze(filteredWords))
                .Returns(analyzed);

            sortererMock
                .Setup(s => s.Sort(analyzed))
                .Returns(Result.Ok(sorted));

            var textSettings = new TextSettings()
                .SetFontFamily("Arial")
                .SetFontSizeRange(12, 72);

            var result = cloudGenerator.Generate(
                words,
                new CanvasSettings(),
                textSettings,
                new[] { filterMock.Object });

            Assert.That(result.IsSuccess, Is.True);

            filterMock.Verify(f => f.ShouldInclude(It.IsAny<string>()), Times.Never);
            analyzerMock.Verify(a => a.Analyze(filteredWords), Times.Once);
            algorithmMock.Verify(a => a.Reset(), Times.Once);
            algorithmMock.Verify(a => a.PutNextRectangle(It.IsAny<Size>()), Times.Never);
        }

        [Test]
        public void Generate_AllWordsFilteredOut_GeneratesSuccessfuly_Test()
        {
            var words = new List<string> { "in", "a", "for" };
            
            var filtered = new List<string>();

            var analyzed = new WordsWithFrequency
            {
                WordsWithFreq = new List<WordFrequencyData> { },
                MaxFreq = 0,
                MinFreq = 0
            };

            var sorted = analyzed;

            filterMock
                .Setup(f => f.Filter(words))
                .Returns(filtered);

            analyzerMock
               .Setup(a => a.Analyze(filtered))
               .Returns(analyzed);

            sortererMock
                .Setup(s => s.Sort(analyzed))
                .Returns(Result.Ok(sorted));

            var result = cloudGenerator.Generate(
                words,
                new CanvasSettings(),
                new TextSettings(),
                new[] { filterMock.Object });

            Assert.That(result.IsSuccess, Is.True);
            filterMock.Verify(f => f.ShouldInclude(It.IsAny<string>()), Times.Exactly(3));
            analyzerMock.Verify(a => a.Analyze(filtered), Times.Once);
            algorithmMock.Verify(a => a.Reset(), Times.Once);
            algorithmMock.Verify(a => a.PutNextRectangle(It.IsAny<Size>()), Times.Never);
        }

        [Test]
        public void Generate_NormalFlow_CallsAllDependencies_Test()
        {
            var words = new List<string> { "hello", "world", "hello" };
            var filteredWords = new List<string> { "hello", "world", "hello" };

            var analyzed = new WordsWithFrequency
            {
                WordsWithFreq = new List<WordFrequencyData>
                {
                    new WordFrequencyData { Word = "hello", Frequency = 2 },
                    new WordFrequencyData { Word = "world", Frequency = 1 }
                },
                MaxFreq = 2,
                MinFreq = 1
            };

            var sorted = analyzed;

            filterMock
                .Setup(f => f.ShouldInclude(It.IsAny<string>()))
                .Returns(true);

            analyzerMock
                .Setup(a => a.Analyze(filteredWords))
                .Returns(analyzed);

            sortererMock
                .Setup(s => s.Sort(analyzed))
                .Returns(Result.Ok(sorted));

            var textSettings = new TextSettings()
                .SetFontFamily("Arial")
                .SetFontSizeRange(12, 72);

            fontSizeCalculatorMock
                .Setup(f => f.Calculate(2, 1, 2, 12f, 72f))
                .Returns(50f);

            fontSizeCalculatorMock
                .Setup(f => f.Calculate(1, 1, 2, 12f, 72f))
                .Returns(20f);

            textMeasurerMock
                .Setup(t => t.Measure("hello", 50f, "Arial"))
                .Returns(new Size(100, 30));

            textMeasurerMock
                .Setup(t => t.Measure("world", 20f, "Arial"))
                .Returns(new Size(80, 25));

            algorithmMock
                .Setup(a => a.PutNextRectangle(It.IsAny<Size>()))
                .Returns(new Rectangle(0, 0, 100, 30));

            var result = cloudGenerator.Generate(
                words,
                new CanvasSettings(),
                textSettings,
                new[] { filterMock.Object });

            Assert.That(result.IsSuccess, Is.True);

            filterMock.Verify(f => f.ShouldInclude(It.IsAny<string>()), Times.Exactly(3));
            analyzerMock.Verify(a => a.Analyze(filteredWords), Times.Once);
            algorithmMock.Verify(a => a.Reset(), Times.Once);
            algorithmMock.Verify(a => a.PutNextRectangle(It.IsAny<Size>()), Times.Exactly(2));
        }

    }
}
