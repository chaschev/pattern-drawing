using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class AbcdPattern : PatternBase
    {
        private ChartTriangle _leftTriangle;
        private ChartTriangle _rightTriangle;

        private long _id;

        public AbcdPattern(Chart chart, Color color, bool showLabels, Color labelsColor) : base(chart, "ABCD", color, showLabels, labelsColor)
        {
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject)
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
                StopDrawing();

                return;
            }

            if (_leftTriangle == null)
            {
                _id = DateTime.Now.Ticks;

                var name = string.Format("{0}_{1}_Left", ObjectName, _id);

                DrawTriangle(obj, name, ref _leftTriangle);
            }
            else if (_rightTriangle == null && MouseUpNumber == 3)
            {
                var name = string.Format("{0}_{1}_Right", ObjectName, _id);

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
                _rightTriangle.Time3 = obj.TimeValue;
                _rightTriangle.Y3 = obj.YValue;
            }
        }

        protected override void DrawLabels()
        {
            if (_leftTriangle == null || _rightTriangle == null) return;

            var aLabelName = string.Format("{0}_Label_A", _leftTriangle.Name);
            var bLabelName = string.Format("{0}_Label_B", _leftTriangle.Name);
            var cLabelName = string.Format("{0}_Label_C", _rightTriangle.Name);
            var dLabelName = string.Format("{0}_Label_D", _rightTriangle.Name);

            var labelA = Chart.DrawText(aLabelName, "A", _leftTriangle.Time1, _leftTriangle.Y1, LabelsColor);

            labelA.IsInteractive = true;

            var labelB = Chart.DrawText(bLabelName, "B", _leftTriangle.Time2, _leftTriangle.Y2, LabelsColor);

            labelB.IsInteractive = true;

            var labelC = Chart.DrawText(cLabelName, "C", _leftTriangle.Time3, _leftTriangle.Y3, LabelsColor);

            labelC.IsInteractive = true;

            var labelD = Chart.DrawText(dLabelName, "D", _rightTriangle.Time3, _rightTriangle.Y3, LabelsColor);

            labelD.IsInteractive = true;
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var leftTriangle = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("Left",
                StringComparison.OrdinalIgnoreCase)) as ChartTriangle;

            var rightTriangle = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("Right",
                StringComparison.OrdinalIgnoreCase)) as ChartTriangle;

            if (leftTriangle == null || rightTriangle == null) return;

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
            }
        }
    }
}