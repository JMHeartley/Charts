using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFChartControls.Models;

namespace WPFChartControls
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

        public static readonly DependencyProperty LegendPositionProperty =
            DependencyProperty.Register(nameof(LegendPosition), typeof(LegendPosition), typeof(_2DPieChart));

        public _2DPieChart()
        {
            StrokeBrush = Brushes.White;
            StrokeThickness = 5;
            FontSize = 20;
            LegendPosition = LegendPosition.Right;

            InitializeComponent();
            UpdateLegendPosition();
        }

        /// <summary>
        ///     Gets or sets the collection of pie categories to be displayed in the chart.
        /// </summary>
        public ICollection<PieCategory> Categories
        {
            get => (ICollection<PieCategory>)GetValue(CategoriesProperty);
            set => SetValue(CategoriesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the brush used to draw the strokes of the pie slices.
        /// </summary>
        public SolidColorBrush StrokeBrush
        {
            get => (SolidColorBrush)GetValue(StrokeBrushProperty);
            set => SetValue(StrokeBrushProperty, value);
        }

        /// <summary>
        ///     Gets or sets the thickness of the strokes of the pie slices.
        /// </summary>
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        /// <summary>
        ///     Gets or set the position of the pie chart legend.
        /// </summary>
        public LegendPosition LegendPosition
        {
            get => (LegendPosition)GetValue(LegendPositionProperty);
            set => SetValue(LegendPositionProperty, value);
        }

        /// <summary>
        ///     Redraws the pie chart based on the current data and size.
        /// </summary>
        private void RedrawPieChart()
        {
            MainCanvas.Children.Clear();

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
                var previousAngleLineX = radius * Math.Cos(previousAngle * Math.PI / 180) + centerX;
                var previousAngleLineY = radius * Math.Sin(previousAngle * Math.PI / 180) + centerY;

                var newAngle = category.Percentage * 360 / 100 + previousAngle;

                var arcX = radius * Math.Cos(newAngle * Math.PI / 180) + centerX;
                var arcY = radius * Math.Sin(newAngle * Math.PI / 180) + centerY;

                var previousAngleLineSegment = new LineSegment(new Point(previousAngleLineX, previousAngleLineY), isStroked: false);
                var arcSegment = new ArcSegment
                {
                    Size = new Size(radius, radius),
                    Point = new Point(arcX, arcY),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = category.Percentage > 50
                };
                var newAngleLineSegment = new LineSegment(new Point(centerX, centerY), isStroked: false);

                var pathFigure = new PathFigure(
                    new Point(centerX, centerY),
                    new List<PathSegment>
                    {
                        previousAngleLineSegment,
                        arcSegment,
                        newAngleLineSegment
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
                    X2 = previousAngleLineSegment.Point.X,
                    Y2 = previousAngleLineSegment.Point.Y,
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

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e) => RedrawPieChart();

        private void LegendColumn_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // ensure chart is redrawn after legend is populated / size is initialized
            RedrawPieChart();
        }

        private void UpdateLegendPosition()
        {
            switch (LegendPosition)
            {
                case LegendPosition.Bottom:
                    Grid.SetColumn(Legend, value: 1);
                    Grid.SetRow(Legend, value: 2);
                    break;
                case LegendPosition.Left:
                    Grid.SetColumn(Legend, value: 0);
                    Grid.SetRow(Legend, value: 1);

                    break;
                case LegendPosition.Right:
                    Grid.SetColumn(Legend, value: 2);
                    Grid.SetRow(Legend, value: 1);

                    break;
                case LegendPosition.Top:
                    Grid.SetColumn(Legend, value: 1);
                    Grid.SetRow(Legend, value: 0);

                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{nameof(LegendPosition)} is not a valid value.");
            }
        }
    }
}