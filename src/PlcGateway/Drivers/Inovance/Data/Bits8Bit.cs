using System;

namespace PlcGateway.Drivers.Inovance.Data
{
    public struct Bits8Bit
    {
        private readonly byte _value;

        public Bits8Bit(byte value)
        {
            _value = value;
        }

        public byte[] GetBytes()
        {
            return BitConverter.GetBytes(_value);
        }

        public static implicit operator Bits8Bit(byte value)
        {
            return new Bits8Bit(value);
        }

        public static implicit operator byte(Bits8Bit bits)
        {
            return bits._value;
        }
    }
}
