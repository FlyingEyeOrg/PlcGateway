using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSubnetScanner
{
    public class NetworkInfo
    {
        public string Address { get; set; } = string.Empty;

        public string IPv4Mask { get; set; } = string.Empty;

        public int PrefixLength { get; set; }
    }

    internal class IPScanner
    {

    }
}
