using Charts._2DPie;
using System.Collections.Generic;
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

            Categories = TestCategories.Case6;

            detailsItemsControl.ItemsSource = Categories;

            var _2dPie = new _2DPie();
            var uiElements = _2dPie.Create(pieWidth, pieHeight, Categories);
            foreach (var uiElement in uiElements)
            {
                mainCanvas.Children.Add(uiElement);
            }
        }

        private List<Category> Categories { get; }
    }
}