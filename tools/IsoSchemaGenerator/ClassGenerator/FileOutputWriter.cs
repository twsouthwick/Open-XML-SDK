// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CodeDom;
using System.IO;
using XmlSchemaClassGenerator;

namespace IsoSchemaGenerator.ClassGenerator
{
    public class FileOutputWriter : OutputWriter
    {
        private readonly string _directory;

        public FileOutputWriter(GeneratorOptions options)
        {
            _directory = options.OutputDirectory;

            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
        }

        public override void Write(CodeNamespace cn)
        {
            var file = Path.Combine(_directory, cn.Name + ".cs");
            var cu = new CodeCompileUnit();

            cu.Namespaces.Add(cn);

            using (var fs = File.OpenWrite(file))
            using (var writer = new StreamWriter(fs))
            {
                Write(writer, cu);
            }
        }
    }
}
