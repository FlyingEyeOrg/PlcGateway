using System;
using System.Net;
using System.Text;
using Inovance.EtherNetIP.Native;
using PlcGateway.Core.Exceptions.YourNamespace.Exceptions;
using PlcGateway.Drivers.Inovance.Data;
using PlcGateway.Drivers.Inovance.Native;

namespace PlcGateway.Drivers.Inovance
{
    /// <summary>
    /// 汇川 EIP 驱动基础实现
    /// </summary>
    internal class InovanceEIPDriverImpl : IDisposable
    {
        public Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 汇川最大传输字节 1994
        /// </summary>
        public const int TagDataMaxSize = 1994;

        public string? PLCIPAddress { get; set; }

        public string? HostIPAddress { get; set; }

        private static readonly object _syncLock = new object();

        public bool IsConnected => _connectId.HasValue && !_disposed;

        private int? _connectId = null;
        private bool _disposed = false;

        public void Write(string tagName, Structure value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = value.GetBytes();
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_STRUCT, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write byte value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, Bits8Bit value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = value.GetBytes();
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_BYTE, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write byte value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, Bits16Bit value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = value.GetBytes();
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_WORD, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write ushort value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, Bits32Bit value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = value.GetBytes();
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_DWORD, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write uint value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, Bits64Bit value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = value.GetBytes();
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_LWORD, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write ulong value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, byte value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_USINT, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write byte value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, sbyte value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_SINT, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write sbyte value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, bool value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BytesConverter.ToMemberBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_BOOL, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write bool value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, short value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_INT, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write short value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, ushort value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_UINT, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write ushort value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, int value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_DINT, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write int value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, uint value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_UDINT, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write uint value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, long value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_LINT, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write long value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, ulong value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_ULINT, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write ulong value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, float value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_REAL, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write float value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, double value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_LREAL, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write double value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public void Write(string tagName, string value)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var data = this.DefaultEncoding.GetBytes(value);
            // 注意：这里应该是 TAG_TYPE_STRING 而不是 TAG_TYPE_LREAL
            var code = UnsafeNativeMethods.EipWriteTagExt(_connectId!.Value, tagName, TagType.TAG_TYPE_STRING, data, data.Length, 1);

            if (code != ErrorCode.SUCCESS)
            {
                throw new BusinessException("PLC:WriteFailed", $"Failed to write string value to tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("Value", value)
                    .WithData("ValueLength", value.Length)
                    .WithData("ErrorCode", (int)code);
            }
        }

        public TValue Read<TValue>(string tagName)
        {
            if (!IsConnected)
            {
                throw new BusinessException("PLC:NotConnected", "PLC is not connected. Call Connect() first.");
            }

            var buffer = new byte[TagDataMaxSize];
            var result = UnsafeNativeMethods.EipReadTagExt(this._connectId!.Value, tagName, out var tagType, buffer, TagDataMaxSize, 1);

            if (result < 0)
            {
                throw new BusinessException("PLC:ReadFailed", $"Failed to read tag '{tagName}'.",
                    $"Error code: {result} (0x{result:X8})")
                    .WithData("TagName", tagName)
                    .WithData("ErrorCode", result);
            }

            return Parse<TValue>(tagType, buffer);
        }

        private Type GetActualType(TagType tagType)
        {
            return tagType switch
            {
                TagType.TAG_TYPE_SINT => typeof(sbyte),
                TagType.TAG_TYPE_INT => typeof(short),
                TagType.TAG_TYPE_DINT => typeof(int),
                TagType.TAG_TYPE_LINT => typeof(long),
                TagType.TAG_TYPE_USINT => typeof(byte),
                TagType.TAG_TYPE_UINT => typeof(ushort),
                TagType.TAG_TYPE_UDINT => typeof(uint),
                TagType.TAG_TYPE_ULINT => typeof(ulong),
                TagType.TAG_TYPE_REAL => typeof(float),
                TagType.TAG_TYPE_LREAL => typeof(double),
                TagType.TAG_TYPE_BYTE => typeof(Bits8Bit),
                TagType.TAG_TYPE_WORD => typeof(Bits16Bit),
                TagType.TAG_TYPE_DWORD => typeof(Bits32Bit),
                TagType.TAG_TYPE_LWORD => typeof(Bits64Bit),
                TagType.TAG_TYPE_STRUCT => typeof(Structure),
                TagType.TAG_TYPE_BOOL => typeof(bool),
                TagType.TAG_TYPE_STRING => typeof(string),
                _ => typeof(object)
            };
        }

        private TValue Parse<TValue>(TagType tagType, byte[] bytes)
        {
            var actualType = GetActualType(tagType);

            if (actualType != typeof(TValue))
            {
                throw new BusinessException("PLC:TypeMismatch",
                    $"Tag type mismatch. Expected: {typeof(TValue).Name}, Actual: {actualType.Name}.")
                    .WithData("ExpectedType", typeof(TValue).FullName)
                    .WithData("ActualType", actualType.FullName)
                    .WithData("TagType", tagType.ToString());
            }

            object result = (tagType) switch
            {
                TagType.TAG_TYPE_SINT => (sbyte)bytes[0],
                TagType.TAG_TYPE_INT => BitConverter.ToInt16(bytes, 0),
                TagType.TAG_TYPE_DINT => BitConverter.ToInt32(bytes, 0),
                TagType.TAG_TYPE_LINT => BitConverter.ToInt64(bytes, 0),
                TagType.TAG_TYPE_USINT => bytes[0],
                TagType.TAG_TYPE_UINT => BitConverter.ToUInt16(bytes, 0),
                TagType.TAG_TYPE_UDINT => BitConverter.ToUInt32(bytes, 0),
                TagType.TAG_TYPE_ULINT => BitConverter.ToUInt64(bytes, 0),
                TagType.TAG_TYPE_REAL => BitConverter.ToSingle(bytes, 0),
                TagType.TAG_TYPE_LREAL => BitConverter.ToDouble(bytes, 0),
                TagType.TAG_TYPE_BYTE => (Bits8Bit)bytes[0],
                TagType.TAG_TYPE_WORD => (Bits16Bit)BitConverter.ToUInt16(bytes, 0),
                TagType.TAG_TYPE_DWORD => (Bits32Bit)BitConverter.ToUInt32(bytes, 0),
                TagType.TAG_TYPE_LWORD => (Bits64Bit)BitConverter.ToUInt64(bytes, 0),
                TagType.TAG_TYPE_STRUCT => (Structure)bytes,
                TagType.TAG_TYPE_BOOL => BitConverter.ToBoolean(bytes, 0),
                TagType.TAG_TYPE_STRING => this.DefaultEncoding.GetString(bytes, 0, bytes.Length).TrimEnd('\0'),
                _ => throw new BusinessException("PLC:UnsupportedTagType", $"Unsupported tag type: {tagType}.")
                    .WithData("TagType", tagType.ToString())
            };

            return (TValue)result;
        }

        public void Connect()
        {
            if (IsConnected)
                return;

            lock (_syncLock)
            {
                if (_disposed)
                    throw new BusinessException("PLC:DriverDisposed", "Driver instance has been disposed.")
                        .WithData("DriverType", nameof(InovanceEIPDriverImpl));

                // 验证主机IP
                if (string.IsNullOrWhiteSpace(HostIPAddress))
                    throw new BusinessException("PLC:InvalidHostIP", "Host IP address cannot be null or empty.")
                        .WithData("Property", nameof(HostIPAddress));

                if (!IPAddress.TryParse(HostIPAddress, out var hostIP))
                    throw new BusinessException("PLC:InvalidHostIPFormat",
                        $"Invalid host IP address format: '{HostIPAddress}'. Expected format: xxx.xxx.xxx.xxx")
                        .WithData("HostIPAddress", HostIPAddress!)
                        .WithData("Property", nameof(HostIPAddress));

                // 验证设备IP
                if (string.IsNullOrWhiteSpace(PLCIPAddress))
                    throw new BusinessException("PLC:InvalidDeviceIP", "Device IP address cannot be null or empty.")
                        .WithData("Property", nameof(PLCIPAddress));

                if (!IPAddress.TryParse(PLCIPAddress, out var deviceIP))
                    throw new BusinessException("PLC:InvalidDeviceIPFormat",
                        $"Invalid device IP address format: '{PLCIPAddress}'. Expected format: xxx.xxx.xxx.xxx")
                        .WithData("PLCIPAddress", PLCIPAddress!)
                        .WithData("Property", nameof(PLCIPAddress));

                // 启动协议栈
                EipProtocolStack.EipStart(hostIP);

                // 打开连接
                try
                {
                    var result = UnsafeNativeMethods.EipOpenConnection(
                        deviceIP.ToString(),
                        out int connectId);

                    if (result != Native.ErrorCode.SUCCESS)
                    {
                        // 连接失败，清理并抛异常
                        EipProtocolStack.EipStop(hostIP);
                        throw new BusinessException("PLC:ConnectionFailed",
                            $"Failed to connect to PLC at {deviceIP}.",
                            $"Error: {result} (0x{(int)result:X8}).")
                            .WithData("PLCIPAddress", PLCIPAddress!)
                            .WithData("HostIPAddress", HostIPAddress!)
                            .WithData("ErrorCode", (int)result);
                    }

                    var status = UnsafeNativeMethods.EipGetConnectionState(connectId);

                    if (status != ConnectionState.CONNECTION_ESTABLISHED)
                    {
                        // 连接失败，清理并抛异常
                        EipProtocolStack.EipStop(hostIP);
                        throw new BusinessException("PLC:ConnectionFailed",
                            $"Failed to connect to PLC at {deviceIP}.",
                            $"ConnectionState: {status} (0x{(int)status:X8}).")
                            .WithData("PLCIPAddress", PLCIPAddress!)
                            .WithData("HostIPAddress", HostIPAddress!)
                            .WithData("ConnectionState", status.ToString());
                    }

                    _connectId = connectId;  // 成功时设置
                }
                catch
                {
                    // 任何异常都清理协议栈
                    EipProtocolStack.EipStop(hostIP);
                    throw;  // 重新抛出原始异常
                }
            }
        }

        public void Disconnect()
        {
            lock (_syncLock)
            {
                if (!IsConnected)
                {
                    return;
                }

                int connectId = _connectId!.Value;  // 保存ID
                _connectId = null;  // 立即清理状态，避免重复操作

                try
                {
                    var result = UnsafeNativeMethods.EipCloseConnection(connectId);

                    if (result != Native.ErrorCode.SUCCESS)
                    {
                        // 如果你想抛异常：
                        throw new BusinessException(
                            "PLC:DisconnectFailed",
                            $"Failed to disconnect from PLC. Connection ID: {connectId}.",
                            $"Error: {result} (0x{(int)result:X8})")
                            .WithData("ConnectionId", connectId)
                            .WithData("ErrorCode", (int)result)
                            .WithLogLevel(LogLevel.Warning);
                    }
                }
                catch (Exception ex) when (!(ex is BusinessException))
                {
                    // 原生方法抛异常
                    throw new BusinessException(
                        "PLC:DisconnectError",
                        $"Error while disconnecting from PLC.",
                        $"Internal error: {ex.Message}")
                        .WithData("ConnectionId", connectId)
                        .WithData("InnerException", ex.ToString());
                }
                finally
                {
                    // 无论成功失败，都清理协议栈
                    if (!string.IsNullOrWhiteSpace(HostIPAddress) &&
                        IPAddress.TryParse(HostIPAddress, out var hostIP))
                    {
                        EipProtocolStack.EipStop(hostIP);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            if (IsConnected)
                Disconnect();
        }
    }
}
