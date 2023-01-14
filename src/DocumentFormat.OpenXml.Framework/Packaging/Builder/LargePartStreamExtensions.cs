// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DocumentFormat.OpenXml.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;

namespace DocumentFormat.OpenXml.Packaging.Builder;

internal static class LargePartStreamExtensions
{
    internal static OpenXmlPackage EnableLargePartStreams(this OpenXmlPackage package)
    {
        var feature = package.Features.GetRequired<IPackageFeature>();

        if (!feature.Capabilities.HasFlagFast(PackageCapabilities.LargePartStreams))
        {
            return package;
        }

        // Required to support feature
        if (!feature.Capabilities.HasFlagFast(PackageCapabilities.Reload))
        {
            return package;
        }

        var newFeature = new TemporaryStreamPackage(feature);

        package.Features.Set<IPackageFeature>(newFeature);
        package.OnClose(newFeature.Dispose);

        return package;
    }

    private sealed class TemporaryStreamPackage : DelegatePackage, IDisposable
    {
        private Dictionary<Uri, string>? _streams;

        public TemporaryStreamPackage(IPackageFeature package)
            : base(package)
        {
        }

        public override PackageCapabilities Capabilities => base.Capabilities & PackageCapabilities.LargePartStreams;

        public Stream GetStream(Uri uri, FileMode mode, FileAccess access, IPackagePart originalPart)
        {
            if (_streams is not null && _streams.TryGetValue(uri, out var existingPath))
            {
                return File.Open(existingPath, mode, access);
            }

            if (_streams is null)
            {
                _streams = new();
            }

            var path = Path.GetTempFileName();
            _streams[uri] = path;
            var file = File.Open(path, mode, access);

            using var original = originalPart.GetStream(mode, access);
            original.CopyTo(file);
            file.Position = 0;

            return file;
        }

        public override void Save()
        {
            base.Save();

            if (_streams is { } streams)
            {
                Feature.Reload(access: FileAccess.Write);

                WriteStreams(streams);
                _streams = null;

                base.Save();

                Feature.Reload();
            }
        }

        private void WriteStreams(Dictionary<Uri, string> files)
        {
            foreach (var stream in files)
            {
                using var file = File.OpenRead(stream.Value);
                using var partStream = base.GetPart(stream.Key).GetStream();

                file.CopyTo(partStream);
            }
        }

        private IPackagePart Wrap(IPackagePart part) => new TemporaryStreamPart(this, part);

        public override IPackagePart CreatePart(Uri partUri, string contentType, CompressionOption compressionOption)
            => Wrap(base.CreatePart(partUri, contentType, compressionOption));

        public override void DeletePart(Uri uri)
        {
            if (_streams is not null && _streams.TryGetValue(uri, out var existingPath))
            {
                File.Delete(existingPath);
            }

            base.DeletePart(uri);
        }

        public override IPackagePart GetPart(Uri uriTarget)
            => Wrap(base.GetPart(uriTarget));

        public override IEnumerable<IPackagePart> GetParts()
        {
            foreach (var inner in base.GetParts())
            {
                yield return Wrap(inner);
            }
        }

        public void Dispose()
        {
            if (_streams is not null)
            {
                foreach (var item in _streams.Values)
                {
                    File.Delete(item);
                }
            }
        }
    }

    private sealed class TemporaryStreamPart : DelegatePackagePart
    {
        private readonly TemporaryStreamPackage _package;

        public TemporaryStreamPart(TemporaryStreamPackage package, IPackagePart part)
            : base(package, part)
        {
            _package = package;
        }

        public override Stream GetStream(FileMode open, FileAccess write)
        {
            return _package.GetStream(Uri, open, write, OriginalPart);
        }
    }
}
