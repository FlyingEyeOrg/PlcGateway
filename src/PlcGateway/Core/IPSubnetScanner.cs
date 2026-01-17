using System.Net.NetworkInformation;

namespace PlcGateway.Core
{
    internal class IPSubnetScanner
    {
        private void Pint()
        {
            var options = new PingOptions()
            {

            };

            using var ping = new Ping();

            ping.SendPingAsync("");
        }
    }
}
