using System;
using PlcGateway.Core.Exceptions;

namespace PlcGateway.Drivers.Beckhoff.Data
{
    public readonly struct LongDateTimeOfDay
    {
        public const int Size = sizeof(ulong);
        public const ulong NanosecondsPerDay = 86400000000000; // 24 * 60 * 60 * 1,000,000,000
        public const ulong MaxValue = NanosecondsPerDay - 1; // 单位：纳秒，最大值为 86399999999999

        public ulong Value { get; } // 单位：纳秒

        public LongDateTimeOfDay(ulong nanosecondsSinceMidnight)
        {
            if (nanosecondsSinceMidnight > MaxValue)
            {
                throw new BusinessException(
                    code: "TIME_OF_DAY_OUT_OF_RANGE",
                    message: $"Time of day value {nanosecondsSinceMidnight} ns is out of range",
                    details: $"Value must be between 0 and {MaxValue} ns (23:59:59.999999999)"
                );
            }

            this.Value = nanosecondsSinceMidnight;
        }

        public static implicit operator LongDateTimeOfDay(ulong nanosecondsSinceMidnight)
        {
            return new LongDateTimeOfDay(nanosecondsSinceMidnight);
        }

        public static implicit operator ulong(LongDateTimeOfDay dateTimeOfDay)
        {
            return dateTimeOfDay.Value;
        }

        public static implicit operator TimeSpan(LongDateTimeOfDay dateTimeOfDay)
        {
            // 纳秒转换为 TimeSpan
            long ticks = (long)(dateTimeOfDay.Value / 100); // 1 tick = 100 nanoseconds
            return TimeSpan.FromTicks(ticks);
        }

        public static implicit operator LongDateTimeOfDay(TimeSpan timeSpan)
        {
            if (timeSpan < TimeSpan.Zero)
            {
                throw new BusinessException(
                    code: "TIME_OF_DAY_NEGATIVE",
                    message: "Time of day cannot be negative",
                    details: $"TimeSpan: {timeSpan}"
                );
            }

            if (timeSpan >= TimeSpan.FromDays(1))
            {
                throw new BusinessException(
                    code: "TIME_OF_DAY_EXCEEDS_24H",
                    message: "Time of day exceeds 24 hours",
                    details: $"TimeSpan: {timeSpan}, Maximum: 23:59:59.999999999"
                );
            }

            // 将 TimeSpan 转换为纳秒
            ulong nanoseconds = (ulong)(timeSpan.Ticks * 100L); // 1 tick = 100 nanoseconds

            if (nanoseconds > MaxValue)
            {
                throw new BusinessException(
                    code: "TIME_OF_DAY_INVALID",
                    message: "Invalid time of day value",
                    details: $"TimeSpan: {timeSpan}, Total nanoseconds: {nanoseconds}"
                );
            }

            return new LongDateTimeOfDay(nanoseconds);
        }
    }
}
