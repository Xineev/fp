using Moq;
using NUnit.Framework;
using System.Drawing;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Core.Services;

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
        private Mock<ISorterer> sortererMock;
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
            sortererMock = new Mock<ISorterer>();

            cloudGenerator = new CloudGenerator(
                algorithmMock.Object,
                analyzerMock.Object,
                rendererMock.Object,
                fontSizeCalculatorMock.Object,
                textMeasurerMock.Object,
                sortererMock.Object);
        }

        [Test]
        public void Generate_EmptyWords_ReturnsFailResult_AndDoesNotRender_Test()
        {
            filterMock
                .Setup(f => f.Filter(It.IsAny<List<string>>()))
                .Returns(new List<string>());

            var result = cloudGenerator.Generate(
                new List<string>(),
                new CanvasSettings(),
                new TextSettings(),
                new[] { filterMock.Object });

            Assert.That(result.IsSuccess, Is.False);

            rendererMock.Verify(r => r.Render(
                It.IsAny<IEnumerable<CloudItem>>(),
                It.IsAny<CanvasSettings>(),
                It.IsAny<TextSettings>()),
                Times.Never);
        }

        [Test]
        public void Generate_AllWordsFilteredOut_ReturnsFailResult_Test()
        {
            var words = new List<string> { "in", "a", "for" };

            filterMock
                .Setup(f => f.Filter(words))
                .Returns(new List<string>());

            var result = cloudGenerator.Generate(
                words,
                new CanvasSettings(),
                new TextSettings(),
                new[] { filterMock.Object });

            Assert.That(result.IsSuccess, Is.False);

            rendererMock.Verify(r => r.Render(
                It.IsAny<IEnumerable<CloudItem>>(),
                It.IsAny<CanvasSettings>(),
                It.IsAny<TextSettings>()),
                Times.Never);
        }

        [Test]
        public void Generate_NormalFlow_CallsAllDependencies_Test()
        {
            var words = new List<string> { "hello", "world", "hello" };
            var filteredWords = new List<string> { "hello", "world", "hello" };

            var analyzed = new Dictionary<string, int> { { "hello", 2 }, { "world", 1 } };

            var sorted = new List<(string Word, int Frequency)> { ("hello", 2), ("world", 1) };

            filterMock
                .Setup(f => f.Filter(words))
                .Returns(filteredWords);

            analyzerMock
                .Setup(a => a.Analyze(filteredWords))
                .Returns(analyzed);

            sortererMock
                .Setup(s => s.Sort(analyzed))
                .Returns(sorted);

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

            rendererMock
                .Setup(r => r.Render(
                    It.IsAny<IEnumerable<CloudItem>>(),
                    It.IsAny<CanvasSettings>(),
                    It.IsAny<TextSettings>()))
                .Returns(new Bitmap(1, 1));

            var result = cloudGenerator.Generate(
                words,
                new CanvasSettings(),
                textSettings,
                new[] { filterMock.Object });

            Assert.That(result.IsSuccess, Is.True);
            result.GetValueOrThrow().Dispose();

            filterMock.Verify(f => f.Filter(words), Times.Once);
            analyzerMock.Verify(a => a.Analyze(filteredWords), Times.Once);
            algorithmMock.Verify(a => a.Reset(), Times.Once);
            algorithmMock.Verify(a => a.PutNextRectangle(It.IsAny<Size>()), Times.Exactly(2));

            rendererMock.Verify(r => r.Render(
                It.Is<IEnumerable<CloudItem>>(items => items.Count() == 2),
                It.IsAny<CanvasSettings>(),
                It.Is<TextSettings>(ts =>
                    ts.FontFamily == "Arial" &&
                    Math.Abs(ts.MinFontSize - 12) < float.Epsilon &&
                    Math.Abs(ts.MaxFontSize - 72) < float.Epsilon)),
                Times.Once);
        }

    }
}
