// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Xml.Schema;

namespace IsoSchemaGenerator
{
    public class FolderSchemaGenerator : ISchemaBuilder
    {
        private readonly string _path;

        public FolderSchemaGenerator(string path)
        {
            _path = path;
        }

        public XmlSchemaSet BuildSchemaSet()
        {
            var schemas = new XmlSchemaSet();

            schemas.Add("http://www.w3.org/XML/1998/namespace", "http://www.w3.org/2001/xml.xsd");

            foreach (var file in Directory.EnumerateFiles(_path, "*.xsd"))
            {
                schemas.Add(null, file);
            }

            schemas.Compile();

            return schemas;
        }
    }
}
