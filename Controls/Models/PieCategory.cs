using System.Windows.Media;

namespace WPFChartControls.Models
{
    /// <summary>
    ///     Represents a category in a pie chart with a percentage, title, and color brush.
    /// </summary>
    public class PieCategory
    {
        /// <summary>
        ///     Gets or sets the percentage of the pie category.
        /// </summary>
        public float Percentage { get; set; }

        /// <summary>
        ///     Gets or sets the title of the pie category.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the color brush of the pie category.
        /// </summary>
        public Brush ColorBrush { get; set; }
    }
}