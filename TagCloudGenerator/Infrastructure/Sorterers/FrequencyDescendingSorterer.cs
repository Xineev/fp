using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Sorterers
{
    public class FrequencyDescendingSorterer : ISorterer
    {
        public List<(string Word, int Frequency)> Sort(Dictionary<string, int> wordsWithFreqs)
        {
            return wordsWithFreqs
                .OrderByDescending(w => w.Value)
                .ThenBy(w => w.Key)
                .Select(kvpair => (kvpair.Key, kvpair.Value))
                .ToList();
        }
    }
}
