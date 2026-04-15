using PlcGateway.Abstractions;
using PlcGateway.Core.Converter;
using PlcGateway.Core.Exceptions;
using PlcGateway.Drivers.Inovance.Data;
using PlcGateway.Drivers.Inovance.Exceptions;
using System;
using System.Net;
using System.Text;
using static PlcGateway.Drivers.Inovance.InovanceErrorCode;

namespace PlcGateway.Drivers.Inovance
{
    public class ExInovanceEIPDriver : IPLCDriver
    {
        private readonly ExInovanceEIPDriverImpl _driver;

        public bool IsConnected => _driver.IsConnected;

        public ExInovanceEIPDriver(IPAddress hostIPAddress, IPAddress plcIPAddress, Encoding encoding)
        {
            _driver = new ExInovanceEIPDriverImpl(hostIPAddress, plcIPAddress, encoding);
        }

        public ExInovanceEIPDriver(IPAddress hostIPAddress, IPAddress plcIPAddress)
        {
            _driver = new ExInovanceEIPDriverImpl(hostIPAddress, plcIPAddress);
        }

        public void Connect()
        {
            _driver.Connect();
        }

        public TValue Read<TValue>(string address)
        {
            return _driver.Read<TValue>(address);
        }

        public void Write<TValue>(string address, TValue value)
        {
            if (value is null)
            {
                throw new InovanceException(
                    code: INOVANCE_NULL_VALUE, "Value cannot be null.")
                    .WithData("Parameter", nameof(value))
                    .WithData("Address", address);
            }

            Writer<TValue>.Write(address, value, _driver);
        }

        /// <summary>
        /// 数据写入器，避免动态反射
        /// </summary>
        private static class Writer<T>
        {
            public static readonly Action<string, T, ExInovanceEIPDriverImpl> Write = CreateWriter();

            private static Action<string, T, ExInovanceEIPDriverImpl> CreateWriter()
            {
                // 基元类型
                if (typeof(T) == typeof(bool))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, bool>(val));

                if (typeof(T) == typeof(byte))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, byte>(val));

                if (typeof(T) == typeof(sbyte))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, sbyte>(val));

                if (typeof(T) == typeof(short))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, short>(val));

                if (typeof(T) == typeof(ushort))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, ushort>(val));

                if (typeof(T) == typeof(int))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, int>(val));

                if (typeof(T) == typeof(uint))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, uint>(val));

                if (typeof(T) == typeof(long))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, long>(val));

                if (typeof(T) == typeof(ulong))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, ulong>(val));

                if (typeof(T) == typeof(float))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, float>(val));

                if (typeof(T) == typeof(double))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, double>(val));

                if (typeof(T) == typeof(string))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, string>(val));

                // Bits 类型
                if (typeof(T) == typeof(Bits8Bit))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, Bits8Bit>(val));

                if (typeof(T) == typeof(Bits16Bit))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, Bits16Bit>(val));

                if (typeof(T) == typeof(Bits32Bit))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, Bits32Bit>(val));

                if (typeof(T) == typeof(Bits64Bit))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, Bits64Bit>(val));

                // Structure 类型
                if (typeof(T) == typeof(Structure))
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, Structure>(val));

                // 不支持的类型
                return (addr, val, drv) =>
                {
                    var typeName = typeof(T).Name;
                    throw new InovanceException(
                        INOVANCE_UNSUPPORTED_TYPE, $"Type '{typeName}' is not supported.")
                        .WithData("Type", typeName)
                        .WithData("Address", addr)
                        .WithData("SupportedTypes", "bool, byte, sbyte, short, ushort, int, uint, long, ulong, float, double, string, Bits8Bit, Bits16Bit, Bits32Bit, Bits64Bit");
                };
            }
        }

        public void Disconnect()
        {
            _driver.Disconnect();
        }

        public void Dispose()
        {
            _driver.Dispose();
        }
    }
}
