using System.Net;
using PlcGateway.Abstractions;
using PlcGateway.Core.Exceptions.YourNamespace.Exceptions;
using PlcGateway.Drivers.Inovance.Data;

namespace PlcGateway.Drivers.Inovance
{
    public class InovanceEIPDriver : IPLCDriver
    {
        private readonly InovanceEIPDriverImpl _driver;

        public InovanceEIPDriver(IPAddress hostIPAddress, IPAddress plcIPAddress)
        {
            _driver = new InovanceEIPDriverImpl();
            _driver.HostIPAddress = hostIPAddress.ToString();
            _driver.PLCIPAddress = plcIPAddress.ToString();
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
            switch (value)
            {
                case bool boolValue:
                    _driver.Write(address, boolValue);
                    break;
                case byte byteValue:
                    _driver.Write(address, byteValue);
                    break;
                case sbyte sbyteValue:
                    _driver.Write(address, sbyteValue);
                    break;
                case short shortValue:
                    _driver.Write(address, shortValue);
                    break;
                case ushort ushortValue:
                    _driver.Write(address, ushortValue);
                    break;
                case int intValue:
                    _driver.Write(address, intValue);
                    break;
                case uint uintValue:
                    _driver.Write(address, uintValue);
                    break;
                case long longValue:
                    _driver.Write(address, longValue);
                    break;
                case ulong ulongValue:
                    _driver.Write(address, ulongValue);
                    break;
                case float floatValue:
                    _driver.Write(address, floatValue);
                    break;
                case double doubleValue:
                    _driver.Write(address, doubleValue);
                    break;
                case string stringValue:
                    _driver.Write(address, stringValue);
                    break;
                case Bits8Bit bits8Bit:
                    _driver.Write(address, bits8Bit);
                    break;
                case Bits16Bit bits16Bit:
                    _driver.Write(address, bits16Bit);
                    break;
                case Bits32Bit bits32Bit:
                    _driver.Write(address, bits32Bit);
                    break;
                case Bits64Bit bits64Bit:
                    _driver.Write(address, bits64Bit);
                    break;
                case Structure structure:
                    _driver.Write(address, structure);
                    break;
                case null:
                    throw new BusinessException("PLC:ArgumentNull", "Value cannot be null.")
                        .WithData("Parameter", nameof(value))
                        .WithData("Address", address);
                default:
                    var typeName = typeof(TValue).Name;
                    throw new BusinessException("PLC:UnsupportedType", $"Type '{typeName}' is not supported.")
                        .WithData("Type", typeName)
                        .WithData("Address", address)
                        .WithData("SupportedTypes", "bool, byte, sbyte, short, ushort, int, uint, long, ulong, float, double, string, Bits8Bit, Bits16Bit, Bits32Bit, Bits64Bit");
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
