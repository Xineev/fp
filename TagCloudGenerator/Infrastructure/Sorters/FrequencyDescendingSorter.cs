using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Infrastructure.Sorterers
{
    public class FrequencyDescendingSorter : ISorter
    {
        public Result<WordsWithFrequency> Sort(Dictionary<string, int> wordsWithFreqs)
        {
            if (wordsWithFreqs == null)
                return Result.Fail<WordsWithFrequency>("Found no words to sort");

            var minFreq = int.MaxValue;
            var maxFreq = int.MinValue;

            var words = new List<WordFrequencyData>();

            foreach (var word in wordsWithFreqs) 
            {
                if(word.Key == null)
                    return Result.Fail<WordsWithFrequency>("Sorter found null key in dictionary");

                var wordWithFreq = new WordFrequencyData { Word = word.Key, Frequency = word.Value };
                minFreq = Math.Min(minFreq, word.Value);
                maxFreq = Math.Max(maxFreq, word.Value);

                words.Add(wordWithFreq);
            }

            var sorted = words.OrderByDescending(w => w.Frequency).ThenBy(w => w.Word).ToList();

            var result = Result.Ok(new WordsWithFrequency { WordsWithFreq = sorted, MaxFreq = maxFreq, MinFreq = minFreq });
            return result;
        }
    }
}
