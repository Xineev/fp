using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IFormatReader
    {
        bool CanRead(string filePath);
        Result<List<string>> TryRead(string filePath);
    }
}
