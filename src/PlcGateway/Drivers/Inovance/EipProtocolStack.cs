using System.Collections.Generic;
using System.Net;
using Inovance.EtherNetIP.Native;
using PlcGateway.Drivers.Inovance.Exceptions;

namespace PlcGateway.Drivers.Inovance
{
    /// <summary>
    /// EIP 协议栈
    /// </summary>
    internal static class EipProtocolStack
    {
        private static readonly Dictionary<IPAddress, int> _protocolStack = new();
        private static readonly object _syncLock = new object();

        /// <summary>
        /// 启动 EIP 协议栈
        /// </summary>
        /// <param name="address">启动指定 IP 地址的 EIP 协议栈</param>
        /// <exception cref="EipStartException">启动失败异常</exception>
        public static void EipStart(IPAddress address)
        {
            lock (_syncLock)
            {
                if (_protocolStack.TryGetValue(address, out var count))
                {
                    _protocolStack[address] = count + 1;
                    return;
                }

                if (!UnsafeNativeMethods.EipStartExt(address.ToString()))
                {
                    throw new EipStartException(address.ToString());
                }

                _protocolStack[address] = 1;
            }
        }

        public static bool EipStop(IPAddress address)
        {
            lock (_syncLock)
            {
                if (!_protocolStack.TryGetValue(address, out var count) || count <= 0)
                {
                    return false;
                }

                if (count > 1)
                {
                    _protocolStack[address] = count - 1;
                    return false;
                }

                _protocolStack.Remove(address);

                if (_protocolStack.Count == 0)
                {
                    UnsafeNativeMethods.EipStop();
                }

                return true;
            }
        }

        public static int GetCount(IPAddress address)
        {
            lock (_syncLock)
            {
                return _protocolStack.TryGetValue(address, out var count) ? count : 0;
            }
        }
    }
}