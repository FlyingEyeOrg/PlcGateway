using System;
using System.Net;
using PlcGateway.Drivers.Inovance;

namespace PInvokeTest
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("开始测试汇川驱动公开 API...");

            try
            {
                RunSmokeTest();
                Console.WriteLine("公开 API 烟雾测试完成。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试失败: {ex.Message}");
                Console.WriteLine($"堆栈: {ex}");
            }

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }

        private static void RunSmokeTest()
        {
            var hostIp = IPAddress.Parse("127.0.0.1");
            var plcIp = IPAddress.Parse("127.0.0.1");

            Console.WriteLine($"Host IP: {hostIp}");
            Console.WriteLine($"PLC IP: {plcIp}");
            Console.WriteLine("将通过公开驱动对象验证参数检查、连接调用与释放流程。\n");

            using var driver = new InovanceEIPDriver(hostIp, plcIp);

            try
            {
                driver.Connect();
                Console.WriteLine("Connect 调用成功。\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connect 调用返回异常: {ex.Message}");
            }

            try
            {
                driver.Disconnect();
                Console.WriteLine("Disconnect 调用已完成。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Disconnect 调用返回异常: {ex.Message}");
            }
        }
    }
}