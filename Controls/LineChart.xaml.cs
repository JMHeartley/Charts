using Controls.Models;
using System.Collections.Generic;
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

        private readonly List<LineHolder> holders;
        private readonly double interval = 100;
        private readonly List<LineValue> values;
        private Polyline chartPolyline;

        private Point origin;
        private Line xAxisLine, yAxisLine;

        public LineChart()
        {
            AxisStrokeBrush = Brushes.LightGray;
            AxisStrokeThickness = 1;
            GridLineStrokeBrush = Brushes.LightGray;
            GridLineStrokeThickness = 10;
            GridLineOpacity = 1;
            InnerPadding = new Thickness(100);

            InitializeComponent();

            holders = new List<LineHolder>();
            values = TestLineValues.Case3;

            Paint();

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

        public void Paint()
        {
            if (ActualWidth <= 0 || ActualHeight <= 0)
            {
                return;
            }

            ChartCanvas.Children.Clear();
            holders.Clear();

            // axis lines
            xAxisLine = new Line
            {
                X1 = InnerPadding.Left,
                Y1 = ActualHeight - InnerPadding.Bottom,
                X2 = ActualWidth - InnerPadding.Right,
                Y2 = ActualHeight - InnerPadding.Bottom,
                Stroke = AxisStrokeBrush,
                StrokeThickness = AxisStrokeThickness
            };
            yAxisLine = new Line
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

            origin = new Point(xAxisLine.X1, yAxisLine.Y2);

            var xTextBlock0 = new TextBlock { Text = "0" };
            ChartCanvas.Children.Add(xTextBlock0);
            Canvas.SetLeft(xTextBlock0, origin.X);
            Canvas.SetTop(xTextBlock0, origin.Y + 5);

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
                Canvas.SetLeft(textBlock, xPoint - 12.5);
                Canvas.SetTop(textBlock, line.Y2 + 5);

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
            double x = 0, y = 0;
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
            chartPolyline = new Polyline
            {
                Stroke = new SolidColorBrush(Color.FromRgb(r: 68, g: 114, b: 196)),
                StrokeThickness = 10
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
                Canvas.SetLeft(oEllipse, holder.Point.X - 5);
                Canvas.SetTop(oEllipse, holder.Point.Y - 5);
            }

            // add connection points to polyline
            foreach (var value in values)
            {
                var holder = holders.FirstOrDefault(h => h.X == value.X && h.Y == value.Y);
                if (holder != null)
                {
                    chartPolyline.Points.Add(holder.Point);
                }
            }
        }
    }
}