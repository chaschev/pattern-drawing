using cAlgo.API;
using System;

namespace cAlgo.Patterns
{
    public class TrianglePattern : PatternBase
    {
        private ChartTriangle _triangle;

        public TrianglePattern(Chart chart, Color color, bool showLabels, Color labelsColor) : base(chart, "Triangle", color, showLabels,
            labelsColor)
        {
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 3) StopDrawing();
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (_triangle == null) return;

            if (MouseUpNumber == 1)
            {
                _triangle.Time2 = obj.TimeValue;
                _triangle.Y2 = obj.YValue;
            }
            else if (MouseUpNumber == 2)
            {
                _triangle.Time3 = obj.TimeValue;
                _triangle.Y3 = obj.YValue;
            }
        }

        protected override void OnMouseDown(ChartMouseEventArgs obj)
        {
            var name = string.Format("{0}_{1}", ObjectName, DateTime.Now.Ticks);

            _triangle = Chart.DrawTriangle(name, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, Color);

            _triangle.IsInteractive = true;
            _triangle.IsFilled = true;
        }

        protected override void OnDrawingStopped()
        {
            _triangle = null;
        }

        protected override void DrawLabels()
        {
            if (_triangle == null) return;

            var aLabelName = string.Format("{0}_Label_A", _triangle.Name);
            var bLabelName = string.Format("{0}_Label_B", _triangle.Name);
            var cLabelName = string.Format("{0}_Label_C", _triangle.Name);

            var labelA = Chart.DrawText(aLabelName, "A", _triangle.Time1, _triangle.Y1, LabelsColor);

            labelA.IsInteractive = true;

            var labelB = Chart.DrawText(bLabelName, "B", _triangle.Time2, _triangle.Y2, LabelsColor);

            labelB.IsInteractive = true;

            var labelC = Chart.DrawText(cLabelName, "C", _triangle.Time3, _triangle.Y3, LabelsColor);

            labelC.IsInteractive = true;
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            if (chartObject.ObjectType != ChartObjectType.Triangle) return;

            var triangle = chartObject as ChartTriangle;

            foreach (var label in labels)
            {
                switch (label.Text)
                {
                    case "A":
                        label.Time = triangle.Time1;
                        label.Y = triangle.Y1;
                        break;

                    case "B":
                        label.Time = triangle.Time2;
                        label.Y = triangle.Y2;
                        break;

                    case "C":
                        label.Time = triangle.Time3;
                        label.Y = triangle.Y3;
                        break;
                }
            }
        }
    }
}