using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class ElliottTripleComboWavePattern : ElliottWavePatternBase
    {
        public ElliottTripleComboWavePattern(PatternConfig config) : base("Elliott Triple Combo Wave (WXYXZ)", config, 5)
        {
        }

        protected override void DrawLabels()
        {
            if (FirstLine == null || SecondLine == null || ThirdLine == null || FourthLine == null || FifthLine == null) return;

            DrawLabelText("(0)", FirstLine.Time1, FirstLine.Y1);
            DrawLabelText("(W)", SecondLine.Time1, SecondLine.Y1);
            DrawLabelText("(X)", ThirdLine.Time1, ThirdLine.Y1);
            DrawLabelText("(Y)", FourthLine.Time1, FourthLine.Y1);
            DrawLabelText("(X2)", FifthLine.Time1, FifthLine.Y1);
            DrawLabelText("(Z)", FifthLine.Time2, FifthLine.Y2);
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var firstLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FirstLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var secondLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("SecondLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var thirdLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("ThirdLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var fourthLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FourthLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var fifthLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FifthLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            if (firstLine == null || secondLine == null || thirdLine == null || fourthLine == null || fifthLine == null) return;

            foreach (var label in labels)
            {
                switch (label.Text)
                {
                    case "(0)":
                        label.Time = firstLine.Time1;
                        label.Y = firstLine.Y1;
                        break;

                    case "(W)":
                        label.Time = secondLine.Time1;
                        label.Y = secondLine.Y1;
                        break;

                    case "(X)":
                        label.Time = thirdLine.Time1;
                        label.Y = thirdLine.Y1;
                        break;

                    case "(Y)":
                        label.Time = fourthLine.Time1;
                        label.Y = fourthLine.Y1;
                        break;

                    case "(X2)":
                        label.Time = fifthLine.Time1;
                        label.Y = fifthLine.Y1;
                        break;

                    case "(Z)":
                        label.Time = fifthLine.Time2;
                        label.Y = fifthLine.Y2;
                        break;
                }
            }
        }
    }
}