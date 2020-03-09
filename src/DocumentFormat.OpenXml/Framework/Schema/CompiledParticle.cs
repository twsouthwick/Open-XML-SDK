// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DocumentFormat.OpenXml.Framework.Schema;
using DocumentFormat.OpenXml.Validation.Schema;
using System;
using System.Collections.Generic;

namespace DocumentFormat.OpenXml.Framework
{
    internal class CompiledParticle : IComparer<OpenXmlElement>
    {
        private readonly PackageCache _cache = PackageCache.Cache;

        private readonly Lazy<LookupItem[]> _lookup;

        public CompiledParticle(ParticleConstraint particle)
        {
            Particle = particle;
            _lookup = new Lazy<LookupItem[]>(() => ParticleCompiler.Compile(Particle, _cache), true);
        }

        public ReadOnlyArray<LookupItem> Lookup => _lookup.Value;

        public ParticleConstraint Particle { get; }

        public ParticlePath Find<TElement>()
            where TElement : OpenXmlElement
            => GetPath(typeof(TElement));

        public ParticlePath Find(object obj)
            => GetPath(obj?.GetType());

        public OpenXmlElement CreateElement(byte nsId, string name)
        {
            foreach (var l in Lookup)
            {
                if (l.Data.Info.Schema.NamespaceId == nsId && l.Data.Info.Schema.Tag == name)
                {
                    var factory = _cache.GetFactory<OpenXmlElement>(l.Type);

                    return factory();
                }
            }

            return null;
        }

        private ParticlePath GetPath(Type type)
        {
            if (type is null)
            {
                return null;
            }

            foreach (var (t, p) in Lookup)
            {
                if (t == type)
                {
                    return p;
                }
            }

            return null;
        }

        public int Compare(OpenXmlElement x, OpenXmlElement y)
        {
            var xPath = GetPath(x?.GetType());
            var yPath = GetPath(y?.GetType());

            if (xPath is null && yPath is null)
            {
                return 0;
            }

            if (xPath is null)
            {
                return -1;
            }

            if (yPath is null)
            {
                return 1;
            }

            return xPath.CompareTo(yPath);
        }
    }
}
