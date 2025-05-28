// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Xml;

namespace DocumentFormat.OpenXml.Spreadsheet
{
    /// <summary>
    /// This provides additional constructors than in the generated files.
    /// </summary>
    public partial class CellValue
    {
        private const string DateTimeFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff";
        private const string DateTimeOffsetFormatString = DateTimeFormatString + "zzz";

        /// <summary>
        /// Create a cell value with an integer value.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Cell CreateCell(int i) => new Cell()
        {
            DataType = CellValues.Number,
            CellValue = new CellValueInt(i),
        };

        /// <summary>
        /// Instantiates an instance of <see cref="CellValue"/> for a <see cref="DateTime"/>. Dates must
        /// be in ISO 8601 format, which this constructor ensures
        /// </summary>
        /// <param name="dateTime">DateTime for cell</param>
        public CellValue(DateTime dateTime)
            : this(ToCellFormat(dateTime))
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="CellValue"/> for a <see cref="DateTimeOffset"/>. Dates must
        /// be in ISO 8601 format, which this constructor ensures
        /// </summary>
        /// <param name="dateTimeOffset">DateTime for cell</param>
        public CellValue(DateTimeOffset dateTimeOffset)
            : this(ToCellFormat(dateTimeOffset))
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="CellValue"/> for a <see cref="bool"/>.
        /// </summary>
        /// <param name="value">Boolean value</param>
        public CellValue(bool value)
            : this(ToCellFormat(value))
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="CellValue"/> for a <see cref="double"/>.
        /// </summary>
        /// <param name="value">Number.</param>
        public CellValue(double value)
            : this(ToCellFormat(value))
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="CellValue"/> for a <see cref="int"/>.
        /// </summary>
        /// <param name="value">Number.</param>
        public CellValue(int value)
            : this(ToCellFormat(value))
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="CellValue"/> for a <see cref="decimal"/>.
        /// </summary>
        /// <param name="value">Number.</param>
        public CellValue(decimal value)
            : this(ToCellFormat(value))
        {
        }

        /// <summary>
        /// Attempts to parse cell value to retrieve a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dt">The result if successful.</param>
        /// <returns>Success or failure</returns>
        public bool TryGetDateTime(out DateTime dt) => TryGetDateTime(InnerText, out dt);

        private static bool TryGetDateTime(string input, out DateTime dt)
        {
            return DateTime.TryParseExact(input, DateTimeFormatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
        }

        /// <summary>
        /// Attempts to parse cell value to retrieve a <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="dt">The result if successful.</param>
        /// <returns>Success or failure</returns>
        public bool TryGetDateTimeOffset(out DateTimeOffset dt) => TryGetDateTimeOffset(InnerText, out dt);

        private static bool TryGetDateTimeOffset(string input, out DateTimeOffset dt)
        {
            return DateTimeOffset.TryParseExact(input, DateTimeOffsetFormatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
        }

        /// <summary>
        /// Attempts to parse cell value to retrieve a <see cref="double"/>.
        /// </summary>
        /// <param name="dbl">The result if successful.</param>
        /// <returns>Success or failure</returns>
        public bool TryGetDouble(out double dbl) => TryGetDouble(InnerText, out dbl);

        private static bool TryGetDouble(string input, out double dbl)
        {
            return double.TryParse(input, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out dbl);
        }

        /// <summary>
        /// Attempts to parse cell value to retrieve a <see cref="int"/>.
        /// </summary>
        /// <param name="value">The result if successful.</param>
        /// <returns>Success or failure</returns>
        public bool TryGetInt(out int value) => TryGetInt(InnerText, out value);

        private static bool TryGetInt(string input, out int value)
        {
            return int.TryParse(input, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out value);
        }

        /// <summary>
        /// Attempts to parse cell value to retrieve a <see cref="decimal"/>.
        /// </summary>
        /// <param name="value">The result if successful.</param>
        /// <returns>Success or failure</returns>
        public bool TryGetDecimal(out decimal value) => TryGetDecimal(InnerText, out value);

        private static bool TryGetDecimal(string input, out decimal value)
        {
            return decimal.TryParse(input, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out value);
        }

        private static string ToCellFormat(int input) => input.ToString(CultureInfo.InvariantCulture);

        private static string ToCellFormat(double input) => input.ToString(CultureInfo.InvariantCulture);

        private static string ToCellFormat(decimal input) => input.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Attempts to parse cell value to retrieve a <see cref="bool"/>.
        /// </summary>
        /// <param name="value">The result if successful.</param>
        /// <returns>Success or failure</returns>
        public bool TryGetBoolean(out bool value) => TryGetBoolean(InnerText, out value);

        private static bool TryGetBoolean(string input, out bool value)
        {
            switch (input)
            {
                case "0":
                case "false":
                    value = false;
                    return true;
                case "1":
                case "true":
                    value = true;
                    return true;
                default:
                    value = false;
                    return false;
            }
        }

        private static string ToCellFormat(DateTime dateTime)
            => dateTime.ToString(DateTimeFormatString, CultureInfo.InvariantCulture);

        private static string ToCellFormat(DateTimeOffset dateTime)
            => dateTime.ToString(DateTimeOffsetFormatString, CultureInfo.InvariantCulture);

        private static string ToCellFormat(bool input)
            => input ? "true" : "false";

        private sealed class CellValueInt : CellValue
        {
            private int _value;

            public CellValueInt(int value)
            {
                _value = value;
            }

            public override string Text
            {
                get => ToCellFormat(_value);
                set
                {
                    if (!TryGetInt(value, out _value))
                    {
                        throw new InvalidOperationException("Could not parse object");
                    }
                }
            }

            internal override void WriteContentTo(XmlWriter w)
            {
                w.WriteValue(_value);
            }
        }
    }
}
