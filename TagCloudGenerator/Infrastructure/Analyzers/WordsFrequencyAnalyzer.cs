using System.Drawing;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Infrastructure.Analyzers
{
    public class WordsFrequencyAnalyzer : IAnalyzer
    {
        public Dictionary<string, int> Analyze(List<string> words)
        {
            var wordFreqDictionary = new Dictionary<string, int>();

            if (words == null || words.Count == 0)
                return new Dictionary<string, int>();

            foreach (string word in words) 
            {
                if(wordFreqDictionary.TryAdd(word, 1))
                    continue;
                else wordFreqDictionary[word]++;
            }

            return wordFreqDictionary;
        }
    }
}
