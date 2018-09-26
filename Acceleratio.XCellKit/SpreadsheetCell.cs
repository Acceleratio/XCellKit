﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Acceleratio.XCellKit
{
    public enum SpreadsheetDataTypeEnum
    {
        String,
        Number,
        DateTime,
        Other
    }
    public class SpreadsheetCell
    {
        // ovo je izvuceno u static jer dobivamo masivna ubrzanja
        // kad se svaki put nova instanca dodjeljuje u  Space = SpaceProcessingModeValues.Preserve
        // stvara se novi objekt ovog dolje tipa, a da bi se dobio innertext mora se iz enum descriptiona preko atributa procitati text vrijednost
        // ako imamo 500 000 redaka to osjetno usporava, ovako ce se samo jednom to desiti
        private static EnumValue<SpaceProcessingModeValues> PreserveSpaceEnumValue = SpaceProcessingModeValues.Preserve;

        private static readonly Cell openXmlCellElementForWriting = new Cell();

        private object _value;

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                WrapText = _value != null && (_value.ToString().Contains("\n") || _value.ToString().Length > 200);
            }
        }

        public System.Drawing.Font Font { get; set; }
        public System.Drawing.Color? BackgroundColor { get; set; }
        public System.Drawing.Color? ForegroundColor { get; set; }
        public HorizontalAligment? Alignment { get; set; }
        public VerticalAlignment? VerticalAlignment { get; set; }
        public int Indent { get; set; }
        public SpreadsheetDataTypeEnum SpreadsheetDataType { get; set; }
        /// <summary>
        /// Used to set an image in the cell based on the imageindex within the collection of images that were provided to the sheet.DrawingsManager.SetImages function
        /// IMPORTANT: Do not use cell images if you expect a high number of rows in the document
        /// this will kill the excel performance if each row has an image when dealing with > 100 000 rows
        /// </summary>
        public int ImageIndex { get; set; }
        public double ImageScaleFactor { get; set; }
        public System.Drawing.Size? MergedCellsRange { get; set; }
        public bool ParticipatesInAutoWidthColumnCalculation { get; set; }
        public bool WrapText { get; private set; }

        public SpreadsheetCell()
        {
            ParticipatesInAutoWidthColumnCalculation = true;
            VerticalAlignment = XCellKit.VerticalAlignment.Center;
            Indent = 0;
            SpreadsheetDataType = SpreadsheetDataTypeEnum.String;
        }

        public virtual void WriteCell(OpenXmlWriter writer, int columnIndex, int rowIndex, SpreadsheetStylesManager stylesManager, SpreadsheetHyperlinkManager hyperlinkManager)
        {
            if (Value == null)
            {
                return;
            }
            var openXmlAtts = new List<OpenXmlAttribute>();
            var columnLetter = SpreadsheetHelper.ExcelColumnFromNumber(columnIndex);
            var position = string.Format("{0}{1}", columnLetter, rowIndex);
            var positionAtt = new OpenXmlAttribute("r", null, position);
            openXmlAtts.Add(positionAtt);

            var styleAtt = getStyleAttribute(stylesManager);
            if (styleAtt.HasValue)
            {
                openXmlAtts.Add(styleAtt.Value);
            }

            var sValue = Value.ToString();
          
            if (SpreadsheetDataType == SpreadsheetDataTypeEnum.Number)
            {
                double numberValue = 0;
                if (double.TryParse(sValue, out numberValue))
                {
                    var typeAtt = new OpenXmlAttribute("t", null, "n");
                    openXmlAtts.Add(typeAtt);
                    writer.WriteStartElement(new Cell(), openXmlAtts);
                    writer.WriteElement(new CellValue(numberValue.ToString(CultureInfo.InvariantCulture)));
                    writer.WriteEndElement();
                }
            }
            else if (SpreadsheetDataType == SpreadsheetDataTypeEnum.String)
            {
                // Total number of characters that a cell can contain is 32,767.
                if (sValue.Length > 32767)
                {
                    sValue = sValue.Substring(0, 32767);
                }
                var typeAtt = new OpenXmlAttribute("t", null, "inlineStr");
                openXmlAtts.Add(typeAtt);
                writer.WriteStartElement(new Cell(), openXmlAtts);                
                writer.WriteStartElement(new InlineString());
                writer.WriteElement(new Text(sValue) { Space = PreserveSpaceEnumValue });
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            else if (SpreadsheetDataType == SpreadsheetDataTypeEnum.DateTime)
            {
                var dateTime = DateTime.MinValue;
                if (DateTime.TryParse(sValue, out dateTime))
                {
                    writer.WriteStartElement(new Cell(), openXmlAtts);
                    writer.WriteElement(new CellValue(dateTime.ToOADate().ToString(CultureInfo.InvariantCulture)));
                    writer.WriteEndElement();
                }
            }
            else if (SpreadsheetDataType == SpreadsheetDataTypeEnum.Other)
            {
                var typeAttribute = new OpenXmlAttribute("t", null, "str");
                openXmlAtts.Add(typeAttribute);
                writer.WriteStartElement(new Cell(), openXmlAtts);
                writer.WriteElement(new CellValue(sValue));
                writer.WriteEndElement();
            }
        }

        protected virtual OpenXmlAttribute? getStyleAttribute(SpreadsheetStylesManager stylesManager)
        {
            OpenXmlAttribute? styleAtt = null;
            if (Font != null || BackgroundColor != null || ForegroundColor != null || Alignment != null || VerticalAlignment != null || Indent != 0 || SpreadsheetDataType == SpreadsheetDataTypeEnum.DateTime || WrapText)
            {
                var spredsheetStyle = new SpreadsheetStyle()
                {
                    Font = Font,
                    BackgroundColor = BackgroundColor,
                    ForegroundColor = ForegroundColor,
                    Alignment = Alignment.HasValue ? SpreadsheetHelper.GetHorizontalAlignmentValue(Alignment.Value) : (HorizontalAlignmentValues?)null,
                    VerticalAlignment = VerticalAlignment.HasValue ? SpreadsheetHelper.GetVerticalAlignmentValues(VerticalAlignment.Value) : (VerticalAlignmentValues?)null,
                    Indent = Indent,
                    IsDate = SpreadsheetDataType == SpreadsheetDataTypeEnum.DateTime,
                    WrapText = WrapText
                };
                styleAtt = new OpenXmlAttribute("s", null, ((UInt32)stylesManager.GetStyleIndex(spredsheetStyle)).ToString());
            }
            return styleAtt;
        }
    }
}
