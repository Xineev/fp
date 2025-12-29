using System.Drawing;
using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface IRenderer
    {
        public Result<Bitmap> Render(IEnumerable<CloudItem> items, CanvasSettings canvasSettings, TextSettings textSettings);
    }
}
