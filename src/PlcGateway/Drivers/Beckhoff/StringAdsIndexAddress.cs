using PlcGateway.Core;
using PlcGateway.Drivers.Beckhoff.Exceptions;
using System;
using System.Text.RegularExpressions;

using static PlcGateway.Drivers.Beckhoff.AdsErrorCode;

namespace PlcGateway.Drivers.Beckhoff
{
    /// <summary>
    /// 数据格式：[IndexGroup, IndexOffset, DataLength]
    /// 支持十进制和十六进制（0x前缀）数字
    /// </summary>
    public readonly struct StringAdsIndexAddress
    {
        // 可能我一个地址，对应多个字符串，例如：
        // [0x01, 1, 2]
        // [1, 1, 0x02]
        // 两个表示同一个地址
        private static readonly Mapper<StringAdsIndexAddress, string> _mapper = new();

        public uint IndexGroup { get; }

        public uint IndexOffset { get; }

        public int DataLength { get; }

        /// <summary>
        /// 初始化StringRawAddress结构体
        /// </summary>
        /// <param name="indexGroup">索引组（内存区域标识符）</param>
        /// <param name="indexOffset">内存区域内的字节偏移量</param>
        /// <param name="dataLength">字符串数据长度（必须大于0）</param>
        /// <exception cref="BeckhoffException">当参数无效时抛出</exception>
        public StringAdsIndexAddress(uint indexGroup, uint indexOffset, int dataLength)
        {
            if (dataLength <= 0)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_STRING_LENGTH,
                    message: "Data length must be greater than zero",
                    details: $"Data length must be a positive integer. Provided value: {dataLength}"
                );
            }

            const uint MAX_ALLOWED_LENGTH = 65535; // 合理的PLC字符串长度限制
            if (dataLength > MAX_ALLOWED_LENGTH)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_STRING_LENGTH,
                    message: $"Data length exceeds maximum allowed limit of {MAX_ALLOWED_LENGTH}",
                    details: $"Provided data length: {dataLength}. Maximum allowed: {MAX_ALLOWED_LENGTH}"
                );
            }

            IndexGroup = indexGroup;
            IndexOffset = indexOffset;
            DataLength = dataLength;
        }

        /// <summary>
        /// 从字符串隐式转换到StringRawAddress
        /// 支持格式：[IndexGroup, IndexOffset, DataLength]
        /// </summary>
        public static implicit operator StringAdsIndexAddress(string indices)
        {
            if (_mapper.TryGetKey(indices, out var address))
            {
                return address;
            }

            var (indexGroup, indexOffset, length) = ParseIndexes(indices);
            var newAddress = new StringAdsIndexAddress(indexGroup, indexOffset, length);

            _mapper.Add(newAddress, indices);

            return newAddress;
        }

        /// <summary>
        /// 从StringRawAddress隐式转换到字符串
        /// 格式：[IndexGroup, IndexOffset, DataLength]
        /// </summary>
        public static implicit operator string(StringAdsIndexAddress address)
        {
            return $"[{address.IndexGroup},{address.IndexOffset},{address.DataLength}]";
        }

        public override string ToString()
        {
            return (string)this;
        }

        private static (uint indexGroup, uint indexOffset, int dataLength) ParseIndexes(string indices)
        {
            if (string.IsNullOrWhiteSpace(indices))
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_INDICES_FORMAT,
                    message: "Indices string cannot be null or empty",
                    details: "Provide indices in format: [indexGroup, indexOffset, dataLength]. Example: [0x4020, 0, 256]"
                );
            }

            // 严格匹配 [int, int, int] 格式，支持十进制和十六进制
            string pattern = @"^\[\s*(0x[0-9a-fA-F]+|\d+)\s*,\s*(0x[0-9a-fA-F]+|\d+)\s*,\s*(0x[0-9a-fA-F]+|\d+)\s*\]$";
            var match = Regex.Match(indices, pattern);

            if (!match.Success)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_INDICES_FORMAT,
                    message: "Invalid indices format. Expected: [indexGroup, indexOffset, dataLength]",
                    details: $"Provided indices: '{indices}'. Valid examples: [0x4020, 0, 256], [100, 200, 1024], [0x4021, 10, 512]"
                );
            }

            try
            {
                // 解析匹配的组（支持十进制和十六进制）
                uint indexGroup = match.Groups[1].Value.StartsWith("0x")
                    ? Convert.ToUInt32(match.Groups[1].Value.Substring(2), 16)
                    : uint.Parse(match.Groups[1].Value);

                uint indexOffset = match.Groups[2].Value.StartsWith("0x")
                    ? Convert.ToUInt32(match.Groups[2].Value.Substring(2), 16)
                    : uint.Parse(match.Groups[2].Value);

                int dataLength = match.Groups[3].Value.StartsWith("0x")
                    ? Convert.ToInt32(match.Groups[3].Value.Substring(2), 16)
                    : int.Parse(match.Groups[3].Value);

                // 验证dataLength必须大于0
                if (dataLength <= 0)
                {
                    throw new BeckhoffException(
                        code: ADS_INVALID_STRING_LENGTH,
                        message: "Data length must be greater than zero",
                        details: $"Data length must be a positive integer. Provided value: {dataLength}"
                    );
                }

                const uint MAX_ALLOWED_LENGTH = 65535;
                if (dataLength > MAX_ALLOWED_LENGTH)
                {
                    throw new BeckhoffException(
                        code: ADS_INVALID_STRING_LENGTH,
                        message: $"Data length exceeds maximum allowed limit of {MAX_ALLOWED_LENGTH}",
                        details: $"Provided data length: {dataLength}. Maximum allowed: {MAX_ALLOWED_LENGTH}"
                    );
                }

                return (indexGroup, indexOffset, dataLength);
            }
            catch (BeckhoffException)
            {
                throw; // 重新抛出BeckhoffException
            }
            catch (FormatException ex)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_INDEX_NUMBER_FORMAT,
                    message: "Failed to parse numbers in indices string",
                    details: $"Indices: '{indices}', inner exception: {ex.Message}. Use decimal (e.g., 123) or hex (e.g., 0x7B) format"
                );
            }
            catch (OverflowException ex)
            {
                throw new BeckhoffException(
                    code: ADS_INDEX_NUMBER_OVERFLOW,
                    message: "Number in indices is out of valid range",
                    details: $"Indices: '{indices}', valid range: 0 to {uint.MaxValue}, inner exception: {ex.Message}"
                );
            }
        }
    }
}
