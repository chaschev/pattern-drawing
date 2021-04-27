using cAlgo.API;

namespace cAlgo.Patterns
{
    public class FibonacciRetracementLevel
    {
        public double Percent { get; set; }

        public Color Color { get; set; }

        public int ColorAlpha { get; set; }

        public Color FillColor
        {
            get
            {
                return Color.FromArgb(ColorAlpha, Color.R, Color.G, Color.B);
            }
        }

        public LineStyle Style { get; set; }

        public int Thickness { get; set; }
    }
}