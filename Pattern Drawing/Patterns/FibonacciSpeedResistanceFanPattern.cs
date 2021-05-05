using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cAlgo.Patterns
{
    public class FibonacciSpeedResistanceFanPattern : FanPatternBase
    {
        private ChartRectangle _rectangle;

        private ChartTrendLine[] _horizontalTrendLines;
        private ChartTrendLine[] _verticalTrendLines;

        private readonly FibonacciSpeedResistanceFanSettings _settings;

        public FibonacciSpeedResistanceFanPattern(PatternConfig config, FibonacciSpeedResistanceFanSettings settings) : base("Fibonacci Speed Resistance Fan", config, settings.SideFanSettings, settings.MainFanSettings)
        {
            _settings = settings;
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            base.OnPatternChartObjectsUpdated(id, updatedChartObject, patternObjects);

            var rectangle = patternObjects.FirstOrDefault(iObject => iObject.ObjectType == ChartObjectType.Rectangle) as ChartRectangle;

            if (rectangle == null) return;

            var trendLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine).Cast<ChartTrendLine>();

            var mainFan = trendLines.First(iLine => iLine.Name.IndexOf("1x1", StringComparison.OrdinalIgnoreCase) > -1);

            rectangle.Time1 = mainFan.GetStartTime();
            rectangle.Time2 = mainFan.GetEndTime();

            rectangle.Y1 = mainFan.GetTopPrice();
            rectangle.Y2 = mainFan.GetBottomPrice();

            var horizontalLines = trendLines.Where(iTrendLine => iTrendLine.Name.IndexOf("HorizontalLine", StringComparison.OrdinalIgnoreCase) > -1).ToArray();

            DrawOrUpdateHorizontalLines(rectangle, horizontalLines);

            var verticalLines = trendLines.Where(iTrendLine => iTrendLine.Name.IndexOf("VerticalLine", StringComparison.OrdinalIgnoreCase) > -1).ToArray();

            DrawOrUpdateVerticalLines(rectangle, verticalLines);
        }

        protected override void OnDrawingStopped()
        {
            base.OnDrawingStopped();

            _rectangle = null;
            _horizontalTrendLines = null;
            _verticalTrendLines = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (_rectangle == null)
            {
                var name = GetObjectName("Rectangle");

                _rectangle = Chart.DrawRectangle(name, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, _settings.RectangleColor, _settings.RectangleThickness, _settings.RectangleStyle);

                _rectangle.IsInteractive = true;
                _rectangle.IsLocked = true;
            }

            base.OnMouseUp(obj);
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (_rectangle != null)
            {
                _rectangle.Time2 = obj.TimeValue;
                _rectangle.Y2 = obj.YValue;

                _horizontalTrendLines = new ChartTrendLine[_settings.SideFanSettings.Where(iSideFan => iSideFan.Percent > 0).Count()];

                DrawOrUpdateHorizontalLines(_rectangle, _horizontalTrendLines);

                _verticalTrendLines = new ChartTrendLine[_settings.SideFanSettings.Where(iSideFan => iSideFan.Percent < 0).Count()];

                DrawOrUpdateVerticalLines(_rectangle, _verticalTrendLines);
            }

            base.OnMouseMove(obj);
        }

        protected override void UpdateSideFans(ChartTrendLine mainFan, Dictionary<string, ChartTrendLine> sideFans)
        {
            var startBarIndex = mainFan.GetStartBarIndex(Chart.Bars, Chart.Symbol);

            var barsNumber = mainFan.GetBarsNumber(Chart.Bars, Chart.Symbol);

            var mainFanPriceDelta = Math.Abs(mainFan.Y2 - mainFan.Y1);

            for (var iFan = 0; iFan < SideFanSettings.Length; iFan++)
            {
                var fanSettings = SideFanSettings[iFan];

                double y2;
                DateTime time2;

                if (fanSettings.Percent < 0)
                {
                    var yAmount = mainFanPriceDelta * fanSettings.Percent;

                    y2 = mainFan.Y2 > mainFan.Y1 ? mainFan.Y2 + yAmount : mainFan.Y2 - yAmount;

                    time2 = mainFan.Time2;
                }
                else
                {
                    y2 = mainFan.Y2;
                    time2 = Chart.Bars.GetOpenTime(startBarIndex + (barsNumber * fanSettings.Percent), Chart.Symbol);
                }

                ChartTrendLine fanLine;

                if (!sideFans.TryGetValue(fanSettings.Name, out fanLine)) continue;

                fanLine.Time1 = mainFan.Time1;
                fanLine.Time2 = time2;

                fanLine.Y1 = mainFan.Y1;
                fanLine.Y2 = y2;
            }
        }

        protected override void DrawSideFans(ChartTrendLine mainFan)
        {
            var mainFanPriceDelta = Math.Abs(mainFan.Y2 - mainFan.Y1);

            var startBarIndex = mainFan.GetStartBarIndex(Chart.Bars, Chart.Symbol);

            var barsNumber = mainFan.GetBarsNumber(Chart.Bars, Chart.Symbol);

            for (var iFan = 0; iFan < SideFanSettings.Length; iFan++)
            {
                var fanSettings = SideFanSettings[iFan];

                double y2;
                DateTime time2;

                if (fanSettings.Percent < 0)
                {
                    var yAmount = mainFanPriceDelta * fanSettings.Percent;

                    y2 = mainFan.Y2 > mainFan.Y1 ? mainFan.Y2 + yAmount : mainFan.Y2 - yAmount;

                    time2 = mainFan.Time2;
                }
                else
                {
                    y2 = mainFan.Y2;
                    time2 = Chart.Bars.GetOpenTime(startBarIndex + (barsNumber * fanSettings.Percent), Chart.Symbol);
                }

                var objectName = GetObjectName(fanSettings.Name);

                var trendLine = Chart.DrawTrendLine(objectName, mainFan.Time1, mainFan.Y1, time2, y2, fanSettings.Color, fanSettings.Thickness, fanSettings.Style);

                trendLine.IsInteractive = true;
                trendLine.IsLocked = true;
                trendLine.ExtendToInfinity = true;

                SideFanLines[fanSettings.Name] = trendLine;
            }
        }

        private void DrawOrUpdateHorizontalLines(ChartRectangle rectangle, ChartTrendLine[] horizontalLines)
        {
            var startTime = rectangle.GetStartTime();
            var endTime = rectangle.GetEndTime();

            var verticalDelta = rectangle.GetPriceDelta();

            var lineLevels = _settings.SideFanSettings.Where(iSideFan => iSideFan.Percent < 0).Select(iSideFan => Math.Abs(iSideFan.Percent * verticalDelta)).ToArray();

            for (int i = 0; i < lineLevels.Length; i++)
            {
                var level = rectangle.Y2 > rectangle.Y1 ? rectangle.Y1 + lineLevels[i] : rectangle.Y1 - lineLevels[i];

                var horizontalLine = horizontalLines[i];

                if (horizontalLine == null)
                {
                    var objectName = GetObjectName(string.Format("HorizontalLine{0}", i + 1));

                    horizontalLines[i] = Chart.DrawTrendLine(objectName, startTime, level, endTime, level, _settings.PriceLevelsColor, _settings.PriceLevelsThickness, _settings.PriceLevelsStyle);

                    horizontalLines[i].IsInteractive = true;
                    horizontalLines[i].IsLocked = true;
                }
                else
                {
                    horizontalLine.Time1 = startTime;
                    horizontalLine.Time2 = endTime;

                    horizontalLine.Y1 = level;
                    horizontalLine.Y2 = level;
                }
            }
        }

        private void DrawOrUpdateVerticalLines(ChartRectangle rectangle, ChartTrendLine[] verticalLines)
        {
            var startBarIndex = rectangle.GetStartBarIndex(Chart.Bars, Chart.Symbol);

            var barsNumber = rectangle.GetBarsNumber(Chart.Bars, Chart.Symbol);

            var lineLevels = _settings.SideFanSettings.Where(iSideFan => iSideFan.Percent > 0).Select(iSideFan => iSideFan.Percent * barsNumber).ToArray();

            var rectangleEndTime = rectangle.GetEndTime();

            for (int i = 0; i < lineLevels.Length; i++)
            {
                var barIndex = startBarIndex + lineLevels[i];

                var time = Chart.Bars.GetOpenTime(barIndex, Chart.Symbol);

                if (time > rectangleEndTime)
                {
                    time = rectangleEndTime;
                }

                var verticalLine = verticalLines[i];

                if (verticalLine == null)
                {
                    var objectName = GetObjectName(string.Format("VerticalLine{0}", i + 1));

                    verticalLines[i] = Chart.DrawTrendLine(objectName, time, rectangle.Y1, time, rectangle.Y2, _settings.TimeLevelsColor, _settings.TimeLevelsThickness, _settings.TimeLevelsStyle);

                    verticalLines[i].IsInteractive = true;
                    verticalLines[i].IsLocked = true;
                }
                else
                {
                    verticalLine.Time1 = time;
                    verticalLine.Time2 = time;

                    verticalLine.Y1 = rectangle.Y1;
                    verticalLine.Y2 = rectangle.Y2;
                }
            }
        }

        protected override void DrawLabels()
        {
            if (_rectangle == null) return;

            DrawLabels(_rectangle, Id);
        }

        private void DrawLabels(ChartRectangle rectangle, long id)
        {
            DrawLabelText(Math.Round(rectangle.GetPriceDelta(), Chart.Symbol.Digits).ToString(), rectangle.GetStartTime(), rectangle.GetTopPrice(), id, objectNameKey: "Price", fontSize: 10);
            DrawLabelText(rectangle.GetBarsNumber(Chart.Bars, Chart.Symbol).ToString(), rectangle.GetEndTime(), rectangle.GetBottomPrice(), id, objectNameKey: "BarsNumber", fontSize: 10);
            DrawLabelText(rectangle.GetPriceToBarsRatio(Chart.Bars, Chart.Symbol).ToString("0." + new string('#', 339)), rectangle.GetEndTime(), rectangle.GetTopPrice(), id, objectNameKey: "PriceToBarsRatio", fontSize: 10);
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var rectangle = patternObjects.FirstOrDefault(iObject => iObject is ChartRectangle) as ChartRectangle;

            var trendLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine).Cast<ChartTrendLine>();

            if (rectangle == null) return;

            if (labels.Length == 0)
            {
                DrawLabels(rectangle, id);

                return;
            }

            foreach (var label in labels)
            {
                var labelKey = label.Name.Split('_').Last();

                switch (labelKey)
                {
                    case "Price":
                        label.Text = Math.Round(rectangle.GetPriceDelta(), Chart.Symbol.Digits).ToString();
                        label.Time = rectangle.GetStartTime();
                        label.Y = rectangle.GetTopPrice();
                        break;

                    case "BarsNumber":
                        label.Text = rectangle.GetBarsNumber(Chart.Bars, Chart.Symbol).ToString();
                        label.Time = rectangle.GetEndTime();
                        label.Y = rectangle.GetBottomPrice();
                        break;

                    case "PriceToBarsRatio":
                        label.Text = rectangle.GetPriceToBarsRatio(Chart.Bars, Chart.Symbol).ToString();
                        label.Time = rectangle.GetEndTime();
                        label.Y = rectangle.GetTopPrice();
                        break;
                }
            }
        }
    }
}