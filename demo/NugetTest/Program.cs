using System.Net;
using PlcGateway.Drivers.Inovance;

namespace NugetTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var kk = new InovanceEIPDriver(IPAddress.Parse("169.254.52.63"), IPAddress.Parse("169.254.52.63"));

            kk.Connect();

            kk.Read<int>("asdf");

            Console.WriteLine("Hello, World!");

            Console.ReadKey();
        }
    }
}
