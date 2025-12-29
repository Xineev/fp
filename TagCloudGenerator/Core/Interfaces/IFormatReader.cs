using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IFormatReader
    {
        bool CanRead(string filePath);
        List<string> TryRead(string filePath);
    }
}
