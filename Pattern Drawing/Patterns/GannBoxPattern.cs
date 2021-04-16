using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class GannBoxPattern : PatternBase
    {
        private ChartRectangle _rectangle;

        private ChartTrendLine[] _horizontalTrendLines;
        private ChartTrendLine[] _verticalTrendLines;

        public GannBoxPattern(PatternConfig config) : base("Gann Box", config)
        {
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            var chartObjects = Chart.Objects.ToArray();

            var objectNameId = string.Format("{0}_{1}", ObjectName, id);

            foreach (var chartObject in chartObjects)
            {
                if (chartObject == updatedChartObject
                    || chartObject.ObjectType != ChartObjectType.Triangle
                    || !chartObject.Name.StartsWith(objectNameId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }
        }

        protected override void OnDrawingStopped()
        {
            _rectangle = null;
            _horizontalTrendLines = null;
            _verticalTrendLines = null;
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

        private void DrawHorizontalLines(ChartRectangle rectangle)
        {
            _horizontalTrendLines = new ChartTrendLine[5];

            DateTime startTime, endTime;

            if (rectangle.Time1 < rectangle.Time2)
            {
                startTime = rectangle.Time1;
                endTime = rectangle.Time2;
            }
            else
            {
                startTime = rectangle.Time2;
                endTime = rectangle.Time1;
            }

            var diff = Math.Abs(rectangle.Y2 - rectangle.Y1);

            var lineLevels = new double[]
            {
                diff * 0.25,
                diff * 0.382,
                diff * 0.5,
                diff * 0.618,
                diff * 0.75
            };

            for (int i = 0; i < lineLevels.Length; i++)
            {
                var level = rectangle.Y2 > rectangle.Y1 ? rectangle.Y1 + lineLevels[i] : rectangle.Y1 - lineLevels[i];

                var objectName = GetObjectName(string.Format("HorizontalLine{0}", i));

                _horizontalTrendLines[i] = Chart.DrawTrendLine(objectName, startTime, level, endTime, level, Color);

                _horizontalTrendLines[i].IsInteractive = true;
            }
        }

        private void DrawVerticalLines(ChartRectangle rectangle)
        {
            _verticalTrendLines = new ChartTrendLine[5];

            var rectangleFirstBarIndex = Chart.Bars.GetBarIndex(rectangle.Time1);
            var rectangleSecondBarIndex = Chart.Bars.GetBarIndex(rectangle.Time2);

            double startBarIndex, endBarIndex;

            if (rectangleFirstBarIndex < rectangleSecondBarIndex)
            {
                startBarIndex = rectangleFirstBarIndex;
                endBarIndex = rectangleSecondBarIndex;
            }
            else
            {
                startBarIndex = rectangleSecondBarIndex;
                endBarIndex = rectangleFirstBarIndex;
            }

            var diff = endBarIndex - startBarIndex;

            var lineLevels = new double[]
            {
                diff * 0.25,
                diff * 0.382,
                diff * 0.5,
                diff * 0.618,
                diff * 0.75
            };

            for (int i = 0; i < lineLevels.Length; i++)
            {
                var barIndex = startBarIndex + lineLevels[i];

                var time = Chart.Bars.GetOpenTime(barIndex);

                var objectName = GetObjectName(string.Format("VerticalLine{0}", i));

                _verticalTrendLines[i] = Chart.DrawTrendLine(objectName, time, rectangle.Y1, time, rectangle.Y2, Color);

                _verticalTrendLines[i].IsInteractive = true;
            }
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber > 1 || _rectangle == null) return;

            _rectangle.Time2 = obj.TimeValue;
            _rectangle.Y2 = obj.YValue;

            DrawHorizontalLines(_rectangle);
            DrawVerticalLines(_rectangle);
        }

        protected override void DrawLabels()
        {
            if (_rectangle == null || _horizontalTrendLines == null || _verticalTrendLines == null) return;

            DrawLabels(_rectangle, _horizontalTrendLines, _verticalTrendLines, Id);
        }

        private void DrawLabels(ChartRectangle rectangle, ChartTrendLine[] horizontalTrendLines, ChartTrendLine[] verticalTrendLines, long id)
        {
            var timeDistance = TimeSpan.FromHours(Chart.Bars.GetTimeDiff().TotalHours * 2);
            var priceDistance = Chart.Bars.ClosePrices.GetAverageDistance(10) / 2;

            DrawLabelText("0", rectangle.Time1, rectangle.Y1, id, objectNameKey: "0.0");
            DrawLabelText("1", rectangle.Time1, rectangle.Y2, id, objectNameKey: "1.1");

            DrawLabelText("0", rectangle.Time1.Add(timeDistance), rectangle.Y1 + priceDistance, id, objectNameKey: "0.2");
            DrawLabelText("1", rectangle.Time2.Add(timeDistance), rectangle.Y1 + priceDistance, id, objectNameKey: "1.3");

            DrawLabelText("0", rectangle.Time2, rectangle.Y1, id, objectNameKey: "0.4");
            DrawLabelText("1", rectangle.Time2, rectangle.Y2, id, objectNameKey: "1.5");

            DrawLabelText("0", rectangle.Time1.Add(timeDistance), rectangle.Y2 + priceDistance, id, objectNameKey: "0.6");
            DrawLabelText("1", rectangle.Time2.Add(timeDistance), rectangle.Y2 + priceDistance, id, objectNameKey: "1.7");
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var rectangle = patternObjects.FirstOrDefault(iObject => iObject is ChartRectangle) as ChartRectangle;

            var horizontalLines = patternObjects.Where(iObject => iObject is ChartTrendLine && iObject.Name.EndsWith("Horizontal", StringComparison.OrdinalIgnoreCase)).Cast<ChartTrendLine>().ToArray();
            var verticalLines = patternObjects.Where(iObject => iObject is ChartTrendLine && iObject.Name.EndsWith("Vertical", StringComparison.OrdinalIgnoreCase)).Cast<ChartTrendLine>().ToArray();

            if (rectangle == null || horizontalLines == null || verticalLines == null) return;

            if (labels.Length == 0)
            {
                DrawLabels(rectangle, horizontalLines, verticalLines, id);

                return;
            }

            foreach (var label in labels)
            {
                //var labelTriangle = triangles.FirstOrDefault(iTriangle => iTriangle.Name.EndsWith(label.Text,
                //    StringComparison.OrdinalIgnoreCase));

                //if (labelTriangle == null) continue;

                //label.Time = labelTriangle.Time2;
                //label.Y = labelTriangle.Y2;
            }
        }
    }
}