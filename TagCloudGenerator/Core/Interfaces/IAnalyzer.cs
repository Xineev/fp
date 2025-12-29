using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IAnalyzer
    {
        Dictionary<string, int> Analyze(List<string> words);
    }
}
