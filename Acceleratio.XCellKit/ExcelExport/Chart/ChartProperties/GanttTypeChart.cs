﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using Outline = DocumentFormat.OpenXml.Drawing.Outline;
using Values = DocumentFormat.OpenXml.Drawing.Charts.Values;

namespace Acceleratio.XCellKit.ExcelExport.Helpers
{
    public static class GanttTypeChart
    {
        /// <summary>
        /// Distinct colors for gantt bars. There might be some property instead of this.
        /// </summary>
        private static string[] ColourValues = new string[] {
            "800000", "008000", "000080", "808000", "800080", "008080", "808080",
            "C00000", "00C000", "0000C0", "C0C000", "C000C0", "00C0C0", "C0C0C0",
            "400000", "004000", "000040", "404000", "400040", "004040", "404040",
            "200000", "002000", "000020", "202000", "200020", "002020", "202020",
            "600000", "006000", "000060", "606000", "600060", "006060", "606060",
            "A00000", "00A000", "0000A0", "A0A000", "A000A0", "00A0A0", "A0A0A0",
            "E00000", "00E000", "0000E0", "E0E000", "E000E0", "00E0E0", "E0E0E0",
        };

        /// <summary>
        /// Set display, width, color and fill of borders and data (line, bar etc.) in chart.
        /// </summary>
        public static ChartShapeProperties SetChartShapeProperties(OpenXmlCompositeElement chartSeries, bool visible = true, uint colorPoints = 0)
        {
            ChartShapeProperties chartShapeProperties1 = new ChartShapeProperties();

            Outline outline1 = new Outline() { Width = 28575, CapType = LineCapValues.Round };
            Round round1 = new Round();

            outline1.Append(new NoFill());
            outline1.Append(round1);
            EffectList effectList1 = new EffectList();

            if (!visible)
            {
                chartShapeProperties1.Append(new NoFill());
            }

            chartShapeProperties1.Append(outline1);
            chartShapeProperties1.Append(effectList1);
            Marker marker1 = new Marker();
            Symbol symbol1 = new Symbol() { Val = MarkerStyleValues.None };

            marker1.Append(symbol1);
            Smooth smooth1 = new Smooth() { Val = false };

            chartSeries.Append(chartShapeProperties1);
            chartSeries.Append(marker1);
            chartSeries.Append(smooth1);

            for (uint i = 0; i < colorPoints; i++)
            {
                chartSeries.Append(colorChartLines(i));
            }

            return chartShapeProperties1;
        }

        /// <summary>
        /// Distinct color for each bar.
        /// </summary>
        private static DataPoint colorChartLines(uint lineIndex)
        {
            DataPoint dataPoint = new DataPoint();
            Index index = new Index() { Val = lineIndex };
            InvertIfNegative invertIfNegative3 = new InvertIfNegative() { Val = false };
            Bubble3D bubble3D1 = new Bubble3D() { Val = false };

            ChartShapeProperties chartShapeProperties = new ChartShapeProperties();

            SolidFill solidFill = new SolidFill();
            solidFill.SchemeColor = new SchemeColor() { Val = SchemeColorValues.Accent1 };
            solidFill.RgbColorModelHex = new RgbColorModelHex() { Val = ColourValues[lineIndex] };

            Outline outline = new Outline() { Width = 28575, CapType = LineCapValues.Round };
            outline.Append(new NoFill());
            outline.Append(new Round());
            chartShapeProperties.Append(solidFill);
            chartShapeProperties.Append(outline);
            chartShapeProperties.Append(new EffectList());

            dataPoint.Append(index);
            dataPoint.Append(chartShapeProperties);

            return dataPoint;
        }

