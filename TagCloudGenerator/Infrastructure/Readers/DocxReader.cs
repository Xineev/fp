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

            return Result.Of(() =>
            {
                using (var doc = WordprocessingDocument.Open(filePath, false))
                {
                    if (doc.MainDocumentPart == null)
                        throw new InvalidOperationException($"Document has no main part: '{filePath}'");

                    if (doc.MainDocumentPart.Document?.Body == null)
                        throw new InvalidOperationException($"Document has no body content: '{filePath}'");

                    return doc.MainDocumentPart.Document.Body
                        .Elements<Paragraph>()
                        .Select(p => p.InnerText)
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .ToList();
                }
            }, "Failed to read DOCX file");
        }
    }
}
