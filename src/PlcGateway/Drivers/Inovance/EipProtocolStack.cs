using System.Collections.Concurrent;
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
        private static readonly ConcurrentDictionary<IPAddress, int> _protocolStack = new();
        private static readonly object _stopLock = new object();

        /// <summary>
        /// 启动 EIP 协议栈
        /// </summary>
        /// <param name="address">启动指定 IP 地址的 EIP 协议栈</param>
        /// <exception cref="EipStartException">启动失败异常</exception>
        public static void EipStart(IPAddress address)
        {
            _protocolStack.AddOrUpdate(
                address,
                addr =>
                {
                    // 首次启动协议栈时，进行初始化一次即可
                    if (!UnsafeNativeMethods.EipStartExt(address.ToString()))
                    {
                        throw new EipStartException(address.ToString());
                    }
                    return 1;
                },
                (addr, count) => count + 1
            );
        }

        public static bool EipStop(IPAddress address)
        {
            bool removed = false;

            _protocolStack.AddOrUpdate(
                address,
                addValueFactory: addr => 0,
                updateValueFactory: (addr, count) =>
                {
                    if (count <= 1)
                    {
                        removed = true;
                        return 0;
                    }
                    return count - 1;
                }
            );

            if (removed)
            {
                _protocolStack.TryRemove(address, out _);

                // 关键：在锁内检查并调用
                lock (_stopLock)
                {
                    if (_protocolStack.IsEmpty)
                    {
                        UnsafeNativeMethods.EipStop();
                    }
                }
            }

            return removed;
        }

        public static int GetCount(IPAddress address)
        {
            return _protocolStack.TryGetValue(address, out int count) ? count : 0;
        }
    }
}