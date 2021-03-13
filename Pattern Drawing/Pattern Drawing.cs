using cAlgo.API;
using cAlgo.Controls;
using cAlgo.Helpers;
using cAlgo.Patterns;

namespace cAlgo
{
    /// <summary>
    /// This indicator allows you to draw chart patterns
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class PatternDrawing : Indicator
    {
        private StackPanel _panel;

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

        [Parameter("Background Color", DefaultValue = "Gray", Group = "Container Panel")]
        public string PanelBackgroundColor { get; set; }

        [Parameter("Opacity", DefaultValue = 1, MinValue = 0, MaxValue = 1, Group = "Container Panel")]
        public double PanelOpacity { get; set; }

        [Parameter("Margin", DefaultValue = 3, Group = "Container Panel")]
        public double PanelMargin { get; set; }

        [Parameter("Background Disable Color", DefaultValue = "#FFCCCCCC", Group = "Buttons")]
        public string ButtonsBackgroundDisableColor { get; set; }

        [Parameter("Background Disable Color", DefaultValue = "Red", Group = "Buttons")]
        public string ButtonsBackgroundEnableColor { get; set; }

        [Parameter("Foreground Color", DefaultValue = "Blue", Group = "Buttons")]
        public string ButtonsForegroundColor { get; set; }

        [Parameter("Margin", DefaultValue = 1, Group = "Buttons")]
        public double ButtonsMargin { get; set; }

        [Parameter("Width", DefaultValue = 200, Group = "Buttons")]
        public double ButtonsWidth { get; set; }

        [Parameter("Height", DefaultValue = 20, Group = "Buttons")]
        public double ButtonsHeight { get; set; }

        protected override void Initialize()
        {
            _panel = new StackPanel
            {
                HorizontalAlignment = PanelHorizontalAlignment,
                VerticalAlignment = PanelVerticalAlignment,
                Orientation = PanelOrientation,
                BackgroundColor = ColorParser.Parse(PanelBackgroundColor),
                Opacity = PanelOpacity,
                Margin = PanelMargin,
            };

            _buttonsBackgroundDisableColor = ColorParser.Parse(ButtonsBackgroundDisableColor);
            _buttonsBackgroundEnableColor = ColorParser.Parse(ButtonsBackgroundEnableColor);

            _buttonsStyle = new Style();

            _buttonsStyle.Set(ControlProperty.Margin, ButtonsMargin);
            _buttonsStyle.Set(ControlProperty.BackgroundColor, _buttonsBackgroundDisableColor);
            _buttonsStyle.Set(ControlProperty.ForegroundColor, ColorParser.Parse(ButtonsForegroundColor));
            _buttonsStyle.Set(ControlProperty.Width, ButtonsWidth);
            _buttonsStyle.Set(ControlProperty.Height, ButtonsHeight);

            var patternsColor = ColorParser.Parse(PatternsColor, PatternsColorAlpha);
            var patternsLabelsColor = ColorParser.Parse(PatternsLabelColor, PatternsLabelColorAlpha);

            var patternConfig = new PatternConfig(Chart, patternsColor, PatternsLabelShow, patternsLabelsColor, PatternsLabelInteractive)
            {
                Print = Print
            };

            AddPatternButton(new TrianglePattern(patternConfig));
            AddPatternButton(new CyclicLinesPattern(patternConfig));
            AddPatternButton(new HeadAndShouldersPattern(patternConfig));
            AddPatternButton(new CypherPattern(patternConfig));
            AddPatternButton(new AbcdPattern(patternConfig));
            AddPatternButton(new ThreeDrivesPattern(patternConfig));
            AddPatternButton(new ElliottImpulseWavePattern(patternConfig));
            AddPatternButton(new ElliottTriangleWavePattern(patternConfig));
            AddPatternButton(new ElliottTripleComboWavePattern(patternConfig));
            AddPatternButton(new ElliottCorrectionWavePattern(patternConfig));
            AddPatternButton(new ElliottDoubleComboWavePattern(patternConfig));

            _panel.AddChild(new PatternsShowHideButton(Chart)
            {
                Style = _buttonsStyle,
                OnColor = _buttonsBackgroundEnableColor,
                OffColor = _buttonsBackgroundDisableColor,
            });

            _panel.AddChild(new PatternsRemoveAllButton(Chart)
            {
                Style = _buttonsStyle,
            });

            Chart.AddControl(_panel);
        }

        public override void Calculate(int index)
        {
        }

        private void AddPatternButton(IPattern pattern)
        {
            _panel.AddChild(new PatternButton(pattern)
            {
                Style = _buttonsStyle,
                OnColor = _buttonsBackgroundEnableColor,
                OffColor = _buttonsBackgroundDisableColor,
            });
        }
    }
}