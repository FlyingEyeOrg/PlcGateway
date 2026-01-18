using PlcGateway.Core.Exceptions.YourNamespace.Exceptions;
using System;
using System.Text;
using System.Text.RegularExpressions;
using TwinCAT.Ads;

namespace PlcGateway.Drivers.Beckhoff
{
    internal class BeckhoffDriverRawImpl : BeckhoffDriverImplBase
    {
        public Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        public BeckhoffDriverRawImpl(AmsNetId amsNetId, int port) : base(amsNetId, port)
        {
        }

        public void Write(string indices, sbyte value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            var code = this.AdsClient.TryWrite(indexGroup, indexOffset, BitConverter.GetBytes(value));

            if (code != AdsErrorCode.NoError)
            {
                throw new BusinessException("", "", "");
            }
        }

        public void Write(string indices, byte value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.Write(indexGroup, indexOffset, BitConverter.GetBytes(value));
        }

        public void Write(string indices, bool value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.TryWrite(indexGroup, indexOffset, BitConverter.GetBytes(value));
        }

        public void Write(string indices, short value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.TryWrite(indexGroup, indexOffset, BitConverter.GetBytes(value));
        }

        public void Write(string indices, ushort value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.TryWrite(indexGroup, indexOffset, BitConverter.GetBytes(value));
        }

        public void Write(string indices, int value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.TryWrite(indexGroup, indexOffset, BitConverter.GetBytes(value));
        }

        public void Write(string indices, uint value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.TryWrite(indexGroup, indexOffset, BitConverter.GetBytes(value));
        }

        public void Write(string indices, long value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.TryWrite(indexGroup, indexOffset, BitConverter.GetBytes(value));
        }

        public void Write(string indices, ulong value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.TryWrite(indexGroup, indexOffset, BitConverter.GetBytes(value));
        }

        public void Write(string indices, float value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.TryWrite(indexGroup, indexOffset, BitConverter.GetBytes(value));
        }

        public void Write(string indices, double value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.TryWrite(indexGroup, indexOffset, BitConverter.GetBytes(value));
        }

        public void Write(string indices, string value)
        {
            var (indexGroup, indexOffset) = ParseIndexes(indices);
            this.AdsClient.TryWrite(indexGroup, indexOffset, DefaultEncoding.GetBytes(value));
        }

        public void Read(string address)
        {
            var (indexGroup, indexOffset) = ParseIndexes(address);
            var buffer = new byte[4];
            this.AdsClient.ReadAnyString(indexGroup, indexOffset, 0, this.DefaultEncoding); ;
        }

        public (uint indexGroup, uint indexOffset) ParseIndexes(string indices)
        {
            if (string.IsNullOrWhiteSpace(indices))
            {
                throw new BusinessException("indices string cannot be null or empty");
            }

            // Strict pattern matching for [int, int] format
            string pattern = @"^\[\s*(-?\d+)\s*,\s*(-?\d+)\s*\]$";
            var match = Regex.Match(indices, pattern);

            if (!match.Success)
            {
                throw new BusinessException($"Invalid format. Expected format: [indexGroup, indexOffset] but got: {indices}");
            }

            try
            {
                // Parse the matched groups
                uint indexGroup = uint.Parse(match.Groups[1].Value);
                uint indexOffset = uint.Parse(match.Groups[2].Value);

                return (indexGroup, indexOffset);
            }
            catch (FormatException ex)
            {
                throw new BusinessException($"Failed to parse numbers: {ex.Message}");
            }
            catch (OverflowException ex)
            {
                throw new BusinessException($"Number is out of integer range: {ex.Message}");
            }
        }
    }
}
