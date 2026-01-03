
namespace TagCloudGenerator.Core.Models
{
    public class WordsWithFrequency
    {
        public List<WordFrequencyData> WordsWithFreq { get; set; }
        public int MaxFreq { get; set; }
        public int MinFreq { get; set; }

        public WordsWithFrequency() { }
    }
}
