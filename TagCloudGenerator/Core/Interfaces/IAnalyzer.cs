using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IAnalyzer
    {
        Result<WordsWithFrequency> Analyze(List<string> words);
    }
}
