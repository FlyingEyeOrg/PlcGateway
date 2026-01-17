namespace PlcGateway.Drivers.Inovance.Native
{
    internal enum TagType : int
    {
        TAG_TYPE_UNDEFINE = -1,
        TAG_TYPE_BOOL = 0xC1,
        TAG_TYPE_SINT = 0xC2,
        TAG_TYPE_INT = 0xC3,
        TAG_TYPE_DINT = 0xC4,
        TAG_TYPE_LINT = 0xC5,
        TAG_TYPE_USINT = 0xC6,
        TAG_TYPE_UINT = 0xC7,
        TAG_TYPE_UDINT = 0xC8,
        TAG_TYPE_ULINT = 0xC9,
        TAG_TYPE_REAL = 0xCA,
        TAG_TYPE_LREAL = 0xCB,
        TAG_TYPE_STRING = 0xD0,
        TAG_TYPE_BYTE = 0xD1,
        TAG_TYPE_WORD = 0xD2,
        TAG_TYPE_DWORD = 0xD3,
        TAG_TYPE_LWORD = 0xD4,
        TAG_TYPE_STRUCT = 0xA2,
        TAG_TYPE_ARRAY = 0xA3
    }
}
