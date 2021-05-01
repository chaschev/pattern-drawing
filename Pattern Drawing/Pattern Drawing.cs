using cAlgo.API;
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

        [Parameter("Color", DefaultValue = "Red", Group = "Patterns Color")]
        public string PatternsColor { get; set; }

        [Parameter("Alpha", DefaultValue = 100, MinValue = 0, MaxValue = 255, Group = "Patterns Color")]
        public int PatternsColorAlpha { get; set; }

        [Parameter("Show", DefaultValue = true, Group = "Patterns Label")]
        public bool PatternsLabelShow { get; set; }

        [Parameter("Color", DefaultValue = "Yellow", Group = "Patterns Label")]
        public string PatternsLabelColor { get; set; }

        [Parameter("Alpha", DefaultValue = 100, MinValue = 0, MaxValue = 255, Group = "Patterns Label")]
        public int PatternsLabelColorAlpha { get; set; }

        [Parameter("Locked", DefaultValue = true, Group = "Patterns Label")]
        public bool PatternsLabelLocked { get; set; }

        [Parameter("Orientation", DefaultValue = Orientation.Vertical, Group = "Container Panel")]
        public Orientation PanelOrientation { get; set; }

        [Parameter("Horizontal Alignment", DefaultValue = HorizontalAlignment.Left, Group = "Container Panel")]
        public HorizontalAlignment PanelHorizontalAlignment { get; set; }

        [Parameter("Vertical Alignment", DefaultValue = VerticalAlignment.Top, Group = "Container Panel")]
        public VerticalAlignment PanelVerticalAlignment { get; set; }

        [Parameter("Margin", DefaultValue = 3, Group = "Container Panel")]
        public double PanelMargin { get; set; }

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

        [Parameter("Number", DefaultValue = 100, MinValue = 1, Group = "Cycles")]
        public int CyclesNumber { get; set; }

        [Parameter("Enable", DefaultValue = false, Group = "TimeFrame Visibility")]
        public bool IsTimeFrameVisibilityEnabled { get; set; }

        [Parameter("TimeFrame", Group = "TimeFrame Visibility")]
        public TimeFrame VisibilityTimeFrame { get; set; }

        [Parameter("Only Buttons", Group = "TimeFrame Visibility")]
        public bool VisibilityOnlyButtons { get; set; }

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

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowFirstFibonacciRetracement { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0, MinValue = 0, Group = "Fibonacci Retracement")]
        public double FirstFibonacciRetracementPercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Fibonacci Retracement")]
        public string FirstFibonacciRetracementColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int FirstFibonacciRetracementAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int FirstFibonacciRetracementThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle FirstFibonacciRetracementStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowSecondFibonacciRetracement { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.236, MinValue = 0, Group = "Fibonacci Retracement")]
        public double SecondFibonacciRetracementPercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Fibonacci Retracement")]
        public string SecondFibonacciRetracementColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int SecondFibonacciRetracementAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int SecondFibonacciRetracementThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle SecondFibonacciRetracementStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowThirdFibonacciRetracement { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.382, MinValue = 0, Group = "Fibonacci Retracement")]
        public double ThirdFibonacciRetracementPercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Fibonacci Retracement")]
        public string ThirdFibonacciRetracementColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int ThirdFibonacciRetracementAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int ThirdFibonacciRetracementThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle ThirdFibonacciRetracementStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowFourthFibonacciRetracement { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.5, MinValue = 0, Group = "Fibonacci Retracement")]
        public double FourthFibonacciRetracementPercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Fibonacci Retracement")]
        public string FourthFibonacciRetracementColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int FourthFibonacciRetracementAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int FourthFibonacciRetracementThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle FourthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowFifthFibonacciRetracement { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 0.618, MinValue = 0, Group = "Fibonacci Retracement")]
        public double FifthFibonacciRetracementPercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Fibonacci Retracement")]
        public string FifthFibonacciRetracementColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int FifthFibonacciRetracementAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int FifthFibonacciRetracementThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle FifthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowSixthFibonacciRetracement { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 0.786, MinValue = 0, Group = "Fibonacci Retracement")]
        public double SixthFibonacciRetracementPercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Fibonacci Retracement")]
        public string SixthFibonacciRetracementColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int SixthFibonacciRetracementAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int SixthFibonacciRetracementThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle SixthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowSeventhFibonacciRetracement { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public double SeventhFibonacciRetracementPercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Fibonacci Retracement")]
        public string SeventhFibonacciRetracementColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int SeventhFibonacciRetracementAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int SeventhFibonacciRetracementThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle SeventhFibonacciRetracementStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowEighthFibonacciRetracement { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 1.618, MinValue = 0, Group = "Fibonacci Retracement")]
        public double EighthFibonacciRetracementPercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Fibonacci Retracement")]
        public string EighthFibonacciRetracementColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int EighthFibonacciRetracementAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int EighthFibonacciRetracementThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle EighthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowNinthFibonacciRetracement { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2.618, MinValue = 0, Group = "Fibonacci Retracement")]
        public double NinthFibonacciRetracementPercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Fibonacci Retracement")]
        public string NinthFibonacciRetracementColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int NinthFibonacciRetracementAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int NinthFibonacciRetracementThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle NinthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 10th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowTenthFibonacciRetracement { get; set; }

        [Parameter("10th Level Percent", DefaultValue = 3.618, MinValue = 0, Group = "Fibonacci Retracement")]
        public double TenthFibonacciRetracementPercent { get; set; }

        [Parameter("10th Level Color", DefaultValue = "Aquamarine", Group = "Fibonacci Retracement")]
        public string TenthFibonacciRetracementColor { get; set; }

        [Parameter("10th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int TenthFibonacciRetracementAlpha { get; set; }

        [Parameter("10th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int TenthFibonacciRetracementThickness { get; set; }

        [Parameter("10th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle TenthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 11th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowEleventhFibonacciRetracement { get; set; }

        [Parameter("11th Level Percent", DefaultValue = 4.236, MinValue = 0, Group = "Fibonacci Retracement")]
        public double EleventhFibonacciRetracementPercent { get; set; }

        [Parameter("11th Level Color", DefaultValue = "Chocolate", Group = "Fibonacci Retracement")]
        public string EleventhFibonacciRetracementColor { get; set; }

        [Parameter("11th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int EleventhFibonacciRetracementAlpha { get; set; }

        [Parameter("11th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int EleventhFibonacciRetracementThickness { get; set; }

        [Parameter("11th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle EleventhFibonacciRetracementStyle { get; set; }

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

            var patternConfig = new PatternConfig(Chart, patternsColor, PatternsLabelShow, patternsLabelsColor, PatternsLabelLocked)
            {
                Print = Print
            };

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

            AddElliottCorrectionWavePattern(patternConfig);
            AddElliottImpulseWavgePattern(patternConfig);
            AddElliottTriangleWavePattern(patternConfig);
            AddElliottTripleComboWavePattern(patternConfig);
            AddElliottDoubleComboWavePattern(patternConfig);

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

        public override void Calculate(int index)
        {
        }

        private void ShowHideButton_TurnedOff(Controls.ToggleButton obj)
        {
            ChangePatternsVisibility(false);

            obj.Text = "Hide";
        }

        private void ShowHideButton_TurnedOn(Controls.ToggleButton obj)
        {
            ChangePatternsVisibility(true);

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

        private void ChangePatternsVisibility(bool isHidden)
        {
            var chartObjects = Chart.Objects.ToArray();

            foreach (var chartObject in chartObjects)
            {
                if (!chartObject.IsPattern()) continue;

                chartObject.IsHidden = isHidden;
            }
        }

        private void CheckTimeFrameVisibility()
        {
            if (IsTimeFrameVisibilityEnabled)
            {
                if (TimeFrame != VisibilityTimeFrame)
                {
                    _mainButtonsPanel.IsVisible = false;

                    if (!VisibilityOnlyButtons) ChangePatternsVisibility(true);
                }
                else if (!VisibilityOnlyButtons)
                {
                    ChangePatternsVisibility(false);
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
                new GannFanPattern(patternConfig, new GannFanSettings
                {
                    OneThickness = GannFanOneThickness,
                    OneStyle = GannFanOneStyle,
                    OneColor = ColorParser.Parse(GannFanOneColor),
                    TwoThickness = GannFanTwoThickness,
                    TwoStyle = GannFanTwoStyle,
                    TwoColor = ColorParser.Parse(GannFanTwoColor),
                    ThreeThickness = GannFanThreeThickness,
                    ThreeStyle = GannFanThreeStyle,
                    ThreeColor = ColorParser.Parse(GannFanThreeColor),
                    FourThickness = GannFanFourThickness,
                    FourStyle = GannFanFourStyle,
                    FourColor = ColorParser.Parse(GannFanFourColor),
                    EightThickness = GannFanEightThickness,
                    EightStyle = GannFanEightStyle,
                    EightColor = ColorParser.Parse(GannFanEightColor),
                })
            };

            InitializePatterns(gannPatternsGroupButton.Patterns);
        }

        private IEnumerable<FibonacciRetracementLevel> GetFibonacciRetracementLevels()
        {
            var fibonacciRetracementLevels = new List<FibonacciRetracementLevel>();

            if (ShowFirstFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = FirstFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(FirstFibonacciRetracementColor),
                    Style = FirstFibonacciRetracementStyle,
                    Thickness = FirstFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(FirstFibonacciRetracementColor, FirstFibonacciRetracementAlpha),
                });
            }

            if (ShowSecondFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = SecondFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(SecondFibonacciRetracementColor),
                    Style = SecondFibonacciRetracementStyle,
                    Thickness = SecondFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(SecondFibonacciRetracementColor, SecondFibonacciRetracementAlpha),
                });
            }

            if (ShowThirdFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = ThirdFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(ThirdFibonacciRetracementColor),
                    Style = ThirdFibonacciRetracementStyle,
                    Thickness = ThirdFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(ThirdFibonacciRetracementColor, ThirdFibonacciRetracementAlpha),
                });
            }

            if (ShowFourthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = FourthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(FourthFibonacciRetracementColor),
                    Style = FourthFibonacciRetracementStyle,
                    Thickness = FourthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(FourthFibonacciRetracementColor, FourthFibonacciRetracementAlpha),
                });
            }

            if (ShowFifthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = FifthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(FifthFibonacciRetracementColor),
                    Style = FifthFibonacciRetracementStyle,
                    Thickness = FifthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(FifthFibonacciRetracementColor, FifthFibonacciRetracementAlpha),
                });
            }

            if (ShowSixthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = SixthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(SixthFibonacciRetracementColor),
                    Style = SixthFibonacciRetracementStyle,
                    Thickness = SixthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(SixthFibonacciRetracementColor, SixthFibonacciRetracementAlpha),
                });
            }

            if (ShowSeventhFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = SeventhFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(SeventhFibonacciRetracementColor),
                    Style = SeventhFibonacciRetracementStyle,
                    Thickness = SeventhFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(SeventhFibonacciRetracementColor, SeventhFibonacciRetracementAlpha),
                });
            }

            if (ShowEighthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = EighthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(EighthFibonacciRetracementColor),
                    Style = EighthFibonacciRetracementStyle,
                    Thickness = EighthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(EighthFibonacciRetracementColor, EighthFibonacciRetracementAlpha),
                });
            }

            if (ShowNinthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = NinthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(NinthFibonacciRetracementColor),
                    Style = NinthFibonacciRetracementStyle,
                    Thickness = NinthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(NinthFibonacciRetracementColor, NinthFibonacciRetracementAlpha),
                });
            }

            if (ShowTenthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = TenthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(TenthFibonacciRetracementColor),
                    Style = TenthFibonacciRetracementStyle,
                    Thickness = TenthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(TenthFibonacciRetracementColor, TenthFibonacciRetracementAlpha),
                });
            }

            if (ShowEleventhFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new FibonacciRetracementLevel
                {
                    Percent = EleventhFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(EleventhFibonacciRetracementColor),
                    Style = EleventhFibonacciRetracementStyle,
                    Thickness = EleventhFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(EleventhFibonacciRetracementColor, EleventhFibonacciRetracementAlpha),
                });
            }

            return fibonacciRetracementLevels;
        }

        private void AddFibonacciPatterns(PatternConfig patternConfig)
        {
            var gannPatternsGroupButton = AddPatternGroupButton("Fibonacci");

            gannPatternsGroupButton.Patterns = new IPattern[]
            {
                new FibonacciRetracementPattern(patternConfig, GetFibonacciRetracementLevels()),
            };

            InitializePatterns(gannPatternsGroupButton.Patterns);
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