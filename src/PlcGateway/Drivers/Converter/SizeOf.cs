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
            if (typeof(T) == typeof(int)) return 4;
            if (typeof(T) == typeof(bool)) return 1;
            if (typeof(T) == typeof(double)) return 8;
            if (typeof(T) == typeof(float)) return 4;
            if (typeof(T) == typeof(long)) return 8;
            if (typeof(T) == typeof(ulong)) return 8;
            if (typeof(T) == typeof(uint)) return 4;
            if (typeof(T) == typeof(short)) return 2;
            if (typeof(T) == typeof(ushort)) return 2;
            if (typeof(T) == typeof(sbyte)) return 1;
            if (typeof(T) == typeof(byte)) return 1;

            // Beckhoff 数据类型检查
            if (typeof(T) == typeof(BeckhoffData.Date)) return BeckhoffData.Date.Size;
            if (typeof(T) == typeof(BeckhoffData.DateTime)) return BeckhoffData.DateTime.Size;
            if (typeof(T) == typeof(BeckhoffData.DateTimeOfDay)) return BeckhoffData.DateTimeOfDay.Size;
            if (typeof(T) == typeof(BeckhoffData.LongDate)) return BeckhoffData.LongDate.Size;
            if (typeof(T) == typeof(BeckhoffData.LongDateTime)) return BeckhoffData.LongDateTime.Size;
            if (typeof(T) == typeof(BeckhoffData.LongDateTimeOfDay)) return BeckhoffData.LongDateTimeOfDay.Size;
            if (typeof(T) == typeof(BeckhoffData.LongTime)) return BeckhoffData.LongTime.Size;
            if (typeof(T) == typeof(BeckhoffData.Time)) return BeckhoffData.Time.Size;

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