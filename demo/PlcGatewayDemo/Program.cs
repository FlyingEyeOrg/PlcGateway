using System.Net;
using System.Security.Cryptography;
using PlcGateway.Drivers.Inovance;

namespace NugetTest
{
    internal class Program
    {
        private static uint GenerateGlobalSeed()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[sizeof(uint)];
                rng.GetBytes(randomBytes);
                return BitConverter.ToUInt32(randomBytes, 0);
            }
        }

        static void Main(string[] args)
        {
            var kk12 = GenerateGlobalSeed();

            var kk = new ExInovanceEIPDriver(IPAddress.Parse("169.254.52.63"), IPAddress.Parse("169.254.52.63"));

            kk.Connect();

            kk.Write("hello", 0);
            kk.Write("hello", 0.2);
            kk.Write("hello", "123");

            kk.Read<int>("hello");
            kk.Read<bool>("hello");

            Console.WriteLine("Hello, World!");

            Console.ReadKey();
        }
    }
}
