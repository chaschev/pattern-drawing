using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class GannSquarePattern : PatternBase
    {
        private ChartRectangle _rectangle;

        private ChartTrendLine[] _horizontalTrendLines;
        private ChartTrendLine[] _verticalTrendLines;

        private ChartTrendLine _middleLine;

        private ChartTrendLine _topLine;

        private ChartTrendLine _bottomLine;

        public GannSquarePattern(PatternConfig config) : base("Gann Square", config)
        {
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            if (updatedChartObject.ObjectType != ChartObjectType.Rectangle) return;

            var rectangle = updatedChartObject as ChartRectangle;

            var trendLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine).Cast<ChartTrendLine>();

            var horizontalLines = trendLines.Where(iTrendLine => iTrendLine.Name.IndexOf("HorizontalLine", StringComparison.OrdinalIgnoreCase) > -1).ToArray();

            DrawOrUpdateHorizontalLines(rectangle, horizontalLines);

            var verticalLines = trendLines.Where(iTrendLine => iTrendLine.Name.IndexOf("VerticalLine", StringComparison.OrdinalIgnoreCase) > -1).ToArray();

            DrawOrUpdateVerticalLines(rectangle, verticalLines);

            var middleLine = trendLines.FirstOrDefault(iTrendLine => iTrendLine.Name.IndexOf("MiddleLine", StringComparison.OrdinalIgnoreCase) > -1);
            var topLine = trendLines.FirstOrDefault(iTrendLine => iTrendLine.Name.IndexOf("TopLine", StringComparison.OrdinalIgnoreCase) > -1);
            var bottomLine = trendLines.FirstOrDefault(iTrendLine => iTrendLine.Name.IndexOf("BottomLine", StringComparison.OrdinalIgnoreCase) > -1);

            DrawOrUpdateOtherLines(rectangle, ref middleLine, ref topLine, ref bottomLine);
        }

        protected override void OnDrawingStopped()
        {
            _rectangle = null;
            _horizontalTrendLines = null;
            _verticalTrendLines = null;

            _middleLine = null;
            _topLine = null;
            _bottomLine = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 2)
            {
                FinishDrawing();

                return;
            }

            if (_rectangle == null)
            {
                var name = GetObjectName("Rectangle");

                _rectangle = Chart.DrawRectangle(name, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, Color);

                _rectangle.IsInteractive = true;
            }
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber > 1 || _rectangle == null) return;

            _rectangle.Time2 = obj.TimeValue;
            _rectangle.Y2 = obj.YValue;

            _horizontalTrendLines = new ChartTrendLine[4];

            DrawOrUpdateHorizontalLines(_rectangle, _horizontalTrendLines);

            _verticalTrendLines = new ChartTrendLine[4];

            DrawOrUpdateVerticalLines(_rectangle, _verticalTrendLines);

            DrawOrUpdateOtherLines(_rectangle, ref _middleLine, ref _topLine, ref _bottomLine);
        }

        private void DrawOrUpdateOtherLines(ChartRectangle rectangle, ref ChartTrendLine middleLine, ref ChartTrendLine topLine, ref ChartTrendLine bottomLine)
        {
            var startTime = rectangle.GetStartTime();
            var endTime = rectangle.GetEndTime();

            var topPrice = rectangle.GetTopPrice();
            var bottomPrice = rectangle.GetBottomPrice();

            if (middleLine == null)
            {
                var objectName = GetObjectName("MiddleLine");

                middleLine = Chart.DrawTrendLine(objectName, startTime, bottomPrice, endTime, topPrice, Color);

                middleLine.IsInteractive = true;
                middleLine.IsLocked = true;
            }
            else
            {
                middleLine.Time1 = startTime;
                middleLine.Time2 = endTime;

                middleLine.Y1 = bottomPrice;
                middleLine.Y2 = topPrice;
            }

            var startBarIndex = rectangle.GetStartBarIndex(Chart.Bars);
            var endBarIndex = rectangle.GetEndBarIndex(Chart.Bars);

            var rectangleHorizontalDelta = endBarIndex - startBarIndex;

            var fiftyPercentHorizontal = rectangleHorizontalDelta * 0.5;

            var topLineEndbarIndex = startBarIndex + fiftyPercentHorizontal;

            var topLineEndtime = Chart.Bars.GetOpenTime(topLineEndbarIndex);

            if (topLine == null)
            {
                var objectName = GetObjectName("TopLine");

                topLine = Chart.DrawTrendLine(objectName, startTime, bottomPrice, topLineEndtime, topPrice, Color);

                topLine.IsInteractive = true;
                topLine.IsLocked = true;
            }
            else
            {
                topLine.Time1 = startTime;
                topLine.Time2 = topLineEndtime;

                topLine.Y1 = bottomPrice;
                topLine.Y2 = topPrice;
            }

            var rectangleVerticalDelta = Math.Abs(rectangle.Y2 - rectangle.Y1);

            var fiftyPercentVertical = rectangleVerticalDelta * 0.5;

            var bottomLineLevel = bottomPrice + fiftyPercentVertical;

            if (bottomLine == null)
            {
                var objectName = GetObjectName("BottomLine");

                bottomLine = Chart.DrawTrendLine(objectName, startTime, bottomPrice, endTime, bottomLineLevel, Color);

                bottomLine.IsInteractive = true;
                bottomLine.IsLocked = true;
            }
            else
            {
                bottomLine.Time1 = startTime;
                bottomLine.Time2 = endTime;

                bottomLine.Y1 = bottomPrice;
                bottomLine.Y2 = bottomLineLevel;
            }
        }

        private void DrawOrUpdateHorizontalLines(ChartRectangle rectangle, ChartTrendLine[] horizontalLines)
        {
            var startTime = rectangle.GetStartTime();
            var endTime = rectangle.GetEndTime();

            var verticalDelta = rectangle.GetPriceDelta();

            var lineLevels = new double[]
            {
                verticalDelta * 0.2,
                verticalDelta * 0.4,
                verticalDelta * 0.6,
                verticalDelta * 0.8,
            };

            for (int i = 0; i < lineLevels.Length; i++)
            {
                var level = rectangle.Y2 > rectangle.Y1 ? rectangle.Y1 + lineLevels[i] : rectangle.Y1 - lineLevels[i];

                var horizontalLine = horizontalLines[i];

                if (horizontalLine == null)
                {
                    var objectName = GetObjectName(string.Format("HorizontalLine{0}", i + 1));

                    horizontalLines[i] = Chart.DrawTrendLine(objectName, startTime, level, endTime, level, Color);

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
            var startBarIndex = rectangle.GetStartBarIndex(Chart.Bars);
            var endBarIndex = rectangle.GetEndBarIndex(Chart.Bars);

            var diff = endBarIndex - startBarIndex;

            var lineLevels = new double[]
            {
                diff * 0.2,
                diff * 0.4,
                diff * 0.6,
                diff * 0.8,
            };

            for (int i = 0; i < lineLevels.Length; i++)
            {
                var barIndex = startBarIndex + lineLevels[i];

                var time = Chart.Bars.GetOpenTime(barIndex);

                var verticalLine = verticalLines[i];

                if (verticalLine == null)
                {
                    var objectName = GetObjectName(string.Format("VerticalLine{0}", i + 1));

                    verticalLines[i] = Chart.DrawTrendLine(objectName, time, rectangle.Y1, time, rectangle.Y2, Color);

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
            if (_rectangle == null || _horizontalTrendLines == null || _verticalTrendLines == null) return;

            DrawLabels(_rectangle, _horizontalTrendLines, _verticalTrendLines, Id);
        }

        private void DrawLabels(ChartRectangle rectangle, ChartTrendLine[] horizontalLines, ChartTrendLine[] verticalLines, long id)
        {
            DrawLabelText(Math.Round(rectangle.GetPriceDelta(), Chart.Symbol.Digits).ToString(), rectangle.GetStartTime(), rectangle.GetTopPrice(), id, objectNameKey: "Price", fontSize: 10);
            DrawLabelText(rectangle.GetBarsNumber(Chart.Bars).ToString(), rectangle.GetEndTime(), rectangle.GetBottomPrice(), id, objectNameKey: "BarsNumber", fontSize: 10);
            DrawLabelText(rectangle.GetPriceToBarsRatio(Chart.Bars).ToString(), rectangle.GetEndTime(), rectangle.GetTopPrice(), id, objectNameKey: "PriceToBarsRatio", fontSize: 10);
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
                        label.Text = rectangle.GetBarsNumber(Chart.Bars).ToString();
                        label.Time = rectangle.GetEndTime();
                        label.Y = rectangle.GetBottomPrice();
                        break;

                    case "PriceToBarsRatio":
                        label.Text = rectangle.GetPriceToBarsRatio(Chart.Bars).ToString();
                        label.Time = rectangle.GetEndTime();
                        label.Y = rectangle.GetTopPrice();
                        break;
                }
            }
        }
    }
}