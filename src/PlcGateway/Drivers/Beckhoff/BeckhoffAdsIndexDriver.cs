using System;
using System.Text;
using PlcGateway.Abstractions;
using PlcGateway.Core.Converter;
using PlcGateway.Core.Exceptions;
using TwinCAT.Ads;
using BeckhoffData = PlcGateway.Drivers.Beckhoff.Data;
using static PlcGateway.Drivers.Beckhoff.BeckhoffErrorCode;

namespace PlcGateway.Drivers.Beckhoff
{
    public class BeckhoffAdsIndexDriver : IPLCDriver
    {
        private readonly BeckhoffAdsIndexDriverImpl _driver;

        public bool IsConnected => _driver.IsConnected;

        public BeckhoffAdsIndexDriver(AmsNetId amsNetId, AmsPort port, Encoding defaultEncoding)
        {
            _driver = new BeckhoffAdsIndexDriverImpl(amsNetId, port, defaultEncoding);
        }

        public BeckhoffAdsIndexDriver(AmsNetId amsNetId, AmsPort port)
        {
            _driver = new BeckhoffAdsIndexDriverImpl(amsNetId, port);
        }

        public void Connect()
        {
            _driver.Connect();
        }

        public void Disconnect()
        {
            _driver.Disconnect();
        }

        public void Dispose()
        {
            _driver.Dispose();
        }

        public TValue Read<TValue>(string address)
        {
            if (TypeEquality<string, TValue>.AreSameType)
            {
                var value = _driver.Read((StringAdsIndexAddress)address);
                return SelfConverter.ToSelf<string, TValue>(value);
            }

            return _driver.Read<TValue>(address);
        }

        /// <summary>
        /// 数据写入器，避免动态反射
        /// 使用泛型缓存优化性能
        /// </summary>
        private static class Writer<T>
        {
            public static readonly Action<string, T, BeckhoffAdsIndexDriverImpl> Write = CreateWriter();

            private static Action<string, T, BeckhoffAdsIndexDriverImpl> CreateWriter()
            {
                // 基元类型
                if (TypeEquality<T, bool>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, bool>(val));

                if (TypeEquality<T, byte>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, byte>(val));

                if (TypeEquality<T, sbyte>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, sbyte>(val));

                if (TypeEquality<T, short>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, short>(val));

                if (TypeEquality<T, ushort>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, ushort>(val));

                if (TypeEquality<T, int>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, int>(val));

                if (TypeEquality<T, uint>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, uint>(val));

                if (TypeEquality<T, long>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, long>(val));

                if (TypeEquality<T, ulong>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, ulong>(val));

                if (TypeEquality<T, float>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, float>(val));

                if (TypeEquality<T, double>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, double>(val));

                if (TypeEquality<T, string>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, string>(val));

                #region Beckhoff 专有数据类型
                if (TypeEquality<T, BeckhoffData.Date>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, BeckhoffData.Date>(val));

                if (TypeEquality<T, BeckhoffData.DateTime>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, BeckhoffData.DateTime>(val));

                if (TypeEquality<T, BeckhoffData.DateTimeOfDay>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, BeckhoffData.DateTimeOfDay>(val));

                if (TypeEquality<T, BeckhoffData.LongDate>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, BeckhoffData.LongDate>(val));

                if (TypeEquality<T, BeckhoffData.LongDateTime>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, BeckhoffData.LongDateTime>(val));

                if (TypeEquality<T, BeckhoffData.LongDateTimeOfDay>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, BeckhoffData.LongDateTimeOfDay>(val));

                if (TypeEquality<T, BeckhoffData.LongTime>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, BeckhoffData.LongTime>(val));

                if (TypeEquality<T, BeckhoffData.Time>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, BeckhoffData.Time>(val));
                #endregion

                // 不支持的类型
                return (addr, val, drv) =>
                {
                    var typeName = typeof(T).Name;
                    throw new BusinessException(
                        ADS_UNSUPPORTED_TYPE,
                        $"Type '{typeName}' is not supported.")
                        .WithData("Type", typeName)
                        .WithData("Address", addr)
                        .WithData("SupportedTypes",
                            "bool, byte, sbyte, short, ushort, int, uint, long, ulong, float, double, string, " +
                            "BeckhoffData.Date, BeckhoffData.DateTime, BeckhoffData.DateTimeOfDay, BeckhoffData.LongDate, " +
                            "BeckhoffData.LongDateTime, BeckhoffData.LongDateTimeOfDay, BeckhoffData.LongTime, BeckhoffData.Time");
                };
            }
        }

        public void Write<TValue>(string indices, TValue value)
        {
            if (value is null)
            {
                throw new BusinessException(ADS_NULL_VALUE, "Value cannot be null.")
                    .WithData("Parameter", nameof(value))
                    .WithData("Indices", indices);
            }

            Writer<TValue>.Write(indices, value, _driver);
        }
    }
}
