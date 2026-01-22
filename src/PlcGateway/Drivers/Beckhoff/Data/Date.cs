using PlcGateway.Drivers.Beckhoff.Exceptions;

using static PlcGateway.Drivers.Beckhoff.AdsErrorCode;

namespace PlcGateway.Drivers.Beckhoff.Data
{
    /// <summary>
    /// 倍福 DATE
    /// </summary>
    public readonly struct Date
    {
        public const int Size = sizeof(uint);

        private static readonly System.DateTime DateBase = new System.DateTime(1970, 1, 1);

        public const uint MaxValue = 4294967295; // 单位 s
        public static readonly System.DateTime MaxDateTime = DateBase + System.TimeSpan.FromSeconds(MaxValue);

        public uint Value { get; }

        public Date(uint value)
        {
            this.Value = value;
        }

        public static implicit operator Date(uint value)
        {
            return new Date(value);
        }

        public static implicit operator uint(Date date)
        {
            return date.Value;
        }

        public static implicit operator System.DateTime(Date date)
        {
            return Date.DateBase + System.TimeSpan.FromSeconds(date.Value);
        }

        public static implicit operator Date(System.DateTime date)
        {
            if (date < DateBase)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: $"DateTime {date:yyyy-MM-dd} is before Unix epoch (1970-01-01)",
                    details: $"Minimum allowed date is 1970-01-01"
                );
            }

            var span = date - DateBase;

            if (span.TotalSeconds > MaxValue)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: $"DateTime {date:yyyy-MM-dd} exceeds maximum allowed date",
                    details: $"Provided: {date:yyyy-MM-dd}, Maximum: {MaxDateTime:yyyy-MM-dd}"
                );
            }

            if (span.TotalSeconds < 0)
            {
                throw new BeckhoffException(
                    code: ADS_INVALID_ARGUMENT,
                    message: "TimeSpan calculation resulted in negative value",
                    details: $"DateTime: {date:yyyy-MM-dd}, Base: 1970-01-01"
                );
            }

            return new Date((uint)span.TotalSeconds);
        }
    }
}
