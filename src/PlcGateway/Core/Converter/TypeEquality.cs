namespace PlcGateway.Core.Converter
{
    public static class TypeEquality<TSource, TTarget>
    {
        public static readonly bool AreSameType =
            typeof(TSource) == typeof(TTarget);
    }
}
