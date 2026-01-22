using PlcGateway.Drivers.Beckhoff.Exceptions;
using System;

using static PlcGateway.Drivers.Beckhoff.AdsErrorCode;

namespace PlcGateway.Drivers.Beckhoff.Data
{
    public readonly struct LongDateTime
    {
        public const int Size = sizeof(ulong);
        private const long TicksPerNanosecond = 100; // 1 tick = 100 nanoseconds
        private static readonly System.DateTime DateBase = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

        public const ulong MaxValue = ulong.MaxValue; // 单位: 纳秒
        public static readonly System.DateTime MaxDateTime =
            DateBase + System.TimeSpan.FromTicks((long)(MaxValue / TicksPerNanosecond));

        public ulong Value { get; } // 单位: 纳秒

        public LongDateTime(ulong value)
        {
            this.Value = value;
        }

        public static implicit operator LongDateTime(ulong value)
        {
            return new LongDateTime(value);
        }

        public static implicit operator ulong(LongDateTime date)
        {
            return date.Value;
        }

        public static implicit operator System.DateTime(LongDateTime date)
        {
            // 纳秒转换为 ticks: 1 tick = 100 nanoseconds
            long ticks = (long)(date.Value / TicksPerNanosecond);
            return DateBase + System.TimeSpan.FromTicks(ticks);
        }

        public static implicit operator LongDateTime(System.DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc)
            {
                date = date.ToUniversalTime();
            }

            if (date < DateBase)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: $"DateTime {date:yyyy-MM-dd HH:mm:ss.fffffff} is before Unix epoch (1970-01-01)",
                    details: $"Minimum allowed date is 1970-01-01"
                );
            }

            var span = date - DateBase;
            var ticks = span.Ticks;

            // 将 ticks 转换为纳秒
            ulong nanoseconds = (ulong)(ticks * TicksPerNanosecond);

            if (nanoseconds > MaxValue)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: $"DateTime {date:yyyy-MM-dd HH:mm:ss.fffffff} exceeds maximum allowed date",
                    details: $"Provided: {date:yyyy-MM-dd HH:mm:ss.fffffff}, Maximum: {MaxDateTime:yyyy-MM-dd HH:mm:ss.fffffff}"
                );
            }

            return new LongDateTime(nanoseconds);
        }
    }
}
