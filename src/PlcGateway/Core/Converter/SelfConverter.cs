using PlcGateway.Core.Exceptions;
using System.Runtime.CompilerServices;
using static PlcGateway.Drivers.DriverErrorCode;

namespace PlcGateway.Core.Converter
{
    public static class SelfConverter
    {
        public static TTarget ToSelf<TSource, TTarget>(TSource value)
        {
            if (!TypeEquality<TSource, TTarget>.AreSameType)
            {
                throw new BusinessException(DRIVER_SELF_CONVERTER_TYPE_MISMATCH,
                 $"Cannot convert from '{typeof(TSource).Name}' to '{typeof(TTarget).Name}'. Types must be identical.",
                 $"Source: {typeof(TSource).FullName} | Target: {typeof(TTarget).FullName} | Assembly: {typeof(TSource).Assembly.GetName().Name}");
            }

            return Unsafe.As<TSource, TTarget>(ref Unsafe.AsRef(in value));
        }
    }
}