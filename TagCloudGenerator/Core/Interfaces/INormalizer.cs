namespace TagCloudGenerator.Core.Interfaces
{
    public interface INormalizer
    {
        public List<string> Normalize(List<string> words);
    }
}
