using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IReaderRepository
    {
        public bool TryGetReader(string filePath, out IFormatReader outputReader);
    }
}