using System;
using System.Text.RegularExpressions;
using PlcGateway.Core;
using PlcGateway.Core.Exceptions;

using static PlcGateway.Drivers.Beckhoff.AdsErrorCode;

namespace PlcGateway.Drivers.Beckhoff
{
    public readonly struct AdsIndexAddress : IEquatable<AdsIndexAddress>
    {
        // 可能我一个地址，对应多个字符串，例如：
        // [0x01, 1]
        // [1, 1]
        // 两个表示同一个地址
        private static readonly Mapper<AdsIndexAddress, string> _mapper = new();

        public uint IndexGroup { get; }

        public uint IndexOffset { get; }

        public AdsIndexAddress(uint indexGroup, uint indexOffset)
        {
            IndexGroup = indexGroup;
            IndexOffset = indexOffset;
        }

        public static implicit operator AdsIndexAddress(string indices)
        {
            if (_mapper.TryGetKey(indices, out var address))
            {
                return address;
            }

            var (indexGroup, indexOffset) = ParseIndexes(indices);
            var newAddress = new AdsIndexAddress(indexGroup, indexOffset);

            _mapper.Add(newAddress, indices);

            return newAddress;
        }

        public static implicit operator string(AdsIndexAddress address)
        {
            return $"[{address.IndexGroup},{address.IndexOffset}]";
        }

        public override string ToString()
        {
            return (string)this;
        }

        private static (uint indexGroup, uint indexOffset) ParseIndexes(string indices)
        {
            if (string.IsNullOrWhiteSpace(indices))
            {
                throw new BusinessException(
                    code: ADS_INVALID_INDICES_FORMAT,
                    message: "Indices string cannot be null or empty",
                    details: "Provide indices in format: [indexGroup, indexOffset]. Example: [0x4020, 0] for M memory area"
                );
            }

            // Strict pattern matching for [int, int] format
            string pattern = @"^\[\s*(0x[0-9a-fA-F]+|\d+)\s*,\s*(0x[0-9a-fA-F]+|\d+)\s*\]$";
            var match = Regex.Match(indices, pattern);

            if (!match.Success)
            {
                throw new BusinessException(
                    code: ADS_INVALID_INDICES_FORMAT,
                    message: $"Invalid indices format. Expected: [indexGroup, indexOffset]",
                    details: $"Provided indices: '{indices}'. Valid examples: [0x4020, 0] (M area), [0x4021, 10] (I area), [0x4022, 20] (Q area), [0x4040, 100] (DB1 area)"
                );
            }

            try
            {
                // Parse the matched groups (support decimal and hex)
                uint indexGroup = match.Groups[1].Value.StartsWith("0x")
                    ? Convert.ToUInt32(match.Groups[1].Value.Substring(2), 16)
                    : uint.Parse(match.Groups[1].Value);

                uint indexOffset = match.Groups[2].Value.StartsWith("0x")
                    ? Convert.ToUInt32(match.Groups[2].Value.Substring(2), 16)
                    : uint.Parse(match.Groups[2].Value);

                return (indexGroup, indexOffset);
            }
            catch (FormatException ex)
            {
                throw new BusinessException(
                    code: ADS_INVALID_INDEX_NUMBER_FORMAT,
                    message: $"Failed to parse numbers in indices string",
                    details: $"Indices: '{indices}', inner exception: {ex.Message}. Use decimal (e.g., 123) or hex (e.g., 0x7B) format"
                );
            }
            catch (OverflowException ex)
            {
                throw new BusinessException(
                    code: ADS_INDEX_NUMBER_OVERFLOW,
                    message: $"Number in indices is out of valid range",
                    details: $"Indices: '{indices}', valid range: 0 to {uint.MaxValue}, inner exception: {ex.Message}"
                );
            }
        }

        public bool Equals(AdsIndexAddress other)
        {
            return this.IndexGroup == other.IndexGroup
                && this.IndexOffset == other.IndexOffset;
        }

        public override int GetHashCode()
        {
#if NETSTANDARD2_1
            return System.HashCode.Combine(IndexGroup, IndexOffset);
#elif NETSTANDARD2_0
            return PlcGateway.Core.HashCode.Combine(IndexGroup, IndexOffset);
#endif
        }
    }
}
