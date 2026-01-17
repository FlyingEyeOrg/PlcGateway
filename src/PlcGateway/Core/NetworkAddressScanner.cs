using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PlcGateway.Core
{
    public class NetworkAddressScanner
    {
        /// <summary>
        /// 扫描目标地址，返回本地同子网的网卡IP地址
        /// </summary>
        /// <param name="targetAddress">目标IP地址</param>
        /// <returns>本地同子网的网卡IP地址列表，如果没有匹配的则返回空列表</returns>
        public static async Task<IPAddress?> TryScanAsync(IPAddress targetAddress)
        {
            if (targetAddress == null)
                throw new ArgumentNullException(nameof(targetAddress));

            // 1. 首先Ping目标地址
            bool isReachable = await PingTargetAsync(targetAddress);

            if (!isReachable)
            {
                return null;
            }

            // 2. 获取本地网卡信息
            var localInfos = GetActiveNetworkIPAddressInformations();

            if (!localInfos.Any())
            {
                return null;
            }

            // 3. 计算匹配的网卡IP
            foreach (var info in localInfos)
            {
                if (NetworkAddress.GetNetworkAddress(targetAddress, info.IPv4Mask)
                    == NetworkAddress.GetNetworkAddress(info.Address, info.IPv4Mask))
                {
                    return info.Address;
                }
            }

            return null;
        }

        /// <summary>
        /// Ping目标地址
        /// </summary>
        private static async Task<bool> PingTargetAsync(IPAddress targetAddress)
        {
            using var ping = new Ping();

            try
            {
                var reply = await ping.SendPingAsync(
                    targetAddress,
                    (int)TimeSpan.FromSeconds(3).TotalMilliseconds
                );

                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch (PingException)
            {
            }

            return false;
        }

        /// <summary>
        /// 获取活动的网络接口的 IPV4 地址信息
        /// </summary>
        private static List<UnicastIPAddressInformation> GetActiveNetworkIPAddressInformations()
        {
            var activeInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            var list = new List<UnicastIPAddressInformation>();

            foreach (NetworkInterface nic in activeInterfaces)
            {
                if (nic.OperationalStatus == OperationalStatus.Up &&
                    nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                {
                    foreach (var ua in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (ua.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            list.Add(ua);
                        }
                    }
                }
            }

            return list;
        }
    }
}