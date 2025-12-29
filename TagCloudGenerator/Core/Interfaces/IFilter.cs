
namespace TagCloudGenerator.Core.Interfaces
{
    public interface IFilter
    {
        List<string> Filter(List<string> words);
    }
}
