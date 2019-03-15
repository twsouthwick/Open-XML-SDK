// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Serilog;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;

namespace IsoSchemaGenerator.Cleaners
{
    public class PitchFamilyCleaner : ISchemaCleaner
    {
        private readonly ILogger _logger;

        public PitchFamilyCleaner(ILogger logger)
        {
            _logger = logger;
        }

        public XName QualifiedName => XName.Get("CT_TextFont", "http://purl.oclc.org/ooxml/drawingml/main");

        public void Clean(XmlSchemaType type)
        {
            if (type is XmlSchemaComplexType ct)
            {
                const string DefaultValue = "00";
                var attribute = ct.Attributes.OfType<XmlSchemaAttribute>().First(t => t.Name == "pitchFamily");

                _logger.Information("Changing default value of {Attribute} from {PreviousDefault} to {DefaultValue}", attribute, attribute.DefaultValue, DefaultValue);
                attribute.DefaultValue = DefaultValue;
            }
            else
            {
                _logger.Error("{Type} must be a simple type", type);
                throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}
