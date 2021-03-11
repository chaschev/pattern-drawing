using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class ElliottCorrectionWavePattern : ElliottWavePatternBase
    {
        public ElliottCorrectionWavePattern(PatternConfig config) : base("Elliott Correction Wave (ABC)", config, 3)
        {
        }

        protected override void DrawLabels()
        {
            if (FirstLine == null || SecondLine == null || ThirdLine == null) return;

            DrawLabelText("(0)", FirstLine.Time1, FirstLine.Y1);
            DrawLabelText("(A)", SecondLine.Time1, SecondLine.Y1);
            DrawLabelText("(B)", ThirdLine.Time1, ThirdLine.Y1);
            DrawLabelText("(C)", ThirdLine.Time2, ThirdLine.Y2);
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var firstLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FirstLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var secondLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("SecondLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var thirdLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("ThirdLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            if (firstLine == null || secondLine == null || thirdLine == null) return;

            foreach (var label in labels)
            {
                switch (label.Text)
                {
                    case "(0)":
                        label.Time = firstLine.Time1;
                        label.Y = firstLine.Y1;
                        break;

                    case "(A)":
                        label.Time = secondLine.Time1;
                        label.Y = secondLine.Y1;
                        break;

                    case "(B)":
                        label.Time = thirdLine.Time1;
                        label.Y = thirdLine.Y1;
                        break;

                    case "(C)":
                        label.Time = thirdLine.Time2;
                        label.Y = thirdLine.Y2;
                        break;
                }
            }
        }
    }
}