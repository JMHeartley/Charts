using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFChartControls.Models;

namespace WPFChartControls
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

        public static readonly DependencyProperty XIntervalCountProperty =
            DependencyProperty.Register(nameof(XIntervalCount), typeof(int), typeof(LineChart));

        public static readonly DependencyProperty YIntervalCountProperty =
            DependencyProperty.Register(nameof(YIntervalCount), typeof(int), typeof(LineChart));

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
            XIntervalCount = 12;
            YIntervalCount = 12;

            InitializeComponent();

            SizeChanged += (sender, e) => Paint();
        }

        /// <summary>
        ///     Gets or sets the collection of line values to be displayed in the chart.
        /// </summary>
        public ICollection<LineValue> Values
        {
            get => (ICollection<LineValue>)GetValue(ValuesProperty);
            set => SetValue(ValuesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the brush used to draw the axis strokes.
        /// </summary>
        public SolidColorBrush AxisStrokeBrush
        {
            get => (SolidColorBrush)GetValue(AxisStrokeBrushProperty);
            set => SetValue(AxisStrokeBrushProperty, value);
        }

        /// <summary>
        ///     Gets or sets the thickness of the axis strokes.
        /// </summary>
        public double AxisStrokeThickness
        {
            get => (double)GetValue(AxisStrokeThicknessProperty);
            set => SetValue(AxisStrokeThicknessProperty, value);
        }

        /// <summary>
        ///     Gets or sets the brush used to draw the grid lines.
        /// </summary>
        public SolidColorBrush GridLineStrokeBrush
        {
            get => (SolidColorBrush)GetValue(GridLineStrokeBrushProperty);
            set => SetValue(GridLineStrokeBrushProperty, value);
        }

        /// <summary>
        ///     Gets or sets the thickness of the grid lines.
        /// </summary>
        public double GridLineStrokeThickness
        {
            get => (double)GetValue(GridLineStrokeThicknessProperty);
            set => SetValue(GridLineStrokeThicknessProperty, value);
        }

        /// <summary>
        ///     Gets or sets the opacity of the grid lines.
        /// </summary>
        public double GridLineOpacity
        {
            get => (double)GetValue(GridLineOpacityProperty);
            set => SetValue(GridLineOpacityProperty, value);
        }

        /// <summary>
        ///     Gets or sets the inner padding of the chart area.
        /// </summary>
        public Thickness InnerPadding
        {
            get => (Thickness)GetValue(InnerPaddingProperty);
            set => SetValue(InnerPaddingProperty, value);
        }

        /// <summary>
        ///     Gets or sets the brush used to draw the value line.
        /// </summary>
        public SolidColorBrush ValueLineStrokeBrush
        {
            get => (SolidColorBrush)GetValue(ValueLineStrokeBrushProperty);
            set => SetValue(ValueLineStrokeBrushProperty, value);
        }

        /// <summary>
        ///     Gets or sets the thickness of the value line.
        /// </summary>
        public double ValueLineStrokeThickness
        {
            get => (double)GetValue(ValueLineStrokeThicknessProperty);
            set => SetValue(ValueLineStrokeThicknessProperty, value);
        }

        /// <summary>
        ///     Gets or sets the number of intervals to be displayed on the X-axis.
        /// </summary>
        public int XIntervalCount
        {
            get => (int)GetValue(XIntervalCountProperty);
            set => SetValue(XIntervalCountProperty, value);
        }

        /// <summary>
        ///     Gets or sets the number of intervals to be displayed on the Y-axis.
        /// </summary>
        public int YIntervalCount
        {
            get => (int)GetValue(YIntervalCountProperty);
            set => SetValue(YIntervalCountProperty, value);
        }

        /// <summary>
        ///     Paints the line chart on the canvas.
        /// </summary>
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

            var origin = new Point(InnerPadding.Left, ActualHeight - InnerPadding.Bottom);

            var xMaxValue = Values.Max(value => value.X);
            if (xMaxValue % XIntervalCount != 0)
            {
                xMaxValue = (int)Math.Ceiling(xMaxValue / XIntervalCount) * XIntervalCount;
            }

            var xIntervalNumberToValueRatio = xMaxValue / XIntervalCount;

            var chartInnerWidth = ActualWidth - InnerPadding.Left - InnerPadding.Right;
            var xIntervalNumberToPositionRatio = chartInnerWidth / XIntervalCount;

            for (var currentXIntervalNumber = 0; currentXIntervalNumber <= XIntervalCount; currentXIntervalNumber++)
            {
                var currentXPosition = origin.X + currentXIntervalNumber * xIntervalNumberToPositionRatio;
                var isChartBorder = currentXIntervalNumber == 0 || currentXIntervalNumber == XIntervalCount;

                var line = new Line
                {
                    X1 = currentXPosition,
                    Y1 = InnerPadding.Top,
                    X2 = currentXPosition,
                    Y2 = origin.Y,
                    Stroke = isChartBorder ? AxisStrokeBrush : GridLineStrokeBrush,
                    StrokeThickness = isChartBorder ? AxisStrokeThickness : GridLineStrokeThickness,
                    Opacity = GridLineOpacity
                };
                ChartCanvas.Children.Add(line);

                var textBlock = new TextBlock { Text = $"{currentXIntervalNumber * xIntervalNumberToValueRatio}" };
                ChartCanvas.Children.Add(textBlock);

                var textBlockEstimatedSize = EstimateSize(textBlock);
                Canvas.SetLeft(textBlock, currentXPosition - textBlockEstimatedSize.Width / 2);
                Canvas.SetTop(textBlock, line.Y2 + X_AXIS_TEXT_BLOCK_TOP_MARGIN);
            }

            var yMaxValue = Values.Max(value => value.Y);
            if (yMaxValue % YIntervalCount != 0)
            {
                yMaxValue = (int)Math.Ceiling(yMaxValue / YIntervalCount) * YIntervalCount;
            }

            var yIntervalNumberToValueRatio = yMaxValue / YIntervalCount;

            var chartInnerHeight = ActualHeight - InnerPadding.Top - InnerPadding.Bottom;
            var yIntervalNumberToPositionRatio = chartInnerHeight / YIntervalCount;

            for (var currentYIntervalNumber = 0; currentYIntervalNumber <= YIntervalCount; currentYIntervalNumber++)
            {
                var currentYPosition = origin.Y - currentYIntervalNumber * yIntervalNumberToPositionRatio;
                var isChartBorder = currentYIntervalNumber == 0 || currentYIntervalNumber == YIntervalCount;

                var line = new Line
                {
                    X1 = origin.X,
                    Y1 = currentYPosition,
                    X2 = ActualWidth - InnerPadding.Right,
                    Y2 = currentYPosition,
                    Stroke = isChartBorder ? AxisStrokeBrush : GridLineStrokeBrush,
                    StrokeThickness = isChartBorder ? AxisStrokeThickness : GridLineStrokeThickness,
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
            var valueLine = new Polyline
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
            ChartCanvas.Children.Add(valueLine);
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