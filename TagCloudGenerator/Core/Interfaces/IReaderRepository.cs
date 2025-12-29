using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IReaderRepository
    {
        public Result<IFormatReader> TryGetReader(string filePath);
    }
}