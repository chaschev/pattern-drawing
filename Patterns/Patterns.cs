using cAlgo.API;
using cAlgo.Controls;
using cAlgo.Helpers;

namespace cAlgo
{
    /// <summary>
    /// This indicator allows you to draw chart patterns
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class Patterns : Indicator
    {
        private StackPanel _panel;

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

            var buttonsBackgroundDisableColor = ColorParser.Parse(ButtonsBackgroundDisableColor);
            var buttonsBackgroundEnableColor = ColorParser.Parse(ButtonsBackgroundEnableColor);

            var buttonsStyle = new Style();

            buttonsStyle.Set(ControlProperty.Margin, ButtonsMargin);
            buttonsStyle.Set(ControlProperty.BackgroundColor, buttonsBackgroundDisableColor);
            buttonsStyle.Set(ControlProperty.ForegroundColor, ColorParser.Parse(ButtonsForegroundColor));
            buttonsStyle.Set(ControlProperty.Width, ButtonsWidth);
            buttonsStyle.Set(ControlProperty.Height, ButtonsHeight);

            _panel.AddChild(new TriangleButton(Chart)
            {
                Style = buttonsStyle,
                OnColor = buttonsBackgroundEnableColor,
                OffColor = buttonsBackgroundDisableColor
            });

            Chart.AddControl(_panel);
        }

        public override void Calculate(int index)
        {
        }
    }
}