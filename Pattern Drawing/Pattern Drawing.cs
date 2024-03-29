﻿using cAlgo.API;
using cAlgo.Controls;
using cAlgo.Helpers;
using cAlgo.Patterns;
using System.Collections.Generic;
using System.Linq;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class PatternDrawing : Indicator
    {
        private StackPanel _mainButtonsPanel;

        private StackPanel _groupButtonsPanel;

        private StackPanel _mainPanel;

        private Color _buttonsBackgroundDisableColor;

        private Color _buttonsBackgroundEnableColor;

        private Style _buttonsStyle;

        private readonly List<Button> _buttons = new List<Button>();

        private Button _expandButton;

        #region Patterns color parameters

        [Parameter("Color", DefaultValue = "Red", Group = "Patterns Color")]
        public string PatternsColor { get; set; }

        [Parameter("Alpha", DefaultValue = 100, MinValue = 0, MaxValue = 255, Group = "Patterns Color")]
        public int PatternsColorAlpha { get; set; }

        #endregion Patterns color parameters

        #region Patterns Label parameters

        [Parameter("Show", DefaultValue = true, Group = "Patterns Label")]
        public bool PatternsLabelShow { get; set; }

        [Parameter("Color", DefaultValue = "Yellow", Group = "Patterns Label")]
        public string PatternsLabelColor { get; set; }

        [Parameter("Alpha", DefaultValue = 100, MinValue = 0, MaxValue = 255, Group = "Patterns Label")]
        public int PatternsLabelColorAlpha { get; set; }

        [Parameter("Locked", DefaultValue = true, Group = "Patterns Label")]
        public bool PatternsLabelLocked { get; set; }

        [Parameter("Link Style", DefaultValue = true, Group = "Patterns Label")]
        public bool PatternsLabelLinkStyle { get; set; }

        #endregion Patterns Label parameters

        #region Container Panel parameters

        [Parameter("Orientation", DefaultValue = Orientation.Vertical, Group = "Container Panel")]
        public Orientation PanelOrientation { get; set; }

        [Parameter("Horizontal Alignment", DefaultValue = HorizontalAlignment.Left, Group = "Container Panel")]
        public HorizontalAlignment PanelHorizontalAlignment { get; set; }

        [Parameter("Vertical Alignment", DefaultValue = VerticalAlignment.Top, Group = "Container Panel")]
        public VerticalAlignment PanelVerticalAlignment { get; set; }

        [Parameter("Margin", DefaultValue = 3, Group = "Container Panel")]
        public double PanelMargin { get; set; }

        #endregion Container Panel parameters

        #region Buttons parameters

        [Parameter("Disable Color", DefaultValue = "#FFCCCCCC", Group = "Buttons")]
        public string ButtonsBackgroundDisableColor { get; set; }

        [Parameter("Enable Color", DefaultValue = "Red", Group = "Buttons")]
        public string ButtonsBackgroundEnableColor { get; set; }

        [Parameter("Text Color", DefaultValue = "Blue", Group = "Buttons")]
        public string ButtonsForegroundColor { get; set; }

        [Parameter("Margin", DefaultValue = 1, Group = "Buttons")]
        public double ButtonsMargin { get; set; }

        [Parameter("Transparency", DefaultValue = 0.5, MinValue = 0, MaxValue = 1, Group = "Buttons")]
        public double ButtonsTransparency { get; set; }

        #endregion Buttons parameters

        #region Cycles parameters

        [Parameter("Number", DefaultValue = 100, MinValue = 1, Group = "Cycles")]
        public int CyclesNumber { get; set; }

        #endregion Cycles parameters

        #region TimeFrame Visibility parameters

        [Parameter("Enable", DefaultValue = false, Group = "TimeFrame Visibility")]
        public bool IsTimeFrameVisibilityEnabled { get; set; }

        [Parameter("TimeFrame", Group = "TimeFrame Visibility")]
        public TimeFrame VisibilityTimeFrame { get; set; }

        [Parameter("Only Buttons", Group = "TimeFrame Visibility")]
        public bool VisibilityOnlyButtons { get; set; }

        #endregion TimeFrame Visibility parameters

        #region Gann Box parameters

        [Parameter("Rectangle Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Box")]
        public int GannBoxRectangleThickness { get; set; }

        [Parameter("Rectangle Style", DefaultValue = LineStyle.Solid, Group = "Gann Box")]
        public LineStyle GannBoxRectangleStyle { get; set; }

        [Parameter("Rectangle Color", DefaultValue = "Blue", Group = "Gann Box")]
        public string GannBoxRectangleColor { get; set; }

        [Parameter("Price Levels Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Box")]
        public int GannBoxPriceLevelsThickness { get; set; }

        [Parameter("Price Levels Style", DefaultValue = LineStyle.Solid, Group = "Gann Box")]
        public LineStyle GannBoxPriceLevelsStyle { get; set; }

        [Parameter("Price Levels Color", DefaultValue = "Magenta", Group = "Gann Box")]
        public string GannBoxPriceLevelsColor { get; set; }

        [Parameter("Time Levels Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Box")]
        public int GannBoxTimeLevelsThickness { get; set; }

        [Parameter("Time Levels Style", DefaultValue = LineStyle.Solid, Group = "Gann Box")]
        public LineStyle GannBoxTimeLevelsStyle { get; set; }

        [Parameter("Time Levels Color", DefaultValue = "Yellow", Group = "Gann Box")]
        public string GannBoxTimeLevelsColor { get; set; }

        #endregion Gann Box parameters

        #region Gann Square parameters

        [Parameter("Rectangle Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Square")]
        public int GannSquareRectangleThickness { get; set; }

        [Parameter("Rectangle Style", DefaultValue = LineStyle.Solid, Group = "Gann Square")]
        public LineStyle GannSquareRectangleStyle { get; set; }

        [Parameter("Rectangle Color", DefaultValue = "Blue", Group = "Gann Square")]
        public string GannSquareRectangleColor { get; set; }

        [Parameter("Price Levels Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Square")]
        public int GannSquarePriceLevelsThickness { get; set; }

        [Parameter("Price Levels Style", DefaultValue = LineStyle.Solid, Group = "Gann Square")]
        public LineStyle GannSquarePriceLevelsStyle { get; set; }

        [Parameter("Price Levels Color", DefaultValue = "Magenta", Group = "Gann Square")]
        public string GannSquarePriceLevelsColor { get; set; }

        [Parameter("Time Levels Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Square")]
        public int GannSquareTimeLevelsThickness { get; set; }

        [Parameter("Time Levels Style", DefaultValue = LineStyle.Solid, Group = "Gann Square")]
        public LineStyle GannSquareTimeLevelsStyle { get; set; }

        [Parameter("Time Levels Color", DefaultValue = "Yellow", Group = "Gann Square")]
        public string GannSquareTimeLevelsColor { get; set; }

        [Parameter("Fans Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Square")]
        public int GannSquareFansThickness { get; set; }

        [Parameter("Fans Style", DefaultValue = LineStyle.Solid, Group = "Gann Square")]
        public LineStyle GannSquareFansStyle { get; set; }

        [Parameter("Fans Color", DefaultValue = "Brown", Group = "Gann Square")]
        public string GannSquareFansColor { get; set; }

        #endregion Gann Square parameters

        #region Gann Fan parameters

        [Parameter("1/1 Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Fan")]
        public int GannFanOneThickness { get; set; }

        [Parameter("1/1 Style", DefaultValue = LineStyle.Solid, Group = "Gann Fan")]
        public LineStyle GannFanOneStyle { get; set; }

        [Parameter("1/1 Color", DefaultValue = "Red", Group = "Gann Fan")]
        public string GannFanOneColor { get; set; }

        [Parameter("1/2 and 2/1 Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Fan")]
        public int GannFanTwoThickness { get; set; }

        [Parameter("1/2 and 2/1 Style", DefaultValue = LineStyle.Solid, Group = "Gann Fan")]
        public LineStyle GannFanTwoStyle { get; set; }

        [Parameter("1/2 and 2/1 Color", DefaultValue = "Brown", Group = "Gann Fan")]
        public string GannFanTwoColor { get; set; }

        [Parameter("1/3 and 3/1 Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Fan")]
        public int GannFanThreeThickness { get; set; }

        [Parameter("1/3 and 3/1 Style", DefaultValue = LineStyle.Solid, Group = "Gann Fan")]
        public LineStyle GannFanThreeStyle { get; set; }

        [Parameter("1/3 and 3/1 Color", DefaultValue = "Lime", Group = "Gann Fan")]
        public string GannFanThreeColor { get; set; }

        [Parameter("1/4 and 4/1 Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Fan")]
        public int GannFanFourThickness { get; set; }

        [Parameter("1/4 and 4/1 Style", DefaultValue = LineStyle.Solid, Group = "Gann Fan")]
        public LineStyle GannFanFourStyle { get; set; }

        [Parameter("1/4 and 4/1 Color", DefaultValue = "Magenta", Group = "Gann Fan")]
        public string GannFanFourColor { get; set; }

        [Parameter("1/8 and 8/1 Thickness", DefaultValue = 1, MinValue = 1, Group = "Gann Fan")]
        public int GannFanEightThickness { get; set; }

        [Parameter("1/8 and 8/1 Style", DefaultValue = LineStyle.Solid, Group = "Gann Fan")]
        public LineStyle GannFanEightStyle { get; set; }

        [Parameter("1/8 and 8/1 Color", DefaultValue = "Blue", Group = "Gann Fan")]
        public string GannFanEightColor { get; set; }

        #endregion Gann Fan parameters

        #region Fibonacci Retracement parameters

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowFirstFibonacciRetracement { get; set; }

        [Parameter("Fill 1st Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillFirstFibonacciRetracement { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0, Group = "Fibonacci Retracement")]
        public double FirstFibonacciRetracementPercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Fibonacci Retracement")]
        public string FirstFibonacciRetracementColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int FirstFibonacciRetracementAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int FirstFibonacciRetracementThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle FirstFibonacciRetracementStyle { get; set; }

        [Parameter("1st Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool FirstFibonacciRetracementExtendToInfinity { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowSecondFibonacciRetracement { get; set; }

        [Parameter("Fill 2nd Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillSecondFibonacciRetracement { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.236, Group = "Fibonacci Retracement")]
        public double SecondFibonacciRetracementPercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Fibonacci Retracement")]
        public string SecondFibonacciRetracementColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int SecondFibonacciRetracementAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int SecondFibonacciRetracementThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle SecondFibonacciRetracementStyle { get; set; }

        [Parameter("2nd Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool SecondFibonacciRetracementExtendToInfinity { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowThirdFibonacciRetracement { get; set; }

        [Parameter("Fill 3rd Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillThirdFibonacciRetracement { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.382, Group = "Fibonacci Retracement")]
        public double ThirdFibonacciRetracementPercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Fibonacci Retracement")]
        public string ThirdFibonacciRetracementColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int ThirdFibonacciRetracementAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int ThirdFibonacciRetracementThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle ThirdFibonacciRetracementStyle { get; set; }

        [Parameter("3rd Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool ThirdFibonacciRetracementExtendToInfinity { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowFourthFibonacciRetracement { get; set; }

        [Parameter("Fill 4th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillFourthFibonacciRetracement { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.5, Group = "Fibonacci Retracement")]
        public double FourthFibonacciRetracementPercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Fibonacci Retracement")]
        public string FourthFibonacciRetracementColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int FourthFibonacciRetracementAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int FourthFibonacciRetracementThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle FourthFibonacciRetracementStyle { get; set; }

        [Parameter("4th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool FourthFibonacciRetracementExtendToInfinity { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowFifthFibonacciRetracement { get; set; }

        [Parameter("Fill 5th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillFifthFibonacciRetracement { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 0.618, Group = "Fibonacci Retracement")]
        public double FifthFibonacciRetracementPercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Fibonacci Retracement")]
        public string FifthFibonacciRetracementColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int FifthFibonacciRetracementAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int FifthFibonacciRetracementThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle FifthFibonacciRetracementStyle { get; set; }

        [Parameter("5th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool FifthFibonacciRetracementExtendToInfinity { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowSixthFibonacciRetracement { get; set; }

        [Parameter("Fill 6th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillSixthFibonacciRetracement { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 0.786, Group = "Fibonacci Retracement")]
        public double SixthFibonacciRetracementPercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Fibonacci Retracement")]
        public string SixthFibonacciRetracementColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int SixthFibonacciRetracementAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int SixthFibonacciRetracementThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle SixthFibonacciRetracementStyle { get; set; }

        [Parameter("6th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool SixthFibonacciRetracementExtendToInfinity { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowSeventhFibonacciRetracement { get; set; }

        [Parameter("Fill 7th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillSeventhFibonacciRetracement { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1, Group = "Fibonacci Retracement")]
        public double SeventhFibonacciRetracementPercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Fibonacci Retracement")]
        public string SeventhFibonacciRetracementColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int SeventhFibonacciRetracementAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int SeventhFibonacciRetracementThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle SeventhFibonacciRetracementStyle { get; set; }

        [Parameter("7th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool SeventhFibonacciRetracementExtendToInfinity { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowEighthFibonacciRetracement { get; set; }

        [Parameter("Fill 8th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillEighthFibonacciRetracement { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 1.618, Group = "Fibonacci Retracement")]
        public double EighthFibonacciRetracementPercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Fibonacci Retracement")]
        public string EighthFibonacciRetracementColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int EighthFibonacciRetracementAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int EighthFibonacciRetracementThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle EighthFibonacciRetracementStyle { get; set; }

        [Parameter("8th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool EighthFibonacciRetracementExtendToInfinity { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowNinthFibonacciRetracement { get; set; }

        [Parameter("Fill 9th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillNinthFibonacciRetracement { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2.618, Group = "Fibonacci Retracement")]
        public double NinthFibonacciRetracementPercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Fibonacci Retracement")]
        public string NinthFibonacciRetracementColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int NinthFibonacciRetracementAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int NinthFibonacciRetracementThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle NinthFibonacciRetracementStyle { get; set; }

        [Parameter("9th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool NinthFibonacciRetracementExtendToInfinity { get; set; }

        [Parameter("Show 10th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowTenthFibonacciRetracement { get; set; }

        [Parameter("Fill 10th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillTenthFibonacciRetracement { get; set; }

        [Parameter("10th Level Percent", DefaultValue = 3.618, Group = "Fibonacci Retracement")]
        public double TenthFibonacciRetracementPercent { get; set; }

        [Parameter("10th Level Color", DefaultValue = "Aquamarine", Group = "Fibonacci Retracement")]
        public string TenthFibonacciRetracementColor { get; set; }

        [Parameter("10th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int TenthFibonacciRetracementAlpha { get; set; }

        [Parameter("10th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int TenthFibonacciRetracementThickness { get; set; }

        [Parameter("10th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle TenthFibonacciRetracementStyle { get; set; }

        [Parameter("10th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool TenthFibonacciRetracementExtendToInfinity { get; set; }

        [Parameter("Show 11th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowEleventhFibonacciRetracement { get; set; }

        [Parameter("Fill 11th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillEleventhFibonacciRetracement { get; set; }

        [Parameter("11th Level Percent", DefaultValue = 4.236, Group = "Fibonacci Retracement")]
        public double EleventhFibonacciRetracementPercent { get; set; }

        [Parameter("11th Level Color", DefaultValue = "Chocolate", Group = "Fibonacci Retracement")]
        public string EleventhFibonacciRetracementColor { get; set; }

        [Parameter("11th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int EleventhFibonacciRetracementAlpha { get; set; }

        [Parameter("11th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int EleventhFibonacciRetracementThickness { get; set; }

        [Parameter("11th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle EleventhFibonacciRetracementStyle { get; set; }

        [Parameter("11th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Retracement")]
        public bool EleventhFibonacciRetracementExtendToInfinity { get; set; }

        #endregion Fibonacci Retracement parameters

        #region Fibonacci Expansion parameters

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowFirstFibonacciExpansion { get; set; }

        [Parameter("Fill 1st Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillFirstFibonacciExpansion { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0, Group = "Fibonacci Expansion")]
        public double FirstFibonacciExpansionPercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Fibonacci Expansion")]
        public string FirstFibonacciExpansionColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int FirstFibonacciExpansionAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int FirstFibonacciExpansionThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle FirstFibonacciExpansionStyle { get; set; }

        [Parameter("1st Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool FirstFibonacciExpansionExtendToInfinity { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowSecondFibonacciExpansion { get; set; }

        [Parameter("Fill 2nd Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillSecondFibonacciExpansion { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.236, Group = "Fibonacci Expansion")]
        public double SecondFibonacciExpansionPercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Fibonacci Expansion")]
        public string SecondFibonacciExpansionColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int SecondFibonacciExpansionAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int SecondFibonacciExpansionThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle SecondFibonacciExpansionStyle { get; set; }

        [Parameter("2nd Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool SecondFibonacciExpansionExtendToInfinity { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowThirdFibonacciExpansion { get; set; }

        [Parameter("Fill 3rd Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillThirdFibonacciExpansion { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.382, Group = "Fibonacci Expansion")]
        public double ThirdFibonacciExpansionPercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Fibonacci Expansion")]
        public string ThirdFibonacciExpansionColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int ThirdFibonacciExpansionAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int ThirdFibonacciExpansionThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle ThirdFibonacciExpansionStyle { get; set; }

        [Parameter("3rd Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool ThirdFibonacciExpansionExtendToInfinity { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowFourthFibonacciExpansion { get; set; }

        [Parameter("Fill 4th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillFourthFibonacciExpansion { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.5, Group = "Fibonacci Expansion")]
        public double FourthFibonacciExpansionPercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Fibonacci Expansion")]
        public string FourthFibonacciExpansionColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int FourthFibonacciExpansionAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int FourthFibonacciExpansionThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle FourthFibonacciExpansionStyle { get; set; }

        [Parameter("4th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool FourthFibonacciExpansionExtendToInfinity { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowFifthFibonacciExpansion { get; set; }

        [Parameter("Fill 5th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillFifthFibonacciExpansion { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 0.618, Group = "Fibonacci Expansion")]
        public double FifthFibonacciExpansionPercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Fibonacci Expansion")]
        public string FifthFibonacciExpansionColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int FifthFibonacciExpansionAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int FifthFibonacciExpansionThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle FifthFibonacciExpansionStyle { get; set; }

        [Parameter("5th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool FifthFibonacciExpansionExtendToInfinity { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowSixthFibonacciExpansion { get; set; }

        [Parameter("Fill 6th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillSixthFibonacciExpansion { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 0.786, Group = "Fibonacci Expansion")]
        public double SixthFibonacciExpansionPercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Fibonacci Expansion")]
        public string SixthFibonacciExpansionColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int SixthFibonacciExpansionAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int SixthFibonacciExpansionThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle SixthFibonacciExpansionStyle { get; set; }

        [Parameter("6th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool SixthFibonacciExpansionExtendToInfinity { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowSeventhFibonacciExpansion { get; set; }

        [Parameter("Fill 7th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillSeventhFibonacciExpansion { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1, Group = "Fibonacci Expansion")]
        public double SeventhFibonacciExpansionPercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Fibonacci Expansion")]
        public string SeventhFibonacciExpansionColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int SeventhFibonacciExpansionAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int SeventhFibonacciExpansionThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle SeventhFibonacciExpansionStyle { get; set; }

        [Parameter("7th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool SeventhFibonacciExpansionExtendToInfinity { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowEighthFibonacciExpansion { get; set; }

        [Parameter("Fill 8th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillEighthFibonacciExpansion { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 1.618, Group = "Fibonacci Expansion")]
        public double EighthFibonacciExpansionPercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Fibonacci Expansion")]
        public string EighthFibonacciExpansionColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int EighthFibonacciExpansionAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int EighthFibonacciExpansionThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle EighthFibonacciExpansionStyle { get; set; }

        [Parameter("8th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool EighthFibonacciExpansionExtendToInfinity { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowNinthFibonacciExpansion { get; set; }

        [Parameter("Fill 9th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillNinthFibonacciExpansion { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2.618, Group = "Fibonacci Expansion")]
        public double NinthFibonacciExpansionPercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Fibonacci Expansion")]
        public string NinthFibonacciExpansionColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int NinthFibonacciExpansionAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int NinthFibonacciExpansionThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle NinthFibonacciExpansionStyle { get; set; }

        [Parameter("9th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool NinthFibonacciExpansionExtendToInfinity { get; set; }

        [Parameter("Show 10th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowTenthFibonacciExpansion { get; set; }

        [Parameter("Fill 10th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillTenthFibonacciExpansion { get; set; }

        [Parameter("10th Level Percent", DefaultValue = 3.618, Group = "Fibonacci Expansion")]
        public double TenthFibonacciExpansionPercent { get; set; }

        [Parameter("10th Level Color", DefaultValue = "Aquamarine", Group = "Fibonacci Expansion")]
        public string TenthFibonacciExpansionColor { get; set; }

        [Parameter("10th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int TenthFibonacciExpansionAlpha { get; set; }

        [Parameter("10th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int TenthFibonacciExpansionThickness { get; set; }

        [Parameter("10th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle TenthFibonacciExpansionStyle { get; set; }

        [Parameter("10th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool TenthFibonacciExpansionExtendToInfinity { get; set; }

        [Parameter("Show 11th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool ShowEleventhFibonacciExpansion { get; set; }

        [Parameter("Fill 11th Level", DefaultValue = true, Group = "Fibonacci Expansion")]
        public bool FillEleventhFibonacciExpansion { get; set; }

        [Parameter("11th Level Percent", DefaultValue = 4.236, Group = "Fibonacci Expansion")]
        public double EleventhFibonacciExpansionPercent { get; set; }

        [Parameter("11th Level Color", DefaultValue = "Chocolate", Group = "Fibonacci Expansion")]
        public string EleventhFibonacciExpansionColor { get; set; }

        [Parameter("11th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Expansion")]
        public int EleventhFibonacciExpansionAlpha { get; set; }

        [Parameter("11th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Expansion")]
        public int EleventhFibonacciExpansionThickness { get; set; }

        [Parameter("11th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Expansion")]
        public LineStyle EleventhFibonacciExpansionStyle { get; set; }

        [Parameter("11th Level Extend To Infinity", DefaultValue = false, Group = "Fibonacci Expansion")]
        public bool EleventhFibonacciExpansionExtendToInfinity { get; set; }

        #endregion Fibonacci Expansion parameters

        #region Fibonacci Speed Resistance Fan parameters

        [Parameter("Rectangle Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanRectangleThickness { get; set; }

        [Parameter("Rectangle Style", DefaultValue = LineStyle.Dots, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanRectangleStyle { get; set; }

        [Parameter("Rectangle Color", DefaultValue = "Blue", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanRectangleColor { get; set; }

        [Parameter("Extended Lines Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanExtendedLinesThickness { get; set; }

        [Parameter("Extended Lines Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanExtendedLinesStyle { get; set; }

        [Parameter("Extended Lines Color", DefaultValue = "Blue", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanExtendedLinesColor { get; set; }

        [Parameter("Show Price Levels", DefaultValue = true, Group = "Fibonacci Speed Resistance Fan")]
        public bool FibonacciSpeedResistanceFanShowPriceLevels { get; set; }

        [Parameter("Price Levels Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanPriceLevelsThickness { get; set; }

        [Parameter("Price Levels Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanPriceLevelsStyle { get; set; }

        [Parameter("Price Levels Color", DefaultValue = "Magenta", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanPriceLevelsColor { get; set; }

        [Parameter("Show Time Levels", DefaultValue = true, Group = "Fibonacci Speed Resistance Fan")]
        public bool FibonacciSpeedResistanceFanShowTimeLevels { get; set; }

        [Parameter("Time Levels Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanTimeLevelsThickness { get; set; }

        [Parameter("Time Levels Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanTimeLevelsStyle { get; set; }

        [Parameter("Time Levels Color", DefaultValue = "Yellow", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanTimeLevelsColor { get; set; }

        [Parameter("Main Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanMainFanThickness { get; set; }

        [Parameter("Main Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanMainFanStyle { get; set; }

        [Parameter("Main Fan Color", DefaultValue = "Yellow", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanMainFanColor { get; set; }

        [Parameter("1st Fan Percent", DefaultValue = 0.25, Group = "Fibonacci Speed Resistance Fan")]
        public double FibonacciSpeedResistanceFanFirstFanPercent { get; set; }

        [Parameter("1st Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanFirstFanThickness { get; set; }

        [Parameter("1st Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanFirstFanStyle { get; set; }

        [Parameter("1st Fan Color", DefaultValue = "Red", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanFirstFanColor { get; set; }

        [Parameter("2nd Fan Percent", DefaultValue = 0.382, Group = "Fibonacci Speed Resistance Fan")]
        public double FibonacciSpeedResistanceFanSecondFanPercent { get; set; }

        [Parameter("2nd Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanSecondFanThickness { get; set; }

        [Parameter("2nd Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanSecondFanStyle { get; set; }

        [Parameter("2nd Fan Color", DefaultValue = "Brown", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanSecondFanColor { get; set; }

        [Parameter("3rd Fan Percent", DefaultValue = 0.5, Group = "Fibonacci Speed Resistance Fan")]
        public double FibonacciSpeedResistanceFanThirdFanPercent { get; set; }

        [Parameter("3rd Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanThirdFanThickness { get; set; }

        [Parameter("3rd Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanThirdFanStyle { get; set; }

        [Parameter("3rd Fan Color", DefaultValue = "Lime", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanThirdFanColor { get; set; }

        [Parameter("4th Fan Percent", DefaultValue = 0.618, Group = "Fibonacci Speed Resistance Fan")]
        public double FibonacciSpeedResistanceFanFourthFanPercent { get; set; }

        [Parameter("4th Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanFourthFanThickness { get; set; }

        [Parameter("4th Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanFourthFanStyle { get; set; }

        [Parameter("4th Fan Color", DefaultValue = "Magenta", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanFourthFanColor { get; set; }

        [Parameter("5th Fan Percent", DefaultValue = 0.75, Group = "Fibonacci Speed Resistance Fan")]
        public double FibonacciSpeedResistanceFanFifthFanPercent { get; set; }

        [Parameter("5th Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanFifthFanThickness { get; set; }

        [Parameter("5th Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanFifthFanStyle { get; set; }

        [Parameter("5th Fan Color", DefaultValue = "Blue", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanFifthFanColor { get; set; }

        #endregion Fibonacci Speed Resistance Fan parameters

        #region Fibonacci Time Zone parameters

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowFirstFibonacciTimeZone { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0, Group = "Fibonacci Time Zone")]
        public double FirstFibonacciTimeZonePercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Fibonacci Time Zone")]
        public string FirstFibonacciTimeZoneColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int FirstFibonacciTimeZoneAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int FirstFibonacciTimeZoneThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle FirstFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowSecondFibonacciTimeZone { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 1, Group = "Fibonacci Time Zone")]
        public double SecondFibonacciTimeZonePercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Fibonacci Time Zone")]
        public string SecondFibonacciTimeZoneColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int SecondFibonacciTimeZoneAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int SecondFibonacciTimeZoneThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle SecondFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowThirdFibonacciTimeZone { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 2, Group = "Fibonacci Time Zone")]
        public double ThirdFibonacciTimeZonePercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Fibonacci Time Zone")]
        public string ThirdFibonacciTimeZoneColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int ThirdFibonacciTimeZoneAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int ThirdFibonacciTimeZoneThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle ThirdFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowFourthFibonacciTimeZone { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 3, Group = "Fibonacci Time Zone")]
        public double FourthFibonacciTimeZonePercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Fibonacci Time Zone")]
        public string FourthFibonacciTimeZoneColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int FourthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int FourthFibonacciTimeZoneThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle FourthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowFifthFibonacciTimeZone { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 5, Group = "Fibonacci Time Zone")]
        public double FifthFibonacciTimeZonePercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Fibonacci Time Zone")]
        public string FifthFibonacciTimeZoneColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int FifthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int FifthFibonacciTimeZoneThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle FifthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowSixthFibonacciTimeZone { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 8, Group = "Fibonacci Time Zone")]
        public double SixthFibonacciTimeZonePercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Fibonacci Time Zone")]
        public string SixthFibonacciTimeZoneColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int SixthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int SixthFibonacciTimeZoneThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle SixthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowSeventhFibonacciTimeZone { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 13, Group = "Fibonacci Time Zone")]
        public double SeventhFibonacciTimeZonePercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Fibonacci Time Zone")]
        public string SeventhFibonacciTimeZoneColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int SeventhFibonacciTimeZoneAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int SeventhFibonacciTimeZoneThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle SeventhFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowEighthFibonacciTimeZone { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 21, Group = "Fibonacci Time Zone")]
        public double EighthFibonacciTimeZonePercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Fibonacci Time Zone")]
        public string EighthFibonacciTimeZoneColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int EighthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int EighthFibonacciTimeZoneThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle EighthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowNinthFibonacciTimeZone { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 34, Group = "Fibonacci Time Zone")]
        public double NinthFibonacciTimeZonePercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Fibonacci Time Zone")]
        public string NinthFibonacciTimeZoneColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int NinthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int NinthFibonacciTimeZoneThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle NinthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 10th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowTenthFibonacciTimeZone { get; set; }

        [Parameter("10th Level Percent", DefaultValue = 55, Group = "Fibonacci Time Zone")]
        public double TenthFibonacciTimeZonePercent { get; set; }

        [Parameter("10th Level Color", DefaultValue = "Aquamarine", Group = "Fibonacci Time Zone")]
        public string TenthFibonacciTimeZoneColor { get; set; }

        [Parameter("10th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int TenthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("10th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int TenthFibonacciTimeZoneThickness { get; set; }

        [Parameter("10th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle TenthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 11th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowEleventhFibonacciTimeZone { get; set; }

        [Parameter("11th Level Percent", DefaultValue = 89, Group = "Fibonacci Time Zone")]
        public double EleventhFibonacciTimeZonePercent { get; set; }

        [Parameter("11th Level Color", DefaultValue = "Chocolate", Group = "Fibonacci Time Zone")]
        public string EleventhFibonacciTimeZoneColor { get; set; }

        [Parameter("11th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int EleventhFibonacciTimeZoneAlpha { get; set; }

        [Parameter("11th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int EleventhFibonacciTimeZoneThickness { get; set; }

        [Parameter("11th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle EleventhFibonacciTimeZoneStyle { get; set; }

        #endregion Fibonacci Time Zone parameters

        #region Trend Based Fibonacci Time Parameters

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowFirstTrendBasedFibonacciTime { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0, Group = "Trend Based Fibonacci Time")]
        public double FirstTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Trend Based Fibonacci Time")]
        public string FirstTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int FirstTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int FirstTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle FirstTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowSecondTrendBasedFibonacciTime { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.382, Group = "Trend Based Fibonacci Time")]
        public double SecondTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Trend Based Fibonacci Time")]
        public string SecondTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int SecondTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int SecondTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle SecondTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowThirdTrendBasedFibonacciTime { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.5, Group = "Trend Based Fibonacci Time")]
        public double ThirdTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Trend Based Fibonacci Time")]
        public string ThirdTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int ThirdTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int ThirdTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle ThirdTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowFourthTrendBasedFibonacciTime { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.618, Group = "Trend Based Fibonacci Time")]
        public double FourthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Trend Based Fibonacci Time")]
        public string FourthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int FourthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int FourthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle FourthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowFifthTrendBasedFibonacciTime { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 1, Group = "Trend Based Fibonacci Time")]
        public double FifthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Trend Based Fibonacci Time")]
        public string FifthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int FifthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int FifthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle FifthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowSixthTrendBasedFibonacciTime { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 1.382, Group = "Trend Based Fibonacci Time")]
        public double SixthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Trend Based Fibonacci Time")]
        public string SixthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int SixthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int SixthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle SixthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowSeventhTrendBasedFibonacciTime { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1.618, Group = "Trend Based Fibonacci Time")]
        public double SeventhTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Trend Based Fibonacci Time")]
        public string SeventhTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int SeventhTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int SeventhTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle SeventhTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowEighthTrendBasedFibonacciTime { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 2, Group = "Trend Based Fibonacci Time")]
        public double EighthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Trend Based Fibonacci Time")]
        public string EighthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int EighthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int EighthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle EighthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowNinthTrendBasedFibonacciTime { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2.382, Group = "Trend Based Fibonacci Time")]
        public double NinthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Trend Based Fibonacci Time")]
        public string NinthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int NinthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int NinthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle NinthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 10th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowTenthTrendBasedFibonacciTime { get; set; }

        [Parameter("10th Level Percent", DefaultValue = 2.618, Group = "Trend Based Fibonacci Time")]
        public double TenthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("10th Level Color", DefaultValue = "Aquamarine", Group = "Trend Based Fibonacci Time")]
        public string TenthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("10th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int TenthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("10th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int TenthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("10th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle TenthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 11th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowEleventhTrendBasedFibonacciTime { get; set; }

        [Parameter("11th Level Percent", DefaultValue = 3, Group = "Trend Based Fibonacci Time")]
        public double EleventhTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("11th Level Color", DefaultValue = "Chocolate", Group = "Trend Based Fibonacci Time")]
        public string EleventhTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("11th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int EleventhTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("11th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int EleventhTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("11th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle EleventhTrendBasedFibonacciTimeStyle { get; set; }

        #endregion Trend Based Fibonacci Time Parameters

        #region Fibonacci Channel parameters

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowFirstFibonacciChannel { get; set; }

        [Parameter("Fill 1st Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillFirstFibonacciChannel { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0, Group = "Fibonacci Channel")]
        public double FirstFibonacciChannelPercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Fibonacci Channel")]
        public string FirstFibonacciChannelColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int FirstFibonacciChannelAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int FirstFibonacciChannelThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle FirstFibonacciChannelStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowSecondFibonacciChannel { get; set; }

        [Parameter("Fill 2nd Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillSecondFibonacciChannel { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.236, Group = "Fibonacci Channel")]
        public double SecondFibonacciChannelPercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Fibonacci Channel")]
        public string SecondFibonacciChannelColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int SecondFibonacciChannelAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int SecondFibonacciChannelThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle SecondFibonacciChannelStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowThirdFibonacciChannel { get; set; }

        [Parameter("Fill 3rd Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillThirdFibonacciChannel { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.382, Group = "Fibonacci Channel")]
        public double ThirdFibonacciChannelPercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Fibonacci Channel")]
        public string ThirdFibonacciChannelColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int ThirdFibonacciChannelAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int ThirdFibonacciChannelThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle ThirdFibonacciChannelStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowFourthFibonacciChannel { get; set; }

        [Parameter("Fill 4th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillFourthFibonacciChannel { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.5, Group = "Fibonacci Channel")]
        public double FourthFibonacciChannelPercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Fibonacci Channel")]
        public string FourthFibonacciChannelColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int FourthFibonacciChannelAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int FourthFibonacciChannelThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle FourthFibonacciChannelStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowFifthFibonacciChannel { get; set; }

        [Parameter("Fill 5th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillFifthFibonacciChannel { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 0.618, Group = "Fibonacci Channel")]
        public double FifthFibonacciChannelPercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Fibonacci Channel")]
        public string FifthFibonacciChannelColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int FifthFibonacciChannelAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int FifthFibonacciChannelThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle FifthFibonacciChannelStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowSixthFibonacciChannel { get; set; }

        [Parameter("Fill 6th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillSixthFibonacciChannel { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 0.786, Group = "Fibonacci Channel")]
        public double SixthFibonacciChannelPercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Fibonacci Channel")]
        public string SixthFibonacciChannelColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int SixthFibonacciChannelAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int SixthFibonacciChannelThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle SixthFibonacciChannelStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowSeventhFibonacciChannel { get; set; }

        [Parameter("Fill 7th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillSeventhFibonacciChannel { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1, Group = "Fibonacci Channel")]
        public double SeventhFibonacciChannelPercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Fibonacci Channel")]
        public string SeventhFibonacciChannelColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int SeventhFibonacciChannelAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int SeventhFibonacciChannelThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle SeventhFibonacciChannelStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowEighthFibonacciChannel { get; set; }

        [Parameter("Fill 8th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillEighthFibonacciChannel { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 1.618, Group = "Fibonacci Channel")]
        public double EighthFibonacciChannelPercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Fibonacci Channel")]
        public string EighthFibonacciChannelColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int EighthFibonacciChannelAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int EighthFibonacciChannelThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle EighthFibonacciChannelStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowNinthFibonacciChannel { get; set; }

        [Parameter("Fill 9th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillNinthFibonacciChannel { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2.618, Group = "Fibonacci Channel")]
        public double NinthFibonacciChannelPercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Fibonacci Channel")]
        public string NinthFibonacciChannelColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int NinthFibonacciChannelAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int NinthFibonacciChannelThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle NinthFibonacciChannelStyle { get; set; }

        [Parameter("Show 10th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowTenthFibonacciChannel { get; set; }

        [Parameter("Fill 10th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillTenthFibonacciChannel { get; set; }

        [Parameter("10th Level Percent", DefaultValue = 3.618, Group = "Fibonacci Channel")]
        public double TenthFibonacciChannelPercent { get; set; }

        [Parameter("10th Level Color", DefaultValue = "Aquamarine", Group = "Fibonacci Channel")]
        public string TenthFibonacciChannelColor { get; set; }

        [Parameter("10th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int TenthFibonacciChannelAlpha { get; set; }

        [Parameter("10th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int TenthFibonacciChannelThickness { get; set; }

        [Parameter("10th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle TenthFibonacciChannelStyle { get; set; }

        [Parameter("Show 11th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowEleventhFibonacciChannel { get; set; }

        [Parameter("Fill 11th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillEleventhFibonacciChannel { get; set; }

        [Parameter("11th Level Percent", DefaultValue = 4.236, Group = "Fibonacci Channel")]
        public double EleventhFibonacciChannelPercent { get; set; }

        [Parameter("11th Level Color", DefaultValue = "Chocolate", Group = "Fibonacci Channel")]
        public string EleventhFibonacciChannelColor { get; set; }

        [Parameter("11th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int EleventhFibonacciChannelAlpha { get; set; }

        [Parameter("11th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int EleventhFibonacciChannelThickness { get; set; }

        [Parameter("11th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle EleventhFibonacciChannelStyle { get; set; }

        #endregion Fibonacci Channel parameters

        #region Original Pitchfork parameters

        [Parameter("Median Thickness", DefaultValue = 1, MinValue = 1, Group = "Original Pitchfork")]
        public int OriginalPitchforkMedianThickness { get; set; }

        [Parameter("Median Style", DefaultValue = LineStyle.Solid, Group = "Original Pitchfork")]
        public LineStyle OriginalPitchforkMedianStyle { get; set; }

        [Parameter("Median Color", DefaultValue = "Blue", Group = "Original Pitchfork")]
        public string OriginalPitchforkMedianColor { get; set; }

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Original Pitchfork")]
        public bool ShowFirstOriginalPitchfork { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0.25, Group = "Original Pitchfork")]
        public double FirstOriginalPitchforkPercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Original Pitchfork")]
        public string FirstOriginalPitchforkColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Original Pitchfork")]
        public int FirstOriginalPitchforkAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Original Pitchfork")]
        public int FirstOriginalPitchforkThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Original Pitchfork")]
        public LineStyle FirstOriginalPitchforkStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Original Pitchfork")]
        public bool ShowSecondOriginalPitchfork { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.382, Group = "Original Pitchfork")]
        public double SecondOriginalPitchforkPercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Original Pitchfork")]
        public string SecondOriginalPitchforkColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Original Pitchfork")]
        public int SecondOriginalPitchforkAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Original Pitchfork")]
        public int SecondOriginalPitchforkThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Original Pitchfork")]
        public LineStyle SecondOriginalPitchforkStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Original Pitchfork")]
        public bool ShowThirdOriginalPitchfork { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.5, Group = "Original Pitchfork")]
        public double ThirdOriginalPitchforkPercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Original Pitchfork")]
        public string ThirdOriginalPitchforkColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Original Pitchfork")]
        public int ThirdOriginalPitchforkAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Original Pitchfork")]
        public int ThirdOriginalPitchforkThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Original Pitchfork")]
        public LineStyle ThirdOriginalPitchforkStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Original Pitchfork")]
        public bool ShowFourthOriginalPitchfork { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.618, Group = "Original Pitchfork")]
        public double FourthOriginalPitchforkPercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Original Pitchfork")]
        public string FourthOriginalPitchforkColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Original Pitchfork")]
        public int FourthOriginalPitchforkAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Original Pitchfork")]
        public int FourthOriginalPitchforkThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Original Pitchfork")]
        public LineStyle FourthOriginalPitchforkStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Original Pitchfork")]
        public bool ShowFifthOriginalPitchfork { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 0.75, Group = "Original Pitchfork")]
        public double FifthOriginalPitchforkPercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Original Pitchfork")]
        public string FifthOriginalPitchforkColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Original Pitchfork")]
        public int FifthOriginalPitchforkAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Original Pitchfork")]
        public int FifthOriginalPitchforkThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Original Pitchfork")]
        public LineStyle FifthOriginalPitchforkStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Original Pitchfork")]
        public bool ShowSixthOriginalPitchfork { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 1, Group = "Original Pitchfork")]
        public double SixthOriginalPitchforkPercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Original Pitchfork")]
        public string SixthOriginalPitchforkColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Original Pitchfork")]
        public int SixthOriginalPitchforkAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Original Pitchfork")]
        public int SixthOriginalPitchforkThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Original Pitchfork")]
        public LineStyle SixthOriginalPitchforkStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Original Pitchfork")]
        public bool ShowSeventhOriginalPitchfork { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1.5, Group = "Original Pitchfork")]
        public double SeventhOriginalPitchforkPercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Original Pitchfork")]
        public string SeventhOriginalPitchforkColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Original Pitchfork")]
        public int SeventhOriginalPitchforkAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Original Pitchfork")]
        public int SeventhOriginalPitchforkThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Original Pitchfork")]
        public LineStyle SeventhOriginalPitchforkStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Original Pitchfork")]
        public bool ShowEighthOriginalPitchfork { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 1.75, Group = "Original Pitchfork")]
        public double EighthOriginalPitchforkPercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Original Pitchfork")]
        public string EighthOriginalPitchforkColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Original Pitchfork")]
        public int EighthOriginalPitchforkAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Original Pitchfork")]
        public int EighthOriginalPitchforkThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Original Pitchfork")]
        public LineStyle EighthOriginalPitchforkStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Original Pitchfork")]
        public bool ShowNinthOriginalPitchfork { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2, Group = "Original Pitchfork")]
        public double NinthOriginalPitchforkPercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Original Pitchfork")]
        public string NinthOriginalPitchforkColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Original Pitchfork")]
        public int NinthOriginalPitchforkAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Original Pitchfork")]
        public int NinthOriginalPitchforkThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Original Pitchfork")]
        public LineStyle NinthOriginalPitchforkStyle { get; set; }

        #endregion Original Pitchfork parameters

        #region Schiff Pitchfork parameters

        [Parameter("Median Thickness", DefaultValue = 1, MinValue = 1, Group = "Schiff Pitchfork")]
        public int SchiffPitchforkMedianThickness { get; set; }

        [Parameter("Median Style", DefaultValue = LineStyle.Solid, Group = "Schiff Pitchfork")]
        public LineStyle SchiffPitchforkMedianStyle { get; set; }

        [Parameter("Median Color", DefaultValue = "Blue", Group = "Schiff Pitchfork")]
        public string SchiffPitchforkMedianColor { get; set; }

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Schiff Pitchfork")]
        public bool ShowFirstSchiffPitchfork { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0.25, Group = "Schiff Pitchfork")]
        public double FirstSchiffPitchforkPercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Schiff Pitchfork")]
        public string FirstSchiffPitchforkColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Schiff Pitchfork")]
        public int FirstSchiffPitchforkAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Schiff Pitchfork")]
        public int FirstSchiffPitchforkThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Schiff Pitchfork")]
        public LineStyle FirstSchiffPitchforkStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Schiff Pitchfork")]
        public bool ShowSecondSchiffPitchfork { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.382, Group = "Schiff Pitchfork")]
        public double SecondSchiffPitchforkPercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Schiff Pitchfork")]
        public string SecondSchiffPitchforkColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Schiff Pitchfork")]
        public int SecondSchiffPitchforkAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Schiff Pitchfork")]
        public int SecondSchiffPitchforkThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Schiff Pitchfork")]
        public LineStyle SecondSchiffPitchforkStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Schiff Pitchfork")]
        public bool ShowThirdSchiffPitchfork { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.5, Group = "Schiff Pitchfork")]
        public double ThirdSchiffPitchforkPercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Schiff Pitchfork")]
        public string ThirdSchiffPitchforkColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Schiff Pitchfork")]
        public int ThirdSchiffPitchforkAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Schiff Pitchfork")]
        public int ThirdSchiffPitchforkThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Schiff Pitchfork")]
        public LineStyle ThirdSchiffPitchforkStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Schiff Pitchfork")]
        public bool ShowFourthSchiffPitchfork { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.618, Group = "Schiff Pitchfork")]
        public double FourthSchiffPitchforkPercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Schiff Pitchfork")]
        public string FourthSchiffPitchforkColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Schiff Pitchfork")]
        public int FourthSchiffPitchforkAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Schiff Pitchfork")]
        public int FourthSchiffPitchforkThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Schiff Pitchfork")]
        public LineStyle FourthSchiffPitchforkStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Schiff Pitchfork")]
        public bool ShowFifthSchiffPitchfork { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 0.75, Group = "Schiff Pitchfork")]
        public double FifthSchiffPitchforkPercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Schiff Pitchfork")]
        public string FifthSchiffPitchforkColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Schiff Pitchfork")]
        public int FifthSchiffPitchforkAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Schiff Pitchfork")]
        public int FifthSchiffPitchforkThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Schiff Pitchfork")]
        public LineStyle FifthSchiffPitchforkStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Schiff Pitchfork")]
        public bool ShowSixthSchiffPitchfork { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 1, Group = "Schiff Pitchfork")]
        public double SixthSchiffPitchforkPercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Schiff Pitchfork")]
        public string SixthSchiffPitchforkColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Schiff Pitchfork")]
        public int SixthSchiffPitchforkAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Schiff Pitchfork")]
        public int SixthSchiffPitchforkThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Schiff Pitchfork")]
        public LineStyle SixthSchiffPitchforkStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Schiff Pitchfork")]
        public bool ShowSeventhSchiffPitchfork { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1.5, Group = "Schiff Pitchfork")]
        public double SeventhSchiffPitchforkPercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Schiff Pitchfork")]
        public string SeventhSchiffPitchforkColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Schiff Pitchfork")]
        public int SeventhSchiffPitchforkAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Schiff Pitchfork")]
        public int SeventhSchiffPitchforkThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Schiff Pitchfork")]
        public LineStyle SeventhSchiffPitchforkStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Schiff Pitchfork")]
        public bool ShowEighthSchiffPitchfork { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 1.75, Group = "Schiff Pitchfork")]
        public double EighthSchiffPitchforkPercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Schiff Pitchfork")]
        public string EighthSchiffPitchforkColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Schiff Pitchfork")]
        public int EighthSchiffPitchforkAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Schiff Pitchfork")]
        public int EighthSchiffPitchforkThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Schiff Pitchfork")]
        public LineStyle EighthSchiffPitchforkStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Schiff Pitchfork")]
        public bool ShowNinthSchiffPitchfork { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2, Group = "Schiff Pitchfork")]
        public double NinthSchiffPitchforkPercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Schiff Pitchfork")]
        public string NinthSchiffPitchforkColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Schiff Pitchfork")]
        public int NinthSchiffPitchforkAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Schiff Pitchfork")]
        public int NinthSchiffPitchforkThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Schiff Pitchfork")]
        public LineStyle NinthSchiffPitchforkStyle { get; set; }

        #endregion Schiff Pitchfork parameters

        #region Modified Schiff Pitchfork parameters

        [Parameter("Median Thickness", DefaultValue = 1, MinValue = 1, Group = "Modified Schiff Pitchfork")]
        public int ModifiedSchiffPitchforkMedianThickness { get; set; }

        [Parameter("Median Style", DefaultValue = LineStyle.Solid, Group = "Modified Schiff Pitchfork")]
        public LineStyle ModifiedSchiffPitchforkMedianStyle { get; set; }

        [Parameter("Median Color", DefaultValue = "Blue", Group = "Modified Schiff Pitchfork")]
        public string ModifiedSchiffPitchforkMedianColor { get; set; }

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Modified Schiff Pitchfork")]
        public bool ShowFirstModifiedSchiffPitchfork { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0.25, Group = "Modified Schiff Pitchfork")]
        public double FirstModifiedSchiffPitchforkPercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Modified Schiff Pitchfork")]
        public string FirstModifiedSchiffPitchforkColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Modified Schiff Pitchfork")]
        public int FirstModifiedSchiffPitchforkAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Modified Schiff Pitchfork")]
        public int FirstModifiedSchiffPitchforkThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Modified Schiff Pitchfork")]
        public LineStyle FirstModifiedSchiffPitchforkStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Modified Schiff Pitchfork")]
        public bool ShowSecondModifiedSchiffPitchfork { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.382, Group = "Modified Schiff Pitchfork")]
        public double SecondModifiedSchiffPitchforkPercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Modified Schiff Pitchfork")]
        public string SecondModifiedSchiffPitchforkColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Modified Schiff Pitchfork")]
        public int SecondModifiedSchiffPitchforkAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Modified Schiff Pitchfork")]
        public int SecondModifiedSchiffPitchforkThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Modified Schiff Pitchfork")]
        public LineStyle SecondModifiedSchiffPitchforkStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Modified Schiff Pitchfork")]
        public bool ShowThirdModifiedSchiffPitchfork { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.5, Group = "Modified Schiff Pitchfork")]
        public double ThirdModifiedSchiffPitchforkPercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Modified Schiff Pitchfork")]
        public string ThirdModifiedSchiffPitchforkColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Modified Schiff Pitchfork")]
        public int ThirdModifiedSchiffPitchforkAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Modified Schiff Pitchfork")]
        public int ThirdModifiedSchiffPitchforkThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Modified Schiff Pitchfork")]
        public LineStyle ThirdModifiedSchiffPitchforkStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Modified Schiff Pitchfork")]
        public bool ShowFourthModifiedSchiffPitchfork { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.618, Group = "Modified Schiff Pitchfork")]
        public double FourthModifiedSchiffPitchforkPercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Modified Schiff Pitchfork")]
        public string FourthModifiedSchiffPitchforkColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Modified Schiff Pitchfork")]
        public int FourthModifiedSchiffPitchforkAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Modified Schiff Pitchfork")]
        public int FourthModifiedSchiffPitchforkThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Modified Schiff Pitchfork")]
        public LineStyle FourthModifiedSchiffPitchforkStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Modified Schiff Pitchfork")]
        public bool ShowFifthModifiedSchiffPitchfork { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 0.75, Group = "Modified Schiff Pitchfork")]
        public double FifthModifiedSchiffPitchforkPercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Modified Schiff Pitchfork")]
        public string FifthModifiedSchiffPitchforkColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Modified Schiff Pitchfork")]
        public int FifthModifiedSchiffPitchforkAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Modified Schiff Pitchfork")]
        public int FifthModifiedSchiffPitchforkThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Modified Schiff Pitchfork")]
        public LineStyle FifthModifiedSchiffPitchforkStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Modified Schiff Pitchfork")]
        public bool ShowSixthModifiedSchiffPitchfork { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 1, Group = "Modified Schiff Pitchfork")]
        public double SixthModifiedSchiffPitchforkPercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Modified Schiff Pitchfork")]
        public string SixthModifiedSchiffPitchforkColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Modified Schiff Pitchfork")]
        public int SixthModifiedSchiffPitchforkAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Modified Schiff Pitchfork")]
        public int SixthModifiedSchiffPitchforkThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Modified Schiff Pitchfork")]
        public LineStyle SixthModifiedSchiffPitchforkStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Modified Schiff Pitchfork")]
        public bool ShowSeventhModifiedSchiffPitchfork { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1.5, Group = "Modified Schiff Pitchfork")]
        public double SeventhModifiedSchiffPitchforkPercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Modified Schiff Pitchfork")]
        public string SeventhModifiedSchiffPitchforkColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Modified Schiff Pitchfork")]
        public int SeventhModifiedSchiffPitchforkAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Modified Schiff Pitchfork")]
        public int SeventhModifiedSchiffPitchforkThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Modified Schiff Pitchfork")]
        public LineStyle SeventhModifiedSchiffPitchforkStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Modified Schiff Pitchfork")]
        public bool ShowEighthModifiedSchiffPitchfork { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 1.75, Group = "Modified Schiff Pitchfork")]
        public double EighthModifiedSchiffPitchforkPercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Modified Schiff Pitchfork")]
        public string EighthModifiedSchiffPitchforkColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Modified Schiff Pitchfork")]
        public int EighthModifiedSchiffPitchforkAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Modified Schiff Pitchfork")]
        public int EighthModifiedSchiffPitchforkThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Modified Schiff Pitchfork")]
        public LineStyle EighthModifiedSchiffPitchforkStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Modified Schiff Pitchfork")]
        public bool ShowNinthModifiedSchiffPitchfork { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2, Group = "Modified Schiff Pitchfork")]
        public double NinthModifiedSchiffPitchforkPercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Modified Schiff Pitchfork")]
        public string NinthModifiedSchiffPitchforkColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Modified Schiff Pitchfork")]
        public int NinthModifiedSchiffPitchforkAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Modified Schiff Pitchfork")]
        public int NinthModifiedSchiffPitchforkThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Modified Schiff Pitchfork")]
        public LineStyle NinthModifiedSchiffPitchforkStyle { get; set; }

        #endregion Modified Schiff Pitchfork parameters

        #region Pitchfan parameters

        [Parameter("Median Thickness", DefaultValue = 1, MinValue = 1, Group = "Pitchfan")]
        public int PitchfanMedianThickness { get; set; }

        [Parameter("Median Style", DefaultValue = LineStyle.Solid, Group = "Pitchfan")]
        public LineStyle PitchfanMedianStyle { get; set; }

        [Parameter("Median Color", DefaultValue = "Blue", Group = "Pitchfan")]
        public string PitchfanMedianColor { get; set; }

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Pitchfan")]
        public bool ShowFirstPitchfan { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0.25, Group = "Pitchfan")]
        public double FirstPitchfanPercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Pitchfan")]
        public string FirstPitchfanColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Pitchfan")]
        public int FirstPitchfanAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Pitchfan")]
        public int FirstPitchfanThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Pitchfan")]
        public LineStyle FirstPitchfanStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Pitchfan")]
        public bool ShowSecondPitchfan { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.382, Group = "Pitchfan")]
        public double SecondPitchfanPercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Pitchfan")]
        public string SecondPitchfanColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Pitchfan")]
        public int SecondPitchfanAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Pitchfan")]
        public int SecondPitchfanThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Pitchfan")]
        public LineStyle SecondPitchfanStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Pitchfan")]
        public bool ShowThirdPitchfan { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.5, Group = "Pitchfan")]
        public double ThirdPitchfanPercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Pitchfan")]
        public string ThirdPitchfanColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Pitchfan")]
        public int ThirdPitchfanAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Pitchfan")]
        public int ThirdPitchfanThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Pitchfan")]
        public LineStyle ThirdPitchfanStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Pitchfan")]
        public bool ShowFourthPitchfan { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.618, Group = "Pitchfan")]
        public double FourthPitchfanPercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Pitchfan")]
        public string FourthPitchfanColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Pitchfan")]
        public int FourthPitchfanAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Pitchfan")]
        public int FourthPitchfanThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Pitchfan")]
        public LineStyle FourthPitchfanStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Pitchfan")]
        public bool ShowFifthPitchfan { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 0.75, Group = "Pitchfan")]
        public double FifthPitchfanPercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Pitchfan")]
        public string FifthPitchfanColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Pitchfan")]
        public int FifthPitchfanAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Pitchfan")]
        public int FifthPitchfanThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Pitchfan")]
        public LineStyle FifthPitchfanStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Pitchfan")]
        public bool ShowSixthPitchfan { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 1, Group = "Pitchfan")]
        public double SixthPitchfanPercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Pitchfan")]
        public string SixthPitchfanColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Pitchfan")]
        public int SixthPitchfanAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Pitchfan")]
        public int SixthPitchfanThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Pitchfan")]
        public LineStyle SixthPitchfanStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Pitchfan")]
        public bool ShowSeventhPitchfan { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1.5, Group = "Pitchfan")]
        public double SeventhPitchfanPercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Pitchfan")]
        public string SeventhPitchfanColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Pitchfan")]
        public int SeventhPitchfanAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Pitchfan")]
        public int SeventhPitchfanThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Pitchfan")]
        public LineStyle SeventhPitchfanStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Pitchfan")]
        public bool ShowEighthPitchfan { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 1.75, Group = "Pitchfan")]
        public double EighthPitchfanPercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Pitchfan")]
        public string EighthPitchfanColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Pitchfan")]
        public int EighthPitchfanAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Pitchfan")]
        public int EighthPitchfanThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Pitchfan")]
        public LineStyle EighthPitchfanStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Pitchfan")]
        public bool ShowNinthPitchfan { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2, Group = "Pitchfan")]
        public double NinthPitchfanPercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Pitchfan")]
        public string NinthPitchfanColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Pitchfan")]
        public int NinthPitchfanAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Pitchfan")]
        public int NinthPitchfanThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Pitchfan")]
        public LineStyle NinthPitchfanStyle { get; set; }

        #endregion Pitchfan parameters

        #region Measure parameters

        [Parameter("Up Color", DefaultValue = "Blue", Group = "Measure")]
        public string MeasureUpColor { get; set; }

        [Parameter("Down Color", DefaultValue = "Red", Group = "Measure")]
        public string MeasureDownColor { get; set; }

        [Parameter("Color Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Measure")]
        public int MeasureColorAlpha { get; set; }

        [Parameter("Thickness", DefaultValue = 1, Group = "Measure")]
        public int MeasureThickness { get; set; }

        [Parameter("Style", DefaultValue = LineStyle.Solid, Group = "Measure")]
        public LineStyle MeasureStyle { get; set; }

        [Parameter("Filled", DefaultValue = true, Group = "Measure")]
        public bool MeasureIsFilled { get; set; }

        [Parameter("Text Color", DefaultValue = "Yellow", Group = "Measure")]
        public string MeasureTextColor { get; set; }

        [Parameter("Font Size", DefaultValue = 10, Group = "Measure")]
        public int MeasureFontSize { get; set; }

        [Parameter("Text Bold", DefaultValue = true, Group = "Measure")]
        public bool MeasureIsTextBold { get; set; }

        #endregion Measure parameters

        #region Overridden methods

        protected override void Initialize()
        {
            _mainPanel = new StackPanel
            {
                HorizontalAlignment = PanelHorizontalAlignment,
                VerticalAlignment = PanelVerticalAlignment,
                Orientation = PanelOrientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal,
                BackgroundColor = Color.Transparent,
            };

            _mainButtonsPanel = new StackPanel
            {
                Orientation = PanelOrientation,
                Margin = PanelMargin
            };

            _mainPanel.AddChild(_mainButtonsPanel);

            _groupButtonsPanel = new StackPanel
            {
                Orientation = PanelOrientation,
                Margin = PanelMargin,
                IsVisible = false
            };

            _mainPanel.AddChild(_groupButtonsPanel);

            _buttonsBackgroundDisableColor = ColorParser.Parse(ButtonsBackgroundDisableColor);
            _buttonsBackgroundEnableColor = ColorParser.Parse(ButtonsBackgroundEnableColor);

            _buttonsStyle = new Style();

            _buttonsStyle.Set(ControlProperty.Margin, ButtonsMargin);
            _buttonsStyle.Set(ControlProperty.BackgroundColor, _buttonsBackgroundDisableColor);
            _buttonsStyle.Set(ControlProperty.ForegroundColor, ColorParser.Parse(ButtonsForegroundColor));
            _buttonsStyle.Set(ControlProperty.HorizontalContentAlignment, HorizontalAlignment.Center);
            _buttonsStyle.Set(ControlProperty.VerticalContentAlignment, VerticalAlignment.Center);
            _buttonsStyle.Set(ControlProperty.Opacity, ButtonsTransparency);

            var patternsColor = ColorParser.Parse(PatternsColor, PatternsColorAlpha);
            var patternsLabelsColor = ColorParser.Parse(PatternsLabelColor, PatternsLabelColorAlpha);

            var patternConfig = new PatternConfig(Chart, patternsColor, PatternsLabelShow, patternsLabelsColor, PatternsLabelLocked, PatternsLabelLinkStyle, new Logger(this.GetType().Name, Print));

            _expandButton = new Button
            {
                Style = _buttonsStyle,
                Text = "Expand Patterns"
            };

            _expandButton.Click += ExpandButton_Click;

            _mainButtonsPanel.AddChild(_expandButton);

            AddPatternButton(new TrianglePattern(patternConfig));
            AddPatternButton(new CyclesPattern(patternConfig, CyclesNumber));
            AddPatternButton(new HeadAndShouldersPattern(patternConfig));
            AddPatternButton(new CypherPattern(patternConfig));
            AddPatternButton(new AbcdPattern(patternConfig));
            AddPatternButton(new ThreeDrivesPattern(patternConfig));

            AddGannPatterns(patternConfig);

            AddFibonacciPatterns(patternConfig);

            AddPitchforkPatterns(patternConfig);

            AddElliottCorrectionWavePattern(patternConfig);
            AddElliottImpulseWavgePattern(patternConfig);
            AddElliottTriangleWavePattern(patternConfig);
            AddElliottTripleComboWavePattern(patternConfig);
            AddElliottDoubleComboWavePattern(patternConfig);

            AddPatternButton(new MeasurePattern(patternConfig, new MeasureSettings
            {
                Thickness = MeasureThickness,
                Style = MeasureStyle,
                UpColor = ColorParser.Parse(MeasureUpColor, MeasureColorAlpha),
                DownColor = ColorParser.Parse(MeasureDownColor, MeasureColorAlpha),
                TextColor = ColorParser.Parse(MeasureTextColor),
                IsFilled = MeasureIsFilled,
                FontSize = MeasureFontSize,
                IsTextBold = MeasureIsTextBold
            }));

            var showHideButton = new Controls.ToggleButton()
            {
                Style = _buttonsStyle,
                OnColor = _buttonsBackgroundEnableColor,
                OffColor = _buttonsBackgroundDisableColor,
                Text = "Hide",
                IsVisible = false
            };

            showHideButton.TurnedOn += ShowHideButton_TurnedOn;
            showHideButton.TurnedOff += ShowHideButton_TurnedOff;

            _mainButtonsPanel.AddChild(showHideButton);
            _buttons.Add(showHideButton);

            var saveButton = new PatternsSaveButton(Chart)
            {
                Style = _buttonsStyle,
                IsVisible = false
            };

            _mainButtonsPanel.AddChild(saveButton);
            _buttons.Add(saveButton);

            var loadButton = new PatternsLoadButton(Chart)
            {
                Style = _buttonsStyle,
                IsVisible = false
            };

            _mainButtonsPanel.AddChild(loadButton);
            _buttons.Add(loadButton);

            var removeAllButton = new PatternsRemoveAllButton(Chart)
            {
                Style = _buttonsStyle,
                IsVisible = false
            };

            _mainButtonsPanel.AddChild(removeAllButton);
            _buttons.Add(removeAllButton);

            var collapseButton = new Button
            {
                Style = _buttonsStyle,
                Text = "Collapse",
                IsVisible = false
            };

            collapseButton.Click += CollapseButton_Click;

            _mainButtonsPanel.AddChild(collapseButton);
            _buttons.Add(collapseButton);

            Chart.AddControl(_mainPanel);

            CheckTimeFrameVisibility();
        }

        public override void Calculate(int index)
        {
        }

        #endregion Overridden methods

        private void CollapseButton_Click(ButtonClickEventArgs obj)
        {
            _buttons.ForEach(iButton => iButton.IsVisible = false);

            _groupButtonsPanel.IsVisible = false;

            _expandButton.IsVisible = true;
        }

        private void ExpandButton_Click(ButtonClickEventArgs obj)
        {
            _buttons.ForEach(iButton => iButton.IsVisible = true);

            obj.Button.IsVisible = false;
        }

        private void ShowHideButton_TurnedOff(Controls.ToggleButton obj)
        {
            Chart.ChangePatternsVisibility(false);

            obj.Text = "Hide";
        }

        private void ShowHideButton_TurnedOn(Controls.ToggleButton obj)
        {
            Chart.ChangePatternsVisibility(true);

            obj.Text = "Show";
        }

        private void AddPatternButton(IPattern pattern)
        {
            var button = new PatternButton(pattern)
            {
                Style = _buttonsStyle,
                OnColor = _buttonsBackgroundEnableColor,
                OffColor = _buttonsBackgroundDisableColor,
                IsVisible = false
            };

            _buttons.Add(button);

            _mainButtonsPanel.AddChild(button);

            pattern.Initialize();
        }

        private PatternGroupButton AddPatternGroupButton(string text)
        {
            var groupButton = new PatternGroupButton(_groupButtonsPanel)
            {
                Text = text,
                Style = _buttonsStyle,
                OnColor = _buttonsBackgroundEnableColor,
                OffColor = _buttonsBackgroundDisableColor,
                IsVisible = false
            };

            _buttons.Add(groupButton);

            _mainButtonsPanel.AddChild(groupButton);

            return groupButton;
        }

        private void CheckTimeFrameVisibility()
        {
            if (IsTimeFrameVisibilityEnabled)
            {
                if (TimeFrame != VisibilityTimeFrame)
                {
                    _mainButtonsPanel.IsVisible = false;

                    if (!VisibilityOnlyButtons) Chart.ChangePatternsVisibility(true);
                }
                else if (!VisibilityOnlyButtons)
                {
                    Chart.ChangePatternsVisibility(false);
                }
            }
        }

        private void InitializePatterns(IEnumerable<IPattern> patterns)
        {
            foreach (var pattern in patterns)
            {
                pattern.Initialize();
            }
        }

        private void AddGannPatterns(PatternConfig patternConfig)
        {
            var gannPatternsGroupButton = AddPatternGroupButton("Gann");

            gannPatternsGroupButton.Patterns = new IPattern[]
            {
                new GannBoxPattern(patternConfig, new GannBoxSettings
                {
                    RectangleThickness = GannBoxRectangleThickness,
                    RectangleStyle = GannBoxRectangleStyle,
                    RectangleColor = ColorParser.Parse(GannBoxRectangleColor),
                    PriceLevelsThickness = GannBoxPriceLevelsThickness,
                    PriceLevelsStyle = GannBoxPriceLevelsStyle,
                    PriceLevelsColor = ColorParser.Parse(GannBoxPriceLevelsColor),
                    TimeLevelsThickness = GannBoxTimeLevelsThickness,
                    TimeLevelsStyle = GannBoxTimeLevelsStyle,
                    TimeLevelsColor = ColorParser.Parse(GannBoxTimeLevelsColor),
                }),
                new GannSquarePattern(patternConfig, new GannSquareSettings
                {
                    RectangleThickness = GannSquareRectangleThickness,
                    RectangleStyle = GannSquareRectangleStyle,
                    RectangleColor = ColorParser.Parse(GannSquareRectangleColor),
                    PriceLevelsThickness = GannSquarePriceLevelsThickness,
                    PriceLevelsStyle = GannSquarePriceLevelsStyle,
                    PriceLevelsColor = ColorParser.Parse(GannSquarePriceLevelsColor),
                    TimeLevelsThickness = GannSquareTimeLevelsThickness,
                    TimeLevelsStyle = GannSquareTimeLevelsStyle,
                    TimeLevelsColor = ColorParser.Parse(GannSquareTimeLevelsColor),
                    FansThickness = GannSquareFansThickness,
                    FansStyle = GannSquareFansStyle,
                    FansColor = ColorParser.Parse(GannSquareFansColor),
                }),
                new GannFanPattern(patternConfig, new SideFanSettings[]
                {
                    new SideFanSettings
                    {
                        Name = "1x2",
                        Percent = 0.416,
                        Color = ColorParser.Parse(GannFanTwoColor),
                        Style = GannFanTwoStyle,
                        Thickness = GannFanTwoThickness
                    },
                    new SideFanSettings
                    {
                        Name = "1x3",
                        Percent = 0.583,
                        Color = ColorParser.Parse(GannFanThreeColor),
                        Style = GannFanThreeStyle,
                        Thickness = GannFanThreeThickness
                    },
                    new SideFanSettings
                    {
                        Name = "1x4",
                        Percent = 0.666,
                        Color = ColorParser.Parse(GannFanFourColor),
                        Style = GannFanFourStyle,
                        Thickness = GannFanFourThickness
                    },
                    new SideFanSettings
                    {
                        Name = "1x8",
                        Percent = 0.833,
                        Color = ColorParser.Parse(GannFanEightColor),
                        Style = GannFanEightStyle,
                        Thickness = GannFanEightThickness
                    },
                    new SideFanSettings
                    {
                        Name = "2x1",
                        Percent = -0.416,
                        Color = ColorParser.Parse(GannFanTwoColor),
                        Style = GannFanTwoStyle,
                        Thickness = GannFanTwoThickness
                    },
                    new SideFanSettings
                    {
                        Name = "3x1",
                        Percent = -0.583,
                        Color = ColorParser.Parse(GannFanThreeColor),
                        Style = GannFanThreeStyle,
                        Thickness = GannFanThreeThickness
                    },
                    new SideFanSettings
                    {
                        Name = "4x1",
                        Percent = -0.666,
                        Color = ColorParser.Parse(GannFanFourColor),
                        Style = GannFanFourStyle,
                        Thickness = GannFanFourThickness
                    },
                    new SideFanSettings
                    {
                        Name = "8x1",
                        Percent = -0.833,
                        Color = ColorParser.Parse(GannFanEightColor),
                        Style = GannFanEightStyle,
                        Thickness = GannFanEightThickness
                    },
                }, new FanSettings
                {
                    Color = ColorParser.Parse(GannFanOneColor),
                    Style = GannFanOneStyle,
                    Thickness = GannFanOneThickness
                })
            };

            InitializePatterns(gannPatternsGroupButton.Patterns);
        }

        private IEnumerable<Patterns.FibonacciLevel> GetFibonacciRetracementLevels()
        {
            var fibonacciRetracementLevels = new List<Patterns.FibonacciLevel>();

            if (ShowFirstFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = FirstFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(FirstFibonacciRetracementColor),
                    Style = FirstFibonacciRetracementStyle,
                    Thickness = FirstFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(FirstFibonacciRetracementColor, FirstFibonacciRetracementAlpha),
                    IsFilled = FillFirstFibonacciRetracement,
                    ExtendToInfinity = FirstFibonacciRetracementExtendToInfinity,
                });
            }

            if (ShowSecondFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = SecondFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(SecondFibonacciRetracementColor),
                    Style = SecondFibonacciRetracementStyle,
                    Thickness = SecondFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(SecondFibonacciRetracementColor, SecondFibonacciRetracementAlpha),
                    IsFilled = FillSecondFibonacciRetracement,
                    ExtendToInfinity = SecondFibonacciRetracementExtendToInfinity,
                });
            }

            if (ShowThirdFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = ThirdFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(ThirdFibonacciRetracementColor),
                    Style = ThirdFibonacciRetracementStyle,
                    Thickness = ThirdFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(ThirdFibonacciRetracementColor, ThirdFibonacciRetracementAlpha),
                    IsFilled = FillThirdFibonacciRetracement,
                    ExtendToInfinity = ThirdFibonacciRetracementExtendToInfinity,
                });
            }

            if (ShowFourthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = FourthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(FourthFibonacciRetracementColor),
                    Style = FourthFibonacciRetracementStyle,
                    Thickness = FourthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(FourthFibonacciRetracementColor, FourthFibonacciRetracementAlpha),
                    IsFilled = FillFourthFibonacciRetracement,
                    ExtendToInfinity = FourthFibonacciRetracementExtendToInfinity,
                });
            }

            if (ShowFifthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = FifthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(FifthFibonacciRetracementColor),
                    Style = FifthFibonacciRetracementStyle,
                    Thickness = FifthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(FifthFibonacciRetracementColor, FifthFibonacciRetracementAlpha),
                    IsFilled = FillFifthFibonacciRetracement,
                    ExtendToInfinity = FifthFibonacciRetracementExtendToInfinity,
                });
            }

            if (ShowSixthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = SixthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(SixthFibonacciRetracementColor),
                    Style = SixthFibonacciRetracementStyle,
                    Thickness = SixthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(SixthFibonacciRetracementColor, SixthFibonacciRetracementAlpha),
                    IsFilled = FillSixthFibonacciRetracement,
                    ExtendToInfinity = SixthFibonacciRetracementExtendToInfinity,
                });
            }

            if (ShowSeventhFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = SeventhFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(SeventhFibonacciRetracementColor),
                    Style = SeventhFibonacciRetracementStyle,
                    Thickness = SeventhFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(SeventhFibonacciRetracementColor, SeventhFibonacciRetracementAlpha),
                    IsFilled = FillSeventhFibonacciRetracement,
                    ExtendToInfinity = SeventhFibonacciRetracementExtendToInfinity,
                });
            }

            if (ShowEighthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = EighthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(EighthFibonacciRetracementColor),
                    Style = EighthFibonacciRetracementStyle,
                    Thickness = EighthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(EighthFibonacciRetracementColor, EighthFibonacciRetracementAlpha),
                    IsFilled = FillEighthFibonacciRetracement,
                    ExtendToInfinity = EighthFibonacciRetracementExtendToInfinity,
                });
            }

            if (ShowNinthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = NinthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(NinthFibonacciRetracementColor),
                    Style = NinthFibonacciRetracementStyle,
                    Thickness = NinthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(NinthFibonacciRetracementColor, NinthFibonacciRetracementAlpha),
                    IsFilled = FillNinthFibonacciRetracement,
                    ExtendToInfinity = NinthFibonacciRetracementExtendToInfinity,
                });
            }

            if (ShowTenthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = TenthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(TenthFibonacciRetracementColor),
                    Style = TenthFibonacciRetracementStyle,
                    Thickness = TenthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(TenthFibonacciRetracementColor, TenthFibonacciRetracementAlpha),
                    IsFilled = FillTenthFibonacciRetracement,
                    ExtendToInfinity = TenthFibonacciRetracementExtendToInfinity,
                });
            }

            if (ShowEleventhFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = EleventhFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(EleventhFibonacciRetracementColor),
                    Style = EleventhFibonacciRetracementStyle,
                    Thickness = EleventhFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(EleventhFibonacciRetracementColor, EleventhFibonacciRetracementAlpha),
                    IsFilled = FillEleventhFibonacciRetracement,
                    ExtendToInfinity = EleventhFibonacciRetracementExtendToInfinity,
                });
            }

            return fibonacciRetracementLevels;
        }

        private IEnumerable<Patterns.FibonacciLevel> GetFibonacciExpansionLevels()
        {
            var fibonacciExpansionLevels = new List<Patterns.FibonacciLevel>();

            if (ShowFirstFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = FirstFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(FirstFibonacciExpansionColor),
                    Style = FirstFibonacciExpansionStyle,
                    Thickness = FirstFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(FirstFibonacciExpansionColor, FirstFibonacciExpansionAlpha),
                    IsFilled = FillFirstFibonacciExpansion,
                    ExtendToInfinity = FirstFibonacciExpansionExtendToInfinity,
                });
            }

            if (ShowSecondFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = SecondFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(SecondFibonacciExpansionColor),
                    Style = SecondFibonacciExpansionStyle,
                    Thickness = SecondFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(SecondFibonacciExpansionColor, SecondFibonacciExpansionAlpha),
                    IsFilled = FillSecondFibonacciExpansion,
                    ExtendToInfinity = SecondFibonacciExpansionExtendToInfinity,
                });
            }

            if (ShowThirdFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = ThirdFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(ThirdFibonacciExpansionColor),
                    Style = ThirdFibonacciExpansionStyle,
                    Thickness = ThirdFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(ThirdFibonacciExpansionColor, ThirdFibonacciExpansionAlpha),
                    IsFilled = FillThirdFibonacciExpansion,
                    ExtendToInfinity = ThirdFibonacciExpansionExtendToInfinity,
                });
            }

            if (ShowFourthFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = FourthFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(FourthFibonacciExpansionColor),
                    Style = FourthFibonacciExpansionStyle,
                    Thickness = FourthFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(FourthFibonacciExpansionColor, FourthFibonacciExpansionAlpha),
                    IsFilled = FillFourthFibonacciExpansion,
                    ExtendToInfinity = FourthFibonacciExpansionExtendToInfinity,
                });
            }

            if (ShowFifthFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = FifthFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(FifthFibonacciExpansionColor),
                    Style = FifthFibonacciExpansionStyle,
                    Thickness = FifthFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(FifthFibonacciExpansionColor, FifthFibonacciExpansionAlpha),
                    IsFilled = FillFifthFibonacciExpansion,
                    ExtendToInfinity = FifthFibonacciExpansionExtendToInfinity,
                });
            }

            if (ShowSixthFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = SixthFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(SixthFibonacciExpansionColor),
                    Style = SixthFibonacciExpansionStyle,
                    Thickness = SixthFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(SixthFibonacciExpansionColor, SixthFibonacciExpansionAlpha),
                    IsFilled = FillSixthFibonacciExpansion,
                    ExtendToInfinity = SixthFibonacciExpansionExtendToInfinity,
                });
            }

            if (ShowSeventhFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = SeventhFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(SeventhFibonacciExpansionColor),
                    Style = SeventhFibonacciExpansionStyle,
                    Thickness = SeventhFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(SeventhFibonacciExpansionColor, SeventhFibonacciExpansionAlpha),
                    IsFilled = FillSeventhFibonacciExpansion,
                    ExtendToInfinity = SeventhFibonacciExpansionExtendToInfinity,
                });
            }

            if (ShowEighthFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = EighthFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(EighthFibonacciExpansionColor),
                    Style = EighthFibonacciExpansionStyle,
                    Thickness = EighthFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(EighthFibonacciExpansionColor, EighthFibonacciExpansionAlpha),
                    IsFilled = FillEighthFibonacciExpansion,
                    ExtendToInfinity = EighthFibonacciExpansionExtendToInfinity,
                });
            }

            if (ShowNinthFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = NinthFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(NinthFibonacciExpansionColor),
                    Style = NinthFibonacciExpansionStyle,
                    Thickness = NinthFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(NinthFibonacciExpansionColor, NinthFibonacciExpansionAlpha),
                    IsFilled = FillNinthFibonacciExpansion,
                    ExtendToInfinity = NinthFibonacciExpansionExtendToInfinity,
                });
            }

            if (ShowTenthFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = TenthFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(TenthFibonacciExpansionColor),
                    Style = TenthFibonacciExpansionStyle,
                    Thickness = TenthFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(TenthFibonacciExpansionColor, TenthFibonacciExpansionAlpha),
                    IsFilled = FillTenthFibonacciExpansion,
                    ExtendToInfinity = TenthFibonacciExpansionExtendToInfinity,
                });
            }

            if (ShowEleventhFibonacciExpansion)
            {
                fibonacciExpansionLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = EleventhFibonacciExpansionPercent,
                    LineColor = ColorParser.Parse(EleventhFibonacciExpansionColor),
                    Style = EleventhFibonacciExpansionStyle,
                    Thickness = EleventhFibonacciExpansionThickness,
                    FillColor = ColorParser.Parse(EleventhFibonacciExpansionColor, EleventhFibonacciExpansionAlpha),
                    IsFilled = FillEleventhFibonacciExpansion,
                    ExtendToInfinity = EleventhFibonacciExpansionExtendToInfinity,
                });
            }

            return fibonacciExpansionLevels;
        }

        private IEnumerable<Patterns.FibonacciLevel> GetFibonacciTimeZoneLevels()
        {
            var result = new List<Patterns.FibonacciLevel>();

            if (ShowFirstFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FirstFibonacciTimeZonePercent,
                    Style = FirstFibonacciTimeZoneStyle,
                    Thickness = FirstFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(FirstFibonacciTimeZoneColor, FirstFibonacciTimeZoneAlpha),
                });
            }

            if (ShowSecondFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SecondFibonacciTimeZonePercent,
                    Style = SecondFibonacciTimeZoneStyle,
                    Thickness = SecondFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(SecondFibonacciTimeZoneColor, SecondFibonacciTimeZoneAlpha),
                });
            }

            if (ShowThirdFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = ThirdFibonacciTimeZonePercent,
                    Style = ThirdFibonacciTimeZoneStyle,
                    Thickness = ThirdFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(ThirdFibonacciTimeZoneColor, ThirdFibonacciTimeZoneAlpha),
                });
            }

            if (ShowFourthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FourthFibonacciTimeZonePercent,
                    Style = FourthFibonacciTimeZoneStyle,
                    Thickness = FourthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(FourthFibonacciTimeZoneColor, FourthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowFifthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FifthFibonacciTimeZonePercent,
                    Style = FifthFibonacciTimeZoneStyle,
                    Thickness = FifthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(FifthFibonacciTimeZoneColor, FifthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowSixthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SixthFibonacciTimeZonePercent,
                    Style = SixthFibonacciTimeZoneStyle,
                    Thickness = SixthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(SixthFibonacciTimeZoneColor, SixthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowSeventhFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SeventhFibonacciTimeZonePercent,
                    Style = SeventhFibonacciTimeZoneStyle,
                    Thickness = SeventhFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(SeventhFibonacciTimeZoneColor, SeventhFibonacciTimeZoneAlpha),
                });
            }

            if (ShowEighthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EighthFibonacciTimeZonePercent,
                    Style = EighthFibonacciTimeZoneStyle,
                    Thickness = EighthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(EighthFibonacciTimeZoneColor, EighthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowNinthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = NinthFibonacciTimeZonePercent,
                    Style = NinthFibonacciTimeZoneStyle,
                    Thickness = NinthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(NinthFibonacciTimeZoneColor, NinthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowTenthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = TenthFibonacciTimeZonePercent,
                    Style = TenthFibonacciTimeZoneStyle,
                    Thickness = TenthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(TenthFibonacciTimeZoneColor, TenthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowEleventhFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EleventhFibonacciTimeZonePercent,
                    Style = EleventhFibonacciTimeZoneStyle,
                    Thickness = EleventhFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(EleventhFibonacciTimeZoneColor, EleventhFibonacciTimeZoneAlpha),
                });
            }

            return result;
        }

        private IEnumerable<Patterns.FibonacciLevel> GetTrendBasedFibonacciTimeLevels()
        {
            var result = new List<Patterns.FibonacciLevel>();

            if (ShowFirstTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FirstTrendBasedFibonacciTimePercent,
                    Style = FirstTrendBasedFibonacciTimeStyle,
                    Thickness = FirstTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(FirstTrendBasedFibonacciTimeColor, FirstTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowSecondTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SecondTrendBasedFibonacciTimePercent,
                    Style = SecondTrendBasedFibonacciTimeStyle,
                    Thickness = SecondTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(SecondTrendBasedFibonacciTimeColor, SecondTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowThirdTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = ThirdTrendBasedFibonacciTimePercent,
                    Style = ThirdTrendBasedFibonacciTimeStyle,
                    Thickness = ThirdTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(ThirdTrendBasedFibonacciTimeColor, ThirdTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowFourthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FourthTrendBasedFibonacciTimePercent,
                    Style = FourthTrendBasedFibonacciTimeStyle,
                    Thickness = FourthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(FourthTrendBasedFibonacciTimeColor, FourthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowFifthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FifthTrendBasedFibonacciTimePercent,
                    Style = FifthTrendBasedFibonacciTimeStyle,
                    Thickness = FifthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(FifthTrendBasedFibonacciTimeColor, FifthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowSixthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SixthTrendBasedFibonacciTimePercent,
                    Style = SixthTrendBasedFibonacciTimeStyle,
                    Thickness = SixthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(SixthTrendBasedFibonacciTimeColor, SixthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowSeventhTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SeventhTrendBasedFibonacciTimePercent,
                    Style = SeventhTrendBasedFibonacciTimeStyle,
                    Thickness = SeventhTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(SeventhTrendBasedFibonacciTimeColor, SeventhTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowEighthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EighthTrendBasedFibonacciTimePercent,
                    Style = EighthTrendBasedFibonacciTimeStyle,
                    Thickness = EighthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(EighthTrendBasedFibonacciTimeColor, EighthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowNinthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = NinthTrendBasedFibonacciTimePercent,
                    Style = NinthTrendBasedFibonacciTimeStyle,
                    Thickness = NinthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(NinthTrendBasedFibonacciTimeColor, NinthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowTenthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = TenthTrendBasedFibonacciTimePercent,
                    Style = TenthTrendBasedFibonacciTimeStyle,
                    Thickness = TenthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(TenthTrendBasedFibonacciTimeColor, TenthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowEleventhTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EleventhTrendBasedFibonacciTimePercent,
                    Style = EleventhTrendBasedFibonacciTimeStyle,
                    Thickness = EleventhTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(EleventhTrendBasedFibonacciTimeColor, EleventhTrendBasedFibonacciTimeAlpha),
                });
            }

            return result;
        }

        private IEnumerable<Patterns.FibonacciLevel> GetFibonacciChannelLevels()
        {
            var result = new List<Patterns.FibonacciLevel>();

            if (ShowFirstFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FirstFibonacciChannelPercent,
                    Style = FirstFibonacciChannelStyle,
                    Thickness = FirstFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(FirstFibonacciChannelColor, FirstFibonacciChannelAlpha),
                    IsFilled = FillFirstFibonacciChannel,
                });
            }

            if (ShowSecondFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SecondFibonacciChannelPercent,
                    Style = SecondFibonacciChannelStyle,
                    Thickness = SecondFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(SecondFibonacciChannelColor, SecondFibonacciChannelAlpha),
                    IsFilled = FillSecondFibonacciChannel,
                });
            }

            if (ShowThirdFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = ThirdFibonacciChannelPercent,
                    Style = ThirdFibonacciChannelStyle,
                    Thickness = ThirdFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(ThirdFibonacciChannelColor, ThirdFibonacciChannelAlpha),
                    IsFilled = FillThirdFibonacciChannel,
                });
            }

            if (ShowFourthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FourthFibonacciChannelPercent,
                    Style = FourthFibonacciChannelStyle,
                    Thickness = FourthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(FourthFibonacciChannelColor, FourthFibonacciChannelAlpha),
                    IsFilled = FillFourthFibonacciChannel,
                });
            }

            if (ShowFifthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FifthFibonacciChannelPercent,
                    Style = FifthFibonacciChannelStyle,
                    Thickness = FifthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(FifthFibonacciChannelColor, FifthFibonacciChannelAlpha),
                    IsFilled = FillFifthFibonacciChannel,
                });
            }

            if (ShowSixthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SixthFibonacciChannelPercent,
                    Style = SixthFibonacciChannelStyle,
                    Thickness = SixthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(SixthFibonacciChannelColor, SixthFibonacciChannelAlpha),
                    IsFilled = FillSixthFibonacciChannel,
                });
            }

            if (ShowSeventhFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SeventhFibonacciChannelPercent,
                    Style = SeventhFibonacciChannelStyle,
                    Thickness = SeventhFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(SeventhFibonacciChannelColor, SeventhFibonacciChannelAlpha),
                    IsFilled = FillSeventhFibonacciChannel,
                });
            }

            if (ShowEighthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EighthFibonacciChannelPercent,
                    Style = EighthFibonacciChannelStyle,
                    Thickness = EighthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(EighthFibonacciChannelColor, EighthFibonacciChannelAlpha),
                    IsFilled = FillEighthFibonacciChannel,
                });
            }

            if (ShowNinthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = NinthFibonacciChannelPercent,
                    Style = NinthFibonacciChannelStyle,
                    Thickness = NinthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(NinthFibonacciChannelColor, NinthFibonacciChannelAlpha),
                    IsFilled = FillNinthFibonacciChannel,
                });
            }

            if (ShowTenthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = TenthFibonacciChannelPercent,
                    Style = TenthFibonacciChannelStyle,
                    Thickness = TenthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(TenthFibonacciChannelColor, TenthFibonacciChannelAlpha),
                    IsFilled = FillTenthFibonacciChannel,
                });
            }

            if (ShowEleventhFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EleventhFibonacciChannelPercent,
                    Style = EleventhFibonacciChannelStyle,
                    Thickness = EleventhFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(EleventhFibonacciChannelColor, EleventhFibonacciChannelAlpha),
                    IsFilled = FillEleventhFibonacciChannel,
                });
            }

            return result;
        }

        private void AddFibonacciPatterns(PatternConfig patternConfig)
        {
            var patternsGroupButton = AddPatternGroupButton("Fibonacci");

            patternsGroupButton.Patterns = new IPattern[]
            {
                new FibonacciRetracementPattern(patternConfig, GetFibonacciRetracementLevels()),
                new FibonacciExpansionPattern(patternConfig, GetFibonacciExpansionLevels()),
                new FibonacciSpeedResistanceFanPattern(patternConfig, new FibonacciSpeedResistanceFanSettings
                {
                    RectangleThickness = FibonacciSpeedResistanceFanRectangleThickness,
                    RectangleStyle = FibonacciSpeedResistanceFanRectangleStyle,
                    RectangleColor = ColorParser.Parse(FibonacciSpeedResistanceFanRectangleColor),
                    PriceLevelsThickness = FibonacciSpeedResistanceFanPriceLevelsThickness,
                    PriceLevelsStyle = FibonacciSpeedResistanceFanPriceLevelsStyle,
                    PriceLevelsColor = ColorParser.Parse(FibonacciSpeedResistanceFanPriceLevelsColor),
                    TimeLevelsThickness = FibonacciSpeedResistanceFanTimeLevelsThickness,
                    TimeLevelsStyle = FibonacciSpeedResistanceFanTimeLevelsStyle,
                    TimeLevelsColor = ColorParser.Parse(FibonacciSpeedResistanceFanTimeLevelsColor),
                    ExtendedLinesThickness = FibonacciSpeedResistanceFanExtendedLinesThickness,
                    ExtendedLinesStyle = FibonacciSpeedResistanceFanExtendedLinesStyle,
                    ExtendedLinesColor = ColorParser.Parse(FibonacciSpeedResistanceFanExtendedLinesColor),
                    ShowPriceLevels = FibonacciSpeedResistanceFanShowPriceLevels,
                    ShowTimeLevels = FibonacciSpeedResistanceFanShowTimeLevels,
                    MainFanSettings = new FanSettings
                    {
                        Color = ColorParser.Parse(FibonacciSpeedResistanceFanMainFanColor),
                        Style = FibonacciSpeedResistanceFanMainFanStyle,
                        Thickness = FibonacciSpeedResistanceFanMainFanThickness
                    },
                    SideFanSettings = new SideFanSettings[]
                    {
                        new SideFanSettings
                        {
                            Name = "1x2",
                            Percent = FibonacciSpeedResistanceFanFirstFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFirstFanColor),
                            Style = FibonacciSpeedResistanceFanFirstFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFirstFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "1x3",
                            Percent = FibonacciSpeedResistanceFanSecondFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanSecondFanColor),
                            Style = FibonacciSpeedResistanceFanSecondFanStyle,
                            Thickness = FibonacciSpeedResistanceFanSecondFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "1x4",
                            Percent = FibonacciSpeedResistanceFanThirdFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanThirdFanColor),
                            Style = FibonacciSpeedResistanceFanThirdFanStyle,
                            Thickness = FibonacciSpeedResistanceFanThirdFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "1x8",
                            Percent = FibonacciSpeedResistanceFanFourthFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFourthFanColor),
                            Style = FibonacciSpeedResistanceFanFourthFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFourthFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "1x9",
                            Percent = FibonacciSpeedResistanceFanFifthFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFifthFanColor),
                            Style = FibonacciSpeedResistanceFanFifthFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFifthFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "2x1",
                            Percent = -FibonacciSpeedResistanceFanFirstFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFirstFanColor),
                            Style = FibonacciSpeedResistanceFanFirstFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFirstFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "3x1",
                            Percent = -FibonacciSpeedResistanceFanSecondFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanSecondFanColor),
                            Style = FibonacciSpeedResistanceFanSecondFanStyle,
                            Thickness = FibonacciSpeedResistanceFanSecondFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "4x1",
                            Percent = -FibonacciSpeedResistanceFanThirdFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanThirdFanColor),
                            Style = FibonacciSpeedResistanceFanThirdFanStyle,
                            Thickness = FibonacciSpeedResistanceFanThirdFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "8x1",
                            Percent = -FibonacciSpeedResistanceFanFourthFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFourthFanColor),
                            Style = FibonacciSpeedResistanceFanFourthFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFourthFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "9x1",
                            Percent = -FibonacciSpeedResistanceFanFifthFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFifthFanColor),
                            Style = FibonacciSpeedResistanceFanFifthFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFifthFanThickness
                        }
                    }
                }),
                new FibonacciTimeZonePattern(patternConfig, GetFibonacciTimeZoneLevels()),
                new TrendBasedFibonacciTimePattern(patternConfig, GetTrendBasedFibonacciTimeLevels()),
                new FibonacciChannelPattern(patternConfig, GetFibonacciChannelLevels())
            };

            InitializePatterns(patternsGroupButton.Patterns);
        }

        private Dictionary<double, PercentLineSettings> GetOriginalPitchforkLevels()
        {
            var originalPitchforkLevels = new Dictionary<double, PercentLineSettings>();

            if (ShowFirstOriginalPitchfork)
            {
                originalPitchforkLevels.Add(FirstOriginalPitchforkPercent, new PercentLineSettings
                {
                    Percent = FirstOriginalPitchforkPercent,
                    LineColor = ColorParser.Parse(FirstOriginalPitchforkColor),
                    Style = FirstOriginalPitchforkStyle,
                    Thickness = FirstOriginalPitchforkThickness,
                });
            }

            if (ShowSecondOriginalPitchfork)
            {
                originalPitchforkLevels.Add(SecondOriginalPitchforkPercent, new PercentLineSettings
                {
                    Percent = SecondOriginalPitchforkPercent,
                    LineColor = ColorParser.Parse(SecondOriginalPitchforkColor),
                    Style = SecondOriginalPitchforkStyle,
                    Thickness = SecondOriginalPitchforkThickness,
                });
            }

            if (ShowThirdOriginalPitchfork)
            {
                originalPitchforkLevels.Add(ThirdOriginalPitchforkPercent, new PercentLineSettings
                {
                    Percent = ThirdOriginalPitchforkPercent,
                    LineColor = ColorParser.Parse(ThirdOriginalPitchforkColor),
                    Style = ThirdOriginalPitchforkStyle,
                    Thickness = ThirdOriginalPitchforkThickness,
                });
            }

            if (ShowFourthOriginalPitchfork)
            {
                originalPitchforkLevels.Add(FourthOriginalPitchforkPercent, new PercentLineSettings
                {
                    Percent = FourthOriginalPitchforkPercent,
                    LineColor = ColorParser.Parse(FourthOriginalPitchforkColor),
                    Style = FourthOriginalPitchforkStyle,
                    Thickness = FourthOriginalPitchforkThickness,
                });
            }

            if (ShowFifthOriginalPitchfork)
            {
                originalPitchforkLevels.Add(FifthOriginalPitchforkPercent, new PercentLineSettings
                {
                    Percent = FifthOriginalPitchforkPercent,
                    LineColor = ColorParser.Parse(FifthOriginalPitchforkColor),
                    Style = FifthOriginalPitchforkStyle,
                    Thickness = FifthOriginalPitchforkThickness,
                });
            }

            if (ShowSixthOriginalPitchfork)
            {
                originalPitchforkLevels.Add(SixthOriginalPitchforkPercent, new PercentLineSettings
                {
                    Percent = SixthOriginalPitchforkPercent,
                    LineColor = ColorParser.Parse(SixthOriginalPitchforkColor),
                    Style = SixthOriginalPitchforkStyle,
                    Thickness = SixthOriginalPitchforkThickness,
                });
            }

            if (ShowSeventhOriginalPitchfork)
            {
                originalPitchforkLevels.Add(SeventhOriginalPitchforkPercent, new PercentLineSettings
                {
                    Percent = SeventhOriginalPitchforkPercent,
                    LineColor = ColorParser.Parse(SeventhOriginalPitchforkColor),
                    Style = SeventhOriginalPitchforkStyle,
                    Thickness = SeventhOriginalPitchforkThickness,
                });
            }

            if (ShowEighthOriginalPitchfork)
            {
                originalPitchforkLevels.Add(EighthOriginalPitchforkPercent, new PercentLineSettings
                {
                    Percent = EighthOriginalPitchforkPercent,
                    LineColor = ColorParser.Parse(EighthOriginalPitchforkColor),
                    Style = EighthOriginalPitchforkStyle,
                    Thickness = EighthOriginalPitchforkThickness,
                });
            }

            if (ShowNinthOriginalPitchfork)
            {
                originalPitchforkLevels.Add(NinthOriginalPitchforkPercent, new PercentLineSettings
                {
                    Percent = NinthOriginalPitchforkPercent,
                    LineColor = ColorParser.Parse(NinthOriginalPitchforkColor),
                    Style = NinthOriginalPitchforkStyle,
                    Thickness = NinthOriginalPitchforkThickness,
                });
            }

            return originalPitchforkLevels;
        }

        private Dictionary<double, PercentLineSettings> GetSchiffPitchforkLevels()
        {
            var schiffPitchforkLevels = new Dictionary<double, PercentLineSettings>();

            if (ShowFirstSchiffPitchfork)
            {
                schiffPitchforkLevels.Add(FirstSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = FirstSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(FirstSchiffPitchforkColor),
                    Style = FirstSchiffPitchforkStyle,
                    Thickness = FirstSchiffPitchforkThickness,
                });
            }

            if (ShowSecondSchiffPitchfork)
            {
                schiffPitchforkLevels.Add(SecondSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = SecondSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(SecondSchiffPitchforkColor),
                    Style = SecondSchiffPitchforkStyle,
                    Thickness = SecondSchiffPitchforkThickness,
                });
            }

            if (ShowThirdSchiffPitchfork)
            {
                schiffPitchforkLevels.Add(ThirdSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = ThirdSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(ThirdSchiffPitchforkColor),
                    Style = ThirdSchiffPitchforkStyle,
                    Thickness = ThirdSchiffPitchforkThickness,
                });
            }

            if (ShowFourthSchiffPitchfork)
            {
                schiffPitchforkLevels.Add(FourthSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = FourthSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(FourthSchiffPitchforkColor),
                    Style = FourthSchiffPitchforkStyle,
                    Thickness = FourthSchiffPitchforkThickness,
                });
            }

            if (ShowFifthSchiffPitchfork)
            {
                schiffPitchforkLevels.Add(FifthSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = FifthSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(FifthSchiffPitchforkColor),
                    Style = FifthSchiffPitchforkStyle,
                    Thickness = FifthSchiffPitchforkThickness,
                });
            }

            if (ShowSixthSchiffPitchfork)
            {
                schiffPitchforkLevels.Add(SixthSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = SixthSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(SixthSchiffPitchforkColor),
                    Style = SixthSchiffPitchforkStyle,
                    Thickness = SixthSchiffPitchforkThickness,
                });
            }

            if (ShowSeventhSchiffPitchfork)
            {
                schiffPitchforkLevels.Add(SeventhSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = SeventhSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(SeventhSchiffPitchforkColor),
                    Style = SeventhSchiffPitchforkStyle,
                    Thickness = SeventhSchiffPitchforkThickness,
                });
            }

            if (ShowEighthSchiffPitchfork)
            {
                schiffPitchforkLevels.Add(EighthSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = EighthSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(EighthSchiffPitchforkColor),
                    Style = EighthSchiffPitchforkStyle,
                    Thickness = EighthSchiffPitchforkThickness,
                });
            }

            if (ShowNinthSchiffPitchfork)
            {
                schiffPitchforkLevels.Add(NinthSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = NinthSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(NinthSchiffPitchforkColor),
                    Style = NinthSchiffPitchforkStyle,
                    Thickness = NinthSchiffPitchforkThickness,
                });
            }

            return schiffPitchforkLevels;
        }

        private Dictionary<double, PercentLineSettings> GetModifiedSchiffPitchforkLevels()
        {
            var modifiedSchiffPitchforkLevels = new Dictionary<double, PercentLineSettings>();

            if (ShowFirstModifiedSchiffPitchfork)
            {
                modifiedSchiffPitchforkLevels.Add(FirstModifiedSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = FirstModifiedSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(FirstModifiedSchiffPitchforkColor),
                    Style = FirstModifiedSchiffPitchforkStyle,
                    Thickness = FirstModifiedSchiffPitchforkThickness,
                });
            }

            if (ShowSecondModifiedSchiffPitchfork)
            {
                modifiedSchiffPitchforkLevels.Add(SecondModifiedSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = SecondModifiedSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(SecondModifiedSchiffPitchforkColor),
                    Style = SecondModifiedSchiffPitchforkStyle,
                    Thickness = SecondModifiedSchiffPitchforkThickness,
                });
            }

            if (ShowThirdModifiedSchiffPitchfork)
            {
                modifiedSchiffPitchforkLevels.Add(ThirdModifiedSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = ThirdModifiedSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(ThirdModifiedSchiffPitchforkColor),
                    Style = ThirdModifiedSchiffPitchforkStyle,
                    Thickness = ThirdModifiedSchiffPitchforkThickness,
                });
            }

            if (ShowFourthModifiedSchiffPitchfork)
            {
                modifiedSchiffPitchforkLevels.Add(FourthModifiedSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = FourthModifiedSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(FourthModifiedSchiffPitchforkColor),
                    Style = FourthModifiedSchiffPitchforkStyle,
                    Thickness = FourthModifiedSchiffPitchforkThickness,
                });
            }

            if (ShowFifthModifiedSchiffPitchfork)
            {
                modifiedSchiffPitchforkLevels.Add(FifthModifiedSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = FifthModifiedSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(FifthModifiedSchiffPitchforkColor),
                    Style = FifthModifiedSchiffPitchforkStyle,
                    Thickness = FifthModifiedSchiffPitchforkThickness,
                });
            }

            if (ShowSixthModifiedSchiffPitchfork)
            {
                modifiedSchiffPitchforkLevels.Add(SixthModifiedSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = SixthModifiedSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(SixthModifiedSchiffPitchforkColor),
                    Style = SixthModifiedSchiffPitchforkStyle,
                    Thickness = SixthModifiedSchiffPitchforkThickness,
                });
            }

            if (ShowSeventhModifiedSchiffPitchfork)
            {
                modifiedSchiffPitchforkLevels.Add(SeventhModifiedSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = SeventhModifiedSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(SeventhModifiedSchiffPitchforkColor),
                    Style = SeventhModifiedSchiffPitchforkStyle,
                    Thickness = SeventhModifiedSchiffPitchforkThickness,
                });
            }

            if (ShowEighthModifiedSchiffPitchfork)
            {
                modifiedSchiffPitchforkLevels.Add(EighthModifiedSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = EighthModifiedSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(EighthModifiedSchiffPitchforkColor),
                    Style = EighthModifiedSchiffPitchforkStyle,
                    Thickness = EighthModifiedSchiffPitchforkThickness,
                });
            }

            if (ShowNinthModifiedSchiffPitchfork)
            {
                modifiedSchiffPitchforkLevels.Add(NinthModifiedSchiffPitchforkPercent, new PercentLineSettings
                {
                    Percent = NinthModifiedSchiffPitchforkPercent,
                    LineColor = ColorParser.Parse(NinthModifiedSchiffPitchforkColor),
                    Style = NinthModifiedSchiffPitchforkStyle,
                    Thickness = NinthModifiedSchiffPitchforkThickness,
                });
            }

            return modifiedSchiffPitchforkLevels;
        }

        private SideFanSettings[] GetPitchfanSideFanSettings()
        {
            var result = new List<SideFanSettings>();

            if (ShowFirstPitchfan)
            {
                result.Add(new SideFanSettings
                {
                    Percent = FirstPitchfanPercent,
                    Color = ColorParser.Parse(FirstPitchfanColor),
                    Style = FirstPitchfanStyle,
                    Thickness = FirstPitchfanThickness,
                });
            }

            if (ShowSecondPitchfan)
            {
                result.Add(new SideFanSettings
                {
                    Percent = SecondPitchfanPercent,
                    Color = ColorParser.Parse(SecondPitchfanColor),
                    Style = SecondPitchfanStyle,
                    Thickness = SecondPitchfanThickness,
                });
            }

            if (ShowThirdPitchfan)
            {
                result.Add(new SideFanSettings
                {
                    Percent = ThirdPitchfanPercent,
                    Color = ColorParser.Parse(ThirdPitchfanColor),
                    Style = ThirdPitchfanStyle,
                    Thickness = ThirdPitchfanThickness,
                });
            }

            if (ShowFourthPitchfan)
            {
                result.Add(new SideFanSettings
                {
                    Percent = FourthPitchfanPercent,
                    Color = ColorParser.Parse(FourthPitchfanColor),
                    Style = FourthPitchfanStyle,
                    Thickness = FourthPitchfanThickness,
                });
            }

            if (ShowFifthPitchfan)
            {
                result.Add(new SideFanSettings
                {
                    Percent = FifthPitchfanPercent,
                    Color = ColorParser.Parse(FifthPitchfanColor),
                    Style = FifthPitchfanStyle,
                    Thickness = FifthPitchfanThickness,
                });
            }

            if (ShowSixthPitchfan)
            {
                result.Add(new SideFanSettings
                {
                    Percent = SixthPitchfanPercent,
                    Color = ColorParser.Parse(SixthPitchfanColor),
                    Style = SixthPitchfanStyle,
                    Thickness = SixthPitchfanThickness,
                });
            }

            if (ShowSeventhPitchfan)
            {
                result.Add(new SideFanSettings
                {
                    Percent = SeventhPitchfanPercent,
                    Color = ColorParser.Parse(SeventhPitchfanColor),
                    Style = SeventhPitchfanStyle,
                    Thickness = SeventhPitchfanThickness,
                });
            }

            if (ShowEighthPitchfan)
            {
                result.Add(new SideFanSettings
                {
                    Percent = EighthPitchfanPercent,
                    Color = ColorParser.Parse(EighthPitchfanColor),
                    Style = EighthPitchfanStyle,
                    Thickness = EighthPitchfanThickness,
                });
            }

            if (ShowNinthPitchfan)
            {
                result.Add(new SideFanSettings
                {
                    Percent = NinthPitchfanPercent,
                    Color = ColorParser.Parse(NinthPitchfanColor),
                    Style = NinthPitchfanStyle,
                    Thickness = NinthPitchfanThickness,
                });
            }

            result.ToList().ForEach(iSettings => result.Add(new SideFanSettings
            {
                Percent = -iSettings.Percent,
                Color = iSettings.Color,
                Style = iSettings.Style,
                Thickness = iSettings.Thickness,
            }));

            return result.ToArray();
        }

        private void AddPitchforkPatterns(PatternConfig patternConfig)
        {
            var patternsGroupButton = AddPatternGroupButton("Pitchfork");

            patternsGroupButton.Patterns = new IPattern[]
            {
                new OriginalPitchforkPattern(patternConfig, new LineSettings
                {
                    LineColor = ColorParser.Parse(OriginalPitchforkMedianColor),
                    Style = OriginalPitchforkMedianStyle,
                    Thickness = OriginalPitchforkMedianThickness
                }, GetOriginalPitchforkLevels()),
                new SchiffPitchforkPattern(patternConfig, new LineSettings
                {
                    LineColor = ColorParser.Parse(SchiffPitchforkMedianColor),
                    Style = SchiffPitchforkMedianStyle,
                    Thickness = SchiffPitchforkMedianThickness
                }, GetSchiffPitchforkLevels()),
                new ModifiedSchiffPitchforkPattern(patternConfig, new LineSettings
                {
                    LineColor = ColorParser.Parse(ModifiedSchiffPitchforkMedianColor),
                    Style = ModifiedSchiffPitchforkMedianStyle,
                    Thickness = ModifiedSchiffPitchforkMedianThickness
                }, GetModifiedSchiffPitchforkLevels()),
                new PitchfanPattern(patternConfig, GetPitchfanSideFanSettings(), new FanSettings
                {
                    Color = ColorParser.Parse(PitchfanMedianColor),
                    Style = PitchfanMedianStyle,
                    Thickness = PitchfanMedianThickness
                })
            };

            InitializePatterns(patternsGroupButton.Patterns);
        }

        private void AddElliottImpulseWavgePattern(PatternConfig patternConfig)
        {
            var elliottImpulseWavePatternGroupButton = AddPatternGroupButton("EW 12345");

            elliottImpulseWavePatternGroupButton.Patterns = new IPattern[]
            {
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.SuperMellennium),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.Mellennium),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.SubMellennium),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.GrandSuperCycle),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.SuperCycle),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.Cycle),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.Primary),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.Intermediate),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.Minor),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.Minute),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.Minuette),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.SubMinuette),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.Micro),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.SubMicro),
                new ElliottImpulseWavePattern(patternConfig, ElliottWaveDegree.Minuscule),
            };

            InitializePatterns(elliottImpulseWavePatternGroupButton.Patterns);
        }

        private void AddElliottCorrectionWavePattern(PatternConfig patternConfig)
        {
            var elliottCorrectionWavePatternGroupButton = AddPatternGroupButton("EW ABC");

            elliottCorrectionWavePatternGroupButton.Patterns = new IPattern[]
            {
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.SuperMellennium),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.Mellennium),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.SubMellennium),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.GrandSuperCycle),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.SuperCycle),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.Cycle),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.Primary),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.Intermediate),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.Minor),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.Minute),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.Minuette),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.SubMinuette),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.Micro),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.SubMicro),
                new ElliottCorrectionWavePattern(patternConfig, ElliottWaveDegree.Minuscule),
            };

            InitializePatterns(elliottCorrectionWavePatternGroupButton.Patterns);
        }

        private void AddElliottTriangleWavePattern(PatternConfig patternConfig)
        {
            var elliottTriangleWavePatternGroupButton = AddPatternGroupButton("EW ABCDE");

            elliottTriangleWavePatternGroupButton.Patterns = new IPattern[]
            {
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.SuperMellennium),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.Mellennium),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.SubMellennium),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.GrandSuperCycle),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.SuperCycle),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.Cycle),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.Primary),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.Intermediate),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.Minor),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.Minute),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.Minuette),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.SubMinuette),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.Micro),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.SubMicro),
                new ElliottTriangleWavePattern(patternConfig, ElliottWaveDegree.Minuscule),
            };

            InitializePatterns(elliottTriangleWavePatternGroupButton.Patterns);
        }

        private void AddElliottTripleComboWavePattern(PatternConfig patternConfig)
        {
            var elliottTripleComboWavePatternGroupButton = AddPatternGroupButton("EW WXYXZ");

            elliottTripleComboWavePatternGroupButton.Patterns = new IPattern[]
            {
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.SuperMellennium),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.Mellennium),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.SubMellennium),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.GrandSuperCycle),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.SuperCycle),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.Cycle),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.Primary),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.Intermediate),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.Minor),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.Minute),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.Minuette),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.SubMinuette),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.Micro),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.SubMicro),
                new ElliottTripleComboWavePattern(patternConfig, ElliottWaveDegree.Minuscule),
            };

            InitializePatterns(elliottTripleComboWavePatternGroupButton.Patterns);
        }

        private void AddElliottDoubleComboWavePattern(PatternConfig patternConfig)
        {
            var elliottDoubleComboWavePatternGroupButton = AddPatternGroupButton("EW WXY");

            elliottDoubleComboWavePatternGroupButton.Patterns = new IPattern[]
            {
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.SuperMellennium),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.Mellennium),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.SubMellennium),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.GrandSuperCycle),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.SuperCycle),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.Cycle),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.Primary),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.Intermediate),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.Minor),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.Minute),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.Minuette),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.SubMinuette),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.Micro),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.SubMicro),
                new ElliottDoubleComboWavePattern(patternConfig, ElliottWaveDegree.Minuscule),
            };

            InitializePatterns(elliottDoubleComboWavePatternGroupButton.Patterns);
        }
    }
}