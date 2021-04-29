using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class ThreeDrivesPattern : PatternBase
    {
        private ChartTrendLine _firstLine, _secondLine, _thirdLine, _fourthLine, _fifthLine, _sixthLine;

        private ChartTrendLine _firstConnectionLine, _secondConnectionLine;

        public ThreeDrivesPattern(PatternConfig config) : base("Three Drives", config)
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
                UpdateSideLines(updatedLine, patternObjects, "FourthLine", "SixthLine");
            }
            else if (updatedLine.Name.EndsWith("SixthLine", StringComparison.OrdinalIgnoreCase))
            {
                UpdateSideLines(updatedLine, patternObjects, "FifthLine", null);
            }

            var firstLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FirstLine", StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;
            var thirdLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("ThirdLine", StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;
            var fifthLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FifthLine", StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            if (firstLine == null || thirdLine == null || fifthLine == null) return;

            DrawNonInteractiveObjects(firstLine, thirdLine, fifthLine, id);
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
            _sixthLine = null;
            _firstConnectionLine = null;
            _secondConnectionLine = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 7)
            {
                if (_firstLine != null && _thirdLine != null && _fifthLine != null)
                {
                    DrawNonInteractiveObjects(_firstLine, _thirdLine, _fifthLine, Id);
                }

                FinishDrawing();

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
            else if (_sixthLine == null && MouseUpNumber == 6)
            {
                var name = GetObjectName("SixthLine");

                DrawLine(obj, name, ref _sixthLine);
            }
        }

        private void DrawNonInteractiveObjects(ChartTrendLine firstLine, ChartTrendLine thirdLine, ChartTrendLine fifthLine, long id)
        {
            var firstLineName = GetObjectName("FirstConnectionLine", id);

            _firstConnectionLine = Chart.DrawTrendLine(firstLineName, firstLine.Time2, firstLine.Y2, thirdLine.Time2, thirdLine.Y2, Color, 1, LineStyle.Dots);

            _firstConnectionLine.IsInteractive = true;
            _firstConnectionLine.IsLocked = true;

            var secondLineName = GetObjectName("SecondConnectionLine", id);

            _secondConnectionLine = Chart.DrawTrendLine(secondLineName, thirdLine.Time2, thirdLine.Y2, fifthLine.Time2, fifthLine.Y2, Color, 1, LineStyle.Dots);

            _secondConnectionLine.IsInteractive = true;
            _secondConnectionLine.IsLocked = true;
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

                case 6:
                    if (_sixthLine == null) return;

                    _sixthLine.Time2 = obj.TimeValue;
                    _sixthLine.Y2 = obj.YValue;
                    return;
            }
        }

        protected override void DrawLabels()
        {
            if (_firstLine == null || _thirdLine == null || _fifthLine == null || _firstConnectionLine == null || _secondConnectionLine == null) return;

            DrawLabels(_firstLine, _thirdLine, _fifthLine, _firstConnectionLine, _secondConnectionLine, Id);
        }

        private void DrawLabels(ChartTrendLine firstLine, ChartTrendLine thirdLine, ChartTrendLine fifthLine, ChartTrendLine firstConnectionLine, ChartTrendLine secondConnectionLine, long id)
        {
            DrawOrUpdateFirstConnectionLineLabel(firstLine, thirdLine, firstConnectionLine, id);
            DrawOrUpdateSecondConnectionLineLabel(fifthLine, thirdLine, secondConnectionLine, id);
        }

        private void DrawOrUpdateFirstConnectionLineLabel(ChartTrendLine firstLine, ChartTrendLine thirdLine, ChartTrendLine firstConnectionLine, long id, ChartText label = null)
        {
            var firstLineLength = firstLine.Y2 - firstLine.Y1;

            var diffLength = thirdLine.Y2 - firstLine.Y2;

            var ratio = Math.Round(1 + diffLength / firstLineLength, 3);

            var labelTime = firstConnectionLine.GetLineCenterTime();

            var labelY = firstConnectionLine.GetLineCenterY();

            if (label == null)
            {
                DrawLabelText(ratio.ToString(), labelTime, labelY, id, objectNameKey: "FirstConnection");
            }
            else
            {
                label.Text = ratio.ToString();
                label.Time = labelTime;
                label.Y = labelY;
            }
        }

        private void DrawOrUpdateSecondConnectionLineLabel(ChartTrendLine fifthLine, ChartTrendLine thirdLine, ChartTrendLine secondConnectionLine, long id, ChartText label = null)
        {
            var fifthLineLength = fifthLine.Y2 - fifthLine.Y1;

            var diffLength = thirdLine.Y2 - fifthLine.Y2;

            var ratio = Math.Round(diffLength / fifthLineLength, 3);

            var labelTime = secondConnectionLine.GetLineCenterTime();

            var labelY = secondConnectionLine.GetLineCenterY();

            if (label == null)
            {
                DrawLabelText(ratio.ToString(), labelTime, labelY, id, objectNameKey: "SecondConnection");
            }
            else
            {
                label.Text = ratio.ToString();
                label.Time = labelTime;
                label.Y = labelY;
            }
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var firstLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FirstLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var thirdLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("ThirdLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var fifthLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FifthLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var firstConnectionLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FirstConnectionLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var secondConnectionLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("SecondConnectionLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            if (firstLine == null || thirdLine == null || firstConnectionLine == null || secondConnectionLine == null) return;

            if (labels.Length == 0)
            {
                DrawLabels(firstLine, thirdLine, fifthLine, firstConnectionLine, secondConnectionLine, id);

                return;
            }

            foreach (var label in labels)
            {
                if (label.Name.EndsWith("FirstConnection", StringComparison.OrdinalIgnoreCase)) DrawOrUpdateFirstConnectionLineLabel(firstLine, thirdLine, firstConnectionLine, id, label);
                if (label.Name.EndsWith("SecondConnection", StringComparison.OrdinalIgnoreCase)) DrawOrUpdateSecondConnectionLineLabel(fifthLine, thirdLine, secondConnectionLine, id, label);
            }
        }

        protected override ChartObject[] GetFrontObjects()
        {
            return new ChartObject[] { _firstLine, _secondLine, _thirdLine, _fourthLine, _fifthLine, _sixthLine };
        }
    }
}