using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Cluster.NetworkResources
{
    internal static class MessageSender
    {
        //IP address for the messege
        private static IPAddress _serverAddy;
        
        //Message sending method, string message and ip address for where to send it
        public static void SendMessage(string message,IPAddress ipAddress)
        {
            //Thread for sending message
            var sendMessageThread = new Thread(() => ThreadSendMessage(message,ipAddress));
            //Start thread
            sendMessageThread.Start();
        }

        //Method for sending message
        private static void ThreadSendMessage(string message,IPAddress ipAddress)
        {
            _serverAddy = ipAddress;

            //Port number
            int authPort = 12457;

            //Socket 
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Buffed message
            byte[] out_buffer = System.Text.Encoding.ASCII.GetBytes(message);
            soc.SendTimeout = 2000;                 // 2 seconds to live - send attempts
            
            try
            {
                soc.Connect(_serverAddy, authPort);  // create persistant connection
                soc.Send(out_buffer);               // send the encrypted creds (name/address parsing done server side
            }
            catch (SocketException e)                 //soc.connect fails, soc.send/receive times out
            {
                soc.Close();                        // free the resource
                Reporter.WriteContent(e.ToString(), 1);
            }
            catch (Exception ex)
            {
                soc.Close();                        // free the resource
                Reporter.WriteContent(ex.ToString(), 1);
            }

            soc.Close();                             // free the resource
            
        }

        //Same thing as above but is hard coded to send a message to its self.
        public static void SendMessageToSelf()
        {
            _serverAddy = System.Net.IPAddress.Parse(GetLocalIpAddress());
            int authPort = 12457;

            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byte[] out_buffer = System.Text.Encoding.ASCII.GetBytes("51,Test");
            soc.SendTimeout = 20000;                 // 2 seconds to live - send/receive attempts


            try
            {
                soc.Connect(_serverAddy, authPort);  // create persistant connection
                soc.Send(out_buffer);               // send the encrypted creds (name/address parsing done server side
            }
            catch (SocketException)                 //soc.connect fails, soc.send/receive times out
            {
                soc.Close();                        // free the resource
            }
            catch (Exception e)
            {
                soc.Close();                        // free the resource
            }

            soc.Close();                             // free the resource
        }

        private static string GetLocalIpAddress()
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
