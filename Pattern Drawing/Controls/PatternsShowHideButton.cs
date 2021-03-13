using cAlgo.API;
using cAlgo.Helpers;
using System.Linq;

namespace cAlgo.Controls
{
    public class PatternsShowHideButton : ToggleButton
    {
        private readonly Chart _chart;

        public PatternsShowHideButton(Chart chart)
        {
            _chart = chart;

            Text = "Hide";
        }

        protected override void OnTurnedOn()
        {
            Text = "Show";

            ChangePatternsVisibility(true);
        }

        protected override void OnTurnedOff()
        {
            Text = "Hide";

            ChangePatternsVisibility(false);
        }

        private void ChangePatternsVisibility(bool isHidden)
        {
            var chartObjects = _chart.Objects.ToArray();

            foreach (var chartObject in chartObjects)
            {
                if (!chartObject.IsPattern()) continue;

                chartObject.IsHidden = isHidden;
            }
        }
    }
}