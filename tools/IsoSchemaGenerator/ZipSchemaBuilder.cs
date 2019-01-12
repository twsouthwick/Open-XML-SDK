// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Serilog;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace IsoSchemaGenerator
{
    public class ZipSchemaBuilder : ISchemaBuilder
    {
        private const string BaseUri = "schema:///";

        private readonly string _path;
        private readonly ILogger _logger;

        public ZipSchemaBuilder(GeneratorOptions options, ILogger logger)
        {
            _path = options.SchemaPath;
            _logger = logger;
        }

        public XmlSchemaSet BuildSchemaSet()
        {
            using (var fs = File.OpenRead(_path))
            using (var outer = new ZipArchive(fs))
            {
                using (var outerStream = outer.GetEntry("OfficeOpenXML-XMLSchema-Strict.zip").Open())
                using (var archive = new ZipArchive(outerStream))
                {
                    var schemaSet = new XmlSchemaSet
                    {
                        XmlResolver = new ZipXmlResolver(archive, _logger),
                    };

                    foreach (var entry in archive.Entries)
                    {
                        if (entry.Name.EndsWith(".xsd", StringComparison.OrdinalIgnoreCase))
                        {
                            _logger.Information("Adding '{Schema}' to schemas.", entry.Name);

                            schemaSet.Add(null, BaseUri + entry.Name);
                        }
                    }

                    using (var stream = typeof(Program).Assembly.GetManifestResourceStream("IsoSchemaGenerator.AdditionalSchemas.Additional.xsd"))
                    {
                        void Validation(object sender, ValidationEventArgs e)
                        {
                        }

                        var s = XmlSchema.Read(stream, Validation);

                        foreach (var item in s.Items.OfType<XmlSchemaSimpleType>())
                        {
                            foreach (XmlSchema schema in schemaSet.Schemas("http://purl.oclc.org/ooxml/officeDocument/sharedTypes"))
                            {
                                var onOff = schema.SchemaTypes.Values.OfType<XmlSchemaSimpleType>().Single(t => t.Name == "ST_OnOff");
                                onOff.Content = item.Content;
                            }
                        }
                    }

                    schemaSet.Compile();

                    return schemaSet;
                }
            }
        }

        private class ZipXmlResolver : XmlResolver
        {
            private readonly ZipArchive _archive;
            private readonly ILogger _logger;

            public ZipXmlResolver(ZipArchive archive, ILogger logger)
            {
                _archive = archive;
                _logger = logger;
            }

            public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
            {
                _logger.Debug("Resolving '{SchemaUri}' for schema", absoluteUri);
                return _archive.GetEntry(absoluteUri.Segments[1]).Open();
            }
        }
    }
}
