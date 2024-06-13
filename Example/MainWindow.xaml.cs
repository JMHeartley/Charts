using System.Windows;
using WPFChartControls.Models;

namespace Example
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ColumnChart.Items = TestColumnItems.Case4;
        }
    }
}