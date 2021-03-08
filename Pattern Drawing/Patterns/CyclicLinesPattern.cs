using cAlgo.API;
using System;
using System.Globalization;
using System.Linq;

namespace cAlgo.Patterns
{
    public class CyclicLinesPattern : PatternBase
    {
        private int? _mouseDownBarIndex;

        private long _id;

        public CyclicLinesPattern(Chart chart, Color color) : base(chart, "Cyclic Lines", color)
        {
            Chart.ObjectsRemoved += Chart_ObjectsRemoved;

            DrawingStopped += args => _mouseDownBarIndex = null;
        }

        private void Chart_ObjectsRemoved(ChartObjectsRemovedEventArgs obj)
        {
            var removedCycleLines = obj.ChartObjects.Where(iRemovedObject => iRemovedObject.Name.StartsWith("Pattern_Cyclic_Lines",
                StringComparison.OrdinalIgnoreCase)).ToArray();

            if (removedCycleLines.Length == 0) return;

            Chart.ObjectsRemoved -= Chart_ObjectsRemoved;

            try
            {
                foreach (var cycleLine in removedCycleLines)
                {
                    var cycleLineNameSplit = cycleLine.Name.Split('_');

                    long id;

                    if (cycleLineNameSplit.Length < 4
                        || !long.TryParse(cycleLineNameSplit[3], NumberStyles.Any, CultureInfo.InvariantCulture, out id))
                    {
                        continue;
                    }

                    RemoveCycles(id);
                }
            }
            finally
            {
                Chart.ObjectsRemoved += Chart_ObjectsRemoved;
            }
        }

        private void RemoveCycles(long id)
        {
            var cyclesNameId = string.Format("Pattern_Cyclic_Lines_{0}", id);

            var chartObjects = Chart.Objects.ToArray();

            foreach (var chartObject in chartObjects)
            {
                if (chartObject.Name.StartsWith(cyclesNameId, StringComparison.OrdinalIgnoreCase))
                {
                    Chart.RemoveObject(chartObject.Name);
                }
            }
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 2)
            {
                StopDrawing();
            }
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (!_mouseDownBarIndex.HasValue) return;

            var mouseMoveBarIndex = (int)obj.BarIndex;

            var diff = mouseMoveBarIndex - _mouseDownBarIndex.Value;

            for (int i = 0; i < 100; i++)
            {
                var name = string.Format("Pattern_Cyclic_Lines_{0}_{1}", _id, i);

                var lineIndex = _mouseDownBarIndex.Value + (diff * i);

                var verticalLine = Chart.DrawVerticalLine(name, lineIndex, Color);

                verticalLine.IsInteractive = true;
            }
        }

        protected override void OnMouseDown(ChartMouseEventArgs obj)
        {
            if (_mouseDownBarIndex.HasValue) return;

            _mouseDownBarIndex = (int)obj.BarIndex;

            _id = DateTime.Now.Ticks;
        }
    }
}