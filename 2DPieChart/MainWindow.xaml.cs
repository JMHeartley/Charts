using System.Windows;
using WPFChartControls.Models;

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

            PieChart.Categories = TestPieCategories.Case6;
        }
    }
}