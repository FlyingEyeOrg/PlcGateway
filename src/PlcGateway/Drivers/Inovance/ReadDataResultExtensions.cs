using System;
using System.Collections.Generic;
using System.Text;
using Inovance.EtherNetIP.Native;

namespace PlcGateway.Drivers.Inovance
{
    internal static class ReadDataResultExtensions
    {
        public static float ToFloat(this ref ReadDataResult result)
        {
            return 0;
        }
    }
}
