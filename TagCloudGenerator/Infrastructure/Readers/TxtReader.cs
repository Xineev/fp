using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Readers
{
    public class TxtReader : IFormatReader
    {
        public bool CanRead(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase);
        }

        public List<string> TryRead(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath).ToList();
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error reading file: {e.Message}");
                return null;
            }
        }
    }
}
