// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DocumentFormat.OpenXml.Packaging
{
    /// <summary>
    /// Represents a (RelationshipId, OpenXmlPart) pair.
    /// </summary>
    public readonly struct IdPartPair : IEquatable<IdPartPair>
    {
        /// <summary>
        /// Gets the relationship ID in the pair.
        /// </summary>
        public string RelationshipId { get; }

        /// <summary>
        /// Gets the OpenXmlPart in the pair.
        /// </summary>
        public OpenXmlPart OpenXmlPart { get; }

        /// <summary>
        /// Initializes a new instance of the IdPartPair with the specified id and part.
        /// </summary>
        /// <param name="id">The relationship ID.</param>
        /// <param name="part">The OpenXmlPart.</param>
        public IdPartPair(string id, OpenXmlPart part)
        {
            RelationshipId = id;
            OpenXmlPart = part;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is IdPartPair idPart)
            {
                return string.Equals(idPart.RelationshipId, RelationshipId, StringComparison.Ordinal)
                    && Equals(idPart.OpenXmlPart, OpenXmlPart);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            const int Multiplier = 17;
            var hashCode = 23;

            unchecked
            {
                hashCode = hashCode * Multiplier + StringComparer.Ordinal.GetHashCode(RelationshipId);
                hashCode = hashCode * Multiplier + (OpenXmlPart?.GetHashCode() ?? 0);

                return hashCode;
            }
        }

        /// <summary>
        /// Override the equals operator
        /// </summary>
        public static bool operator ==(IdPartPair left, IdPartPair right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Override the not equals operator
        /// </summary>
        public static bool operator !=(IdPartPair left, IdPartPair right)
        {
            return !(left == right);
        }

        public bool Equals(IdPartPair other)
        {
            return this == other;
        }
    }
}
