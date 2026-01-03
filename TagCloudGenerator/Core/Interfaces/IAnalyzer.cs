using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IAnalyzer
    {
        Result<Dictionary<string, int>> Analyze(List<string> words);
    }
}
