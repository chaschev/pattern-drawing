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

        private ChartTrendLine _extendedHorizontalLine;
        private ChartTrendLine _extendedVerticalLine;

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

            var mainFan = trendLines.FirstOrDefault(iLine => iLine.Name.IndexOf("1x1", StringComparison.OrdinalIgnoreCase) > -1);

            if (mainFan == null) return;

            rectangle.Time1 = mainFan.GetStartTime();
            rectangle.Time2 = mainFan.GetEndTime();

            rectangle.Y1 = mainFan.GetTopPrice();
            rectangle.Y2 = mainFan.GetBottomPrice();

            var horizontalLines = trendLines.Where(iTrendLine => iTrendLine.Name.IndexOf("HorizontalLine", StringComparison.OrdinalIgnoreCase) > -1).ToArray();

            DrawOrUpdateHorizontalLines(rectangle, horizontalLines);

            var verticalLines = trendLines.Where(iTrendLine => iTrendLine.Name.IndexOf("VerticalLine", StringComparison.OrdinalIgnoreCase) > -1).ToArray();

            DrawOrUpdateVerticalLines(rectangle, verticalLines);

            var extendedHorizontalLine = trendLines.FirstOrDefault(iTrendLine => iTrendLine.Name.IndexOf("ExtendedHorizontalLine", StringComparison.OrdinalIgnoreCase) > -1);
            var extendedVerticalLine = trendLines.FirstOrDefault(iTrendLine => iTrendLine.Name.IndexOf("ExtendedVerticalLine", StringComparison.OrdinalIgnoreCase) > -1);

            if (extendedHorizontalLine != null && extendedVerticalLine != null)
            {
                DrawOrUpdateExtendedSideLines(rectangle, mainFan, ref extendedHorizontalLine, ref extendedVerticalLine);
            }
        }

        protected override void OnDrawingStopped()
        {
            base.OnDrawingStopped();

            _rectangle = null;
            _horizontalTrendLines = null;
            _verticalTrendLines = null;
            _extendedHorizontalLine = null;
            _extendedVerticalLine = null;
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

                DrawOrUpdateExtendedSideLines(_rectangle, null, ref _extendedHorizontalLine, ref _extendedVerticalLine);
            }

            base.OnMouseMove(obj);
        }

        protected override void UpdateSideFans(ChartTrendLine mainFan, Dictionary<string, ChartTrendLine> sideFans)
        {
            var endBarIndex = mainFan.GetEndBarIndex(Chart.Bars, Chart.Symbol);

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
                    time2 = Chart.Bars.GetOpenTime(endBarIndex - (barsNumber * fanSettings.Percent), Chart.Symbol);
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

            var endBarIndex = mainFan.GetEndBarIndex(Chart.Bars, Chart.Symbol);

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
                    time2 = Chart.Bars.GetOpenTime(endBarIndex - (barsNumber * fanSettings.Percent), Chart.Symbol);
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

        private void DrawOrUpdateExtendedSideLines(ChartRectangle rectangle, ChartTrendLine mainFanLine, ref ChartTrendLine horizontalLine, ref ChartTrendLine verticalLine)
        {
            DateTime time1, time2;
            double y1, y2;

            if (mainFanLine != null)
            {
                time1 = mainFanLine.Time1;
                time2 = mainFanLine.Time2;

                y1 = mainFanLine.Y1;
                y2 = mainFanLine.Y2;
            }
            else
            {
                time1 = rectangle.Time1;
                time2 = rectangle.Time2;

                y1 = rectangle.Y1;
                y2 = rectangle.Y2;
            }

            var rectangleTimeDelta = rectangle.GetTimeDelta();

            var horizontalLineTime2 = time2 > time1 ? time2.Add(rectangleTimeDelta) : time2.Add(-rectangleTimeDelta);

            if (horizontalLine == null)
            {
                var name = GetObjectName("ExtendedHorizontalLine");

                horizontalLine = Chart.DrawTrendLine(name, time1, y1, horizontalLineTime2, y1, _settings.ExtendedLinesColor, _settings.ExtendedLinesThickness, _settings.ExtendedLinesStyle);

                horizontalLine.IsInteractive = true;
                horizontalLine.IsLocked = true;
                horizontalLine.ExtendToInfinity = true;
            }
            else
            {
                horizontalLine.Time1 = time1;
                horizontalLine.Time2 = horizontalLineTime2;
                horizontalLine.Y1 = y1;
                horizontalLine.Y2 = y1;
            }

            var rectanglePriceDelta = rectangle.GetPriceDelta();

            var verticalLineY2 = y2 > y1 ? y2 + rectanglePriceDelta : y2 - rectanglePriceDelta;

            if (verticalLine == null)
            {
                var name = GetObjectName("ExtendedVerticalLine");

                verticalLine = Chart.DrawTrendLine(name, time1, y1, time1, verticalLineY2, _settings.ExtendedLinesColor, _settings.ExtendedLinesThickness, _settings.ExtendedLinesStyle);

                verticalLine.IsInteractive = true;
                verticalLine.IsLocked = true;
                verticalLine.ExtendToInfinity = true;
            }
            else
            {
                verticalLine.Time1 = time1;
                verticalLine.Time2 = time1;
                verticalLine.Y1 = y1;
                verticalLine.Y2 = verticalLineY2;
            }
        }

        protected override void DrawLabels()
        {
            if (_rectangle == null || _horizontalTrendLines == null || _verticalTrendLines == null) return;

            DrawLabels(_rectangle, _horizontalTrendLines, _verticalTrendLines, Id);
        }

        private void DrawLabels(ChartRectangle rectangle, ChartTrendLine[] horizontalLines, ChartTrendLine[] verticalLines, long id)
        {
            var timeDistance = -TimeSpan.FromHours(Chart.Bars.GetTimeDiff().TotalHours * 2);

            DrawLabelText("0", rectangle.Time1, rectangle.Y1, id, objectNameKey: "0.0", fontSize: 10, color: _settings.MainFanSettings.Color);
            DrawLabelText("1", rectangle.Time1, rectangle.Y2, id, objectNameKey: "1.1", fontSize: 10, color: _settings.MainFanSettings.Color);

            DrawLabelText("0", rectangle.Time1.Add(timeDistance), rectangle.Y1, id, objectNameKey: "0.2", fontSize: 10, color: _settings.MainFanSettings.Color);
            DrawLabelText("1", rectangle.Time2.Add(timeDistance), rectangle.Y1, id, objectNameKey: "1.3", fontSize: 10, color: _settings.MainFanSettings.Color);

            DrawLabelText("0", rectangle.Time2, rectangle.Y1, id, objectNameKey: "0.4", fontSize: 10, color: _settings.MainFanSettings.Color);
            DrawLabelText("1", rectangle.Time2, rectangle.Y2, id, objectNameKey: "1.5", fontSize: 10, color: _settings.MainFanSettings.Color);

            DrawLabelText("0", rectangle.Time1.Add(timeDistance), rectangle.Y2, id, objectNameKey: "0.6", fontSize: 10, color: _settings.MainFanSettings.Color);
            DrawLabelText("1", rectangle.Time2.Add(timeDistance), rectangle.Y2, id, objectNameKey: "1.7", fontSize: 10, color: _settings.MainFanSettings.Color);

            for (var i = 0; i < horizontalLines.Length; i++)
            {
                var horizontalLine = horizontalLines[i];

                var fanSettings = _settings.SideFanSettings[i];

                var text = fanSettings.Percent.ToString();
                var color = fanSettings.Color;

                switch (i)
                {
                    case 0:
                        DrawLabelText(text, horizontalLine.Time1, horizontalLine.Y1, id, objectNameKey: "Horizontal1.0", fontSize: 10, color: color);
                        DrawLabelText(text, horizontalLine.Time2, horizontalLine.Y2, id, objectNameKey: "Horizontal1.1", fontSize: 10, color: color);

                        break;

                    case 1:
                        DrawLabelText(text, horizontalLine.Time1, horizontalLine.Y1, id, objectNameKey: "Horizontal2.0", fontSize: 10, color: color);
                        DrawLabelText(text, horizontalLine.Time2, horizontalLine.Y2, id, objectNameKey: "Horizontal2.1", fontSize: 10, color: color);

                        break;

                    case 2:
                        DrawLabelText(text, horizontalLine.Time1, horizontalLine.Y1, id, objectNameKey: "Horizontal3.0", fontSize: 10, color: color);
                        DrawLabelText(text, horizontalLine.Time2, horizontalLine.Y2, id, objectNameKey: "Horizontal3.1", fontSize: 10, color: color);

                        break;

                    case 3:
                        DrawLabelText(text, horizontalLine.Time1, horizontalLine.Y1, id, objectNameKey: "Horizontal4.0", fontSize: 10, color: color);
                        DrawLabelText(text, horizontalLine.Time2, horizontalLine.Y2, id, objectNameKey: "Horizontal4.1", fontSize: 10, color: color);

                        break;

                    case 4:
                        DrawLabelText(text, horizontalLine.Time1, horizontalLine.Y1, id, objectNameKey: "Horizontal5.0", fontSize: 10, color: color);
                        DrawLabelText(text, horizontalLine.Time2, horizontalLine.Y2, id, objectNameKey: "Horizontal5.1", fontSize: 10, color: color);

                        break;
                }
            }

            for (var i = 0; i < verticalLines.Length; i++)
            {
                var verticalLine = verticalLines[i];

                var fanSettings = _settings.SideFanSettings[_settings.SideFanSettings.Length - i - 1];

                var text = Math.Abs(fanSettings.Percent).ToString();
                var color = fanSettings.Color;

                switch (i)
                {
                    case 0:

                        DrawLabelText(text, verticalLine.Time1, verticalLine.Y1, id, objectNameKey: "Vertical1.0", fontSize: 10, color: color);
                        DrawLabelText(text, verticalLine.Time2, verticalLine.Y2, id, objectNameKey: "Vertical1.1", fontSize: 10, color: color);
                        break;

                    case 1:

                        DrawLabelText(text, verticalLine.Time1, verticalLine.Y1, id, objectNameKey: "Vertical2.0", fontSize: 10, color: color);
                        DrawLabelText(text, verticalLine.Time2, verticalLine.Y2, id, objectNameKey: "Vertical2.1", fontSize: 10, color: color);
                        break;

                    case 2:

                        DrawLabelText(text, verticalLine.Time1, verticalLine.Y1, id, objectNameKey: "Vertical3.0", fontSize: 10, color: color);
                        DrawLabelText(text, verticalLine.Time2, verticalLine.Y2, id, objectNameKey: "Vertical3.1", fontSize: 10, color: color);
                        break;

                    case 3:

                        DrawLabelText(text, verticalLine.Time1, verticalLine.Y1, id, objectNameKey: "Vertical4.0", fontSize: 10, color: color);
                        DrawLabelText(text, verticalLine.Time2, verticalLine.Y2, id, objectNameKey: "Vertical4.1", fontSize: 10, color: color);
                        break;

                    case 4:

                        DrawLabelText(text, verticalLine.Time1, verticalLine.Y1, id, objectNameKey: "Vertical5.0", fontSize: 10, color: color);
                        DrawLabelText(text, verticalLine.Time2, verticalLine.Y2, id, objectNameKey: "Vertical5.1", fontSize: 10, color: color);
                        break;
                }
            }
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var rectangle = patternObjects.FirstOrDefault(iObject => iObject is ChartRectangle) as ChartRectangle;

            var trendLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine).Cast<ChartTrendLine>();

            var horizontalLines = trendLines.Where(iTrendLine => iTrendLine.Name.IndexOf("HorizontalLine", StringComparison.OrdinalIgnoreCase) > -1).ToArray();

            var verticalLines = trendLines.Where(iTrendLine => iTrendLine.Name.IndexOf("VerticalLine", StringComparison.OrdinalIgnoreCase) > -1).ToArray();

            if (rectangle == null || horizontalLines == null || verticalLines == null) return;

            if (labels.Length == 0)
            {
                DrawLabels(rectangle, horizontalLines, verticalLines, id);

                return;
            }

            ChartTrendLine chartTrendLine;

            foreach (var label in labels)
            {
                var labelKey = label.Name.Split('_').Last();

                switch (labelKey)
                {
                    case "Horizontal1.0":
                        chartTrendLine = horizontalLines.First(iLine => iLine.Name.EndsWith("HorizontalLine1", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time1;
                        label.Y = chartTrendLine.Y1;
                        break;

                    case "Horizontal1.1":
                        chartTrendLine = horizontalLines.First(iLine => iLine.Name.EndsWith("HorizontalLine1", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time2;
                        label.Y = chartTrendLine.Y2;
                        break;

                    case "Horizontal2.0":
                        chartTrendLine = horizontalLines.First(iLine => iLine.Name.EndsWith("HorizontalLine2", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time1;
                        label.Y = chartTrendLine.Y1;
                        break;

                    case "Horizontal2.1":
                        chartTrendLine = horizontalLines.First(iLine => iLine.Name.EndsWith("HorizontalLine2", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time2;
                        label.Y = chartTrendLine.Y2;
                        break;

                    case "Horizontal3.0":
                        chartTrendLine = horizontalLines.First(iLine => iLine.Name.EndsWith("HorizontalLine3", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time1;
                        label.Y = chartTrendLine.Y1;
                        break;

                    case "Horizontal3.1":
                        chartTrendLine = horizontalLines.First(iLine => iLine.Name.EndsWith("HorizontalLine3", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time2;
                        label.Y = chartTrendLine.Y2;
                        break;

                    case "Horizontal4.0":
                        chartTrendLine = horizontalLines.First(iLine => iLine.Name.EndsWith("HorizontalLine4", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time1;
                        label.Y = chartTrendLine.Y1;
                        break;

                    case "Horizontal4.1":
                        chartTrendLine = horizontalLines.First(iLine => iLine.Name.EndsWith("HorizontalLine4", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time2;
                        label.Y = chartTrendLine.Y2;
                        break;

                    case "Horizontal5.0":
                        chartTrendLine = horizontalLines.First(iLine => iLine.Name.EndsWith("HorizontalLine5", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time1;
                        label.Y = chartTrendLine.Y1;
                        break;

                    case "Horizontal5.1":
                        chartTrendLine = horizontalLines.First(iLine => iLine.Name.EndsWith("HorizontalLine5", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time2;
                        label.Y = chartTrendLine.Y2;
                        break;

                    case "Vertical1.0":
                        chartTrendLine = verticalLines.First(iLine => iLine.Name.EndsWith("VerticalLine1", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time1;
                        label.Y = chartTrendLine.Y1;
                        break;

                    case "Vertical1.1":
                        chartTrendLine = verticalLines.First(iLine => iLine.Name.EndsWith("VerticalLine1", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time2;
                        label.Y = chartTrendLine.Y2;
                        break;

                    case "Vertical2.0":
                        chartTrendLine = verticalLines.First(iLine => iLine.Name.EndsWith("VerticalLine2", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time1;
                        label.Y = chartTrendLine.Y1;
                        break;

                    case "Vertical2.1":
                        chartTrendLine = verticalLines.First(iLine => iLine.Name.EndsWith("VerticalLine2", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time2;
                        label.Y = chartTrendLine.Y2;
                        break;

                    case "Vertical3.0":
                        chartTrendLine = verticalLines.First(iLine => iLine.Name.EndsWith("VerticalLine3", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time1;
                        label.Y = chartTrendLine.Y1;
                        break;

                    case "Vertical3.1":
                        chartTrendLine = verticalLines.First(iLine => iLine.Name.EndsWith("VerticalLine3", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time2;
                        label.Y = chartTrendLine.Y2;
                        break;

                    case "Vertical4.0":
                        chartTrendLine = verticalLines.First(iLine => iLine.Name.EndsWith("VerticalLine4", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time1;
                        label.Y = chartTrendLine.Y1;
                        break;

                    case "Vertical4.1":
                        chartTrendLine = verticalLines.First(iLine => iLine.Name.EndsWith("VerticalLine4", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time2;
                        label.Y = chartTrendLine.Y2;
                        break;

                    case "Vertical5.0":
                        chartTrendLine = verticalLines.First(iLine => iLine.Name.EndsWith("VerticalLine5", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time1;
                        label.Y = chartTrendLine.Y1;
                        break;

                    case "Vertical5.1":
                        chartTrendLine = verticalLines.First(iLine => iLine.Name.EndsWith("VerticalLine5", StringComparison.OrdinalIgnoreCase));

                        label.Time = chartTrendLine.Time2;
                        label.Y = chartTrendLine.Y2;
                        break;

                    default:
                        {
                            var timeDistance = -TimeSpan.FromHours(Chart.Bars.GetTimeDiff().TotalHours * 2);

                            switch (labelKey)
                            {
                                case "0.0":
                                    label.Time = rectangle.Time1;
                                    label.Y = rectangle.Y1;
                                    break;

                                case "1.1":
                                    label.Time = rectangle.Time1;
                                    label.Y = rectangle.Y2;
                                    break;

                                case "0.2":
                                    label.Time = rectangle.Time1.Add(timeDistance);
                                    label.Y = rectangle.Y1;
                                    break;

                                case "1.3":
                                    label.Time = rectangle.Time2.Add(timeDistance);
                                    label.Y = rectangle.Y1;
                                    break;

                                case "0.4":
                                    label.Time = rectangle.Time2;
                                    label.Y = rectangle.Y1;
                                    break;

                                case "1.5":
                                    label.Time = rectangle.Time2;
                                    label.Y = rectangle.Y2;
                                    break;

                                case "0.6":
                                    label.Time = rectangle.Time1.Add(timeDistance);
                                    label.Y = rectangle.Y2;
                                    break;

                                case "1.7":
                                    label.Time = rectangle.Time2.Add(timeDistance);
                                    label.Y = rectangle.Y2;
                                    break;
                            }

                            break;
                        }
                }
            }
        }
    }
}