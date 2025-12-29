using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Readers
{
    public class TxtReader : IFormatReader
    {
        public bool CanRead(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase);
        }

        public Result<List<string>> TryRead(string filePath)
        {
            return Result.Of(() => { return File.ReadAllLines(filePath).ToList(); }, "Faield to read .txt file");
        }
    }
}
