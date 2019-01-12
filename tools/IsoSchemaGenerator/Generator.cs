// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace IsoSchemaGenerator
{
    public class Generator
    {
        private readonly ISchemaBuilder _schemaBuilder;

        public Generator(ISchemaBuilder schemaBuilder)
        {
            _schemaBuilder = schemaBuilder;
        }

        public void Run()
        {
            var schemas = _schemaBuilder.BuildSchemaSet();
            schemas.Compile();
        }
    }
}
