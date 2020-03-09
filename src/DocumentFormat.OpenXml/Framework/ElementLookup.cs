// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace DocumentFormat.OpenXml.Framework
{
    internal class ElementLookup
    {
        private readonly Dictionary<ChildLookup, Func<OpenXmlElement>> _activatorLookup;

        private ElementLookup(Dictionary<ChildLookup, Func<OpenXmlElement>> activators)
        {
            _activatorLookup = activators;
        }

        public static ElementLookup Create(PackageCache cache)
        {
            var activatorLookup = new Dictionary<ChildLookup, Func<OpenXmlElement>>();

            foreach (var child in GetAllRootElements())
            {
                var schema = child.GetTypeInfo().GetCustomAttribute<SchemaAttrAttribute>();

                if (schema is object)
                {
                    var info = new ChildLookup(schema.NamespaceId, schema.Tag);

                    activatorLookup.Add(info, cache.GetFactory<OpenXmlElement>(child));
                }
            }

            return new ElementLookup(activatorLookup);
        }

        public int Count => _activatorLookup.Count;

        public OpenXmlElement Create(byte id, string name)
        {
            if (_activatorLookup.TryGetValue(new ChildLookup(id, name), out var activator))
            {
                return activator();
            }

            return null;
        }

        private static IEnumerable<Type> GetAllRootElements()
        {
            var type = typeof(OpenXmlPartRootElement);
            var types = type.GetTypeInfo().Assembly.GetTypes();

            foreach (var elementType in types)
            {
                if (!elementType.GetTypeInfo().IsAbstract && type.GetTypeInfo().IsAssignableFrom(elementType.GetTypeInfo()))
                {
                    yield return elementType;
                }
            }
        }

        private readonly struct ChildLookup
        {
            public ChildLookup(byte ns, string name)
            {
                Namespace = ns;
                Name = name;
            }

            public byte Namespace { get; }

            public string Name { get; }
        }
    }
}
