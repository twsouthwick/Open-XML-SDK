// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Schema;
using XmlSchemaClassGenerator;

namespace IsoSchemaGenerator.ClassGenerator
{
    public class GeneratorClassGenerator : IClassGenerator
    {
        private readonly Generator _generator;

        public GeneratorClassGenerator(Generator generator)
        {
            _generator = generator;
        }

        public void Generate(XmlSchemaSet schemaSet)
        {
            _generator.Generate(schemaSet);
        }
    }
}
