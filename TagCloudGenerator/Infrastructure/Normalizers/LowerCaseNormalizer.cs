using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Normalizers
{
    public class LowerCaseNormalizer : INormalizer
    {
        public Result<List<string>> Normalize(List<string> words)
        {
            if (words == null || words.Count == 0)
                return Result.Ok(words);

            return Result.Of(() => words.Select(w => w.ToLower()).ToList());
        }
    }
}
