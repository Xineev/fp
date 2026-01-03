using System.Drawing;
using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Infrastructure.Measurers
{
    public class GraphicsTextMeasurer : ITextMeasurer
    {
        public Result<Size> Measure(string word, float fontSize, string fontFamily)
        {
            if (fontSize < 0)
                return Result.Fail<Size>("Font size can't be negative number");

            if(word == null)
                return Result.Fail<Size>("Can't measure word which is null");

            using var font = new Font(fontFamily, fontSize);
            using var bitmap = new Bitmap(1, 1);
            using var graphics = Graphics.FromImage(bitmap);

            var size = graphics.MeasureString(word, font);
            return Result.Ok(
                new Size((int)Math.Ceiling(size.Width), 
                (int)Math.Ceiling(size.Height)));
        }
    }
}
