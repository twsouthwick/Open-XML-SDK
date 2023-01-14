// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DocumentFormat.OpenXml.Features;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Packaging;

namespace DocumentFormat.OpenXml.Packaging.Builder;

internal static class PackageCachingExtensions
{
    internal static OpenXmlPackage EnableCaching(this OpenXmlPackage package)
    {
        var feature = package.Features.GetRequired<IPackageFeature>();

        if (!feature.Capabilities.HasFlagFast(PackageCapabilities.Cached))
        {
            package.Features.Set<IPackageFeature>(new CachingPackage(feature));
        }

        return package;
    }

    private sealed class CachingPackage : DelegatePackage
    {
        private readonly Dictionary<Uri, CachingPart> _parts = new();

        public CachingPackage(IPackageFeature package)
            : base(package)
        {
        }

        private IPackagePart GetOrCreatePart(IPackagePart part)
        {
            if (_parts.TryGetValue(part.Uri, out var existing))
            {
                Debug.Assert(existing.OriginalPart == part);
                return existing;
            }

            var newItem = new CachingPart(this, part);
            _parts[part.Uri] = newItem;
            return newItem;
        }

        public override IPackagePart CreatePart(Uri partUri, string contentType, CompressionOption compressionOption)
            => GetOrCreatePart(base.CreatePart(partUri, contentType, compressionOption));

        public override void DeletePart(Uri uri)
        {
            _parts.Remove(uri);
            base.DeletePart(uri);
        }

        public override IPackagePart GetPart(Uri uriTarget)
            => GetOrCreatePart(base.GetPart(uriTarget));

        public override IEnumerable<IPackagePart> GetParts()
        {
            foreach (var inner in base.GetParts())
            {
                yield return GetOrCreatePart(inner);
            }
        }
    }

    private sealed class CachingPart : DelegatePackagePart
    {
        public CachingPart(IPackage package, IPackagePart part)
            : base(package, part)
        {
        }
    }
}
