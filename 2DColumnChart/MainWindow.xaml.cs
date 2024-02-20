using Controls.Models;
using System.Windows;

namespace _2DColumnChart
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