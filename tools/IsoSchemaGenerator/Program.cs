// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace IsoSchemaGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var options = new GeneratorOptions
            {
                InputPath = args[0],
            };

            var generator = new ZipSchemaGenerator(options.InputPath);

            var schemas = generator.BuildSchemaSet();
            schemas.Compile();
        }
    }
}
