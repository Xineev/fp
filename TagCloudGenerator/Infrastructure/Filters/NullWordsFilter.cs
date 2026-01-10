using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Filters
{
    public class NullWordsFilter : IFilter
    {
        public Result<List<string>> Filter(List<string> words)
        {
            if (words == null || words.Count == 0)
                return Result.Ok(words);

            return Result.Ok(words.Where(w => w != null).ToList());
        }

        public bool ShouldInclude(string word)
        {
            return word != null;
        }
    }
}
