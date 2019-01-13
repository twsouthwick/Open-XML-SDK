// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Serilog;
using System;
using System.Linq;
using System.Xml.Schema;

namespace IsoSchemaGenerator
{
    public class SchemaGenerator
    {
        private readonly ISchemaBuilder _schemaBuilder;
        private readonly ISchemaCleaner[] _cleaners;
        private readonly ILogger _logger;
        private readonly IClassGenerator _generator;

        public SchemaGenerator(
            ISchemaBuilder schemaBuilder,
            ISchemaCleaner[] cleaners,
            IClassGenerator generator,
            ILogger logger)
        {
            _schemaBuilder = schemaBuilder;
            _cleaners = cleaners;
            _logger = logger;
            _generator = generator;
        }

        public void Run()
        {
            var schemas = _schemaBuilder.BuildSchemaSet();

            foreach (var cleaner in _cleaners)
            {
                _logger.Information("Running cleaner '{Cleaner}'", cleaner);

                foreach (var schema in schemas.Schemas(cleaner.QualifiedName.Namespace.NamespaceName).OfType<XmlSchema>())
                {
                    foreach (var type in schema.SchemaTypes.Values.OfType<XmlSchemaType>())
                    {
                        if (string.Equals(cleaner.QualifiedName.LocalName, type.Name, StringComparison.Ordinal))
                        {
                            _logger.Information("Found matching element '{@Element}' for cleaner '{Cleaner}'", type, cleaner);

                            cleaner.Clean(type);
                        }
                    }
                }
            }

            schemas.Compile();

            _generator.Generate(schemas);
        }
    }
}
