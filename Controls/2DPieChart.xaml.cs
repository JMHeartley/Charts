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
            double chartSize;
            if (MainCanvas.ActualWidth > MainCanvas.ActualHeight)
            {
                chartSize = MainCanvas.ActualHeight;
            }
            else
            {
                chartSize = MainCanvas.ActualWidth;
            }

            if (chartSize <= 0
                || double.IsNaN(chartSize)
                || Categories is null
                || !Categories.Any())
            {
                return;
            }

            var centerX = chartSize / 2;
            var centerY = chartSize / 2;
            var radius = chartSize / 2;

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

            var angle = 0f;
            var previousAngle = 0f;
            foreach (var category in Categories)
            {
                var line1X = radius * Math.Cos(angle * Math.PI / 180) + centerX;
                var line1Y = radius * Math.Sin(angle * Math.PI / 180) + centerY;

                angle = category.Percentage * 360 / 100 + previousAngle;
                Debug.WriteLine($"The current angle is {angle} degrees");

                previousAngle = angle;

                var arcX = radius * Math.Cos(angle * Math.PI / 180) + centerX;
                var arcY = radius * Math.Sin(angle * Math.PI / 180) + centerY;

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
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainCanvas.Children.Clear();

            CreateChart();
        }
    }
}