// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace DocumentFormat.OpenXml.Framework.Tests
{
    public class ElementLookupTests
    {
        [Fact]
        public void ExpectedLookupCount()
        {
            var cache = new PackageCache();

            Assert.Equal(84, cache.Lookup.Count);
        }

        [Fact]
        public void KnownType()
        {
            var cache = new PackageCache();

            Assert.IsType<Wordprocessing.Document>(cache.Lookup.Create(23, "document"));
        }

        [Fact]
        public void UnknownType()
        {
            var cache = new PackageCache();

            Assert.Null(cache.Lookup.Create(24, "document"));
        }
    }
}
