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

        public List<string> TryRead(string filePath)
        {
            try
            {
                using var doc = WordprocessingDocument.Open(filePath, false);
                return doc.MainDocumentPart.Document.Body
                    .Elements<Paragraph>()
                    .Select(p => p.InnerText)
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"DOCX read error: {e.Message}");
                return new List<string>();
            }
        }
    }
}
