using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hidapi.net_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HidDevice device = new HidDevice(0x28de, 0x1205);
            bool result = device.OpenDevice();
            device.OnInputReceived += Device_OnInputReceived;
            device.BeginRead();

            Console.ReadLine();

        }

        private static void Device_OnInputReceived(object sender, HidDeviceInputReceivedEventArgs e)
        {
            Console.WriteLine(BitConverter.ToString(e.Buffer).Replace("-", " "));
        }
    }
}
