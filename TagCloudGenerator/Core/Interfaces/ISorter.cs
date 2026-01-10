using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ISorter
    {
        public Result<WordsWithFrequency> Sort(WordsWithFrequency wordsWithFreqs);
    }
}
