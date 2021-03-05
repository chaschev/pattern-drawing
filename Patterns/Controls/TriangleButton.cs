using cAlgo.API;
using System;

namespace cAlgo.Controls
{
    public class TriangleButton : PatternDrawingButton
    {
        private ChartTriangle _triangle;

        public TriangleButton(Chart chart) : base(chart)
        {
            Text = "Triangle";
        }

        protected override void OnTurnedOff()
        {
            base.OnTurnedOff();

            _triangle = null;
        }

        protected override void Chart_MouseUp(ChartMouseEventArgs obj)
        {
            if (_triangle == null) return;

            base.Chart_MouseUp(obj);
        }

        protected override void Chart_MouseMove(ChartMouseEventArgs obj)
        {
            if (_triangle == null) return;

            base.Chart_MouseMove(obj);

            var index = (int)obj.BarIndex;

            if (MouseUpNumber == 1)
            {
                _triangle.Time2 = Chart.Bars.OpenTimes[index];
                _triangle.Y2 = obj.YValue;
            }
            else if (MouseUpNumber == 2)
            {
                _triangle.Time3 = Chart.Bars.OpenTimes[index];
                _triangle.Y3 = obj.YValue;
            }
        }

        protected override void Chart_MouseDown(ChartMouseEventArgs obj)
        {
            base.Chart_MouseDown(obj);

            var name = string.Format("Patterns_Triangle_{0}", DateTime.Now.Ticks);

            var index = (int)obj.BarIndex;

            _triangle = Chart.DrawTriangle(name, index, obj.YValue, index, obj.YValue, index, obj.YValue, Color.Red);

            _triangle.IsInteractive = true;
        }
    }
}