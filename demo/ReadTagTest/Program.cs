using PlcGateway.Drivers.Inovance;

namespace ReadTagTest
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // bool 01 00 = true

            // 2000000
            // 00 24 f4 49

            // 9
            // 00 10 41
            var val = (float)(9);

            var bytes = BitConverter.GetBytes(val);
            var val1 = BitConverter.ToSingle(bytes, 0);
            var str = MainFrom.BytesToHexString(bytes);
            var form = new MainFrom();
            form.Show();
            Application.Run(form);
        }
    }
}
