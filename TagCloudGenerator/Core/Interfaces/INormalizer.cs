using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface INormalizer
    {
        public Result<List<string>> Normalize(List<string> words);
    }
}
