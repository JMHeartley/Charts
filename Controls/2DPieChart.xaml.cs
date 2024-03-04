using Controls.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Controls
{
    /// <summary>
    ///     Interaction logic for _2DPieChart.xaml
    /// </summary>
    public partial class _2DPieChart : UserControl
    {
        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register(nameof(Categories), typeof(ICollection<PieCategory>), typeof(_2DPieChart));

        public static readonly DependencyProperty StrokeBrushProperty =
            DependencyProperty.Register(nameof(StrokeBrush), typeof(SolidColorBrush), typeof(_2DPieChart));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(_2DPieChart));

        public _2DPieChart()
        {
            StrokeBrush = Brushes.White;
            StrokeThickness = 5;

            InitializeComponent();
        }

        public ICollection<PieCategory> Categories
        {
            get => (ICollection<PieCategory>)GetValue(CategoriesProperty);
            set => SetValue(CategoriesProperty, value);
        }

        public SolidColorBrush StrokeBrush
        {
            get => (SolidColorBrush)GetValue(StrokeBrushProperty);
            set => SetValue(StrokeBrushProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        private void CreateChart()
        {
            var centerX = MainCanvas.ActualWidth / 2;
            var centerY = MainCanvas.ActualHeight / 2;
            var radius = Math.Min(centerX, centerY);

            if (radius <= 0
                || double.IsNaN(radius)
                || Categories is null
                || !Categories.Any())
            {
                return;
            }

            var accumulatedPercentage = Categories.Select(category => category.Percentage)
                .Aggregate((accumulatePercentage, currentPercentage) => accumulatePercentage + currentPercentage);
            if ((int)accumulatedPercentage != 100)
            {
                MessageBox.Show($"Total percentage must be 100, {nameof(Categories)} added up to {accumulatedPercentage}.",
                    "Unable to Draw Pie Chart", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Categories.Any(category => (int)category.Percentage == 100))
            {
                var fullPieCategory = Categories.Single(category => (int)category.Percentage == 100);

                var fullCircle = new Ellipse
                {
                    Width = radius * 2,
                    Height = radius * 2,
                    Fill = fullPieCategory.ColorBrush,
                    Stroke = StrokeBrush,
                    StrokeThickness = StrokeThickness
                };

                Canvas.SetLeft(fullCircle, centerX - radius);
                Canvas.SetTop(fullCircle, centerY - radius);

                MainCanvas.Children.Add(fullCircle);
                return;
            }

            Categories.Aggregate(seed: 0f, (previousAngle, category) => DrawSlice(previousAngle, category));
            return;

            float DrawSlice(float previousAngle, PieCategory category)
            {
                var line1X = radius * Math.Cos(previousAngle * Math.PI / 180) + centerX;
                var line1Y = radius * Math.Sin(previousAngle * Math.PI / 180) + centerY;

                var newAngle = category.Percentage * 360 / 100 + previousAngle;
                Debug.WriteLine($"The current angle is {newAngle} degrees");

                var arcX = radius * Math.Cos(newAngle * Math.PI / 180) + centerX;
                var arcY = radius * Math.Sin(newAngle * Math.PI / 180) + centerY;

                var line1Segment = new LineSegment(new Point(line1X, line1Y), isStroked: false);
                var arcSegment = new ArcSegment
                {
                    Size = new Size(radius, radius),
                    Point = new Point(arcX, arcY),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = category.Percentage > 50
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
                MainCanvas.Children.Add(path);

                var outline1 = new Line
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = line1Segment.Point.X,
                    Y2 = line1Segment.Point.Y,
                    Stroke = StrokeBrush,
                    StrokeThickness = StrokeThickness
                };
                var outline2 = new Line
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = arcSegment.Point.X,
                    Y2 = arcSegment.Point.Y,
                    Stroke = StrokeBrush,
                    StrokeThickness = StrokeThickness
                };

                MainCanvas.Children.Add(outline1);
                MainCanvas.Children.Add(outline2);

                return newAngle;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainCanvas.Children.Clear();

            CreateChart();
        }
    }
}