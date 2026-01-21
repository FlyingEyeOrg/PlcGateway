using PlcGateway.Core.Converter;
using PlcGateway.Core.Exceptions;
using static PlcGateway.Drivers.ErrorCode;
using BeckhoffData = PlcGateway.Drivers.Beckhoff.Data;

namespace PlcGateway.Drivers.Converter
{
    /// <summary>
    /// 类型大小缓存类
    /// 每个泛型类型 T 在静态构造函数中计算大小并缓存
    /// </summary>
    /// <typeparam name="T">要检查的类型</typeparam>
    internal static class SizeOf<T>
    {
        /// <summary>
        /// 类型的大小（字节数）
        /// 对于不支持的类型会抛出异常
        /// </summary>
        public static readonly int Value = CalculateSize();

        private static int CalculateSize()
        {
            // 内置类型检查
            if (TypeEquality<int, T>.AreSameType) return 4;
            if (TypeEquality<bool, T>.AreSameType) return 1;
            if (TypeEquality<double, T>.AreSameType) return 8;
            if (TypeEquality<float, T>.AreSameType) return 4;
            if (TypeEquality<long, T>.AreSameType) return 8;
            if (TypeEquality<ulong, T>.AreSameType) return 8;
            if (TypeEquality<uint, T>.AreSameType) return 4;
            if (TypeEquality<short, T>.AreSameType) return 2;
            if (TypeEquality<ushort, T>.AreSameType) return 2;
            if (TypeEquality<sbyte, T>.AreSameType) return 1;
            if (TypeEquality<byte, T>.AreSameType) return 1;

            // Beckhoff 数据类型检查
            if (TypeEquality<BeckhoffData.Date, T>.AreSameType) return BeckhoffData.Date.Size;
            if (TypeEquality<BeckhoffData.DateTime, T>.AreSameType) return BeckhoffData.DateTime.Size;
            if (TypeEquality<BeckhoffData.DateTimeOfDay, T>.AreSameType) return BeckhoffData.DateTimeOfDay.Size;
            if (TypeEquality<BeckhoffData.LongDate, T>.AreSameType) return BeckhoffData.LongDate.Size;
            if (TypeEquality<BeckhoffData.LongDateTime, T>.AreSameType) return BeckhoffData.LongDateTime.Size;
            if (TypeEquality<BeckhoffData.LongDateTimeOfDay, T>.AreSameType) return BeckhoffData.LongDateTimeOfDay.Size;
            if (TypeEquality<BeckhoffData.LongTime, T>.AreSameType) return BeckhoffData.LongTime.Size;
            if (TypeEquality<BeckhoffData.Time, T>.AreSameType) return BeckhoffData.Time.Size;

            var typeName = typeof(T).Name;
            var fullTypeName = typeof(T).FullName;

            throw new BusinessException(
                code: SIZE_OF_UNSUPPORTED_TYPE,
                message: $"Unsupported data type: {typeName}",
                details: $"Type '{fullTypeName}' is not supported. See TypeSize<T> class for supported types."
            );
        }
    }
}