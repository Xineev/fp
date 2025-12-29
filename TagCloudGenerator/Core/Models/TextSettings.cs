using System.Drawing;

namespace TagCloudGenerator.Core.Models
{
    public class TextSettings
    {
        public string FontFamily { get; private set; } = "Arial";
        public float MinFontSize { get; private set; } = 12f;
        public float MaxFontSize { get; private set; } = 72f;
        public Color TextColor { get; private set; } = Color.Black;

        public TextSettings SetFontFamily(string? font)
        {
            if (!string.IsNullOrWhiteSpace(font)) FontFamily = font;
            return this;
        }

        public TextSettings SetMinFontSize(float? size)
        {
            MinFontSize = size is >= 0 ? size.Value : MinFontSize;
            return this;
        }

        public TextSettings SetMaxFontSize(float? size)
        {
            MaxFontSize = size is >= 0 ? size.Value : MaxFontSize;
            return this;
        }

        public TextSettings SetFontSizeRange(float? minSize, float? maxSize)
        {
            SetMinFontSize(minSize);
            SetMaxFontSize(maxSize);
            return this;
        }

        public TextSettings SetTextColor(Color? color)
        {
            if (color.HasValue) TextColor = color.Value;
            return this;
        }
    }
}
