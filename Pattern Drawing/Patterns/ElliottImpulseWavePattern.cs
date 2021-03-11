using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class ElliottImpulseWavePattern : PatternBase
    {
        private ChartTrendLine _firstLine, _secondLine, _thirdLine, _fourthLine, _fifthLine;

        public ElliottImpulseWavePattern(Chart chart, Color color, bool showLabels, Color labelsColor) : base(chart, "Elliott Impulse Wave (12345)", color,
            showLabels, labelsColor)
        {
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            var updatedLine = updatedChartObject as ChartTrendLine;

            if (updatedLine == null) return;

            if (updatedLine.Name.EndsWith("FirstLine", StringComparison.OrdinalIgnoreCase))
            {
                UpdateSideLines(updatedLine, patternObjects, null, "SecondLine");
            }
            else if (updatedLine.Name.EndsWith("SecondLine", StringComparison.OrdinalIgnoreCase))
            {
                UpdateSideLines(updatedLine, patternObjects, "FirstLine", "ThirdLine");
            }
            else if (updatedLine.Name.EndsWith("ThirdLine", StringComparison.OrdinalIgnoreCase))
            {
                UpdateSideLines(updatedLine, patternObjects, "SecondLine", "FourthLine");
            }
            else if (updatedLine.Name.EndsWith("FourthLine", StringComparison.OrdinalIgnoreCase))
            {
                UpdateSideLines(updatedLine, patternObjects, "ThirdLine", "FifthLine");
            }
            else if (updatedLine.Name.EndsWith("FifthLine", StringComparison.OrdinalIgnoreCase))
            {
                UpdateSideLines(updatedLine, patternObjects, "FourthLine", null);
            }
        }

        private void UpdateSideLines(ChartTrendLine line, ChartObject[] patternObjects, string leftLineName, string rightLineName)
        {
            if (!string.IsNullOrWhiteSpace(leftLineName))
            {
                var leftLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith(leftLineName,
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

                if (leftLine != null)
                {
                    leftLine.Time2 = line.Time1;
                    leftLine.Y2 = line.Y1;
                }
            }

            if (!string.IsNullOrWhiteSpace(rightLineName))
            {
                var rightLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith(rightLineName,
                    StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

                if (rightLine != null)
                {
                    rightLine.Time1 = line.Time2;
                    rightLine.Y1 = line.Y2;
                }
            }
        }

        protected override void OnDrawingStopped()
        {
            _firstLine = null;
            _secondLine = null;
            _thirdLine = null;
            _fourthLine = null;
            _fifthLine = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 6)
            {
                StopDrawing();

                return;
            }

            if (_firstLine == null)
            {
                var name = GetObjectName("FirstLine");

                DrawLine(obj, name, ref _firstLine);
            }
            else if (_secondLine == null && MouseUpNumber == 2)
            {
                var name = GetObjectName("SecondLine");

                DrawLine(obj, name, ref _secondLine);
            }
            else if (_thirdLine == null && MouseUpNumber == 3)
            {
                var name = GetObjectName("ThirdLine");

                DrawLine(obj, name, ref _thirdLine);
            }
            else if (_fourthLine == null && MouseUpNumber == 4)
            {
                var name = GetObjectName("FourthLine");

                DrawLine(obj, name, ref _fourthLine);
            }
            else if (_fifthLine == null && MouseUpNumber == 5)
            {
                var name = GetObjectName("FifthLine");

                DrawLine(obj, name, ref _fifthLine);
            }
        }

        private void DrawLine(ChartMouseEventArgs mouseEventArgs, string name, ref ChartTrendLine line)
        {
            line = Chart.DrawTrendLine(name, mouseEventArgs.TimeValue, mouseEventArgs.YValue, mouseEventArgs.TimeValue,
                mouseEventArgs.YValue, Color);

            line.IsInteractive = true;
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            switch (MouseUpNumber)
            {
                case 1:
                    _firstLine.Time2 = obj.TimeValue;
                    _firstLine.Y2 = obj.YValue;
                    return;

                case 2:
                    if (_secondLine == null) return;

                    _secondLine.Time2 = obj.TimeValue;
                    _secondLine.Y2 = obj.YValue;
                    return;

                case 3:
                    if (_thirdLine == null) return;

                    _thirdLine.Time2 = obj.TimeValue;
                    _thirdLine.Y2 = obj.YValue;
                    return;

                case 4:
                    if (_fourthLine == null) return;

                    _fourthLine.Time2 = obj.TimeValue;
                    _fourthLine.Y2 = obj.YValue;
                    return;

                case 5:
                    if (_fifthLine == null) return;

                    _fifthLine.Time2 = obj.TimeValue;
                    _fifthLine.Y2 = obj.YValue;
                    return;
            }
        }

        protected override void DrawLabels()
        {
            if (_firstLine == null || _secondLine == null || _thirdLine == null || _fourthLine == null || _fifthLine == null) return;

            DrawLabelText("(0)", _firstLine.Time1, _firstLine.Y1);
            DrawLabelText("(1)", _secondLine.Time1, _secondLine.Y1);
            DrawLabelText("(2)", _thirdLine.Time1, _thirdLine.Y1);
            DrawLabelText("(3)", _fourthLine.Time1, _fourthLine.Y1);
            DrawLabelText("(4)", _fifthLine.Time1, _fifthLine.Y1);
            DrawLabelText("(5)", _fifthLine.Time2, _fifthLine.Y2);
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

                    case "(1)":
                        label.Time = secondLine.Time1;
                        label.Y = secondLine.Y1;
                        break;

                    case "(2)":
                        label.Time = thirdLine.Time1;
                        label.Y = thirdLine.Y1;
                        break;

                    case "(3)":
                        label.Time = fourthLine.Time1;
                        label.Y = fourthLine.Y1;
                        break;

                    case "(4)":
                        label.Time = fifthLine.Time1;
                        label.Y = fifthLine.Y1;
                        break;

                    case "(5)":
                        label.Time = fifthLine.Time2;
                        label.Y = fifthLine.Y2;
                        break;
                }
            }
        }
    }
}