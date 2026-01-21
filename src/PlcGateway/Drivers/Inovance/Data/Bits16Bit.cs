using System;

namespace PlcGateway.Drivers.Inovance.Data
{
    public struct Bits16Bit
    {
        private readonly ushort _value;

        public Bits16Bit(ushort value)
        {
            _value = value;
        }

        public byte[] GetBytes()
        {
            return BitConverter.GetBytes(_value);
        }

        public static implicit operator Bits16Bit(ushort value)
        {
            return new Bits16Bit(value);
        }

        public static implicit operator ushort(Bits16Bit bites)
        {
            return bites._value;
        }
    }
}