        /// <summary>
        /// Design settings for X axis.
        /// </summary>
        public static CategoryAxis SetGanttCategoryAxis(PlotArea plotArea, bool hide = false)
        {
            return plotArea.AppendChild<CategoryAxis>(new CategoryAxis(new AxisId()
                    { Val = new UInt32Value(48650112u) }, new Scaling(new Orientation()
                {
                    Val = new EnumValue<DocumentFormat.
                        OpenXml.Drawing.Charts.OrientationValues>(DocumentFormat.OpenXml.Drawing.Charts.OrientationValues.MinMax)
                }),
                new Delete() { Val = hide },
                new AxisPosition() { Val = new EnumValue<AxisPositionValues>(AxisPositionValues.Bottom) },
                new MajorTickMark() { Val = TickMarkValues.None },
                new MinorTickMark() { Val = TickMarkValues.Outside },
                new TickLabelPosition() { Val = new EnumValue<TickLabelPositionValues>(TickLabelPositionValues.NextTo) },
                new CrossingAxis() { Val = new UInt32Value(48672768U) },
                new Crosses() { Val = new EnumValue<CrossesValues>(CrossesValues.AutoZero) },
                new AutoLabeled() { Val = new BooleanValue(true) },
                new LabelAlignment() { Val = new EnumValue<LabelAlignmentValues>(LabelAlignmentValues.Center) },
                new LabelOffset() { Val = new UInt16Value((ushort)100) }));
        }

        /// <summary>
        /// Design settings for Y axis.
        /// </summary>
        public static ValueAxis SetGanttValueAxis(PlotArea plotArea)
        {
            MajorGridlines majorGridlines1 = new MajorGridlines();
            ChartShapeProperties chartShapeProperties2 = new ChartShapeProperties();
            Outline outline2 = new Outline();
            SolidFill solidFill2 = new SolidFill();
            SchemeColor schemeColor2 = new SchemeColor() { Val = SchemeColorValues.Accent1 };
            Alpha alpha1 = new Alpha() { Val = 10000 };
            schemeColor2.Append(alpha1);
            solidFill2.Append(schemeColor2);
            outline2.Append(solidFill2);
            chartShapeProperties2.Append(outline2);
            majorGridlines1.Append(chartShapeProperties2);

            return plotArea.AppendChild<ValueAxis>(new ValueAxis(new AxisId() { Val = new UInt32Value(48672768u) },
                new Scaling(new Orientation()
                {
                    Val = new EnumValue<DocumentFormat.OpenXml.Drawing.Charts.OrientationValues>(
                        DocumentFormat.OpenXml.Drawing.Charts.OrientationValues.MinMax)
                }, new MinAxisValue()
                {
                    Val = 0.37500000000000006D
                }),
                new Delete() { Val = false },
                new AxisPosition() { Val = new EnumValue<AxisPositionValues>(AxisPositionValues.Left) },
                majorGridlines1,
                new MajorTickMark() { Val = TickMarkValues.None },
                new MinorTickMark() { Val = TickMarkValues.None },
                new MajorUnit() { Val = 4.1666666666666713E-2D },
                new DocumentFormat.OpenXml.Drawing.Charts.NumberingFormat() { FormatCode = "[$-F400]h:mm:ss\\ AM/PM", SourceLinked = false },
                new TickLabelPosition()
                {
                    Val = new EnumValue<TickLabelPositionValues>
                        (TickLabelPositionValues.NextTo)
                }, new CrossingAxis() { Val = new UInt32Value(48650112U) },
                new Crosses() { Val = new EnumValue<CrossesValues>(CrossesValues.AutoZero) },
                new CrossBetween() { Val = new EnumValue<CrossBetweenValues>(CrossBetweenValues.Between) }));


            //new MinAxisValue()
            //{
            //    Val = CalculateExcelTime(data.Min(x => x.Start.Hours > 0 ? x.Start.Subtract(new TimeSpan(1, 0, 0)) : x.Start))
            //}),
            //new MaxAxisValue()
            //{
            //    Val = CalculateExcelTime(data.Max(x => (x.End + x.Start).Add(new TimeSpan(1, 0, 0))))
            //},
        }

        /// <summary>
        /// Position the chart on the worksheet using a TwoCellAnchor object and append a GraphicFrame to the TwoCellAnchor object.
        /// </summary>
        public static TwoCellAnchor TwoCellAnchor(DrawingsPart drawingsPart, ChartPart chartPart)
        {
            drawingsPart.WorksheetDrawing = new WorksheetDrawing();
            TwoCellAnchor twoCellAnchor = drawingsPart.WorksheetDrawing.AppendChild<TwoCellAnchor>(new TwoCellAnchor());

            // Pozicija charta.
            twoCellAnchor.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.FromMarker(new ColumnId("1"),
                new ColumnOffset("581025"),
                new RowId("1"),
                new RowOffset("114300")));
            twoCellAnchor.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.ToMarker(new ColumnId("20"),
                new ColumnOffset("276225"),
                new RowId("16"),
                new RowOffset("0")));

