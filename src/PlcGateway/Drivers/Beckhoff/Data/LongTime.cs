using System;
using PlcGateway.Core.Exceptions;

namespace PlcGateway.Drivers.Beckhoff.Data
{
    /// <summary>
    /// 倍福 LTIME 类型
    /// </summary>
    public readonly struct LongTime
    {
        public const int Size = sizeof(ulong);
        public const ulong MaxValue = 18446744073709551615; // 纳秒

        public ulong Value { get; }

        public LongTime(ulong value)
        {
            this.Value = value;
        }

        public static implicit operator LongTime(ulong value)
        {
            return new LongTime(value);
        }

        public static implicit operator ulong(LongTime lTime)
        {
            return lTime.Value;
        }

        public static implicit operator TimeSpan(LongTime lTime)
        {
            // 检查是否超出 TimeSpan 范围
            if (lTime.Value / 100 > long.MaxValue)  // 转换为 ticks 检查
            {
                throw new BusinessException(
                    code: "LTIME_TO_TIMESPAN_OVERFLOW",
                    message: "LTime value too large for TimeSpan"
                );
            }

            double milliseconds = lTime.Value / 1_000_000.0;
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        public static implicit operator LongTime(TimeSpan time)
        {
            if (time < TimeSpan.Zero)
            {
                throw new BusinessException(
                    code: "LTIME_NEGATIVE_TIMESPAN",
                    message: "TimeSpan cannot be negative"
                );
            }

            double totalNs = time.TotalMilliseconds * 1_000_000.0;

            if (totalNs < 0 || totalNs > MaxValue)
            {
                throw new BusinessException(
                    code: "LTIME_OUT_OF_RANGE",
                    message: $"TimeSpan value {time} exceeds LTime range"
                );
            }

            return new LongTime((ulong)totalNs);
        }
    }
}
