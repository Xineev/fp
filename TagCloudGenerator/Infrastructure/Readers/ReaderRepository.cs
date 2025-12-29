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

        public bool TryGetReader(string filePath, out IFormatReader outputReader)
        {
            outputReader = _readers.FirstOrDefault(r => r.CanRead(filePath));
            if (outputReader == null)
                return false;

            return true;
        }
    }
}