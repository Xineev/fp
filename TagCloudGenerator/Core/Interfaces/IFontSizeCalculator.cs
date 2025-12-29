
namespace TagCloudGenerator.Core.Interfaces
{
    public interface IFontSizeCalculator
    {
        float Calculate(int wordFrequency, int minFrequency, int maxFrequency, float minFontSize, float maxFontSize);
    }
}
