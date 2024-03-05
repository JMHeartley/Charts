namespace WPFChartControls.Models
{
    public class LineValue
    {
        public LineValue(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}