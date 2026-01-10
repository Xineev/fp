using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Infrastructure.Sorterers
{
    public class FrequencyDescendingSorter : ISorter
    {
        public Result<WordsWithFrequency> Sort(WordsWithFrequency words)
        {
            if (words == null || words.WordsWithFreq.Count == 0)
                return Result.Ok(words);

            var sorted = words.WordsWithFreq.OrderByDescending(w => w.Frequency).ThenBy(w => w.Word).ToList();

            var result = Result.Ok(new WordsWithFrequency { WordsWithFreq = sorted, MaxFreq = words.MaxFreq, MinFreq = words.MinFreq });
            return result;
        }
    }
}
