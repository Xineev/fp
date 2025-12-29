using System.Drawing;
using TagCloudGenerator.Algorithms;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ITagCloudAlgorithm
    {
        Rectangle PutNextRectangle(Size rectangleSize);

        public ITagCloudAlgorithm Reset();
    }
}
