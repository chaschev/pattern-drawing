using cAlgo.API;
using System;

namespace cAlgo.Controls
{
    public class TriangleButton : ToggleButton
    {
        private readonly Chart _chart;

        private ChartTriangle _triangle;

        private int _mouseUpNumber;

        public TriangleButton(Chart chart)
        {
            Text = "Triangle";

            Click += args =>
            {
                if (IsOn)
                {
                    TurnOff();
                }
                else
                {
                    TurnOn();
                }
            };

            _chart = chart;
        }

        protected override void OnTurnedOn()
        {
            _chart.MouseDown += Chart_MouseDown;
            _chart.MouseMove += Chart_MouseMove;
            _chart.MouseUp += Chart_MouseUp;

            _chart.IsScrollingEnabled = false;
        }

        protected override void OnTurnedOff()
        {
            _chart.MouseDown -= Chart_MouseDown;
            _chart.MouseMove -= Chart_MouseMove;
            _chart.MouseUp -= Chart_MouseUp;

            _chart.IsScrollingEnabled = true;

            _triangle = null;

            _mouseUpNumber = 0;
        }

        private void Chart_MouseUp(ChartMouseEventArgs obj)
        {
            if (_triangle == null) return;

            _mouseUpNumber++;

            if (_mouseUpNumber == 3)
            {
                TurnOff();
            }
        }

        private void Chart_MouseMove(ChartMouseEventArgs obj)
        {
            if (_triangle == null) return;

            var index = (int)obj.BarIndex;

            if (_mouseUpNumber == 1)
            {
                _triangle.Time2 = _chart.Bars.OpenTimes[index];
                _triangle.Y2 = obj.YValue;
            }
            else if (_mouseUpNumber == 2)
            {
                _triangle.Time3 = _chart.Bars.OpenTimes[index];
                _triangle.Y3 = obj.YValue;
            }
        }

        private void Chart_MouseDown(ChartMouseEventArgs obj)
        {
            var name = string.Format("Patterns_Triangle_{0}", DateTime.Now.Ticks);

            var index = (int)obj.BarIndex;

            _triangle = _chart.DrawTriangle(name, index, obj.YValue, index, obj.YValue, index, obj.YValue, Color.Red);

            _triangle.IsInteractive = true;
        }
    }
}