using Controls.Models;
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

        private readonly List<LineHolder> holders = new List<LineHolder>();
        private readonly double interval = 100;

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
            holders.Clear();

            if (ActualWidth <= 0
                || ActualHeight <= 0
                || Values is null
                || !Values.Any())
            {
                return;
            }


            // axis lines
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
                Y1 = InnerPadding.Top - 50,
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
            Canvas.SetLeft(xTextBlock0, origin.X);
            Canvas.SetTop(xTextBlock0, origin.Y + X_AXIS_TEXT_BLOCK_TOP_MARGIN);

            // y axis lines
            var xValue = InnerPadding.Left;
            var xPoint = origin.X + interval;
            while (xPoint < xAxisLine.X2)
            {
                var line = new Line
                {
                    X1 = xPoint,
                    Y1 = yAxisLine.Y1,
                    X2 = xPoint,
                    Y2 = yAxisLine.Y2,
                    Stroke = GridLineStrokeBrush,
                    StrokeThickness = GridLineStrokeThickness,
                    Opacity = GridLineOpacity
                };
                ChartCanvas.Children.Add(line);

                var textBlock = new TextBlock { Text = $"{xValue}" };
                ChartCanvas.Children.Add(textBlock);

                var textBlockEstimatedSize = EstimateSize(textBlock);
                Canvas.SetLeft(textBlock, xPoint - textBlockEstimatedSize.Width / 2);
                Canvas.SetTop(textBlock, line.Y2 + X_AXIS_TEXT_BLOCK_TOP_MARGIN);

                xPoint += interval;
                xValue += interval;
            }


            var yTextBlock0 = new TextBlock { Text = "0" };
            ChartCanvas.Children.Add(yTextBlock0);
            Canvas.SetLeft(yTextBlock0, origin.X - 20);
            Canvas.SetTop(yTextBlock0, origin.Y - 10);

            // x axis lines
            var yValue = InnerPadding.Top;
            var yPoint = origin.Y - interval;
            while (yPoint > yAxisLine.Y1)
            {
                var line = new Line
                {
                    X1 = xAxisLine.X1,
                    Y1 = yPoint,
                    X2 = xAxisLine.X2,
                    Y2 = yPoint,
                    Stroke = GridLineStrokeBrush,
                    StrokeThickness = GridLineStrokeThickness,
                    Opacity = GridLineOpacity
                };

                ChartCanvas.Children.Add(line);

                var textBlock = new TextBlock { Text = $"{yValue}" };
                ChartCanvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, line.X1 - 30);
                Canvas.SetTop(textBlock, yPoint - 10);

                yPoint -= interval;
                yValue += interval;
            }

            // connections
            var x = 0d;
            var y = 0d;
            xPoint = origin.X;
            yPoint = origin.Y;
            while (xPoint < xAxisLine.X2)
            {
                while (yPoint > yAxisLine.Y1)
                {
                    var holder = new LineHolder
                    {
                        X = x,
                        Y = y,
                        Point = new Point(xPoint, yPoint)
                    };

                    holders.Add(holder);

                    yPoint -= interval;
                    y += interval;
                }

                xPoint += interval;
                yPoint = origin.Y;
                x += 100;
                y = 0;
            }

            // polyline
            var chartPolyline = new Polyline
            {
                Stroke = ValueLineStrokeBrush,
                StrokeThickness = ValueLineStrokeThickness
            };
            ChartCanvas.Children.Add(chartPolyline);

            // showing where are the connections points
            foreach (var holder in holders)
            {
                var oEllipse = new Ellipse
                {
                    Fill = Brushes.Red,
                    Width = 10,
                    Height = 10,
                    Opacity = 0
                };

                ChartCanvas.Children.Add(oEllipse);
                Canvas.SetLeft(oEllipse, holder.Point.X - oEllipse.Width / 2);
                Canvas.SetTop(oEllipse, holder.Point.Y - oEllipse.Height / 2);
            }

            // add connection points to polyline
            foreach (var value in Values)
            {
                var holder = holders.FirstOrDefault(h => h.X == value.X && h.Y == value.Y);
                if (holder != default)
                {
                    chartPolyline.Points.Add(holder.Point);
                }
            }
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