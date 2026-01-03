using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Normalizers
{
    public class LowerCaseNormalizer : INormalizer
    {
        public Result<List<string>> Normalize(List<string> words)
        {
            if (words == null)
                return Result.Fail<List<string>>("Can't normalize if list is null");

            return Result.Of(() => words.Select(w => w.ToLower()).ToList());
        }
    }
}
