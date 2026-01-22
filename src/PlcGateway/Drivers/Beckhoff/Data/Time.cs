using System;
using PlcGateway.Drivers.Beckhoff.Exceptions;
using static PlcGateway.Drivers.Beckhoff.AdsErrorCode;

namespace PlcGateway.Drivers.Beckhoff.Data
{
    /// <summary>
    /// 倍福 TIME 类型
    /// </summary>
    public readonly struct Time
    {
        public const int Size = sizeof(uint);
        public const uint MaxValue = 4294967295;  // uint.MaxValue

        public uint Value { get; }

        public Time(uint value)
        {
            this.Value = value;
        }

        public static implicit operator Time(uint milliseconds)
        {
            return new Time(milliseconds);
        }

        public static implicit operator uint(Time time)
        {
            return time.Value;
        }

        public static implicit operator Time(TimeSpan value)
        {
            if (value < TimeSpan.Zero)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: "Time cannot be negative",
                    details: $"Provided TimeSpan: {value}. Minimum allowed value is TimeSpan.Zero."
                );
            }

            var totalMs = (uint)value.TotalMilliseconds;

            // 检查是否超过最大值
            if (totalMs > MaxValue)
            {
                TimeSpan maxTimeSpan = TimeSpan.FromMilliseconds(MaxValue);
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: $"Time exceeds maximum allowed value",
                    details: $"Provided: {value} ({totalMs:F0} ms), " +
                            $"Maximum: {maxTimeSpan} ({MaxValue} ms)"
                );
            }

            return new Time(totalMs);
        }

        public static implicit operator TimeSpan(Time time)
        {
            return TimeSpan.FromMilliseconds(time.Value);
        }
    }
}
