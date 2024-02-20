using Controls.Models;
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

            PieChart.Categories = TestPieCategories.Case6;
        }
    }
}