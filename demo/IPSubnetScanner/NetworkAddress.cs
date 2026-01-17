using System.Net;

namespace IPSubnetScanner
{
    internal class NetworkAddress
    {
        public static string GetAddress(IPAddress ip, IPAddress mask)
        {
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
    }
}
