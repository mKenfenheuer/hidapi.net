using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hidapi.net_Test
{
    internal class Program
    {
        static FileStream file;
        static void Main(string[] args)
        {
            file = File.OpenWrite("recording.bin");
            
            HidDevice device = new HidDevice(0x28de, 0x1205);
            bool result = device.OpenDevice();

            byte[] request = new byte[] { 0xAE, 0x15, 0x01 };
            byte[] response = device.RequestFeatureReport(request);

            byte[] serial = new byte[response.Length - 5];
            Array.Copy(response, 4, serial, 0, serial.Length);

            string data = Encoding.ASCII.GetString(serial).TrimEnd((Char)0);
            Console.WriteLine(data);

            device.OnInputReceived += Device_OnInputReceived;
            device.BeginRead();


            Console.ReadLine();
            device.EndRead();
            file.Flush();
            file.Close();

        }

        private static void Device_OnInputReceived(object sender, HidDeviceInputReceivedEventArgs e)
        {
            if (file.CanWrite)
            {
                file.Write(e.Buffer, 0, e.Buffer.Length);
                file.Flush();
            }
            Console.WriteLine(BitConverter.ToString(e.Buffer).Replace("-", " "));
        }
    }
}
