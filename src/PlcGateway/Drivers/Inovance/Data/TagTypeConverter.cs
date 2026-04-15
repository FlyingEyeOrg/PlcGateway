using PlcGateway.Core.Exceptions;
using PlcGateway.Drivers.Inovance.Exceptions;
using PlcGateway.Drivers.Inovance.Native;
using System;

using static PlcGateway.Drivers.Inovance.InovanceErrorCode;

namespace PlcGateway.Drivers.Inovance.Data
{
    internal static class TagTypeConverter
    {
        // 预缓存所有 Type
        private static class TypeCache
        {
            public static readonly Type SByte = typeof(sbyte);
            public static readonly Type Short = typeof(short);
            public static readonly Type Int = typeof(int);
            public static readonly Type Long = typeof(long);
            public static readonly Type Byte = typeof(byte);
            public static readonly Type UShort = typeof(ushort);
            public static readonly Type UInt = typeof(uint);
            public static readonly Type ULong = typeof(ulong);
            public static readonly Type Float = typeof(float);
            public static readonly Type Double = typeof(double);
            public static readonly Type Bits8Bit = typeof(Bits8Bit);
            public static readonly Type Bits16Bit = typeof(Bits16Bit);
            public static readonly Type Bits32Bit = typeof(Bits32Bit);
            public static readonly Type Bits64Bit = typeof(Bits64Bit);
            public static readonly Type Structure = typeof(Structure);
            public static readonly Type Bool = typeof(bool);
            public static readonly Type String = typeof(string);
        }

        public static Type GetType(TagType tagType)
        {
            return tagType switch
            {
                TagType.TAG_TYPE_SINT => TypeCache.SByte,
                TagType.TAG_TYPE_INT => TypeCache.Short,
                TagType.TAG_TYPE_DINT => TypeCache.Int,
                TagType.TAG_TYPE_LINT => TypeCache.Long,
                TagType.TAG_TYPE_USINT => TypeCache.Byte,
                TagType.TAG_TYPE_UINT => TypeCache.UShort,
                TagType.TAG_TYPE_UDINT => TypeCache.UInt,
                TagType.TAG_TYPE_ULINT => TypeCache.ULong,
                TagType.TAG_TYPE_REAL => TypeCache.Float,
                TagType.TAG_TYPE_LREAL => TypeCache.Double,
                TagType.TAG_TYPE_BYTE => TypeCache.Bits8Bit,
                TagType.TAG_TYPE_WORD => TypeCache.Bits16Bit,
                TagType.TAG_TYPE_DWORD => TypeCache.Bits32Bit,
                TagType.TAG_TYPE_LWORD => TypeCache.Bits64Bit,
                TagType.TAG_TYPE_STRUCT => TypeCache.Structure,
                TagType.TAG_TYPE_BOOL => TypeCache.Bool,
                TagType.TAG_TYPE_STRING => TypeCache.String,
                _ => throw new InovanceException(
                    code: INOVANCE_UNKNOWN_TAG_TYPE,
                    message: $"Unknown or unsupported TagType: {tagType}",
                    details: $"TagType value: {(int)tagType} (0x{(int)tagType:X}) | Supported range: 0-{(int)TagType.TAG_TYPE_STRING}"
                )
            };
        }
    }

    internal static class TagTypeConverter<TTarget>
    {
        public static readonly TagType Value = GetTagType();

        private static TagType GetTagType()
        {
            if (typeof(TTarget) == typeof(sbyte)) return TagType.TAG_TYPE_SINT;
            if (typeof(TTarget) == typeof(short)) return TagType.TAG_TYPE_INT;
            if (typeof(TTarget) == typeof(int)) return TagType.TAG_TYPE_DINT;
            if (typeof(TTarget) == typeof(long)) return TagType.TAG_TYPE_LINT;
            if (typeof(TTarget) == typeof(byte)) return TagType.TAG_TYPE_USINT;
            if (typeof(TTarget) == typeof(ushort)) return TagType.TAG_TYPE_UINT;
            if (typeof(TTarget) == typeof(uint)) return TagType.TAG_TYPE_UDINT;
            if (typeof(TTarget) == typeof(ulong)) return TagType.TAG_TYPE_ULINT;
            if (typeof(TTarget) == typeof(float)) return TagType.TAG_TYPE_REAL;
            if (typeof(TTarget) == typeof(double)) return TagType.TAG_TYPE_LREAL;
            if (typeof(TTarget) == typeof(Bits8Bit)) return TagType.TAG_TYPE_BYTE;
            if (typeof(TTarget) == typeof(Bits16Bit)) return TagType.TAG_TYPE_WORD;
            if (typeof(TTarget) == typeof(Bits32Bit)) return TagType.TAG_TYPE_DWORD;
            if (typeof(TTarget) == typeof(Bits64Bit)) return TagType.TAG_TYPE_LWORD;
            if (typeof(TTarget) == typeof(Structure)) return TagType.TAG_TYPE_STRUCT;
            if (typeof(TTarget) == typeof(bool)) return TagType.TAG_TYPE_BOOL;
            if (typeof(TTarget) == typeof(string)) return TagType.TAG_TYPE_STRING;

            throw new InovanceException(
                 code: INOVANCE_UNKNOWN_TAG_TYPE,
                 message: $"Cannot convert type '{typeof(TTarget).Name}' to TagType",
                 details: $"Type: {typeof(TTarget).FullName} | Assembly: {typeof(TTarget).Assembly.GetName().Name}"
             );
        }
    }
}
