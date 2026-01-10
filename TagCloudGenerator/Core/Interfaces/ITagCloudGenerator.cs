using System.Drawing;
using TagCloudGenerator.Core.Models;
using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Core.Interfaces
{
    public interface ITagCloudGenerator
    {
        public Result<List<CloudItem>> Generate(List<string> words, CanvasSettings canvasSettings, TextSettings textSettings, ICollection<IFilter> filters);
    }
}
