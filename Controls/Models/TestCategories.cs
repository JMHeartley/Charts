using System.Collections.Generic;
using System.Windows.Media;

namespace Controls.Models
{
    public static class TestCategories
    {
        public static List<Category> Case1 = new List<Category>
        {
            new Category
            {
                Title = "Category#01",
                Percentage = 10,
                ColorBrush = Brushes.Gold
            },

            new Category
            {
                Title = "Category#02",
                Percentage = 30,
                ColorBrush = Brushes.Pink
            },

            new Category
            {
                Title = "Category#03",
                Percentage = 60,
                ColorBrush = Brushes.CadetBlue
            }
        };

        public static List<Category> Case2 = new List<Category>
        {
            new Category
            {
                Title = "Category#01",
                Percentage = 20,
                ColorBrush = Brushes.Gold
            },

            new Category
            {
                Title = "Category#02",
                Percentage = 80,
                ColorBrush = Brushes.LightBlue
            }
        };

        public static List<Category> Case3 = new List<Category>
        {
            new Category
            {
                Title = "Category#01",
                Percentage = 50,
                ColorBrush = Brushes.Gold
            },

            new Category
            {
                Title = "Category#02",
                Percentage = 50,
                ColorBrush = Brushes.LightBlue
            }
        };

        public static List<Category> Case4 = new List<Category>
        {
            new Category
            {
                Title = "Category#01",
                Percentage = 30,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4472C4"))
            },

            new Category
            {
                Title = "Category#02",
                Percentage = 30,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED7D31"))
            },

            new Category
            {
                Title = "Category#03",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000"))
            },

            new Category
            {
                Title = "Category#04",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5B9BD5"))
            },

            new Category
            {
                Title = "Category#05",
                Percentage = 10,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5A5A5"))
            }
        };

        public static List<Category> Case5 = new List<Category>
        {
            new Category
            {
                Title = "Category#01",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4472C4"))
            },

            new Category
            {
                Title = "Category#02",
                Percentage = 30,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED7D31"))
            },

            new Category
            {
                Title = "Category#03",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000"))
            },

            new Category
            {
                Title = "Category#04",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5B9BD5"))
            },

            new Category
            {
                Title = "Category#05",
                Percentage = 10,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5A5A5"))
            }
        };

        public static List<Category> Case6 = new List<Category>
        {
            new Category
            {
                Title = "Category#01",
                Percentage = 20,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4472C4"))
            },

            new Category
            {
                Title = "Category#02",
                Percentage = 60,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED7D31"))
            },

            new Category
            {
                Title = "Category#03",
                Percentage = 5,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000"))
            },

            new Category
            {
                Title = "Category#04",
                Percentage = 10,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5B9BD5"))
            },

            new Category
            {
                Title = "Category#05",
                Percentage = 5,
                ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5A5A5"))
            }
        };
    }
}