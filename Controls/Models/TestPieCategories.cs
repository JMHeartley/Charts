using System.Collections.Generic;
using System.Windows.Media;

namespace WPFChartControls.Models
{
    /// <summary>
    ///     Provides test cases of <see cref="PieCategory" /> instances.
    /// </summary>
    public static class TestPieCategories
    {
        public static List<PieCategory> Case1 = new List<PieCategory>
        {
            new PieCategory
            {
                Title = "Category#01",
                Percentage = 10,
                ColorBrush = Brushes.Gold
            },

            new PieCategory
            {
                Title = "Category#02",
                Percentage = 30,
                ColorBrush = Brushes.Pink
            },

            new PieCategory
            {
                Title = "Category#03",
                Percentage = 60,
                ColorBrush = Brushes.CadetBlue
            }
        };

        public static List<PieCategory> Case2 = new List<PieCategory>
        {
            new PieCategory
            {
                Title = "Category#01",
                Percentage = 0,
                ColorBrush = Brushes.Gold
            },

            new PieCategory
            {
                Title = "Category#02",
                Percentage = 100,
                ColorBrush = Brushes.LightBlue
            }
        };

        public static List<PieCategory> Case3 = new List<PieCategory>
        {
            new PieCategory
            {
                Title = "Category#01",
                Percentage = 50,
                ColorBrush = Brushes.Gold
            },

            new PieCategory
            {
                Title = "Category#02",
                Percentage = 50,
                ColorBrush = Brushes.LightBlue
            }
        };

        public static List<PieCategory> Case4 = new List<PieCategory>
        {
            new PieCategory
            {
                Title = "Category#01",
                Percentage = 30,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4472C4"))
            },

            new PieCategory
            {
                Title = "Category#02",
                Percentage = 30,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED7D31"))
            },

            new PieCategory
            {
                Title = "Category#03",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000"))
            },

            new PieCategory
            {
                Title = "Category#04",
                Percentage = 5,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5B9BD5"))
            },

            new PieCategory
            {
                Title = "Category#05",
                Percentage = 15,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5A5A5"))
            }
        };

        public static List<PieCategory> Case5 = new List<PieCategory>
        {
            new PieCategory
            {
                Title = "Category#01",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4472C4"))
            },

            new PieCategory
            {
                Title = "Category#02",
                Percentage = 30,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED7D31"))
            },

            new PieCategory
            {
                Title = "Category#03",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000"))
            },

            new PieCategory
            {
                Title = "Category#04",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5B9BD5"))
            },

            new PieCategory
            {
                Title = "Category#05",
                Percentage = 10,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5A5A5"))
            }
        };

        public static List<PieCategory> Case6 = new List<PieCategory>
        {
            new PieCategory
            {
                Title = "Category#01",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4472C4"))
            },

            new PieCategory
            {
                Title = "Category#02",
                Percentage = 60,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED7D31"))
            },

            new PieCategory
            {
                Title = "Category#03",
                Percentage = 5,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000"))
            },

            new PieCategory
            {
                Title = "Category#04",
                Percentage = 10,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5B9BD5"))
            },

            new PieCategory
            {
                Title = "Category#05",
                Percentage = 5,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5A5A5"))
            }
        };
    }
}