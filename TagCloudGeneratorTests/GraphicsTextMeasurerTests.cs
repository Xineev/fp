using NUnit.Framework;
using TagCloudGenerator.Infrastructure.Measurers;

namespace TagCloudGeneratorTests
{
    public class GraphicsTextMeasurerTests
    {
        private GraphicsTextMeasurer measurer = new GraphicsTextMeasurer();

        [Test]
        public void Measure_EmptyString_ReturnsValidSize_Test()
        {
            var result = measurer.Measure("", 12f, "Arial");
            Assert.That(result.Width, Is.GreaterThanOrEqualTo(0));
            Assert.That(result.Height, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void Measure_SingleCharacter_ReturnsNonZeroSize_Test()
        {
            var result = measurer.Measure("A", 12f, "Arial");
            Assert.That(result.Width, Is.GreaterThan(0));
            Assert.That(result.Height, Is.GreaterThan(0));
        }

        [Test]
        public void Measure_LongerWord_ReturnsWiderSize_Test()
        {
            var shortSize = measurer.Measure("A", 12f, "Arial");
            var longSize = measurer.Measure("ABCDEFGHIJ", 12f, "Arial");
            Assert.That(longSize.Width, Is.GreaterThan(shortSize.Width));
        }

        [Test]
        public void Measure_LargerFont_ReturnsLargerSize_Test()
        {
            var smallSize = measurer.Measure("Test", 12f, "Arial");
            var largeSize = measurer.Measure("Test", 24f, "Arial");
            Assert.That(largeSize.Width, Is.GreaterThan(smallSize.Width));
            Assert.That(largeSize.Height, Is.GreaterThan(smallSize.Height));
        }

        [Test]
        public void Measure_DifferentFontFamily_ReturnsDifferentSize_Test()
        {
            var arialSize = measurer.Measure("Test", 12f, "Arial");
            var timesSize = measurer.Measure("Test", 12f, "Times New Roman");
            Assert.That(timesSize.Width, Is.GreaterThan(0));
            Assert.That(timesSize.Height, Is.GreaterThan(0));
        }

        [Test]
        public void Measure_SameParametersTwice_ReturnsSameSize_Test()
        {
            var size1 = measurer.Measure("Consistent", 16f, "Arial");
            var size2 = measurer.Measure("Consistent", 16f, "Arial");
            Assert.That(size1.Width, Is.EqualTo(size2.Width));
            Assert.That(size1.Height, Is.EqualTo(size2.Height));
        }

        [Test]
        public void Measure_WithSpaces_ReturnsCorrectSize_Test()
        {
            var sizeWithoutSpace = measurer.Measure("HelloWorld", 12f, "Arial");
            var sizeWithSpace = measurer.Measure("Hello World", 12f, "Arial");
            Assert.That(sizeWithSpace.Width, Is.GreaterThan(sizeWithoutSpace.Width));
        }

        [Test]
        public void Measure_VeryLargeFont_ReturnsProportionalSize_Test()
        {
            var result = measurer.Measure("Test", 100f, "Arial");
            Assert.That(result.Width, Is.GreaterThan(50));
            Assert.That(result.Height, Is.GreaterThan(50));
        }
    }
}
