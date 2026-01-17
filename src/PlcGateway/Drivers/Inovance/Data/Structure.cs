namespace PlcGateway.Drivers.Inovance.Data
{
    internal struct Structure
    {
        private readonly byte[] _value;

        public Structure(byte[] value)
        {
            _value = value;
        }

        public byte[] GetBytes()
        {
            return _value;
        }

        public static implicit operator Structure(byte[] value)
        {
            return new Structure(value);
        }

        public static implicit operator byte[](Structure structValue)
        {
            return structValue._value;
        }
    }
}
