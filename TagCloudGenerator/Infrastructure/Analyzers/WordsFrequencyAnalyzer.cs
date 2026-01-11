using DocumentFormat.OpenXml.Office2010.PowerPoint;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Infrastructure.Analyzers
{
    public class WordsFrequencyAnalyzer : IAnalyzer
    {
        public Result<WordsWithFrequency> Analyze(List<string> words)
        {
            var wordFreqList = new List<WordFrequencyData>();
            var wordFreqDictionary = new Dictionary<string, int>();
            var minFrequency = int.MaxValue;
            var maxFrequency = int.MinValue;

            if (words == null || words.Count == 0)
                return Result.Ok(new WordsWithFrequency
                {
                    WordsWithFreq = wordFreqList,
                    MaxFreq = 0,
                    MinFreq = 0
                });

            foreach (string word in words) 
            {
                if (wordFreqDictionary.TryGetValue(word, out int currentCount))
                {
                    wordFreqDictionary[word] = currentCount + 1;
                    int newCount = currentCount + 1;

                    minFrequency = Math.Min(newCount, minFrequency);
                    maxFrequency = Math.Max(newCount, maxFrequency);
                }
                else
                {
                    wordFreqDictionary[word] = 1;

                    minFrequency = Math.Min(1, minFrequency);
                    maxFrequency = Math.Max(1, maxFrequency);
                }
            }

            foreach (var pair in wordFreqDictionary)
                wordFreqList.Add(new WordFrequencyData { 
                    Word = pair.Key, 
                    Frequency = pair.Value 
                });

            return Result.Ok(new WordsWithFrequency { 
                WordsWithFreq = wordFreqList, 
                MaxFreq = maxFrequency, 
                MinFreq = minFrequency 
            });
        }
    }
}
