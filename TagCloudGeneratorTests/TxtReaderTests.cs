using NUnit.Framework;
using TagCloudGenerator.Infrastructure.Readers;

namespace TagCloudGeneratorTests
{
    public class TxtReaderTests
    {
        private TxtReader reader = new TxtReader();
        private string tempFilePath = Path.GetTempFileName();

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }

        [Test]
        public void TryRead_ExistingFile_ReturnsLines_Test()
        {
            var expectedLines = new[] { "line1", "line2", "line3" };
            WriteToTempFile(string.Join(Environment.NewLine, expectedLines));

            var result = reader.TryRead(tempFilePath);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.GetValueOrThrow().ToList(), Is.EqualTo(expectedLines));
        }

        [Test]
        public void TryRead_EmptyFile_ReturnsEmpty_Test()
        {
            WriteToTempFile("");
            var result = reader.TryRead(tempFilePath);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.GetValueOrThrow(), Is.Empty);
        }

        [Test]
        public void TryRead_FileWithWhitespaceLines_ReturnsAllLines_Test()
        {
            WriteToTempFile("line1\n\nline2\n  \nline3");
            var result = reader.TryRead(tempFilePath);

            Assert.That(result.IsSuccess, Is.True);
            var lines = result.GetValueOrThrow().ToList();

            Assert.That(lines.Count, Is.EqualTo(5));
            Assert.That(lines[0], Is.EqualTo("line1"));
            Assert.That(lines[1], Is.EqualTo(""));
            Assert.That(lines[2], Is.EqualTo("line2"));
            Assert.That(lines[3], Is.EqualTo("  "));
            Assert.That(lines[4], Is.EqualTo("line3"));
        }

        [Test]
        public void TryRead_FileWithWindowsLineEndings_ReturnsCorrectLines_Test()
        {
            WriteToTempFile("line1\r\nline2\r\nline3");
            var result = reader.TryRead(tempFilePath);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.GetValueOrThrow().ToList(), Is.EqualTo(new[] { "line1", "line2", "line3" }));
        }

        [Test]
        public void TryRead_NonExistentFile_ReturnsNull_Test()
        {
            var nonExistentPath = "C:\\nonexistent\\file.txt";
            var result = reader.TryRead(nonExistentPath);
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void TryRead_FileWithOneLine_ReturnsOneLine_Test()
        {
            WriteToTempFile("single line");
            var result = reader.TryRead(tempFilePath);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.GetValueOrThrow().Count, Is.EqualTo(1));
            Assert.That(result.GetValueOrThrow()[0], Is.EqualTo("single line"));
        }

        [Test]
        public void TryRead_FileWithTrailingNewline_ReturnsCorrectLines_Test()
        {
            WriteToTempFile("line1\nline2\n");
            var result = reader.TryRead(tempFilePath);

            Assert.That(result.IsSuccess, Is.True);

            var lines = result.GetValueOrThrow();
            Assert.That(lines.Count, Is.EqualTo(2));
            Assert.That(lines[0], Is.EqualTo("line1"));
            Assert.That(lines[1], Is.EqualTo("line2"));
        }

        private void WriteToTempFile(string content)
        {
            File.WriteAllText(tempFilePath, content);
        }
    }
}
