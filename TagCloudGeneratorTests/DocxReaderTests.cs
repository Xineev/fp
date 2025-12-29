using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NUnit.Framework;
using TagCloudGenerator.Infrastructure;
using TagCloudGenerator.Infrastructure.Readers;

namespace TagCloudGeneratorTests
{
    public class DocxReaderTests
    {
        private string tempFilePath;
        private DocxReader reader;

        [SetUp]
        public void Setup()
        {
            tempFilePath = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid() + ".docx");

            reader = new DocxReader();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }

        [Test]
        public void CanRead_DocxExtension_ReturnsTrue_Test()
        {
            Assert.That(reader.CanRead("file.docx"), Is.True);
            Assert.That(reader.CanRead("file.txt"), Is.False);
        }

        [Test]
        public void TryRead_DocxWithParagraphs_ReturnsParagraphs_Test()
        {
            CreateDocx(new[] { "word1", "word2", "word3" });

            var result = reader.TryRead(tempFilePath);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.GetValueOrThrow(), Is.EqualTo(new[] { "word1", "word2", "word3" }));
        }

        [Test]
        public void TryRead_EmptyDocx_ReturnsEmptyList_Test()
        {
            CreateDocx(Array.Empty<string>());

            var result = reader.TryRead(tempFilePath);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.GetValueOrThrow(), Is.Empty);
        }

        [Test]
        public void TryRead_DocxWithEmptyParagraphs_IgnoresThem_Test()
        {
            CreateDocx(new[] { "word1", "", "   ", "word2" });
            var result = reader.TryRead(tempFilePath);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.GetValueOrThrow(), Is.EqualTo(new[] { "word1", "word2" }));
        }

        [Test]
        public void TryRead_NonExistentFile_ReturnsFailResult_Test()
        {
            var result = reader.TryRead("nonexistent.docx");

            Assert.That(result.IsSuccess, Is.False);
        }

        private void CreateDocx(IEnumerable<string> lines)
        {
            using var doc = WordprocessingDocument.Create(
                tempFilePath,
                DocumentFormat.OpenXml.WordprocessingDocumentType.Document);

            var body = new Body();

            foreach (var line in lines)
            {
                body.AppendChild(
                    new Paragraph(
                        new Run(new Text(line))));
            }

            doc.AddMainDocumentPart().Document =
                new Document(body);
        }
    }
}
