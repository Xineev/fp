
namespace TagCloudGenerator.Core.Interfaces
{
    public interface ISorterer
    {
        public List<(string Word, int Frequency)> Sort(Dictionary<string, int> wordsWithFreqs);
    }
}
