using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Readers
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly IEnumerable<IFormatReader> _readers;

        public ReaderRepository(IEnumerable<IFormatReader> readers)
        {
            _readers = readers;
        }

        public bool CanRead(string filePath)
        {
            return _readers.Any(r => r.CanRead(filePath));
        }

        public Result<IFormatReader> TryGetReader(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Result.Fail<IFormatReader>("File path is empty");

            var reader = _readers.FirstOrDefault(r => r.CanRead(filePath));

            if (reader == null)
                return Result.Fail<IFormatReader>($"No reader found for file '{filePath}'");

            return Result.Ok(reader);
        }
    }
}