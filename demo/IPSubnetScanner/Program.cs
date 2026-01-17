using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace NetworkInterfaceInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("==================================================================");
            Console.WriteLine("                   本地网络接口信息获取工具");
            Console.WriteLine("==================================================================\n");

            try
            {
                // 获取所有网络接口
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();


                if (networkInterfaces.Length == 0)
                {
                    Console.WriteLine("❌ 未找到任何网络接口");
                    return;
                }

                Console.WriteLine($"找到 {networkInterfaces.Length} 个网络接口\n");

                int interfaceCount = 0;
                foreach (var ni in networkInterfaces)
                {
                    interfaceCount++;
                    DisplayNetworkInterfaceInfo(ni, interfaceCount);
                }

                // 显示摘要信息
                DisplaySummaryInfo(networkInterfaces);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 获取网络接口信息时出错: {ex.Message}");
            }

            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }

        #region 显示单个网络接口信息
        private static void DisplayNetworkInterfaceInfo(NetworkInterface ni, int index)
        {
            Console.WriteLine(new string('=', 80));
            Console.WriteLine($"📡 网络接口 #{index}: {ni.Name}");
            Console.WriteLine(new string('=', 80));

            // 基本状态信息
            Console.WriteLine($"状态: {GetStatusWithIcon(ni.OperationalStatus)}");
            Console.WriteLine($"描述: {ni.Description}");
            Console.WriteLine($"类型: {ni.NetworkInterfaceType}");
            Console.WriteLine($"ID: {ni.Id}");

            // 物理地址
            Console.WriteLine($"物理地址(MAC): {FormatMacAddress(ni.GetPhysicalAddress()?.ToString())}");

            // 速度信息
            if (ni.Speed > 0)
            {
                Console.WriteLine($"速度: {FormatSpeed(ni.Speed)}");
            }

            // IP配置信息
            var ipProperties = ni.GetIPProperties();
            if (ipProperties != null)
            {
                DisplayIPAddresses(ipProperties);
                DisplayGatewayInfo(ipProperties);
                DisplayDNSInfo(ipProperties);
                DisplayDHCPInfo(ipProperties);
                DisplayMulticastInfo(ipProperties);
            }

            // 统计信息
            var stats = ni.GetIPStatistics();
            if (stats != null)
            {
                DisplayStatistics(stats);
            }

            // IPv6支持
            if (ni.Supports(NetworkInterfaceComponent.IPv6))
            {
                Console.WriteLine("IPv6: ✅ 支持");
            }

            Console.WriteLine(); // 空行分隔
        }
        #endregion

        #region 显示IP地址信息
        private static void DisplayIPAddresses(IPInterfaceProperties ipProps)
        {
            Console.WriteLine("\n📡 IP地址信息:");
            Console.WriteLine("  " + new string('-', 50));

            bool hasIPv4 = false;
            bool hasIPv6 = false;

            // IPv4地址
            foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
            {
                if (addr.Address.AddressFamily == AddressFamily.InterNetwork) // IPv4
                {
                    hasIPv4 = true;
                    Console.WriteLine($"  IPv4地址: {addr.Address}");
                    Console.WriteLine($"    子网掩码: {addr.IPv4Mask}");
                    Console.WriteLine($"    网络地址: {CalculateNetworkAddress(addr.Address, addr.IPv4Mask)}");
                    Console.WriteLine($"    前缀长度: {addr.PrefixLength}");

                    if (addr.DuplicateAddressDetectionState != DuplicateAddressDetectionState.Invalid)
                    {
                        Console.WriteLine($"    重复地址检测: {addr.DuplicateAddressDetectionState}");
                    }
                }
                else if (addr.Address.AddressFamily == AddressFamily.InterNetworkV6) // IPv6
                {
                    hasIPv6 = true;
                    if (!addr.Address.IsIPv6LinkLocal && !addr.Address.IsIPv6SiteLocal)
                    {
                        Console.WriteLine($"  IPv6地址: {addr.Address}");
                        Console.WriteLine($"    前缀长度: {addr.PrefixLength}");
                    }
                }
            }

            if (!hasIPv4 && !hasIPv6)
            {
                Console.WriteLine("  无IP地址配置");
            }
        }
        #endregion

        #region 显示网关信息
        private static void DisplayGatewayInfo(IPInterfaceProperties ipProps)
        {
            if (ipProps.GatewayAddresses.Count > 0)
            {
                Console.WriteLine("\n🌉 网关信息:");
                Console.WriteLine("  " + new string('-', 50));

                foreach (GatewayIPAddressInformation gateway in ipProps.GatewayAddresses)
                {
                    Console.WriteLine($"  {gateway.Address}");
                }
            }
        }
        #endregion

        #region 显示DNS信息
        private static void DisplayDNSInfo(IPInterfaceProperties ipProps)
        {
            if (ipProps.DnsAddresses.Count > 0)
            {
                Console.WriteLine("\n🔍 DNS服务器:");
                Console.WriteLine("  " + new string('-', 50));

                int dnsCount = 1;
                foreach (IPAddress dns in ipProps.DnsAddresses)
                {
                    Console.WriteLine($"  DNS{dnsCount++}: {dns}");
                }

                if (!string.IsNullOrEmpty(ipProps.DnsSuffix))
                {
                    Console.WriteLine($"  DNS后缀: {ipProps.DnsSuffix}");
                }
            }
        }
        #endregion

        #region 显示DHCP信息
        private static void DisplayDHCPInfo(IPInterfaceProperties ipProps)
        {
            if (ipProps.DhcpServerAddresses.Count > 0)
            {
                Console.WriteLine("\n⚡ DHCP服务器:");
                Console.WriteLine("  " + new string('-', 50));

                foreach (IPAddress dhcp in ipProps.DhcpServerAddresses)
                {
                    Console.WriteLine($"  {dhcp}");
                }
            }
        }
        #endregion

        #region 显示多播信息
        private static void DisplayMulticastInfo(IPInterfaceProperties ipProps)
        {
            if (ipProps.MulticastAddresses.Count > 0)
            {
                Console.WriteLine("\n📢 多播地址:");
                Console.WriteLine("  " + new string('-', 50));

                int count = 0;
                foreach (MulticastIPAddressInformation multicast in ipProps.MulticastAddresses)
                {
                    if (count++ < 5) // 只显示前5个
                    {
                        Console.WriteLine($"  {multicast.Address}");
                    }
                }

                if (count > 5)
                {
                    Console.WriteLine($"  ... 还有 {count - 5} 个多播地址");
                }
            }
        }
        #endregion

        #region 显示统计信息
        private static void DisplayStatistics(IPInterfaceStatistics stats)
        {
            Console.WriteLine("\n📊 网络统计:");
            Console.WriteLine("  " + new string('-', 50));
            Console.WriteLine($"  接收字节数: {FormatBytes(stats.BytesReceived)}");
            Console.WriteLine($"  发送字节数: {FormatBytes(stats.BytesSent)}");
            Console.WriteLine($"  接收包数: {stats.UnicastPacketsReceived:N0}");
            Console.WriteLine($"  发送包数: {stats.UnicastPacketsSent:N0}");
            Console.WriteLine($"  接收错误数: {stats.IncomingPacketsWithErrors:N0}");
            Console.WriteLine($"  发送错误数: {stats.OutgoingPacketsWithErrors:N0}");
        }
        #endregion

        #region 显示摘要信息
        private static void DisplaySummaryInfo(NetworkInterface[] interfaces)
        {
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("📈 网络接口摘要");
            Console.WriteLine(new string('=', 80));

            int upCount = interfaces.Count(ni => ni.OperationalStatus == OperationalStatus.Up);
            int downCount = interfaces.Count(ni => ni.OperationalStatus == OperationalStatus.Down);
            int ethernetCount = interfaces.Count(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet);
            int wirelessCount = interfaces.Count(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211);
            int tunnelCount = interfaces.Count(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel);
            int loopbackCount = interfaces.Count(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Loopback);

            Console.WriteLine($"总接口数: {interfaces.Length}");
            Console.WriteLine($"活动接口: {upCount}");
            Console.WriteLine($"非活动接口: {downCount}");
            Console.WriteLine($"以太网接口: {ethernetCount}");
            Console.WriteLine($"无线接口: {wirelessCount}");
            Console.WriteLine($"隧道接口: {tunnelCount}");
            Console.WriteLine($"回环接口: {loopbackCount}");

            // 显示活动接口的IP地址
            Console.WriteLine("\n🎯 活动接口的IP地址:");
            foreach (var ni in interfaces.Where(ni => ni.OperationalStatus == OperationalStatus.Up))
            {
                var ipProps = ni.GetIPProperties();
                foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork) // IPv4
                    {
                        Console.WriteLine($"  {ni.Name}: {addr.Address} ({ni.NetworkInterfaceType})");
                    }
                }
            }
        }
        #endregion

        #region 辅助方法
        private static string GetStatusWithIcon(OperationalStatus status)
        {
            return status switch
            {
                OperationalStatus.Up => "✅ 已连接",
                OperationalStatus.Down => "❌ 已断开",
                OperationalStatus.Testing => "🔧 测试中",
                OperationalStatus.Unknown => "❓ 未知",
                OperationalStatus.Dormant => "💤 休眠",
                OperationalStatus.NotPresent => "🚫 不存在",
                OperationalStatus.LowerLayerDown => "⬇️ 下层断开",
                _ => status.ToString()
            };
        }

        private static string FormatMacAddress(string mac)
        {
            if (string.IsNullOrEmpty(mac) || mac.Length != 12)
                return mac ?? "未知";

            // 将 "001A2B3C4D5E" 格式化为 "00-1A-2B-3C-4D-5E"
            var result = new StringBuilder();
            for (int i = 0; i < mac.Length; i++)
            {
                result.Append(mac[i]);
                if ((i + 1) % 2 == 0 && i < mac.Length - 1)
                    result.Append('-');
            }
            return result.ToString();
        }

        private static string FormatSpeed(long speed)
        {
            if (speed >= 1_000_000_000) // 1 Gbps
                return $"{(speed / 1_000_000_000.0):F1} Gbps";
            else if (speed >= 1_000_000) // 1 Mbps
                return $"{(speed / 1_000_000.0):F1} Mbps";
            else if (speed >= 1_000) // 1 Kbps
                return $"{(speed / 1_000.0):F1} Kbps";
            else
                return $"{speed} bps";
        }

        private static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:F2} {sizes[order]}";
        }

        private static string CalculateNetworkAddress(IPAddress ip, IPAddress mask)
        {
            if (ip == null || mask == null)
                return "未知";

            byte[] ipBytes = ip.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();

            if (ipBytes.Length != 4 || maskBytes.Length != 4)
                return "无效";

            byte[] networkBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }

            return new IPAddress(networkBytes).ToString();
        }
        #endregion
    }
}