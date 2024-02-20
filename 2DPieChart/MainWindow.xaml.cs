using Charts._2DPie;
using System.Windows;

namespace _2DPieChart
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            const float pieWidth = 650;
            const float pieHeight = 650;

            mainCanvas.Width = pieWidth;
            mainCanvas.Height = pieHeight;

            var categories = TestCategories.Case6;

            DataContext = categories;

            var _2dPie = new _2DPie();
            var uiElements = _2dPie.Create(pieWidth, pieHeight, categories);
            foreach (var uiElement in uiElements)
            {
                mainCanvas.Children.Add(uiElement);
            }
        }
    }
}