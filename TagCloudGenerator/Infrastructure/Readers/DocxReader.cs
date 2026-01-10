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
            var docOpeningResult = Result.Of(() => 
                WordprocessingDocument.Open(filePath, false),
                $"Failed to get document: '{filePath}'");

            if (!docOpeningResult.IsSuccess)
                return Result.Fail<List<string>>(docOpeningResult.Error);

            var doc = docOpeningResult.Value;

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

            doc.Dispose();

            return result;
        }
    }
}
