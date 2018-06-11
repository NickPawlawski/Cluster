using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Cluster.NetworkResources;

namespace Cluster.Child.GUI
{
    class Connection
    {
        //Boolean flag for if the child is connected
        private bool _isConnected;
        //Thread to repeat attempts to connect
        private readonly Thread _connectionThread;
        //Boolean flag to let the thread die
        private bool _killThread;

        //Method to kill the thread
        public void KillThread()
        {
            _killThread = true;

            _connectionThread.Join(5500);
        }

        //Sets the connection to true
        public void Connected()
        {
            _isConnected = true;
        }

        //Sets the connection to false
        public void KillConnection()
        {
            _isConnected = false;
            Reporter.WriteContent("Connection To Parent has been lost",1);
        }

        //Constructor for the connection object
        public Connection()
        {
            _isConnected = false;
            _connectionThread = new Thread(ConnectThread);
            _connectionThread.Start();
        }

        //Method used for connecting to the parent
        public void ConnectThread()
        {
            var i = 0;
            while (!_killThread)//while the thread is told to stay alive
            {
                if (!_isConnected)//while the child is not connected
                {
                    MessageSender.SendMessage("0,"+ IPAddress.Parse(GetLocalIpAddress()), SoftwareConfiguration.ParentIp);
                }
                else//Print an incrementing number to the screen
                {
                    //Reporter.WriteContent(i.ToString(),1);
                    i++;
                }
                //Sleep for 10 seconds
                Thread.Sleep(10000);
            }

            Reporter.WriteContent("Connection Thread Dying",1);
        }
        //Method for getting the childs IP address
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
