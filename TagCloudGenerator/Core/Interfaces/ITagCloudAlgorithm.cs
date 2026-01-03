using System.Drawing;
using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ITagCloudAlgorithm
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);

        public ITagCloudAlgorithm Reset();
    }
}
