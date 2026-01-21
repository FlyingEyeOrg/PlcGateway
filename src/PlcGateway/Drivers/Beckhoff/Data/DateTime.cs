using PlcGateway.Drivers.Beckhoff.Exceptions;

namespace PlcGateway.Drivers.Beckhoff.Data
{
    public readonly struct DateTime
    {
        public const int Size = sizeof(uint);
        private static readonly System.DateTime DateBase = new System.DateTime(1970, 1, 1);

        public const uint MaxValue = 4294967295; // 单位 s
        public static readonly System.DateTime MaxDateTime = DateBase + System.TimeSpan.FromSeconds(MaxValue);

        public uint Value { get; }

        public DateTime(uint value)
        {
            this.Value = value;
        }

        public static implicit operator DateTime(uint value)
        {
            return new DateTime(value);
        }

        public static implicit operator uint(DateTime date)
        {
            return date.Value;
        }

        public static implicit operator System.DateTime(DateTime date)
        {
            return DateTime.DateBase + System.TimeSpan.FromSeconds(date.Value);
        }

        public static implicit operator DateTime(System.DateTime date)
        {
            if (date < DateBase)
            {
                throw new BeckhoffException(
                    code: "DATE_BEFORE_EPOCH",
                    message: $"DateTime {date:yyyy-MM-dd} is before Unix epoch (1970-01-01)",
                    details: $"Minimum allowed date is 1970-01-01"
                );
            }

            var span = date - DateBase;

            if (span.TotalSeconds > MaxValue)
            {
                throw new BeckhoffException(
                    code: "DATE_EXCEEDS_MAXIMUM",
                    message: $"DateTime {date:yyyy-MM-dd} exceeds maximum allowed date",
                    details: $"Provided: {date:yyyy-MM-dd}, Maximum: {MaxDateTime:yyyy-MM-dd}"
                );
            }

            if (span.TotalSeconds < 0)
            {
                throw new BeckhoffException(
                    code: "DATE_INVALID_TIMESPAN",
                    message: "TimeSpan calculation resulted in negative value",
                    details: $"DateTime: {date:yyyy-MM-dd}, Base: 1970-01-01"
                );
            }

            return new DateTime((uint)span.TotalSeconds);
        }
    }
}
