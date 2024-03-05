using Controls.Models;
using System.Windows;

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