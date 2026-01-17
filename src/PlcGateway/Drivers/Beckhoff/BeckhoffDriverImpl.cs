using System.Threading;
using TwinCAT.Ads;

namespace PlcGateway.Drivers.Beckhoff
{
    internal class BeckhoffDriverImpl
    {
        public BeckhoffDriverImpl()
        {
            
        }

        public async void Connect()
        {
            using (AdsClient client = new AdsClient())
            {
                CancellationToken cancel = CancellationToken.None;

                uint valueToRead = 0;
                uint valueToWrite = 42;

                client.Connect(AmsNetId.Local, 851);
                ResultWrite resultWrite = await client.WriteAnyAsync(0x4020, 0x0, valueToWrite, cancel);
                bool succeeded = resultWrite.Succeeded;

                ResultValue<uint> resultRead = await client.ReadAnyAsync<uint>(0x4020, 0x0, cancel);

                if (resultRead.Succeeded)
                {
                    valueToRead = (uint)resultRead.Value;
                }
            }
        }
    }
}
