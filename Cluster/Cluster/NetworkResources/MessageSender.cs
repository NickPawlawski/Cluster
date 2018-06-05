using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Cluster.NetworkResources
{
    internal class MessageSender
    {
        private System.Net.IPAddress serverAddy;
        private byte[] in_buffer = new byte[64];

        public MessageSender() { }

        public void SendMessage(String message)
        {
            serverAddy = System.Net.IPAddress.Parse(SoftwareConfiguration.ParentIp);
            int authPort = 12457;
            string errMsg;

            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byte[] out_buffer = System.Text.Encoding.ASCII.GetBytes(message);
            soc.SendTimeout = 20000;                 // 2 seconds to live - send/receive attempts


            try
            {
                soc.Connect(serverAddy, authPort);  // create persistant connection
                soc.Send(out_buffer);               // send the encrypted creds (name/address parsing done server side

            }
            catch (SocketException)                 //soc.connect fails, soc.send/receive times out
            {
                soc.Close();                        // free the resource
                errMsg = "Bad connection to remote server";

            }
            catch (Exception e)
            {
                soc.Close();                        // free the resource
                errMsg = "Undefined Socket error: " + e.ToString();

            }

            soc.Close();                             // free the resource
            Console.WriteLine("Message sending complete");
        }

        public void SendMessageToSelf()
        {


            serverAddy = System.Net.IPAddress.Parse(GetLocalIPAddress());
            int authPort = 12457;
            string errMsg;

            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byte[] out_buffer = System.Text.Encoding.ASCII.GetBytes("Test");
            soc.SendTimeout = 20000;                 // 2 seconds to live - send/receive attempts


            try
            {
                soc.Connect(serverAddy, authPort);  // create persistant connection
                soc.Send(out_buffer);               // send the encrypted creds (name/address parsing done server side

            }
            catch (SocketException)                 //soc.connect fails, soc.send/receive times out
            {
                soc.Close();                        // free the resource
                errMsg = "Bad connection to remote server";

            }
            catch (Exception e)
            {
                soc.Close();                        // free the resource
                errMsg = "Undefined Socket error: " + e.ToString();

            }

            soc.Close();                             // free the resource
            Console.WriteLine("Message sending complete");
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
