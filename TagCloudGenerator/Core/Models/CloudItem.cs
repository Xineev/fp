using System.Drawing;

namespace TagCloudGenerator.Core.Models
{
    public class CloudItem
    {
        public string Word { get; }
        public Rectangle Rectangle { get; }
        public float FontSize { get; }
        public Color? TextColor { get; }
        public string FontFamily { get; }
        public FontStyle FontStyle { get; }
        public int Frequency { get; }

        public CloudItem(
            string word,
            Rectangle rectangle,
            float fontSize,
            Color? textColor = null,
            string fontFamily = "Arial",
            FontStyle fontStyle = FontStyle.Regular,
            int frequency = 1,
            double weight = 1.0)
        {
            Word = word ?? throw new ArgumentNullException(nameof(word));
            Rectangle = rectangle;
            FontSize = fontSize;
            TextColor = textColor;
            FontFamily = fontFamily ?? throw new ArgumentNullException(nameof(fontFamily));
            FontStyle = fontStyle;
            Frequency = frequency;
        }

        public CloudItem WithRectangle(Rectangle newRectangle)
        {
            return new CloudItem(Word, newRectangle, FontSize, TextColor, FontFamily, FontStyle, Frequency);
        }

        public CloudItem WithFontSize(float newFontSize)
        {
            return new CloudItem(Word, Rectangle, newFontSize, TextColor, FontFamily, FontStyle, Frequency);
        }

        public CloudItem WithColor(Color newColor)
        {
            return new CloudItem(Word, Rectangle, FontSize, newColor, FontFamily, FontStyle, Frequency);
        }
    }
}
