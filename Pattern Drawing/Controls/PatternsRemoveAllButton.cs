using cAlgo.API;
using cAlgo.Helpers;
using System.Linq;

namespace cAlgo.Controls
{
    public class PatternsRemoveAllButton : Button
    {
        private readonly Chart _chart;

        public PatternsRemoveAllButton(Chart chart)
        {
            _chart = chart;

            Text = "Remove All";

            Click += PatternsRemoveAllButton_Click;
        }

        private void PatternsRemoveAllButton_Click(ButtonClickEventArgs obj)
        {
            var chartObjects = _chart.Objects.ToArray();

            foreach (var chartObject in chartObjects)
            {
                if (!chartObject.IsPattern()) continue;

                _chart.RemoveObject(chartObject.Name);
            }
        }
    }
}