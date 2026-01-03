using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Analyzers
{
    public class WordsFrequencyAnalyzer : IAnalyzer
    {
        public Result<Dictionary<string, int>> Analyze(List<string> words)
        {
            var wordFreqDictionary = new Dictionary<string, int>();

            if (words == null)
                return Result.Fail<Dictionary<string, int>>("Missing words list to analyze");

            foreach (string word in words) 
            {
                if(word == null) 
                    return Result.Fail<Dictionary<string, int>>("Word in a list is null");
                
                if (wordFreqDictionary.TryAdd(word, 1))
                    continue;
                wordFreqDictionary[word]++;
            }

            return Result.Ok(wordFreqDictionary);
        }
    }
}
