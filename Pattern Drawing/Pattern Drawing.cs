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

        [Parameter("Interactive", DefaultValue = "Red", Group = "Patterns Label")]
        public bool PatternsLabelInteractive { get; set; }

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

        [Parameter("Transparency", DefaultValue = 1, MinValue = 0, MaxValue = 1, Group = "Buttons")]
        public double ButtonsTransparency { get; set; }

        [Parameter("Number", DefaultValue = 100, MinValue = 1, Group = "Cycles")]
        public int CyclesNumber { get; set; }

        [Parameter("Enable", DefaultValue = false, Group = "TimeFrame Visibility")]
        public bool IsTimeFrameVisibilityEnabled { get; set; }

        [Parameter("TimeFrame", Group = "TimeFrame Visibility")]
        public TimeFrame VisibilityTimeFrame { get; set; }

        [Parameter("Only Buttons", Group = "TimeFrame Visibility")]
        public bool VisibilityOnlyButtons { get; set; }

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

            var patternConfig = new PatternConfig(Chart, patternsColor, PatternsLabelShow, patternsLabelsColor, PatternsLabelInteractive)
            {
                Print = Print
            };

            AddPatternButton(new TrianglePattern(patternConfig));
            AddPatternButton(new CyclesPattern(patternConfig, CyclesNumber));
            AddPatternButton(new HeadAndShouldersPattern(patternConfig));
            AddPatternButton(new CypherPattern(patternConfig));
            AddPatternButton(new AbcdPattern(patternConfig));
            AddPatternButton(new ThreeDrivesPattern(patternConfig));
            AddPatternButton(new GannBoxPattern(patternConfig));
            AddPatternButton(new GannSquarePattern(patternConfig));
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
                Text = "Hide"
            };

            showHideButton.TurnedOn += ShowHideButton_TurnedOn;
            showHideButton.TurnedOff += ShowHideButton_TurnedOff;

            _mainButtonsPanel.AddChild(showHideButton);

            _mainButtonsPanel.AddChild(new PatternsSaveButton(Chart)
            {
                Style = _buttonsStyle
            });

            _mainButtonsPanel.AddChild(new PatternsLoadButton(Chart)
            {
                Style = _buttonsStyle
            });

            _mainButtonsPanel.AddChild(new PatternsRemoveAllButton(Chart)
            {
                Style = _buttonsStyle
            });

            Chart.AddControl(_mainPanel);

            CheckTimeFrameVisibility();
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
            _mainButtonsPanel.AddChild(new PatternButton(pattern)
            {
                Style = _buttonsStyle,
                OnColor = _buttonsBackgroundEnableColor,
                OffColor = _buttonsBackgroundDisableColor
            });

            pattern.Initialize();
        }

        private PatternGroupButton AddPatternGroupButton(string text)
        {
            var groupButton = new PatternGroupButton(_groupButtonsPanel)
            {
                Text = text,
                Style = _buttonsStyle,
                OnColor = _buttonsBackgroundEnableColor,
                OffColor = _buttonsBackgroundDisableColor
            };

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