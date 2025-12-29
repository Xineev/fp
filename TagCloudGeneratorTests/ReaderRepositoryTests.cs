using Moq;
using NUnit.Framework;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Infrastructure.Readers;

namespace TagCloudGeneratorTests
{
    public class ReaderRepositoryTests
    {
        private Mock<IFormatReader> firstReaderMock;
        private Mock<IFormatReader> secondReaderMock;
        private ReaderRepository readerRepository;

        [SetUp]
        public void Setup()
        {
            firstReaderMock = new Mock<IFormatReader>();
            secondReaderMock = new Mock<IFormatReader>();

            readerRepository = new ReaderRepository(
                new[] { firstReaderMock.Object, secondReaderMock.Object });
        }

        [Test]
        public void CanRead_WhenAnyReaderCanRead_ReturnsTrue()
        {
            firstReaderMock.Setup(r => r.CanRead("file.docx")).Returns(false);
            secondReaderMock.Setup(r => r.CanRead("file.docx")).Returns(true);

            var result = readerRepository.CanRead("file.docx");

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanRead_WhenNoReaderCanRead_ReturnsFalse()
        {
            firstReaderMock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);
            secondReaderMock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);

            var result = readerRepository.CanRead("file.unknown");

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryRead_UsesFirstMatchingReader()
        {
            var expected = new List<string> { "word1", "word2" };

            firstReaderMock.Setup(r => r.CanRead("file.docx")).Returns(false);
            secondReaderMock.Setup(r => r.CanRead("file.docx")).Returns(true);
            secondReaderMock.Setup(r => r.TryRead("file.docx")).Returns(expected);

            var result = readerRepository.TryGetReader("file.docx");
            Assert.That(result.IsSuccess, Is.True);

            Assert.That(result.GetValueOrThrow().TryRead("file.docx").GetValueOrThrow(), Is.EqualTo(expected));

            secondReaderMock.Verify(r => r.TryRead("file.docx"), Times.Once);
            firstReaderMock.Verify(r => r.TryRead(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void TryRead_WhenNoReaderFound_ReturnsError()
        {
            firstReaderMock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);
            secondReaderMock.Setup(r => r.CanRead(It.IsAny<string>())).Returns(false);

            Assert.That(readerRepository.TryGetReader("file.xyz").IsSuccess, Is.False);
        }
    }
}
