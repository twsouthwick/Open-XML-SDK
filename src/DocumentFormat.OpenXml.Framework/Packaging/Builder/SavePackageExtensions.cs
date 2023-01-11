// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DocumentFormat.OpenXml.Features;
using System;

namespace DocumentFormat.OpenXml.Packaging.Builder;

internal static class SavePackageExtensions
{
    internal static IPackageFeature EnableSavePackage(this IPackageFeature feature)
    {
        var capabilities = feature.Capabilities;

        if (capabilities.HasFlagFast(PackageCapabilities.Save))
        {
            return feature;
        }
        else if (capabilities.HasFlagFast(PackageCapabilities.Reload))
        {
            return new SaveablePackage(feature);
        }
        else
        {
            return feature;
        }
    }

    private sealed class SaveablePackage : DelegatePackage
    {
        public SaveablePackage(IPackageFeature package)
            : base(package)
        {
        }

        public override PackageCapabilities Capabilities => base.Capabilities & PackageCapabilities.Save;

        public override void Save() => Feature.Reload();
    }
}
