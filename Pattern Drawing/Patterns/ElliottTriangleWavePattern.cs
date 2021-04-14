﻿using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class ElliottTriangleWavePattern : ElliottWavePatternBase
    {
        public ElliottTriangleWavePattern(PatternConfig config, ElliottWaveDegree degree) : base("EW ABCDE", config, 5, degree)
        {
        }

        protected override void DrawLabels()
        {
            if (FirstLine == null || SecondLine == null || ThirdLine == null || FourthLine == null || FifthLine == null) return;

            DrawLabels(FirstLine, SecondLine, ThirdLine, FourthLine, FifthLine, Id);
        }

        private void DrawLabels(ChartTrendLine firstLine, ChartTrendLine secondLine, ChartTrendLine thirdLine, ChartTrendLine fourthLine, ChartTrendLine fifthLine, long id)
        {
            switch (Degree)
            {
                case ElliottWaveDegree.SuperMellennium:
                    DrawLabelText("((0))", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("((A))", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("((B))", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("((C))", fourthLine.Time1, fourthLine.Y1, id, isBold: true);
                    DrawLabelText("((D))", fifthLine.Time1, fifthLine.Y1, id, isBold: true);
                    DrawLabelText("((E))", fifthLine.Time2, fifthLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.Mellennium:
                    DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("(A)", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("(B)", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("(C)", fourthLine.Time1, fourthLine.Y1, id, isBold: true);
                    DrawLabelText("(D)", fifthLine.Time1, fifthLine.Y1, id, isBold: true);
                    DrawLabelText("(E)", fifthLine.Time2, fifthLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.SubMellennium:
                    DrawLabelText("0", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("A", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("B", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("C", fourthLine.Time1, fourthLine.Y1, id, isBold: true);
                    DrawLabelText("D", fifthLine.Time1, fifthLine.Y1, id, isBold: true);
                    DrawLabelText("E", fifthLine.Time2, fifthLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.GrandSuperCycle:
                    DrawLabelText("((0))", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("((a))", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("((b))", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("((c))", fourthLine.Time1, fourthLine.Y1, id, isBold: true);
                    DrawLabelText("((d))", fifthLine.Time1, fifthLine.Y1, id, isBold: true);
                    DrawLabelText("((e))", fifthLine.Time2, fifthLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.SuperCycle:
                    DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("(a)", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("(b)", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("(c)", fourthLine.Time1, fourthLine.Y1, id, isBold: true);
                    DrawLabelText("(d)", fifthLine.Time1, fifthLine.Y1, id, isBold: true);
                    DrawLabelText("(e)", fifthLine.Time2, fifthLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.Cycle:
                    DrawLabelText("0", firstLine.Time1, firstLine.Y1, id, isBold: true);
                    DrawLabelText("a", secondLine.Time1, secondLine.Y1, id, isBold: true);
                    DrawLabelText("b", thirdLine.Time1, thirdLine.Y1, id, isBold: true);
                    DrawLabelText("c", fourthLine.Time1, fourthLine.Y1, id, isBold: true);
                    DrawLabelText("d", fifthLine.Time1, fifthLine.Y1, id, isBold: true);
                    DrawLabelText("e", fifthLine.Time2, fifthLine.Y2, id, isBold: true);
                    break;

                case ElliottWaveDegree.Primary:
                    DrawLabelText("((0))", firstLine.Time1, firstLine.Y1, id, fontSize: 10);
                    DrawLabelText("((A))", secondLine.Time1, secondLine.Y1, id, fontSize: 10);
                    DrawLabelText("((B))", thirdLine.Time1, thirdLine.Y1, id, fontSize: 10);
                    DrawLabelText("((C))", fourthLine.Time1, fourthLine.Y1, id, fontSize: 10);
                    DrawLabelText("((D))", fifthLine.Time1, fifthLine.Y1, id, fontSize: 10);
                    DrawLabelText("((E))", fifthLine.Time2, fifthLine.Y2, id, fontSize: 10);
                    break;

                case ElliottWaveDegree.Intermediate:
                    DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id, fontSize: 10);
                    DrawLabelText("(A)", secondLine.Time1, secondLine.Y1, id, fontSize: 10);
                    DrawLabelText("(B)", thirdLine.Time1, thirdLine.Y1, id, fontSize: 10);
                    DrawLabelText("(C)", fourthLine.Time1, fourthLine.Y1, id, fontSize: 10);
                    DrawLabelText("(D)", fifthLine.Time1, fifthLine.Y1, id, fontSize: 10);
                    DrawLabelText("(E)", fifthLine.Time2, fifthLine.Y2, id, fontSize: 10);
                    break;

                case ElliottWaveDegree.Minor:
                    DrawLabelText("0", firstLine.Time1, firstLine.Y1, id, fontSize: 10);
                    DrawLabelText("A", secondLine.Time1, secondLine.Y1, id, fontSize: 10);
                    DrawLabelText("B", thirdLine.Time1, thirdLine.Y1, id, fontSize: 10);
                    DrawLabelText("C", fourthLine.Time1, fourthLine.Y1, id, fontSize: 10);
                    DrawLabelText("D", fifthLine.Time1, fifthLine.Y1, id, fontSize: 10);
                    DrawLabelText("E", fifthLine.Time2, fifthLine.Y2, id, fontSize: 10);
                    break;

                case ElliottWaveDegree.Minute:
                    DrawLabelText("((0))", firstLine.Time1, firstLine.Y1, id, fontSize: 10);
                    DrawLabelText("((a))", secondLine.Time1, secondLine.Y1, id, fontSize: 10);
                    DrawLabelText("((b))", thirdLine.Time1, thirdLine.Y1, id, fontSize: 10);
                    DrawLabelText("((c))", fourthLine.Time1, fourthLine.Y1, id, fontSize: 10);
                    DrawLabelText("((d))", fifthLine.Time1, fifthLine.Y1, id, fontSize: 10);
                    DrawLabelText("((e))", fifthLine.Time2, fifthLine.Y2, id, fontSize: 10);
                    break;

                case ElliottWaveDegree.Minuette:
                    DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id, fontSize: 10);
                    DrawLabelText("(a)", secondLine.Time1, secondLine.Y1, id, fontSize: 10);
                    DrawLabelText("(b)", thirdLine.Time1, thirdLine.Y1, id, fontSize: 10);
                    DrawLabelText("(c)", fourthLine.Time1, fourthLine.Y1, id, fontSize: 10);
                    DrawLabelText("(d)", fifthLine.Time1, fifthLine.Y1, id, fontSize: 10);
                    DrawLabelText("(e)", fifthLine.Time2, fifthLine.Y2, id, fontSize: 10);
                    break;

                case ElliottWaveDegree.SubMinuette:
                    DrawLabelText("0", firstLine.Time1, firstLine.Y1, id, fontSize: 10);
                    DrawLabelText("a", secondLine.Time1, secondLine.Y1, id, fontSize: 10);
                    DrawLabelText("b", thirdLine.Time1, thirdLine.Y1, id, fontSize: 10);
                    DrawLabelText("c", fourthLine.Time1, fourthLine.Y1, id, fontSize: 10);
                    DrawLabelText("d", fifthLine.Time1, fifthLine.Y1, id, fontSize: 10);
                    DrawLabelText("e", fifthLine.Time2, fifthLine.Y2, id, fontSize: 10);
                    break;

                case ElliottWaveDegree.Micro:
                    DrawLabelText("((0))", firstLine.Time1, firstLine.Y1, id, fontSize: 7);
                    DrawLabelText("((A))", secondLine.Time1, secondLine.Y1, id, fontSize: 7);
                    DrawLabelText("((B))", thirdLine.Time1, thirdLine.Y1, id, fontSize: 7);
                    DrawLabelText("((C))", fourthLine.Time1, fourthLine.Y1, id, fontSize: 7);
                    DrawLabelText("((D))", fifthLine.Time1, fifthLine.Y1, id, fontSize: 7);
                    DrawLabelText("((E))", fifthLine.Time2, fifthLine.Y2, id, fontSize: 7);
                    break;

                case ElliottWaveDegree.SubMicro:
                    DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id, fontSize: 7);
                    DrawLabelText("(A)", secondLine.Time1, secondLine.Y1, id, fontSize: 7);
                    DrawLabelText("(B)", thirdLine.Time1, thirdLine.Y1, id, fontSize: 7);
                    DrawLabelText("(C)", fourthLine.Time1, fourthLine.Y1, id, fontSize: 7);
                    DrawLabelText("(D)", fifthLine.Time1, fifthLine.Y1, id, fontSize: 7);
                    DrawLabelText("(E)", fifthLine.Time2, fifthLine.Y2, id, fontSize: 7);
                    break;

                case ElliottWaveDegree.Minuscule:
                    DrawLabelText("0", firstLine.Time1, firstLine.Y1, id, fontSize: 7);
                    DrawLabelText("A", secondLine.Time1, secondLine.Y1, id, fontSize: 7);
                    DrawLabelText("B", thirdLine.Time1, thirdLine.Y1, id, fontSize: 7);
                    DrawLabelText("C", fourthLine.Time1, fourthLine.Y1, id, fontSize: 7);
                    DrawLabelText("D", fifthLine.Time1, fifthLine.Y1, id, fontSize: 7);
                    DrawLabelText("E", fifthLine.Time2, fifthLine.Y2, id, fontSize: 7);
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

            var fourthLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FourthLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var fifthLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FifthLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            if (firstLine == null || secondLine == null || thirdLine == null || fourthLine == null || fifthLine == null) return;

            if (labels.Length == 0)
            {
                DrawLabels(firstLine, secondLine, thirdLine, fourthLine, fifthLine, id);

                return;
            }

            string firstLabelText;
            string secondLabelText;
            string thirdLabelText;
            string fourthLabelText;
            string fifthLabelText;
            string sixthLabelText;

            switch (Degree)
            {
                case ElliottWaveDegree.SuperMellennium:
                case ElliottWaveDegree.Primary:
                case ElliottWaveDegree.Micro:
                    firstLabelText = "((0))";
                    secondLabelText = "((A))";
                    thirdLabelText = "((B))";
                    fourthLabelText = "((C))";
                    fifthLabelText = "((D))";
                    sixthLabelText = "((E))";
                    break;

                case ElliottWaveDegree.Mellennium:
                case ElliottWaveDegree.Intermediate:
                case ElliottWaveDegree.SubMicro:
                    firstLabelText = "(0)";
                    secondLabelText = "(A)";
                    thirdLabelText = "(B)";
                    fourthLabelText = "(C)";
                    fifthLabelText = "(D)";
                    sixthLabelText = "(E)";
                    break;

                case ElliottWaveDegree.SubMellennium:
                case ElliottWaveDegree.Minor:
                case ElliottWaveDegree.Minuscule:
                    firstLabelText = "0";
                    secondLabelText = "A";
                    thirdLabelText = "B";
                    fourthLabelText = "C";
                    fifthLabelText = "D";
                    sixthLabelText = "E";
                    break;

                case ElliottWaveDegree.GrandSuperCycle:
                case ElliottWaveDegree.Minute:
                    firstLabelText = "((0))";
                    secondLabelText = "((a))";
                    thirdLabelText = "((b))";
                    fourthLabelText = "((c))";
                    fifthLabelText = "((d))";
                    sixthLabelText = "((e))";
                    break;

                case ElliottWaveDegree.SuperCycle:
                case ElliottWaveDegree.Minuette:
                    firstLabelText = "(0)";
                    secondLabelText = "(a)";
                    thirdLabelText = "(b)";
                    fourthLabelText = "(c)";
                    fifthLabelText = "(d)";
                    sixthLabelText = "(e)";
                    break;

                case ElliottWaveDegree.Cycle:
                case ElliottWaveDegree.SubMinuette:
                    firstLabelText = "0";
                    secondLabelText = "a";
                    thirdLabelText = "b";
                    fourthLabelText = "c";
                    fifthLabelText = "d";
                    sixthLabelText = "e";
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
                    label.Time = fourthLine.Time1;
                    label.Y = fourthLine.Y1;
                }
                else if (label.Text.Equals(fifthLabelText, StringComparison.Ordinal))
                {
                    label.Time = fifthLine.Time1;
                    label.Y = fifthLine.Y1;
                }
                else if (label.Text.Equals(sixthLabelText, StringComparison.Ordinal))
                {
                    label.Time = fifthLine.Time2;
                    label.Y = fifthLine.Y2;
                }
            }
        }
    }
}