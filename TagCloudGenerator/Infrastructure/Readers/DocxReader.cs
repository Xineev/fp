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
            return Result.Of(() =>
            {
                using (var doc = WordprocessingDocument.Open(filePath, true))
                {
                    if(doc.MainDocumentPart != null && doc.MainDocumentPart.Document.Body != null)
                    {
                        return doc.MainDocumentPart.Document.Body
                        .Elements<Paragraph>()
                        .Select(p => p.InnerText)
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .ToList();
                    }

                    return new List<string>();
                }
            }, "Failed to read DOCX file");
        }
    }
}
