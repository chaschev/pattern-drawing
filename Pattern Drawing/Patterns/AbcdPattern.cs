using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class AbcdPattern : PatternBase
    {
        private ChartTriangle _leftTriangle;
        private ChartTriangle _rightTriangle;

        public AbcdPattern(PatternConfig config) : base("ABCD", config)
        {
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            var otherTriangle = updatedChartObject as ChartTriangle;

            if (otherTriangle == null) return;

            var chartObjects = Chart.Objects.ToArray();

            var objectNameId = string.Format("{0}_{1}", ObjectName, id);

            foreach (var chartObject in chartObjects)
            {
                if (chartObject == updatedChartObject
                    || chartObject.ObjectType != ChartObjectType.Triangle
                    || !chartObject.Name.StartsWith(objectNameId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var triangle = chartObject as ChartTriangle;

                if (triangle.Name.EndsWith("Left", StringComparison.InvariantCultureIgnoreCase))
                {
                    triangle.Time3 = otherTriangle.Time1;
                    triangle.Y3 = otherTriangle.Y1;
                }
                else
                {
                    triangle.Time1 = otherTriangle.Time3;
                    triangle.Y1 = otherTriangle.Y3;
                }

                triangle.Time2 = otherTriangle.Time2;
                triangle.Y2 = otherTriangle.Y2;
            }
        }

        protected override void OnDrawingStopped()
        {
            _leftTriangle = null;
            _rightTriangle = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 4)
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

                _rightTriangle.Time2 = _leftTriangle.Time2;
                _rightTriangle.Y2 = _leftTriangle.Y2;
                _rightTriangle.Color = Color.FromArgb(70, Color);
            }
        }

        private void DrawTriangle(ChartMouseEventArgs mouseEventArgs, string name, ref ChartTriangle triangle)
        {
            triangle = Chart.DrawTriangle(name, mouseEventArgs.TimeValue, mouseEventArgs.YValue, mouseEventArgs.TimeValue,
                mouseEventArgs.YValue, mouseEventArgs.TimeValue, mouseEventArgs.YValue, Color);

            triangle.IsInteractive = true;
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
                _rightTriangle.Time3 = obj.TimeValue;
                _rightTriangle.Y3 = obj.YValue;
            }
        }

        protected override void DrawLabels()
        {
            if (_leftTriangle == null || _rightTriangle == null) return;

            DrawLabels(_leftTriangle, _rightTriangle, Id);
        }

        private void DrawLabels(ChartTriangle leftTriangle, ChartTriangle rightTriangle, long id)
        {
            DrawLabelText("A", leftTriangle.Time1, leftTriangle.Y1, id);
            DrawLabelText("B", leftTriangle.Time2, leftTriangle.Y2, id);
            DrawLabelText("C", leftTriangle.Time3, leftTriangle.Y3, id);
            DrawLabelText("D", rightTriangle.Time3, rightTriangle.Y3, id);

            DrawLabelAc(leftTriangle, id);
            DrawLabelBd(leftTriangle, rightTriangle, id);
        }

        private void DrawLabelAc(ChartTriangle leftTriangle, long id, ChartText label = null)
        {
            var abLength = leftTriangle.Y2 - leftTriangle.Y1;

            var bcLength = leftTriangle.Y2 - leftTriangle.Y3;

            var ratio = Math.Round(bcLength / abLength, 3);

            var labelTime = leftTriangle.Time1.AddMilliseconds((leftTriangle.Time3 - leftTriangle.Time1).TotalMilliseconds * 0.7);

            var labelY = leftTriangle.Y1 + ((leftTriangle.Y3 - leftTriangle.Y1) / 2);

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

        private void DrawLabelBd(ChartTriangle leftTriangle, ChartTriangle rightTriangle, long id, ChartText label = null)
        {
            var abLength = leftTriangle.Y2 - leftTriangle.Y1;

            var bdLength = rightTriangle.Y3 - rightTriangle.Y2;

            var ratio = Math.Round(1 + bdLength / abLength, 3);

            var labelTime = rightTriangle.Time2.AddMilliseconds((rightTriangle.Time3 - rightTriangle.Time2).TotalMilliseconds * 0.7);

            var labelY = rightTriangle.Y2 + ((rightTriangle.Y3 - rightTriangle.Y2) / 2);

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

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var leftTriangle = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("Left",
                StringComparison.OrdinalIgnoreCase)) as ChartTriangle;

            var rightTriangle = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("Right",
                StringComparison.OrdinalIgnoreCase)) as ChartTriangle;

            if (leftTriangle == null || rightTriangle == null) return;

            if (labels.Length == 0)
            {
                DrawLabels(leftTriangle, rightTriangle, id);

                return;
            }

            foreach (var label in labels)
            {
                switch (label.Text)
                {
                    case "A":
                        label.Time = leftTriangle.Time1;
                        label.Y = leftTriangle.Y1;
                        break;

                    case "B":
                        label.Time = leftTriangle.Time2;
                        label.Y = leftTriangle.Y2;
                        break;

                    case "C":
                        label.Time = leftTriangle.Time3;
                        label.Y = leftTriangle.Y3;
                        break;

                    case "D":
                        label.Time = rightTriangle.Time3;
                        label.Y = rightTriangle.Y3;
                        break;
                }

                if (label.Name.EndsWith("AC", StringComparison.OrdinalIgnoreCase)) DrawLabelAc(leftTriangle, id, label);
                if (label.Name.EndsWith("BD", StringComparison.OrdinalIgnoreCase)) DrawLabelBd(leftTriangle, rightTriangle, id, label);
            }
        }
    }
}