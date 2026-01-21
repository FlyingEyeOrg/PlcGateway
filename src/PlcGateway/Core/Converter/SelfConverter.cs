using System.Runtime.CompilerServices;
using PlcGateway.Core.Exceptions;

namespace PlcGateway.Core.Converter
{
    public static class SelfConverter
    {
        public static TTarget ToSelf<TSource, TTarget>(TSource value)
        {
            if (!TypeEquality<TSource, TTarget>.AreSameType)
            {
                throw new BusinessException("TYPE_MISMATCH",
                 $"Cannot convert from '{typeof(TSource).Name}' to '{typeof(TTarget).Name}'. Types must be identical.",
                 $"Source: {typeof(TSource).FullName} | Target: {typeof(TTarget).FullName} | Assembly: {typeof(TSource).Assembly.GetName().Name}");
            }

            return Unsafe.As<TSource, TTarget>(ref Unsafe.AsRef(in value));
        }
    }
}