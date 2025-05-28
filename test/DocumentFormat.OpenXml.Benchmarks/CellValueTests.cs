// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Xml;

namespace DocumentFormat.OpenXml.Benchmarks
{
    public class CellValueTests
    {
        [Benchmark]
        public object CreateIntCellInnerText()
        {
            var row = new Row();

            for (int i = 0; i < 10_000; i++)
            {
                row.Append(CellValue.CreateCell(i));
            }

            using var str = new StringWriter();
            using var writer = XmlWriter.Create(str, new XmlWriterSettings()
            {
                ConformanceLevel = ConformanceLevel.Fragment,
            });

            row.WriteTo(writer);

            return str.ToString();
        }

        [Benchmark(Baseline = true)]
        public object CreateOriginalCellInnerTest()
        {
            var row = new Row();

            for (int i = 0; i < 10_000; i++)
            {
                row.Append(new Cell()
                {
                    DataType = CellValues.Number,
                    CellValue = new CellValue(i),
                });
            }

            using var str = new StringWriter();
            using var writer = XmlWriter.Create(str, new XmlWriterSettings()
            {
                ConformanceLevel = ConformanceLevel.Fragment,
            });

            row.WriteTo(writer);

            return str.ToString();
        }
    }
}
