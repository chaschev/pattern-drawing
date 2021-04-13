using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class ElliottCorrectionWavePattern : ElliottWavePatternBase
    {
        public ElliottCorrectionWavePattern(PatternConfig config, ElliottWaveDegree degree) : base("EW ABC", config, 3, degree)
        {
        }

        protected override void DrawLabels()
        {
            if (FirstLine == null || SecondLine == null || ThirdLine == null) return;

            DrawLabels(FirstLine, SecondLine, ThirdLine, Id);
        }

        private void DrawLabels(ChartTrendLine firstLine, ChartTrendLine secondLine, ChartTrendLine thirdLine, long id)
        {
            switch (Degree)
            {
                case ElliottWaveDegree.SuperMellennium:
                    DrawLabelText("((0))", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("((A))", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("((B))", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("((C))", thirdLine.Time2, thirdLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.Mellennium:
                    DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("(A)", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("(B)", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("(C)", thirdLine.Time2, thirdLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.SubMellennium:
                    DrawLabelText("0", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("A", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("B", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("C", thirdLine.Time2, thirdLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.GrandSuperCycle:
                    DrawLabelText("((0))", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("((a))", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("((b))", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("((c))", thirdLine.Time2, thirdLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.SuperCycle:
                    DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("(a)", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("(b)", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("(c)", thirdLine.Time2, thirdLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.Cycle:
                    DrawLabelText("0", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("a", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("b", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("c", thirdLine.Time2, thirdLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.Primary:
                    DrawLabelText("((0))", firstLine.Time1, firstLine.Y1, id, fontSize: 12);
                    DrawLabelText("((A))", secondLine.Time1, secondLine.Y1, id, fontSize: 12);
                    DrawLabelText("((B))", thirdLine.Time1, thirdLine.Y1, id, fontSize: 12);
                    DrawLabelText("((C))", thirdLine.Time2, thirdLine.Y2, id, fontSize: 12);
                    break;

                case ElliottWaveDegree.Intermediate:
                    DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id, fontSize: 12);
                    DrawLabelText("(A)", secondLine.Time1, secondLine.Y1, id, fontSize: 12);
                    DrawLabelText("(B)", thirdLine.Time1, thirdLine.Y1, id, fontSize: 12);
                    DrawLabelText("(C)", thirdLine.Time2, thirdLine.Y2, id, fontSize: 12);
                    break;

                case ElliottWaveDegree.Minor:
                    DrawLabelText("0", firstLine.Time1, firstLine.Y1, id, fontSize: 12);
                    DrawLabelText("A", secondLine.Time1, secondLine.Y1, id, fontSize: 12);
                    DrawLabelText("B", thirdLine.Time1, thirdLine.Y1, id, fontSize: 12);
                    DrawLabelText("C", thirdLine.Time2, thirdLine.Y2, id, fontSize: 12);
                    break;

                case ElliottWaveDegree.Minute:
                    DrawLabelText("((0))", firstLine.Time1, firstLine.Y1, id, fontSize: 12);
                    DrawLabelText("((a))", secondLine.Time1, secondLine.Y1, id, fontSize: 12);
                    DrawLabelText("((b))", thirdLine.Time1, thirdLine.Y1, id, fontSize: 12);
                    DrawLabelText("((c))", thirdLine.Time2, thirdLine.Y2, id, fontSize: 12);
                    break;

                case ElliottWaveDegree.Minuette:
                    DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id, fontSize: 12);
                    DrawLabelText("(a)", secondLine.Time1, secondLine.Y1, id, fontSize: 12);
                    DrawLabelText("(b)", thirdLine.Time1, thirdLine.Y1, id, fontSize: 12);
                    DrawLabelText("(c)", thirdLine.Time2, thirdLine.Y2, id, fontSize: 12);
                    break;

                case ElliottWaveDegree.SubMinuette:
                    DrawLabelText("0", firstLine.Time1, firstLine.Y1, id, fontSize: 12);
                    DrawLabelText("a", secondLine.Time1, secondLine.Y1, id, fontSize: 12);
                    DrawLabelText("b", thirdLine.Time1, thirdLine.Y1, id, fontSize: 12);
                    DrawLabelText("c", thirdLine.Time2, thirdLine.Y2, id, fontSize: 12);
                    break;

                case ElliottWaveDegree.Micro:
                    DrawLabelText("((0))", firstLine.Time1, firstLine.Y1, id, fontSize: 10);
                    DrawLabelText("((A))", secondLine.Time1, secondLine.Y1, id, fontSize: 10);
                    DrawLabelText("((B))", thirdLine.Time1, thirdLine.Y1, id, fontSize: 10);
                    DrawLabelText("((C))", thirdLine.Time2, thirdLine.Y2, id, fontSize: 10);
                    break;

                case ElliottWaveDegree.SubMicro:
                    DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id, fontSize: 10);
                    DrawLabelText("(A)", secondLine.Time1, secondLine.Y1, id, fontSize: 10);
                    DrawLabelText("(B)", thirdLine.Time1, thirdLine.Y1, id, fontSize: 10);
                    DrawLabelText("(C)", thirdLine.Time2, thirdLine.Y2, id, fontSize: 10);
                    break;

                case ElliottWaveDegree.Minuscule:
                    DrawLabelText("0", firstLine.Time1, firstLine.Y1, id, fontSize: 10);
                    DrawLabelText("A", secondLine.Time1, secondLine.Y1, id, fontSize: 10);
                    DrawLabelText("B", thirdLine.Time1, thirdLine.Y1, id, fontSize: 10);
                    DrawLabelText("C", thirdLine.Time2, thirdLine.Y2, id, fontSize: 10);
                    break;
            }
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

            if (labels.Length == 0)
            {
                DrawLabels(firstLine, secondLine, thirdLine, id);

                return;
            }

            string firstLabelText;
            string secondLabelText;
            string thirdLabelText;
            string fourthLabelText;

            switch (Degree)
            {
                case ElliottWaveDegree.SuperMellennium:
                    firstLabelText = "((0))";
                    secondLabelText = "((A))";
                    thirdLabelText = "((B))";
                    fourthLabelText = "((C))";
                    break;

                case ElliottWaveDegree.Mellennium:
                    firstLabelText = "(0)";
                    secondLabelText = "(A)";
                    thirdLabelText = "(B)";
                    fourthLabelText = "(C)";
                    break;

                case ElliottWaveDegree.SubMellennium:
                    firstLabelText = "0";
                    secondLabelText = "A";
                    thirdLabelText = "B";
                    fourthLabelText = "C";
                    break;

                case ElliottWaveDegree.GrandSuperCycle:
                    firstLabelText = "((0))";
                    secondLabelText = "((a))";
                    thirdLabelText = "((b))";
                    fourthLabelText = "((c))";
                    break;

                case ElliottWaveDegree.SuperCycle:
                    firstLabelText = "(0)";
                    secondLabelText = "(a)";
                    thirdLabelText = "(b)";
                    fourthLabelText = "(c)";
                    break;

                case ElliottWaveDegree.Cycle:
                    firstLabelText = "0";
                    secondLabelText = "a";
                    thirdLabelText = "b";
                    fourthLabelText = "c";
                    break;

                case ElliottWaveDegree.Primary:
                    firstLabelText = "((0))";
                    secondLabelText = "((A))";
                    thirdLabelText = "((B))";
                    fourthLabelText = "((C))";
                    break;

                case ElliottWaveDegree.Intermediate:
                    firstLabelText = "(0)";
                    secondLabelText = "(A)";
                    thirdLabelText = "(B)";
                    fourthLabelText = "(C)";
                    break;

                case ElliottWaveDegree.Minor:
                    firstLabelText = "0";
                    secondLabelText = "A";
                    thirdLabelText = "B";
                    fourthLabelText = "C";
                    break;

                case ElliottWaveDegree.Minute:
                    firstLabelText = "((0))";
                    secondLabelText = "((a))";
                    thirdLabelText = "((b))";
                    fourthLabelText = "((c))";
                    break;

                case ElliottWaveDegree.Minuette:
                    firstLabelText = "(0)";
                    secondLabelText = "(a)";
                    thirdLabelText = "(b)";
                    fourthLabelText = "(c)";
                    break;

                case ElliottWaveDegree.SubMinuette:
                    firstLabelText = "0";
                    secondLabelText = "a";
                    thirdLabelText = "b";
                    fourthLabelText = "c";
                    break;

                case ElliottWaveDegree.Micro:
                    firstLabelText = "((0))";
                    secondLabelText = "((A))";
                    thirdLabelText = "((B))";
                    fourthLabelText = "((C))";
                    break;

                case ElliottWaveDegree.SubMicro:
                    firstLabelText = "(0)";
                    secondLabelText = "(A)";
                    thirdLabelText = "(B)";
                    fourthLabelText = "(C)";
                    break;

                case ElliottWaveDegree.Minuscule:
                    firstLabelText = "0";
                    secondLabelText = "A";
                    thirdLabelText = "B";
                    fourthLabelText = "C";
                    break;

                default:
                    throw new InvalidOperationException("Invalid degree");
            }

            foreach (var label in labels)
            {
                if (label.Text.Equals(firstLabelText, StringComparison.Ordinal))
                {
                    label.Time = firstLine.Time1;
                    label.Y = firstLine.Y1;
                }
                else if (label.Text.Equals(secondLabelText, StringComparison.Ordinal))
                {
                    label.Time = secondLine.Time1;
                    label.Y = secondLine.Y1;
                }
                else if (label.Text.Equals(thirdLabelText, StringComparison.Ordinal))
                {
                    label.Time = thirdLine.Time1;
                    label.Y = thirdLine.Y1;
                }
                else if (label.Text.Equals(fourthLabelText, StringComparison.Ordinal))
                {
                    label.Time = thirdLine.Time2;
                    label.Y = thirdLine.Y2;
                }
            }
        }
    }
}