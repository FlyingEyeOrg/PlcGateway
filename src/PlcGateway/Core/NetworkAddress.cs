using System;
using System.Net;

namespace PlcGateway.Core
{
    internal static class NetworkAddress
    {
        /// <summary>
        /// Gets the network address (subnet address) for a given IP and subnet mask
        /// </summary>
        /// <param name="ip">IP address</param>
        /// <param name="mask">Subnet mask</param>
        /// <returns>Network address as string</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters are null</exception>
        /// <exception cref="ArgumentException">Thrown when parameters are invalid</exception>
        public static string GetNetworkAddress(IPAddress ip, IPAddress mask)
        {
            // 1. Null check
            if (ip == null)
            {
                throw new ArgumentNullException(nameof(ip), "IP address cannot be null.");
            }

            if (mask == null)
            {
                throw new ArgumentNullException(nameof(mask), "Subnet mask cannot be null.");
            }

            // 2. Validate address family (IPv4 only)
            if (ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                throw new ArgumentException(
                    $"Only IPv4 addresses are supported. Provided address family: {ip.AddressFamily}. IP: {ip}",
                    nameof(ip)
                );
            }

            if (mask.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                throw new ArgumentException(
                    $"Only IPv4 subnet masks are supported. Provided address family: {mask.AddressFamily}. Mask: {mask}",
                    nameof(mask)
                );
            }

            // 3. Get byte arrays
            byte[] ipBytes = ip.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();

            // 4. Validate byte array length (should be 4 for IPv4)
            if (ipBytes.Length != 4)
            {
                throw new ArgumentException(
                    $"IPv4 address must be 4 bytes. Actual length: {ipBytes.Length} bytes. IP: {ip}",
                    nameof(ip)
                );
            }

            if (maskBytes.Length != 4)
            {
                throw new ArgumentException(
                    $"IPv4 subnet mask must be 4 bytes. Actual length: {maskBytes.Length} bytes. Mask: {mask}",
                    nameof(mask)
                );
            }

            // 5. Optional: Validate subnet mask format
            if (!IsValidSubnetMask(maskBytes))
            {
                throw new ArgumentException(
                    $"Invalid subnet mask format: {mask}. A valid subnet mask must have consecutive 1s followed by consecutive 0s (e.g., 255.255.255.0).",
                    nameof(mask)
                );
            }

            // 6. Calculate network address
            byte[] networkBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }

            return new IPAddress(networkBytes).ToString();
        }

        /// <summary>
        /// Validates if the provided bytes represent a valid subnet mask
        /// </summary>
        /// <param name="maskBytes">Subnet mask bytes (must be 4 bytes for IPv4)</param>
        /// <returns>True if valid subnet mask, false otherwise</returns>
        private static bool IsValidSubnetMask(byte[] maskBytes)
        {
            if (maskBytes == null || maskBytes.Length != 4)
                return false;

            // Convert to 32-bit integer
            uint maskValue = (uint)((maskBytes[0] << 24) | (maskBytes[1] << 16) | (maskBytes[2] << 8) | maskBytes[3]);

            // Special cases: all zeros or all ones
            if (maskValue == 0x00000000 || maskValue == 0xFFFFFFFF)
                return false; // Usually considered invalid, depends on requirements

            // Valid subnet mask: ~mask + 1 should be a power of two
            // This ensures consecutive 1s followed by consecutive 0s
            uint inverted = ~maskValue;
            return (inverted & (inverted + 1)) == 0;
        }
    }
}
