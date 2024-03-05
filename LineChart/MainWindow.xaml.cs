using System.Windows;
using WPFChartControls.Models;

namespace LineChart
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LineChart.Values = TestLineValues.Case3;
        }
    }
}