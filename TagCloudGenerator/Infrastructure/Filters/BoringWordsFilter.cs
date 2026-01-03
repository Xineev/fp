using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Filters
{
    public class BoringWordsFilter : IFilter
    {
        private string[] boringWords = ["in", "it", "a", "as", "for", "of", "on"];

        public BoringWordsFilter() { }

        public BoringWordsFilter(IEnumerable<string> words) 
        {
            boringWords = words.ToArray();
        }

        public Result<List<string>> Filter(List<string> words)
        {
            if (words == null) 
                return Result.Fail<List<string>>("Filter missing list of words");

            return words.Where(w => !boringWords.Contains(w)).ToList();
        }

        //А надо ли тут оборачивать в Result?
        //С одной стороны оставляя bool мы маскируем исключение, с другой Result исполняет аналогичную возвращаемому значению задачу
        public bool ShouldInclude(string word)
        {
            if(word == null) return false;

            return !boringWords.Contains(word);
        }
    }
}
