using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cAlgo.Patterns
{
    public class OriginalPitchforkPattern : PatternBase
    {
        private readonly Dictionary<double, ChartTrendLine> _horizontalTrendLines = new Dictionary<double, ChartTrendLine>();
        private readonly Dictionary<double, ChartTrendLine> _verticalTrendLines = new Dictionary<double, ChartTrendLine>();
        private readonly LineSettings _medianLineSettings;
        private readonly Dictionary<double, PercentLineSettings> _levelsSettings;
        private ChartTrendLine _medianLine;
        private ChartTrendLine _controllerLine;

        public OriginalPitchforkPattern(PatternConfig config, LineSettings medianLineSettings, Dictionary<double, PercentLineSettings> levelsSettings) : base("Original Pitchfork", config)
        {
            _medianLineSettings = medianLineSettings;
            _levelsSettings = levelsSettings;
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            var trendLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine).Cast<ChartTrendLine>();

            var mainFan = trendLines.FirstOrDefault(iLine => iLine.Name.IndexOf("MainFan", StringComparison.OrdinalIgnoreCase) > -1);

            if (mainFan == null) return;
        }

        protected override void OnDrawingStopped()
        {
            _medianLine = null;
            _controllerLine = null;

            _horizontalTrendLines.Clear();
            _verticalTrendLines.Clear();
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 3)
            {
                FinishDrawing();

                return;
            }

            if (_medianLine == null)
            {
                var name = GetObjectName("MedianLine");

                _medianLine = Chart.DrawTrendLine(name, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, _medianLineSettings.LineColor, _medianLineSettings.Thickness, _medianLineSettings.Style);

                _medianLine.IsInteractive = true;
                _medianLine.ExtendToInfinity = true;
            }
            else if (_controllerLine == null)
            {
                var name = GetObjectName("ControllerLine");

                _controllerLine = Chart.DrawTrendLine(name, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, _medianLineSettings.LineColor, _medianLineSettings.Thickness, _medianLineSettings.Style);

                _controllerLine.IsInteractive = true;
            }
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (_medianLine == null) return;

            if (_controllerLine == null)
            {
                _medianLine.Time2 = obj.TimeValue;
                _medianLine.Y2 = obj.YValue;
            }
            else
            {
                _controllerLine.Time2 = obj.TimeValue;
                _controllerLine.Y2 = obj.YValue;

                _medianLine.Time2 = _controllerLine.GetStartTime().AddTicks(_controllerLine.GetTimeDelta().Ticks / 2);
                _medianLine.Y2 = _controllerLine.GetBottomPrice() + _controllerLine.GetPriceDelta() / 2;

                DrawPercentLevels(_medianLine, _controllerLine);
            }
        }

        protected override ChartObject[] GetFrontObjects()
        {
            return new ChartObject[] { _medianLine, _controllerLine };
        }

        private void DrawPercentLevels(ChartTrendLine medianLine, ChartTrendLine controllerLine)
        {
            foreach (var levelSettings in _levelsSettings)
            {
                var topLevelName = GetObjectName(string.Format("Level_{0}_Top", levelSettings.Key));

                //var topLevel = Chart.DrawTrendLine(topLevelName, )
            }
        }
    }
}