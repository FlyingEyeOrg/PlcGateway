using System;
using System.Net;
using System.Text;
using Inovance.EtherNetIP.Native;
using PlcGateway.Core.Exceptions;
using PlcGateway.Drivers.Converter;
using PlcGateway.Drivers.Inovance.Data;
using PlcGateway.Drivers.Inovance.Native;

using static PlcGateway.Drivers.Inovance.InovanceErrorCode;

namespace PlcGateway.Drivers.Inovance
{
    internal class ExInovanceEIPDriverImpl : IDisposable
    {
        public ExInovanceEIPDriverImpl(IPAddress hostIPAddress, IPAddress plcIPAddress, Encoding encoding)
        {
            this.PLCIPAddress = plcIPAddress.ToString();
            this.HostIPAddress = hostIPAddress.ToString();
            this.DefaultEncoding = encoding;
        }

        public ExInovanceEIPDriverImpl(IPAddress hostIPAddress, IPAddress plcIPAddress)
        {
            this.PLCIPAddress = plcIPAddress.ToString();
            this.HostIPAddress = hostIPAddress.ToString();
            this.DefaultEncoding = Encoding.UTF8;
        }

        public Encoding DefaultEncoding { get; }

        public string PLCIPAddress { get; }

        public string HostIPAddress { get; }

        private static readonly object _syncLock = new object();

        public bool IsConnected => _connectId.HasValue && !_disposed;

        private int? _connectId = null;
        private bool _disposed = false;

        public void Write(string tagName, Structure value)
        {
            if (!IsConnected)
            {
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = value.GetBytes();
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write byte value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = value.GetBytes();
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write byte value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = value.GetBytes();
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write ushort value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = value.GetBytes();
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write uint value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = value.GetBytes();
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write ulong value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write byte value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write sbyte value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write bool value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write short value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write ushort value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write int value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write uint value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write long value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write ulong value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write float value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = BitConverter.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write double value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var data = this.DefaultEncoding.GetBytes(value);
            var code = UnsafeNativeMethods.EipWriteTagExt3(_connectId!.Value, tagName, data, data.Length, 1);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_WRITE_ERROR, $"Failed to write string value to tag '{tagName}'.",
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
                throw new BusinessException(INOVANCE_NOT_CONNECTED, "PLC is not connected. Call Connect() first.");
            }

            var tag = new ReadDataTag()
            {
                pName = tagName,
                iElementCount = 1,
            };

            NativeErrorCode code = UnsafeNativeMethods.EipReadTagExt2(this._connectId!.Value, ref tag, out var readValueResult);

            if (code != NativeErrorCode.SUCCESS)
            {
                throw new BusinessException(INOVANCE_READ_ERROR, $"Failed to read tag '{tagName}'.",
                    $"Error code: {code} (0x{(int)code:X8})")
                    .WithData("TagName", tagName)
                    .WithData("ErrorCode", code);
            }

            var buffer = readValueResult.GetData();
            var tagType = readValueResult.eType;

            // 释放标签内存
            UnsafeNativeMethods.DeleteTagListStru([readValueResult], 1);

            var targetTag = TagTypeConverter<TValue>.Value;

            if (targetTag != tagType)
            {
                var actualType = TagTypeConverter.GetType(targetTag);

                throw new BusinessException(INOVANCE_TYPE_MISMATCH,
                    $"Tag type mismatch. Expected: {typeof(TValue).Name}, Actual: {actualType.Name}.")
                    .WithData("ExpectedType", typeof(TValue).FullName)
                    .WithData("ActualType", actualType.FullName)
                    .WithData("TagType", tagType.ToString());
            }

            return ByteArrayConverter<TValue>.Convert(buffer, this.DefaultEncoding);
        }

        public void Connect()
        {
            if (IsConnected)
                return;

            lock (_syncLock)
            {
                if (_disposed)
                    throw new BusinessException(INOVANCE_ALREADY_DISPOSED, "Driver instance has been disposed.")
                        .WithData("DriverType", nameof(InovanceEIPDriverImpl));

                // 验证主机IP
                if (string.IsNullOrWhiteSpace(HostIPAddress))
                    throw new BusinessException(INOVANCE_INVALID_CONNECTION_PARAMETERS, "Host IP address cannot be null or empty.")
                        .WithData("Property", nameof(HostIPAddress));

                if (!IPAddress.TryParse(HostIPAddress, out var hostIP))
                    throw new BusinessException(INOVANCE_INVALID_CONNECTION_PARAMETERS,
                        $"Invalid host IP address format: '{HostIPAddress}'. Expected format: xxx.xxx.xxx.xxx")
                        .WithData("HostIPAddress", HostIPAddress!)
                        .WithData("Property", nameof(HostIPAddress));

                // 验证设备IP
                if (string.IsNullOrWhiteSpace(PLCIPAddress))
                    throw new BusinessException(INOVANCE_INVALID_CONNECTION_PARAMETERS, "Device IP address cannot be null or empty.")
                        .WithData("Property", nameof(PLCIPAddress));

                if (!IPAddress.TryParse(PLCIPAddress, out var deviceIP))
                    throw new BusinessException(INOVANCE_INVALID_CONNECTION_PARAMETERS,
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

                    if (result != Native.NativeErrorCode.SUCCESS)
                    {
                        // 连接失败，清理并抛异常
                        EipProtocolStack.EipStop(hostIP);
                        throw new BusinessException(INOVANCE_CONNECTION_ERROR,
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
                        throw new BusinessException(INOVANCE_VERIFY_CONNECTION_FAILED,
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

                    if (result != Native.NativeErrorCode.SUCCESS)
                    {
                        // 如果你想抛异常：
                        throw new BusinessException(
                            INOVANCE_DISCONNECTION_ERROR,
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
                       INOVANCE_DISCONNECTION_ERROR,
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
