namespace WPFChartControls.Models
{
    /// <summary>
    ///     Represents a value in a line chart with X and Y coordinates.
    /// </summary>
    public class LineValue
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LineValue" /> class.
        /// </summary>
        /// <param name="x">The X coordinate of the value.</param>
        /// <param name="y">The Y coordinate of the value.</param>
        public LineValue(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///     Gets or sets the X coordinate of the value.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        ///     Gets or sets the Y coordinate of the value.
        /// </summary>
        public double Y { get; set; }
    }
}