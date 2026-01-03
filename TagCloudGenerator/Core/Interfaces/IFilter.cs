using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IFilter
    {
        Result<List<string>> Filter(List<string> words);

        bool ShouldInclude(string word);
    }
}
