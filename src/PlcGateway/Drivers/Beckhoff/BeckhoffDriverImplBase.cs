using PlcGateway.Core.Exceptions.YourNamespace.Exceptions;
using System;
using TwinCAT.Ads;

namespace PlcGateway.Drivers.Beckhoff
{
    internal class BeckhoffDriverImplBase : IDisposable
    {
        public AmsNetId AmsNetId { get; }

        public int Port { get; }

        protected readonly AdsClient AdsClient = new AdsClient();

        public bool IsConnected => AdsClient.IsConnected;

        public BeckhoffDriverImplBase(AmsNetId amsNetId, int port)
        {
            AmsNetId = amsNetId;
            Port = port;
        }

        public void Connect()
        {
            if (IsConnected)
            {
                return;
            }

            AdsClient.Connect(AmsNetId, this.Port);

            if (AdsClient.IsConnected)
            {
                throw new BusinessException("");
            }
        }

        public void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }

            AdsClient.Disconnect();

            if (AdsClient.IsConnected)
            {
                throw new BusinessException("");
            }
        }

        public void Dispose()
        {
            AdsClient.Dispose();
        }
    }
}
