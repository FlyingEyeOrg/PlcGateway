using PlcGateway.Core.Exceptions;

using static PlcGateway.Drivers.Inovance.InovanceErrorCode;

namespace PlcGateway.Drivers.Inovance.Exceptions
{
    public class EipStartException :
                         UserFriendlyException
    {
        public EipStartException(string address) : base(INOVANCE_EIP_START_ERROR, $"Failed to start EIP protocol stack for address: {address}. " +
                            "Possible reasons: " +
                            "1. Protocol stack already started. " +
                            "2. Insufficient system resources. " +
                            "3. Network interface not available. " +
                            "4. Invalid IP address configuration. " +
                            "5. Permission denied.")
        {
        }
    }
}