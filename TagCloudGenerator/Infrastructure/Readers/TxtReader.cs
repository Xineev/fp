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
            return Result.Of(() => 
                File.ReadAllLines(filePath).ToList(), 
                $"Failed to read .txt file: '{filePath}'");
        }
    }
}
