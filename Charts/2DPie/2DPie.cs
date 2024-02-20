using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Charts._2DPie
{
    public class _2DPie
    {
        public List<UIElement> Create(float pieWidth, float pieHeight, List<Category> categories)
        {
            var uiElements = new List<UIElement>();

            var centerX = pieWidth / 2;
            var centerY = pieHeight / 2;
            var radius = pieWidth / 2;

            var angle = 0f;
            var previousAngle = 0f;
            foreach (var category in categories)
            {
                var line1X = radius * Math.Cos(angle * Math.PI / 180) + centerX;
                var line1Y = radius * Math.Sin(angle * Math.PI / 180) + centerY;

                angle = category.Percentage * 360 / 100 + previousAngle;
                Debug.WriteLine(angle);

                var arcX = radius * Math.Cos(angle * Math.PI / 180) + centerX;
                var arcY = radius * Math.Sin(angle * Math.PI / 180) + centerY;

                var line1Segment = new LineSegment(new Point(line1X, line1Y), isStroked: false);
                var arcWidth = radius;
                var arcHeight = radius;
                var isLargeArc = category.Percentage > 50;
                var arcSegment = new ArcSegment
                {
                    Size = new Size(arcWidth, arcHeight),
                    Point = new Point(arcX, arcY),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = isLargeArc
                };
                var line2Segment = new LineSegment(new Point(centerX, centerY), isStroked: false);

                var pathFigure = new PathFigure(
                    new Point(centerX, centerY),
                    new List<PathSegment>
                    {
                        line1Segment,
                        arcSegment,
                        line2Segment
                    },
                    closed: true);

                var pathFigures = new List<PathFigure> { pathFigure };
                var pathGeometry = new PathGeometry(pathFigures);
                var path = new Path
                {
                    Fill = category.ColorBrush,
                    Data = pathGeometry
                };
                uiElements.Add(path);

                previousAngle = angle;


                // draw outlines
                var outline1 = new Line
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = line1Segment.Point.X,
                    Y2 = line1Segment.Point.Y,
                    Stroke = Brushes.White,
                    StrokeThickness = 5
                };
                var outline2 = new Line
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = arcSegment.Point.X,
                    Y2 = arcSegment.Point.Y,
                    Stroke = Brushes.White,
                    StrokeThickness = 5
                };

                uiElements.Add(outline1);
                uiElements.Add(outline2);
            }

            return uiElements;
        }
    }
}