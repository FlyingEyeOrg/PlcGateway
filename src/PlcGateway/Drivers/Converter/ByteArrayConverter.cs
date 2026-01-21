using PlcGateway.Core.Converter;
using PlcGateway.Core.Exceptions;
using PlcGateway.Drivers.Inovance.Data;
using System;
using System.Text;
using static PlcGateway.Drivers.DriverErrorCode;
using BeckhoffData = PlcGateway.Drivers.Beckhoff.Data;
using InovanceData = PlcGateway.Drivers.Inovance.Data;

namespace PlcGateway.Drivers.Converter
{
    /// <summary>
    /// 字节转换器
    /// </summary>
    /// <typeparam name="TTarget">需要转换到指定的目标类型</typeparam>
    public static class ByteArrayConverter<TTarget>
    {
        public static readonly Func<byte[], Encoding?, TTarget> Convert = CreateConverter();

        private static Func<byte[], Encoding?, TTarget> CreateConverter()
        {
            if (TypeEquality<TTarget, bool>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<bool, TTarget>(BitConverter.ToBoolean(bytes, 0));

            if (TypeEquality<TTarget, sbyte>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<sbyte, TTarget>((sbyte)bytes[0]);

            if (TypeEquality<TTarget, byte>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<byte, TTarget>(bytes[0]);

            if (TypeEquality<TTarget, short>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<short, TTarget>(BitConverter.ToInt16(bytes, 0));

            if (TypeEquality<TTarget, ushort>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<ushort, TTarget>(BitConverter.ToUInt16(bytes, 0));

            if (TypeEquality<TTarget, int>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<int, TTarget>(BitConverter.ToInt32(bytes, 0));

            if (TypeEquality<TTarget, uint>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<uint, TTarget>(BitConverter.ToUInt32(bytes, 0));

            if (TypeEquality<TTarget, long>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<long, TTarget>(BitConverter.ToInt64(bytes, 0));

            if (TypeEquality<TTarget, ulong>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<ulong, TTarget>(BitConverter.ToUInt64(bytes, 0));

            if (TypeEquality<TTarget, float>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<float, TTarget>(BitConverter.ToSingle(bytes, 0));

            if (TypeEquality<TTarget, double>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<double, TTarget>(BitConverter.ToDouble(bytes, 0));

            if (TypeEquality<TTarget, string>.AreSameType)
                return (bytes, encoding) => SelfConverter.ToSelf<string, TTarget>((encoding ?? Encoding.UTF8).GetString(bytes).TrimEnd('\0'));

            #region 倍福专有数据类型
            if (TypeEquality<TTarget, BeckhoffData.Date>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<BeckhoffData.Date, TTarget>((BeckhoffData.Date)BitConverter.ToUInt32(bytes, 0));

            if (TypeEquality<TTarget, BeckhoffData.DateTime>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<BeckhoffData.DateTime, TTarget>((BeckhoffData.DateTime)BitConverter.ToUInt32(bytes, 0));

            if (TypeEquality<TTarget, BeckhoffData.DateTimeOfDay>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<BeckhoffData.DateTimeOfDay, TTarget>((BeckhoffData.DateTimeOfDay)BitConverter.ToUInt32(bytes, 0));

            if (TypeEquality<TTarget, BeckhoffData.LongDate>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<BeckhoffData.LongDate, TTarget>((BeckhoffData.LongDate)BitConverter.ToUInt64(bytes, 0));

            if (TypeEquality<TTarget, BeckhoffData.LongDateTime>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<BeckhoffData.LongDateTime, TTarget>((BeckhoffData.LongDateTime)BitConverter.ToUInt64(bytes, 0));

            if (TypeEquality<TTarget, BeckhoffData.LongDateTimeOfDay>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<BeckhoffData.LongDateTimeOfDay, TTarget>((BeckhoffData.LongDateTimeOfDay)BitConverter.ToUInt64(bytes, 0));

            if (TypeEquality<TTarget, BeckhoffData.LongTime>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<BeckhoffData.LongTime, TTarget>((BeckhoffData.LongTime)BitConverter.ToUInt64(bytes, 0));

            if (TypeEquality<TTarget, BeckhoffData.Time>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<BeckhoffData.Time, TTarget>((BeckhoffData.Time)BitConverter.ToUInt32(bytes, 0));
            #endregion

            #region 汇川专有数据类型
            if (TypeEquality<TTarget, InovanceData.Bits8Bit>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<InovanceData.Bits8Bit, TTarget>((Bits8Bit)bytes[0]);

            if (TypeEquality<TTarget, InovanceData.Bits16Bit>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<InovanceData.Bits16Bit, TTarget>((Bits16Bit)BitConverter.ToUInt16(bytes, 0));

            if (TypeEquality<TTarget, InovanceData.Bits32Bit>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<InovanceData.Bits32Bit, TTarget>((Bits32Bit)BitConverter.ToUInt32(bytes, 0));

            if (TypeEquality<TTarget, InovanceData.Bits64Bit>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<InovanceData.Bits64Bit, TTarget>((Bits64Bit)BitConverter.ToUInt64(bytes, 0));

            if (TypeEquality<TTarget, InovanceData.Structure>.AreSameType)
                return (bytes, _) => SelfConverter.ToSelf<InovanceData.Structure, TTarget>((Structure)bytes);
            #endregion

            return (bytes, _) => throw new BusinessException(
                code: DRIVER_BYTE_ARRAY_CONVERTER_UNSUPPORTED_TYPE,
                message: $"Unsupported data type: {typeof(TTarget).FullName}",
                details: "Cannot convert bytes to the specified type"
            );
        }
    }
}
