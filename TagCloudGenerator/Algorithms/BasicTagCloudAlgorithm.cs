using System.Drawing;
using TagCloudGenerator.Core.Interfaces;

namespace TagCloudGenerator.Algorithms
{
    public class BasicTagCloudAlgorithm : ITagCloudAlgorithm
    {
        private Point center;

        private readonly Random random = new Random();

        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        private double currentAngle = 0;
        private double currentRadius = 0;

        private int countToGenerate = 10;

        private (int min, int max) width = new(20, 21);
        private (int min, int max) height = new(20, 21);

        private double angleStep = 0.1;
        private double radiusStep = 0.5;

        public BasicTagCloudAlgorithm(Point center)
        {
            if (center.X <= 0 || center.Y <= 0) throw new ArgumentException("Center coordinates must be positives");
            this.center = center;
        }

        public BasicTagCloudAlgorithm()
        {
            this.center = new Point(0, 0);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0) throw new ArgumentException("Rectangle sizes must be positives");

            if (rectangles.Count == 0)
            {
                return PutFirstRectangle(rectangleSize);
            }

            return PlaceNextRectange(rectangleSize);
        }

        private Rectangle PlaceNextRectange(Size rectangleSize)
        {
            while (true)
            {
                double x = center.X + currentRadius * Math.Cos(currentAngle);
                double y = center.Y + currentRadius * Math.Sin(currentAngle);

                var potentialCenter = new Point((int)(x - rectangleSize.Width / 2), (int)(y - rectangleSize.Height / 2));
                var potentialRectangle = new Rectangle(potentialCenter, rectangleSize);

                if (!IntersectWithAny(potentialRectangle))
                {
                    rectangles.Add(potentialRectangle);
                    return potentialRectangle;
                }

                currentAngle += angleStep;
                if (currentAngle >= 2 * Math.PI)
                {
                    currentAngle = 0;
                    currentRadius += radiusStep;
                }
            }
        }

        private bool IntersectWithAny(Rectangle potentialRectangle)
        {
            return rectangles.Any(rect => rect.IntersectsWith(potentialRectangle));
        }

        private Rectangle PutFirstRectangle(Size rectangleSize)
        {
            var firstCenter = new Point((int)(center.X - rectangleSize.Width / 2), (int)(center.Y - rectangleSize.Width / 2));
            var first = new Rectangle(firstCenter, rectangleSize);
            currentRadius = rectangleSize.Height / 2;
            rectangles.Add(first);
            return first;
        }

        public BasicTagCloudAlgorithm WithCenterAt(Point point)
        {
            if (point.X <= 0 || point.Y <= 0) throw new ArgumentException("Center coordinates must be positives");
            center = point;
            return this;
        }

        public ITagCloudAlgorithm Reset()
        {
            rectangles.Clear();
            return this;
        }
    }
}
