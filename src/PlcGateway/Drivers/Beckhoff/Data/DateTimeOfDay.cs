using PlcGateway.Drivers.Beckhoff.Exceptions;
using System;

using static PlcGateway.Drivers.Beckhoff.AdsErrorCode;

namespace PlcGateway.Drivers.Beckhoff.Data
{
    public readonly struct DateTimeOfDay
    {
        public const int Size = sizeof(uint);
        public const uint MaxValue = 86399999; // 单位：毫秒 (24小时-1毫秒)

        public uint Value { get; }

        public DateTimeOfDay(uint millisecondsSinceMidnight)
        {
            if (millisecondsSinceMidnight > MaxValue)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: $"Time of day value {millisecondsSinceMidnight} ms is out of range",
                    details: $"Value must be between 0 and {MaxValue} ms (23:59:59.999)"
                );
            }

            this.Value = millisecondsSinceMidnight;
        }

        public static implicit operator DateTimeOfDay(uint millisecondsSinceMidnight)
        {
            return new DateTimeOfDay(millisecondsSinceMidnight);
        }

        public static implicit operator uint(DateTimeOfDay dateTimeOfDay)
        {
            return dateTimeOfDay.Value;
        }

        public static implicit operator TimeSpan(DateTimeOfDay dateTimeOfDay)
        {
            return TimeSpan.FromMilliseconds(dateTimeOfDay.Value);
        }

        public static implicit operator DateTimeOfDay(TimeSpan timeSpan)
        {
            if (timeSpan < TimeSpan.Zero)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: "Time of day cannot be negative",
                    details: $"TimeSpan: {timeSpan}"
                );
            }

            if (timeSpan >= TimeSpan.FromDays(1))
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: "Time of day exceeds 24 hours",
                    details: $"TimeSpan: {timeSpan}, Maximum: 23:59:59.999"
                );
            }

            double totalMs = timeSpan.TotalMilliseconds;

            if (totalMs < 0 || totalMs > MaxValue)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: "Invalid time of day value",
                    details: $"TimeSpan: {timeSpan}, Total milliseconds: {totalMs:F0}"
                );
            }

            return new DateTimeOfDay((uint)totalMs);
        }
    }
}
