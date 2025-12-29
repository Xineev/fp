using System.Drawing;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ITextMeasurer
    {
        Size Measure(string word, float fontSize, string fontFamily);
    }
}
