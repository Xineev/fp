using System.Drawing;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.Infrastructure;

namespace TagCloudGenerator.Algorithms
{
    public class BasicTagCloudAlgorithm : ITagCloudAlgorithm
    {
        private Point center;

        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        private double currentAngle = 0;
        private double currentRadius = 0;

        private double angleStep = 0.1;
        private double radiusStep = 0.5;

        public BasicTagCloudAlgorithm(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                center = new Point(0, 0);

            this.center = center;
        }

        public BasicTagCloudAlgorithm()
        {
            center = new Point(0, 0);
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Fail<Rectangle>("Rectangle sizes must be positives");

            if (rectangles.Count == 0)
            {
                return Result.Ok(PutFirstRectangle(rectangleSize));
            }

            return Result.Ok(PlaceNextRectangle(rectangleSize));
        }

        private Rectangle PlaceNextRectangle(Size rectangleSize)
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

        public ITagCloudAlgorithm Reset()
        {
            rectangles.Clear();
            return this;
        }
    }
}
