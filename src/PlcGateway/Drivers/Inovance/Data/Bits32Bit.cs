using System;

namespace PlcGateway.Drivers.Inovance.Data
{
    public struct Bits32Bit
    {
        private readonly uint _value;

        public Bits32Bit(uint value)
        {
            _value = value;
        }

        public byte[] GetBytes()
        {
            return BitConverter.GetBytes(_value);
        }

        public static implicit operator Bits32Bit(uint value)
        {
            return new Bits32Bit(value);
        }

        public static implicit operator uint(Bits32Bit bites)
        {
            return bites._value;
        }
    }
}
