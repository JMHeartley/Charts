using System.Collections.Generic;

namespace WPFChartControls.Models
{
    /// <summary>
    ///     Provides test cases of <see cref="LineValue" /> instances.
    /// </summary>
    public static class TestLineValues
    {
        public static List<LineValue> Case1 = new List<LineValue>
        {
            new LineValue(x: 0, y: 0),
            new LineValue(x: 100, y: 100),
            new LineValue(x: 200, y: 200),
            new LineValue(x: 300, y: 300),
            new LineValue(x: 400, y: 200),
            new LineValue(x: 500, y: 500),
            new LineValue(x: 600, y: 500),
            new LineValue(x: 700, y: 500),
            new LineValue(x: 800, y: 500),
            new LineValue(x: 900, y: 600),
            new LineValue(x: 1000, y: 200),
            new LineValue(x: 1100, y: 100),
            new LineValue(x: 1200, y: 400)
        };

        public static List<LineValue> Case2 = new List<LineValue>
        {
            new LineValue(x: 0, y: 0),
            new LineValue(x: 100, y: 200),
            new LineValue(x: 200, y: 100),
            new LineValue(x: 300, y: 200),
            new LineValue(x: 400, y: 300),
            new LineValue(x: 500, y: 400),
            new LineValue(x: 600, y: 500),
            new LineValue(x: 700, y: 400),
            new LineValue(x: 800, y: 500),
            new LineValue(x: 900, y: 600),
            new LineValue(x: 1000, y: 300),
            new LineValue(x: 1100, y: 100),
            new LineValue(x: 1200, y: 400)
        };

        public static List<LineValue> Case3 = new List<LineValue>
        {
            new LineValue(x: 0, y: 0),
            new LineValue(x: 100, y: 100),
            new LineValue(x: 200, y: 400),
            new LineValue(x: 300, y: 200),
            new LineValue(x: 400, y: 400),
            new LineValue(x: 500, y: 300),
            new LineValue(x: 600, y: 100),
            new LineValue(x: 700, y: 700),
            new LineValue(x: 800, y: 200),
            new LineValue(x: 900, y: 600),
            new LineValue(x: 1000, y: 600),
            new LineValue(x: 1100, y: 0),
            new LineValue(x: 1200, y: 100),
            new LineValue(x: 1300, y: 100)
        };
    }
}