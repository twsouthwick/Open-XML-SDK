// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Linq;
using System.Xml.Schema;

namespace IsoSchemaGenerator
{
    public interface ISchemaCleaner
    {
        XName QualifiedName { get; }

        void Clean(XmlSchemaType type);
    }
}
