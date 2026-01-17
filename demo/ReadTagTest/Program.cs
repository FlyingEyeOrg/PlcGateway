using PlcGateway.Drivers.Inovance;

namespace ReadTagTest
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var form = new MainFrom();
            form.Show();
            Application.Run(form);
        }
    }
}
