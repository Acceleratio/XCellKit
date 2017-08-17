﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Acceleratio.XCellKit
{
    internal class PieTypeChart : ChartPropertiesSetup
    {
        public override bool AxisX { get; set; } = false;
        public override bool AxisY { get; set; } = false;

        public override OpenXmlCompositeElement CreateChart(PlotArea plotArea)
        {
            var chart = plotArea.AppendChild<PieChart>(new PieChart());
            chart.Append(new VaryColors() { Val = true });
            chart.Append(new FirstSliceAngle() { Val = (UInt16Value)0U });

            return chart;
        }

        public override OpenXmlCompositeElement CreateChartSeries(string title, uint seriesNumber, OpenXmlCompositeElement chart)
        {
            // Create new line series with specified name.
            var chartSeries = chart.AppendChild<PieChartSeries>(new PieChartSeries(
                new Index() { Val = new UInt32Value(seriesNumber) },
                new Order() { Val = new UInt32Value(seriesNumber) },
                new SeriesText(new NumericValue() { Text = title })));

            return chartSeries;
        }
    }
}
