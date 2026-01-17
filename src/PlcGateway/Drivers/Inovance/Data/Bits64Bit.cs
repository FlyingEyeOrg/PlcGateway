using System;

namespace PlcGateway.Drivers.Inovance.Data
{
    public class Bits64Bit
    {
        private readonly ulong _value;

        public Bits64Bit(ulong value)
        {
            _value = value;
        }

        public byte[] GetBytes()
        {
            return BitConverter.GetBytes(_value);
        }

        public static implicit operator Bits64Bit(ulong value)
        {
            return new Bits64Bit(value);
        }

        public static implicit operator ulong(Bits64Bit bites)
        {
            return bites._value;
        }
    }
}
