using Controls.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LineChart
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<LineHolder> holders;
        private readonly double interval = 100;
        private readonly List<LineValue> values;
        private readonly double xAxisStart = 100;
        private readonly double yAxisStart = 100;
        private Polyline chartPolyline;

        private Point origin;
        private Line xAxisLine, yAxisLine;


        public MainWindow()
        {
            InitializeComponent();

            holders = new List<LineHolder>();
            values = new List<LineValue>
            {
                //new LineValue(0,0),
                //new LineValue(100,100),
                //new LineValue(200,200),
                //new LineValue(300,300),
                //new LineValue(400,200),
                //new LineValue(500,500),
                //new LineValue(600,500),
                //new LineValue(700,500),
                //new LineValue(800,500),
                //new LineValue(900,600),
                //new LineValue(1000,200),
                //new LineValue(1100,100),
                //new LineValue(1200,400),

                //new LineValue(0,0),
                //new LineValue(100,200),
                //new LineValue(200,100),
                //new LineValue(300,200),
                //new LineValue(400,300),
                //new LineValue(500,400),
                //new LineValue(600,500),
                //new LineValue(700,400),
                //new LineValue(800,500),
                //new LineValue(900,600),
                //new LineValue(1000,300),
                //new LineValue(1100,100),
                //new LineValue(1200,400),

                new LineValue(x: 0, y: 0),
                new LineValue(x: 100, y: 100),
                new LineValue(x: 200, y: 400),
                new LineValue(x: 300, y: 200),
                new LineValue(x: 400, y: 400),
                new LineValue(x: 500, y: 300),
                new LineValue(x: 600, y: 100),
                new LineValue(x: 700, y: 700),
                new LineValue(x: 800, y: 200),
                new LineValue(x: 900, y: 600),
                new LineValue(x: 1000, y: 600),
                new LineValue(x: 1100, y: 0),
                new LineValue(x: 1200, y: 100),
                new LineValue(x: 1300, y: 100)
            };

            Paint();

            StateChanged += (sender, e) => Paint();
            SizeChanged += (sender, e) => Paint();
        }


        public void Paint()
        {
            if (ActualWidth > 0 && ActualHeight > 0)
            {
                chartCanvas.Children.Clear();
                holders.Clear();

                // axis lines
                xAxisLine = new Line
                {
                    X1 = xAxisStart,
                    Y1 = ActualHeight - yAxisStart,
                    X2 = ActualWidth - xAxisStart,
                    Y2 = ActualHeight - yAxisStart,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 1
                };
                yAxisLine = new Line
                {
                    X1 = xAxisStart,
                    Y1 = yAxisStart - 50,
                    X2 = xAxisStart,
                    Y2 = ActualHeight - yAxisStart,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 1
                };

                chartCanvas.Children.Add(xAxisLine);
                chartCanvas.Children.Add(yAxisLine);

                origin = new Point(xAxisLine.X1, yAxisLine.Y2);

                var xTextBlock0 = new TextBlock { Text = $"{0}" };
                chartCanvas.Children.Add(xTextBlock0);
                Canvas.SetLeft(xTextBlock0, origin.X);
                Canvas.SetTop(xTextBlock0, origin.Y + 5);

                // y axis lines
                var xValue = xAxisStart;
                var xPoint = origin.X + interval;
                while (xPoint < xAxisLine.X2)
                {
                    var line = new Line
                    {
                        X1 = xPoint,
                        Y1 = yAxisStart - 50,
                        X2 = xPoint,
                        Y2 = ActualHeight - yAxisStart,
                        Stroke = Brushes.LightGray,
                        StrokeThickness = 10,
                        Opacity = 1
                    };

                    chartCanvas.Children.Add(line);

                    var textBlock = new TextBlock { Text = $"{xValue}" };

                    chartCanvas.Children.Add(textBlock);
                    Canvas.SetLeft(textBlock, xPoint - 12.5);
                    Canvas.SetTop(textBlock, line.Y2 + 5);

                    xPoint += interval;
                    xValue += interval;
                }


                var yTextBlock0 = new TextBlock { Text = $"{0}" };
                chartCanvas.Children.Add(yTextBlock0);
                Canvas.SetLeft(yTextBlock0, origin.X - 20);
                Canvas.SetTop(yTextBlock0, origin.Y - 10);

                // x axis lines
                var yValue = yAxisStart;
                var yPoint = origin.Y - interval;
                while (yPoint > yAxisLine.Y1)
                {
                    var line = new Line
                    {
                        X1 = xAxisStart,
                        Y1 = yPoint,
                        X2 = ActualWidth - xAxisStart,
                        Y2 = yPoint,
                        Stroke = Brushes.LightGray,
                        StrokeThickness = 10,
                        Opacity = 1
                    };

                    chartCanvas.Children.Add(line);

                    var textBlock = new TextBlock { Text = $"{yValue}" };
                    chartCanvas.Children.Add(textBlock);
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
                chartCanvas.Children.Add(chartPolyline);

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

                    chartCanvas.Children.Add(oEllipse);
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
}