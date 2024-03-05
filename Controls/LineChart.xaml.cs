using Controls.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Controls
{
    /// <summary>
    ///     Interaction logic for LineChart.xaml
    /// </summary>
    public partial class LineChart : UserControl
    {
        private const int X_AXIS_TEXT_BLOCK_TOP_MARGIN = 5;

        private const int Y_AXIS_TEXT_BLOCK_RIGHT_MARGIN = 10;

        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register(nameof(Values), typeof(ICollection<LineValue>), typeof(LineChart));

        public static readonly DependencyProperty AxisStrokeBrushProperty =
            DependencyProperty.Register(nameof(AxisStrokeBrush), typeof(SolidColorBrush), typeof(LineChart));

        public static readonly DependencyProperty AxisStrokeThicknessProperty =
            DependencyProperty.Register(nameof(AxisStrokeThickness), typeof(double), typeof(LineChart));

        public static readonly DependencyProperty GridLineStrokeBrushProperty =
            DependencyProperty.Register(nameof(GridLineStrokeBrush), typeof(SolidColorBrush), typeof(LineChart));

        public static readonly DependencyProperty GridLineStrokeThicknessProperty =
            DependencyProperty.Register(nameof(GridLineStrokeThickness), typeof(double), typeof(LineChart));

        public static readonly DependencyProperty GridLineOpacityProperty =
            DependencyProperty.Register(nameof(GridLineOpacity), typeof(double), typeof(LineChart));

        public static readonly DependencyProperty InnerPaddingProperty =
            DependencyProperty.Register(nameof(InnerPadding), typeof(Thickness), typeof(LineChart));

        public static readonly DependencyProperty ValueLineStrokeBrushProperty =
            DependencyProperty.Register(nameof(ValueLineStrokeBrush), typeof(SolidColorBrush), typeof(LineChart));

        public static readonly DependencyProperty ValueLineStrokeThicknessProperty =
            DependencyProperty.Register(nameof(ValueLineStrokeThickness), typeof(double), typeof(LineChart));

        private readonly int XIntervalCount = 12;
        private readonly int YIntervalCount = 12;

        public LineChart()
        {
            AxisStrokeBrush = Brushes.LightGray;
            AxisStrokeThickness = 1;
            GridLineStrokeBrush = Brushes.LightGray;
            GridLineStrokeThickness = 10;
            GridLineOpacity = 1;
            InnerPadding = new Thickness(100);
            ValueLineStrokeBrush = new SolidColorBrush(Color.FromRgb(r: 68, g: 114, b: 196));
            ValueLineStrokeThickness = 10;

            InitializeComponent();

            SizeChanged += (sender, e) => Paint();
        }

        public ICollection<LineValue> Values
        {
            get => (ICollection<LineValue>)GetValue(ValuesProperty);
            set => SetValue(ValuesProperty, value);
        }

        public SolidColorBrush AxisStrokeBrush
        {
            get => (SolidColorBrush)GetValue(AxisStrokeBrushProperty);
            set => SetValue(AxisStrokeBrushProperty, value);
        }

        public double AxisStrokeThickness
        {
            get => (double)GetValue(AxisStrokeThicknessProperty);
            set => SetValue(AxisStrokeThicknessProperty, value);
        }

        public SolidColorBrush GridLineStrokeBrush
        {
            get => (SolidColorBrush)GetValue(GridLineStrokeBrushProperty);
            set => SetValue(GridLineStrokeBrushProperty, value);
        }

        public double GridLineStrokeThickness
        {
            get => (double)GetValue(GridLineStrokeThicknessProperty);
            set => SetValue(GridLineStrokeThicknessProperty, value);
        }

        public double GridLineOpacity
        {
            get => (double)GetValue(GridLineOpacityProperty);
            set => SetValue(GridLineOpacityProperty, value);
        }

        public Thickness InnerPadding
        {
            get => (Thickness)GetValue(InnerPaddingProperty);
            set => SetValue(InnerPaddingProperty, value);
        }

        public SolidColorBrush ValueLineStrokeBrush
        {
            get => (SolidColorBrush)GetValue(ValueLineStrokeBrushProperty);
            set => SetValue(ValueLineStrokeBrushProperty, value);
        }

        public double ValueLineStrokeThickness
        {
            get => (double)GetValue(ValueLineStrokeThicknessProperty);
            set => SetValue(ValueLineStrokeThicknessProperty, value);
        }

        public void Paint()
        {
            ChartCanvas.Children.Clear();

            if (ActualWidth <= 0
                || ActualHeight <= 0
                || Values is null
                || !Values.Any())
            {
                return;
            }

            var xAxisLine = new Line
            {
                X1 = InnerPadding.Left,
                Y1 = ActualHeight - InnerPadding.Bottom,
                X2 = ActualWidth - InnerPadding.Right,
                Y2 = ActualHeight - InnerPadding.Bottom,
                Stroke = AxisStrokeBrush,
                StrokeThickness = AxisStrokeThickness
            };
            var yAxisLine = new Line
            {
                X1 = InnerPadding.Left,
                Y1 = InnerPadding.Top,
                X2 = InnerPadding.Right,
                Y2 = ActualHeight - InnerPadding.Bottom,
                Stroke = AxisStrokeBrush,
                StrokeThickness = AxisStrokeThickness
            };

            ChartCanvas.Children.Add(xAxisLine);
            ChartCanvas.Children.Add(yAxisLine);

            var origin = new Point(xAxisLine.X1, yAxisLine.Y2);

            var xTextBlock0 = new TextBlock { Text = "0" };
            ChartCanvas.Children.Add(xTextBlock0);
            var xTextBlock0EstimatedSize = EstimateSize(xTextBlock0);
            Canvas.SetLeft(xTextBlock0, origin.X - xTextBlock0EstimatedSize.Width / 2);
            Canvas.SetTop(xTextBlock0, origin.Y + X_AXIS_TEXT_BLOCK_TOP_MARGIN);

            var xMaxValue = Values.Max(value => value.X);
            if (xMaxValue % XIntervalCount != 0)
            {
                xMaxValue = (int)Math.Ceiling(xMaxValue / XIntervalCount) * XIntervalCount;
            }

            var xIntervalNumberToValueRatio = xMaxValue / XIntervalCount;

            var chartInnerWidth = xAxisLine.X2 - xAxisLine.X1;
            var xIntervalNumberToPositionRatio = chartInnerWidth / XIntervalCount;

            for (var currentXIntervalNumber = 1; currentXIntervalNumber <= XIntervalCount; currentXIntervalNumber++)
            {
                var currentXPosition = origin.X + currentXIntervalNumber * xIntervalNumberToPositionRatio;

                var line = new Line
                {
                    X1 = currentXPosition,
                    Y1 = yAxisLine.Y1,
                    X2 = currentXPosition,
                    Y2 = yAxisLine.Y2,
                    Stroke = GridLineStrokeBrush,
                    StrokeThickness = GridLineStrokeThickness,
                    Opacity = GridLineOpacity
                };
                ChartCanvas.Children.Add(line);

                var textBlock = new TextBlock { Text = $"{currentXIntervalNumber * xIntervalNumberToValueRatio}" };
                ChartCanvas.Children.Add(textBlock);

                var textBlockEstimatedSize = EstimateSize(textBlock);
                Canvas.SetLeft(textBlock, currentXPosition - textBlockEstimatedSize.Width / 2);
                Canvas.SetTop(textBlock, line.Y2 + X_AXIS_TEXT_BLOCK_TOP_MARGIN);
            }

            var yTextBlock0 = new TextBlock { Text = "0" };
            ChartCanvas.Children.Add(yTextBlock0);
            var yTextBlock0EstimatedSize = EstimateSize(yTextBlock0);
            Canvas.SetLeft(yTextBlock0, origin.X - yTextBlock0EstimatedSize.Width - Y_AXIS_TEXT_BLOCK_RIGHT_MARGIN);
            Canvas.SetTop(yTextBlock0, origin.Y - yTextBlock0EstimatedSize.Height / 2);

            var yMaxValue = Values.Max(value => value.Y);
            if (yMaxValue % YIntervalCount != 0)
            {
                yMaxValue = (int)Math.Ceiling(yMaxValue / YIntervalCount) * YIntervalCount;
            }

            var yIntervalNumberToValueRatio = yMaxValue / YIntervalCount;

            var chartInnerHeight = yAxisLine.Y2 - yAxisLine.Y1;
            var yIntervalNumberToPositionRatio = chartInnerHeight / YIntervalCount;

            for (var currentYIntervalNumber = 1; currentYIntervalNumber <= YIntervalCount; currentYIntervalNumber++)
            {
                var currentYPosition = origin.Y - currentYIntervalNumber * yIntervalNumberToPositionRatio;
                var line = new Line
                {
                    X1 = xAxisLine.X1,
                    Y1 = currentYPosition,
                    X2 = xAxisLine.X2,
                    Y2 = currentYPosition,
                    Stroke = GridLineStrokeBrush,
                    StrokeThickness = GridLineStrokeThickness,
                    Opacity = GridLineOpacity
                };
                ChartCanvas.Children.Add(line);

                var textBlock = new TextBlock { Text = $"{currentYIntervalNumber * yIntervalNumberToValueRatio}" };
                ChartCanvas.Children.Add(textBlock);

                var textBlockEstimatedSize = EstimateSize(textBlock);
                Canvas.SetLeft(textBlock, line.X1 - textBlockEstimatedSize.Width - Y_AXIS_TEXT_BLOCK_RIGHT_MARGIN);
                Canvas.SetTop(textBlock, currentYPosition - textBlockEstimatedSize.Height / 2);
            }

            var xValueToPositionRatio = chartInnerWidth / xMaxValue;
            var yValueToPositionRatio = chartInnerHeight / yMaxValue;
            var chartPolyline = new Polyline
            {
                Stroke = ValueLineStrokeBrush,
                StrokeThickness = ValueLineStrokeThickness,
                Points = new PointCollection(Values.Select(value =>
                    new Point
                    {
                        X = origin.X + value.X * xValueToPositionRatio,
                        Y = origin.Y - value.Y * yValueToPositionRatio
                    })
                )
            };
            ChartCanvas.Children.Add(chartPolyline);
        }

        private static Size EstimateSize(TextBlock textBlock)
        {
            var formattedText = new FormattedText(
                textBlock.Text,
                CultureInfo.CurrentCulture,
                textBlock.FlowDirection,
                new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                textBlock.FontSize,
                textBlock.Foreground);

            return new Size(formattedText.Width, formattedText.Height);
        }
    }
}