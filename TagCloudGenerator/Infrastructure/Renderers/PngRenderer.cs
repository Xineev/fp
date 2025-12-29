using System.Drawing;
using System.Drawing.Imaging;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Core.Models;

namespace TagCloudGenerator.Infrastructure.Renderers
{
    public class PngRenderer : IRenderer
    {
        public Bitmap Render(IEnumerable<CloudItem> items, CanvasSettings canvasSettings, TextSettings textSettings)
        {
            var itemsList = items.ToList();
            var bitmap = new Bitmap(canvasSettings.CanvasSize.Width, canvasSettings.CanvasSize.Height);
            using var graphics = Graphics.FromImage(bitmap);
            ConfigureGraphics(graphics);
            graphics.Clear(canvasSettings.BackgroundColor);
            var (offsetX, offsetY) = CalculateOffset(itemsList, canvasSettings);

            using var brush = new SolidBrush(textSettings.TextColor);
            using var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            using var pen = new Pen(textSettings.TextColor, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
            foreach (var item in itemsList)
            {
                DrawCloudItem(graphics, item, offsetX, offsetY, canvasSettings, textSettings, brush, pen, stringFormat);
            }

            return bitmap;
        }

        private (int offsetX, int offsetY) CalculateOffset(
            List<CloudItem> items,
            CanvasSettings settings)
        {
            var minX = items.Min(i => i.Rectangle.X);
            var minY = items.Min(i => i.Rectangle.Y);
            var maxX = items.Max(i => i.Rectangle.Right);
            var maxY = items.Max(i => i.Rectangle.Bottom);

            var cloudWidth = maxX - minX;
            var cloudHeight = maxY - minY;

            var offsetX = (settings.CanvasSize.Width - cloudWidth) / 2 - minX;
            var offsetY = (settings.CanvasSize.Height - cloudHeight) / 2 - minY;

            return (offsetX, offsetY);
        }

        private void DrawCloudItem(
            Graphics graphics,
            CloudItem item,
            int offsetX,
            int offsetY,
            CanvasSettings canvasSettings,
            TextSettings textSettings,
            SolidBrush brush,
            Pen pen,
            StringFormat stringFormat)
        {
            var drawRect = new Rectangle(
                item.Rectangle.X + offsetX,
                item.Rectangle.Y + offsetY,
                item.Rectangle.Width,
                item.Rectangle.Height);

            var color = item.TextColor ?? textSettings.TextColor;

            using var font = new Font(
                item.FontFamily,
                item.FontSize,
                item.FontStyle,
                GraphicsUnit.Pixel);

            graphics.DrawString(item.Word, font, brush, drawRect, stringFormat);

            if (canvasSettings.ShowRectangles)
            {
                graphics.DrawRectangle(pen, drawRect);
            }
        }

        private void ConfigureGraphics(Graphics graphics)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        }

        private Rectangle CalculateCloudBounds(List<CloudItem> items)
        {
            var minX = items.Min(i => i.Rectangle.Left);
            var minY = items.Min(i => i.Rectangle.Top);
            var maxX = items.Max(i => i.Rectangle.Right);
            var maxY = items.Max(i => i.Rectangle.Bottom);

            return Rectangle.FromLTRB(minX, minY, maxX, maxY);
        }

        private bool FitsCanvas(Rectangle cloudBounds, Size canvasSize)
        {
            return cloudBounds.Width <= canvasSize.Width
                && cloudBounds.Height <= canvasSize.Height;
        }
    }
}
