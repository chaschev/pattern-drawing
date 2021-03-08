using cAlgo.API;
using cAlgo.Controls;
using cAlgo.Helpers;
using cAlgo.Patterns;

namespace cAlgo
{
    /// <summary>
    /// This indicator allows you to draw chart patterns
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PatternDrawing : Indicator
    {
        private StackPanel _panel;

        private Color _buttonsBackgroundDisableColor;

        private Color _buttonsBackgroundEnableColor;

        private Style _buttonsStyle;

        [Parameter("Color", DefaultValue = "Red", Group = "Patterns")]
        public string PatternsColor { get; set; }

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

        [Parameter("Width", DefaultValue = 100, Group = "Buttons")]
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

            var patternsColor = ColorParser.Parse(PatternsColor);

            AddPatternButton(new TrianglePattern(Chart, patternsColor));
            AddPatternButton(new CyclicLinesPattern(Chart, patternsColor));
            AddPatternButton(new HeadAndShouldersPattern(Chart, patternsColor));

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
                Width = 150
            });
        }
    }
}