            DocumentFormat.OpenXml.Drawing.Spreadsheet.GraphicFrame graphicFrame =
                twoCellAnchor.AppendChild<DocumentFormat.OpenXml.
                    Drawing.Spreadsheet.GraphicFrame>(new DocumentFormat.OpenXml.Drawing.
                    Spreadsheet.GraphicFrame());
            graphicFrame.Macro = "";

            // Ime charta.
            graphicFrame.Append(new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualGraphicFrameProperties(
                new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties() { Id = new UInt32Value(2u), Name = "Chart 1" },
                new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualGraphicFrameDrawingProperties()));

            graphicFrame.Append(new Transform(new Offset() { X = 0L, Y = 0L },
                new Extents() { Cx = 0L, Cy = 0L }));

            graphicFrame.Append(new Graphic(new GraphicData(new ChartReference() { Id = drawingsPart.GetIdOfPart(chartPart) })
                { Uri = "http://schemas.openxmlformats.org/drawingml/2006/chart" }));

            twoCellAnchor.Append(new ClientData());

            return twoCellAnchor;
        }

        /// <summary>
        /// Create and insert data to Axis
        /// </summary>
        public static void SetChartAxis(OpenXmlCompositeElement barChartSeries1, OpenXmlCompositeElement barChartSeries2, List<GanttData> data)
        {
            uint i = 0;
            // Y axis - prvi bar je za početnu poziciju (ne prikazuje se)
            StringLiteral stringLiteral1 = barChartSeries1.AppendChild<CategoryAxisData>(new CategoryAxisData()).AppendChild<StringLiteral>(new StringLiteral());
            stringLiteral1.Append(new PointCount() { Val = new UInt32Value((uint)data.Count) });

            StringLiteral stringLiteral2 = barChartSeries2.AppendChild<CategoryAxisData>(new CategoryAxisData()).AppendChild<StringLiteral>(new StringLiteral());
            stringLiteral2.Append(new PointCount() { Val = new UInt32Value((uint)data.Count) });

            // X axis - prvi bar je za početnu poziciju (ne prikazuje se)
            NumberLiteral numberLiteral1 = barChartSeries1.AppendChild<Values>(new Values()).AppendChild<NumberLiteral>(new NumberLiteral());
            numberLiteral1.Append(new FormatCode("General"));
            numberLiteral1.Append(new PointCount() { Val = new UInt32Value((uint)data.Count) });

            NumberLiteral numberLiteral2 = barChartSeries2.AppendChild<Values>(new Values()).AppendChild<NumberLiteral>(new NumberLiteral());
            numberLiteral2.Append(new FormatCode("General"));
            numberLiteral2.Append(new PointCount() { Val = new UInt32Value((uint)data.Count) });

            // Set values to X and Y axis.
            foreach (GanttData key in data)
            {
                stringLiteral1.AppendChild<StringPoint>(new StringPoint() { Index = new UInt32Value(i) })
                    .AppendChild<NumericValue>(new NumericValue(key.Name));

                stringLiteral2.AppendChild<StringPoint>(new StringPoint() { Index = new UInt32Value(i) })
                    .AppendChild<NumericValue>(new NumericValue(key.Name));

                numberLiteral1.AppendChild<NumericPoint>(new NumericPoint() { Index = new UInt32Value(i) })
                    .Append(new NumericValue(GanttTypeChart.CalculateExcelTime(key.Start).ToString()));

                numberLiteral2.AppendChild<NumericPoint>(new NumericPoint() { Index = new UInt32Value(i) })
                    .Append(new NumericValue(GanttTypeChart.CalculateExcelTime(key.End.Subtract(key.Start)).ToString()));

                i++;
            }
        }

        /// <summary>
        /// Turns TimeSpan to double that excel uses for time values.
        /// </summary>
        public static double CalculateExcelTime(TimeSpan time)
        {
            return time.TotalSeconds / 86400;
        }
    }
}