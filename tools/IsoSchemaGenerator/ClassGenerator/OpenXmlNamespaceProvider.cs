// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using XmlSchemaClassGenerator;

namespace IsoSchemaGenerator.ClassGenerator
{
    public class OpenXmlNamespaceProvider : NamespaceProvider
    {
        protected override string OnGenerateNamespace(NamespaceKey key)
        {
            var uri = new Uri(key.XmlSchemaNamespace);

            return uri.AbsolutePath.Replace("/", ".").Substring(1);
        }
    }
}
