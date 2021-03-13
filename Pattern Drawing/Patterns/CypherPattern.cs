using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class CypherPattern : PatternBase
    {
        private ChartTriangle _leftTriangle;
        private ChartTriangle _rightTriangle;

        private ChartTrendLine _xdLine;
        private ChartTrendLine _acLine;

        public CypherPattern(PatternConfig config) : base("Cypher", config)
        {
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            var otherTriangle = updatedChartObject as ChartTriangle;

            if (otherTriangle == null) return;

            var chartObjects = Chart.Objects.ToArray();

            var objectNameId = string.Format("{0}_{1}", ObjectName, id);

            var leftTriangle = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("Left",
                StringComparison.OrdinalIgnoreCase)) as ChartTriangle;

            var rightTriangle = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("Right",
                StringComparison.OrdinalIgnoreCase)) as ChartTriangle;

            if (leftTriangle == null || rightTriangle == null) return;

            foreach (var chartObject in patternObjects)
            {
                if (chartObject == updatedChartObject)
                {
                    continue;
                }

                var nameSplit = chartObject.Name.Split('_');

                switch (nameSplit.Last())
                {
                    case "Left":
                        leftTriangle.Time3 = otherTriangle.Time1;
                        leftTriangle.Y3 = otherTriangle.Y1;
                        break;

                    case "Right":
                        rightTriangle.Time1 = otherTriangle.Time3;
                        rightTriangle.Y1 = otherTriangle.Y3;
                        break;
                }
            }

            DrawNonInteractiveObjects(leftTriangle, rightTriangle, id);
        }

        protected override void OnDrawingStopped()
        {
            _leftTriangle = null;
            _rightTriangle = null;
            _xdLine = null;
            _acLine = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 5)
            {
                FinishDrawing();

                return;
            }

            if (_leftTriangle == null)
            {
                var name = GetObjectName("Left");

                DrawTriangle(obj, name, ref _leftTriangle);
            }
            else if (_rightTriangle == null && MouseUpNumber == 3)
            {
                var name = GetObjectName("Right");

                DrawTriangle(obj, name, ref _rightTriangle);
            }
        }

        private void DrawTriangle(ChartMouseEventArgs mouseEventArgs, string name, ref ChartTriangle triangle)
        {
            triangle = Chart.DrawTriangle(name, mouseEventArgs.TimeValue, mouseEventArgs.YValue, mouseEventArgs.TimeValue,
                mouseEventArgs.YValue, mouseEventArgs.TimeValue, mouseEventArgs.YValue, Color);

            triangle.IsInteractive = true;

            triangle.IsFilled = true;
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 1)
            {
                _leftTriangle.Time2 = obj.TimeValue;
                _leftTriangle.Y2 = obj.YValue;
            }
            else if (MouseUpNumber == 2)
            {
                _leftTriangle.Time3 = obj.TimeValue;
                _leftTriangle.Y3 = obj.YValue;
            }
            else if (MouseUpNumber == 3)
            {
                _rightTriangle.Time2 = obj.TimeValue;
                _rightTriangle.Y2 = obj.YValue;
            }
            else if (MouseUpNumber == 4)
            {
                _rightTriangle.Time3 = obj.TimeValue;
                _rightTriangle.Y3 = obj.YValue;
            }
        }

        protected override void DrawLabels()
        {
            if (_leftTriangle == null || _rightTriangle == null || _acLine == null || _xdLine == null) return;

            DrawLabels(_leftTriangle, _rightTriangle, _acLine, _xdLine, Id);
        }

        private void DrawLabels(ChartTriangle leftTriangle, ChartTriangle rightTriangle, ChartTrendLine acLine, ChartTrendLine xdLine, long id)
        {
            DrawLabelText("X", leftTriangle.Time1, leftTriangle.Y1, id);

            DrawLabelText("A", leftTriangle.Time2, leftTriangle.Y2, id);

            DrawLabelText("B", leftTriangle.Time3, leftTriangle.Y3, id);

            DrawLabelText("C", rightTriangle.Time2, rightTriangle.Y2, id);

            DrawLabelText("D", rightTriangle.Time3, rightTriangle.Y3, id);

            DrawOrUpdateAcLabel(leftTriangle, rightTriangle, acLine, id);

            DrawOrUpdateXdLabel(leftTriangle, rightTriangle, xdLine, id);

            DrawOrUpdateBdLabel(rightTriangle, id);
        }

        protected override void DrawNonInteractiveObjects()
        {
            if (_leftTriangle == null || _rightTriangle == null) return;

            DrawNonInteractiveObjects(_leftTriangle, _rightTriangle, Id);
        }

        private void DrawNonInteractiveObjects(ChartTriangle leftTriangle, ChartTriangle rightTriangle, long id)
        {
            var acLineName = GetObjectName("ACLine", id);

            _acLine = Chart.DrawTrendLine(acLineName, leftTriangle.Time2, leftTriangle.Y2, rightTriangle.Time2, rightTriangle.Y2, Color, 1, LineStyle.Dots);

            var xdLineName = GetObjectName("XDLine", id);

            _xdLine = Chart.DrawTrendLine(xdLineName, leftTriangle.Time1, leftTriangle.Y1, rightTriangle.Time3, rightTriangle.Y3, Color, 1, LineStyle.Dots);
        }

        private void DrawOrUpdateBdLabel(ChartTriangle rightTriangle, long id, ChartText label = null)
        {
            var cdLength = rightTriangle.Y2 - rightTriangle.Y3;

            var bcLength = rightTriangle.Y2 - rightTriangle.Y1;

            var ratio = Math.Round(cdLength / bcLength, 3);

            var labelTime = rightTriangle.Time1.AddMilliseconds((rightTriangle.Time3 - rightTriangle.Time1).TotalMilliseconds * 0.3);

            var labelY = rightTriangle.Y1 + ((rightTriangle.Y3 - rightTriangle.Y1) / 2);

            if (label == null)
            {
                DrawLabelText(ratio.ToString(), labelTime, labelY, id, objectNameKey: "BD");
            }
            else
            {
                label.Text = ratio.ToString();
                label.Time = labelTime;
                label.Y = labelY;
            }
        }

        private void DrawOrUpdateXbLabel(ChartTriangle leftTriangle, long id, ChartText label = null)
        {
            var abLength = leftTriangle.Y2 - leftTriangle.Y3;

            var xaLength = leftTriangle.Y2 - leftTriangle.Y1;

            var ratio = Math.Round(abLength / xaLength, 3);

            var labelTime = leftTriangle.Time1.AddMilliseconds((leftTriangle.Time3 - leftTriangle.Time1).TotalMilliseconds * 0.7);

            var labelY = leftTriangle.Y1 + ((leftTriangle.Y3 - leftTriangle.Y1) / 2);

            if (label == null)
            {
                DrawLabelText(ratio.ToString(), labelTime, labelY, id, objectNameKey: "XB");
            }
            else
            {
                label.Text = ratio.ToString();
                label.Time = labelTime;
                label.Y = labelY;
            }
        }

        private void DrawOrUpdateXdLabel(ChartTriangle leftTriangle, ChartTriangle rightTriangle, ChartTrendLine line, long id, ChartText label = null)
        {
            var labelTime = line.GetLineCenterTime();

            var labelY = line.GetLineCenterY();

            var xdLength = rightTriangle.Y2 - rightTriangle.Y3;

            var xcLength = rightTriangle.Y2 - leftTriangle.Y1;

            var ratio = Math.Round(xdLength / xcLength, 3);

            if (label == null)
            {
                DrawLabelText(ratio.ToString(), labelTime, labelY, id, objectNameKey: "XD");
            }
            else
            {
                label.Text = ratio.ToString();
                label.Time = labelTime;
                label.Y = labelY;
            }
        }

        private void DrawOrUpdateAcLabel(ChartTriangle leftTriangle, ChartTriangle rightTriangle, ChartTrendLine line, long id, ChartText label = null)
        {
            var labelTime = line.GetLineCenterTime();

            var labelY = line.GetLineCenterY();

            var acLength = rightTriangle.Y2 - leftTriangle.Y2;

            var xaLength = leftTriangle.Y2 - leftTriangle.Y1;

            var ratio = Math.Round(1 + acLength / xaLength, 3);

            if (label == null)
            {
                DrawLabelText(ratio.ToString(), labelTime, labelY, id, objectNameKey: "AC");
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
            var leftTriangle = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("Left",
                StringComparison.OrdinalIgnoreCase)) as ChartTriangle;

            var rightTriangle = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("Right",
                StringComparison.OrdinalIgnoreCase)) as ChartTriangle;

            var acLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("ACLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var xdLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("XDLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            if (leftTriangle == null || rightTriangle == null || acLine == null || xdLine == null) return;

            if (labels.Length == 0)
            {
                DrawLabels(leftTriangle, rightTriangle, acLine, xdLine, id);

                return;
            }

            foreach (var label in labels)
            {
                switch (label.Text)
                {
                    case "X":
                        label.Time = leftTriangle.Time1;
                        label.Y = leftTriangle.Y1;
                        break;

                    case "A":
                        label.Time = leftTriangle.Time2;
                        label.Y = leftTriangle.Y2;
                        break;

                    case "B":
                        label.Time = leftTriangle.Time3;
                        label.Y = leftTriangle.Y3;
                        break;

                    case "C":
                        label.Time = rightTriangle.Time2;
                        label.Y = rightTriangle.Y2;
                        break;

                    case "D":
                        label.Time = rightTriangle.Time3;
                        label.Y = rightTriangle.Y3;
                        break;
                }

                if (label.Name.EndsWith("AC", StringComparison.OrdinalIgnoreCase)) DrawOrUpdateAcLabel(leftTriangle, rightTriangle, acLine, id, label);
                if (label.Name.EndsWith("XD", StringComparison.OrdinalIgnoreCase)) DrawOrUpdateXdLabel(leftTriangle, rightTriangle, xdLine, id, label);
                if (label.Name.EndsWith("XB", StringComparison.OrdinalIgnoreCase)) DrawOrUpdateXbLabel(leftTriangle, id, label);
                if (label.Name.EndsWith("BD", StringComparison.OrdinalIgnoreCase)) DrawOrUpdateBdLabel(rightTriangle, id, label);
            }
        }
    }
}