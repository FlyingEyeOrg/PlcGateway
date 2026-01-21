namespace PlcGateway.Abstractions
{
    public enum DataType
    {
        // Boolean
        Boolean,

        // 8-bit integral types
        Byte,       // 0-255
        SByte,      // -128 to 127

        // 16-bit integral types
        Int16,      // -32,768 to 32,767
        UInt16,     // 0 to 65,535

        // 32-bit integral types
        Int32,      // -2,147,483,648 to 2,147,483,647
        UInt32,     // 0 to 4,294,967,295

        // 64-bit integral types
        Int64,      // -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
        UInt64,     // 0 to 18,446,744,073,709,551,615

        // Floating point types
        Single,     // ±1.5 x 10^-45 to ±3.4 x 10^38, ~6-9 digits precision
        Double,     // ±5.0 x 10^-324 to ±1.7 x 10^308, ~15-17 digits precision
        Decimal,    // ±1.0 x 10^-28 to ±7.9228 x 10^28, 28-29 digits precision

        // String
        String,      // Unicode string

        // 汇川 PLC 专属类型
        Bits8Bit,   // 8位 
        Bits16Bit,  // 16位
        Bits32Bit,  // 32位
        Bits64Bit,   // 64位
        Structure    // 结构体
    }
}
