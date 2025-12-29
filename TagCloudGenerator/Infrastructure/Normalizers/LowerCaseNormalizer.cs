using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Normalizers
{
    public class LowerCaseNormalizer : INormalizer
    {
        public List<string> Normalize(List<string> words)
        {
            if (words == null || words.Count == 0) return new List<string>();

            return words.Select(w => w.ToLower()).ToList();
        }
    }
}
