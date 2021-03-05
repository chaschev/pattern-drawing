using cAlgo.API;

namespace cAlgo.Controls
{
    public class PatternDrawingButton : ToggleButton
    {
        private readonly Chart _chart;

        private int _mouseUpNumber;

        public PatternDrawingButton(Chart chart)
        {
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

        protected Chart Chart
        {
            get { return _chart; }
        }

        protected int MouseUpNumber
        {
            get { return _mouseUpNumber; }
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

            _mouseUpNumber = 0;
        }

        protected virtual void Chart_MouseMove(ChartMouseEventArgs obj)
        {
        }

        protected virtual void Chart_MouseDown(ChartMouseEventArgs obj)
        {
        }

        protected virtual void Chart_MouseUp(ChartMouseEventArgs obj)
        {
            _mouseUpNumber++;

            if (_mouseUpNumber == 3)
            {
                TurnOff();
            }
        }
    }
}