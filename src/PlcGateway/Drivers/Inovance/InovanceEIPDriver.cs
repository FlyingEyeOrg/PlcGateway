using System;
using System.Net;
using System.Text;
using PlcGateway.Abstractions;
using PlcGateway.Core.Converter;
using PlcGateway.Core.Exceptions;
using PlcGateway.Drivers.Inovance.Data;

namespace PlcGateway.Drivers.Inovance
{
    public class InovanceEIPDriver : IPLCDriver
    {
        private readonly InovanceEIPDriverImpl _driver;

        public InovanceEIPDriver(IPAddress hostIPAddress, IPAddress plcIPAddress, Encoding encoding)
        {
            _driver = new InovanceEIPDriverImpl(hostIPAddress, plcIPAddress, encoding);
        }

        public InovanceEIPDriver(IPAddress hostIPAddress, IPAddress plcIPAddress)
        {
            _driver = new InovanceEIPDriverImpl(hostIPAddress, plcIPAddress);
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
                throw new BusinessException("PLC:ArgumentNull", "Value cannot be null.")
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
            public static readonly Action<string, T, InovanceEIPDriverImpl> Write = CreateWriter();

            private static Action<string, T, InovanceEIPDriverImpl> CreateWriter()
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

                // Bits 类型
                if (TypeEquality<T, Bits8Bit>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, Bits8Bit>(val));

                if (TypeEquality<T, Bits16Bit>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, Bits16Bit>(val));

                if (TypeEquality<T, Bits32Bit>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, Bits32Bit>(val));

                if (TypeEquality<T, Bits64Bit>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, Bits64Bit>(val));

                // Structure 类型
                if (TypeEquality<T, Structure>.AreSameType)
                    return (addr, val, drv) => drv.Write(addr, SelfConverter.ToSelf<T, Structure>(val));

                // 不支持的类型
                return (addr, val, drv) =>
                {
                    var typeName = typeof(T).Name;
                    throw new BusinessException("PLC:UnsupportedType", $"Type '{typeName}' is not supported.")
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
