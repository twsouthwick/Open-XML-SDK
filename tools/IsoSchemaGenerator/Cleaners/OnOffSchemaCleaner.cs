// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Serilog;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;

namespace IsoSchemaGenerator.Cleaners
{
    public class OnOffSchemaCleaner : ISchemaCleaner
    {
        private readonly ILogger _logger;

        public OnOffSchemaCleaner(ILogger logger)
        {
            _logger = logger;
        }

        public XName QualifiedName => XName.Get("ST_OnOff", "http://purl.oclc.org/ooxml/officeDocument/sharedTypes");

        public void Clean(XmlSchemaType type)
        {
            if (type is XmlSchemaSimpleType simpleType)
            {
                using (var stream = typeof(Program).Assembly.GetManifestResourceStream("IsoSchemaGenerator.AdditionalSchemas.ST_OnOff.xsd"))
                {
                    void Validation(object sender, ValidationEventArgs e)
                    {
                    }

                    var updated = XmlSchema.Read(stream, Validation).Items.OfType<XmlSchemaSimpleType>().Single();

                    simpleType.Content = updated.Content;
                }
            }
            else
            {
                _logger.Error("{Type} must be a simple type", type);
                throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}
