using System.Drawing;
using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ITextMeasurer
    {
        Result<Size> Measure(string word, float fontSize, string fontFamily);
    }
}
