﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Schema;

namespace IsoSchemaGenerator
{
    public interface ISchemaBuilder
    {
        XmlSchemaSet BuildSchemaSet();
    }
}
