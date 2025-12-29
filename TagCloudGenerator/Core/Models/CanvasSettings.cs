using System.Drawing;

namespace TagCloudGenerator.Core.Models
{
    public class CanvasSettings
    {
        public Color BackgroundColor { get; private set; } = Color.White;
        public Size CanvasSize { get; private set; } = new Size(1000, 1000);
        public bool ShowRectangles { get; private set; } = true;
        public int EdgePadding { get; private set; } = 50;

        public CanvasSettings SetWidth(int? width)
        {
            var size = CanvasSize;

            size.Width = width is > 0 ? width.Value : CanvasSize.Width;
            CanvasSize = size;

            return this;
        }

        public CanvasSettings SetHeight(int? height)
        {
            var size = CanvasSize;

            size.Height = height is > 0 ? height.Value : CanvasSize.Height;
            CanvasSize = size;

            return this;
        }

        public CanvasSettings SetSize(int? width, int? height)
        {
            var size = CanvasSize;

            size.Width = width is > 0 ? width.Value : CanvasSize.Width;
            size.Height = height is > 0 ? height.Value : CanvasSize.Height;
            CanvasSize = size;

            return this;
        }

        public CanvasSettings SetBackgroundColor(Color? color)
        {
            if (color.HasValue)
                BackgroundColor = color.Value;
            return this;
        }

        public CanvasSettings WithShowRectangles(bool value = true)
        {
            ShowRectangles = value;
            return this;
        }

        public CanvasSettings SetPadding(int? padding)
        {
            EdgePadding = padding is >= 0 ? (int)padding.Value : EdgePadding;
            return this;
        }
    }
}
