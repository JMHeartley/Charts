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
    ///     Interaction logic for _2DColumnChart.xaml
    /// </summary>
    public partial class _2DColumnChart : UserControl
    {
        private const int X_AXIS_TEXT_BLOCK_TOP_MARGIN = 5;

        private const int Y_AXIS_TEXT_BLOCK_RIGHT_MARGIN = 10;

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(List<ColumnItem>), typeof(_2DColumnChart));

        public static readonly DependencyProperty ColumnBrushProperty =
            DependencyProperty.Register(nameof(ColumnBrush), typeof(SolidColorBrush), typeof(_2DColumnChart));

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(SolidColorBrush), typeof(_2DColumnChart));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(_2DColumnChart));

        public static readonly DependencyProperty IntervalCountProperty =
            DependencyProperty.Register(nameof(IntervalCount), typeof(int), typeof(_2DColumnChart));

        public _2DColumnChart()
        {
            ColumnBrush = Brushes.Gold;
            Stroke = Brushes.LightGray;
            StrokeThickness = 1;
            IntervalCount = 8;

            InitializeComponent();
        }

        public List<ColumnItem> Items
        {
            get => (List<ColumnItem>)GetValue(ItemsProperty);
            set
            {
                SetValue(ItemsProperty, value);

                if (Items is null || !Items.Any())
                {
                    return;
                }

                Paint();
            }
        }

        public SolidColorBrush ColumnBrush
        {
            get => (SolidColorBrush)GetValue(ColumnBrushProperty);
            set => SetValue(ColumnBrushProperty, value);
        }

        public SolidColorBrush Stroke
        {
            get => (SolidColorBrush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public int IntervalCount
        {
            get => (int)GetValue(IntervalCountProperty);
            set => SetValue(IntervalCountProperty, value);
        }

        private void Paint()
        {
            float chartWidth = 1200;
            float chartHeight = 700;
            float chartPadding = 100;
            var chartInnerHeight = chartHeight - chartPadding * 2;
            const float originalBlockWidthRatio = 0.583333f;
            var blockWidth = chartWidth / Items.Count * originalBlockWidthRatio;
            var blockMargin = (chartWidth / Items.Count - blockWidth) / 2;
            MainCanvas.Width = chartWidth;
            MainCanvas.Height = chartHeight;

            var yAxisEndPoint = new Point(chartPadding, chartPadding);
            var origin = new Point(chartPadding, chartHeight - chartPadding);
            var xAxisEndPoint = new Point(chartWidth - chartPadding, chartHeight - chartPadding);

            var yAxisStartLine = new Line
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
                X1 = yAxisEndPoint.X,
                Y1 = yAxisEndPoint.Y,
                X2 = origin.X,
                Y2 = origin.Y
            };
            MainCanvas.Children.Add(yAxisStartLine);

            var yAxisEndLine = new Line
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
                X1 = xAxisEndPoint.X,
                Y1 = xAxisEndPoint.Y,
                X2 = xAxisEndPoint.X,
                Y2 = yAxisEndPoint.Y
            };
            MainCanvas.Children.Add(yAxisEndLine);

            double yValue = 0;
            var currentYValue = origin.Y;
            while (currentYValue >= yAxisEndPoint.Y)
            {
                var yLine = new Line
                {
                    Stroke = Stroke,
                    StrokeThickness = StrokeThickness,
                    X1 = origin.X,
                    Y1 = currentYValue,
                    X2 = xAxisEndPoint.X,
                    Y2 = currentYValue
                };
                MainCanvas.Children.Add(yLine);

                var yAxisTextBlock = new TextBlock
                {
                    Text = $"{yValue}",
                    Foreground = Foreground,
                    FontSize = FontSize,
                    Width = 30,
                    TextAlignment = TextAlignment.Right
                };
                MainCanvas.Children.Add(yAxisTextBlock);

                Canvas.SetLeft(yAxisTextBlock, origin.X - yAxisTextBlock.Width - Y_AXIS_TEXT_BLOCK_RIGHT_MARGIN);
                Canvas.SetTop(yAxisTextBlock, currentYValue - 12.5);

                var intervalYValue = chartInnerHeight / IntervalCount;
                var intervalValue = Items.Max(item => item.Value) / IntervalCount;
                currentYValue -= intervalYValue;
                yValue += intervalValue;
            }

            var heightValueScale = chartInnerHeight / Items.Max(item => item.Value);
            var currentXValue = origin.X + blockMargin;
            foreach (var item in Items)
            {
                var block = new Rectangle
                {
                    Fill = ColumnBrush,
                    Width = blockWidth,
                    Height = heightValueScale * item.Value
                };

                MainCanvas.Children.Add(block);
                Canvas.SetLeft(block, currentXValue);
                Canvas.SetTop(block, origin.Y - block.Height);

                var blockHeader = new TextBlock
                {
                    Text = item.Header,
                    FontSize = FontSize,
                    Foreground = Foreground,
                    TextAlignment = TextAlignment.Center,
                    Width = block.Width,
                    TextWrapping = TextWrapping.Wrap
                };
                MainCanvas.Children.Add(blockHeader);
                Canvas.SetLeft(blockHeader, currentXValue);
                Canvas.SetTop(blockHeader, origin.Y + X_AXIS_TEXT_BLOCK_TOP_MARGIN);

                currentXValue += block.Width + blockMargin;
            }
        }
    }
}