using PlcGateway.Abstractions;
using PlcGateway.Core.Converter;
using PlcGateway.Drivers.Beckhoff.Exceptions;
using System;
using System.Text;
using TwinCAT.Ads;
using static PlcGateway.Drivers.Beckhoff.AdsErrorCode;
using BeckhoffData = PlcGateway.Drivers.Beckhoff.Data;

namespace PlcGateway.Drivers.Beckhoff
{
    public class BeckhoffAdsSymbolDriver : IPLCDriver
    {
        private readonly BeckhoffAdsSymbolDriverImpl _driver;

        public bool IsConnected => _driver.IsConnected;

        public BeckhoffAdsSymbolDriver(AmsNetId amsNetId, AmsPort port, Encoding defaultEncoding)
        {
            _driver = new BeckhoffAdsSymbolDriverImpl(amsNetId, port, defaultEncoding);
        }

        public BeckhoffAdsSymbolDriver(AmsNetId amsNetId, AmsPort port)
        {
            _driver = new BeckhoffAdsSymbolDriverImpl(amsNetId, port);
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

        public TValue Read<TValue>(string instancePath)
        {
            return _driver.Read<TValue>(instancePath);
        }

        /// <summary>
        /// 数据写入器，避免动态反射
        /// 使用泛型缓存优化性能
        /// </summary>
        private static class Writer<T>
        {
            public static readonly Action<string, T, BeckhoffAdsSymbolDriverImpl> Write = CreateWriter();

            private static Action<string, T, BeckhoffAdsSymbolDriverImpl> CreateWriter()
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
                    throw new BeckhoffException(ADS_UNSUPPORTED_TYPE, $"Type '{typeName}' is not supported.")
                        .WithData("Type", typeName)
                        .WithData("Address", addr)
                        .WithData("SupportedTypes",
                            "bool, byte, sbyte, short, ushort, int, uint, long, ulong, float, double, string, " +
                            "BeckhoffData.Date, BeckhoffData.DateTime, BeckhoffData.DateTimeOfDay, BeckhoffData.LongDate, " +
                            "BeckhoffData.LongDateTime, BeckhoffData.LongDateTimeOfDay, BeckhoffData.LongTime, BeckhoffData.Time");
                };
            }
        }

        public void Write<TValue>(string instancePath, TValue value)
        {
            if (value is null)
            {
                throw new BeckhoffException(ADS_NULL_VALUE, "Value cannot be null.")
                    .WithData("Parameter", nameof(value))
                    .WithData("Indices", instancePath);
            }

            Writer<TValue>.Write(instancePath, value, _driver);
        }
    }
}
