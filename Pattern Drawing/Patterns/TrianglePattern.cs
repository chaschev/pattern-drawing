using cAlgo.API;

namespace cAlgo.Patterns
{
    public class TrianglePattern : PatternBase
    {
        private ChartTriangle _triangle;

        public TrianglePattern(PatternConfig config) : base("Triangle", config)
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
            var name = GetObjectName();

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

            DrawLabelText("A", _triangle.Time1, _triangle.Y1);
            DrawLabelText("B", _triangle.Time2, _triangle.Y2);
            DrawLabelText("C", _triangle.Time3, _triangle.Y3);
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