using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace cAlgo.Patterns
{
    public class FibonacciChannelPattern : PatternBase
    {
        private readonly IEnumerable<FibonacciLevel> _fibonacciLevels;

        private ChartTrendLine _zeroLine, _onePercentLine;

        public FibonacciChannelPattern(PatternConfig config, IEnumerable<FibonacciLevel> fibonacciLevels) : base("Fibonacci Channel Pattern", config)
        {
            _fibonacciLevels = fibonacciLevels;
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            if (updatedChartObject.ObjectType != ChartObjectType.TrendLine) return;

            var mainLine = patternObjects.FirstOrDefault(iLine => iLine.Name.LastIndexOf("MainLine", StringComparison.OrdinalIgnoreCase) >= 0) as ChartTrendLine;
            var distanceLine = patternObjects.FirstOrDefault(iLine => iLine.Name.LastIndexOf("DistanceLine", StringComparison.OrdinalIgnoreCase) >= 0) as ChartTrendLine;

            if (mainLine == null || distanceLine == null) return;

            if (updatedChartObject == mainLine)
            {
                distanceLine.Time1 = mainLine.Time2;
                distanceLine.Y1 = mainLine.Y2;
            }
            else if (updatedChartObject == distanceLine)
            {
                mainLine.Time2 = distanceLine.Time1;
                mainLine.Y2 = distanceLine.Y1;
            }

            var verticalLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.VerticalLine).Cast<ChartVerticalLine>().ToArray();

            //UpdateFibonacciLevels(mainLine, distanceLine, verticalLines);
        }

        protected override void OnDrawingStopped()
        {
            _zeroLine = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 1)
            {
                var zeroLineName = GetObjectName("ZeroLine");

                _zeroLine = Chart.DrawTrendLine(zeroLineName, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, Color, 1, LineStyle.Dots);

                _zeroLine.IsInteractive = true;
            }
            else if (MouseUpNumber == 3)
            {
                FinishDrawing();
            }
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 1)
            {
                _zeroLine.Time2 = obj.TimeValue;
                _zeroLine.Y2 = obj.YValue;
            }
            else if (MouseUpNumber == 2 && _zeroLine != null)
            {
                var onePercentLineName = GetObjectName("OnePercentLine");

                var onePercentBarIndex = Chart.Bars.GetBarIndex(obj.TimeValue, Chart.Symbol);

                var zeroLineBarsDelta = _zeroLine.GetBarsNumber(Chart.Bars, Chart.Symbol);
                var zeroLinePriceDelta = _zeroLine.GetPriceDelta();

                double secondBarIndex, secondPrice;

                if (_zeroLine.Time1 < _zeroLine.Time2)
                {
                    secondBarIndex = onePercentBarIndex + zeroLineBarsDelta;
                    secondPrice = obj.YValue - zeroLinePriceDelta;
                }
                else
                {
                    secondBarIndex = onePercentBarIndex - zeroLineBarsDelta;
                    secondPrice = obj.YValue + zeroLinePriceDelta;
                }

                _onePercentLine = Chart.DrawTrendLine(onePercentLineName, obj.TimeValue, obj.YValue, Chart.Bars.GetOpenTime(secondBarIndex, Chart.Symbol), secondPrice, Color, 1, LineStyle.Dots);

                _onePercentLine.IsInteractive = true;

                DrawFibonacciLevels(_zeroLine, _onePercentLine, Id);
            }
        }

        private void DrawFibonacciLevels(ChartTrendLine zeroLine, ChartTrendLine onePercentLine, long id)
        {
            var zeroFirstBarIndex = Chart.Bars.GetBarIndex(zeroLine.Time1, Chart.Symbol);
            var zeroSecondBarIndex = Chart.Bars.GetBarIndex(zeroLine.Time2, Chart.Symbol);

            var onePercentFirstBarIndex = Chart.Bars.GetBarIndex(onePercentLine.Time1, Chart.Symbol);

            var barsDelta = Math.Abs(zeroFirstBarIndex - onePercentFirstBarIndex);
            var priceDelta = Math.Abs(zeroLine.Y1 - onePercentLine.Y1);

            var zeroLineBarsDelta = zeroLine.GetBarsNumber(Chart.Bars, Chart.Symbol);
            var zeroLinePriceDelta = zeroLine.GetPriceDelta();

            foreach (var level in _fibonacciLevels)
            {
                if (level.Percent == 0)
                {
                    zeroLine.Color = level.LineColor;
                    zeroLine.Thickness = level.Thickness;
                    zeroLine.LineStyle = level.Style;
                }
                else if (level.Percent == 1)
                {
                    onePercentLine.Color = level.LineColor;
                    onePercentLine.Thickness = level.Thickness;
                    onePercentLine.LineStyle = level.Style;
                }
                else
                {
                    if (level.Percent != 0.5) continue;

                    var levelName = GetObjectName(string.Format("Level_{0}", level.Percent.ToString(CultureInfo.InvariantCulture)), id);

                    var barsAmount = barsDelta * level.Percent;
                    var priceAmount = priceDelta * level.Percent;

                    double firstBarIndex, secondBarIndex, firstPrice, secondPrice;

                    if (onePercentLine.Time1 < zeroLine.Time1)
                    {
                        if (zeroLine.Time1 < zeroLine.Time2)
                        {
                            firstBarIndex = zeroFirstBarIndex - barsAmount;
                            secondBarIndex = firstBarIndex + zeroLineBarsDelta;

                            if (_onePercentLine.Y1 > _zeroLine.Y1)
                            {
                                firstPrice = zeroLine.Y1 + priceAmount;
                                secondPrice = firstPrice - zeroLinePriceDelta;
                            }
                            else
                            {
                                firstPrice = zeroLine.Y1 - priceAmount;
                                secondPrice = firstPrice - zeroLinePriceDelta;
                            }
                        }
                        else
                        {
                            firstBarIndex = zeroFirstBarIndex - barsAmount;
                            secondBarIndex = firstBarIndex - zeroLineBarsDelta;

                            if (_onePercentLine.Y1 > _zeroLine.Y1)
                            {
                                firstPrice = zeroLine.Y1 + priceAmount;
                                secondPrice = firstPrice + zeroLinePriceDelta;
                            }
                            else
                            {
                                firstPrice = zeroLine.Y1 - priceAmount;
                                secondPrice = firstPrice + zeroLinePriceDelta;
                            }
                        }
                    }
                    else
                    {
                        if (zeroLine.Time1 < zeroLine.Time2)
                        {
                            firstBarIndex = zeroFirstBarIndex + barsAmount;
                            secondBarIndex = firstBarIndex + zeroLineBarsDelta;

                            if (_onePercentLine.Y1 > _zeroLine.Y1)
                            {
                                firstPrice = zeroLine.Y1 + priceAmount;
                                secondPrice = firstPrice - zeroLinePriceDelta;
                            }
                            else
                            {
                                firstPrice = zeroLine.Y1 - priceAmount;
                                secondPrice = firstPrice - zeroLinePriceDelta;
                            }
                        }
                        else
                        {
                            firstBarIndex = zeroFirstBarIndex + barsAmount;
                            secondBarIndex = firstBarIndex - zeroLineBarsDelta;

                            if (_onePercentLine.Y1 > _zeroLine.Y1)
                            {
                                firstPrice = zeroLine.Y1 + priceAmount;
                                secondPrice = firstPrice + zeroLinePriceDelta;
                            }
                            else
                            {
                                firstPrice = zeroLine.Y1 - priceAmount;
                                secondPrice = firstPrice + zeroLinePriceDelta;
                            }
                        }
                    }

                    var firstTime = Chart.Bars.GetOpenTime(firstBarIndex, Chart.Symbol);
                    var secondTime = Chart.Bars.GetOpenTime(secondBarIndex, Chart.Symbol);

                    var levelLine = Chart.DrawTrendLine(levelName, firstTime, firstPrice, secondTime, secondPrice, level.FillColor, level.Thickness, level.Style);

                    levelLine.IsInteractive = true;
                    levelLine.IsLocked = true;
                }
            }
        }

        protected override ChartObject[] GetFrontObjects()
        {
            return new ChartObject[] { _zeroLine, _onePercentLine };
        }
    }
}