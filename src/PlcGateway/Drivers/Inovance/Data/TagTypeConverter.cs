using System;
using PlcGateway.Core.Converter;
using PlcGateway.Core.Exceptions;
using PlcGateway.Drivers.Inovance.Native;

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
                _ => throw new BusinessException(
                    code: "UNKNOWN_TAG_TYPE",
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
            if (TypeEquality<TTarget, sbyte>.AreSameType) return TagType.TAG_TYPE_SINT;
            if (TypeEquality<TTarget, short>.AreSameType) return TagType.TAG_TYPE_INT;
            if (TypeEquality<TTarget, int>.AreSameType) return TagType.TAG_TYPE_DINT;
            if (TypeEquality<TTarget, long>.AreSameType) return TagType.TAG_TYPE_LINT;
            if (TypeEquality<TTarget, byte>.AreSameType) return TagType.TAG_TYPE_USINT;
            if (TypeEquality<TTarget, ushort>.AreSameType) return TagType.TAG_TYPE_UINT;
            if (TypeEquality<TTarget, uint>.AreSameType) return TagType.TAG_TYPE_UDINT;
            if (TypeEquality<TTarget, ulong>.AreSameType) return TagType.TAG_TYPE_ULINT;
            if (TypeEquality<TTarget, float>.AreSameType) return TagType.TAG_TYPE_REAL;
            if (TypeEquality<TTarget, double>.AreSameType) return TagType.TAG_TYPE_LREAL;
            if (TypeEquality<TTarget, Bits8Bit>.AreSameType) return TagType.TAG_TYPE_BYTE;
            if (TypeEquality<TTarget, Bits16Bit>.AreSameType) return TagType.TAG_TYPE_WORD;
            if (TypeEquality<TTarget, Bits32Bit>.AreSameType) return TagType.TAG_TYPE_DWORD;
            if (TypeEquality<TTarget, Bits64Bit>.AreSameType) return TagType.TAG_TYPE_LWORD;
            if (TypeEquality<TTarget, Structure>.AreSameType) return TagType.TAG_TYPE_STRUCT;
            if (TypeEquality<TTarget, bool>.AreSameType) return TagType.TAG_TYPE_BOOL;
            if (TypeEquality<TTarget, string>.AreSameType) return TagType.TAG_TYPE_STRING;

            throw new BusinessException(
                 code: "UNSUPPORTED_TAG_TYPE",
                 message: $"Cannot convert type '{typeof(TTarget).Name}' to TagType",
                 details: $"Type: {typeof(TTarget).FullName} | Assembly: {typeof(TTarget).Assembly.GetName().Name}"
             );
        }
    }
}
