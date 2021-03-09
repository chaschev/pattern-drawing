using cAlgo.API;
using System;
using System.Globalization;
using System.Linq;

namespace cAlgo.Patterns
{
    public abstract class PatternBase : IPattern
    {
        private readonly Chart _chart;

        private int _mouseUpNumber;
        private string _name;
        private readonly Color _color;
        private bool _isMouseDown;
        private bool _isDrawing;

        public PatternBase(Chart chart, string name, Color color)
        {
            _chart = chart;
            _name = name;
            _color = color;

            ObjectName = string.Format("Pattern_{0}", _name.Replace(" ", "").Replace("_", ""));

            _chart.ObjectsRemoved += Chart_ObjectsRemoved;
            _chart.ObjectsUpdated += Chart_ObjectsUpdated;
        }

        private void Chart_ObjectsUpdated(ChartObjectsUpdatedEventArgs obj)
        {
            if (IsDrawing) return;

            var updatedPatternObjects = obj.ChartObjects.Where(iRemovedObject => iRemovedObject.Name.StartsWith(ObjectName,
                StringComparison.OrdinalIgnoreCase)).ToArray();

            if (updatedPatternObjects.Length == 0) return;

            Chart.ObjectsUpdated -= Chart_ObjectsUpdated;

            try
            {
                foreach (var chartObject in updatedPatternObjects)
                {
                    long id;

                    if (!TryGetChartObjectPatternId(chartObject.Name, out id))
                    {
                        continue;
                    }

                    OnPatternChartObjectsUpdated(id, chartObject);
                }
            }
            finally
            {
                Chart.ObjectsUpdated += Chart_ObjectsUpdated;
            }
        }

        public event Action<IPattern> DrawingStarted;

        public event Action<IPattern> DrawingStopped;

        protected Chart Chart
        {
            get { return _chart; }
        }

        protected int MouseUpNumber
        {
            get { return _mouseUpNumber; }
        }

        protected bool IsMouseDown
        {
            get { return _isMouseDown; }
        }

        protected Color Color
        {
            get { return _color; }
        }

        public string Name
        {
            get { return _name; }
        }

        public bool IsDrawing
        {
            get { return _isDrawing; }
        }

        public string ObjectName { get; private set; }

        public void StartDrawing()
        {
            if (IsDrawing) return;

            _isDrawing = true;

            _chart.MouseDown += Chart_MouseDown;
            _chart.MouseMove += Chart_MouseMove;
            _chart.MouseUp += Chart_MouseUp;

            _chart.IsScrollingEnabled = false;

            OnDrawingStarted();

            var drawingStarted = DrawingStarted;

            if (drawingStarted != null)
            {
                drawingStarted.Invoke(this);
            }
        }

        public void StopDrawing()
        {
            if (!IsDrawing) return;

            _isDrawing = false;

            _chart.MouseDown -= Chart_MouseDown;
            _chart.MouseMove -= Chart_MouseMove;
            _chart.MouseUp -= Chart_MouseUp;

            _chart.IsScrollingEnabled = true;

            _mouseUpNumber = 0;

            OnDrawingStopped();

            var drawingStopped = DrawingStopped;

            if (drawingStopped != null)
            {
                drawingStopped.Invoke(this);
            }
        }

        protected virtual void OnDrawingStopped()
        {
        }

        protected virtual void OnDrawingStarted()
        {
        }

        private void Chart_MouseMove(ChartMouseEventArgs obj)
        {
            OnMouseMove(obj);
        }

        private void Chart_MouseDown(ChartMouseEventArgs obj)
        {
            _isMouseDown = true;

            OnMouseDown(obj);
        }

        private void Chart_MouseUp(ChartMouseEventArgs obj)
        {
            _isMouseDown = false;

            _mouseUpNumber++;

            OnMouseUp(obj);
        }

        private void Chart_ObjectsRemoved(ChartObjectsRemovedEventArgs obj)
        {
            var removedPatternObjects = obj.ChartObjects.Where(iRemovedObject => iRemovedObject.Name.StartsWith(ObjectName,
                StringComparison.OrdinalIgnoreCase)).ToArray();

            if (removedPatternObjects.Length == 0) return;

            Chart.ObjectsRemoved -= Chart_ObjectsRemoved;

            try
            {
                foreach (var chartObject in removedPatternObjects)
                {
                    long id;

                    if (!TryGetChartObjectPatternId(chartObject.Name, out id))
                    {
                        continue;
                    }

                    RemoveObjects(id);
                }
            }
            finally
            {
                Chart.ObjectsRemoved += Chart_ObjectsRemoved;
            }
        }

        private void RemoveObjects(long id)
        {
            var patternObjectNames = string.Format("{0}_{1}", ObjectName, id);

            var chartObjects = Chart.Objects.ToArray();

            foreach (var chartObject in chartObjects)
            {
                if (chartObject.Name.StartsWith(patternObjectNames, StringComparison.OrdinalIgnoreCase))
                {
                    Chart.RemoveObject(chartObject.Name);
                }
            }
        }

        protected virtual void OnMouseMove(ChartMouseEventArgs obj)
        {
        }

        protected virtual void OnMouseDown(ChartMouseEventArgs obj)
        {
        }

        protected virtual void OnMouseUp(ChartMouseEventArgs obj)
        {
        }

        protected bool TryGetChartObjectPatternId(string chartObjectName, out long id)
        {
            var objectNameSplit = chartObjectName.Split('_');

            if (objectNameSplit.Length < 3
                || !long.TryParse(objectNameSplit[2], NumberStyles.Any, CultureInfo.InvariantCulture, out id))
            {
                id = 0;

                return false;
            }

            return true;
        }

        protected virtual void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject)
        {
        }
    }
}