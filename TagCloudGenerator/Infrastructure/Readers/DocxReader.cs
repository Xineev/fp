using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Readers
{
    public class DocxReader : IFormatReader
    {
        public bool CanRead(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".docx", StringComparison.OrdinalIgnoreCase);
        }

        public Result<List<string>> TryRead(string filePath)
        {
            using var doc = WordprocessingDocument.Open(filePath, false);

            if (doc.MainDocumentPart == null)
                return Result.Fail<List<string>>("Missing MainDocumentPart");

            if (doc.MainDocumentPart.Document.Body == null)
                return Result.Fail<List<string>>("Missing document body");

            var result = Result.Of(() =>
            {
                return doc.MainDocumentPart.Document.Body
                    .Elements<Paragraph>()
                    .Select(p => p.InnerText)
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .ToList();
            },
            $"Failed to load .docx document: '{filePath}'");

            return result;
        }
    }
}
